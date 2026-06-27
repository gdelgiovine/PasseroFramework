using Dapper;
using Passero.Framework;
using Passero.Framework.Base;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Passero.Framework.QueryEngine
{
    public sealed class QueryEngine : IQueryEngine
    {
        public QueryEngine(Compiler compiler = null)
        {
            Compiler = compiler;
        }

        public Compiler Compiler { get; }

        public CompiledQuery BuildSelect<TModel>(
            ViewModel<TModel> viewModel,
            SelectQueryRequest<TModel> request = null)
            where TModel : class
        {
            var dbObject = ResolveDbObject(viewModel);
            var compiler = ResolveCompiler(viewModel);

            request ??= new SelectQueryRequest<TModel>();

            var query = new Query(dbObject.GetTableName());

            if (request.Distinct)
            {
                query = query.Distinct();
            }

            var selectColumns = ResolveSelectColumns(dbObject, request.SelectColumns);

            query = selectColumns.Count > 0
                ? query.Select(selectColumns.ToArray())
                : query.Select(GetAllColumns(dbObject).ToArray());

            query = ApplyConditions(query, dbObject, request.Conditions);
            query = ApplyOrderBy(query, dbObject, request.OrderBy, request.UseDefaultOrderByClause);

            if (request.Skip.HasValue)
            {
                query = query.Offset(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                query = query.Limit(request.Take.Value);
            }

            return Compile(query, compiler);
        }

        public CompiledQuery BuildSelect<TModel>(
            ViewModel<TModel> viewModel,
            params Expression<Func<TModel, object?>>[] selectColumns)
            where TModel : class
        {
            var request = new SelectQueryRequest<TModel>();

            if (selectColumns != null)
            {
                request.SelectColumns.AddRange(selectColumns.Where(column => column != null));
            }

            return BuildSelect(viewModel, request);
        }

        public CompiledQuery BuildSelect<TModel>(
            ViewModel<TModel> viewModel,
            IEnumerable<QueryConditionNode<TModel>> conditions,
            params Expression<Func<TModel, object?>>[] selectColumns)
            where TModel : class
        {
            var request = new SelectQueryRequest<TModel>();

            if (selectColumns != null)
            {
                request.SelectColumns.AddRange(selectColumns.Where(column => column != null));
            }

            if (conditions != null)
            {
                request.Conditions.AddRange(conditions.Where(condition => condition != null));
            }

            return BuildSelect(viewModel, request);
        }

        public async Task<CompiledQuery> BuildSelectAsync<TModel>(
            ViewModel<TModel> viewModel,
            SelectQueryRequest<TModel> request = null,
            CancellationToken cancellationToken = default)
            where TModel : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(BuildSelect(viewModel, request));
        }

        public CompiledQuery BuildInsert<TModel>(
            ViewModel<TModel> viewModel,
            MutationQueryRequest<TModel> request = null)
            where TModel : class
        {
            var dbObject = ResolveDbObject(viewModel);
            var compiler = ResolveCompiler(viewModel);
            var model = GetRequiredModelItem(viewModel);

            IDictionary<string, object> values = BuildInsertValues(dbObject, model);

            if (values.Count == 0)
            {
                throw new InvalidOperationException("Nessuna colonna inseribile trovata.");
            }

            var query = new Query(dbObject.GetTableName()).AsInsert(values);
            return Compile(query, compiler);
        }

        public CompiledQuery BuildUpdate<TModel>(
            ViewModel<TModel> viewModel,
            MutationQueryRequest<TModel> request = null)
            where TModel : class
        {
            var dbObject = ResolveDbObject(viewModel);
            var compiler = ResolveCompiler(viewModel);
            var currentModel = GetRequiredModelItem(viewModel);
            var shadowModel = viewModel.ModelItemShadow ?? currentModel;

            IDictionary<string, object> values = BuildUpdateValues(dbObject, currentModel);

            if (values.Count == 0)
            {
                throw new InvalidOperationException("Nessuna colonna aggiornabile trovata.");
            }

            var query = new Query(dbObject.GetTableName());
            query = ApplyConditions(query, dbObject, request?.Conditions);

            var keyColumns = ResolveKeyColumns(dbObject, request?.KeyColumns);

            if (keyColumns.Count == 0 && (request?.Conditions == null || request.Conditions.Count == 0))
            {
                throw new InvalidOperationException("Nessuna condizione WHERE disponibile per l'UPDATE.");
            }

            query = ApplyKeyFilters(query, keyColumns, shadowModel);
            query = query.AsUpdate(values);

            return Compile(query, compiler);
        }

        public CompiledQuery BuildDelete<TModel>(
            ViewModel<TModel> viewModel,
            MutationQueryRequest<TModel> request = null)
            where TModel : class
        {
            var dbObject = ResolveDbObject(viewModel);
            var compiler = ResolveCompiler(viewModel);
            var keySource = viewModel.ModelItemShadow ?? viewModel.ModelItem;

            if (keySource == null)
            {
                throw new InvalidOperationException("ModelItem non inizializzato nel ViewModel.");
            }

            var query = new Query(dbObject.GetTableName());
            query = ApplyConditions(query, dbObject, request?.Conditions);

            var keyColumns = ResolveKeyColumns(dbObject, request?.KeyColumns);

            if (keyColumns.Count == 0 && (request?.Conditions == null || request.Conditions.Count == 0))
            {
                throw new InvalidOperationException("Nessuna condizione WHERE disponibile per il DELETE.");
            }

            query = ApplyKeyFilters(query, keyColumns, keySource);
            query = query.AsDelete();

            return Compile(query, compiler);
        }

        private CompiledQuery Compile(Query query, Compiler compiler)
        {
            var result = compiler.Compile(query);
            return new CompiledQuery(result.Sql, ToDynamicParameters(result.Bindings));
        }

        private Compiler ResolveCompiler<TModel>(ViewModel<TModel> viewModel)
            where TModel : class
        {
            if (Compiler != null)
            {
                return Compiler;
            }

            if (viewModel?.Repository?.ProviderFeatures == null)
            {
                return new SqlServerCompiler();
            }

            return CreateCompiler(viewModel.Repository.ProviderFeatures);
        }

        private static DbObject<TModel> ResolveDbObject<TModel>(ViewModel<TModel> viewModel)
            where TModel : class
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (viewModel.Repository is Repository<TModel> dapperRepository && dapperRepository.DbObject != null)
            {
                return dapperRepository.DbObject;
            }

            throw new InvalidOperationException(
                "QueryEngine richiede un ViewModel inizializzato con il repository Dapper e il relativo DbObject.");
        }

        private static TModel GetRequiredModelItem<TModel>(ViewModel<TModel> viewModel)
            where TModel : class
        {
            if (viewModel?.ModelItem == null)
            {
                throw new InvalidOperationException("ModelItem non inizializzato nel ViewModel.");
            }

            return viewModel.ModelItem;
        }

        private static Query ApplyConditions<TModel>(
            Query query,
            DbObject<TModel> dbObject,
            IEnumerable<QueryConditionNode<TModel>> conditions)
            where TModel : class
        {
            if (conditions == null)
            {
                return query;
            }

            var fragment = BuildConditionFragment(dbObject, conditions, QueryLogicalCondition.And);

            if (!fragment.IsEmpty)
            {
                query = query.WhereRaw(fragment.Sql, fragment.Bindings.ToArray());
            }

            return query;
        }

        private static SqlFragment BuildConditionFragment<TModel>(
            DbObject<TModel> dbObject,
            IEnumerable<QueryConditionNode<TModel>> nodes,
            QueryLogicalCondition joinCondition)
            where TModel : class
        {
            if (nodes == null)
            {
                return SqlFragment.Empty;
            }

            var parts = new List<string>();
            var bindings = new List<object>();
            var glue = joinCondition == QueryLogicalCondition.Or ? " OR " : " AND ";

            foreach (var node in nodes)
            {
                if (node == null)
                {
                    continue;
                }

                SqlFragment fragment = node switch
                {
                    QueryFilter<TModel> filter => BuildFilterFragment(dbObject, filter),
                    QueryConditionGroup<TModel> group =>
                        BuildConditionFragment(dbObject, group.Conditions, group.Condition),
                    _ => SqlFragment.Empty
                };

                if (fragment.IsEmpty)
                {
                    continue;
                }

                parts.Add(fragment.Sql);
                bindings.AddRange(fragment.Bindings);
            }

            if (parts.Count == 0)
            {
                return SqlFragment.Empty;
            }

            return new SqlFragment("(" + string.Join(glue, parts) + ")", bindings);
        }

        private static SqlFragment BuildFilterFragment<TModel>(
            DbObject<TModel> dbObject,
            QueryFilter<TModel> filter)
            where TModel : class
        {
            if (filter == null)
            {
                return SqlFragment.Empty;
            }

            var columnName = ResolveColumnName(dbObject, filter.Column);

            switch (filter.Operator)
            {
                case QueryOperator.Equal:
                    return filter.Value == null
                        ? new SqlFragment($"{columnName} IS NULL")
                        : new SqlFragment($"{columnName} = ?", filter.Value);

                case QueryOperator.NotEqual:
                    return filter.Value == null
                        ? new SqlFragment($"{columnName} IS NOT NULL")
                        : new SqlFragment($"{columnName} <> ?", filter.Value);

                case QueryOperator.GreaterThan:
                    return new SqlFragment($"{columnName} > ?", filter.Value);

                case QueryOperator.GreaterThanOrEqual:
                    return new SqlFragment($"{columnName} >= ?", filter.Value);

                case QueryOperator.LessThan:
                    return new SqlFragment($"{columnName} < ?", filter.Value);

                case QueryOperator.LessThanOrEqual:
                    return new SqlFragment($"{columnName} <= ?", filter.Value);

                case QueryOperator.Like:
                    return new SqlFragment($"{columnName} LIKE ?", filter.Value);

                case QueryOperator.NotLike:
                    return new SqlFragment($"{columnName} NOT LIKE ?", filter.Value);

                case QueryOperator.Contains:
                    return new SqlFragment($"{columnName} LIKE ?", BuildLikePattern(filter.Value, "%", "%"));

                case QueryOperator.StartsWith:
                    return new SqlFragment($"{columnName} LIKE ?", BuildLikePattern(filter.Value, string.Empty, "%"));

                case QueryOperator.EndsWith:
                    return new SqlFragment($"{columnName} LIKE ?", BuildLikePattern(filter.Value, "%", string.Empty));

                case QueryOperator.In:
                    return BuildInFragment(columnName, filter.Value, false);

                case QueryOperator.NotIn:
                    return BuildInFragment(columnName, filter.Value, true);

                case QueryOperator.Between:
                    return new SqlFragment($"{columnName} BETWEEN ? AND ?", filter.Value, filter.Value2);

                case QueryOperator.NotBetween:
                    return new SqlFragment($"{columnName} NOT BETWEEN ? AND ?", filter.Value, filter.Value2);

                case QueryOperator.IsNull:
                    return new SqlFragment($"{columnName} IS NULL");

                case QueryOperator.IsNotNull:
                    return new SqlFragment($"{columnName} IS NOT NULL");

                default:
                    throw new NotSupportedException($"Operatore non supportato: {filter.Operator}");
            }
        }

        private static SqlFragment BuildInFragment(string columnName, object value, bool negate)
        {
            var items = ToEnumerable(value).ToArray();

            if (items.Length == 0)
            {
                throw new ArgumentException("La clausola IN richiede almeno un valore.");
            }

            var placeholders = string.Join(", ", Enumerable.Repeat("?", items.Length));
            var sql = $"{columnName} {(negate ? "NOT IN" : "IN")} ({placeholders})";

            return new SqlFragment(sql, items);
        }

        private static Query ApplyKeyFilters<TModel>(
            Query query,
            IReadOnlyCollection<string> keyColumns,
            TModel source)
            where TModel : class
        {
            if (keyColumns == null || keyColumns.Count == 0)
            {
                return query;
            }

            foreach (var keyColumn in keyColumns)
            {
                var property = ResolvePropertyByColumnName(typeof(TModel), keyColumn);

                if (property == null)
                {
                    throw new InvalidOperationException($"Chiave non risolvibile per la colonna '{keyColumn}'.");
                }

                var value = property.GetValue(source);
                query = value == null
                    ? query.WhereNull(keyColumn)
                    : query.Where(keyColumn, "=", value);
            }

            return query;
        }

        private static IReadOnlyCollection<string> ResolveSelectColumns<TModel>(
            DbObject<TModel> dbObject,
            IReadOnlyCollection<Expression<Func<TModel, object?>>> selectColumns)
            where TModel : class
        {
            if (selectColumns == null || selectColumns.Count == 0)
            {
                return Array.Empty<string>();
            }

            var resolved = new List<string>();

            foreach (var expression in selectColumns)
            {
                var property = ResolvePropertyInfo(expression);
                resolved.Add(ResolveColumnName(dbObject, property));
            }

            return resolved;
        }

        private static Query ApplyOrderBy<TModel>(
            Query query,
            DbObject<TModel> dbObject,
            IReadOnlyCollection<QueryOrder<TModel>> orderBy,
            bool useDefaultOrderByClause)
            where TModel : class
        {
            if (orderBy != null && orderBy.Count > 0)
            {
                foreach (var order in orderBy)
                {
                    var property = ResolvePropertyInfo(order.Column);
                    var columnName = ResolveColumnName(dbObject, property);

                    query = order.Descending
                        ? query.OrderByDesc(columnName)
                        : query.OrderBy(columnName);
                }

                return query;
            }

            if (!useDefaultOrderByClause)
            {
                return query;
            }

            foreach (var columnName in ResolveDefaultOrderByColumns(dbObject))
            {
                query = query.OrderBy(columnName);
            }

            return query;
        }

        private static IReadOnlyCollection<string> ResolveDefaultOrderByColumns<TModel>(DbObject<TModel> dbObject)
            where TModel : class
        {
            return dbObject.DbColumns.Values
                .Where(column => column.IsKey)
                .OrderBy(column => column.DataColumn?.Ordinal ?? int.MaxValue)
                .Select(column => column.ColumnName)
                .ToArray();
        }

        private static IReadOnlyCollection<string> GetAllColumns<TModel>(DbObject<TModel> dbObject)
            where TModel : class
        {
            return dbObject.DbColumns.Values
                .OrderBy(column => column.DataColumn?.Ordinal ?? int.MaxValue)
                .Select(column => column.ColumnName)
                .ToArray();
        }

        private static IReadOnlyCollection<string> ResolveKeyColumns<TModel>(
            DbObject<TModel> dbObject,
            Expression<Func<TModel, object?>>[] keyColumns)
            where TModel : class
        {
            if (keyColumns != null && keyColumns.Length > 0)
            {
                return keyColumns
                    .Select(column => ResolveColumnName(dbObject, ResolvePropertyInfo(column)))
                    .ToArray();
            }

            return ResolveDefaultOrderByColumns(dbObject);
        }

        private static IDictionary<string, object> BuildInsertValues<TModel>(DbObject<TModel> dbObject, TModel model)
            where TModel : class
        {
            var values = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)values;

            foreach (var dbColumn in dbObject.DbColumns.Values.OrderBy(c => c.DataColumn?.Ordinal ?? int.MaxValue))
            {
                var property = ResolvePropertyByColumnName(typeof(TModel), dbColumn.ColumnName);

                if (property == null || Utilities.PropertyIsIdentityKey(property))
                {
                    continue;
                }

                dictionary[dbColumn.ColumnName] = property.GetValue(model);
            }

            return dictionary;
        }

        private static IDictionary<string, object> BuildUpdateValues<TModel>(DbObject<TModel> dbObject, TModel model)
            where TModel : class
        {
            var values = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)values;

            foreach (var dbColumn in dbObject.DbColumns.Values.OrderBy(c => c.DataColumn?.Ordinal ?? int.MaxValue))
            {
                if (dbColumn.IsKey)
                {
                    continue;
                }

                var property = ResolvePropertyByColumnName(typeof(TModel), dbColumn.ColumnName);
                if (property == null)
                {
                    continue;
                }

                dictionary[dbColumn.ColumnName] = property.GetValue(model);
            }

            return dictionary;
        }

        private static string ResolveColumnName<TModel>(DbObject<TModel> dbObject, Expression<Func<TModel, object?>> expression)
            where TModel : class
        {
            return ResolveColumnName(dbObject, ResolvePropertyInfo(expression));
        }

        private static string ResolveColumnName<TModel>(DbObject<TModel> dbObject, PropertyInfo propertyInfo)
            where TModel : class
        {
            if (dbObject == null)
            {
                throw new ArgumentNullException(nameof(dbObject));
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (dbObject.DbColumns != null)
            {
                if (dbObject.DbColumns.TryGetValue(propertyInfo.Name, out var dbColumn) &&
                    !string.IsNullOrWhiteSpace(dbColumn?.ColumnName))
                {
                    return dbColumn.ColumnName;
                }

                var mappedColumnName = Utilities.GetMappedColumnName(propertyInfo);

                if (dbObject.DbColumns.TryGetValue(mappedColumnName, out dbColumn) &&
                    !string.IsNullOrWhiteSpace(dbColumn?.ColumnName))
                {
                    return dbColumn.ColumnName;
                }
            }

            throw new InvalidOperationException(
                $"Colonna non risolta per la proprietà '{propertyInfo.Name}' nel DbObject.");
        }

        private static PropertyInfo ResolvePropertyByColumnName(Type modelType, string columnName)
        {
            var properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (string.Equals(property.Name, columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return property;
                }

                var mapped = Utilities.GetMappedColumnName(property);
                if (string.Equals(mapped, columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return property;
                }
            }

            return null;
        }

        private static PropertyInfo ResolvePropertyInfo<TModel>(Expression<Func<TModel, object?>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            MemberExpression memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null &&
                expression.Body is UnaryExpression unary &&
                unary.Operand is MemberExpression unaryMember)
            {
                memberExpression = unaryMember;
            }

            if (memberExpression?.Member is not PropertyInfo propertyInfo)
            {
                throw new ArgumentException("L'espressione deve riferirsi a una proprietà del model.", nameof(expression));
            }

            return propertyInfo;
        }

        private static DynamicParameters ToDynamicParameters(IEnumerable<object> bindings)
        {
            var parameters = new DynamicParameters();

            if (bindings == null)
            {
                return parameters;
            }

            var index = 0;

            foreach (var binding in bindings)
            {
                parameters.Add($"p{index}", binding);
                index++;
            }

            return parameters;
        }

        private static IEnumerable<object> ToEnumerable(object value)
        {
            if (value == null)
            {
                return Array.Empty<object>();
            }

            if (value is string)
            {
                return new[] { value };
            }

            if (value is IEnumerable enumerable)
            {
                var items = new List<object>();

                foreach (var item in enumerable)
                {
                    items.Add(item);
                }

                return items;
            }

            return new[] { value };
        }

        private static string BuildLikePattern(object value, string prefix, string suffix)
        {
            if (value == null)
            {
                return null;
            }

            return $"{prefix}{Convert.ToString(value, CultureInfo.InvariantCulture)}{suffix}";
        }

        private static Compiler CreateCompiler(ProviderFeatures providerFeatures)
        {
            switch (providerFeatures?.Dialect ?? DbDialect.Unknown)
            {
                case DbDialect.SqlServer:
                case DbDialect.SqlServerCe:
                    return new SqlServerCompiler();

                case DbDialect.PostgreSql:
                    return new PostgresCompiler();

                case DbDialect.MySql:
                case DbDialect.MariaDb:
                    return new MySqlCompiler();

                case DbDialect.SQLite:
                    return new SqliteCompiler();

                case DbDialect.Oracle:
                    return new OracleCompiler();

                case DbDialect.DB2:
                case DbDialect.DB2i:
                    return new Db2Compiler();

                default:
                    return new SqlServerCompiler();
            }
        }

        private sealed class SqlFragment
        {
            public static readonly SqlFragment Empty = new SqlFragment(string.Empty);

            public SqlFragment(string sql, params object[] bindings)
            {
                Sql = sql ?? string.Empty;
                Bindings = bindings?.ToList() ?? new List<object>();
            }

            public SqlFragment(string sql, IEnumerable<object> bindings)
            {
                Sql = sql ?? string.Empty;
                Bindings = bindings?.ToList() ?? new List<object>();
            }

            public string Sql { get; }

            public List<object> Bindings { get; }

            public bool IsEmpty => string.IsNullOrWhiteSpace(Sql);
        }
    }
}