
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            PropertyDescriptorCollection properties =TypeDescriptor.GetProperties(type);
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
           
            IList objectList = data as IList;
            foreach (var item in objectList )
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

           

            var _DBConnection = (System.Data.Common .DbConnection) Activator.CreateInstance(DBConnection.GetType());
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


        /// <summary>
        /// Gets the update SQL command.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <returns></returns>
        public static string GetUpdateSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetUpdateCommand().CommandText;

        }

        /// <summary>
        /// Gets the update SQL command using only System.Data.Common interfaces where possible.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>The update command text.</returns>
        public static string GetUpdateSqlCommand(string SQLQuery, DbConnection dbConnection)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));

            // Usa il tipo della connessione per determinare il provider e creare gli oggetti appropriati
            if (dbConnection is Microsoft.Data.SqlClient.SqlConnection sqlConn)
            {
                var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, sqlConn);
                using var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
                return cmdbuilder.GetUpdateCommand().CommandText;
            }
            // Aggiungi supporto per altri provider decommentando e installando i pacchetti NuGet appropriati
            // else if (dbConnection is MySql.Data.MySqlClient.MySqlConnection mySqlConn)
            // {
            //     var da = new MySql.Data.MySqlClient.MySqlDataAdapter(SQLQuery, mySqlConn);
            //     using var cmdbuilder = new MySql.Data.MySqlClient.MySqlCommandBuilder(da);
            //     return cmdbuilder.GetUpdateCommand().CommandText;
            // }
            // else if (dbConnection is Npgsql.NpgsqlConnection pgConn)
            // {
            //     var da = new Npgsql.NpgsqlDataAdapter(SQLQuery, pgConn);
            //     using var cmdbuilder = new Npgsql.NpgsqlCommandBuilder(da);
            //     return cmdbuilder.GetUpdateCommand().CommandText;
            // }
            // else if (dbConnection is Oracle.ManagedDataAccess.Client.OracleConnection oraConn)
            // {
            //     var da = new Oracle.ManagedDataAccess.Client.OracleDataAdapter(SQLQuery, oraConn);
            //     using var cmdbuilder = new Oracle.ManagedDataAccess.Client.OracleCommandBuilder(da);
            //     return cmdbuilder.GetUpdateCommand().CommandText;
            // }
            // else if (dbConnection is Microsoft.Data.Sqlite.SqliteConnection sqliteConn)
            // {
            //     var da = new Microsoft.Data.Sqlite.SqliteDataAdapter(SQLQuery, sqliteConn);
            //     using var cmdbuilder = new Microsoft.Data.Sqlite.SqliteCommandBuilder(da);
            //     return cmdbuilder.GetUpdateCommand().CommandText;
            // }
            else
            {
                throw new NotSupportedException($"Database provider for connection type '{dbConnection.GetType().FullName}' is not supported. Only SqlClient is implemented by default.");
            }
        }




        /// <summary>
        /// Gets the insert SQL command.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <returns></returns>
        public static string GetInsertSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetInsertCommand().CommandText;

        }


        /// <summary>
        /// Gets the delete SQL command.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <returns></returns>
        public static string GetDeleteSqlCommand(string SQLQuery, Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new Microsoft.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetDeleteCommand().CommandText;

        }

    }

}
