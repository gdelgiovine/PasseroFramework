using System;
using System.Data;

namespace Passero.Framework.Base
{
    /// <summary>
    /// Factory pubblica che crea la corretta implementazione di <see cref="IPasseroDbContext"/>
    /// in base all'<see cref="ORMType"/> richiesto e al target framework corrente.
    /// <list type="bullet">
    ///   <item>
    ///     <see cref="ORMType.EntityFramework"/> → sceglie automaticamente EF6 su net48
    ///     o EF Core su net8.0+. <b>Uso consigliato</b> per codice multi-target.
    ///   </item>
    ///   <item>
    ///     <see cref="ORMType.EntityFramework6"/> → forza EF6 (solo net48, eccezione su net8.0+).
    ///   </item>
    ///   <item>
    ///     <see cref="ORMType.EntityFrameworkCore"/> → forza EF Core (solo net8.0+, eccezione su net48).
    ///   </item>
    ///   <item>
    ///     <see cref="ORMType.Dapper"/> → <see cref="DapperDbContext"/> su tutti i target.
    ///   </item>
    /// </list>
    /// </summary>
    public static class ORMContextFactory
    {
        /// <summary>
        /// Registra un tipo entità globalmente nei DbContext EF Core/EF6,
        /// eliminando la necessità di passare <c>entityTypes[]</c> esplicitamente a <see cref="Create"/>.
        /// Viene chiamato automaticamente dal costruttore statico di <see cref="ViewModel{T}"/>.
        /// Su Dapper è un no-op.
        /// </summary>
        /// <param name="entityType">Tipo entità da registrare (es. <c>typeof(Models.Author)</c>).</param>
        public static void RegisterEntity(Type entityType)
        {
            if (entityType == null) return;

#if NET5_0_OR_GREATER
            PasseroEfCoreDbContext.RegisterEntity(entityType);
#else
            PasseroEf6DbContext.RegisterEntity(entityType);
#endif
        }

        /// <summary>
        /// Registra un tipo entità tramite il parametro generico.
        /// Forma abbreviata di <see cref="RegisterEntity(Type)"/>.
        /// </summary>
        public static void RegisterEntity<T>() where T : class
            => RegisterEntity(typeof(T));

        /// <summary>
        /// Crea un <see cref="IPasseroDbContext"/> dalla connection string.
        /// </summary>
        /// <param name="ormType">ORM da usare.</param>
        /// <param name="connectionString">Connection string SQL Server.</param>
        /// <param name="entityTypes">Tipi di entità — richiesti per EF Core/EF6, ignorati da Dapper.</param>
        public static IPasseroDbContext Create(ORMType ormType, string connectionString, Type[] entityTypes = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            var resolvedType = ResolveORMType(ormType);

            switch (resolvedType)
            {
                case ORMType.Dapper:
                    var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                    return new DapperDbContext(conn, ownsConnection: true);

#if NET5_0_OR_GREATER
                case ORMType.EntityFrameworkCore:
                    var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder()
                        .UseSqlServerInternal(connectionString)
                        .Options;
                    return new PasseroEfCoreDbContext(options, entityTypes);
#else
                case ORMType.EntityFramework6:
                    return new PasseroEf6DbContext(connectionString, entityTypes);
#endif

                default:
                    throw new NotSupportedException(
                        $"ORMType '{ormType}' non è supportato sul target framework corrente.");
            }
        }

        /// <summary>
        /// Crea un <see cref="IPasseroDbContext"/> riusando una connessione ADO.NET già aperta.
        /// </summary>
        /// <param name="ormType">ORM da usare.</param>
        /// <param name="existingConnection">Connessione ADO.NET esistente (non verrà chiusa su Dispose).</param>
        /// <param name="entityTypes">Tipi di entità — richiesti per EF Core/EF6, ignorati da Dapper.</param>
        public static IPasseroDbContext Create(ORMType ormType, IDbConnection existingConnection, Type[] entityTypes = null)
        {
            if (existingConnection == null)
                throw new ArgumentNullException(nameof(existingConnection));

            var resolvedType = ResolveORMType(ormType);

            switch (resolvedType)
            {
                case ORMType.Dapper:
                    return new DapperDbContext(existingConnection, ownsConnection: false);

#if NET5_0_OR_GREATER
                case ORMType.EntityFrameworkCore:
                    var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder()
                        .UseSqlServerInternal((System.Data.Common.DbConnection)existingConnection)
                        .Options;
                    return new PasseroEfCoreDbContext(options, entityTypes);
#else
                case ORMType.EntityFramework6:
                    return new PasseroEf6DbContext(existingConnection, contextOwnsConnection: false, entityTypes: entityTypes);
#endif

                default:
                    throw new NotSupportedException(
                        $"ORMType '{ormType}' non è supportato sul target framework corrente.");
            }
        }

        // ── Logica di risoluzione ORMType ─────────────────────────────────────

        /// <summary>
        /// Risolve <see cref="ORMType.EntityFramework"/> nel tipo concreto corretto
        /// per il target framework in uso a compile-time.
        /// Gli altri valori vengono restituiti invariati, con validazione di compatibilità.
        /// </summary>
        private static ORMType ResolveORMType(ORMType requested)
        {
            switch (requested)
            {
                case ORMType.EntityFramework:
#if NET5_0_OR_GREATER
                    return ORMType.EntityFrameworkCore;
#else
                    return ORMType.EntityFramework6;
#endif

                case ORMType.EntityFrameworkCore:
#if NET5_0_OR_GREATER
                    return ORMType.EntityFrameworkCore;
#else
                    throw new NotSupportedException(
                        "ORMType.EntityFrameworkCore richiede .NET 5.0 o superiore. " +
                        "Usare ORMType.EntityFramework6 su .NET Framework 4.8, " +
                        "oppure ORMType.EntityFramework per la selezione automatica.");
#endif

                case ORMType.EntityFramework6:
#if NET5_0_OR_GREATER
                    throw new NotSupportedException(
                        "ORMType.EntityFramework6 è supportato solo su .NET Framework 4.8. " +
                        "Usare ORMType.EntityFrameworkCore su .NET 8, " +
                        "oppure ORMType.EntityFramework per la selezione automatica.");
#else
                    return ORMType.EntityFramework6;
#endif

                case ORMType.Dapper:
                    return ORMType.Dapper;

                default:
                    throw new ArgumentOutOfRangeException(nameof(requested),
                        $"ORMType '{requested}' non riconosciuto.");
            }
        }
    }

#if NET5_0_OR_GREATER
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