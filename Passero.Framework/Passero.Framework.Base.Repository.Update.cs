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
        public bool UndoChanges()
        {
            //var ER = new ExecutionResult($"{mClassName}.UndoChanges()");
            var result = false;
            if (ModelItemShadow != null)
            {
                ModelItem = Utilities.Clone(ModelItemShadow);
                ModelItem = Utilities .WisejClone(ModelItemShadow); 
            }
            //ModelItem = ModelItemShadow;
            if (AddNewState == true)
            {
                AddNewState = false;
            }
            return result;
        }

        /// <summary>
        /// Updates the item using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="Model">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItem(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItem()");

            if (Model == null) Model = _ModelItem;
            if (Transaction == null) Transaction = DbTransaction;
            if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

            try
            {
                var @params = new DynamicParameters();
                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < entityPropsCount; i++)
                {
                    var prop = EntityProperties[i];
                    @params.Add(prop.Name, prop.GetValue(Model));
                }

                // Shadow PK per la WHERE: usa lo shadow interno se disponibile,
                // altrimenti il valore corrente (es. primo salvataggio senza shadow)
                var shadowSource = _ModelItemShadow ?? Model;
                for (int i = 0; i < primaryKeysCount; i++)
                {
                    var prop = EntityPrimaryKeys[i];
                    @params.Add($"{prop.Name}_shadow", prop.GetValue(shadowSource));
                }

                int result = DbConnection.Execute(mSqlUpdateCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                if (result > 0)
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
        /// Updates the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemAsync()");

            if (Model == null) Model = _ModelItem;
            if (Transaction == null) Transaction = DbTransaction;
            if (CommandTimeout == null) CommandTimeout = DbCommandTimeout;

            try
            {
                var @params = new DynamicParameters();
                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < entityPropsCount; i++)
                {
                    var prop = EntityProperties[i];
                    @params.Add(prop.Name, prop.GetValue(Model));
                }

                var shadowSource = _ModelItemShadow ?? Model;
                for (int i = 0; i < primaryKeysCount; i++)
                {
                    var prop = EntityPrimaryKeys[i];
                    @params.Add($"{prop.Name}_shadow", prop.GetValue(shadowSource));
                }

                int result = await DbConnection.ExecuteAsync(mSqlUpdateCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                if (result > 0)
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
        public async Task<ExecutionResult> UpdateItemsAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsAsync()");
            bool esito = false;

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
                esito = await DbConnection.UpdateAsync(ModelItems, Transaction, CommandTimeout);
                ER.Value = esito;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                esito = false;
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
        /// Updates the item.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
        


        public ExecutionResult UpdateItemContrib(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItem()");
            //ValidateConnection();
            bool result = false;

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
                result = DbConnection.Update(Model, Transaction, CommandTimeout);
                if (result)
                {
                    _ModelItemShadow = Model;
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
        private string mSqlUpdateCommand = Utilities.GetUpdateSqlCommand(typeof(ModelClass));
        /// <summary>
        /// SQLs the update command.
        /// </summary>
        /// <param name="Refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public string SqlUpdateCommand(bool Refresh = false)
        {
            if (Refresh)
            {
                mSqlUpdateCommand = Utilities.GetUpdateSqlCommand(typeof(ModelClass));
                EntityPrimaryKeys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(ModelClass));
                EntityProperties = Utilities.GetModelPropertiesInfo(typeof(ModelClass), true);
            }
            return mSqlUpdateCommand;
        }



        /// <summary>
        /// Updates the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="ModelItem  ">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public ExecutionResult UpdateItemEx(ModelClass ModelItem = null,  IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return UpdateItem(ModelItem, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Updates the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="ModelItemShadow">The model item shadow.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public ExecutionResult UpdateItemEx(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return UpdateItem(ModelItem, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Alias of <see cref="UpdateItemAsync"/> kept for backward compatibility.
        /// The <paramref name="ModelItemShadow"/> parameter is accepted but ignored:
        /// the internal shadow is always used as the WHERE key source.
        /// </summary>
        public async Task<ExecutionResult> UpdateItemExAsync(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return await UpdateItemAsync(ModelItem, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Updates the items.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItems(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");
           
            bool esito = false;

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
                esito = DbConnection.Update(ModelItems);
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                esito = false;
            }
            LastExecutionResult = ER;
            return ER;

        }

        public ExecutionResult UpdateItemsEx(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsEx()");
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

                    affectedrecords += DbConnection.Execute(mSqlUpdateCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
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
