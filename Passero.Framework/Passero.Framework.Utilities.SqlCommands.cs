using Dapper;
using Dapper.Contrib.Extensions;
using FastDeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using MiniExcelLibs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wisej.Core;
using Wisej.Web;

namespace Passero.Framework
{
    public static partial class Utilities
    {

        public static string GetUpdateSqlCommand(Type ModelClass)
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
                    sbset.Append($"{colName}=@{pi.Name}, ");
                }
                if (Utilities.PropertyIsExplicitKey(pi) ||  Utilities.PropertyIsIdentityKey(pi))
                {
                    sbwhere.Append($"{colName}=@{pi.Name}_shadow AND ");
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


        public static string GetUpdateSqlCommand_OLD(Type ModelClass)
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
                if (! Utilities.PropertyIsIdentityKey   (pi))
                {
                    sbset.Append($"{pi.Name}=@{pi.Name}, ");
                }
                if (Utilities.PropertyIsExplicitKey(pi) ||  PropertyIsIdentityKey(pi))
                {
                    sbwhere.Append($"{pi.Name}=@{pi.Name}_shadow AND ");
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


        public static string GetInsertSqlCommand(Type ModelClass)
        {
            var properties = Utilities.GetModelPropertiesInfo(ModelClass, true)
                .Where(pi => !Utilities.PropertyIsIdentityKey(pi))
                .ToList();

            var columns = string.Join(", ", properties.Select(pi => GetMappedColumnName(pi)));
            var parameters = string.Join(", ", properties.Select(pi => $"@{pi.Name}"));

            return $"INSERT INTO {  Utilities.GetModelTableName(ModelClass)} ({columns}) VALUES ({parameters}); SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";
        }


        public static string GetInsertSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetInsertCommand().CommandText;

        }


        public static string GetDeleteSqlCommand(Type ModelClass)
        {
            var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(ModelClass);
            var where = string.Join(" AND ", keys.Select(pi => $"{GetMappedColumnName(pi)}=@{pi.Name}"));
            return $"DELETE FROM {Utilities.GetModelTableName(ModelClass)} WHERE ({where})";
        }


        public static string GetDeleteSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetDeleteCommand().CommandText;

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