using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Passero.Framework.Base
{
    public class DbObject<ModelClass> where ModelClass : class
    {
        private const string mClassName = "Passero.Framework.Base.DbObject";

        private static readonly ConcurrentDictionary<string, Dictionary<string, DbColumn>> _schemaCache =
            new ConcurrentDictionary<string, Dictionary<string, DbColumn>>();

        public string Name { get; set; }
        public string NameE { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public DbObjectType DbObjectType { get; set; } = DbObjectType.Table;
        public string Schema { get; set; }

        public Dictionary<string, DbColumn> DbColumns { get; set; } =
            new Dictionary<string, DbColumn>(StringComparer.InvariantCultureIgnoreCase);

        public DataColumn[] PrimaryKeys { get; set; }

        private string mProviderName = "";
        public string ProviderName
        {
            get { return mProviderName; }
        }

        private Passero.Framework.ProviderFeatures _dbProviderFeatures;
        public Passero.Framework.ProviderFeatures DbProviderFeatures
        {
            get { return _dbProviderFeatures; }
            private set { _dbProviderFeatures = value; }
        }

        private IDbConnection mDbConnection;
        public IDbConnection DbConnection
        {
            get { return mDbConnection; }
            set { mDbConnection = value; }
        }

        public string SQLSchemaQuery { get; set; }

        public SqlCommands SqlCommands { get; set; } = new SqlCommands();

        private ModelClass Model;

        public DbObject(IDbConnection DbConnection)
        {
            Model = (ModelClass)Activator.CreateInstance(typeof(ModelClass));
            this.mDbConnection = DbConnection;
            this.SQLSchemaQuery = $"SELECT * FROM {Utilities.GetModelTableName(Model)} WHERE 1=0";
            this.GetSchema();
        }

        private void AssignValue(ref object Obj, object Value)
        {
            if (!ReferenceEquals(Value, DBNull.Value))
            {
                Obj = Value;
            }
        }

        public ExecutionResult GetSchema(string SQLSchemaQuery = "")
        {
            var ER = new ExecutionResult($"{mClassName}.Schema(Of {nameof(ModelClass)}");

            if (this.mDbConnection is null)
            {
                ER.ErrorCode = 2;
                ER.ResultMessage = "DbConnection is null.";
                return ER;
            }

            if (string.IsNullOrEmpty(SQLSchemaQuery.Trim()))
            {
                SQLSchemaQuery = this.SQLSchemaQuery;
            }

            System.Data.Common.DbDataAdapter DataAdapter;

            try
            {
                var dbConnection = this.mDbConnection as System.Data.Common.DbConnection;
                if (dbConnection is null)
                {
                    ER.ErrorCode = 3;
                    ER.ResultMessage = "DbConnection must be a DbConnection type.";
                    return ER;
                }

                var providerFactory = System.Data.Common.DbProviderFactories.GetFactory(dbConnection);
                this.mProviderName = providerFactory.GetType().Namespace;
                this.DbProviderFeatures = Passero.Framework.ProviderFeaturesResolver.FromConnection(dbConnection);

                DataAdapter = providerFactory.CreateDataAdapter();
                var command = providerFactory.CreateCommand();
                command.CommandText = SQLSchemaQuery;
                command.Connection = dbConnection;
                DataAdapter.SelectCommand = command;

                DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                var SchemaColumns = new DataTable();
                DataAdapter.FillSchema(SchemaColumns, SchemaType.Mapped);

                PrimaryKeys = SchemaColumns.PrimaryKey;
                Name = SchemaColumns.TableName;

                DbColumns.Clear();
                foreach (DataColumn DataColumn in SchemaColumns.Columns)
                {
                    var DbColumn = new DbColumn
                    {
                        ColumnName = DataColumn.ColumnName,
                        IsKey = SchemaColumns.PrimaryKey.Contains(DataColumn),
                        DataColumn = DataColumn
                    };

                    DbColumns[DbColumn.ColumnName] = DbColumn;
                }

                BuildSqlCommands();
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
            }

            return ER;
        }

        public ExecutionResult<SqlCommands> BuildSqlCommands()
        {
            var ER = new ExecutionResult<SqlCommands>($"{mClassName}.BuildSqlCommands()");
            ER.Value = new SqlCommands();

            try
            {
                ER.Value.SelectCommand = SQLSchemaQuery;
                ER.Value.InsertCommand = Utilities.GetInsertSqlCommand(this);
                ER.Value.UpdateCommand = Utilities.GetUpdateSqlCommand(this);
                ER.Value.DeleteCommand = Utilities.GetDeleteSqlCommand(this);

                SqlCommands = ER.Value;
                ER.ResultCode = ExecutionResultCodes.Success;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.ResultCode = ExecutionResultCodes.Failed;
            }

            return ER;
        }

        public string GetInsertCommand()
        {
            var properties = Utilities.GetModelPropertiesInfo(typeof(ModelClass), true)
                .Where(pi => !Utilities.PropertyIsIdentityKey(pi))
                .ToList();

            var columns = string.Join(", ", properties.Select(pi => ResolveColumnName(pi)));
            var parameters = string.Join(", ", properties.Select(pi => $"@{pi.Name}"));

            return $"INSERT INTO {GetQuotedTableName()} ({columns}) VALUES ({parameters})";
        }

        public string GetUpdateCommand()
        {
            var properties = Utilities.GetModelPropertiesInfo(typeof(ModelClass), true)
                .ToList();

            var setClause = string.Join(", ",
                properties.Where(pi => !Utilities.PropertyIsIdentityKey(pi))
                    .Select(pi => $"{ResolveColumnName(pi)}=@{pi.Name}"));

            var whereClause = string.Join(" AND ",
                properties.Where(pi => Utilities.PropertyIsExplicitKey(pi) || Utilities.PropertyIsIdentityKey(pi))
                    .Select(pi => $"{ResolveColumnName(pi)}=@{pi.Name}_shadow"));

            return $"UPDATE {GetQuotedTableName()} SET {setClause} WHERE ({whereClause})";
        }

        public string GetDeleteCommand()
        {
            var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(ModelClass));

            var whereClause = string.Join(" AND ",
                keys.Select(pi => $"{ResolveColumnName(pi)}=@{pi.Name}"));

            return $"DELETE FROM {GetQuotedTableName()} WHERE ({whereClause})";
        }

        public string ResolveColumnName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Property name is invalid.", nameof(propertyName));

            if (DbColumns != null && DbColumns.TryGetValue(propertyName, out var dbColumn))
            {
                return DbProviderFeatures?.QuoteIdentifier(dbColumn.ColumnName) ?? dbColumn.ColumnName;
            }

            var property = typeof(ModelClass).GetProperty(propertyName);
            if (property != null)
            {
                var mappedColumnName = Utilities.GetMappedColumnName(property);
                return DbProviderFeatures?.QuoteIdentifier(mappedColumnName) ?? mappedColumnName;
            }

            return DbProviderFeatures?.QuoteIdentifier(propertyName) ?? propertyName;
        }

        public string ResolveColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            return ResolveColumnName(propertyInfo.Name);
        }

        public void GetCachedSchema(bool Refresh = false)
        {
            string cacheKey = GetSchemaCacheKey();

            if (!Refresh && _schemaCache.TryGetValue(cacheKey, out Dictionary<string, DbColumn> cachedSchema))
            {
                DbColumns = new Dictionary<string, DbColumn>(cachedSchema, StringComparer.InvariantCultureIgnoreCase);
                return;
            }

            GetSchema();
            _schemaCache[cacheKey] = new Dictionary<string, DbColumn>(DbColumns, StringComparer.InvariantCultureIgnoreCase);
        }

        private string GetSchemaCacheKey()
        {
            string tableName = Utilities.GetModelTableName(typeof(ModelClass));
            string schemaQuery = SQLSchemaQuery ?? string.Empty;

            return $"{mProviderName}|{tableName}|{schemaQuery}|{typeof(ModelClass).AssemblyQualifiedName}";
        }

        private string GetQuotedTableName()
        {
            string tableName = string.IsNullOrWhiteSpace(Name)
                ? Utilities.GetModelTableName(typeof(ModelClass))
                : Name;

            return DbProviderFeatures?.QuoteQualifiedIdentifier(tableName) ?? tableName;
        }

        public string GetTableName()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                return Name;
            }

            return Utilities.GetModelTableName(typeof(ModelClass));
        }
    }

    public class SqlCommands
    {
        public string SelectCommand { get; set; }
        public string InsertCommand { get; set; }
        public string UpdateCommand { get; set; }
        public string DeleteCommand { get; set; }
    }
}