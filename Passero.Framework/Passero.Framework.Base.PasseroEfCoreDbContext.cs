#if NET5_0_OR_GREATER
using EfCore = Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Passero.Framework.Base
{
    /// <summary>
    /// Implementazione EF Core di <see cref="IPasseroDbContext"/>.
    /// Supporta un DbContext EF Core esterno tramite <see cref="SetDbContext{TContext}"/>.
    /// </summary>
    class PasseroEfCoreDbContext : EfCore.DbContext, IPasseroDbContext
    {
        private readonly Type[] _entityTypes;

        // ✅ ConcurrentDictionary garantisce unicità e thread-safety senza LINQ
        private static readonly ConcurrentDictionary<Type, byte> _dynamicEntityTypes
            = new ConcurrentDictionary<Type, byte>();

        public ORMType ORMType { get; set; } = ORMType.EntityFrameworkCore;

        private EfCore.DbContext _externalContext;
        private EfCore.DbContext ActiveContext => _externalContext ?? this;

        /// <summary>
        /// Transazione EF Core corrente.
        /// <see cref="IDbContextTransaction"/> espone la transazione ADO.NET sottostante
        /// tramite <see cref="IDbContextTransaction.GetDbTransaction"/>.
        /// </summary>
        private IDbContextTransaction _currentEfCoreTransaction;

        internal PasseroEfCoreDbContext(EfCore.DbContextOptions options, Type[] entityTypes)
            : base(options)
        {
            _entityTypes = entityTypes ?? Array.Empty<Type>();
        }

        /// <summary>
        /// Registra un tipo entità in modo thread-safe.
        /// Idempotente: chiamate multiple con lo stesso tipo non causano duplicati.
        /// </summary>
        public static void RegisterEntity(Type entityType)
        {
            if (entityType != null)
                _dynamicEntityTypes.TryAdd(entityType, 0);
        }

        // ── DbContextInstance (get/set) + SetDbContext ────────────────────────

        public object DbContextInstance
        {
            get => (object)_externalContext ?? this;
            set => _externalContext = value as EfCore.DbContext;
        }

        public void SetDbContext<TContext>(TContext context) where TContext : class
        {
            if (context == null)
            {
                _externalContext = null;
                return;
            }

            if (context is EfCore.DbContext efCoreCtx)
                _externalContext = efCoreCtx;
            else
                throw new ArgumentException(
                    $"Il tipo '{typeof(TContext).FullName}' non è un Microsoft.EntityFrameworkCore.DbContext valido per EF Core.",
                    nameof(context));
        }

        public TContext GetDbContext<TContext>() where TContext : class
        {
            var target = (object)_externalContext ?? this;

            if (target is TContext ctx)
                return ctx;

            throw new InvalidCastException(
                $"Il DbContext attivo è '{target.GetType().FullName}' " +
                $"e non può essere convertito in '{typeof(TContext).FullName}'.");
        }

        protected override void OnModelCreating(EfCore.ModelBuilder modelBuilder)
        {
            // ✅ Unione di entità esplicite e dinamiche — tutte distinte
            var allTypes = (_entityTypes ?? Array.Empty<Type>())
                .Union(_dynamicEntityTypes.Keys)
                .Distinct()
                .ToList();

            foreach (var entityType in allTypes)
            {
                var entityBuilder = modelBuilder.Entity(entityType);

                // ── Mapping nome tabella ──────────────────────────────────────
                var schemaTableAttr = entityType
                    .GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
                var dapperTableAttr = entityType
                    .GetCustomAttribute<Dapper.Contrib.Extensions.TableAttribute>();

                string tableName = schemaTableAttr?.Name
                    ?? dapperTableAttr?.Name
                    ?? entityType.Name;

                entityBuilder.ToTable(tableName);

                // ── Chiave primaria ───────────────────────────────────────────
                var keyProps = entityType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p =>
                        p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null ||
                        p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null)
                    .Select(p => p.Name)
                    .ToArray();

                if (keyProps.Length >= 1)
                    entityBuilder.HasKey(keyProps);

                // ── Proprietà da ignorare ─────────────────────────────────────
                foreach (var prop in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    bool isComputed = prop.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() != null;
                    bool isNotMapped = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() != null;

                    if (isComputed || isNotMapped)
                        entityBuilder.Ignore(prop.Name);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        // ── Operazioni delegate ad ActiveContext ──────────────────────────────

        public IDbConnection DbConnection => ActiveContext.Database.GetDbConnection();

        IQueryable<T> IPasseroDbContext.Set<T>() => ActiveContext.Set<T>();

        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query) where T : class
            => await EfCore.EntityFrameworkQueryableExtensions.ToListAsync(query);

        Task<int> IPasseroDbContext.SaveChangesAsync()
            => ActiveContext.SaveChangesAsync(System.Threading.CancellationToken.None);

        void IPasseroDbContext.Add<T>(T entity) => ActiveContext.Set<T>().Add(entity);
        void IPasseroDbContext.AddRange<T>(IEnumerable<T> entities) => ActiveContext.AddRange(entities);
        void IPasseroDbContext.Remove<T>(T entity) => ActiveContext.Set<T>().Remove(entity);
        void IPasseroDbContext.RemoveRange<T>(IEnumerable<T> entities) => ActiveContext.RemoveRange(entities);

        void IPasseroDbContext.MarkModified<T>(T entity)
            => ActiveContext.Entry(entity).State = EfCore.EntityState.Modified;

        public int ExecuteSql(string sql, params object[] parameters)
            => ActiveContext.Database.ExecuteSqlRaw(sql, parameters);

        public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
            => await ActiveContext.Database.ExecuteSqlRawAsync(sql, parameters);

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
            => RelationalQueryableExtensions.FromSqlRaw(ActiveContext.Set<T>(), sql, parameters).AsEnumerable();

        void IPasseroDbContext.EnsureConnectionOpen() => ActiveContext.Database.OpenConnection();

        public void ResetDbContext() => _externalContext = null;

        // ── Transazioni ───────────────────────────────────────────────────────

        /// <inheritdoc/>
        /// <remarks>
        /// Restituisce la transazione ADO.NET sottostante a quella EF Core corrente,
        /// oppure <c>null</c> se nessuna transazione è aperta.
        /// </remarks>
        public IDbTransaction CurrentTransaction
            => _currentEfCoreTransaction?.GetDbTransaction();

        /// <inheritdoc/>
        /// <remarks>
        /// EF Core avvia la transazione tramite <c>Database.BeginTransaction()</c>.
        /// A differenza di EF6, EF Core restituisce <see cref="IDbContextTransaction"/>
        /// che espone la transazione ADO.NET tramite <c>GetDbTransaction()</c>.
        /// </remarks>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_currentEfCoreTransaction != null)
                throw new InvalidOperationException(
                    "PasseroEfCoreDbContext.BeginTransaction: una transazione è già attiva. " +
                    "Eseguire Commit o Rollback prima di avviarne una nuova.");

            _currentEfCoreTransaction = ActiveContext.Database.BeginTransaction(isolationLevel);
            return _currentEfCoreTransaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// EF Core supporta nativamente le API async per le transazioni,
        /// a differenza di EF6 che delega alla versione sincrona.
        /// </remarks>
        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_currentEfCoreTransaction != null)
                throw new InvalidOperationException(
                    "PasseroEfCoreDbContext.BeginTransactionAsync: una transazione è già attiva. " +
                    "Eseguire Commit o Rollback prima di avviarne una nuova.");

            _currentEfCoreTransaction = await ActiveContext.Database.BeginTransactionAsync(isolationLevel);
            return _currentEfCoreTransaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Nessuna transazione attiva.</exception>
        public void CommitTransaction()
        {
            if (_currentEfCoreTransaction == null)
                throw new InvalidOperationException(
                    "PasseroEfCoreDbContext.CommitTransaction: nessuna transazione attiva.");
            try
            {
                _currentEfCoreTransaction.Commit();
            }
            finally
            {
                _currentEfCoreTransaction.Dispose();
                _currentEfCoreTransaction = null;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Nessuna transazione attiva.</exception>
        public void RollbackTransaction()
        {
            if (_currentEfCoreTransaction == null)
                throw new InvalidOperationException(
                    "PasseroEfCoreDbContext.RollbackTransaction: nessuna transazione attiva.");
            try
            {
                _currentEfCoreTransaction.Rollback();
            }
            finally
            {
                _currentEfCoreTransaction.Dispose();
                _currentEfCoreTransaction = null;
            }
        }

        // ── Change Tracking ───────────────────────────────────────────────────

        public PasseroEntityState GetEntityState<T>(T entity) where T : class
            => MapState(ActiveContext.Entry(entity).State);

        public void SetEntityState<T>(T entity, PasseroEntityState state) where T : class
            => ActiveContext.Entry(entity).State = MapState(state);

        public void Attach<T>(T entity) where T : class
            => ActiveContext.Set<T>().Attach(entity);

        public void Detach<T>(T entity) where T : class
            => ActiveContext.Entry(entity).State = EfCore.EntityState.Detached;

        public async Task ReloadAsync<T>(T entity) where T : class
            => await ActiveContext.Entry(entity).ReloadAsync();

        public void DiscardChanges<T>(T entity) where T : class
        {
            var entry = ActiveContext.Entry(entity);
            if (entry.State == EfCore.EntityState.Modified)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EfCore.EntityState.Unchanged;
            }
            else if (entry.State == EfCore.EntityState.Added)
            {
                entry.State = EfCore.EntityState.Detached;
            }
        }

        public bool HasChanges()
            => ActiveContext.ChangeTracker.Entries()
                .Any(e => e.State == EfCore.EntityState.Added
                        || e.State == EfCore.EntityState.Modified
                        || e.State == EfCore.EntityState.Deleted);

        public IReadOnlyList<PasseroEntityEntry> GetChangedEntities()
            => ActiveContext.ChangeTracker.Entries()
                .Where(e => e.State == EfCore.EntityState.Added
                         || e.State == EfCore.EntityState.Modified
                         || e.State == EfCore.EntityState.Deleted)
                .Select(e => new PasseroEntityEntry(e.Entity, e.Entity.GetType(), MapState(e.State)))
                .ToList()
                .AsReadOnly();

        // ── Helpers ───────────────────────────────────────────────────────────

        private static PasseroEntityState MapState(EfCore.EntityState state) => state switch
        {
            EfCore.EntityState.Added => PasseroEntityState.Added,
            EfCore.EntityState.Modified => PasseroEntityState.Modified,
            EfCore.EntityState.Deleted => PasseroEntityState.Deleted,
            EfCore.EntityState.Unchanged => PasseroEntityState.Unchanged,
            _ => PasseroEntityState.Detached
        };

        private static EfCore.EntityState MapState(PasseroEntityState state) => state switch
        {
            PasseroEntityState.Added => EfCore.EntityState.Added,
            PasseroEntityState.Modified => EfCore.EntityState.Modified,
            PasseroEntityState.Deleted => EfCore.EntityState.Deleted,
            PasseroEntityState.Unchanged => EfCore.EntityState.Unchanged,
            _ => EfCore.EntityState.Detached
        };
    }
}
#endif