using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Passero.Framework.Base
{
    /// <summary>
    /// Implementazione di <see cref="IPasseroDbContext"/> basata su Dapper (ADO.NET puro).
    /// Non usa alcun ORM per il change tracking: le operazioni di scrittura
    /// delegano a Dapper.Contrib (Insert/Update/Delete) e le query raw a Dapper.
    /// </summary>
    public sealed class DapperDbContext : IPasseroDbContext
    {
        private readonly IDbConnection _connection;
        private readonly bool _ownsConnection;
        private IDbTransaction _currentTransaction;

        public ORMType ORMType { get; set; } = ORMType.Dapper;

        /// <param name="connection">Connessione ADO.NET da usare.</param>
        /// <param name="ownsConnection">Se <c>true</c>, la connessione viene chiusa e rilasciata su Dispose.</param>
        public DapperDbContext(IDbConnection connection, bool ownsConnection = false)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _ownsConnection = ownsConnection;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Dapper non ha un DbContext EF nativo: restituisce sempre <c>this</c>.
        /// Il <c>set</c> è no-op: non è possibile impostare un DbContext esterno su Dapper.
        /// </remarks>
        public object DbContextInstance
        {
            get => this;
            set { /* no-op: Dapper non ha un DbContext EF da sostituire */ }
        }

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">
        /// <see cref="SetDbContext{TContext}"/> non è supportato da <see cref="DapperDbContext"/>.
        /// </exception>
        public void SetDbContext<TContext>(TContext context) where TContext : class
            => throw new NotSupportedException(
                "SetDbContext non è supportato da DapperDbContext: Dapper non usa un DbContext EF nativo.");

        /// <inheritdoc/>
        public TContext GetDbContext<TContext>() where TContext : class
        {
            if (this is TContext ctx)
                return ctx;

            throw new InvalidCastException(
                $"Il DbContext sottostante è '{GetType().FullName}' " +
                $"e non può essere convertito in '{typeof(TContext).FullName}'.");
        }

        public IDbConnection DbConnection => _connection;

        public int SaveChanges() => 0;

        public Task<int> SaveChangesAsync() => Task.FromResult(0);

        public IQueryable<T> Set<T>() where T : class
            => throw new NotSupportedException(
                "IQueryable<T> non è supportato da DapperDbContext. Usare SqlQuery<T> o Repository<T>.");

        public Task<List<T>> ToListAsync<T>(IQueryable<T> query) where T : class
            => throw new NotSupportedException(
                "ToListAsync(IQueryable) non è supportato da DapperDbContext.");

        public void Add<T>(T entity) where T : class
            => _connection.Insert(entity, _currentTransaction);

        public void AddRange<T>(IEnumerable<T> entities) where T : class
            => _connection.Insert(entities, _currentTransaction);

        public void Remove<T>(T entity) where T : class
            => _connection.Delete(entity, _currentTransaction);

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
            => _connection.Delete(entities, _currentTransaction);

        public void MarkModified<T>(T entity) where T : class
            => _connection.Update(entity, _currentTransaction);

        public int ExecuteSql(string sql, params object[] parameters)
            => _connection.Execute(sql, parameters, _currentTransaction);

        public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
            => await _connection.ExecuteAsync(sql, parameters, _currentTransaction);

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
            => _connection.Query<T>(sql, parameters, _currentTransaction);

        public void EnsureConnectionOpen()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public void ResetDbContext() { }

        // ── Transazioni ───────────────────────────────────────────────────────

        /// <inheritdoc/>
        public IDbTransaction CurrentTransaction => _currentTransaction;

        /// <inheritdoc/>
        /// <remarks>Apre la connessione se non è già aperta, poi avvia la transazione ADO.NET.</remarks>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            EnsureConnectionOpen();
            _currentTransaction = _connection.BeginTransaction(isolationLevel);
            return _currentTransaction;
        }

        /// <inheritdoc/>
        /// <remarks>Su Dapper non esiste un'API async nativa: delega a <see cref="BeginTransaction"/> via Task.Run.</remarks>
        public Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            => Task.FromResult(BeginTransaction(isolationLevel));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Nessuna transazione attiva.</exception>
        public void CommitTransaction()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException(
                    "DapperDbContext.CommitTransaction: nessuna transazione attiva.");
            try
            {
                _currentTransaction.Commit();
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Nessuna transazione attiva.</exception>
        public void RollbackTransaction()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException(
                    "DapperDbContext.RollbackTransaction: nessuna transazione attiva.");
            try
            {
                _currentTransaction.Rollback();
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        // ── Change Tracking (no-op / not supported su Dapper) ────────────────

        /// <inheritdoc/>
        /// <remarks>Dapper non ha change tracker: restituisce sempre <see cref="PasseroEntityState.Detached"/>.</remarks>
        public PasseroEntityState GetEntityState<T>(T entity) where T : class
            => PasseroEntityState.Detached;

        /// <inheritdoc/>
        /// <remarks>No-op su Dapper.</remarks>
        public void SetEntityState<T>(T entity, PasseroEntityState state) where T : class { }

        /// <inheritdoc/>
        /// <remarks>No-op su Dapper.</remarks>
        public void Attach<T>(T entity) where T : class { }

        /// <inheritdoc/>
        /// <remarks>No-op su Dapper.</remarks>
        public void Detach<T>(T entity) where T : class { }

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Dapper non supporta il reload dal change tracker.</exception>
        public Task ReloadAsync<T>(T entity) where T : class
            => throw new NotSupportedException(
                "ReloadAsync non è supportato da DapperDbContext. Ricaricare l'entità manualmente tramite SqlQuery.");

        /// <inheritdoc/>
        /// <remarks>No-op su Dapper.</remarks>
        public void DiscardChanges<T>(T entity) where T : class { }

        /// <inheritdoc/>
        /// <remarks>Dapper non ha change tracker: restituisce sempre <c>false</c>.</remarks>
        public bool HasChanges() => false;

        /// <inheritdoc/>
        /// <remarks>Dapper non ha change tracker: restituisce sempre una lista vuota.</remarks>
        public IReadOnlyList<PasseroEntityEntry> GetChangedEntities()
            => Array.Empty<PasseroEntityEntry>();

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;

            if (_ownsConnection)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}