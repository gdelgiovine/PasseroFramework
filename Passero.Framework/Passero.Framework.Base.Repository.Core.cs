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
    //public interface IRepository<ModelClass> where ModelClass : ModelBase
    public interface  IRepository<ModelClass> : IDisposable
    where ModelClass : class  
    
    {
        IList<ModelClass> GetAllItems();
        void SaveItems(IList<ModelClass> items);
    }

    [Serializable]
        public partial class Repository<ModelClass> : Base.IPasseroRepository<ModelClass>
        where ModelClass : class
    {
        //public BindingSource BindingSource { get; set; }
        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.Base.Repository";
        /// <summary>
        /// The model properties
        /// </summary>
        private Dictionary<string, System.Reflection.PropertyInfo> ModelProperties;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = $"Repository<{typeof(ModelClass).FullName}>";
        /// <summary>
        /// Gets or sets the last execution result.
        /// </summary>
        /// <value>
        /// The last execution result.
        /// </value>
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult(mClassName);
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public ViewModel<ModelClass > ViewModel { get; set; }
        
        /// <summary>
        /// Gets or sets the error notification mode.
        /// </summary>
        /// <value>
        /// The error notification mode.
        /// </value>
        public ErrorNotificationModes ErrorNotificationMode { get; set; } = ErrorNotificationModes.ThrowException;

        /// <summary>
        /// Gets or sets the error notification message box.
        /// </summary>
        /// <value>
        /// The error notification message box.
        /// </value>
        public ErrorNotificationMessageBox ErrorNotificationMessageBox { get; set; }
        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public DynamicParameters Parameters { get; set; }

        /// <summary>
        /// Occurs when [model events].
        /// </summary>
        public event EventHandler ModelEvents;
        /// <summary>
        /// The m SQL query
        /// </summary>
        private string mSQLQuery = "";
        /// <summary>
        /// Gets or sets the SQL query.
        /// </summary>
        /// <value>
        /// The SQL query.
        /// </value>
        public string SQLQuery
        {
            get
            {
                return mSQLQuery;
            }
            set
            {
                mSQLQuery = value;
            }
        }

        private static readonly Func<ModelClass, ModelClass, bool> _compareFunc = CreateCompareFunc();
        private static Func<ModelClass, ModelClass, bool> CreateCompareFunc()
        {
            return (a, b) =>
            {
                var properties = typeof(ModelClass).GetProperties();
                return properties.All(p => Equals(p.GetValue(a), p.GetValue(b)));
            };
        }
        // Aggiungere dispose pattern
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                ModelItems?.Clear();
                ModelItemsShadow?.Clear();
                Parameters = null;
                //DbTransaction?.Dispose();
                //DbConnection?.Dispose();
            }
            _disposed = true;
        }

       


        /// <summary>
        /// SQL dialect inferred from <see cref="DbConnection"/> type via reflection.
        /// Can be overridden explicitly when auto-detection is insufficient.
        /// </summary>
        public SqlDialect SqlDialect
        {
            get => _sqlDialect ?? (_sqlDialect = DetectSqlDialect()).Value;
            set => _sqlDialect = value;
        }
        private SqlDialect? _sqlDialect;

        /// <summary>
        /// Detects the SQL dialect by inspecting the runtime type name of <see cref="DbConnection"/>.
        /// Consistent with the pattern used in <see cref="DataBaseHelper.GetUpdateSqlCommand(string, System.Data.Common.DbConnection)"/>.
        /// Falls back to <see cref="SqlDialect.SqlServer"/> if unrecognized.
        /// </summary>
        private SqlDialect DetectSqlDialect()
        {
            if (DbConnection == null)
                return SqlDialect.SqlServer;

            var typeName = DbConnection.GetType().FullName ?? string.Empty;

            if (typeName.Contains("Npgsql")) return SqlDialect.PostgreSql;
            if (typeName.Contains("SQLite") || typeName.Contains("Sqlite")) return SqlDialect.SQLite;
            if (typeName.Contains("MySql") || typeName.Contains("MariaDb")) return SqlDialect.MySql;
            if (typeName.Contains("Oracle")) return SqlDialect.Oracle;

            return SqlDialect.SqlServer; // Microsoft.Data.SqlClient o System.Data.SqlClient
        }

        /// <summary>
        /// Returns the SQL fragment that retrieves the last generated identity value
        /// for the current <see cref="SqlDialect"/>.
        /// The fragment is appended to the INSERT statement separated by a semicolon.
        /// </summary>
        private string GetIdentityFragment()
        {
            return SqlDialect switch
            {
                SqlDialect.PostgreSql => "SELECT lastval()",
                SqlDialect.SQLite => "SELECT last_insert_rowid()",
                SqlDialect.MySql => "SELECT LAST_INSERT_ID()",
                SqlDialect.Oracle => "SELECT 0 FROM DUAL", // Oracle richiede una sequenza esplicita: override manuale
                _ => "SELECT SCOPE_IDENTITY()" // SqlServer default
            };
        }



        /// <summary>
        /// Resets the model item.
        /// </summary>
        /// <param name="ResetModelItems">if set to <c>true</c> [reset model items].</param>
    }
}
