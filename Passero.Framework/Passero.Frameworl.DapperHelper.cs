using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.VisualBasic;
using MiniExcelLibs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;



namespace Passero.Framework.DapperHelper
{

    /// <summary>
    /// 
    /// </summary>
    public class Utilities
    {


        /// <summary>
        /// Crea DynamicParameters da un oggetto anonimo o qualsiasi oggetto
        /// </summary>
        /// <param name="obj">Oggetto contenente i parametri</param>
        /// <returns>DynamicParameters</returns>
        public static Dapper.DynamicParameters CreateDynamicParameters(object obj)
        {
            var parameters = new Dapper.DynamicParameters();

            if (obj == null)
                return parameters;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                parameters.Add(property.Name, value);
            }

            return parameters;
        }

        /// <summary>
        /// Crea DynamicParameters da un dizionario
        /// </summary>
        /// <param name="dictionary">Dizionario chiave-valore</param>
        /// <returns>DynamicParameters</returns>
        public static Dapper.DynamicParameters CreateDynamicParameters(IDictionary<string, object> dictionary)
        {
            var parameters = new Dapper.DynamicParameters();

            if (dictionary == null)
                return parameters;

            foreach (var kvp in dictionary)
            {
                parameters.Add(kvp.Key, kvp.Value);
            }

            return parameters;
        }

        /// <summary>
        /// Aggiunge parametri da oggetto anonimo a DynamicParameters esistenti
        /// </summary>
        /// <param name="parameters">DynamicParameters esistenti</param>
        /// <param name="obj">Oggetto contenente parametri da aggiungere</param>
        public static void AddParameters(Dapper.DynamicParameters parameters, object obj)
        {
            if (obj == null || parameters == null)
                return;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                parameters.Add(property.Name, value);
            }
        }



        /// <summary>
        /// Gets the properties information.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <param name="ExcludeComputed">if set to <c>true</c> [exclude computed].</param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertiesInfo(Type ModelClass, bool ExcludeComputed = false)
        {
            var properties = ModelClass.GetProperties();

            List<PropertyInfo> x = new List<PropertyInfo>();
            foreach (PropertyInfo p in properties)
            {
                if (p.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.WriteAttribute>() != null)
                {
                    if (ExcludeComputed == false)
                    {
                        x.Add(p);
                    }
                }
                else
                {
                    x.Add(p);
                }
            }
            return x;
        }

        /// <summary>
        /// Gets the primary keys properties information.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPrimaryKeysPropertiesInfo(Type ModelClass)
        {
            var properties = ModelClass.GetProperties();
            List<PropertyInfo> x = new List<PropertyInfo>();
            foreach (PropertyInfo p in properties)
            {
                if (p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null)
                {
                    x.Add(p);
                }
            }
            return x;
        }


        /// <summary>
        /// Gets the primary key names.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <returns></returns>
        public static string GetPrimaryKeyNames(Type ModelClass)
        {
            var properties = ModelClass.GetProperties().Where((p) => p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

            var values = string.Join(",", properties.Select((p) => $"{p.Name}"));
            return values;

        }
        /// <summary>
        /// Gets the primary key names list.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <returns></returns>
        public static List<string> GetPrimaryKeyNamesList(Type ModelClass)
        {
            List<string> x = new List<string>();
            var properties = ModelClass.GetProperties().Where((p) => p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

            var values = string.Join(",", properties.Select((p) => $"{p.Name}"));
            x = values.Split(',').ToList();
            return x;
        }





        /// <summary>
        /// Properties the is writeable.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public static bool PropertyIsWriteable(PropertyInfo pi)
        {

            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1)
            {
                return true;
            }
            WriteAttribute writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        /// <summary>
        /// Properties the is explicit key.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public static bool PropertyIsExplicitKey(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(ExplicitKeyAttribute), false).AsList();
            if (attributes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Properties the is identity key.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public static bool PropertyIsIdentityKey(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(KeyAttribute), false).AsList();
            if (attributes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Properties the is explicit key.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static bool PropertyIsExplicitKey(Type ModelClass, string PropertyName)
        {
            if (string.IsNullOrEmpty(PropertyName.Trim()))
            {
                return false;
            }

            var pi = ModelClass.GetProperty(PropertyName);
            if (pi == null)
            {
                return false;
            }
            var attributes = pi.GetCustomAttributes(typeof(ExplicitKeyAttribute), false).AsList();

            if (attributes.Count == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Properties the is identity key.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static bool PropertyIsIdentityKey(Type ModelClass, string PropertyName)
        {
            if (string.IsNullOrEmpty(PropertyName.Trim()))
            {
                return false;
            }

            var pi = ModelClass.GetProperty(PropertyName);
            if (pi == null)
            {
                return false;
            }

            var attributes = pi.GetCustomAttributes(typeof(KeyAttribute), false).AsList();
            if (attributes.Count == 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Gets the dynamic parameters.
        /// </summary>
        /// <param name="Params">The parameters.</param>
        /// <returns></returns>
        public static DynamicParameters GetDynamicParameters(object Params)
        {
            if (Params == null)
                return null;

            switch (Params.GetType().Name)
            {
                case "VB$AnonymousType_0`1":
                    return null;
                    break;
                case "DynamicParameters":
                    return (DynamicParameters)Params;
            }
            return null;
        }

        /// <summary>
        /// Gets the update SQL command.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <returns></returns>
        public static string GetUpdateSqlCommand(Type ModelClass)
        {
            List<PropertyInfo> properties;
            StringBuilder sbset = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string setlist;
            string wherelist;
            string sql;
            properties = DapperHelper.Utilities.GetPropertiesInfo(ModelClass, true);
            foreach (PropertyInfo pi in properties)
            {
                if (!DapperHelper.Utilities.PropertyIsIdentityKey(pi))
                {
                    sbset.Append($"{pi.Name}=@{pi.Name}, ");
                }
                if (DapperHelper.Utilities.PropertyIsExplicitKey(pi) || DapperHelper.Utilities.PropertyIsIdentityKey(pi))
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
            sql = $"UPDATE {DapperHelper.Utilities.GetTableName(ModelClass)} SET {setlist} WHERE ({wherelist})";
            return sql;
        }





        /// <summary>
        /// Gets the insert SQL command.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <returns></returns>
        public static string GetInsertSqlCommand(string SQLQuery, System.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new System.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new System.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetInsertCommand().CommandText;

        }


        /// <summary>
        /// Gets the delete SQL command.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <returns></returns>
        public static string GetDeleteSqlCommand(string SQLQuery, System.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new System.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new System.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetDeleteCommand().CommandText;

        }



        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <param name="excludeKey">if set to <c>true</c> [exclude key].</param>
        /// <returns></returns>
        public static string GetPropertyNames(Type ModelClass, bool excludeKey = false)
        {

            var properties = ModelClass.GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return values;
        }

        /// <summary>
        /// Gets the properties info2.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <param name="excludeKey">if set to <c>true</c> [exclude key].</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesInfo2(Type ModelClass, bool excludeKey = false)
        {
            var properties = ModelClass.GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            return properties;
        }

        /// <summary>
        /// is the list to CSV file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="PrintHeader">if set to <c>true</c> [print header].</param>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <returns></returns>
        public static bool IListToCSVFile<T>(IList<T> data, string filename, bool PrintHeader = true, string SheetName = "")
        {
            bool save = false;
            try
            {
                MiniExcel.SaveAs(filename, data, PrintHeader, SheetName, ExcelType.CSV);
                save = true;
            }
            catch (Exception)
            {

                throw;
            }

            return save;

        }

        /// <summary>
        /// Objects the list to data table.
        /// </summary>
        /// <param name="ObjectList">The object list.</param>
        /// <returns></returns>
        public static DataTable ObjectListToDataTable(object ObjectList)
        {

            IList collection = (IList)ObjectList;
            Type T = Passero.Framework.ReflectionHelper.GetListType(ObjectList);

            DataTable dataTable = new DataTable(T.Name);
            //Get all the properties
            PropertyInfo[] Props = T.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (var item in collection)
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
        /// <summary>
        /// Lists to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
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

        /// <summary>
        /// is the list to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static DataTable IListToDataTable<T>(IList<T> data)
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
        /// Gets the name of the column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ColumnName">Name of the column.</param>
        /// <returns></returns>
        public static string GetColumnName<T>(string ColumnName)
        {
            var pInfo = typeof(T).GetProperty(ColumnName).GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            string _ColumnName = pInfo.ColumnName;
            return _ColumnName;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetColumnName<T>(object PropertyName)
        {
            string ColumnName = nameof(PropertyName);
            var pInfo = typeof(T).GetProperty(ColumnName).GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            string _ColumnName = pInfo.ColumnName;
            return _ColumnName;
        }

#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        /// <summary>
        /// Gets the data table from dapper query.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromDapperQuery(IDbConnection DbConnection, string SQLQuery, DynamicParameters? Parameters = null)
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        {
            DataTable dataTable = new DataTable();
            IDataReader dataReader;
            dataReader = DbConnection.ExecuteReader(SQLQuery, Parameters);
            dataTable.Load(dataReader);
            return dataTable;
        }


        /// <summary>
        /// Dappers the select.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static List<IDictionary<string, object>> DapperSelect(IDbConnection dbConnection, string query, object parameters)
        {

            {
                var result = dbConnection.Query(query, parameters).ToList();

                return result.Select(x => (IDictionary<string, object>)x).ToList();
            }
        }

        /// <summary>
        /// Resolves the SQL.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.</exception>
        public static string GetTableName<T>()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.</exception>
        public static string GetTableName(Type ModelClass)
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(ModelClass);

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { ModelClass }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }




        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.</exception>
        public static string GetTableName(object Model)
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(Model.GetType());

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { Model.GetType() }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }


        /// <summary>
        /// Sets the name of the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model">The model.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Method '{setTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.</exception>
        public static string SetTableName<T>(T Model, string TableName)
        {

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string setTableName = "SetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? setTableNameMethod = typeof(SqlMapperExtensions).GetMethod(setTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (setTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{setTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            Interaction.CallByName(Model, setTableName, CallType.Set, TableName);
            return setTableName;


        }

    }



    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class QueryFilterItem
    {
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets the comparision operator.
        /// </summary>
        /// <value>
        /// The comparision operator.
        /// </value>
        public ComparisionOperator ComparisionOperator { get; set; }
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public object Values { get; set; }
        /// <summary>
        /// Gets or sets the logical operator.
        /// </summary>
        /// <value>
        /// The logical operator.
        /// </value>
        public LogicOperator LogicalOperator { get; set; } = LogicOperator.And;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilterItem"/> class.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="ComparisionOperatore">The comparision operatore.</param>
        /// <param name="LogicalOperator">The logical operator.</param>
        /// <param name="Values">The values.</param>
        public QueryFilterItem(string ColumnName, ComparisionOperator ComparisionOperatore, LogicOperator LogicalOperator, params object[] Values)
        {
            this.ColumnName = ColumnName;
            this.Values = Values;
            ComparisionOperator = ComparisionOperator;
            this.LogicalOperator = LogicalOperator;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryFilters
    {
        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public Collection<QueryFilter> Filters { get; set; } = new Collection<QueryFilter>();
        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="LogicOperator">The logic operator.</param>
        public void AddFilter(QueryFilter Item, LogicOperator LogicOperator = LogicOperator.None)
        {
            if (Filters.Contains(Item) == false)
            {
                Item.LogicOperator = LogicOperator;
                Filters.Add(Item);
            }
        }

        /// <summary>
        /// Clears the filters.
        /// </summary>
        public void ClearFilters()
        {
            Filters.Clear();
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// Gets or sets the filter items.
        /// </summary>
        /// <value>
        /// The filter items.
        /// </value>
        public Collection<QueryFilterItem> FilterItems { get; set; } = new Collection<QueryFilterItem>();
        /// <summary>
        /// Gets or sets the logic operator.
        /// </summary>
        /// <value>
        /// The logic operator.
        /// </value>
        public LogicOperator LogicOperator { get; set; } = LogicOperator.None;
        /// <summary>
        /// Adds the filter item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void AddFilterItem(QueryFilterItem Item)
        {
            if (FilterItems.Contains(Item) == false)
            {
                FilterItems.Add(Item);
            }
        }

        /// <summary>
        /// Clears the filter items.
        /// </summary>
        public void ClearFilterItems()
        {
            FilterItems.Clear();
        }
        /// <summary>
        /// Adds the filter item.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="ComparisionOperator">The comparision operator.</param>
        /// <param name="LogicalOperator">The logical operator.</param>
        /// <param name="Values">The values.</param>
        public void AddFilterItem(string ColumnName, ComparisionOperator ComparisionOperator, LogicOperator LogicalOperator, params object[] Values)
        {

            var Item = new QueryFilterItem(ColumnName, ComparisionOperator, LogicalOperator, Values);

            if (FilterItems.Contains(Item) == false)
            {
                FilterItems.Add(Item);
            }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum ComparisionOperator
    {
        /// <summary>
        /// The equal to
        /// </summary>
        EqualTo = 0,
        /// <summary>
        /// The greater than
        /// </summary>
        GreaterThan = 1,
        /// <summary>
        /// The less than
        /// </summary>
        LessThan = 2,
        /// <summary>
        /// The greater than or equal
        /// </summary>
        GreaterThanOrEqual = 3,
        /// <summary>
        /// The less than or equal
        /// </summary>
        LessThanOrEqual = 4,
        /// <summary>
        /// The not equal
        /// </summary>
        NotEqual = 5,
        /// <summary>
        /// The between
        /// </summary>
        Between = 6,
        /// <summary>
        /// The not between
        /// </summary>
        NotBetween = 7,
        /// <summary>
        /// The like
        /// </summary>
        Like = 8,
        /// <summary>
        /// The not like
        /// </summary>
        NotLike = 9,
        /// <summary>
        /// The in
        /// </summary>
        In = 10,
        /// <summary>
        /// The not in
        /// </summary>
        NotIn = 11
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum LogicOperator
    {
        /// <summary>
        /// The and
        /// </summary>
        And = 1,
        /// <summary>
        /// The or
        /// </summary>
        Or = 2,
        /// <summary>
        /// The none
        /// </summary>
        None = 0
    }

}