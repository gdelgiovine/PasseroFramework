using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ef6 = System.Data.Entity;

namespace Passero.Framework.Base
{
    /// <summary>
    /// DbContext minimale per uso con EfRepository&lt;T&gt; su .NET Framework 4.8.
    /// Supporta un DbContext EF6 esterno tramite <see cref="SetDbContext{TContext}"/>.
    /// </summary>
    public class PasseroEf6DbContext : Ef6.DbContext, IPasseroDbContext
    {
        public ORMType ORMType { get; set; } = ORMType.EntityFramework6;
        private readonly Type[] _entityTypes;

        /// <summary>
        /// Transazione EF6 corrente.
        /// <see cref="DbContextTransaction"/> espone la transazione ADO.NET sottostante
        /// tramite <see cref="DbContextTransaction.UnderlyingTransaction"/>.
        /// </summary>
        private DbContextTransaction _currentEf6Transaction;
        /// <summary>
        /// DbContext EF6 esterno impostato tramite <see cref="SetDbContext{TContext}"/>.
        /// Se <c>null</c>, tutte le operazioni usano <c>this</c>.
        /// </summary>
        private Ef6.DbContext _externalContext;

        /// <summary>Restituisce il contesto attivo: esterno se impostato, altrimenti this.</summary>
        private Ef6.DbContext ActiveContext => _externalContext ?? this;

    

        // ✅ ConcurrentDictionary garantisce unicità e thread-safety senza LINQ
        private static readonly ConcurrentDictionary<Type, byte> _dynamicEntityTypes
            = new ConcurrentDictionary<Type, byte>();

        /// <summary>
        /// Registra un tipo entità in modo thread-safe.
        /// Idempotente: chiamate multiple con lo stesso tipo non causano duplicati.
        /// </summary>
        public static void RegisterEntity(Type entityType)
        {
            if (entityType != null)
                _dynamicEntityTypes.TryAdd(entityType, 0);
        }

        public PasseroEf6DbContext(string nameOrConnectionString, Type[] entityTypes = null)
            : base(nameOrConnectionString)
        {
            _entityTypes = entityTypes ?? Array.Empty<Type>();
        }

        public PasseroEf6DbContext(IDbConnection existingConnection, bool contextOwnsConnection = false, Type[] entityTypes = null)
            : base((DbConnection)existingConnection, contextOwnsConnection)
        {
            _entityTypes = entityTypes ?? Array.Empty<Type>();
        }

        protected override void OnModelCreating(Ef6.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<
                System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            // ✅ Unione di entità esplicite e dinamiche — tutte distinte
            var allTypes = (_entityTypes ?? Array.Empty<Type>())
                .Union(_dynamicEntityTypes.Keys)
                .Distinct()
                .ToList();

            foreach (var entityType in allTypes)
            {
                var schemaTableAttr = entityType
                    .GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
                var dapperTableAttr = entityType
                    .GetCustomAttribute<Dapper.Contrib.Extensions.TableAttribute>();

                string tableName = schemaTableAttr?.Name
                    ?? dapperTableAttr?.Name
                    ?? entityType.Name;

                // modelBuilder.Entity<TEntity>()
                var entityMethod = typeof(Ef6.DbModelBuilder)
                    .GetMethod(nameof(Ef6.DbModelBuilder.Entity))
                    .MakeGenericMethod(entityType);

                var entityConfig = entityMethod.Invoke(modelBuilder, null);

                // .ToTable("tableName")
                var toTableMethod = entityConfig.GetType()
                    .GetMethod("ToTable", new[] { typeof(string) });
                toTableMethod?.Invoke(entityConfig, new object[] { tableName });

                // ── Chiave primaria composta via [ExplicitKey] di Dapper ──────
                var explicitKeyProps = entityType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null)
                    .ToArray();

                if (explicitKeyProps.Length >= 1)
                {
                    var param = System.Linq.Expressions.Expression.Parameter(entityType, "e");
                    System.Linq.Expressions.LambdaExpression keyLambda;

                    if (explicitKeyProps.Length == 1)
                    {
                        // e => e.Prop1
                        var propExpr = System.Linq.Expressions.Expression.Property(param, explicitKeyProps[0]);
                        keyLambda = System.Linq.Expressions.Expression.Lambda(propExpr, param);
                    }
                    else
                    {
                        // e => new AnonymousKey { Item0 = e.Prop1, Item1 = e.Prop2, ... }
                        var anonCtor = GetAnonymousConstructor(
                            explicitKeyProps.Select(p => p.PropertyType).ToArray());

                        if (anonCtor.HasValue)
                        {
                            var memberBindings = explicitKeyProps
                                .Select(p => System.Linq.Expressions.Expression.Property(param, p))
                                .Cast<System.Linq.Expressions.Expression>()
                                .ToArray();

                            var newExpr = System.Linq.Expressions.Expression.New(
                                anonCtor.Value.Constructor,
                                memberBindings,
                                anonCtor.Value.Members);

                            keyLambda = System.Linq.Expressions.Expression.Lambda(newExpr, param);
                        }
                        else
                        {
                            // Fallback: EF6 rileverà [Key]+[Column(Order)] se presenti sul modello.
                            keyLambda = null;
                        }
                    }

                    if (keyLambda != null)
                    {
                        // HasKey<TKey>(Expression<Func<TEntity, TKey>>) è generico:
                        // occorre chiudere il tipo generico prima di Invoke.
                        var hasKeyMethodOpen = entityConfig.GetType()
                            .GetMethods()
                            .FirstOrDefault(m => m.Name == "HasKey"
                                                 && m.IsGenericMethodDefinition
                                                 && m.GetParameters().Length == 1
                                                 && m.GetParameters()[0].ParameterType.IsGenericType);

                        if (hasKeyMethodOpen != null)
                        {
                            // Il tipo della chiave è il tipo restituito dalla lambda:
                            // per chiave singola è il tipo della proprietà,
                            // per chiave composta è il tipo anonimo sintetico.
                            Type keyType = keyLambda.ReturnType;
                            var hasKeyMethodClosed = hasKeyMethodOpen.MakeGenericMethod(keyType);
                            hasKeyMethodClosed.Invoke(entityConfig, new object[] { keyLambda });
                        }
                    }
                }

                // ── Proprietà da ignorare ([Computed] o [NotMapped]) ──────────
                var ignoreMethod = entityConfig.GetType()
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "Ignore"
                                        && m.GetParameters().Length == 1
                                        && m.GetParameters()[0].ParameterType == typeof(string));

                if (ignoreMethod != null)
                {
                    foreach (var prop in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        bool isComputed = prop.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() != null;
                        bool isNotMapped = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() != null;

                        if (isComputed || isNotMapped)
                            ignoreMethod.Invoke(entityConfig, new object[] { prop.Name });
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Crea a runtime un tipo anonimo sintetico con le proprietà specificate,
        /// per costruire l'Expression Tree della chiave composta richiesta da EF6 HasKey.
        /// </summary>
        private static (System.Reflection.ConstructorInfo Constructor, System.Reflection.MemberInfo[] Members)?
            GetAnonymousConstructor(Type[] propertyTypes)
        {
            try
            {
                var assemblyName = new System.Reflection.AssemblyName("PasseroDynamicKey");
                var assemblyBuilder = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
                    assemblyName, System.Reflection.Emit.AssemblyBuilderAccess.Run);
                var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
                var typeBuilder = moduleBuilder.DefineType(
                    $"AnonymousKey_{Guid.NewGuid():N}",
                    System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Class);

                var fields = new System.Reflection.Emit.FieldBuilder[propertyTypes.Length];
                for (int i = 0; i < propertyTypes.Length; i++)
                {
                    fields[i] = typeBuilder.DefineField($"Item{i}", propertyTypes[i],
                        System.Reflection.FieldAttributes.Public);
                }

                var ctorBuilder = typeBuilder.DefineConstructor(
                    System.Reflection.MethodAttributes.Public,
                    System.Reflection.CallingConventions.Standard,
                    propertyTypes);

                var il = ctorBuilder.GetILGenerator();
                il.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                il.Emit(System.Reflection.Emit.OpCodes.Call,
                    typeof(object).GetConstructor(Type.EmptyTypes));
                for (int i = 0; i < propertyTypes.Length; i++)
                {
                    il.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                    il.Emit(System.Reflection.Emit.OpCodes.Ldarg, i + 1);
                    il.Emit(System.Reflection.Emit.OpCodes.Stfld, fields[i]);
                }
                il.Emit(System.Reflection.Emit.OpCodes.Ret);

                var builtType = typeBuilder.CreateType();
                var ctor = builtType.GetConstructors()[0];
                var members = fields.Select(f => builtType.GetField(f.Name)).Cast<System.Reflection.MemberInfo>().ToArray();

                return (ctor, members);
            }
            catch
            {
                return null;
            }
        }

        // ── DbContextInstance (get/set) + SetDbContext ────────────────────────

        /// <inheritdoc/>
        public object DbContextInstance
        {
            get => (object)_externalContext ?? this;
            set => _externalContext = value as Ef6.DbContext;
        }

        /// <inheritdoc/>
        public void SetDbContext<TContext>(TContext context) where TContext : class
        {
            if (context == null)
            {
                _externalContext = null;
                return;
            }

            if (context is Ef6.DbContext ef6Ctx)
                _externalContext = ef6Ctx;
            else
                throw new ArgumentException(
                    $"Il tipo '{typeof(TContext).FullName}' non è un System.Data.Entity.DbContext valido per EF6.",
                    nameof(context));
        }

        /// <inheritdoc/>
        public TContext GetDbContext<TContext>() where TContext : class
        {
            var target = (object)_externalContext ?? this;

            if (target is TContext ctx)
                return ctx;

            throw new InvalidCastException(
                $"Il DbContext attivo è '{target.GetType().FullName}' " +
                $"e non può essere convertito in '{typeof(TContext).FullName}'.");
        }

        // ── Operazioni delegate ad ActiveContext ──────────────────────────────

        public IDbConnection DbConnection => ActiveContext.Database.Connection;

        IQueryable<T> IPasseroDbContext.Set<T>() => ActiveContext.Set<T>();

        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query) where T : class
            => await Ef6.QueryableExtensions.ToListAsync(query);

        Task<int> IPasseroDbContext.SaveChangesAsync() => ActiveContext.SaveChangesAsync();

        void IPasseroDbContext.Add<T>(T entity) => ActiveContext.Set<T>().Add(entity);
        void IPasseroDbContext.AddRange<T>(IEnumerable<T> entities) => ActiveContext.Set<T>().AddRange(entities);
        void IPasseroDbContext.Remove<T>(T entity) => ActiveContext.Set<T>().Remove(entity);
        void IPasseroDbContext.RemoveRange<T>(IEnumerable<T> entities) => ActiveContext.Set<T>().RemoveRange(entities);

        void IPasseroDbContext.MarkModified<T>(T entity)
            => ActiveContext.Entry(entity).State = Ef6.EntityState.Modified;

        public int ExecuteSql(string sql, params object[] parameters)
            => ActiveContext.Database.ExecuteSqlCommand(sql, parameters);

        public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
            => await ActiveContext.Database.ExecuteSqlCommandAsync(sql, parameters);

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
            => ActiveContext.Database.SqlQuery<T>(sql, parameters).AsEnumerable();

        void IPasseroDbContext.EnsureConnectionOpen()
        {
            if (ActiveContext.Database.Connection.State != ConnectionState.Open)
                ActiveContext.Database.Connection.Open();
        }

        public void ResetDbContext() => _externalContext = null;

        // ── Transazioni ───────────────────────────────────────────────────────

        /// <inheritdoc/>
        /// <remarks>
        /// Restituisce la transazione ADO.NET sottostante a quella EF6 corrente,
        /// oppure <c>null</c> se nessuna transazione è aperta.
        /// </remarks>
        public IDbTransaction CurrentTransaction
            => _currentEf6Transaction?.UnderlyingTransaction;

        /// <inheritdoc/>
        /// <remarks>
        /// EF6 avvia la transazione tramite <c>Database.BeginTransaction()</c> che coordina
        /// internamente il change tracker. La <see cref="Ef6.Database.DbContextTransaction"/>
        /// espone la transazione ADO.NET sottostante tramite <c>UnderlyingTransaction</c>.
        /// </remarks>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_currentEf6Transaction != null)
                throw new InvalidOperationException(
                    "PasseroEf6DbContext.BeginTransaction: una transazione è già attiva. " +
                    "Eseguire Commit o Rollback prima di avviarne una nuova.");

            _currentEf6Transaction = ActiveContext.Database.BeginTransaction(isolationLevel);
            return _currentEf6Transaction.UnderlyingTransaction;
        }

        /// <inheritdoc/>
        /// <remarks>EF6 non ha API async native per BeginTransaction: delega alla versione sincrona.</remarks>
        public Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            => Task.FromResult(BeginTransaction(isolationLevel));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Nessuna transazione attiva.</exception>
        public void CommitTransaction()
        {
            if (_currentEf6Transaction == null)
                throw new InvalidOperationException(
                    "PasseroEf6DbContext.CommitTransaction: nessuna transazione attiva.");
            try
            {
                _currentEf6Transaction.Commit();
            }
            finally
            {
                _currentEf6Transaction.Dispose();
                _currentEf6Transaction = null;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Nessuna transazione attiva.</exception>
        public void RollbackTransaction()
        {
            if (_currentEf6Transaction == null)
                throw new InvalidOperationException(
                    "PasseroEf6DbContext.RollbackTransaction: nessuna transazione attiva.");
            try
            {
                _currentEf6Transaction.Rollback();
            }
            finally
            {
                _currentEf6Transaction.Dispose();
                _currentEf6Transaction = null;
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
            => ActiveContext.Entry(entity).State = Ef6.EntityState.Detached;

        public async Task ReloadAsync<T>(T entity) where T : class
            => await ActiveContext.Entry(entity).ReloadAsync();

        public void DiscardChanges<T>(T entity) where T : class
        {
            var entry = ActiveContext.Entry(entity);
            if (entry.State == Ef6.EntityState.Modified)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = Ef6.EntityState.Unchanged;
            }
            else if (entry.State == Ef6.EntityState.Added)
            {
                entry.State = Ef6.EntityState.Detached;
            }
        }

        public bool HasChanges()
            => ActiveContext.ChangeTracker.Entries()
                .Any(e => e.State == Ef6.EntityState.Added
                        || e.State == Ef6.EntityState.Modified
                        || e.State == Ef6.EntityState.Deleted);

        public IReadOnlyList<PasseroEntityEntry> GetChangedEntities()
            => ActiveContext.ChangeTracker.Entries()
                .Where(e => e.State == Ef6.EntityState.Added
                         || e.State == Ef6.EntityState.Modified
                         || e.State == Ef6.EntityState.Deleted)
                .Select(e => new PasseroEntityEntry(e.Entity, e.Entity.GetType(), MapState(e.State)))
                .ToList()
                .AsReadOnly();

        // ── Helpers ───────────────────────────────────────────────────────────

        private static PasseroEntityState MapState(Ef6.EntityState state) => state switch
        {
            Ef6.EntityState.Added => PasseroEntityState.Added,
            Ef6.EntityState.Modified => PasseroEntityState.Modified,
            Ef6.EntityState.Deleted => PasseroEntityState.Deleted,
            Ef6.EntityState.Unchanged => PasseroEntityState.Unchanged,
            _ => PasseroEntityState.Detached
        };

        private static Ef6.EntityState MapState(PasseroEntityState state) => state switch
        {
            PasseroEntityState.Added => Ef6.EntityState.Added,
            PasseroEntityState.Modified => Ef6.EntityState.Modified,
            PasseroEntityState.Deleted => Ef6.EntityState.Deleted,
            PasseroEntityState.Unchanged => Ef6.EntityState.Unchanged,
            _ => Ef6.EntityState.Detached
        };
    }
}