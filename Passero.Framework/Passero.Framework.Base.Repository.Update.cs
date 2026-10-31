using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Concurrent;
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
        public bool UndoChanges()
        {
            //var ER = new ExecutionResult($"{mClassName}.UndoChanges()");
            var result = false;
            if (ModelItemShadow != null)
            {
                ModelItem = Utilities.Clone(ModelItemShadow);
                //ModelItem = Utilities.WisejClone(ModelItemShadow);
            }
            //ModelItem = ModelItemShadow;
            if (AddNewState == true)
            {
                AddNewState = false;
            }
            return result;
        }


        private void EnsureUpdateSqlCommandInitialized()
        {
            if (ProviderFeatures == null)
            {
                ProviderFeatures = DbConnection != null
                    ? ProviderFeaturesResolver.FromConnection(DbConnection)
                    : new ProviderFeatures();
            }

            if (string.IsNullOrWhiteSpace(mSqlUpdateCommand))
            {
                SqlUpdateCommand(true);
            }

            if (string.IsNullOrWhiteSpace(mSqlUpdateCommand))
            {
                throw new InvalidOperationException("Il comando SQL di update non è stato inizializzato.");
            }
        }

        private void ValidateUpdateContext(ModelClass model)
        {
            if (model == null)
            {
                throw new InvalidOperationException("Nessun modello da aggiornare. Specificare un Model o impostare il modello corrente.");
            }

            if (DbConnection == null)
            {
                throw new InvalidOperationException("La connessione al database non è stata inizializzata.");
            }

            if (EntityProperties == null || EntityProperties.Count == 0)
            {
                throw new InvalidOperationException("Nessuna proprietà disponibile per l'update.");
            }

            if (EntityPrimaryKeys == null || EntityPrimaryKeys.Count == 0)
            {
                throw new InvalidOperationException("Nessuna primary key definita sul modello.");
            }

            DemandSecurityAccess(Passero.Framework.Base.AccessPermission.Write);
        }

        private void ValidateUpdateItemsContext(IEnumerable<ModelClass> modelItems, IList<ModelClass> shadows)
        {
            DemandSecurityAccess(Passero.Framework.Base.AccessPermission.Write);

            if (modelItems == null)
            {
                throw new InvalidOperationException("Nessun elenco di elementi da aggiornare.");
            }

            if (DbConnection == null)
            {
                throw new InvalidOperationException("La connessione al database non è stata inizializzata.");
            }

            if (EntityProperties == null || EntityProperties.Count == 0)
            {
                throw new InvalidOperationException("Nessuna proprietà disponibile per l'update.");
            }

            if (EntityPrimaryKeys == null || EntityPrimaryKeys.Count == 0)
            {
                throw new InvalidOperationException("Nessuna primary key definita sul modello.");
            }

            if (shadows == null)
            {
                throw new InvalidOperationException("Nessuno shadow disponibile per l'aggiornamento in batch.");
            }

            if (modelItems.Cast<object>().Count() != shadows.Count)
            {
                throw new InvalidOperationException(
                    "Il numero di elementi da aggiornare non coincide con il numero di shadow disponibili.");
            }
        }


        private DynamicParameters CreateUpdateParameters(ModelClass model)
        {
            var parameters = new DynamicParameters();

            for (int i = 0; i < EntityProperties.Count; i++)
            {
                var prop = EntityProperties[i];
                parameters.Add(prop.Name, prop.GetValue(model));
            }

            var shadowSource = _ModelItemShadow ?? model;
            for (int i = 0; i < EntityPrimaryKeys.Count; i++)
            {
                var prop = EntityPrimaryKeys[i];
                var shadowValue = prop.GetValue(shadowSource);

                if (shadowValue == null || shadowValue == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        $"Il valore della primary key '{prop.Name}' non è disponibile per il WHERE.");
                }

                parameters.Add($"{prop.Name}_shadow", shadowValue);
            }

            return parameters;
        }
        private DynamicParameters CreateUpdateParameters(ModelClass model, ModelClass shadowSource = null)
        {
            var parameters = new DynamicParameters();

            for (int i = 0; i < EntityProperties.Count; i++)
            {
                var prop = EntityProperties[i];
                parameters.Add(prop.Name, prop.GetValue(model));
            }

            var effectiveShadowSource = shadowSource ?? _ModelItemShadow ?? model;

            for (int i = 0; i < EntityPrimaryKeys.Count; i++)
            {
                var prop = EntityPrimaryKeys[i];
                var shadowValue = prop.GetValue(effectiveShadowSource);

                if (shadowValue == null || shadowValue == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        $"Il valore della primary key '{prop.Name}' non è disponibile per il WHERE.");
                }

                parameters.Add($"{prop.Name}_shadow", shadowValue);
            }

            return parameters;
        }

        /// <summary>
        /// Updates the item using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="Model">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
        public ExecutionResult UpdateItem(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItem()");

            try
            {
                if (Model == null) Model = _ModelItem;
                if (Transaction == null) Transaction = DbTransaction;
                if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

                ValidateUpdateContext(Model);
                EnsureUpdateSqlCommandInitialized();

                var parameters = CreateUpdateParameters(Model);

                int result = DbConnection.Execute(
                    mSqlUpdateCommand,
                    parameters,
                    Transaction,
                    CommandTimeout,
                    CommandType.Text);

                ER.Value = result;

                if (result > 0)
                {
                    _ModelItemShadow = Model;
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = "Aggiornamento eseguito correttamente.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga aggiornata.";
                }
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
        /// Updates the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemAsync()");

            try
            {
                if (Model == null) Model = _ModelItem;
                if (Transaction == null) Transaction = DbTransaction;
                if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

                ValidateUpdateContext(Model);
                EnsureUpdateSqlCommandInitialized();

                var parameters = CreateUpdateParameters(Model);

                int result = await DbConnection.ExecuteAsync(
                    mSqlUpdateCommand,
                    parameters,
                    Transaction,
                    CommandTimeout,
                    CommandType.Text);

                ER.Value = result;

                if (result > 0)
                {
                    _ModelItemShadow = Model;
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = "Aggiornamento eseguito correttamente.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga aggiornata.";
                }
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
        /// Updates the item asynchronously.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemContribAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemAsync()");
            bool result = false;
            DemandSecurityAccess(Passero.Framework.Base.AccessPermission.Write);

            if (Model == null)
            {
                Model = _ModelItem;
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
                result = await DbConnection.UpdateAsync(Model, Transaction, CommandTimeout);
                if (result)
                {
                    _ModelItemShadow = Model;
                }
                ER.Value = result;
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
        /// Updates the items asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        /// 
        public async Task<ExecutionResult> UpdateItemsAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsAsync()");
            int affectedRecords = 0;

            try
            {
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

                var itemsList = ModelItems as IList<ModelClass> ?? ModelItems.ToList();
                var shadowsList = ModelItemsShadow as IList<ModelClass> ?? new List<ModelClass>();

                ValidateUpdateItemsContext(itemsList, shadowsList);
                EnsureUpdateSqlCommandInitialized();

                for (int i = 0; i < itemsList.Count; i++)
                {
                    var currentItem = itemsList[i];
                    var shadowItem = i < shadowsList.Count ? shadowsList[i] : null;

                    if (shadowItem == null)
                    {
                        throw new InvalidOperationException("Nessuno shadow disponibile per l'elemento da aggiornare.");
                    }

                    if (_compareFunc(currentItem, shadowItem))
                    {
                        continue;
                    }

                    ValidateUpdateContext(currentItem);

                    var parameters = CreateUpdateParameters(currentItem, shadowItem);

                    int rowCount = await DbConnection.ExecuteAsync(
                        mSqlUpdateCommand,
                        parameters,
                        Transaction,
                        CommandTimeout,
                        CommandType.Text);

                    affectedRecords += rowCount;

                    if (rowCount > 0 && i < ModelItemsShadow.Count)
                    {
                        ModelItemsShadow[i] = currentItem;
                    }
                }

                ER.Value = affectedRecords;

                if (affectedRecords > 0)
                {
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = "Aggiornamenti eseguiti correttamente.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga aggiornata.";
                }
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedRecords;
            }

            LastExecutionResult = ER;
            return ER;
        }

        public async Task<ExecutionResult> UpdateItemsExAsync(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsExAsync()");
            int affectedrecords = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (ModelItemsShadow == null)
            {
                ModelItemsShadow = this.ModelItemsShadow;
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
                // Converti una sola volta a IList
                var itemsList = ModelItems as IList<ModelClass> ?? ModelItems.ToList();
                var shadowsList = ModelItemsShadow as IList<ModelClass> ?? ModelItemsShadow.ToList();

                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < itemsList.Count; i++)
                {
                    // Skip se non ci sono modifiche
                    if (_compareFunc(itemsList[i], shadowsList[i]))
                    {
                        continue;
                    }

                    var parameters = new DynamicParameters();

                    // Loop ottimizzato con accesso diretto all'indice
                    for (int j = 0; j < entityPropsCount; j++)
                    {
                        var prop = EntityProperties[j];
                        parameters.Add(prop.Name, prop.GetValue(itemsList[i]));
                    }

                    for (int j = 0; j < primaryKeysCount; j++)
                    {
                        var prop = EntityPrimaryKeys[j];
                        parameters.Add($"{prop.Name}_shadow", prop.GetValue(shadowsList[i]));
                    }

                    affectedrecords += await DbConnection.ExecuteAsync(mSqlUpdateCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                    _ModelItemsShadow[i] = itemsList[i];
                }

                ER.Value = affectedrecords;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedrecords;
            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Compatibility wrapper for <see cref="UpdateItem(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="Model">The model to update.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core update flow.</returns>
        public ExecutionResult UpdateItemContrib(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return UpdateItem(Model, Transaction, CommandTimeout);
        }

        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _entityPropertiesCache = new();
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _entityPrimaryKeysCache = new();
        private List<PropertyInfo> _entityProperties;
        private List<PropertyInfo> _entityPrimaryKeys;
        /// <summary>
        /// The entity properties
        /// </summary>
        /// 
        public List<PropertyInfo> EntityProperties
        {
            get
            {
                if (_entityProperties == null)
                {
                    _entityProperties = _entityPropertiesCache.GetOrAdd(typeof(ModelClass), type =>
                            Utilities.GetModelPropertiesInfo(type, true));
                }
                return _entityProperties;
            }
            set
            {
                _entityProperties = value;
                _entityPropertiesCache[typeof(ModelClass)] = value; // Aggiorna il cache
            }
        }
        //public List<PropertyInfo> EntityProperties = DapperHelper.Utilities.GetPropertiesInfo(typeof(ModelClass), true);

        /// <summary>
        /// The entity primary keys
        /// </summary>
        public List<PropertyInfo> EntityPrimaryKeys
        {
            get
            {
                if (_entityPrimaryKeys == null)
                {
                    _entityPrimaryKeys = _entityPrimaryKeysCache.GetOrAdd(typeof(ModelClass), type =>
                        Utilities.GetModelPrimaryKeysPropertiesInfo(type));
                }
                return _entityPrimaryKeys;
            }
            set
            {
                _entityPrimaryKeys = value;
                _entityPrimaryKeysCache[typeof(ModelClass)] = value; // Aggiorna il cache
            }
        }
        //public List<PropertyInfo> EntityPrimaryKeys = DapperHelper.Utilities.GetPrimaryKeysPropertiesInfo(typeof(ModelClass));



        /// <summary>
        /// The m SQL update command
        /// </summary>
        private string mSqlUpdateCommand;
        /// <summary>
        /// SQLs the update command.
        /// </summary>
        /// <param name="Refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public string SqlUpdateCommand(bool Refresh = false)
        {
            if (Refresh)
            {
                mSqlUpdateCommand = Utilities.GetUpdateSqlCommand(typeof(ModelClass),ProviderFeatures );
                EntityPrimaryKeys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(ModelClass));
                EntityProperties = Utilities.GetModelPropertiesInfo(typeof(ModelClass), true);
            }
            return mSqlUpdateCommand;
        }

        public string SqlInsertCommand(bool Refresh = false)
        {
            if (Refresh)
            {
                mSqlInsertCommand = Utilities.GetInsertSqlCommand(typeof(ModelClass), ProviderFeatures);
                EntityPrimaryKeys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(ModelClass));
                EntityProperties = Utilities.GetModelPropertiesInfo(typeof(ModelClass), true);
            }
            return mSqlInsertCommand;
        }

        public string SqlDeleteCommand(bool Refresh = false)
        {
            if (Refresh)
            {
                mSqlDeleteCommand  = Utilities.GetDeleteSqlCommand(typeof(ModelClass), ProviderFeatures);
                EntityPrimaryKeys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(ModelClass));
                EntityProperties = Utilities.GetModelPropertiesInfo(typeof(ModelClass), true);
            }
            return mSqlDeleteCommand;
        }


        /// <summary>
        /// Compatibility overload that preserves the legacy signature and delegates to <see cref="UpdateItem(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="Model">The model to update.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core update flow.</returns>
        public ExecutionResult UpdateItemEx(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return UpdateItem(Model, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Compatibility overload that preserves the legacy shadow-aware signature and delegates to <see cref="UpdateItem(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="Model">The model to update.</param>
        /// <param name="ModelItemShadow">The shadow state used as the comparison key.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core update flow.</returns>
        public ExecutionResult UpdateItemEx(ModelClass Model = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var previousShadow = ModelItemShadow;
            try
            {
                if (ModelItemShadow != null)
                {
                    this.ModelItemShadow = ModelItemShadow;
                }

                return UpdateItem(Model, Transaction, CommandTimeout);
            }
            finally
            {
                this.ModelItemShadow = previousShadow;
            }
        }

        /// <summary>
        /// Compatibility overload for the async update path that preserves the legacy shadow-aware signature and delegates to <see cref="UpdateItemAsync(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="Model">The model to update.</param>
        /// <param name="ModelItemShadow">The shadow state used as the comparison key.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core async update flow.</returns>
        public async Task<ExecutionResult> UpdateItemExAsync(ModelClass Model = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var previousShadow = this.ModelItemShadow;
            try
            {
                if (ModelItemShadow != null)
                {
                    this.ModelItemShadow = ModelItemShadow;
                }

                return await UpdateItemAsync(Model, Transaction, CommandTimeout);
            }
            finally
            {
                this.ModelItemShadow = previousShadow;
            }
        }

        /// <summary>
        /// Updates the items.
        /// </summary>
        /// <param name="ModelItems">The model items to update.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
        public ExecutionResult UpdateItems(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");
            int affectedRecords = 0;

            try
            {
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

                var itemsList = ModelItems as IList<ModelClass> ?? ModelItems.ToList();
                var shadowsList = ModelItemsShadow as IList<ModelClass> ?? new List<ModelClass>();

                ValidateUpdateItemsContext(itemsList, shadowsList);
                EnsureUpdateSqlCommandInitialized();

                for (int i = 0; i < itemsList.Count; i++)
                {
                    var currentItem = itemsList[i];
                    var shadowItem = i < shadowsList.Count ? shadowsList[i] : null;

                    if (shadowItem == null)
                    {
                        throw new InvalidOperationException("Nessuno shadow disponibile per l'elemento da aggiornare.");
                    }

                    if (_compareFunc(currentItem, shadowItem))
                    {
                        continue;
                    }

                    ValidateUpdateContext(currentItem);

                    var parameters = CreateUpdateParameters(currentItem, shadowItem);

                    int rowCount = DbConnection.Execute(
                        mSqlUpdateCommand,
                        parameters,
                        Transaction,
                        CommandTimeout,
                        CommandType.Text);

                    affectedRecords += rowCount;

                    if (rowCount > 0 && i < ModelItemsShadow.Count)
                    {
                        ModelItemsShadow[i] = currentItem;
                    }
                }

                ER.Value = affectedRecords;

                if (affectedRecords > 0)
                {
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = "Aggiornamenti eseguiti correttamente.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga aggiornata.";
                }
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedRecords;
            }

            LastExecutionResult = ER;
            return ER;
        }


        public ExecutionResult UpdateItemsEx(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var previousShadows = this.ModelItemsShadow;
            try
            {
                if (ModelItemsShadow != null)
                {
                    this.ModelItemsShadow = ModelItemsShadow.ToList();
                }

                return UpdateItems(ModelItems, Transaction, CommandTimeout);
            }
            finally
            {
                this.ModelItemsShadow = previousShadows;
            }
        }

        /// <summary>
        /// Gets the empty model item.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        /// <summary>
        /// The m SQL delete command
        /// </summary>
    }
}
