using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Passero.Framework.Base
{
    /// <summary>
    /// Punto di accesso unificato al DbContext Passero.
    /// Visibile su entrambi i target (net48 e net8.0).
    /// Internamente delega a:
    /// <list type="bullet">
    ///   <item>net48  → <see cref="PasseroEf6DbContext"/> (Entity Framework 6)</item>
    ///   <item>net8.0 → <c>PasseroEfCoreDbContext</c> (EF Core)</item>
    /// </list>
    /// Il codice chiamante usa sempre
    /// <c>new PasseroEfDbContext(connectionString, typeof(T))</c> senza alcun <c>#if</c>.
    /// </summary>
    public sealed class PasseroEfDbContext : IPasseroDbContext
    {
        private readonly IPasseroDbContext _inner;

        /// <summary>
        /// Crea un DbContext dalla connection string.
        /// Su net48 usa EF6; su net8.0 usa EF Core.
        /// </summary>
        public PasseroEfDbContext(string connectionString, params Type[] entityTypes)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _inner = PasseroEfDbContextFactory.Create(connectionString, entityTypes);
        }

        /// <summary>
        /// Crea un DbContext riusando una connessione ADO.NET già aperta.
        /// </summary>
        public PasseroEfDbContext(IDbConnection existingConnection, params Type[] entityTypes)
        {
            if (existingConnection == null)
                throw new ArgumentNullException(nameof(existingConnection));

            _inner = PasseroEfDbContextFactory.Create(existingConnection, entityTypes);
        }

        // ── IPasseroDbContext: delega completa a _inner ────────────────────────

        public IDbConnection Connection => _inner.Connection;
        public int SaveChanges() => _inner.SaveChanges();
        public Task<int> SaveChangesAsync() => _inner.SaveChangesAsync();
        public IQueryable<T> Set<T>() where T : class => _inner.Set<T>();
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query) where T : class
                                                                 => _inner.ToListAsync(query);
        public void Add<T>(T entity) where T : class => _inner.Add(entity);
        public void AddRange<T>(IEnumerable<T> e) where T : class => _inner.AddRange(e);
        public void Remove<T>(T entity) where T : class => _inner.Remove(entity);
        public void RemoveRange<T>(IEnumerable<T> e) where T : class => _inner.RemoveRange(e);
        public void MarkModified<T>(T entity) where T : class => _inner.MarkModified(entity);
        public int ExecuteSql(string sql, params object[] p) => _inner.ExecuteSql(sql, p);
        public Task<int> ExecuteSqlAsync(string sql, params object[] p)
                                                                 => _inner.ExecuteSqlAsync(sql, p);
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] p) where T : class
                                                                 => _inner.SqlQuery<T>(sql, p);
        public void EnsureConnectionOpen() => _inner.EnsureConnectionOpen();
        public void Dispose() => _inner.Dispose();
    }
}