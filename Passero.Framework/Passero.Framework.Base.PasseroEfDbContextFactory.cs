using System;
using System.Data;

namespace Passero.Framework.Base
{
    /// <summary>
    /// Factory pubblica che crea la corretta implementazione di <see cref="IPasseroDbContext"/>
    /// in base al target framework corrente: EF6 su net48, EF Core su net8.0+.
    /// È l'unico punto di creazione da usare nel codice chiamante.
    /// </summary>
    public static class PasseroEfDbContextFactory
    {
        /// <summary>
        /// Crea un <see cref="IPasseroDbContext"/> dalla connection string.
        /// Su net48 usa EF6; su net8.0+ usa EF Core.
        /// </summary>
        public static IPasseroDbContext Create(string connectionString, params Type[] entityTypes)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

#if NET5_0_OR_GREATER
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder()
                .UseSqlServerInternal(connectionString)
                .Options;
            return new PasseroEfCoreDbContext(options, entityTypes);
#else
            return new PasseroEf6DbContext(connectionString);
#endif
        }

        /// <summary>
        /// Crea un <see cref="IPasseroDbContext"/> riusando una connessione ADO.NET già aperta.
        /// Su net48 usa EF6; su net8.0+ usa EF Core.
        /// </summary>
        public static IPasseroDbContext Create(IDbConnection existingConnection, params Type[] entityTypes)
        {
            if (existingConnection == null)
                throw new ArgumentNullException(nameof(existingConnection));

#if NET5_0_OR_GREATER
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder()
                .UseSqlServerInternal((System.Data.Common.DbConnection)existingConnection)
                .Options;
            return new PasseroEfCoreDbContext(options, entityTypes);
#else
            return new PasseroEf6DbContext(existingConnection, contextOwnsConnection: false);
#endif
        }
    }

#if NET5_0_OR_GREATER
    /// <summary>
    /// Isola la chiamata a UseSqlServer per evitare ambiguità di overload
    /// con Microsoft.Data.SqlClient durante la risoluzione del compilatore.
    /// </summary>
    internal static class EfCoreOptionsBuilderExtensions
    {
        internal static Microsoft.EntityFrameworkCore.DbContextOptionsBuilder UseSqlServerInternal(
            this Microsoft.EntityFrameworkCore.DbContextOptionsBuilder builder,
            string connectionString)
            => Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions
                .UseSqlServer(builder, connectionString);

        internal static Microsoft.EntityFrameworkCore.DbContextOptionsBuilder UseSqlServerInternal(
            this Microsoft.EntityFrameworkCore.DbContextOptionsBuilder builder,
            System.Data.Common.DbConnection connection)
            => Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions
                .UseSqlServer(builder, connection);
    }
#endif
}