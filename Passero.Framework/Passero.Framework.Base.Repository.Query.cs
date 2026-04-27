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
        public string ResolvedSQLQuery(string SQLQuery = "", DynamicParameters Parameters = null)
        {
            if (SQLQuery != null && Parameters != null)
                return Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return  Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
        }

        /// <summary>
        /// Resolveds the SQL query.
        /// </summary>
        /// <returns></returns>
        public string ResolvedSQLQuery()
        {
            if (this.SQLQuery != null && this.Parameters != null)
                return Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
        }


        public void ValidateModelClass()
        {
            if (!typeof(ModelClass).IsAssignableFrom(typeof(ModelBase)))
            {
                throw new InvalidOperationException("ModelClass deve derivare da ModelBase.");
            }
        }


        ///// <summary>
        ///// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        ///// </summary>
        ///// <param name="DbContext">The database context.</param>
        //public Repository(Base.DbContext DbContext)
        //{

        //    _ModelItem = GetEmptyModel();
        //    SetModelItemShadow();
        //    SetModelItemsShadow();
        //    this.DbContext = DbContext;
        //    DbTransaction = DbContext.DbTransaction;
        //    DbConnection = DbContext.DbConnection ;
        //    DbObject = new Base.DbObject<ModelClass>(DbConnection);


        //}


        /// <summary>
        /// Determines whether [is model data changed] [the specified model shadow].
        /// </summary>
        /// <param name="ModelShadow">The model shadow.</param>
        /// <returns>
        ///   <c>true</c> if [is model data changed] [the specified model shadow]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsModelDataChanged(ModelClass modelShadow = null)
        {

            if (modelShadow is null)
            {
                modelShadow = _ModelItemShadow;
            }

            return !Utilities.ObjectsEquals(_ModelItem, modelShadow);

        }


        /// <summary>
        /// Handles the exeception.
        /// </summary>
        /// <param name="ER">The er.</param>
        public void HandleException(ExecutionResult ER)
        {

            return;

            //if (ER == null)
            //{
            //    return;
            //}

            //switch (ErrorNotificationMode)
            //{
            //    case ErrorNotificationModes.ThrowException:
            //        throw ER.Exception;
            //    case ErrorNotificationModes.Silent:
            //        break;
            //    case ErrorNotificationModes.ShowDialog:
            //        if (ErrorNotificationMessageBox != null)
            //        {
            //            StringBuilder msg = new StringBuilder();
            //            msg.AppendLine($"Context: {ER.Context}");
            //            msg.AppendLine($"Repository: {Name}");
            //            msg.AppendLine($"Error Message: {ER.ResultMessage}");
            //            msg.AppendLine($"Debug Info: {ER.DebugInfo}");
            //            ErrorNotificationMessageBox.Show(msg.ToString());
            //        }
            //        break;
            //    default:
            //        break;
            //}

        }


        /// <summary>
        /// Sets the SQL query.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        public void SetSQLQuery(string SQLQuery, DynamicParameters parameters)
        {
            this.SQLQuery = SQLQuery;
            Parameters = parameters;

        }




        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
    
        public ExecutionResult<ModelClass> GetItem(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var ER = new ExecutionResult<ModelClass>($"{mClassName}.GetItem()");
            ER.Value = null;
            try
            {
                _ModelItem = DbConnection.Query<ModelClass>(query, parameters, transaction, buffered, commandTimeout).SingleOrDefault();
                if (ViewModel != null)
                    ViewModel.ModelItem = _ModelItem;
                SetModelItemShadow();
                mSQLQuery = query;
                Parameters = Utilities.GetDynamicParameters(parameters);
                ER.Value = _ModelItem;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"SQLQuery = {query}";
                LastExecutionResult = ER.ToExecutionResult();
                HandleException(ER.ToExecutionResult());
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;
        }

        /// <summary>
        /// Gets the item asynchronously.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result with the item.</returns>
        public async Task<ExecutionResult<ModelClass>> GetItemAsync(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var ER = new ExecutionResult<ModelClass>($"{mClassName}.GetItemAsync()");
            ER.Value = null;

            try
            {
                _ModelItem = (await DbConnection.QueryAsync<ModelClass>(query, parameters, transaction, commandTimeout)).SingleOrDefault();
                if (ViewModel != null)
                    ViewModel.ModelItem = _ModelItem;
                SetModelItemShadow();
                mSQLQuery = query;
                Parameters = Utilities.GetDynamicParameters(parameters);
                ER.Value = _ModelItem;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"SQLQuery = {query}";
                LastExecutionResult = ER.ToExecutionResult();
                HandleException(ER.ToExecutionResult());
            }
            return ER;
        }

        /// <summary>
        /// Gets the items asynchronously.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result with the list of items.</returns>
        public async Task<ExecutionResult<IList<ModelClass>>> GetItemsAsync(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var ER = new ExecutionResult<IList<ModelClass>>($"{mClassName}.GetItemsAsync()");
            if (string.IsNullOrEmpty(query))
            {
                query = $"SELECT * FROM {Utilities.GetModelTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            _CurrentModelItemIndex = -1;
            try
            {
                _ModelItemsShadow = new List<ModelClass>();
                _ModelItems = (await DbConnection.QueryAsync<ModelClass>(query, parameters, transaction, commandTimeout)).ToList();
                if (_ModelItems.Count > 0)
                {
                    _ModelItem = _ModelItems.First();
                    _CurrentModelItemIndex = 0;
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _ModelItem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                    _CurrentModelItemIndex = 0;
                }
                SQLQuery = query;
                Parameters = Utilities.GetDynamicParameters(parameters);
                ER.Value = _ModelItems;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {query}";
                HandleException(ER.ToExecutionResult());
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;
        }


        /// <summary>
        /// Gets the current item.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetCurrentItem()
        {
            if (_ModelItems != null && _CurrentModelItemIndex > -1)
            {
                return _ModelItems[_CurrentModelItemIndex];
            }
            return null;
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetAllItems(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            return GetItems(mSQLQuery, Parameters, Transaction, Buffered, CommandTimeout);
        }



        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public async Task<ExecutionResult<IList<ModelClass>>>GetAllItemsAsync(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            string query = mSQLQuery;
            if (string.IsNullOrWhiteSpace(query))
            {
                query = $"SELECT * FROM {Utilities.GetModelTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            return await GetItemsAsync(query, Parameters, Transaction, Buffered, CommandTimeout);
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetItems(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var ER = new ExecutionResult<IList<ModelClass>>($"{mClassName}.GetItems()");
            //ValidateConnection();

            if (string.IsNullOrEmpty(query))
            {
                query = $"SELECT * FROM {Utilities.GetModelTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            _CurrentModelItemIndex = -1;
            try
            {
                _ModelItemsShadow = new List<ModelClass>();
                //_ModelItemsShadow.Clear();
                _ModelItems = DbConnection.Query<ModelClass>(query, parameters, transaction, buffered, commandTimeout).ToList();
                if (_ModelItems.Count > 0)
                {
                    _ModelItem = _ModelItems.First();
                    _CurrentModelItemIndex = 0;
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _ModelItem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                    _CurrentModelItemIndex = 0;
                }
                SQLQuery = query;
                Parameters =    Utilities.GetDynamicParameters(parameters);
                ER.Value = _ModelItems;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {query}";
                HandleException(ER.ToExecutionResult());
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }

        /// <summary>
        /// Reloads the items.
        /// </summary>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult ReloadItems(bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.ReloadItems()");
            try
            {
                if (mSQLQuery.IsNullOrWhiteSpace() == false)
                {
                    _ModelItems = DbConnection.Query<ModelClass>(mSQLQuery, Parameters, DbTransaction, Buffered, CommandTimeout).ToList();
                }

                if (_ModelItems.Count() > 0)
                {
                    _ModelItem = _ModelItems.First();
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _ModelItem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                }

            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {mSQLQuery}";
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;

        }





        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
    }
}
