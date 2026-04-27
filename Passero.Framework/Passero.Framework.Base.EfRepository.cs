using Dapper;
#if   NET48
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Passero.Framework.Base;

namespace Passero.Framework
{
    /// <summary>
    /// Repository generico basato su Entity Framework Core.
    /// Disponibile solo su .NET 5+. Dipende da <see cref="IPasseroDbContext"/>
    /// per intercambiabilità con <see cref="Ef6Repository{ModelClass}"/> su .NET 4.8.
    /// </summary>
    public class EfRepository<ModelClass> : IPasseroRepository<ModelClass>
        where ModelClass : class
    {
        private const string _mClassName = "Passero.Framework.EfRepository";
        private readonly IPasseroDbContext _dbContext;
        private bool _disposed;

        private static readonly ConcurrentDictionary<Type, string> _tableNameCache = new();
        // La chiave è "FullName|ORMType" per differenziare EF6 da EF Core per lo stesso tipo.
        private static readonly ConcurrentDictionary<string, string> _selectQueryCache = new();

        // ── IPasseroRepository: identità ─────────────────────────────────────
        public string Name { get; set; } = $"EfRepository<{typeof(ModelClass).FullName}>";
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult(_mClassName);

        // ── IPasseroRepository: connessione ───────────────────────────────────
        public IDbConnection DbConnection
        {
            get => _dbContext.DbConnection;
            set => throw new NotSupportedException(
                "DbConnection non può essere impostata direttamente su EfRepository: " +
                "la connessione è gestita da IPasseroDbContext.");
        }

        public IDbTransaction DbTransaction { get; set; }
        public int DbCommandTimeout { get; set; } = 30;

        // ── IPasseroRepository: stato ─────────────────────────────────────────
        public IList<ModelClass> ModelItems { get; set; } = new List<ModelClass>();
        public ModelClass ModelItem { get; set; }
        public ModelClass ModelItemShadow { get; set; }
        public IList<ModelClass> ModelItemsShadow { get; set; } = new List<ModelClass>();
        public bool AddNewState { get; set; }
        public int CurrentModelItemIndex { get; set; } = -1;
        public int AddNewCurrentModelItemIndex { get; set; } = -1;

        // ── IPasseroRepository: notifiche errori ──────────────────────────────
        public ErrorNotificationModes ErrorNotificationMode { get; set; } = ErrorNotificationModes.ThrowException;
        public ErrorNotificationMessageBox ErrorNotificationMessageBox { get; set; }

        // ── IPasseroRepository: metadati DB ───────────────────────────────────
        public string DefaultSQLQuery { get; set; }
        public DynamicParameters DefaultSQLQueryParameters { get; set; } = new DynamicParameters();
        public DynamicParameters Parameters { get; set; } = new DynamicParameters();
        public string SQLQuery { get; set; }

        /// <summary>
        /// Espone il DbContext come <see cref="IPasseroDbContext"/> per operazioni avanzate.
        /// </summary>
        public IPasseroDbContext DbContext => _dbContext;


        /// <summary>
        /// Riferimento al <see cref="ViewModel{ModelClass}"/> proprietario di questo repository.
        /// Consente al repository di aggiornare direttamente ModelItem, ModelItems e ModelItemShadow
        /// sul ViewModel dopo le operazioni di lettura, in modo speculare a <see cref="Repository{ModelClass}.ViewModel"/>.
        /// </summary>
        public ViewModel<ModelClass> ViewModel { get; set; }

        /// <summary>
        /// Registra <typeparamref name="ModelClass"/> nei DbContext EF al primo utilizzo
        /// di questo repository. Viene eseguito solo quando si usa EF (mai su Dapper-only).
        /// </summary>
        static EfRepository()
        {
            Base.ORMContextFactory.RegisterEntity(typeof(ModelClass));
        }

        public EfRepository(IPasseroDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            ModelItem = GetEmptyModelItem();
            ModelItemShadow = GetEmptyModelItem();
            DefaultSQLQuery = BuildSelectQuery(GetTableName(), dbContext.ORMType);
            Base.ORMContextFactory.RegisterEntity(typeof(ModelClass));
        }

        /// <summary>
        /// Normalizza la query SQL per l'ORM in uso.
        /// Su EF6: riscrive "SELECT * FROM tableName" con la lista esplicita delle colonne
        /// con alias (col_db AS PropName), preservando WHERE, ORDER BY, ecc.
        /// Su EF Core e altri: restituisce la query invariata.
        /// </summary>
        private string NormalizeQuery(string query)
        {
            if (_dbContext.ORMType != Base.ORMType.EntityFramework6)
                return query;

            return RewriteSelectStar(query);
        }

        /// <summary>
        /// Riscrive "SELECT [modificatori] * FROM tableName [...]" sostituendo la clausola SELECT
        /// con la lista esplicita delle colonne da <see cref="DefaultSQLQuery"/>,
        /// preservando eventuali modificatori (TOP, DISTINCT, ecc.) e tutto ciò che
        /// segue il nome della tabella (WHERE, ORDER BY, ecc.).
        /// Se la query non contiene "SELECT ... * FROM" viene restituita invariata.
        /// </summary>
        private string RewriteSelectStar(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return query;

            // Cattura:
            // - gruppo 1 (modifiers): tutto tra SELECT e *, es. "TOP(100) " o "TOP 100 " o ""
            // - gruppo 2 (tail): tutto dopo "FROM tableName", es. " WHERE ..." o ""
            var match = System.Text.RegularExpressions.Regex.Match(
                query,
                @"SELECT\s+(.*?)\*\s+FROM\s+\S+(.*)$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Singleline);

            if (!match.Success)
                return query;

            // Modificatori tra SELECT e *: es. "TOP(100) ", "TOP 100 ", "DISTINCT ", ""
            string modifiers = match.Groups[1].Value.Trim();

            // Tutto ciò che segue "FROM tableName": es. " WHERE au_id=@p" o ""
            string tail = match.Groups[2].Value.Trim();

            // DefaultSQLQuery = "SELECT col1, col2, ... FROM tableName"
            // Se ci sono modificatori li inseriamo dopo SELECT nella DefaultSQLQuery
            string baseQuery = string.IsNullOrWhiteSpace(modifiers)
                ? DefaultSQLQuery
                : System.Text.RegularExpressions.Regex.Replace(
                    DefaultSQLQuery,
                    @"^SELECT\s+",
                    $"SELECT {modifiers} ",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return string.IsNullOrWhiteSpace(tail)
                ? baseQuery
                : $"{baseQuery} {tail}";
        }

        /// <summary>
        /// Costruisce la SELECT con la lista esplicita delle colonne mappate.
        /// Il formato dell'alias dipende dall'ORM:
        /// - EF6 e Dapper: "col_db AS PropName" (il reader restituisce il nome proprietà)
        /// - EF Core: "col_db" senza alias (EF Core mappa col_db → PropName tramite il modello)
        /// Proprietà [Computed] e [NotMapped] vengono escluse.
        /// </summary>
        private static string BuildSelectQuery(string tableName, Base.ORMType ormType)
        {
            // La cache è per tipo + ORMType, dato che la query differisce tra EF6 e EF Core.
            string cacheKey = $"{typeof(ModelClass).FullName}|{ormType}";
            return _selectQueryCache.GetOrAdd(cacheKey, _ =>
            {
                bool useAlias = ormType != Base.ORMType.EntityFrameworkCore;

                var columns = typeof(ModelClass)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p =>
                        p.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() == null &&
                        p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() == null)
                    .Select(p =>
                    {
                        var columnAttr = p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>();
                        string columnName = columnAttr?.Name;

                        if (string.IsNullOrWhiteSpace(columnName) ||
                            string.Equals(columnName, p.Name, StringComparison.Ordinal))
                            return p.Name;

                        // EF6 / Dapper: "email_a AS email"
                        // EF Core:      "email_a" (EF Core mappa tramite HasColumnName nel modello)
                        return useAlias ? $"{columnName} AS {p.Name}" : columnName;
                    });

                return $"SELECT {string.Join(", ", columns)} FROM {tableName}";
            });
        }


        // ── Metadati ──────────────────────────────────────────────────────────
        public string GetTableName()
        {
            return _tableNameCache.GetOrAdd(typeof(ModelClass), type =>
            {
                // 1. Cerca prima l'attributo Dapper.Contrib (usato nei modelli del progetto)
                var dapperAttr = type.GetCustomAttribute<Dapper.Contrib.Extensions.TableAttribute>();
                if (dapperAttr != null)
                    return dapperAttr.Name;

                // 2. Fallback: attributo DataAnnotations.Schema (EF standard)
                var schemaAttr = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
                if (schemaAttr != null)
                    return schemaAttr.Name;

                // 3. Ultimo fallback: nome del tipo
                return type.Name;
            });
        }

        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }

        // ── Shadow ────────────────────────────────────────────────────────────

        public void SetModelItemShadow()
        {
            ModelItemShadow = Utilities.Clone(ModelItem);
        }

        public void SetModelItemsShadow()
        {
            ModelItemsShadow = Utilities.Clone(ModelItems);
        }

        public bool IsModelDataChanged(ModelClass modelShadow = null)
        {
            return !Utilities.ObjectsEquals(ModelItem, modelShadow ?? ModelItemShadow);
        }

        // ── HELPERS PRIVATI ───────────────────────────────────────────────────

        private void ApplyResults(IList<ModelClass> items)
        {
            ModelItems = items;
            if (items.Count > 0)
            {
                CurrentModelItemIndex = 0;
                ModelItem = items[0];
                ModelItemsShadow = Utilities.Clone(items);
                ModelItemShadow = Utilities.Clone(items[0]);
            }
            else
            {
                CurrentModelItemIndex = -1;
                ModelItem = GetEmptyModelItem();
                ModelItemShadow = GetEmptyModelItem();
            }

            if (ViewModel != null)
            {
                ViewModel.ModelItems = ModelItems;
                ViewModel.ModelItem = ModelItem;
                ViewModel.ModelItemsShadow = ModelItemsShadow;
            }
        }

        private void HandleError(ExecutionResult er, Exception ex, string debugInfo = "")
        {
            er.ResultCode = ExecutionResultCodes.Failed;
            er.Exception = ex;
            er.ResultMessage = ex.Message;
            er.ErrorCode = 1;
            er.DebugInfo = debugInfo;
        }

        // ── NAVIGAZIONE ───────────────────────────────────────────────────────

        public ExecutionResult<ModelClass> GetModelItemsAt(int index)
        {
            var er = new ExecutionResult<ModelClass>($"{_mClassName}.GetModelItemsAt()");
            if (ModelItems == null || index < 0 || index >= ModelItems.Count)
            {
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = "Invalid index.";
                er.ErrorCode = 1;
            }
            else
            {
                er.Value = ModelItems[index];
            }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        private ExecutionResult MoveToIndex(int index)
        {
            var er = new ExecutionResult($"{_mClassName}.MoveToIndex()");
            if (ModelItems != null && ModelItems.Count > 0 && index >= 0 && index < ModelItems.Count)
            {
                CurrentModelItemIndex = index;
                ModelItem = ModelItems[index];
            }
            else
            {
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ErrorCode = 1;
                er.ResultMessage = "Invalid index position.";
            }
            return er;
        }

        public ExecutionResult MoveFirstItem() => MoveToIndex(0);
        public ExecutionResult MoveLastItem() => MoveToIndex(ModelItems?.Count - 1 ?? -1);
        public ExecutionResult MovePreviousItem() => MoveToIndex(CurrentModelItemIndex - 1);
        public ExecutionResult MoveNextItem() => MoveToIndex(CurrentModelItemIndex + 1);
        public ExecutionResult MoveAtItem(int index) => MoveToIndex(index);

        // ── READ LINQ ─────────────────────────────────────────────────────────

        public ExecutionResult<IList<ModelClass>> GetAllItems(
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            var er = new ExecutionResult<IList<ModelClass>>($"{_mClassName}.GetAllItems()");
            try
            {
                string queryToRun = NormalizeQuery(SQLQuery);

                if (string.IsNullOrWhiteSpace(queryToRun))
                    queryToRun = DefaultSQLQuery;

                IList<ModelClass> results;

                if (!string.IsNullOrWhiteSpace(queryToRun))
                {
                    var sqlParams = BuildSqlParameters(Parameters);
                    results = _dbContext.SqlQuery<ModelClass>(queryToRun, sqlParams).ToList();
                }
                else
                {
                    results = _dbContext.Set<ModelClass>().ToList();
                }

                ApplyResults(results);
                er.Value = ModelItems;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        public async Task<ExecutionResult<IList<ModelClass>>> GetAllItemsAsync(
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            var er = new ExecutionResult<IList<ModelClass>>($"{_mClassName}.GetAllItemsAsync()");
            try
            {
                string queryToRun = NormalizeQuery(SQLQuery);

                if (string.IsNullOrWhiteSpace(queryToRun))
                    queryToRun = DefaultSQLQuery;

                IList<ModelClass> results;

                if (!string.IsNullOrWhiteSpace(queryToRun))
                {
                    var sqlParams = BuildSqlParameters(Parameters);
                    results = await Task.Run(() =>
                        _dbContext.SqlQuery<ModelClass>(queryToRun, sqlParams).ToList());
                }
                else
                {
                    results = await _dbContext.ToListAsync(_dbContext.Set<ModelClass>());
                }

                ApplyResults(results);
                er.Value = ModelItems;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        /// <summary>Esegue una query LINQ sul Set.</summary>
        public ExecutionResult<IList<ModelClass>> GetItems(
            Func<IQueryable<ModelClass>, IQueryable<ModelClass>> query)
        {
            var er = new ExecutionResult<IList<ModelClass>>($"{_mClassName}.GetItems(LINQ)");
            try
            {
                ApplyResults(query(_dbContext.Set<ModelClass>()).ToList());
                er.Value = ModelItems;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        public async Task<ExecutionResult<IList<ModelClass>>> GetItemsAsync(
            Func<IQueryable<ModelClass>, IQueryable<ModelClass>> query)
        {
            var er = new ExecutionResult<IList<ModelClass>>($"{_mClassName}.GetItemsAsync(LINQ)");
            try
            {
                ApplyResults(await _dbContext.ToListAsync(query(_dbContext.Set<ModelClass>())));
                er.Value = ModelItems;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        // ── HELPER: SetSQLQuery ───────────────────────────────────────────────

        /// <summary>
        /// Imposta la query SQL corrente e i parametri.
        /// Chiamato da <see cref="Controls.QBEForm{T}"/> tramite reflection o interfaccia
        /// dopo la selezione QBE, prima del reload del ViewModel.
        /// </summary>
        public void SetSQLQuery(string sqlQuery, DynamicParameters parameters)
        {
            SQLQuery = sqlQuery;
            //SQLQuery = NormalizeQuery(sqlQuery);
            Parameters = parameters ?? new DynamicParameters();
        }

        // ── READ SQL RAW ──────────────────────────────────────────────────────

        public ExecutionResult<IList<ModelClass>> GetItems(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var er = new ExecutionResult<IList<ModelClass>>($"{_mClassName}.GetItems(SQL)");
            try
            {
                string queryToRun = NormalizeQuery(query);
                SQLQuery = queryToRun;
                var sqlParams = BuildSqlParameters(parameters);
                ApplyResults(_dbContext.SqlQuery<ModelClass>(queryToRun, sqlParams).ToList());
                er.Value = ModelItems;
                er.DebugInfo = queryToRun;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex, query); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        public async Task<ExecutionResult<IList<ModelClass>>> GetItemsAsync(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var er = new ExecutionResult<IList<ModelClass>>($"{_mClassName}.GetItemsAsync(SQL)");
            try
            {
                string queryToRun = NormalizeQuery(query);
                SQLQuery = queryToRun;
                var sqlParams = BuildSqlParameters(parameters);
                var result = await Task.Run(() =>
                    _dbContext.SqlQuery<ModelClass>(queryToRun, sqlParams).ToList());
                ApplyResults(result);
                er.Value = ModelItems;
                er.DebugInfo = queryToRun;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex, query); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        public ExecutionResult<ModelClass> GetItem(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var er = new ExecutionResult<ModelClass>($"{_mClassName}.GetItem(SQL)");
            try
            {
                string queryToRun = NormalizeQuery(query);
                SQLQuery = queryToRun;
                var sqlParams = BuildSqlParameters(parameters);
                var item = _dbContext.SqlQuery<ModelClass>(queryToRun, sqlParams).SingleOrDefault();
                ModelItem = item ?? GetEmptyModelItem();
                SetModelItemShadow();
                er.Value = ModelItem;
                er.DebugInfo = queryToRun;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex, query); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        public async Task<ExecutionResult<ModelClass>> GetItemAsync(
            string query,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null)
        {
            var er = new ExecutionResult<ModelClass>($"{_mClassName}.GetItemAsync(SQL)");
            try
            {
                string queryToRun = NormalizeQuery(query);
                SQLQuery = queryToRun;
                var sqlParams = BuildSqlParameters(parameters);
                var item = await Task.Run(() =>
                    _dbContext.SqlQuery<ModelClass>(queryToRun, sqlParams).SingleOrDefault());
                ModelItem = item ?? GetEmptyModelItem();
                SetModelItemShadow();
                er.Value = ModelItem;
                er.DebugInfo = queryToRun;
            }
            catch (Exception ex) { HandleError(er.ToExecutionResult(), ex, query); }
            LastExecutionResult = er.ToExecutionResult();
            return er;
        }

        public ExecutionResult ReloadItems(bool buffered = true, int? commandTimeout = null)
        {
            if (string.IsNullOrWhiteSpace(SQLQuery) && !string.IsNullOrWhiteSpace(DefaultSQLQuery))
                SQLQuery = DefaultSQLQuery;

            return GetAllItems().ToExecutionResult();
        }

        // ── EXECUTE SQL RAW (non-query) ────────────────────────────────────────

        public ExecutionResult Execute(string sql, params object[] parameters)
        {
            var er = new ExecutionResult($"{_mClassName}.Execute()");
            try
            {
                er.Value = _dbContext.ExecuteSql(sql, parameters);
                er.DebugInfo = sql;
            }
            catch (Exception ex) { HandleError(er, ex, sql); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> ExecuteAsync(string sql, params object[] parameters)
        {
            var er = new ExecutionResult($"{_mClassName}.ExecuteAsync()");
            try
            {
                er.Value = await _dbContext.ExecuteSqlAsync(sql, parameters);
                er.DebugInfo = sql;
            }
            catch (Exception ex) { HandleError(er, ex, sql); }
            LastExecutionResult = er;
            return er;
        }

        // ── WRITE ─────────────────────────────────────────────────────────────

        public ExecutionResult InsertItem(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.InsertItem()");
            try
            {
                model ??= ModelItem;
                _dbContext.Add(model);
                _dbContext.SaveChanges();
                ModelItem = model;
                ModelItemShadow = Utilities.Clone(model);
                AddNewState = false;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> InsertItemAsync(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.InsertItemAsync()");
            try
            {
                model ??= ModelItem;
                _dbContext.Add(model);
                await _dbContext.SaveChangesAsync();
                ModelItem = model;
                ModelItemShadow = Utilities.Clone(model);
                AddNewState = false;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public ExecutionResult InsertItems(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.InsertItems()");
            try
            {
                items ??= ModelItems;
                _dbContext.AddRange(items);
                _dbContext.SaveChanges();
                er.Value = items.Count();
                AddNewState = false;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> InsertItemsAsync(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.InsertItemsAsync()");
            try
            {
                items ??= ModelItems;
                _dbContext.AddRange(items);
                await _dbContext.SaveChangesAsync();
                er.Value = items.Count();
                AddNewState = false;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public ExecutionResult UpdateItem(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItem()");
            try
            {
                model ??= ModelItem;
                _dbContext.MarkModified(model);
                _dbContext.SaveChanges();
                ModelItemShadow = Utilities.Clone(model);
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> UpdateItemAsync(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItemAsync()");
            try
            {
                model ??= ModelItem;
                _dbContext.MarkModified(model);
                await _dbContext.SaveChangesAsync();
                ModelItemShadow = Utilities.Clone(model);
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public ExecutionResult UpdateItemEx(
            ModelClass modelItem = null, ModelClass modelItemShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItemEx()");
            try
            {
                modelItem ??= ModelItem;
                if (!Utilities.ObjectsEquals(modelItem, modelItemShadow ?? ModelItemShadow))
                {
                    _dbContext.MarkModified(modelItem);
                    _dbContext.SaveChanges();
                    ModelItemShadow = Utilities.Clone(modelItem);
                }
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> UpdateItemExAsync(
            ModelClass modelItem = null, ModelClass modelItemShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItemExAsync()");
            try
            {
                modelItem ??= ModelItem;
                if (!Utilities.ObjectsEquals(modelItem, modelItemShadow ?? ModelItemShadow))
                {
                    _dbContext.MarkModified(modelItem);
                    await _dbContext.SaveChangesAsync();
                    ModelItemShadow = Utilities.Clone(modelItem);
                }
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public ExecutionResult UpdateItems(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItems()");
            try
            {
                items ??= ModelItems;
                foreach (var item in items)
                    _dbContext.MarkModified(item);
                _dbContext.SaveChanges();
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> UpdateItemsAsync(
            IEnumerable<ModelClass> items = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItemsAsync()");
            try
            {
                items ??= ModelItems;
                foreach (var item in items)
                    _dbContext.MarkModified(item);
                await _dbContext.SaveChangesAsync();
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        /// <summary>
        /// Aggiorna solo gli item della lista che risultano cambiati rispetto alla lista shadow.
        /// EF non usa SQL generato a colonne: confronta con <see cref="Utilities.ObjectsEquals"/> e
        /// marca modificato solo ciò che è effettivamente cambiato.
        /// </summary>
        public ExecutionResult UpdateItemsEx(
            IEnumerable<ModelClass> items = null, IEnumerable<ModelClass> itemsShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItemsEx()");
            try
            {
                var itemList = (items ?? ModelItems).ToList();
                var shadowList = (itemsShadow ?? ModelItemsShadow).ToList();
                int affected = 0;

                for (int i = 0; i < itemList.Count; i++)
                {
                    var shadow = i < shadowList.Count ? shadowList[i] : null;
                    if (!Utilities.ObjectsEquals(itemList[i], shadow))
                    {
                        _dbContext.MarkModified(itemList[i]);
                        affected++;
                    }
                }

                if (affected > 0)
                    _dbContext.SaveChanges();

                er.Value = affected;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        /// <summary>
        /// Versione asincrona di <see cref="UpdateItemsEx"/>.
        /// </summary>
        public async Task<ExecutionResult> UpdateItemsExAsync(
            IEnumerable<ModelClass> items = null, IEnumerable<ModelClass> itemsShadow = null,
            IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.UpdateItemsExAsync()");
            try
            {
                var itemList = (items ?? ModelItems).ToList();
                var shadowList = (itemsShadow ?? ModelItemsShadow).ToList();
                int affected = 0;

                for (int i = 0; i < itemList.Count; i++)
                {
                    var shadow = i < shadowList.Count ? shadowList[i] : null;
                    if (!Utilities.ObjectsEquals(itemList[i], shadow))
                    {
                        _dbContext.MarkModified(itemList[i]);
                        affected++;
                    }
                }

                if (affected > 0)
                    await _dbContext.SaveChangesAsync();

                er.Value = affected;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public ExecutionResult DeleteItem(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.DeleteItem()");
            try
            {
                model ??= ModelItem;
                _dbContext.Remove(model);
                _dbContext.SaveChanges();
                ModelItems.Remove(model);
                ModelItemsShadow.Remove(model);
                ModelItem = GetEmptyModelItem();
                ModelItemShadow = GetEmptyModelItem();
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> DeleteItemAsync(
            ModelClass model = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.DeleteItemAsync()");
            try
            {
                model ??= ModelItem;
                _dbContext.Remove(model);
                await _dbContext.SaveChangesAsync();
                ModelItems.Remove(model);
                ModelItemsShadow.Remove(model);
                ModelItem = GetEmptyModelItem();
                ModelItemShadow = GetEmptyModelItem();
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public ExecutionResult DeleteItems(
            IEnumerable<ModelClass> items, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.DeleteItems()");
            try
            {
                _dbContext.RemoveRange(items);
                _dbContext.SaveChanges();
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        public async Task<ExecutionResult> DeleteItemsAsync(
            IEnumerable<ModelClass> items, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var er = new ExecutionResult($"{_mClassName}.DeleteItemsAsync()");
            try
            {
                _dbContext.RemoveRange(items);
                await _dbContext.SaveChangesAsync();
                er.Value = true;
            }
            catch (Exception ex) { HandleError(er, ex); }
            LastExecutionResult = er;
            return er;
        }

        // ── HELPER: conversione parametri → SqlParameter[] per EF Core/EF6 ──────

        private static object[] BuildSqlParameters(object parameters)
        {
            if (parameters == null)
                return Array.Empty<object>();

            // Caso 1: già un array di object — restituito direttamente
            if (parameters is object[] arr)
                return arr;

            // Caso 2: DynamicParameters di Dapper — estrarre nome/valore e creare SqlParameter[]
            if (parameters is DynamicParameters dynamicParams)
            {
                return dynamicParams.ParameterNames
                    .Select(name => (object)new SqlParameter(
                        name.StartsWith("@") ? name : $"@{name}",
                        ((SqlMapper.IParameterLookup)dynamicParams)[name] ?? DBNull.Value))
                    .ToArray();
            }

            // Caso 3: oggetto anonimo o POCO — reflection sulle proprietà pubbliche
            return parameters.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => (object)new SqlParameter(
                    $"@{p.Name}",
                    p.GetValue(parameters) ?? DBNull.Value))
                .ToArray();
        }

        // ── DISPOSE ───────────────────────────────────────────────────────────

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                ModelItems?.Clear();
                ModelItemsShadow?.Clear();
            }
            _disposed = true;
        }
    }
}