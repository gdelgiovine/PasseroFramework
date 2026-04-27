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


        public static DataTable GetTrueFalseDataTableForComboBox(string TrueDescription = "True", string FalseDescription = "False")
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(bool));
            table.Columns.Add("Desc", typeof(string));
            table.Rows.Add(new object[] { false, FalseDescription });
            table.Rows.Add(new object[] { true, TrueDescription });
            return table;
        }


        public static DataTable GetYesNoDataTableForComboBox(string YesDescription = "Yes", string NoDescription = "No")
        {
            return GetTrueFalseDataTableForComboBox(YesDescription, NoDescription);
        }

    }
}