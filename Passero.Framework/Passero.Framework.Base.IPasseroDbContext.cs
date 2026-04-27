using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Passero.Framework.Base
{
    public enum ORMType
    {
        EntityFramework,
        EntityFramework6,
        EntityFrameworkCore,
        Dapper
    }

    /// <summary>
    /// Stato di change tracking di un'entità, indipendente da EF6/EF Core.
    /// </summary>
    public enum PasseroEntityState
    {
        Detached = 0,
        Unchanged = 1,
        Added = 2,
        Deleted = 3,
        Modified = 4
    }

    /// <summary>
    /// Descrizione sintetica di un'entità tracciata nel change tracker.
    /// </summary>
    public sealed class PasseroEntityEntry
    {
        /// <summary>Istanza dell'entità tracciata.</summary>
        public object Entity { get; }

        /// <summary>Tipo dell'entità.</summary>
        public Type EntityType { get; }

        /// <summary>Stato corrente nel change tracker.</summary>
        public PasseroEntityState State { get; }

        public PasseroEntityEntry(object entity, Type entityType, PasseroEntityState state)
        {
            Entity = entity;
            EntityType = entityType;
            State = state;
        }
    }

    /// <summary>
    /// Astrazione comune del DbContext per EF6 (.NET 4.8) ed EF Core (.NET 8).
    /// </summary>
    public interface IPasseroDbContext : IDisposable
    {
        /// <summary>Salva le modifiche pendenti nel database.</summary>
        int SaveChanges();

        /// <summary>Salva le modifiche pendenti nel database in modo asincrono.</summary>
        Task<int> SaveChangesAsync();

        /// <summary>Restituisce il tipo di ORM usato internamente.</summary>
        ORMType ORMType { get; set; }

        /// <summary>Restituisce la connessione ADO.NET sottostante.</summary>
        IDbConnection DbConnection { get; }

        /// <summary>
        /// Restituisce il DbContext nativo attivo (esterno se impostato via <see cref="SetDbContext{TContext}"/>,
        /// altrimenti <c>this</c>).
        /// Assegnando questa property si ottiene lo stesso effetto di <see cref="SetDbContext{TContext}"/>.
        /// </summary>
        object DbContextInstance { get; set; }

        /// <summary>
        /// Restituisce il DbContext nativo come <typeparamref name="TContext"/>.
        /// Dopo una chiamata a <see cref="SetDbContext{TContext}"/> restituisce il contesto esterno.
        /// </summary>
        TContext GetDbContext<TContext>() where TContext : class;

        /// <summary>
        /// Speculare di <see cref="GetDbContext{TContext}"/>: imposta un DbContext EF nativo esterno
        /// già istanziato come contesto attivo.
        /// </summary>
        void SetDbContext<TContext>(TContext context) where TContext : class;

        /// <summary>Restituisce un <see cref="IQueryable{T}"/> per il tipo di entità specificato.</summary>
        IQueryable<T> Set<T>() where T : class;

        /// <summary>Materializza in modo asincrono un <see cref="IQueryable{T}"/> in una lista.</summary>
        Task<List<T>> ToListAsync<T>(IQueryable<T> query) where T : class;

        /// <summary>Aggiunge un'entità al contesto per l'inserimento.</summary>
        void Add<T>(T entity) where T : class;

        /// <summary>Aggiunge una sequenza di entità al contesto per l'inserimento.</summary>
        void AddRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>Rimuove un'entità dal contesto per la cancellazione.</summary>
        void Remove<T>(T entity) where T : class;

        /// <summary>Rimuove una sequenza di entità dal contesto per la cancellazione.</summary>
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>Marca un'entità come modificata.</summary>
        void MarkModified<T>(T entity) where T : class;

        /// <summary>Esegue un comando SQL non-query.</summary>
        int ExecuteSql(string sql, params object[] parameters);

        /// <summary>Esegue un comando SQL non-query in modo asincrono.</summary>
        Task<int> ExecuteSqlAsync(string sql, params object[] parameters);

        /// <summary>Esegue una query SQL raw che restituisce entità di tipo <typeparamref name="T"/>.</summary>
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class;

        /// <summary>Apre la connessione se non è già aperta.</summary>
        void EnsureConnectionOpen();

        // ── Transazioni ───────────────────────────────────────────────────────

        /// <summary>
        /// Avvia una transazione ADO.NET sulla connessione sottostante.
        /// Su EF Core/EF6 usa il meccanismo nativo del provider.
        /// </summary>
        IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Avvia una transazione in modo asincrono.
        /// </summary>
        Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Esegue il commit della transazione corrente.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Annulla la transazione corrente.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Transazione ADO.NET attiva, se presente. <c>null</c> se nessuna transazione è aperta.
        /// </summary>
        IDbTransaction CurrentTransaction { get; }

        // ── Change Tracking ───────────────────────────────────────────────────

        /// <summary>Restituisce lo stato corrente dell'entità nel change tracker.</summary>
        PasseroEntityState GetEntityState<T>(T entity) where T : class;

        /// <summary>Imposta esplicitamente lo stato dell'entità nel change tracker.</summary>
        void SetEntityState<T>(T entity, PasseroEntityState state) where T : class;

        /// <summary>Aggancia un'entità al contesto in stato Unchanged.</summary>
        void Attach<T>(T entity) where T : class;

        /// <summary>Scollega un'entità dal change tracker.</summary>
        void Detach<T>(T entity) where T : class;

        /// <summary>Ricarica i valori dell'entità dal database.</summary>
        Task ReloadAsync<T>(T entity) where T : class;

        /// <summary>Annulla le modifiche di un'entità riportandola allo stato originale.</summary>
        void DiscardChanges<T>(T entity) where T : class;

        /// <summary>Restituisce <c>true</c> se ci sono modifiche pendenti.</summary>
        bool HasChanges();

        /// <summary>Restituisce tutte le entità con modifiche pendenti.</summary>
        IReadOnlyList<PasseroEntityEntry> GetChangedEntities();

        /// <summary>
        /// Ripristina il contesto interno originale creato da <see cref="ORMContextFactory"/>.
        /// </summary>
        void ResetDbContext();
    }
}