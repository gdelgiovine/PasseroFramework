using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
//using Passero.Framework.Base;
#nullable enable

namespace Passero.Framework
{
    public partial class Repository<ModelClass>
        where ModelClass : class
    {
        private string mSqlDeleteCommand;

        private void EnsureDeleteSqlCommandInitialized()
        {
            if (ProviderFeatures == null)
            {
                ProviderFeatures = DbConnection != null
                    ? ProviderFeaturesResolver.FromConnection(DbConnection)
                    : new ProviderFeatures();
            }

            if (string.IsNullOrWhiteSpace(mSqlDeleteCommand))
            {
                SqlDeleteCommand(true);
            }

            if (string.IsNullOrWhiteSpace(mSqlDeleteCommand))
            {
                throw new InvalidOperationException("Il comando SQL di delete non è stato inizializzato.");
            }
        }

        private void ValidateDeleteContext(ModelClass model)
        {
            if (model == null)
            {
                throw new InvalidOperationException("Nessun modello da eliminare. Specificare un Model o impostare il modello corrente.");
            }

            if (DbConnection == null)
            {
                throw new InvalidOperationException("La connessione al database non è stata inizializzata.");
            }

            if (EntityPrimaryKeys == null || EntityPrimaryKeys.Count == 0)
            {
                throw new InvalidOperationException("Nessuna primary key definita sul modello.");
            }
        }

        private DynamicParameters CreateDeleteParameters(ModelClass model)
        {
            var parameters = new DynamicParameters();

            for (int i = 0; i < EntityPrimaryKeys.Count; i++)
            {
                var prop = EntityPrimaryKeys[i];
                parameters.Add(prop.Name, prop.GetValue(model));
            }

            return parameters;
        }

        private bool SyncAfterDelete(ModelClass model)
        {
            bool removed = false;

            if (_ModelItems != null)
            {
                var existing = _ModelItems.FirstOrDefault(x => Utilities.ObjectsEquals(x, model));
                if (existing != null)
                {
                    _ModelItems.Remove(existing);
                    removed = true;
                }
            }

            if (_ModelItemsShadow != null)
            {
                var shadow = _ModelItemsShadow.FirstOrDefault(x => Utilities.ObjectsEquals(x, model));
                if (shadow != null)
                {
                    _ModelItemsShadow.Remove(shadow);
                }
            }

            if (_ModelItem != null && Utilities.ObjectsEquals(_ModelItem, model))
            {
                _ModelItem = GetEmptyModelItem();
            }

            if (_ModelItemShadow != null && Utilities.ObjectsEquals(_ModelItemShadow, model))
            {
                _ModelItemShadow = GetEmptyModelItem();
            }

            return removed;
        }

        private void SyncAfterDeleteBatch(IEnumerable<ModelClass> models)
        {
            if (models == null)
            {
                return;
            }

            foreach (var model in models)
            {
                SyncAfterDelete(model);
            }
        }

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

            try
            {
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

                ValidateDeleteContext(ModelItem);
                EnsureDeleteSqlCommandInitialized();

                var parameters = CreateDeleteParameters(ModelItem);
                int affected = DbConnection.Execute(mSqlDeleteCommand, parameters, Transaction, CommandTimeout, CommandType.Text);

                ER.Value = affected;

                if (affected > 0)
                {
                    SyncAfterDelete(ModelItem);
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = "Elemento eliminato correttamente.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga eliminata.";
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
        /// Deletes the item asynchronously using a parameterized SQL command that respects column name mappings.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemAsync(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemAsync()");

            try
            {
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

                ValidateDeleteContext(ModelItem);
                EnsureDeleteSqlCommandInitialized();

                var parameters = CreateDeleteParameters(ModelItem);
                int affected = await DbConnection.ExecuteAsync(mSqlDeleteCommand, parameters, Transaction, CommandTimeout, CommandType.Text);

                ER.Value = affected;

                if (affected > 0)
                {
                    SyncAfterDelete(ModelItem);
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = "Elemento eliminato correttamente.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga eliminata.";
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

                var itemsList = (ModelItems ?? Enumerable.Empty<ModelClass>()).Where(x => x != null).ToList();
                if (itemsList.Count == 0)
                {
                    throw new InvalidOperationException("Nessun elenco di elementi da eliminare.");
                }

                foreach (var model in itemsList)
                {
                    ValidateDeleteContext(model);
                }

                EnsureDeleteSqlCommandInitialized();

                foreach (var model in itemsList)
                {
                    var parameters = CreateDeleteParameters(model);
                    deletedCount += DbConnection.Execute(mSqlDeleteCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                }

                ER.Value = deletedCount;

                if (deletedCount > 0)
                {
                    SyncAfterDeleteBatch(itemsList);
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = $"Eliminati {deletedCount} elementi.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga eliminata.";
                }
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

                var itemsList = (ModelItems ?? Enumerable.Empty<ModelClass>()).Where(x => x != null).ToList();
                if (itemsList.Count == 0)
                {
                    throw new InvalidOperationException("Nessun elenco di elementi da eliminare.");
                }

                foreach (var model in itemsList)
                {
                    ValidateDeleteContext(model);
                }

                EnsureDeleteSqlCommandInitialized();

                foreach (var model in itemsList)
                {
                    var parameters = CreateDeleteParameters(model);
                    deletedCount += await DbConnection.ExecuteAsync(mSqlDeleteCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                }

                ER.Value = deletedCount;

                if (deletedCount > 0)
                {
                    SyncAfterDeleteBatch(itemsList);
                    ER.ResultCode = ExecutionResultCodes.Success;
                    ER.ResultMessage = $"Eliminati {deletedCount} elementi.";
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Warning;
                    ER.ResultMessage = "Nessuna riga eliminata.";
                }
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
        /// Compatibility wrapper for <see cref="DeleteItem(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="ModelItem">The model item to delete.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core delete flow.</returns>
        public ExecutionResult DeleteItemContrib(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return DeleteItem(ModelItem, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Compatibility wrapper for <see cref="DeleteItems(IEnumerable{ModelClass}, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="ModelItems">The model items to delete.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>The execution result from the core batch delete flow.</returns>
        public ExecutionResult DeleteItemsContrib(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return DeleteItems(ModelItems, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Compatibility wrapper for <see cref="DeleteItemAsync(ModelClass, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="ModelItem">The model item to delete.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemContribAsync(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return await DeleteItemAsync(ModelItem, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Compatibility wrapper for <see cref="DeleteItemsAsync(IEnumerable{ModelClass}, IDbTransaction, int?)"/>.
        /// </summary>
        /// <param name="ModelItems">The model items to delete.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemsContribAsync(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            return await DeleteItemsAsync(ModelItems, Transaction, CommandTimeout);
        }

        /// <summary>
        /// Gets or sets the default SQL query.
        /// </summary>
        /// <value>
        /// The default SQL query.
        /// </value>
    }
}
