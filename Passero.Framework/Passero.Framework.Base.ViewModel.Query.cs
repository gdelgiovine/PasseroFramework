using Dapper;
using FastDeepCloner;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Wisej.Web;
using Wisej.Web.Data;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {
        /// <summary>
        /// Reloads the items.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ReloadItems()
        {
            var ERContenxt = $"{mClassName}.ReloadItems()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);
            ER = Repository.ReloadItems();
            ER.Context = ERContenxt;
            return ER;
        }


        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<ModelClass> GetItem(string SqlQuery, object Parameters, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.GetItems()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContenxt);
            ER.Value = null;
            ER = Repository.GetItem(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);
            
           

            switch (UseModelData)
            {
                case UseModelData.External:
                    mModelItemShadow = ER.Value;
                    break;
                case UseModelData.InternalRepository:
                    Repository.ModelItem = ER.Value;
                    break;
                default:
                    break;
            }
            ModelItem = ER.Value;
            //if (ModelItem == null)
            //{
            //    ER.ResultCode = ExecutionResultCodes.Failed;
            //    ER.ResultMessage = $"No data for query\n{Framework .DapperHelper .Utilities .ResolveSQL (SqlQuery,(DynamicParameters)Parameters)}";
            //}
            DataNavigatorRaiseEventBoundCompleted();
            if (!ER.Success)
            {
                HandleExeception(ER.ToExecutionResult());
            }
            return ER;
        }


        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetItems(string SqlQuery, object Parameters = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            string ERContenxt = $"{mClassName}.GetItems()";
            ExecutionResult<IList<ModelClass>> ER = new ExecutionResult<IList<ModelClass>>(ERContenxt);


            IList<ModelClass> x = null;
            try
            {
                mCurrentModelItemIndex = -1;
                Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
                Repository.ErrorNotificationMode = ErrorNotificationMode;

                Repository.SQLQuery = SqlQuery;

                //this.Repository.Parameters = (DynamicParameters)Parameters;
                Repository.Parameters = Utilities.GetDynamicParameters(Parameters);
                ER = Repository.GetItems(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);

                if (ER.Success)
                {
                    x = ER.Value;
                    mCurrentModelItemIndex = 0;
                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            mModelItemShadow = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            mModelItems = x;
                            //If (AutoUpdateModelItemsShadows) Then
                            SetModelItemsShadow();
                            //End If
                            SetModelItemShadow();
                            break;
                        case UseModelData.InternalRepository:
                            Repository.ModelItems = x;
                            Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            //If (AutoUpdateModelItemsShadows) Then
                            Repository.SetModelItemsShadow();
                            //End If
                            Repository.SetModelItemShadow();
                            Repository.CurrentModelItemIndex = 0;
                            break;
                        default:
                            break;
                    }

                    if (mDataBindingMode == DataBindingMode.BindingSource)
                    {
                        mBindingSource.DataSource = ModelItems;
                    }
                }
              
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query\n{Framework.Utilities.ResolveSQL(SqlQuery, (DynamicParameters)Parameters)}";
               
            }
            MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            //DataNavigatorRaiseEventBoundCompleted();
            if (!ER.Success)
            {
                HandleExeception(ER.ToExecutionResult());
            }
            return ER;
        }
        /// <summary>
        /// Sets the binding source.
        /// </summary>
        public void SetBindingSource()
        {
            if (mDataBindingMode == DataBindingMode.BindingSource)
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mBindingSource.DataSource = ModelItems;
                        break;
                    case UseModelData.InternalRepository:
                        mBindingSource.DataSource = Repository.ModelItems;
                        break;
                    default:
                        break;
                }
            }
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
            var ERContenxt = $"{mClassName}.GetAllItems()";
            ExecutionResult<IList<ModelClass>> ER = new ExecutionResult<IList<ModelClass>>(ERContenxt);
            IList<ModelClass> x = null;
            try
            {
                ER = Repository.GetAllItems(Transaction, Buffered, CommandTimeout);
                if (ER.Success)
                {
                    x = ER.Value;
                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            mModelItemShadow = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            mModelItems = x;
                            if (mDataBindingMode == DataBindingMode.BindingSource)
                            {
                                mBindingSource.DataSource = ModelItems;

                            }
                            break;
                            
                        case UseModelData.InternalRepository:
                            Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            Repository.ModelItems = x;
                            if (mDataBindingMode == DataBindingMode.BindingSource)
                            {
                                mBindingSource.DataSource = Repository.ModelItems;

                            }
                            break;
                            
                        default:
                            break;
                    }

                }
                else
                {
                    HandleExeception(ER.ToExecutionResult());
                }   
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {SQLQuery}";
                HandleExeception(ER.ToExecutionResult());
            }

            //if (this.DataNavigator != null)
            //{
            //    ReflectionHelper.CallByName(this.DataNavigator, "InitDataNavigator", Microsoft.VisualBasic.CallType.Method, null);
            //}

            MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            //DataNavigatorRaiseEventBoundCompleted()
            return ER;
        }




        /// <summary>
        /// Gets the item asynchronously.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult<ModelClass>> GetItemAsync(string SqlQuery, object Parameters, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ERContext = $"{mClassName}.GetItemAsync()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContext);
            ER.Value = null;

            try
            {
                ER = await Repository.GetItemAsync(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);
              
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mModelItemShadow = ER.Value;
                        break;
                    case UseModelData.InternalRepository:
                        Repository.ModelItem = ER.Value;
                        break;
                    default:
                        break;
                }

                ModelItem = ER.Value;
                DataNavigatorRaiseEventBoundCompleted();
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query\n{Framework.Utilities.ResolveSQL(SqlQuery, (DynamicParameters)Parameters)}";
               
            }
            if (!ER.Success)
            {
                HandleExeception(ER.ToExecutionResult());
            }

            return ER;
        }


        /// <summary>
        /// Gets the items asynchronously.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult<IList<ModelClass>>> GetItemsAsync(string SqlQuery, object Parameters = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            string ERContext = $"{mClassName}.GetItemsAsync()";
            ExecutionResult<IList<ModelClass>> ER = new ExecutionResult<IList<ModelClass>>(ERContext);

            IList<ModelClass> x = null;
            try
            {
                mCurrentModelItemIndex = -1;
                Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
                Repository.ErrorNotificationMode = ErrorNotificationMode;

                Repository.SQLQuery = SqlQuery;
                Repository.Parameters = Utilities.GetDynamicParameters(Parameters);

                ER = await Repository.GetItemsAsync(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);

                if (ER.Success)
                {
                    x = ER.Value;
                    mCurrentModelItemIndex = 0;

                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            mModelItemShadow = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            mModelItems = x;
                            SetModelItemsShadow();
                            SetModelItemShadow();
                            break;

                        case UseModelData.InternalRepository:
                            Repository.ModelItems = x;
                            Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            Repository.SetModelItemsShadow();
                            Repository.SetModelItemShadow();
                            Repository.CurrentModelItemIndex = 0;
                            break;

                        default:
                            break;
                    }

                    if (mDataBindingMode == DataBindingMode.BindingSource)
                    {
                        mBindingSource.DataSource = ModelItems;
                    }
                }
           
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query\n{Framework.Utilities.ResolveSQL(SqlQuery, (DynamicParameters)Parameters)}";
               //HandleExeception(ER.ToExecutionResult());
            }

            MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            if (!ER.Success)
            {
                HandleExeception(ER.ToExecutionResult());
            }
            return ER;
        }


        /// <summary>
        /// Gets all items asynchronously.
        /// </summary>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult<IList<ModelClass>>> GetAllItemsAsync(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ERContext = $"{mClassName}.GetAllItemsAsync()";
            ExecutionResult<IList<ModelClass>> ER = new ExecutionResult<IList<ModelClass>>(ERContext);
            IList<ModelClass> x = null;
            
            try
            {
                ER = await Repository.GetAllItemsAsync(Transaction, Buffered, CommandTimeout);
                
                if (ER.Success)
                {
                    x = ER.Value;
                    
                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            mModelItemShadow = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            mModelItems = x;
                            if (mDataBindingMode == DataBindingMode.BindingSource)
                            {
                                mBindingSource.DataSource = ModelItems;
                            }
                            break;
                            
                        case UseModelData.InternalRepository:
                            Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            Repository.ModelItems = x;
                            if (mDataBindingMode == DataBindingMode.BindingSource)
                            {
                                mBindingSource.DataSource = Repository.ModelItems;
                            }
                            break;
                            
                        default:
                            break;
                    }
                }
                else
                {
                    HandleExeception(ER.ToExecutionResult());
                }
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {SQLQuery}";
                HandleExeception(ER.ToExecutionResult());
            }

            MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            return ER;
        }
    }
}
