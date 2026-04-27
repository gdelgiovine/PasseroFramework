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
        private readonly string mSqlDeleteCommand = Utilities.GetDeleteSqlCommand(typeof(ModelClass));

        /// <summary>
        /// Deletes the item using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItem(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItem()");

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
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
                var @params = new DynamicParameters();
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < primaryKeysCount; i++)
                {
                    var prop = EntityPrimaryKeys[i];
                    @params.Add(prop.Name, prop.GetValue(ModelItem));
                }

                int affected = DbConnection.Execute(mSqlDeleteCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                bool result = affected > 0;

                if (result)
                {
                    _ModelItems.Remove(ModelItem);
                    _ModelItemsShadow.Remove(ModelItem);
                    _ModelItem = GetEmptyModelItem();
                    _ModelItemShadow = GetEmptyModelItem();
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
        /// Deletes the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemAsync(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemAsync()");

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
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
                var @params = new DynamicParameters();
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < primaryKeysCount; i++)
                {
                    var prop = EntityPrimaryKeys[i];
                    @params.Add(prop.Name, prop.GetValue(ModelItem));
                }

                int affected = await DbConnection.ExecuteAsync(mSqlDeleteCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                bool result = affected > 0;

                if (result)
                {
                    _ModelItems.Remove(ModelItem);
                    _ModelItemsShadow.Remove(ModelItem);
                    _ModelItem = GetEmptyModelItem();
                    _ModelItemShadow = GetEmptyModelItem();
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
        /// Deletes multiple items using a parameterized SQL command that respects column name mappings.
        /// <see cref="ExecutionResult.Value"/> contains the count of deleted rows.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItems(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            int deletedCount = 0;

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
                var primaryKeysCount = EntityPrimaryKeys.Count;

                foreach (var model in ModelItems)
                {
                    var @params = new DynamicParameters();
                    for (int i = 0; i < primaryKeysCount; i++)
                    {
                        var prop = EntityPrimaryKeys[i];
                        @params.Add(prop.Name, prop.GetValue(model));
                    }

                    deletedCount += DbConnection.Execute(mSqlDeleteCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                }

                ER.Value = deletedCount;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = deletedCount;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Deletes multiple items asynchronously using a parameterized SQL command that respects column name mappings.
        /// <see cref="ExecutionResult.Value"/> contains the count of deleted rows.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemsAsync(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemsAsync()");
            int deletedCount = 0;

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
                var primaryKeysCount = EntityPrimaryKeys.Count;

                foreach (var model in ModelItems)
                {
                    var @params = new DynamicParameters();
                    for (int i = 0; i < primaryKeysCount; i++)
                    {
                        var prop = EntityPrimaryKeys[i];
                        @params.Add(prop.Name, prop.GetValue(model));
                    }

                    deletedCount += await DbConnection.ExecuteAsync(mSqlDeleteCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                }

                ER.Value = deletedCount;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = deletedCount;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItemContrib(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItem()");

            bool _result = false;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
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
                _result = DbConnection.Delete(ModelItem, Transaction, CommandTimeout);
                if (_result)
                {
                    _ModelItems.Remove(ModelItem);
                    //If (AutoUpdateModelItemsShadows) Then
                    _ModelItemsShadow.Remove(ModelItem);
                    //End If
                    _ModelItem = GetEmptyModelItem();
                    _ModelItemShadow = GetEmptyModelItem();
                }
                ER.Value = _result;

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
        /// Deletes the items.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItemsContrib(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            bool result = false;

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

                //result = DbConnection.Delete<List<ModelClass>>((List<ModelClass>)ModelItems, Transaction, CommandTimeout);
                result = DbConnection.Delete(ModelItems, Transaction, CommandTimeout);
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
        /// Deletes the item asynchronously.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemContribAsync(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemAsync()");

            bool _result = false;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
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
                _result = await DbConnection.DeleteAsync(ModelItem, Transaction, CommandTimeout);
                if (_result)
                {
                    _ModelItems.Remove(ModelItem);
                    _ModelItemsShadow.Remove(ModelItem);
                    _ModelItem = GetEmptyModelItem();
                    _ModelItemShadow = GetEmptyModelItem();
                }
                ER.Value = _result;
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
        /// Deletes the items asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemsContribAsync(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemsAsync()");
            bool result = false;

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
                result = await DbConnection.DeleteAsync(ModelItems, Transaction, CommandTimeout);
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
        /// Gets or sets the default SQL query.
        /// </summary>
        /// <value>
        /// The default SQL query.
        /// </value>
    }
}
