using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//using Passero.Framework.Base;
#nullable enable

namespace Passero.Framework
{
    public partial class Repository<ModelClass>
        where ModelClass : class
    {
        public Repository<ModelClass> Clone()
        {
            Repository<ModelClass> newrepository = new Repository<ModelClass>();
            newrepository.DbConnection = DbConnection;
            //newrepository.DbContext = DbContext;
            return newrepository;
        }


        private void EnsureInsertSqlCommandInitialized()
        {
            if (ProviderFeatures == null)
            {
                ProviderFeatures = DbConnection != null
                    ? ProviderFeaturesResolver.FromConnection(DbConnection)
                    : new ProviderFeatures();
            }

            if (string.IsNullOrWhiteSpace(mSqlInsertCommand))
            {
                SqlInsertCommand(true);
            }

            if (string.IsNullOrWhiteSpace(mSqlInsertCommand))
            {
                throw new InvalidOperationException("Il comando SQL di insert non è stato inizializzato.");
            }
        }

        private void ValidateInsertContext(ModelClass model)
        {
            if (model == null)
            {
                throw new InvalidOperationException("Nessun modello da inserire. Specificare un Model o impostare il modello corrente.");
            }

            if (DbConnection == null)
            {
                throw new InvalidOperationException("La connessione al database non è stata inizializzata.");
            }

            if (EntityProperties == null || EntityProperties.Count == 0)
            {
                throw new InvalidOperationException("Nessuna proprietà disponibile per l'insert.");
            }
        }

        private long WriteBackIdentity(ModelClass model, object scalarValue)
        {
            if (scalarValue == null || scalarValue == DBNull.Value)
            {
                return 0;
            }

            long generatedId = Convert.ToInt64(scalarValue);

            if (generatedId > 0)
            {
                var identityKey = EntityPrimaryKeys
                    .FirstOrDefault(p => p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

                if (identityKey != null && identityKey.CanWrite)
                {
                    identityKey.SetValue(model, Convert.ChangeType(generatedId, identityKey.PropertyType));
                }
            }

            return generatedId;
        }

        private void SyncInsertedModelState(ModelClass model)
        {
            _ModelItem = model;
            _ModelItemShadow = Utilities.Clone(model);
            ModelItemShadow = _ModelItemShadow;

            if (ModelItems == null)
            {
                ModelItems = new List<ModelClass>();
            }

            if (!ModelItems.Any(x => Utilities.ObjectsEquals(x, model)))
            {
                ModelItems.Add(model);
            }

            if (ModelItemsShadow == null)
            {
                ModelItemsShadow = new List<ModelClass>();
            }

            var existingShadow = ModelItemsShadow
                .Select((item, index) => new { item, index })
                .FirstOrDefault(x => Utilities.ObjectsEquals(x.item, model));

            if (existingShadow != null)
            {
                ModelItemsShadow[existingShadow.index] = Utilities.Clone(model);
            }
            else
            {
                ModelItemsShadow.Add(Utilities.Clone(model));
            }
        }


        /// <summary>
        /// Compatibility wrapper for <see cref="InsertItem(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="Model">The model to insert.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core insert flow.</returns>
        public ExecutionResult InsertItemContrib(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return InsertItem(Model, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Compatibility wrapper for <see cref="InsertItems(IEnumerable{ModelClass}, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="ModelItems">The model items to insert.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core batch insert flow.</returns>
        public ExecutionResult InsertItemsContrib(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return InsertItems(ModelItems, Transaction, CommandTimeout);
        }

        /// <summary>
        /// The m SQL insert command
        /// </summary>
        private  string mSqlInsertCommand; //= Utilities.GetInsertSqlCommand(typeof(ModelClass));

        /// <summary>
        /// Inserts the item using a parameterized SQL command that respects column name mappings.
        /// For identity key ([Key]) models, the generated key value is stored in <see cref="ExecutionResult.Value"/>.
        /// For explicit key ([ExplicitKey]) models, <see cref="ExecutionResult.Value"/> is 0.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
        /// <summary>
        /// Inserts the item using a parameterized SQL command that respects column name mappings.
        /// </summary>
        public ExecutionResult InsertItem(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItem()");

            try
            {
                if (Model == null) Model = ModelItem;
                if (Transaction == null) Transaction = DbTransaction;
                if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

                ValidateInsertContext(Model);
                EnsureInsertSqlCommandInitialized();

                var parameters = new DynamicParameters();
                foreach (var prop in EntityProperties)
                {
                    parameters.Add(prop.Name, prop.GetValue(Model));
                }

                string insertCommand = mSqlInsertCommand;
                string identityFragment = GetIdentityFragment();
                if (!string.IsNullOrWhiteSpace(identityFragment))
                {
                    insertCommand = $"{insertCommand}; {identityFragment}";
                }

                var scalar = DbConnection.ExecuteScalar(insertCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                long generatedId = WriteBackIdentity(Model, scalar);

                ER.Value = generatedId;
                SyncInsertedModelState(Model);

                ER.ResultCode = ExecutionResultCodes.Success;
                ER.ResultMessage = generatedId > 0
                    ? "Inserimento eseguito correttamente."
                    : "Inserimento eseguito correttamente senza identity restituita.";
                mAddNewState = false;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Inserts the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// For identity key ([Key]) models, the generated key value is stored in <see cref="ExecutionResult.Value"/>.
        /// For explicit key ([ExplicitKey]) models, <see cref="ExecutionResult.Value"/> is 0.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>

        public async Task<ExecutionResult> InsertItemAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItemAsync()");

            try
            {
                if (Model == null) Model = ModelItem;
                if (Transaction == null) Transaction = DbTransaction;
                if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

                ValidateInsertContext(Model);
                EnsureInsertSqlCommandInitialized();

                var parameters = new DynamicParameters();
                foreach (var prop in EntityProperties)
                {
                    parameters.Add(prop.Name, prop.GetValue(Model));
                }

                string insertCommand = mSqlInsertCommand;
                string identityFragment = GetIdentityFragment();
                if (!string.IsNullOrWhiteSpace(identityFragment))
                {
                    insertCommand = $"{insertCommand}; {identityFragment}";
                }

                var scalar = await DbConnection.ExecuteScalarAsync(insertCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                long generatedId = WriteBackIdentity(Model, scalar);

                ER.Value = generatedId;
                SyncInsertedModelState(Model);

                ER.ResultCode = ExecutionResultCodes.Success;
                ER.ResultMessage = generatedId > 0
                    ? "Inserimento asincrono eseguito correttamente."
                    : "Inserimento asincrono eseguito correttamente senza identity restituita.";
                mAddNewState = false;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Inserts multiple items using a parameterized SQL command that respects column name mappings.
        /// For identity key ([Key]) models, <see cref="ExecutionResult.Value"/> contains the count of inserted rows.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
        public ExecutionResult InsertItems(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItems()");
            long insertedCount = 0;

            try
            {
                if (ModelItems == null) ModelItems = this.ModelItems;
                if (Transaction == null) Transaction = DbTransaction;
                if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

                if (ModelItems == null)
                {
                    throw new InvalidOperationException("Nessun elenco di elementi da inserire.");
                }

                var itemsList = ModelItems.Where(x => x != null).ToList();
                if (itemsList.Count == 0)
                {
                    throw new InvalidOperationException("Nessun elenco di elementi da inserire.");
                }

                foreach (var model in itemsList)
                {
                    ValidateInsertContext(model);
                }

                EnsureInsertSqlCommandInitialized();

                var entityPropsCount = EntityProperties.Count;

                foreach (var model in itemsList)
                {
                    var parameters = new DynamicParameters();
                    for (int i = 0; i < entityPropsCount; i++)
                    {
                        var prop = EntityProperties[i];
                        parameters.Add(prop.Name, prop.GetValue(model));
                    }

                    string insertCommand = mSqlInsertCommand;
                    string identityFragment = GetIdentityFragment();
                    if (!string.IsNullOrWhiteSpace(identityFragment))
                    {
                        insertCommand = $"{insertCommand}; {identityFragment}";
                    }

                    var scalar = DbConnection.ExecuteScalar(insertCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                    long generatedId = WriteBackIdentity(model, scalar);

                    SyncInsertedModelState(model);
                    insertedCount++;
                }

                ER.Value = insertedCount;
                ER.ResultCode = ExecutionResultCodes.Success;
                ER.ResultMessage = $"Inseriti {insertedCount} elementi.";
                mAddNewState = false;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = insertedCount;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Inserts multiple items asynchronously using a parameterized SQL command that respects column name mappings.
        /// For identity key ([Key]) models, <see cref="ExecutionResult.Value"/> contains the count of inserted rows.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemsAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItemsAsync()");
            long insertedCount = 0;

            try
            {
                if (ModelItems == null) ModelItems = this.ModelItems;
                if (Transaction == null) Transaction = DbTransaction;
                if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

                if (ModelItems == null)
                {
                    throw new InvalidOperationException("Nessun elenco di elementi da inserire.");
                }

                var itemsList = ModelItems.Where(x => x != null).ToList();
                if (itemsList.Count == 0)
                {
                    throw new InvalidOperationException("Nessun elenco di elementi da inserire.");
                }

                foreach (var model in itemsList)
                {
                    ValidateInsertContext(model);
                }

                EnsureInsertSqlCommandInitialized();

                var entityPropsCount = EntityProperties.Count;

                foreach (var model in itemsList)
                {
                    var parameters = new DynamicParameters();
                    for (int i = 0; i < entityPropsCount; i++)
                    {
                        var prop = EntityProperties[i];
                        parameters.Add(prop.Name, prop.GetValue(model));
                    }

                    string insertCommand = mSqlInsertCommand;
                    string identityFragment = GetIdentityFragment();
                    if (!string.IsNullOrWhiteSpace(identityFragment))
                    {
                        insertCommand = $"{insertCommand}; {identityFragment}";
                    }

                    var scalar = await DbConnection.ExecuteScalarAsync(insertCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                    long generatedId = WriteBackIdentity(model, scalar);

                    SyncInsertedModelState(model);
                    insertedCount++;
                }

                ER.Value = insertedCount;
                ER.ResultCode = ExecutionResultCodes.Success;
                ER.ResultMessage = $"Inseriti {insertedCount} elementi.";
                mAddNewState = false;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = insertedCount;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Compatibility wrapper for <see cref="InsertItemAsync(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="Model">The model to insert.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemContribAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return await InsertItemAsync(Model, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Compatibility wrapper for <see cref="InsertItemsAsync(IEnumerable{ModelClass}, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="ModelItems">The model items to insert.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemsContribAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return await InsertItemsAsync(ModelItems, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Undoes the changes.
        /// </summary>
        /// <returns></returns>
    }
}
