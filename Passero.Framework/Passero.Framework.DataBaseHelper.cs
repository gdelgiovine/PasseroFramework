
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public static class DataBaseHelper
    {

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

        public static bool DataColumnIsNumeric(DataColumn col)
        {
            if (col is null)
                return false;
            // Make this const
            Type[] numericTypes = new[] { typeof(byte), typeof(decimal), typeof(double), typeof(short), typeof(int), typeof(long), typeof(sbyte), typeof(float), typeof(ushort), typeof(uint), typeof(ulong) };
            return numericTypes.Contains(col.DataType);
        }
        public static Framework.ExecutionResult PingDB(System.Data.SqlClient.SqlConnection DBConnection)
        {

            var er = new Framework.ExecutionResult();
            er.Context = "PingDB";

            var _DBConnection = new System.Data.SqlClient.SqlConnection();
            _DBConnection.ConnectionString = DBConnection.ConnectionString;
            _DBConnection.Credential = DBConnection.Credential;

            try
            {
                DBConnection.Open();
            }
            catch (Exception ex)
            {
                er.ErrorCode = 1;
                er.ResultCode = ExecutionResultCodes.Failed;
                er.ResultMessage = ex.Message;
                er.Exception = ex;
            }

            return er;
        }


        public static string GetUpdateSqlCommand(string SQLQuery, System.Data.SqlClient.SqlConnection SqlConnection)
        {

            var da = new System.Data.SqlClient.SqlDataAdapter(SQLQuery, SqlConnection);
            var cmdbuilder = new System.Data.SqlClient.SqlCommandBuilder(da);
            return cmdbuilder.GetUpdateCommand().CommandText;

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

    }

}
