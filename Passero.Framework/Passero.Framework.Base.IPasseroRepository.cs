using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Passero.Framework.Base
{
    /// <summary>
    /// Interfaccia comune per i repository Passero.
    /// Implementata da <see cref="Repository{ModelClass}"/> (Dapper)
    /// e da <see cref="EfRepository{ModelClass}"/> (Entity Framework Core).
    /// Il <see cref="ViewModel{ModelClass}"/> dipende solo da questa interfaccia.
    /// </summary>
    /// <typeparam name="ModelClass">Tipo del modello POCO.</typeparam>
    public interface IPasseroRepository<ModelClass> : IDisposable
        where ModelClass : class
    {
        public ViewModel<ModelClass> ViewModel { get; set; }
        // ── Identità ──────────────────────────────────────────────────────────
        string Name { get; set; }
        ExecutionResult LastExecutionResult { get; set; }

        // ── Connessione ───────────────────────────────────────────────────────
        IDbConnection DbConnection { get; set; }
        IDbTransaction DbTransaction { get; set; }
        int DbCommandTimeout { get; set; }

        // ── Stato ─────────────────────────────────────────────────────────────
        ModelClass ModelItem { get; set; }
        IList<ModelClass> ModelItems { get; set; }
        ModelClass ModelItemShadow { get; set; }
        IList<ModelClass> ModelItemsShadow { get; set; }
        int CurrentModelItemIndex { get; set; }
        int AddNewCurrentModelItemIndex { get; set; }
        bool AddNewState { get; set; }

        // ── Shadow ────────────────────────────────────────────────────────────
        void SetModelItemShadow();
        void SetModelItemsShadow();

        // ── Navigazione ───────────────────────────────────────────────────────
        ExecutionResult MoveFirstItem();
        ExecutionResult MoveLastItem();
        ExecutionResult MovePreviousItem();
        ExecutionResult MoveNextItem();
        ExecutionResult MoveAtItem(int index);
        ExecutionResult<ModelClass> GetModelItemsAt(int index);

        // ── Clone / empty ─────────────────────────────────────────────────────
        ModelClass GetEmptyModelItem();
        bool IsModelDataChanged(ModelClass modelShadow = null);

        // ── Notifiche errori ──────────────────────────────────────────────────
        ErrorNotificationModes ErrorNotificationMode { get; set; }
        ErrorNotificationMessageBox ErrorNotificationMessageBox { get; set; }

        // ── Metadati DB ───────────────────────────────────────────────────────
        string DefaultSQLQuery { get; set; }
        DynamicParameters DefaultSQLQueryParameters { get; set; }
        DynamicParameters Parameters { get; set; }
        string SQLQuery { get; set; }
        string GetTableName();

        /// <summary>
        /// Imposta la query SQL corrente e i parametri associati.
        /// Usato da <see cref="Controls.QBEForm{T}"/> per impostare il filtro
        /// risultante prima del reload del ViewModel chiamante.
        /// </summary>
        void SetSQLQuery(string sqlQuery, DynamicParameters parameters);

        // ── READ ──────────────────────────────────────────────────────────────
        ExecutionResult<IList<ModelClass>> GetAllItems(
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null);

        Task<ExecutionResult<IList<ModelClass>>> GetAllItemsAsync(
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null);

        ExecutionResult<IList<ModelClass>> GetItems(
            string query, object parameters = null, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null);

        Task<ExecutionResult<IList<ModelClass>>> GetItemsAsync(
            string query, object parameters = null, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null);

        ExecutionResult<ModelClass> GetItem(
            string query, object parameters = null, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null);

        Task<ExecutionResult<ModelClass>> GetItemAsync(
            string query, object parameters = null, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null);

        ExecutionResult ReloadItems(bool buffered = true, int? commandTimeout = null);

        // ── WRITE ─────────────────────────────────────────────────────────────
        ExecutionResult InsertItem(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> InsertItemAsync(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null);

        ExecutionResult InsertItems(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> InsertItemsAsync(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null);

        ExecutionResult UpdateItem(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> UpdateItemAsync(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null);

        ExecutionResult UpdateItemEx(
            ModelClass modelItem = null, ModelClass modelItemShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> UpdateItemExAsync(
            ModelClass modelItem = null, ModelClass modelItemShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null);

        ExecutionResult UpdateItems(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> UpdateItemsAsync(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null);

        // ── AGGIUNTI: mancanti dalla versione precedente ───────────────────────
        /// <summary>
        /// Aggiorna una lista di item confrontando con la lista shadow, aggiornando solo i cambiati.
        /// </summary>
        ExecutionResult UpdateItemsEx(
            IEnumerable<ModelClass> items = null, IEnumerable<ModelClass> itemsShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Versione asincrona di <see cref="UpdateItemsEx"/>.
        /// </summary>
        Task<ExecutionResult> UpdateItemsExAsync(
            IEnumerable<ModelClass> items = null, IEnumerable<ModelClass> itemsShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null);

        // ── DELETE ────────────────────────────────────────────────────────────
        ExecutionResult DeleteItem(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> DeleteItemAsync(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null);

        ExecutionResult DeleteItems(
            IEnumerable<ModelClass> items, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<ExecutionResult> DeleteItemsAsync(
            IEnumerable<ModelClass> items, IDbTransaction transaction = null, int? commandTimeout = null);
    }
}