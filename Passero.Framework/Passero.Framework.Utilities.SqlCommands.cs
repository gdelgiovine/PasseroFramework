using Dapper;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Passero.Framework
{
    public static partial class Utilities
    {


        /// <summary>
        /// Gets the INSERT SQL command for a model class.
        /// </summary>
        /// <param name="ModelClass">The model class type.</param>
        /// <returns>The parameterized INSERT command.</returns>
        /// 
        public static string GetDefaultOrderByClause(List<System.Reflection.PropertyInfo> EntityPrimaryKeys, ProviderFeatures ProviderFeatures = null)
        {
            if (EntityPrimaryKeys == null || EntityPrimaryKeys.Count == 0)
            {
                return string.Empty;
            }

            var orderByClause = string.Join(", ", EntityPrimaryKeys.Select(pi => QuoteColumnName(pi, ProviderFeatures)));
            return orderByClause;
        }

        public static string GetDefaultOrderByClause_OLD(List<System.Reflection.PropertyInfo> EntityPrimaryKeys, ProviderFeatures ProviderFeatures=null)
        {
            if (EntityPrimaryKeys == null || EntityPrimaryKeys.Count == 0)
            {
                return string.Empty;
            }
            
            
            var orderByClause = string.Join(", ", EntityPrimaryKeys.Select(pi => $"{GetMappedColumnName(pi)}"));
            return orderByClause;
        }



        /// <summary>
        /// Gets the UPDATE SQL command for a model class.
        /// </summary>
        /// <param name="ModelClass">The model class type.</param>
        /// <returns>The parameterized UPDATE command.</returns>


        public static string GetUpdateSqlCommand(Type ModelClass, ProviderFeatures ProviderFeatures)
        {
            List<PropertyInfo> properties;
            StringBuilder sbset = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string setlist;
            string wherelist;
            string sql;

            var tableName = QuoteTableName(ModelClass, ProviderFeatures);
            var parameterPrefix = ProviderFeatures?.ParameterPrefix ?? "@";

            properties = Utilities.GetModelPropertiesInfo(ModelClass, true);
            foreach (PropertyInfo pi in properties)
            {
                var colName = QuoteColumnName(pi, ProviderFeatures);
                if (!Utilities.PropertyIsIdentityKey(pi))
                {
                    sbset.Append($"{colName}={parameterPrefix}{pi.Name}, ");
                }
                if (Utilities.PropertyIsExplicitKey(pi) || Utilities.PropertyIsIdentityKey(pi))
                {
                    sbwhere.Append($"{colName}={parameterPrefix}{pi.Name}_shadow AND ");
                }
            }

            setlist = sbset.ToString().Trim();
            wherelist = sbwhere.ToString().Trim();

            if (setlist.EndsWith(","))
            {
                setlist = setlist.Substring(0, setlist.Length - 1);
            }

            if (wherelist.EndsWith("AND"))
            {
                wherelist = wherelist.Substring(0, wherelist.Length - 3);
            }

            sql = $"UPDATE {tableName} SET {setlist} WHERE ({wherelist})";
            return sql;
        }



        public static string GetUpdateSqlCommand_OLD(Type ModelClass, ProviderFeatures ProviderFeatures )
        {
            List<PropertyInfo> properties;
            StringBuilder sbset = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string setlist;
            string wherelist;
            string sql;
            properties = Utilities.GetModelPropertiesInfo(ModelClass, true);
            foreach (PropertyInfo pi in properties)
            {
                var colName = GetMappedColumnName(pi);
                if (!Utilities.PropertyIsIdentityKey(pi))
                {
                    sbset.Append($"{colName}={ProviderFeatures.ParameterPrefix}{pi.Name}, ");
                }
                if (Utilities.PropertyIsExplicitKey(pi) || Utilities.PropertyIsIdentityKey(pi))
                {
                    sbwhere.Append($"{colName}={ProviderFeatures .ParameterPrefix}{pi.Name}_shadow AND ");
                }
            }
            setlist = sbset.ToString().Trim();
            wherelist = sbwhere.ToString().Trim();
            if (setlist.EndsWith(","))
            {
                setlist = setlist.Substring(0, setlist.Length - 1);
            }
            if (wherelist.EndsWith("AND"))
            {
                wherelist = wherelist.Substring(0, wherelist.Length - 3);
            }
            sql = $"UPDATE {Utilities.GetModelTableName(ModelClass)} SET {setlist} WHERE ({wherelist})";
            return sql;
        }



        /// <summary>
        /// Gets the INSERT SQL command for a model class.
        /// </summary>
        /// <param name="ModelClass">The model class type.</param>
        /// <returns>The parameterized INSERT command.</returns>
        /// 
        public static string GetInsertSqlCommand(Type ModelClass, ProviderFeatures ProviderFeatures)
        {
            var parameterPrefix = ProviderFeatures?.ParameterPrefix ?? "@";

            var properties = Utilities.GetModelPropertiesInfo(ModelClass, true)
                .Where(pi => !Utilities.PropertyIsIdentityKey(pi))
                .ToList();

            var columns = string.Join(", ", properties.Select(pi => QuoteColumnName(pi, ProviderFeatures)));
            var parameters = string.Join(", ", properties.Select(pi => $"{parameterPrefix}{pi.Name}"));

            return $"INSERT INTO {QuoteTableName(ModelClass, ProviderFeatures)} ({columns}) VALUES ({parameters})";
        }
        public static string GetInsertSqlCommand_OLD(Type ModelClass, ProviderFeatures ProviderFeatures)
        {
            //// Esclude le chiavi identity e explicit
            //var properties = Utilities.GetModelPropertiesInfo(ModelClass, true)
            //    .Where(pi => !Utilities.PropertyIsIdentityKey(pi) && !Utilities.PropertyIsExplicitKey(pi))
            //    .ToList();
            // Esclude SOLO le chiavi identity, mantiene le chiavi esplicite
            var properties = Utilities.GetModelPropertiesInfo(ModelClass, true)
                .Where(pi => !Utilities.PropertyIsIdentityKey(pi))
                .ToList();

            
            var columns = string.Join(", ", properties.Select(pi => Utilities.GetMappedColumnName(pi)));
            var parameters = string.Join(", ", properties.Select(pi => $"{ProviderFeatures .ParameterPrefix}{pi.Name}"));

            return $"INSERT INTO {Utilities.GetModelTableName(ModelClass)} ({columns}) VALUES ({parameters})";
        }


        /// <summary>
        /// Gets the DELETE SQL command for a model class.
        /// </summary>
        /// <param name="ModelClass">The model class type.</param>
        /// <returns>The parameterized DELETE command.</returns>

        public static string GetDeleteSqlCommand(Type ModelClass, ProviderFeatures ProviderFeatures)
        {
            var parameterPrefix = ProviderFeatures?.ParameterPrefix ?? "@";

            var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(ModelClass);
            var where = string.Join(" AND ", keys.Select(pi => $"{QuoteColumnName(pi, ProviderFeatures)}={parameterPrefix}{pi.Name}"));
            return $"DELETE FROM {QuoteTableName(ModelClass, ProviderFeatures)} WHERE ({where})";
        }

        private static string QuoteColumnName(PropertyInfo propertyInfo, ProviderFeatures providerFeatures)
        {
            var columnName = GetMappedColumnName(propertyInfo);
            return providerFeatures?.QuoteIdentifier(columnName) ?? columnName;
        }

        private static string QuoteTableName(Type modelClass, ProviderFeatures providerFeatures)
        {
            var tableName = Utilities.GetModelTableName(modelClass);
            return providerFeatures?.QuoteQualifiedIdentifier(tableName) ?? tableName;
        }

        public static string GetDeleteSqlCommand_OLD(Type ModelClass,ProviderFeatures ProviderFeatures )
        {
            var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(ModelClass);
            var where = string.Join(" AND ", keys.Select(pi => $"{GetMappedColumnName(pi)}={ProviderFeatures .ParameterPrefix}{pi.Name}"));
            return $"DELETE FROM {Utilities.GetModelTableName(ModelClass)} WHERE ({where})";
        }


        public static string GetInsertSqlCommandEx(string sqlQuery, IDbConnection connection)
        {
            using DbCommand command = BuildCommand(sqlQuery, connection, builder =>
                builder.GetInsertCommand());

            return command.CommandText;
        }

        public static string GetUpdateSqlCommandEx(string sqlQuery, IDbConnection connection)
        {
            using DbCommand command = BuildCommand(sqlQuery, connection, builder =>
                builder.GetUpdateCommand());

            return command.CommandText;
        }

        public static string GetDeleteSqlCommandEx(string sqlQuery, IDbConnection connection)
        {
            using DbCommand command = BuildCommand(sqlQuery, connection, builder =>
                builder.GetDeleteCommand());

            return command.CommandText;
        }

        private static DbCommand BuildCommand(
            string sqlQuery,
            IDbConnection connection,
            Func<DbCommandBuilder, DbCommand> commandFactory)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentException("La query SQL non può essere vuota.", nameof(sqlQuery));

            if (connection is not DbConnection dbConnection)
                throw new NotSupportedException(
                    "La connessione deve derivare da System.Data.Common.DbConnection.");

            DbProviderFactory factory = DbProviderFactories.GetFactory(dbConnection);

            using DbCommand selectCommand = dbConnection.CreateCommand();
            selectCommand.CommandText = sqlQuery;

            using DbDataAdapter adapter = factory.CreateDataAdapter()
                ?? throw new NotSupportedException("Il provider non supporta DbDataAdapter.");

            adapter.SelectCommand = selectCommand;

            using DbCommandBuilder builder = factory.CreateCommandBuilder()
                ?? throw new NotSupportedException("Il provider non supporta DbCommandBuilder.");

            builder.DataAdapter = adapter;

            return commandFactory(builder);
        }




        public static DataTable GetDataTableFromDapperQuery(IDbConnection DbConnection, string SQLQuery, DynamicParameters? Parameters = null)
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        {
            DataTable dataTable = new DataTable();
            IDataReader dataReader;
            dataReader = DbConnection.ExecuteReader(SQLQuery, Parameters);
            dataTable.Load(dataReader);
            return dataTable;
        }


        public static List<IDictionary<string, object>> DapperSelect(IDbConnection dbConnection, string query, object parameters)
        {

            {
                var result = dbConnection.Query(query, parameters).ToList();

                return result.Select(x => (IDictionary<string, object>)x).ToList();
            }
        }


        public static string ResolveSQL_OLD(string SQL, Dapper.DynamicParameters Parameters)
        {
            string _SQL = SQL;

            if (_SQL.Trim() == "" | _SQL == null)
                return "";

            if (Parameters == null)
                return _SQL;


            foreach (var Name in Parameters.ParameterNames)
            {
                var value = Parameters.Get<dynamic>(Name);
                string ParamName = "@" + Name;
                string ParamValue = "";
                if (value is not null)
                {
                    if (
                        value.GetType() == typeof(string) |
                        value.GetType() == typeof(DateTime)

                        )
                    {

                        ParamValue = $"'{value.ToString()}'";
                    }
                    else
                    {
                        ParamValue = $"{value.ToString()}";
                    }
                    _SQL = Regex.Replace(_SQL, ParamName, ParamValue, RegexOptions.IgnoreCase);
                }
            }

            return _SQL;
        }


        public static string ResolveSQL(string SQL, Dapper.DynamicParameters Parameters, ProviderFeatures providerFeatures)
        {
            if (string.IsNullOrWhiteSpace(SQL))
            {
                return string.Empty;
            }

            if (Parameters == null)
            {
                return SQL;
            }

            providerFeatures ??= new ProviderFeatures();

            string resolvedSql = SQL;
            string parameterPrefix = Regex.Escape(providerFeatures.ParameterPrefix);

            foreach (var name in Parameters.ParameterNames)
            {
                var normalizedName = providerFeatures.NormalizeParameterName(name);
                var value = Parameters.Get<dynamic>(name);
                var parameterPattern = $@"(?<!\w){parameterPrefix}{Regex.Escape(normalizedName)}(?!\w)";
                var literalValue = ToSqlLiteral(value, providerFeatures);

                resolvedSql = Regex.Replace(
                    resolvedSql,
                    parameterPattern,
                    literalValue,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            return resolvedSql;
        }

        //public static string ResolveSQL(string SQL, Dapper.DynamicParameters Parameters, ProviderFeatures ProviderFeatures )
        //{
        //    return ResolveSQL(SQL, Parameters, ProviderFeatures );
        //}

        private static string ToSqlLiteral(object value, ProviderFeatures providerFeatures)
        {
            if (value == null || value is DBNull)
            {
                return "NULL";
            }

            return value switch
            {
                string stringValue => ToSqlStringLiteral(stringValue, providerFeatures),
                char charValue => ToSqlStringLiteral(charValue.ToString(), providerFeatures),
                bool boolValue => ToSqlBooleanLiteral(boolValue, providerFeatures),
                DateTime dateTime => ToSqlDateTimeLiteral(dateTime, providerFeatures),
                DateTimeOffset dateTimeOffset => ToSqlDateTimeOffsetLiteral(dateTimeOffset, providerFeatures),
                Guid guidValue => $"'{guidValue:D}'",
                byte[] bytes => ToSqlBinaryLiteral(bytes),
                Enum enumValue => Convert.ToInt64(enumValue, System.Globalization.CultureInfo.InvariantCulture)
                    .ToString(System.Globalization.CultureInfo.InvariantCulture),
                IFormattable formattable => formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
                _ => ToSqlStringLiteral(value.ToString() ?? string.Empty, providerFeatures)
            };
        }

        private static string ToSqlStringLiteral(string value, ProviderFeatures providerFeatures)
        {
            string escaped = value.Replace("'", "''");

            return providerFeatures.Dialect == DbDialect.SqlServer || providerFeatures.Dialect == DbDialect.SqlServerCe
                ? $"N'{escaped}'"
                : $"'{escaped}'";
        }

        private static string ToSqlBooleanLiteral(bool value, ProviderFeatures providerFeatures)
        {
            return providerFeatures.Dialect switch
            {
                DbDialect.PostgreSql => value ? "TRUE" : "FALSE",
                _ => value ? "1" : "0"
            };
        }

        private static string ToSqlDateTimeLiteral(DateTime value, ProviderFeatures providerFeatures)
        {
            return providerFeatures.Dialect switch
            {
                DbDialect.Oracle =>
                    $"TO_TIMESTAMP('{value:yyyy-MM-dd HH:mm:ss.fffffff}', 'YYYY-MM-DD HH24:MI:SS.FF7')",

                DbDialect.PostgreSql =>
                    $"'{value:yyyy-MM-dd HH:mm:ss.fffffff}'::timestamp",

                _ =>
                    $"'{value:yyyy-MM-dd HH:mm:ss.fffffff}'"
            };
        }

        private static string ToSqlDateTimeOffsetLiteral(DateTimeOffset value, ProviderFeatures providerFeatures)
        {
            return providerFeatures.Dialect switch
            {
                DbDialect.Oracle =>
                    $"TO_TIMESTAMP_TZ('{value:yyyy-MM-dd HH:mm:ss.fffffff zzz}', 'YYYY-MM-DD HH24:MI:SS.FF7 TZH:TZM')",

                DbDialect.PostgreSql =>
                    $"'{value:yyyy-MM-dd HH:mm:ss.fffffff zzz}'::timestamptz",

                _ =>
                    $"'{value:yyyy-MM-dd HH:mm:ss.fffffff zzz}'"
            };
        }

        private static string ToSqlBinaryLiteral(byte[] bytes)
        {
            return "0x" + BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        public static string GetInsertSqlCommand<T>(Passero.Framework.Base.DbObject<T> dbObject) where T : class
{
    if (dbObject == null)
        throw new ArgumentNullException(nameof(dbObject));

    var properties = Utilities.GetModelPropertiesInfo(typeof(T), true)
        .Where(pi => !Utilities.PropertyIsIdentityKey(pi))
        .ToList();

    var columns = string.Join(", ", properties.Select(pi => dbObject.ResolveColumnName(pi)));
    var parameters = string.Join(", ", properties.Select(pi => $"@{pi.Name}"));

    return $"INSERT INTO {dbObject.GetTableName()} ({columns}) VALUES ({parameters})";
}

public static string GetUpdateSqlCommand<T>(Passero.Framework.Base.DbObject<T> dbObject) where T : class
{
    if (dbObject == null)
        throw new ArgumentNullException(nameof(dbObject));

    var properties = Utilities.GetModelPropertiesInfo(typeof(T), true);
    var setClause = string.Join(", ",
        properties.Where(pi => !Utilities.PropertyIsIdentityKey(pi))
                  .Select(pi => $"{dbObject.ResolveColumnName(pi)}=@{pi.Name}"));

    var whereClause = string.Join(" AND ",
        properties.Where(pi => Utilities.PropertyIsExplicitKey(pi) || Utilities.PropertyIsIdentityKey(pi))
                  .Select(pi => $"{dbObject.ResolveColumnName(pi)}=@{pi.Name}_shadow"));

    return $"UPDATE {dbObject.GetTableName()} SET {setClause} WHERE ({whereClause})";
}

public static string GetDeleteSqlCommand<T>(Passero.Framework.Base.DbObject<T> dbObject) where T : class
{
    if (dbObject == null)
        throw new ArgumentNullException(nameof(dbObject));

    var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(T));
    var whereClause = string.Join(" AND ",
        keys.Select(pi => $"{dbObject.ResolveColumnName(pi)}=@{pi.Name}"));

    return $"DELETE FROM {dbObject.GetTableName()} WHERE ({whereClause})";
}
    }
}