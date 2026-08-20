
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Dapper.Contrib.Extensions;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataBaseHelper
    {

        /// <summary>
        /// Lists to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


        /// <summary>
        /// Lists to data table.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static DataTable ListToDataTable(object data)
        {
            Type type = Passero.Framework.ReflectionHelper.GetListType(data);
            if (type == null)
                return null;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            IList objectList = data as IList;
            foreach (var item in objectList)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Datas the column is numeric.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        public static bool DataColumnIsNumeric(DataColumn col)
        {
            if (col is null)
                return false;
            // Make this const
            Type[] numericTypes = new[] { typeof(byte), typeof(decimal), typeof(double), typeof(short), typeof(int), typeof(long), typeof(sbyte), typeof(float), typeof(ushort), typeof(uint), typeof(ulong) };
            return numericTypes.Contains(col.DataType);
        }
        /// <summary>
        /// Pings the database.
        /// </summary>
        /// <param name="DBConnection">The database connection.</param>
        /// <returns></returns>
        public static Framework.ExecutionResult PingDB(IDbConnection DBConnection)
        {

            var er = new Framework.ExecutionResult();
            er.Context = "PingDB";



            var _DBConnection = (System.Data.Common.DbConnection)Activator.CreateInstance(DBConnection.GetType());
            _DBConnection.ConnectionString = DBConnection.ConnectionString;
            //_DBConnection.Credential = DBConnection.Credential;

            try
            {
                _DBConnection.Open();
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
                er.Exception = ex;
            }
            _DBConnection.Close();
            _DBConnection.Dispose();
            return er;
        }


        ///// <summary>
        ///// Gets the update SQL command.
        ///// </summary>
        ///// <param name="SQLQuery">The SQL query.</param>
        ///// <param name="SqlConnection">The SQL connection.</param>
        ///// <returns></returns>
        //public static string GetUpdateSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        //{

        //    var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
        //    var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
        //    return cmdbuilder.GetUpdateCommand().CommandText;

        //}

        ///// <summary>
        ///// Gets the update SQL command using only System.Data.Common interfaces where possible.
        ///// </summary>
        ///// <param name="SQLQuery">The SQL query.</param>
        ///// <param name="dbConnection">The database connection.</param>
        ///// <returns>The update command text.</returns>
        //public static string GetUpdateSqlCommand(string SQLQuery, DbConnection dbConnection)
        //{
        //    if (dbConnection == null)
        //        throw new ArgumentNullException(nameof(dbConnection));

        //    // Usa il tipo della connessione per determinare il provider e creare gli oggetti appropriati
        //    if (dbConnection is Microsoft.Data.SqlClient.SqlConnection sqlConn)
        //    {
        //        var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, sqlConn);
        //        using var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
        //        return cmdbuilder.GetUpdateCommand().CommandText;
        //    }
        //    // Aggiungi supporto per altri provider decommentando e installando i pacchetti NuGet appropriati
        //    // else if (dbConnection is MySql.Data.MySqlClient.MySqlConnection mySqlConn)
        //    // {
        //    //     var da = new MySql.Data.MySqlClient.MySqlDataAdapter(SQLQuery, mySqlConn);
        //    //     using var cmdbuilder = new MySql.Data.MySqlClient.MySqlCommandBuilder(da);
        //    //     return cmdbuilder.GetUpdateCommand().CommandText;
        //    // }
        //    // else if (dbConnection is Npgsql.NpgsqlConnection pgConn)
        //    // {
        //    //     var da = new Npgsql.NpgsqlDataAdapter(SQLQuery, pgConn);
        //    //     using var cmdbuilder = new Npgsql.NpgsqlCommandBuilder(da);
        //    //     return cmdbuilder.GetUpdateCommand().CommandText;
        //    // }
        //    // else if (dbConnection is Oracle.ManagedDataAccess.Client.OracleConnection oraConn)
        //    // {
        //    //     var da = new Oracle.ManagedDataAccess.Client.OracleDataAdapter(SQLQuery, oraConn);
        //    //     using var cmdbuilder = new Oracle.ManagedDataAccess.Client.OracleCommandBuilder(da);
        //    //     return cmdbuilder.GetUpdateCommand().CommandText;
        //    // }
        //    // else if (dbConnection is Microsoft.Data.Sqlite.SqliteConnection sqliteConn)
        //    // {
        //    //     var da = new Microsoft.Data.Sqlite.SqliteDataAdapter(SQLQuery, sqliteConn);
        //    //     using var cmdbuilder = new Microsoft.Data.Sqlite.SqliteCommandBuilder(da);
        //    //     return cmdbuilder.GetUpdateCommand().CommandText;
        //    // }
        //    else
        //    {
        //        throw new NotSupportedException($"Database provider for connection type '{dbConnection.GetType().FullName}' is not supported. Only SqlClient is implemented by default.");
        //    }
        //}


        ///// <summary>
        ///// Gets the INSERT SQL command for a model class.
        ///// </summary>
        ///// <param name="ModelClass">The model class type.</param>
        ///// <param name="dbConnection">The database connection (optional, used to determine dialect).</param>
        ///// <returns>The parameterized INSERT command.</returns>
        //public static string GetInsertSqlCommand(Type ModelClass, IDbConnection dbConnection = null)
        //{
        //    // Esclude le chiavi identity e explicit
        //    var properties = Utilities.GetModelPropertiesInfo(ModelClass, true)
        //        .Where(pi => !Utilities.PropertyIsIdentityKey(pi) && !Utilities.PropertyIsExplicitKey(pi))
        //        .ToList();

        //    var columns = string.Join(", ", properties.Select(pi => Utilities.GetMappedColumnName(pi)));
        //    var parameters = string.Join(", ", properties.Select(pi => $"@{pi.Name}"));

        //    return $"INSERT INTO {Utilities.GetModelTableName(ModelClass)} ({columns}) VALUES ({parameters})";
        //}

        ///// <summary>
        ///// Gets the INSERT SQL command from a SELECT query using CommandBuilder.
        ///// Only for Microsoft.Data.SqlClient connections.
        ///// </summary>
        //[Obsolete("Use overload with IDbConnection parameter instead")]
        //public static string xGetInsertSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        //{
        //    var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
        //    var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
        //    return cmdbuilder.GetInsertCommand().CommandText;
        //}


        ///// <summary>
        ///// Gets the delete SQL command.
        ///// </summary>
        ///// <param name="SQLQuery">The SQL query.</param>
        ///// <param name="SqlConnection">The SQL connection.</param>
        ///// <returns></returns>
        //public static string GetDeleteSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        //{

        //    var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
        //    var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
        //    return cmdbuilder.GetDeleteCommand().CommandText;

        //}

        public static string GetCreateDatabaseSql<TModel>(IDbConnection dbConnection, string databaseName = null)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            var providerFeatures = ProviderFeaturesResolver.FromConnection(dbConnection);
            var targetDatabaseName = string.IsNullOrWhiteSpace(databaseName)
                ? GetDatabaseNameFromConnectionString(dbConnection.ConnectionString)
                : databaseName;

            if (string.IsNullOrWhiteSpace(targetDatabaseName))
                return string.Empty;

            switch (providerFeatures.Dialect)
            {
                case DbDialect.SqlServer:
                    return $"IF DB_ID(N'{EscapeSqlLiteral(targetDatabaseName)}') IS NULL BEGIN CREATE DATABASE [{EscapeSqlIdentifier(targetDatabaseName)}] END";
                case DbDialect.MySql:
                case DbDialect.MariaDb:
                    return $"CREATE DATABASE IF NOT EXISTS `{EscapeSqlIdentifier(targetDatabaseName)}`";
                case DbDialect.PostgreSql:
                    return $"CREATE DATABASE \"{EscapeSqlIdentifier(targetDatabaseName)}\"";
                case DbDialect.SQLite:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        public static ExecutionResult CreateDatabaseIfNotExists<TModel>(IDbConnection dbConnection, string databaseName = null)
        {
            var er = new ExecutionResult($"{nameof(DataBaseHelper)}.CreateDatabaseIfNotExists<{typeof(TModel).Name}>");
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                var sql = GetCreateDatabaseSql<TModel>(dbConnection, databaseName);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    er.ResultCode = ExecutionResultCodes.Warning;
                    er.ResultMessage = "The database creation statement was not generated for the current provider.";
                    return er;
                }

                ExecuteNonQuery(dbConnection, sql);
                er.Value = sql;
                er.ResultCode = ExecutionResultCodes.Success;
                er.ResultMessage = $"Database creation statement executed for model '{typeof(TModel).Name}'.";
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.Exception = ex;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
            }

            return er;
        }

        public static string GetCreateSchemaSql<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            var providerFeatures = ProviderFeaturesResolver.FromConnection(dbConnection);
            var targetSchemaName = string.IsNullOrWhiteSpace(schemaName)
                ? ResolveSchemaName(typeof(TModel))
                : schemaName;

            if (string.IsNullOrWhiteSpace(targetSchemaName))
                return string.Empty;

            switch (providerFeatures.Dialect)
            {
                case DbDialect.SqlServer:
                    return $"IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'{EscapeSqlLiteral(targetSchemaName)}') EXEC('CREATE SCHEMA {providerFeatures.QuoteIdentifier(targetSchemaName)}')";
                case DbDialect.PostgreSql:
                    return $"CREATE SCHEMA IF NOT EXISTS {providerFeatures.QuoteIdentifier(targetSchemaName)}";
                case DbDialect.MySql:
                case DbDialect.MariaDb:
                case DbDialect.SQLite:
                default:
                    return string.Empty;
            }
        }

        public static ExecutionResult CreateSchemaIfNotExists<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            var er = new ExecutionResult($"{nameof(DataBaseHelper)}.CreateSchemaIfNotExists<{typeof(TModel).Name}>");
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                var sql = GetCreateSchemaSql<TModel>(dbConnection, schemaName);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    er.ResultCode = ExecutionResultCodes.Warning;
                    er.ResultMessage = "The schema creation statement was not generated for the current provider.";
                    return er;
                }

                ExecuteNonQuery(dbConnection, sql);
                er.Value = sql;
                er.ResultCode = ExecutionResultCodes.Success;
                er.ResultMessage = $"Schema creation statement executed for model '{typeof(TModel).Name}'.";
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.Exception = ex;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
            }

            return er;
        }

        public static string GetCreateTableSql<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            var providerFeatures = ProviderFeaturesResolver.FromConnection(dbConnection);
            var tableAttribute = typeof(TModel).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            var tableName = Utilities.GetModelTableName(typeof(TModel));
            var qualifiedTableName = GetQualifiedTableName(tableName, schemaName ?? tableAttribute?.Schema, providerFeatures);
            var columnDefinitions = BuildColumnDefinitions<TModel>(providerFeatures);

            if (columnDefinitions.Count == 0)
                return string.Empty;

            switch (providerFeatures.Dialect)
            {
                case DbDialect.SqlServer:
                    return $"IF OBJECT_ID(N'{EscapeSqlLiteral(qualifiedTableName)}', N'U') IS NULL BEGIN CREATE TABLE {qualifiedTableName} ({string.Join(", ", columnDefinitions)}) END";
                case DbDialect.PostgreSql:
                    return $"CREATE TABLE IF NOT EXISTS {qualifiedTableName} ({string.Join(", ", columnDefinitions)})";
                case DbDialect.MySql:
                case DbDialect.MariaDb:
                case DbDialect.SQLite:
                    return $"CREATE TABLE IF NOT EXISTS {qualifiedTableName} ({string.Join(", ", columnDefinitions)})";
                default:
                    return $"CREATE TABLE {qualifiedTableName} ({string.Join(", ", columnDefinitions)})";
            }
        }

        public static ExecutionResult CreateTableIfNotExists<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            var er = new ExecutionResult($"{nameof(DataBaseHelper)}.CreateTableIfNotExists<{typeof(TModel).Name}>");
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                var providerFeatures = ProviderFeaturesResolver.FromConnection(dbConnection);
                var tableAttribute = typeof(TModel).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
                var tableName = Utilities.GetModelTableName(typeof(TModel));
                var qualifiedTableName = GetQualifiedTableName(tableName, schemaName ?? tableAttribute?.Schema, providerFeatures);
                var columnDefinitions = BuildColumnDefinitions<TModel>(providerFeatures);
                var primaryKeyColumns = GetPrimaryKeyColumnNames(typeof(TModel));

                if (columnDefinitions.Count == 0)
                {
                    er.ResultCode = ExecutionResultCodes.Warning;
                    er.ResultMessage = $"No columns were discovered for model '{typeof(TModel).Name}'.";
                    return er;
                }

                var sql = GetCreateTableSql<TModel>(dbConnection, schemaName);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    er.ResultCode = ExecutionResultCodes.Warning;
                    er.ResultMessage = $"The table creation statement was not generated for model '{typeof(TModel).Name}'.";
                    return er;
                }

                ExecuteNonQuery(dbConnection, sql);

                if (primaryKeyColumns.Count > 0)
                {
                    var primaryKeySql = GetPrimaryKeyConstraintSql<TModel>(dbConnection, schemaName);
                    if (!string.IsNullOrWhiteSpace(primaryKeySql))
                    {
                        ExecuteNonQuery(dbConnection, primaryKeySql);
                    }
                }

                er.Value = sql;
                er.ResultCode = ExecutionResultCodes.Success;
                er.ResultMessage = $"Table '{tableName}' is ready.";
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.Exception = ex;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
            }

            return er;
        }

        public static string GetCreatePrimaryKeyIndexSql<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            var providerFeatures = ProviderFeaturesResolver.FromConnection(dbConnection);
            var tableAttribute = typeof(TModel).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            var tableName = Utilities.GetModelTableName(typeof(TModel));
            var qualifiedTableName = GetQualifiedTableName(tableName, schemaName ?? tableAttribute?.Schema, providerFeatures);
            var primaryKeyColumns = GetPrimaryKeyColumnNames(typeof(TModel));

            if (primaryKeyColumns.Count == 0)
                return string.Empty;

            var columnList = string.Join(", ", primaryKeyColumns.Select(x => providerFeatures.QuoteIdentifier(x)));
            var indexName = providerFeatures.QuoteIdentifier($"IX_{tableName}_{string.Join("_", primaryKeyColumns)}");

            switch (providerFeatures.Dialect)
            {
                case DbDialect.SqlServer:
                    return $"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = '{EscapeSqlLiteral($"IX_{tableName}_{string.Join("_", primaryKeyColumns)}")}' AND object_id = OBJECT_ID('{EscapeSqlLiteral(tableName)}')) CREATE UNIQUE INDEX {indexName} ON {qualifiedTableName} ({columnList})";
                case DbDialect.PostgreSql:
                case DbDialect.MySql:
                case DbDialect.MariaDb:
                case DbDialect.SQLite:
                    return $"CREATE UNIQUE INDEX IF NOT EXISTS {indexName} ON {qualifiedTableName} ({columnList})";
                default:
                    return $"CREATE UNIQUE INDEX {indexName} ON {qualifiedTableName} ({columnList})";
            }
        }

        public static ExecutionResult CreatePrimaryKeyIndexesIfNotExists<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            var er = new ExecutionResult($"{nameof(DataBaseHelper)}.CreatePrimaryKeyIndexesIfNotExists<{typeof(TModel).Name}>");
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                var sql = GetCreatePrimaryKeyIndexSql<TModel>(dbConnection, schemaName);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    er.ResultCode = ExecutionResultCodes.Warning;
                    er.ResultMessage = $"No primary key columns were discovered for model '{typeof(TModel).Name}'.";
                    return er;
                }

                ExecuteNonQuery(dbConnection, sql);
                er.Value = sql;
                er.ResultCode = ExecutionResultCodes.Success;
                er.ResultMessage = $"Primary key index creation statement executed for model '{typeof(TModel).Name}'.";
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.Exception = ex;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
            }

            return er;
        }

        public static List<string> GetCreateSchemaStatementsForModel<TModel>(IDbConnection dbConnection, string databaseName = null, string schemaName = null, bool includeCreateDatabase = true, bool includeCreateSchema = true, bool includeCreateTable = true, bool includeCreatePrimaryKeyIndexes = true)
        {
            var statements = new List<string>();
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            if (includeCreateDatabase)
            {
                var databaseSql = GetCreateDatabaseSql<TModel>(dbConnection, databaseName);
                if (!string.IsNullOrWhiteSpace(databaseSql))
                    statements.Add(databaseSql);
            }

            if (includeCreateSchema)
            {
                var schemaSql = GetCreateSchemaSql<TModel>(dbConnection, schemaName);
                if (!string.IsNullOrWhiteSpace(schemaSql))
                    statements.Add(schemaSql);
            }

            if (includeCreateTable)
            {
                var tableSql = GetCreateTableSql<TModel>(dbConnection, schemaName);
                if (!string.IsNullOrWhiteSpace(tableSql))
                    statements.Add(tableSql);
            }

            if (includeCreatePrimaryKeyIndexes)
            {
                var indexSql = GetCreatePrimaryKeyIndexSql<TModel>(dbConnection, schemaName);
                if (!string.IsNullOrWhiteSpace(indexSql))
                    statements.Add(indexSql);
            }

            return statements;
        }

        public static ExecutionResult CreateSchemaForModel<TModel>(IDbConnection dbConnection, string databaseName = null, bool createDatabaseIfNotExists = true, bool createTableIfNotExists = true, bool createPrimaryKeyIndexesIfNotExists = true)
        {
            return CreateSchemaForModel<TModel>(dbConnection, databaseName, null, createDatabaseIfNotExists, true, createTableIfNotExists, createPrimaryKeyIndexesIfNotExists);
        }

        public static ExecutionResult CreateSchemaForModel<TModel>(IDbConnection dbConnection, string databaseName, string schemaName, bool createDatabaseIfNotExists = true, bool createSchemaIfNotExists = true, bool createTableIfNotExists = true, bool createPrimaryKeyIndexesIfNotExists = true)
        {
            var er = new ExecutionResult($"{nameof(DataBaseHelper)}.CreateSchemaForModel<{typeof(TModel).Name}>");
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            try
            {
                var statements = new List<string>();

                if (createDatabaseIfNotExists)
                {
                    var databaseSql = GetCreateDatabaseSql<TModel>(dbConnection, databaseName);
                    if (!string.IsNullOrWhiteSpace(databaseSql))
                        statements.Add(databaseSql);
                }

                if (createSchemaIfNotExists)
                {
                    var schemaSql = GetCreateSchemaSql<TModel>(dbConnection, schemaName);
                    if (!string.IsNullOrWhiteSpace(schemaSql))
                        statements.Add(schemaSql);
                }

                if (createTableIfNotExists)
                {
                    var tableSql = GetCreateTableSql<TModel>(dbConnection, schemaName);
                    if (!string.IsNullOrWhiteSpace(tableSql))
                        statements.Add(tableSql);
                }

                if (createPrimaryKeyIndexesIfNotExists)
                {
                    var indexSql = GetCreatePrimaryKeyIndexSql<TModel>(dbConnection, schemaName);
                    if (!string.IsNullOrWhiteSpace(indexSql))
                        statements.Add(indexSql);
                }

                if (statements.Count == 0)
                {
                    er.ResultCode = ExecutionResultCodes.Warning;
                    er.ResultMessage = $"No DDL statements were generated for model '{typeof(TModel).Name}'.";
                    return er;
                }

                foreach (var sql in statements)
                {
                    ExecuteNonQuery(dbConnection, sql);
                }

                er.Value = statements;
                er.ResultCode = ExecutionResultCodes.Success;
                er.ResultMessage = $"Schema creation statements executed for model '{typeof(TModel).Name}'.";
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.Exception = ex;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
            }

            return er;
        }

        private static void ExecuteNonQuery(IDbConnection dbConnection, string sql)
        {
            using (IDbCommand command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        private static string GetDatabaseNameFromConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return string.Empty;

            try
            {
                var builder = new DbConnectionStringBuilder();
                builder.ConnectionString = connectionString;

                foreach (var key in new[] { "Initial Catalog", "Database", "DatabaseName", "DbName" })
                {
                    if (builder.TryGetValue(key, out object value) && value != null)
                        return value.ToString();
                }
            }
            catch
            {
                // Ignore parsing issues and fall back to empty.
            }

            return string.Empty;
        }

        private static string EscapeSqlIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return string.Empty;

            return identifier.Replace("\"", "\"\"").Replace("`", "``").Replace("[", "[[").Replace("]", "]]" );
        }

        private static string EscapeSqlLiteral(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Replace("'", "''");
        }

        private static string GetQualifiedTableName(string tableName, string schemaName, ProviderFeatures providerFeatures)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                return providerFeatures.QuoteIdentifier(tableName);

            return providerFeatures.QuoteQualifiedIdentifier($"{schemaName}.{tableName}");
        }

        private static List<string> BuildColumnDefinitions<TModel>(ProviderFeatures providerFeatures)
        {
            var definitions = new List<string>();
            var properties = typeof(TModel)
                .GetProperties()
                .Where(p => ShouldCreateColumn(p))
                .ToList();

            foreach (var property in properties)
            {
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                var columnName = !string.IsNullOrWhiteSpace(columnAttribute?.Name)
                    ? columnAttribute.Name
                    : Utilities.GetMappedColumnName(property);

                var quotedColumnName = providerFeatures.QuoteIdentifier(columnName);
                var columnType = GetColumnType(property.PropertyType, providerFeatures, columnAttribute?.TypeName);
                var isPrimaryKey = IsPrimaryKeyProperty(property);
                var identityClause = GetIdentityColumnClause(property, providerFeatures, columnType);
                var nullability = isPrimaryKey ? "NOT NULL" : "NULL";

                var definition = string.IsNullOrWhiteSpace(identityClause)
                    ? $"{quotedColumnName} {columnType}"
                    : $"{quotedColumnName} {identityClause}";

                if (isPrimaryKey && string.IsNullOrWhiteSpace(identityClause))
                {
                    definition = $"{definition} {nullability}";
                }
                else if (isPrimaryKey && !string.IsNullOrWhiteSpace(identityClause) && providerFeatures.Dialect != DbDialect.SQLite)
                {
                    definition = $"{definition} {nullability}";
                }

                definitions.Add(definition);
            }

            return definitions;
        }

        private static bool ShouldCreateColumn(PropertyInfo property)
        {
            if (property == null)
                return false;

            if (property.GetCustomAttribute<NotMappedAttribute>() != null)
                return false;

            if (property.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() != null)
                return false;

            var writeAttribute = property.GetCustomAttribute<Dapper.Contrib.Extensions.WriteAttribute>();
            if (writeAttribute != null && !writeAttribute.Write)
                return false;

            return property.CanWrite;
        }

        private static bool IsPrimaryKeyProperty(PropertyInfo property)
        {
            return Utilities.PropertyIsExplicitKey(property) || Utilities.PropertyIsIdentityKey(property);
        }

        private static bool UsesIdentityGeneration(PropertyInfo property)
        {
            if (property == null)
                return false;

            var generatedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (generatedAttribute != null)
                return generatedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            return IsSupportedIdentityType(targetType) && IsPrimaryKeyProperty(property);
        }

        private static bool IsSupportedIdentityType(Type propertyType)
        {
            if (propertyType == typeof(byte) || propertyType == typeof(short) || propertyType == typeof(int) || propertyType == typeof(long))
                return true;

            return false;
        }

        private static string GetIdentityColumnClause(PropertyInfo property, ProviderFeatures providerFeatures, string baseColumnType)
        {
            if (!UsesIdentityGeneration(property))
                return string.Empty;

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            switch (providerFeatures.Dialect)
            {
                case DbDialect.SqlServer:
                    if (targetType == typeof(byte)) return "tinyint IDENTITY(1,1)";
                    if (targetType == typeof(short)) return "smallint IDENTITY(1,1)";
                    if (targetType == typeof(int)) return "int IDENTITY(1,1)";
                    if (targetType == typeof(long)) return "bigint IDENTITY(1,1)";
                    break;
                case DbDialect.PostgreSql:
                    if (targetType == typeof(byte)) return "smallserial";
                    if (targetType == typeof(short)) return "smallserial";
                    if (targetType == typeof(int)) return "serial";
                    if (targetType == typeof(long)) return "bigserial";
                    break;
                case DbDialect.MySql:
                case DbDialect.MariaDb:
                    if (targetType == typeof(byte) || targetType == typeof(short) || targetType == typeof(int) || targetType == typeof(long))
                        return $"{baseColumnType} AUTO_INCREMENT";
                    break;
                case DbDialect.SQLite:
                    return "INTEGER PRIMARY KEY AUTOINCREMENT";
            }

            return string.Empty;
        }

        private static List<string> GetPrimaryKeyColumnNames(Type modelType)
        {
            return Utilities.GetModelPrimaryKeysPropertiesInfo(modelType)
                .Select(p => p.Name)
                .ToList();
        }

        private static string ResolveSchemaName(Type modelType)
        {
            if (modelType == null)
                return string.Empty;

            var tableAttribute = modelType.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            return tableAttribute?.Schema;
        }

        private static string GetPrimaryKeyConstraintSql<TModel>(IDbConnection dbConnection, string schemaName = null)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            var providerFeatures = ProviderFeaturesResolver.FromConnection(dbConnection);
            var tableAttribute = typeof(TModel).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            var tableName = Utilities.GetModelTableName(typeof(TModel));
            var qualifiedTableName = GetQualifiedTableName(tableName, schemaName ?? tableAttribute?.Schema, providerFeatures);
            var primaryKeyColumns = GetPrimaryKeyColumnNames(typeof(TModel));
            if (primaryKeyColumns.Count == 0)
                return string.Empty;

            var pkConstraintClause = string.Join(", ", primaryKeyColumns.Select(x => providerFeatures.QuoteIdentifier(x)));
            var pkConstraintName = providerFeatures.QuoteIdentifier($"PK_{tableName}");

            switch (providerFeatures.Dialect)
            {
                case DbDialect.SqlServer:
                    return $"IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = '{EscapeSqlLiteral($"{tableName}_PK")}' AND parent_object_id = OBJECT_ID('{EscapeSqlLiteral(tableName)}')) ALTER TABLE {qualifiedTableName} ADD CONSTRAINT {pkConstraintName} PRIMARY KEY ({pkConstraintClause})";
                case DbDialect.PostgreSql:
                    return $"ALTER TABLE {qualifiedTableName} ADD CONSTRAINT {pkConstraintName} PRIMARY KEY ({pkConstraintClause})";
                case DbDialect.MySql:
                case DbDialect.MariaDb:
                    return $"ALTER TABLE {qualifiedTableName} ADD PRIMARY KEY ({pkConstraintClause})";
                case DbDialect.SQLite:
                    return string.Empty;
                default:
                    return $"ALTER TABLE {qualifiedTableName} ADD CONSTRAINT {pkConstraintName} PRIMARY KEY ({pkConstraintClause})";
            }
        }

        private static string GetColumnType(Type propertyType, ProviderFeatures providerFeatures, string explicitTypeName)
        {
            if (!string.IsNullOrWhiteSpace(explicitTypeName))
                return explicitTypeName;

            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (targetType.IsEnum)
                return "int";

            if (targetType == typeof(bool))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "bit" : "boolean";

            if (targetType == typeof(byte))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "tinyint" : "smallint";

            if (targetType == typeof(short))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "smallint" : "smallint";

            if (targetType == typeof(int))
                return "int";

            if (targetType == typeof(long))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "bigint" : "bigint";

            if (targetType == typeof(decimal))
                return "decimal(19,4)";

            if (targetType == typeof(double))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "float" : "double precision";

            if (targetType == typeof(float))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "real" : "real";

            if (targetType == typeof(DateTime))
                return providerFeatures.Dialect == DbDialect.SqlServer ? "datetime2" : "timestamp";

            if (targetType == typeof(Guid))
            {
                switch (providerFeatures.Dialect)
                {
                    case DbDialect.PostgreSql:
                        return "uuid";
                    case DbDialect.MySql:
                    case DbDialect.MariaDb:
                        return "char(36)";
                    case DbDialect.SQLite:
                        return "text";
                    default:
                        return "uniqueidentifier";
                }
            }

            if (targetType == typeof(byte[]))
            {
                switch (providerFeatures.Dialect)
                {
                    case DbDialect.PostgreSql:
                        return "bytea";
                    case DbDialect.SQLite:
                        return "blob";
                    default:
                        return "varbinary(max)";
                }
            }

            if (targetType == typeof(string))
            {
                switch (providerFeatures.Dialect)
                {
                    case DbDialect.PostgreSql:
                        return "text";
                    case DbDialect.MySql:
                    case DbDialect.MariaDb:
                        return "longtext";
                    case DbDialect.SQLite:
                        return "text";
                    default:
                        return "nvarchar(max)";
                }
            }

            return "nvarchar(max)";
        }

    }

}
