﻿using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.ReportingServices.Diagnostics.Internal;
using Microsoft.VisualBasic;
using MiniExcelLibs;
using Newtonsoft.Json.Linq;
using Passero.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Wisej.Web;


namespace Passero.Framework.DapperHelper
{

    public class Utilities
    {

        public static string GetUpdateSqlCommand(string SQLQuery, IDbConnection dbConnection )
        {

            System.Data.Common.DataAdapter da = DbProviderFactories.GetFactory((System.Data.Common .DbConnection)dbConnection).CreateDataAdapter();


            //var cmdbuilder = new System.Data.Common.DbCommandBuilder(da);

            //return cmdbuilder.GetUpdateCommand().CommandText;
            return "";
        }




        public static string GetInsertSqlCommand(string SQLQuery, System.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new System.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new System.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetInsertCommand().CommandText;

        }


        public static string GetDeleteSqlCommand(string SQLQuery, System.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new System.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new System.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetDeleteCommand().CommandText;

        }



        public static string GetPropertyNames(Type  ModelClass, bool excludeKey = false)
        {
            
            var properties = ModelClass.GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return values;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesInfo(Type ModelClass, bool excludeKey = false)
        {
            var properties =ModelClass.GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            return properties;
        }

        public static bool IListToCSVFile<T>(IList<T> data, string filename, bool PrintHeader = true, string SheetName="")
        {
            bool save=false;
            try
            {
                MiniExcel.SaveAs(filename, data, PrintHeader ,SheetName,  ExcelType.CSV);
                save=true;  
            }
            catch (Exception)
            {

                throw;
            }

            return save;

        }

        public static DataTable ObjectListToDataTable(object ObjectList)
        {

            IList collection = (IList)ObjectList;
            Type T= Passero.Framework.ReflectionHelper.GetListType(ObjectList);

            DataTable dataTable = new DataTable(T.Name);
            //Get all the properties
            PropertyInfo[] Props = T.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (var item in collection )
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable IListToDataTable<T>( IList<T> data)
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

        public static string GetColumnName<T>(string ColumnName)
        {
            var pInfo = typeof(T).GetProperty(ColumnName).GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            string _ColumnName = pInfo.ColumnName;
            return _ColumnName;
        }

        public static string GetColumnName<T>(object PropertyName)
        {
            string ColumnName = nameof(PropertyName);
            var pInfo = typeof(T).GetProperty(ColumnName).GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            string _ColumnName = pInfo.ColumnName;
            return _ColumnName;
        }

        public static DataTable GetDataTableFromDapperQuery(IDbConnection DbConnection, string SQLQuery, DynamicParameters? Parameters = null)
        {
            DataTable dataTable = new DataTable();
            IDataReader dataReader;
            dataReader = DbConnection.ExecuteReader(SQLQuery, Parameters);
            dataTable.Load(dataReader);
            return dataTable;
        }

        public static string ResolveSQL(string SQL, Dapper.DynamicParameters  Parameters)
        {
            string _SQL= SQL;
           
            if (_SQL.Trim ()=="" | _SQL==null)
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
                    if (value.GetType() == typeof(string))
                    {

                        ParamValue = $"'{value.ToString()}'";
                    }
                    else
                    {
                        ParamValue = $"{value.ToString()}";
                    }
                    _SQL = Regex.Replace(SQL, ParamName, ParamValue, RegexOptions.IgnoreCase);
                }
            }
     
            return _SQL;
        }

      
        public static string GetTableName<T>()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }
        public static string GetTableName(Type ModelClass)
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(ModelClass);

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { ModelClass  }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }




        public static string GetTableName(object Model)
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(Model.GetType());

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { Model.GetType()  }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }


        public static string SetTableName<T>(T Model,string TableName)
        {
            
            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string setTableName = "SetTableName";
            MethodInfo? setTableNameMethod = typeof(SqlMapperExtensions).GetMethod(setTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (setTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{setTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            Interaction.CallByName(Model, setTableName, CallType.Set, TableName);
            return setTableName;


        }

    }



    [Serializable]
    public class QueryFilterItem
    {
        public string ColumnName { get; set; }
        public ComparisionOperator ComparisionOperator { get; set; }
        public object Values { get; set; }
        public LogicOperator LogicalOperator { get; set; } = LogicOperator.And;

        public QueryFilterItem(string ColumnName, ComparisionOperator ComparisionOperatore, LogicOperator LogicalOperator, params object[] Values)
        {
            this.ColumnName = ColumnName;
            this.Values = Values;
            ComparisionOperator = ComparisionOperator;
            this.LogicalOperator = LogicalOperator;
        }

    }

    public class QueryFilters
    {
        public Collection<QueryFilter> Filters { get; set; } = new Collection<QueryFilter>();
        public void AddFilter(QueryFilter Item, LogicOperator LogicOperator = LogicOperator.None)
        {
            if (Filters.Contains(Item) == false)
            {
                Item.LogicOperator = LogicOperator;
                Filters.Add(Item);
            }
        }

        public void ClearFilters()
        {
            Filters.Clear();
        }

        public string BuildQuery()
        {
            return default;

            /* TODO ERROR: Skipped WarningDirectiveTrivia
            #Disable Warning BC42105 ' La funzione non restituisce un valore per tutti i percorsi del codice
            */
        }
        /* TODO ERROR: Skipped WarningDirectiveTrivia
        #Enable Warning BC42105 ' La funzione non restituisce un valore per tutti i percorsi del codice
        */
    }
    public class QueryFilter
    {
        public Collection<QueryFilterItem> FilterItems { get; set; } = new Collection<QueryFilterItem>();
        public LogicOperator LogicOperator { get; set; } = LogicOperator.None;
        public void AddFilterItem(QueryFilterItem Item)
        {
            if (FilterItems.Contains(Item) == false)
            {
                FilterItems.Add(Item);
            }
        }

        public void ClearFilterItems()
        {
            FilterItems.Clear();
        }
        public void AddFilterItem(string ColumnName, ComparisionOperator ComparisionOperator, LogicOperator LogicalOperator, params object[] Values)
        {

            var Item = new QueryFilterItem(ColumnName, ComparisionOperator, LogicalOperator, Values);

            if (FilterItems.Contains(Item) == false)
            {
                FilterItems.Add(Item);
            }
        }


    }

    [Serializable]
    public enum ComparisionOperator
    {
        EqualTo = 0,
        GreaterThan = 1,
        LessThan = 2,
        GreaterThanOrEqual = 3,
        LessThanOrEqual = 4,
        NotEqual = 5,
        Between = 6,
        NotBetween = 7,
        Like = 8,
        NotLike = 9,
        In = 10,
        NotIn = 11
    }

    [Serializable]
    public enum LogicOperator
    {
        And = 1,
        Or = 2,
        None = 0
    }

}