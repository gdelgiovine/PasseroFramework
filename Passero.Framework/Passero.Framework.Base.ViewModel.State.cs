using Dapper;
using FastDeepCloner;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Wisej.Web;
using Wisej.Web.Data;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {
        public string GetTableName()
        {
            return Repository.GetTableName();   
        }

        /// <summary>
        /// Espone il <see cref="Base.IPasseroDbContext"/> usato per creare il repository interno.
        /// Disponibile solo quando il ViewModel è stato inizializzato tramite
        /// <see cref="ViewModel(Base.IPasseroDbContext, string, string, string)"/>.
        /// Utile per operazioni avanzate (transazioni, query raw, cambio ORM a runtime).
        /// </summary>
        public Base.IPasseroDbContext DbContext { get; private set; }

        /// <summary>
        /// The m data navigator
        /// </summary>
        private dynamic mDataNavigator = null;
        /// <summary>
        /// Gets or sets the data navigator.
        /// </summary>
        /// <value>
        /// The data navigator.
        /// </value>
        public dynamic DataNavigator
        {
            get { return mDataNavigator; }
            set { mDataNavigator = value; }
        }
        /// <summary>
        /// Gets or sets the owner view.
        /// </summary>
        /// <value>
        /// The owner view.
        /// </value>
        public Wisej.Web.Form OwnerView { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = $"ViewModel<{typeof(ModelClass).FullName}>";
        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; } = $"ViewModel<{typeof(ModelClass).FullName}>";
        /// <summary>
        /// Gets or sets the minimum date time.
        /// </summary>
        /// <value>
        /// The minimum date time.
        /// </value>
        public DateTime MinDateTime { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0);
        /// <summary>
        /// Gets or sets the maximum date time.
        /// </summary>
        /// <value>
        /// The maximum date time.
        /// </value>
        public DateTime MaxDateTime { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59, 999);
        /// <summary>
        /// Gets or sets the use model data.
        /// </summary>
        /// <value>
        /// The use model data.
        /// </value>
        public UseModelData UseModelData { get; set; } = UseModelData.InternalRepository;
        /// <summary>
        /// Gets or sets the binding behaviour.
        /// </summary>
        /// <value>
        /// The binding behaviour.
        /// </value>
        public BindingBehaviour bindingBehaviour { get; set; } = BindingBehaviour.SelectInsertUpdate;
        /// <summary>
        /// The m error notification message box
        /// </summary>
        private ErrorNotificationMessageBox mErrorNotificationMessageBox = new ErrorNotificationMessageBox();
        /// <summary>
        /// The m error notification mode
        /// </summary>
        private ErrorNotificationModes mErrorNotificationMode = ErrorNotificationModes.ThrowException;
        /// <summary>
        /// The m model item shadow
        /// </summary>
        private ModelClass mModelItemShadow;
        /// <summary>
        /// The external model shadow
        /// </summary>
        private ModelClass ExternalModelShadow;
        /// <summary>
        /// The m add new current model item index
        /// </summary>
        private int mAddNewCurrentModelItemIndex = -1;
        /// <summary>
        /// The m current model item index
        /// </summary>
        private int mCurrentModelItemIndex = -1;
        /// <summary>
        /// The m model items
        /// </summary>
        private IList<ModelClass> mModelItems;
        /// <summary>
        /// The m model items shadow
        /// </summary>
        private IList<ModelClass> mModelItemsShadow;
        /// <summary>
        /// The m binding source
        /// </summary>
        private BindingSource mBindingSource;
        /// <summary>
        /// The binding source controls
        /// </summary>
        private Dictionary<string, Wisej.Web.Control> BindingSourceControls = new Dictionary<string, Control>();
        /// <summary>
        /// The m data binding mode
        /// </summary>
        private DataBindingMode mDataBindingMode = DataBindingMode.None;


        /// <summary>
        /// Gets or sets the last execution result.
        /// </summary>
        /// <value>
        /// The last execution result.
        /// </value>
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult(mClassName);
        /// <summary>
        /// Gets or sets the error notification mode.
        /// </summary>
        /// <value>
        /// The error notification mode.
        /// </value>
        public ErrorNotificationModes ErrorNotificationMode
        {
            get { return mErrorNotificationMode; }
            set
            {
                mErrorNotificationMode = value;
                Repository.ErrorNotificationMode = value;
            }
        }
        /// <summary>
        /// Gets or sets the error notification message box.
        /// </summary>
        /// <value>
        /// The error notification message box.
        /// </value>
        public ErrorNotificationMessageBox ErrorNotificationMessageBox
        {
            get { return mErrorNotificationMessageBox; }
            set
            {
                mErrorNotificationMessageBox = value;
                Repository.ErrorNotificationMessageBox = value;
            }
        }

        /// <summary>
        /// Handles the exeception.
        /// </summary>
        /// <param name="executionResult">The execution result.</param>
        public void HandleExeception(ExecutionResult executionResult)
        {
            if (executionResult == null)
            {
                return;
            }

            switch (ErrorNotificationMode)
            {
                case ErrorNotificationModes.ShowDialog:
                    StringBuilder msg = new StringBuilder();
                    msg.Append($"{executionResult.Context}\n");
                    msg.Append($"{executionResult.ResultMessage}\n");
                    msg.Append($"{Name}");
                    ReflectionHelper.CallByName(ErrorNotificationMessageBox, "Show", Microsoft.VisualBasic.CallType.Method, msg.ToString());
                    break;
                case ErrorNotificationModes.Silent:
                    break;
                case ErrorNotificationModes.ThrowException:
                    throw executionResult.Exception;
            }

        }
        /// <summary>
        /// The m SQL query
        /// </summary>
        private string mSQLQuery;

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
                mSQLQuery = Repository.SQLQuery;
                return mSQLQuery;
            }
            set
            {
                mSQLQuery = value;
                Repository.SQLQuery = value;
            }
        }

        /// <summary>
        /// Sets the SQL query.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        public void SetSQLQuery(string SQLQuery, DynamicParameters parameters)
        {
            this.SQLQuery = SQLQuery;
            this.Parameters = parameters;

        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //mBindingSource?.Dispose();
                    Repository?.Dispose();
                }
                _disposed = true;
            }
        }

        // Cacheare i PropertyInfo per reflection
        private static readonly ConcurrentDictionary<string, PropertyInfo> _propertyCache =
            new ConcurrentDictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
        

        /// <summary>
        /// Resolveds the SQL query.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public string ResolvedSQLQuery(string SQLQuery = "", DynamicParameters Parameters = null)
        {
            if (SQLQuery != null && Parameters != null)
                return Passero.Framework.Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return Passero.Framework.   Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
        }

        /// <summary>
        /// Resolveds the SQL query.
        /// </summary>
        /// <returns></returns>
        public string ResolvedSQLQuery()
        {
            if (this.SQLQuery != null && this.Parameters != null)
                return Passero.Framework.Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return Passero.Framework.Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
        }
        /// <summary>
        /// Gets or sets the binding source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        public BindingSource BindingSource
        {
            get
            {
                return mBindingSource;
            }
            set
            {
                mBindingSource = value;
            }
        }
    }
}
