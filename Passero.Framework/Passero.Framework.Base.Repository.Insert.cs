using Dapper;
using Dapper.ColumnMapper;
using Dapper.Contrib.Extensions;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Passero.Framework.Extensions;
using Wisej.Web;
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

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItemContrib(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.InsertItem()");
            long x = 0;
            if (Model == null)
            {
                Model = ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {

                x = DbConnection.Insert(Model, Transaction, CommandTimeout);

                ModelItem = Model;
                ModelItemShadow = Model;
                if (ModelItems == null)
                {
                    ModelItems = new List<ModelClass>();
                }
                

                if (ModelItemsShadow == null)
                {
                    ModelItemsShadow = new List<ModelClass>();
                }
                
                ModelItemsShadow.Add(Model);
                

                mAddNewState = false;


            }
            catch (Exception ex)
            {
                //mAddNewState = False
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
        /// Inserts the items.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItemsContrib(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItems()");
            long x = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }
            try
            {

                x = DbConnection.Insert(ModelItems, Transaction, CommandTimeout);
                mAddNewState = false;
                ER.Value = x;

            }
            catch (Exception ex)
            {
                //mAddNewState = False
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = 0;
                HandleException(ER);

            }

            LastExecutionResult = (ER);
            return ER;

        }


        /// <summary>
        /// The m SQL insert command
        /// </summary>
        private readonly string mSqlInsertCommand = Utilities.GetInsertSqlCommand(typeof(ModelClass));

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
            if (Model == null) Model = ModelItem;
            if (Transaction == null) Transaction = DbTransaction;
            if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

            try
            {
                var @params = new DynamicParameters();
                var entityPropsCount = EntityProperties.Count;

                for (int i = 0; i < entityPropsCount; i++)
                {
                    var prop = EntityProperties[i];
                    @params.Add(prop.Name, prop.GetValue(Model));
                }

                //var scalar = DbConnection.ExecuteScalar(mSqlInsertCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                var insertCommand = $"{mSqlInsertCommand}; {GetIdentityFragment()}";
                var scalar = DbConnection.ExecuteScalar(insertCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                
                long generatedId = scalar != null && scalar != DBNull.Value ? Convert.ToInt64(scalar) : 0;

                // Write back the generated identity key into the model before setting the shadow,
                // so that subsequent UpdateItem calls use the correct PK in the WHERE clause.
                if (generatedId > 0)
                {
                    var identityKey = EntityPrimaryKeys
                        .FirstOrDefault(p => p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);
                    if (identityKey != null && identityKey.CanWrite)
                    {
                        identityKey.SetValue(Model, Convert.ChangeType(generatedId, identityKey.PropertyType));
                    }
                }

                ER.Value = generatedId;
                _ModelItem = Model;
                _ModelItemShadow = Utilities.Clone(Model); // shadow coerente con la PK aggiornata
                ModelItemShadow = _ModelItemShadow;

                if (ModelItems == null) ModelItems = new List<ModelClass>();
                if (ModelItemsShadow == null) ModelItemsShadow = new List<ModelClass>();
                ModelItemsShadow.Add(Utilities.Clone(Model));

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
            if (Model == null) Model = ModelItem;
            if (Transaction == null) Transaction = DbTransaction;
            if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

            try
            {
                var @params = new DynamicParameters();
                var entityPropsCount = EntityProperties.Count;

                for (int i = 0; i < entityPropsCount; i++)
                {
                    var prop = EntityProperties[i];
                    @params.Add(prop.Name, prop.GetValue(Model));
                }

                //var scalar = await DbConnection.ExecuteScalarAsync(mSqlInsertCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                
                var insertCommand = $"{mSqlInsertCommand}; {GetIdentityFragment()}";
                var scalar = DbConnection.ExecuteScalar(insertCommand, @params, Transaction, CommandTimeout, CommandType.Text);

                long generatedId = scalar != null && scalar != DBNull.Value ? Convert.ToInt64(scalar) : 0;

                // Write back the generated identity key into the model before setting the shadow,
                // so that subsequent UpdateItem calls use the correct PK in the WHERE clause.
                if (generatedId > 0)
                {
                    var identityKey = EntityPrimaryKeys
                        .FirstOrDefault(p => p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);
                    if (identityKey != null && identityKey.CanWrite)
                    {
                        identityKey.SetValue(Model, Convert.ChangeType(generatedId, identityKey.PropertyType));
                    }
                }

                ER.Value = generatedId;
                _ModelItem = Model;
                _ModelItemShadow = Utilities.Clone(Model); // shadow coerente con la PK aggiornata
                ModelItemShadow = _ModelItemShadow;

                if (ModelItems == null) ModelItems = new List<ModelClass>();
                if (ModelItemsShadow == null) ModelItemsShadow = new List<ModelClass>();
                ModelItemsShadow.Add(Utilities.Clone(Model));

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
        public ExecutionResult InsertItems(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItems()");
            long insertedCount = 0;

            if (ModelItems == null) ModelItems = this.ModelItems;
            if (Transaction == null) Transaction = DbTransaction;
            if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

            try
            {
                // Cache dell'identity key per il write-back (null se ExplicitKey)
                var identityKey = EntityPrimaryKeys
                    .FirstOrDefault(p => p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

                var entityPropsCount = EntityProperties.Count;

                if (this.ModelItemsShadow == null) this.ModelItemsShadow = new List<ModelClass>();

                foreach (var model in ModelItems)
                {
                    var @params = new DynamicParameters();
                    for (int i = 0; i < entityPropsCount; i++)
                    {
                        var prop = EntityProperties[i];
                        @params.Add(prop.Name, prop.GetValue(model));
                    }

                    //var scalar = DbConnection.ExecuteScalar(mSqlInsertCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                    var insertCommand = $"{mSqlInsertCommand}; {GetIdentityFragment()}";
                    var scalar = DbConnection.ExecuteScalar(insertCommand, @params, Transaction, CommandTimeout, CommandType.Text);


                    long generatedId = scalar != null && scalar != DBNull.Value ? Convert.ToInt64(scalar) : 0;

                    // Write back dell'identity key nel singolo item prima di clonarlo nello shadow
                    if (generatedId > 0 && identityKey != null && identityKey.CanWrite)
                    {
                        identityKey.SetValue(model, Convert.ChangeType(generatedId, identityKey.PropertyType));
                    }

                    this.ModelItemsShadow.Add(Utilities.Clone(model));
                    insertedCount++;
                }

                mAddNewState = false;
                ER.Value = insertedCount;
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

            if (ModelItems == null) ModelItems = this.ModelItems;
            if (Transaction == null) Transaction = DbTransaction;
            if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

            try
            {
                // Cache dell'identity key per il write-back (null se ExplicitKey)
                var identityKey = EntityPrimaryKeys
                    .FirstOrDefault(p => p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

                var entityPropsCount = EntityProperties.Count;

                if (this.ModelItemsShadow == null) this.ModelItemsShadow = new List<ModelClass>();

                foreach (var model in ModelItems)
                {
                    var @params = new DynamicParameters();
                    for (int i = 0; i < entityPropsCount; i++)
                    {
                        var prop = EntityProperties[i];
                        @params.Add(prop.Name, prop.GetValue(model));
                    }

                    //var scalar = await DbConnection.ExecuteScalarAsync(mSqlInsertCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                    var insertCommand = $"{mSqlInsertCommand}; {GetIdentityFragment()}";
                    var scalar = DbConnection.ExecuteScalar(insertCommand, @params, Transaction, CommandTimeout, CommandType.Text);


                    long generatedId = scalar != null && scalar != DBNull.Value ? Convert.ToInt64(scalar) : 0;

                    // Write back dell'identity key nel singolo item prima di clonarlo nello shadow
                    if (generatedId > 0 && identityKey != null && identityKey.CanWrite)
                    {
                        identityKey.SetValue(model, Convert.ChangeType(generatedId, identityKey.PropertyType));
                    }

                    this.ModelItemsShadow.Add(Utilities.Clone(model));
                    insertedCount++;
                }

                mAddNewState = false;
                ER.Value = insertedCount;
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
        /// Inserts the item asynchronously.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemContribAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItemAsync()");
            long x = 0;
            if (Model == null)
            {
                Model = ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                x = await DbConnection.InsertAsync(Model, Transaction, CommandTimeout);

                ModelItem = Model;
                ModelItemShadow = Model;
                if (ModelItems == null)
                {
                    ModelItems = new List<ModelClass>();
                }
                if (ModelItemsShadow == null)
                {
                    ModelItemsShadow = new List<ModelClass>();
                }
                ModelItemsShadow.Add(Model);
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
        /// Inserts the items asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemsContribAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItemsAsync()");
            long x = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }
            try
            {
                x = await DbConnection.InsertAsync(ModelItems, Transaction, CommandTimeout);
                mAddNewState = false;
                ER.Value = x;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = 0;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }



        /// <summary>
        /// Undoes the changes.
        /// </summary>
        /// <returns></returns>
    }
}
