using Dapper;
using Dapper.ColumnMapper;
using Dapper.Contrib.Extensions;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Passero.Framework.Extensions;
using Wisej.Web;
//using Passero.Framework.Base;
#nullable enable

namespace Passero.Framework
{
    public partial class Repository<ModelClass>
        where ModelClass : class
    {
        public string DefaultSQLQuery { get; set; } = "";
        /// <summary>
        /// The m default SQL query parameters
        /// </summary>
        private DynamicParameters mDefaultSQLQueryParameters;
        /// <summary>
        /// Gets or sets the default SQL query parameters.
        /// </summary>
        /// <value>
        /// The default SQL query parameters.
        /// </value>
        public DynamicParameters DefaultSQLQueryParameters
        {
            get
            {
                return mDefaultSQLQueryParameters;
            }
            set
            {
                mDefaultSQLQueryParameters = value;
            }
        }



        private static readonly ConcurrentDictionary<Type, string> _tableNameCache = new();
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetTableName()
        {
            return _tableNameCache.GetOrAdd(typeof(ModelClass), type =>
            {
                var tableAttr = type.GetCustomAttribute<TableAttribute>();
                return tableAttr?.Name ?? type.Name;
            });
        }
        public string GetTableNameNoCache()
        {
            string tableName = "";
            var type = typeof(ModelClass);
            var tableAttr = type.GetCustomAttribute<Dapper.Contrib.Extensions.TableAttribute>();

            if (tableAttr is not null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name; // & "s"
        }

        /// <summary>
        /// Sets the name of the table.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        public bool SetTableName(string TableName)
        {


            var type = typeof(ModelClass);
            var tableAttr = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();

            var n = new System.ComponentModel.DataAnnotations.Schema.TableAttribute(TableName);
            n = tableAttr;

            Utilities.Assign(ref tableAttr, n);
            return default;

        }

        /// <summary>
        /// Gets the name of the key column.
        /// </summary>
        /// <returns></returns>
        public string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(ModelClass).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                // Dim keyAttributes As Object() = [property].GetCustomAttributes(GetType(Dapper.Contrib.Extensions.KeyAttribute), True)
                object[] keyAttributes = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true);

                if (keyAttributes is not null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute), true);

                    if (columnAttributes is not null && columnAttributes.Length > 0)
                    {
                        System.ComponentModel.DataAnnotations.Schema.ColumnAttribute columnAttribute = (System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="excludeKey">if set to <c>true</c> [exclude key].</param>
        /// <returns></returns>
        public string GetColumns(bool excludeKey = false)
        {
            var type = typeof(ModelClass);
            // Dim columns = String.Join(", ", type.GetProperties().Where(Function(p) Not excludeKey OrElse Not p.IsDefined(GetType(Dapper.Contrib.Extensions.KeyAttribute))).
            // [Select](Function(p)
            // Dim columnAttr = p.GetCustomAttribute(Of System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)()
            // Return If(columnAttr IsNot Nothing, columnAttr.Name, p.Name)
            // End Function))

            string columns = string.Join(", ", type.GetProperties().Where(p => !excludeKey || !p.IsDefined(typeof(System.ComponentModel.DataAnnotations.KeyAttribute))).Select(p =>
{
    var columnAttr = p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>();
    return columnAttr is not null ? columnAttr.Name : p.Name;
}));
            return columns;
        }

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <param name="excludeKey">if set to <c>true</c> [exclude key].</param>
        /// <returns></returns>
        public string GetPropertyNames(bool excludeKey = false)
        {
            // Dim properties = GetType(ModelClass).GetProperties().Where(Function(p) Not excludeKey OrElse p.GetCustomAttribute(Of Dapper.Contrib.Extensions.KeyAttribute)() Is Nothing)
            var properties = typeof(ModelClass).GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return values;
        }


        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();


        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="excludeKey">if set to <c>true</c> [exclude key].</param>
        /// <returns></returns>
        public IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {

            var properties = _propertyCache.GetOrAdd(typeof(ModelClass), t => t.GetProperties());
            return excludeKey
                ? properties.Where(p => p.GetCustomAttribute<KeyAttribute>() == null)
                : properties;

            //var properties = typeof(ModelClass).GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            //return properties;
        }

        /// <summary>
        /// Gets the name of the key property.
        /// </summary>
        /// <returns></returns>
        public string GetKeyPropertyName()
        {
            // Dim properties = GetType(ModelClass).GetProperties().Where(Function(p) p.GetCustomAttribute(Of Dapper.Contrib.Extensions.KeyAttribute)() IsNot Nothing)
            var properties = typeof(ModelClass).GetProperties().Where(p => p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is not null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }


        private void ValidateConnection(bool OpenIfNotOpen=false)
        {
            if (DbConnection == null)
                throw new InvalidOperationException($"{mClassName}: Database connection not initialized");
            if (OpenIfNotOpen && DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
                if (DbConnection.State != ConnectionState.Open)
                    throw new InvalidOperationException($"{mClassName}: Database connection is not open");
            }
        }

        private void UpdateViewModelAndShadow()
        {
            if (ViewModel != null)
            {
                ViewModel.ModelItem = _ModelItem;
            }
            SetModelItemShadow();
            LastExecutionResult = new ExecutionResult(mClassName);
        }

        private void SetQueryParameters(string query, object parameters)
        {
            mSQLQuery = query;
            Parameters = Utilities.GetDynamicParameters(parameters);
        }

        private void HandleGetItemError(ExecutionResult<ModelClass> er, Exception ex, string query)
        {
            er.ResultCode = ExecutionResultCodes.Failed;
            er.Exception = ex;
            er.ResultMessage = ex.Message;
            er.ErrorCode = 1;
            er.DebugInfo = $"SQLQuery = {query}";
            LastExecutionResult = er.ToExecutionResult();
            HandleException(er.ToExecutionResult());
        }

    }
}
