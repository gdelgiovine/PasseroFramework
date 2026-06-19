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
        public static string GetDefaultOrderByClause(List<System.Reflection.PropertyInfo> EntityPrimaryKeys, ProviderFeatures ProviderFeatures=null)
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


        public static string GetUpdateSqlCommand(Type ModelClass, ProviderFeatures ProviderFeatures )
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
        public static string GetInsertSqlCommand(Type ModelClass, ProviderFeatures ProviderFeatures)
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


        public static string GetDeleteSqlCommand(Type ModelClass,ProviderFeatures ProviderFeatures )
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


        public static string ResolveSQL(string SQL, Dapper.DynamicParameters Parameters)
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

    }
}