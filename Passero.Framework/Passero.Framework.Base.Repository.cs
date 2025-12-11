using Dapper;
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


    /// <summary></summary>
    /// <typeparam name="ModelClass">The type of the odel class.</typeparam>
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelClass"></typeparam>
    /// <remarks></remarks>
    [Serializable]
    //public class Repository<ModelClass> : IDisposable         where ModelClass : ModelBase
    public class Repository<ModelClass> : IDisposable
    where ModelClass : class   
    
    {

        public BindingSource BindingSource { get; set; }
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
                DbTransaction?.Dispose();
                DbConnection?.Dispose();
            }
            _disposed = true;
        }
             


        /// <summary>
        /// Resets the model item.
        /// </summary>
        /// <param name="ResetModelItems">if set to <c>true</c> [reset model items].</param>
        public void ResetModelItem(bool ResetModelItems = true)
        {
            ModelItem = GetEmptyModelItem();
            if (ResetModelItems)
                ModelItems = new List<ModelClass>();
        }
        /// <summary>
        /// Resets the model items.
        /// </summary>
        public void ResetModelItems()
        {
            ModelItems = new List<ModelClass>();
        }

        /// <summary>
        /// Raises the <see cref="E:ModelEvents" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnModelEvents(EventArgs e)
        {
            ModelEvents?.Invoke(this, e);
        }

        /// <summary>
        /// The add new current model item index
        /// </summary>
        private int _AddNewCurrentModelItemIndex = -1;

        /// <summary>
        /// Gets or sets the index of the add new current model item.
        /// </summary>
        /// <value>
        /// The index of the add new current model item.
        /// </value>
        public int AddNewCurrentModelItemIndex
        {
            get
            {
                return _AddNewCurrentModelItemIndex;
            }
            set
            {
                _AddNewCurrentModelItemIndex = value;
            }
        }


        /// <summary>
        /// The current model item index
        /// </summary>
        private int _CurrentModelItemIndex = -1;

        /// <summary>
        /// Gets or sets the index of the current model item.
        /// </summary>
        /// <value>
        /// The index of the current model item.
        /// </value>
        public int CurrentModelItemIndex
        {
            get
            {
                return _CurrentModelItemIndex;
            }
            set
            {
                if (value < -1)
                {
                    value = -1;
                }
                _CurrentModelItemIndex = value;
                if (value > -1)
                {
                    if (_ModelItems.Count < value)
                    {
                        _ModelItem = _ModelItems.ElementAt(_CurrentModelItemIndex);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the model items at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ExecutionResult<ModelClass> GetModelItemsAt(int index)
        {
            var ERContext = $"{mClassName}.GetModelItemsAt()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContext);
            if (_ModelItems == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "Invalid Index!";
                ER.ErrorCode = 0;
            }
            if (index > -1 && index < _ModelItems.Count())
            {
                ER.Value = _ModelItems.ElementAt(index);
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }
     
        private ExecutionResult MoveToIndex(int index)
        {
            var ERContext = $"{mClassName}.MoveToIndex()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                if (index >= 0 && index < _ModelItems.Count)
                {
                    _CurrentModelItemIndex = index;
                    _ModelItem = _ModelItems.ElementAt(index);
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                }
            }
            else
            {
                _ModelItem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Model items collection is empty.";
            }

            return ER;
        }


       

        /// <summary>
        /// Moves to the first item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveFirstItem()
        {
            return MoveToIndex(0);
        }
        /// <summary>
        /// Moves to the last item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveLastItem()
        {
            return MoveToIndex(_ModelItems.Count - 1);
        }

        /// <summary>
        /// Moves to the previous item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MovePreviousItem()
        {
            return MoveToIndex(_CurrentModelItemIndex - 1);
        }

        /// <summary>
        /// Moves to the next item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveNextItem()
        {
            return MoveToIndex(_CurrentModelItemIndex + 1);
        }

        /// <summary>
        /// Moves at item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns></returns>
        public ExecutionResult MoveAtItem(int index)
        {
            return MoveToIndex(index);
        }



        /// <summary>
        /// Gets or sets the model items.
        /// </summary>
        /// <value>
        /// The model items.
        /// </value>
        //private List<ModelClass>? _ModelItems { get; set; } = new List<ModelClass>();
        private IList<ModelClass>? _ModelItems { get; set; } = new List<ModelClass>();

        /// <summary>
        /// Gets or sets the model items.
        /// </summary>
        /// <value>
        /// The model items.
        /// </value>
        public IList<ModelClass>? ModelItems
        {
            get
            {
                return _ModelItems;
            }
            set
            {
                _ModelItems = value;
            }
        }


        /// <summary>
        /// Gets or sets the modeltem.
        /// </summary>
        /// <value>
        /// The modeltem.
        /// </value>
        private ModelClass? _ModelItem { get; set; }
        /// <summary>
        /// Gets or sets the model item.
        /// </summary>
        /// <value>
        /// The model item.
        /// </value>
        public ModelClass? ModelItem
        {
            get
            {
                return _ModelItem;
            }
            set
            {
                _ModelItem = value;
            }
        }

#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        /// <summary>
        /// Gets or sets the model item shadow.
        /// </summary>
        /// <value>
        /// The model item shadow.
        /// </value>
        private ModelClass? _ModelItemShadow { get; set; }
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        /// <summary>
        /// Gets or sets the model item shadow.
        /// </summary>
        /// <value>
        /// The model item shadow.
        /// </value>
        public ModelClass? ModelItemShadow
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        {
            get
            {
                return _ModelItemShadow;
            }
            set
            {
                _ModelItemShadow = value;
            }
        }

        /// <summary>
        /// Gets or sets the model items shadow.
        /// </summary>
        /// <value>
        /// The model items shadow.
        /// </value>
        private IList<ModelClass> _ModelItemsShadow { get; set; } = new List<ModelClass>();
        /// <summary>
        /// Gets or sets the model items shadow.
        /// </summary>
        /// <value>
        /// The model items shadow.
        /// </value>
        public IList<ModelClass> ModelItemsShadow
        {
            get
            {
                return _ModelItemsShadow;
            }
            set
            {
                _ModelItemsShadow = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [m add new state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [m add new state]; otherwise, <c>false</c>.
        /// </value>
        private bool mAddNewState { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [add new state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add new state]; otherwise, <c>false</c>.
        /// </value>
        public bool AddNewState
        {
            get
            {
                return mAddNewState;
            }
            set
            {

                mAddNewState = value;
            }
        }
        /// <summary>
        /// Adds the new.
        /// </summary>
        public void AddNew()
        {
            if (AddNewState == false)
            {
                AddNewState = true;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        //public SqlConnection SqlConnection { get; set; }
        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>

        private IDbConnection mDbConnection;
        public IDbConnection DbConnection
        {
            get
            {
                return mDbConnection;
            }
            set
            {
                mDbConnection = value;
                DbObject = new Base.DbObject<ModelClass>(mDbConnection);
            }
        }
        //public SqlTransaction SqlTransaction { get; set; }
        /// <summary>
        /// Gets or sets the database transaction.
        /// </summary>
        /// <value>
        /// The database transaction.
        /// </value>
        public IDbTransaction DbTransaction { get; set; }
        /// <summary>
        /// Gets or sets the database command timeout.
        /// </summary>
        /// <value>
        /// The database command timeout.
        /// </value>
        public int DbCommandTimeout { get; set; } = 30;

        ///// <summary>
        ///// Gets or sets the database context.
        ///// </summary>
        ///// <value>
        ///// The database context.
        ///// </value>
        //public Base.DbContext DbContext { get; set; }
        
        

        /// <summary>
        /// Gets or sets the database object.
        /// </summary>
        /// <value>
        /// The database object.
        /// </value>
        public Base.DbObject<ModelClass> DbObject { get; set; }

        /// <summary>
        /// Gets the model item clone.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetModelItemClone()
        {
            return Utilities.Clone(_ModelItem);
        }

        /// <summary>
        /// Gets the model items clone.
        /// </summary>
        /// <returns></returns>
        public IList<ModelClass> GetModelItemsClone()
        {
            return Utilities.Clone(_ModelItems);
        }


        /// <summary>
        /// Sets the model item shadow.
        /// </summary>
        public void SetModelItemShadow()
        {
            _ModelItemShadow = Utilities.Clone(_ModelItem);
            //TBD: verifica se è superfluo
            if (ViewModel != null)
            {
                ViewModel.ModelItemShadow = _ModelItemShadow;
            }
        }

        /// <summary>
        /// Sets the model items shadow.
        /// </summary>
        public void SetModelItemsShadow()
        {
            _ModelItemsShadow = Utilities.Clone(_ModelItems);
            if (ViewModel != null)
            {
                ViewModel.ModelItemShadow = _ModelItemShadow;
            }
        }

        /// <summary>
        /// Gets the empty model.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetEmptyModel()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        /// <summary>
        /// Creates the database object.
        /// </summary>
        private void CreateDbObject()
        {
            DbObject = new Base.DbObject<ModelClass>(DbConnection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        /// </summary>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <param name="SqlTransaction">The SQL transaction.</param>
        public Repository(IDbConnection SqlConnection, IDbTransaction SqlTransaction = null)
        {

            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            DbTransaction = SqlTransaction;
            DbConnection = SqlConnection;
            DbObject = new Base.DbObject<ModelClass>(DbConnection);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        /// </summary>
        /// <param name="Model">The model.</param>
        public Repository(ModelClass Model)
        {
            //ValidateModelClass();
            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            //DbObject = new Base.DbObject<ModelClass>(DbConnection);


        }

        public bool UseUpdateEx { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        /// </summary>
        public Repository()
        {
            //ValidateModelClass();
            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            //DbObject = new Base.DbObject<ModelClass>(DbConnection);


        }


        /// <summary>
        /// Resolveds the SQL query.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public string ResolvedSQLQuery(string SQLQuery = "", DynamicParameters Parameters = null)
        {
            if (SQLQuery != null && Parameters != null)
                return Passero.Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return Passero.Framework.DapperHelper.Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
        }

        /// <summary>
        /// Resolveds the SQL query.
        /// </summary>
        /// <returns></returns>
        public string ResolvedSQLQuery()
        {
            if (this.SQLQuery != null && this.Parameters != null)
                return Passero.Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return Passero.Framework.DapperHelper.Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
        }


        public void ValidateModelClass()
        {
            if (!typeof(ModelClass).IsAssignableFrom(typeof(ModelBase)))
            {
                throw new InvalidOperationException("ModelClass deve derivare da ModelBase.");
            }
        }


        ///// <summary>
        ///// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        ///// </summary>
        ///// <param name="DbContext">The database context.</param>
        //public Repository(Base.DbContext DbContext)
        //{

        //    _ModelItem = GetEmptyModel();
        //    SetModelItemShadow();
        //    SetModelItemsShadow();
        //    this.DbContext = DbContext;
        //    DbTransaction = DbContext.DbTransaction;
        //    DbConnection = DbContext.DbConnection ;
        //    DbObject = new Base.DbObject<ModelClass>(DbConnection);


        //}


        /// <summary>
        /// Determines whether [is model data changed] [the specified model shadow].
        /// </summary>
        /// <param name="ModelShadow">The model shadow.</param>
        /// <returns>
        ///   <c>true</c> if [is model data changed] [the specified model shadow]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsModelDataChanged(ModelClass ModelShadow = null)
        {

            if (ModelShadow is null)
            {
                ModelShadow = _ModelItemShadow;
            }

            return !Utilities.ObjectsEquals(_ModelItem, ModelShadow);

        }


        /// <summary>
        /// Handles the exeception.
        /// </summary>
        /// <param name="ER">The er.</param>
        public void HandleException(ExecutionResult ER)
        {

            if (ER == null)
            {
                return;
            }

            switch (ErrorNotificationMode)
            {
                case ErrorNotificationModes.ThrowException:
                    throw ER.Exception;
                case ErrorNotificationModes.Silent:
                    break;
                case ErrorNotificationModes.ShowDialog:
                    if (ErrorNotificationMessageBox != null)
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine($"Context: {ER.Context}");
                        msg.AppendLine($"Repository: {Name}");
                        msg.AppendLine($"Error Message: {ER.ResultMessage}");
                        msg.AppendLine($"Debug Info: {ER.DebugInfo}");
                        ErrorNotificationMessageBox.Show(msg.ToString());
                    }
                    break;
                default:
                    break;
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
            Parameters = parameters;

        }




        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        /// 
    
        public ExecutionResult<ModelClass> GetItem(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult<ModelClass>($"{mClassName}.GetItem()");
            ER.Value = null;
            
            try
            {
                _ModelItem = DbConnection.Query<ModelClass>(Query, Params, Transaction, Buffered, CommandTimeout).SingleOrDefault();
                if (ViewModel != null)
                {
                    ViewModel.ModelItem = _ModelItem;
                }
                SetModelItemShadow();
                LastExecutionResult = ER.ToExecutionResult();
                mSQLQuery = Query;
                Parameters = DapperHelper.Utilities.GetDynamicParameters(Params);
                ER.Value = _ModelItem;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"SQLQuery = {Query}";
                LastExecutionResult = ER.ToExecutionResult();
                HandleException(ER.ToExecutionResult());
            }
            return ER;
        }

        /// <summary>
        /// Gets the item asynchronously.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result with the item.</returns>
        public async Task<ExecutionResult<ModelClass>> GetItemAsync(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult<ModelClass>($"{mClassName}.GetItemAsync()");
            ER.Value = null;

            try
            {
                _ModelItem = (await DbConnection.QueryAsync<ModelClass>(Query, Params, Transaction, CommandTimeout)).SingleOrDefault();
                if (ViewModel != null)
                {
                    ViewModel.ModelItem = _ModelItem;
                }
                SetModelItemShadow();
                LastExecutionResult = ER.ToExecutionResult();
                mSQLQuery = Query;
                Parameters = DapperHelper.Utilities.GetDynamicParameters(Params);
                ER.Value = _ModelItem;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"SQLQuery = {Query}";
                LastExecutionResult = ER.ToExecutionResult();
                HandleException(ER.ToExecutionResult());
            }
            return ER;
        }

        /// <summary>
        /// Gets the items asynchronously.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result with the list of items.</returns>
        public async Task<ExecutionResult<IList<ModelClass>>> GetItemsAsync(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult<IList<ModelClass>>($"{mClassName}.GetItemsAsync()");
            if (Equals(Query, ""))
            {
                Query = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            _CurrentModelItemIndex = -1;
            try
            {
                _ModelItemsShadow = new List<ModelClass>();
                _ModelItems = (await DbConnection.QueryAsync<ModelClass>(Query, Params, Transaction, CommandTimeout)).ToList();
                if (_ModelItems.Count() > 0)
                {
                    _ModelItem = _ModelItems.First();
                    _CurrentModelItemIndex = 0;
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _ModelItem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                    _CurrentModelItemIndex = 0;
                }
                SQLQuery = Query;
                Parameters = DapperHelper.Utilities.GetDynamicParameters(Params);
                ER.Value = _ModelItems;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {Query}";
                HandleException(ER.ToExecutionResult());
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;
        }


        /// <summary>
        /// Gets the current item.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetCurrentItem()
        {
            if (_ModelItems != null && _CurrentModelItemIndex > -1)
            {
                return _ModelItems[_CurrentModelItemIndex];
            }
            return null;
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetAllItems(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            return GetItems(mSQLQuery, Parameters, Transaction, Buffered, CommandTimeout);
        }



        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public async Task<ExecutionResult<IList<ModelClass>>>GetAllItemsAsync(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            string query = mSQLQuery;
            if (string.IsNullOrWhiteSpace(query))
            {
                query = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            return await GetItemsAsync(query, Parameters, Transaction, Buffered, CommandTimeout);
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="Params">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetItems(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult<IList<ModelClass>>($"{mClassName}.GetItems()");
            //ValidateConnection();

            if (Equals(Query, ""))
            {
                Query = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            _CurrentModelItemIndex = -1;
            try
            {
                _ModelItemsShadow = new List<ModelClass>();
                //_ModelItemsShadow.Clear();
                _ModelItems = DbConnection.Query<ModelClass>(Query, Params, Transaction, Buffered, CommandTimeout).ToList();
                if (_ModelItems.Count() > 0)
                {
                    _ModelItem = _ModelItems.First();
                    _CurrentModelItemIndex = 0;
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _ModelItem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                    _CurrentModelItemIndex = 0;
                }
                SQLQuery = Query;
                Parameters = DapperHelper.Utilities.GetDynamicParameters(Params);
                ER.Value = _ModelItems;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {Query}";
                HandleException(ER.ToExecutionResult());
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }

        /// <summary>
        /// Reloads the items.
        /// </summary>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult ReloadItems(bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.ReloadItems()");
            try
            {
                if (mSQLQuery.IsNullOrWhiteSpace() == false)
                {
                    _ModelItems = DbConnection.Query<ModelClass>(mSQLQuery, Parameters, DbTransaction, Buffered, CommandTimeout).ToList();
                }

                if (_ModelItems.Count() > 0)
                {
                    _ModelItem = _ModelItems.First();
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _ModelItem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                }

            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {mSQLQuery}";
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;

        }





        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Repository<ModelClass> Clone()
        {
            Repository<ModelClass> newrepository = new Repository<ModelClass>();
            newrepository.DbConnection = DbConnection;
            //newrepository.DbContext = DbContext;
            return newrepository;
        }

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItem(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.InsertItem()");
            long x = 0;
            if (Model == null)
            {
                Model = ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {

                x = DbConnection.Insert(Model, Transaction, CommandTimeout);

                ModelItem = Model;
                ModelItemShadow = Model;
                if (ModelItems == null)
                {
                    ModelItems = new List<ModelClass>();
                }
                

                if (ModelItemsShadow == null)
                {
                    ModelItemsShadow = new List<ModelClass>();
                }
                
                ModelItemsShadow.Add(Model);
                

                mAddNewState = false;


            }
            catch (Exception ex)
            {
                //mAddNewState = False
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);

            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Inserts the items.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItems(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItems()");
            long x = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }
            try
            {

                x = DbConnection.Insert(ModelItems, Transaction, CommandTimeout);
                mAddNewState = false;
                ER.Value = x;

            }
            catch (Exception ex)
            {
                //mAddNewState = False
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = 0;
                HandleException(ER);

            }

            LastExecutionResult = (ER);
            return ER;

        }


        /// <summary>
        /// Inserts the item asynchronously.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItemAsync()");
            long x = 0;
            if (Model == null)
            {
                Model = ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                x = await DbConnection.InsertAsync(Model, Transaction, CommandTimeout);

                ModelItem = Model;
                ModelItemShadow = Model;
                if (ModelItems == null)
                {
                    ModelItems = new List<ModelClass>();
                }
                if (ModelItemsShadow == null)
                {
                    ModelItemsShadow = new List<ModelClass>();
                }
                ModelItemsShadow.Add(Model);
                mAddNewState = false;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }
            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Inserts the items asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> InsertItemsAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItemsAsync()");
            long x = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }
            try
            {
                x = await DbConnection.InsertAsync(ModelItems, Transaction, CommandTimeout);
                mAddNewState = false;
                ER.Value = x;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Value = 0;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }



        /// <summary>
        /// Undoes the changes.
        /// </summary>
        /// <returns></returns>
        public bool UndoChanges()
        {
            //var ER = new ExecutionResult($"{mClassName}.UndoChanges()");
            var result = false;
            if (ModelItemShadow != null)
            {
                ModelItem = Utilities.Clone(ModelItemShadow);
                ModelItem = Utilities .WisejClone(ModelItemShadow); 
            }
            //ModelItem = ModelItemShadow;
            if (AddNewState == true)
            {
                AddNewState = false;
            }
            return result;
        }


        /// <summary>
        /// Updates the item asynchronously.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemAsync(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemAsync()");
            bool result = false;

            if (Model == null)
            {
                Model = _ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                result = await DbConnection.UpdateAsync(Model, Transaction, CommandTimeout);
                if (result)
                {
                    _ModelItemShadow = Model;
                }
                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Updates the item ex asynchronously.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="ModelItemShadow">The model item shadow.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemExAsync_OLD(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemExAsync()");
            int result = 0;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
            }
            if (ModelItemShadow == null)
            {
                ModelItemShadow = _ModelItemShadow;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                if (!_compareFunc(ModelItem, ModelItemShadow))
                {
                    DynamicParameters @params = new DynamicParameters();
                    foreach (PropertyInfo k in EntityProperties)
                    {
                        @params.Add($"{k.Name}", ReflectionHelper.GetPropertyValue(ModelItem, k.Name));
                    }
                    foreach (PropertyInfo k in EntityPrimaryKeys)
                    {
                        @params.Add($"{k.Name}_shadow", ReflectionHelper.GetPropertyValue(ModelItemShadow, k.Name));
                    }
                    result = await DbConnection.ExecuteAsync(mSqlUpdateCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                    if (result > 0)
                    {
                        _ModelItemShadow = ModelItem;
                    }
                }
                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }
            LastExecutionResult = ER;
            return ER;
        }

        public async Task<ExecutionResult> UpdateItemExAsync(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemExAsync()");
            int result = 0;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
            }
            if (ModelItemShadow == null)
            {
                ModelItemShadow = _ModelItemShadow;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                // Early exit se gli oggetti sono uguali
                if (_compareFunc(ModelItem, ModelItemShadow))
                {
                    ER.Value = 0;
                    return ER;
                }

                var @params = new DynamicParameters();

                // Loop ottimizzato con accesso diretto all'indice
                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < entityPropsCount; i++)
                {
                    var prop = EntityProperties[i];
                    @params.Add(prop.Name, prop.GetValue(ModelItem));
                }

                for (int i = 0; i < primaryKeysCount; i++)
                {
                    var prop = EntityPrimaryKeys[i];
                    @params.Add($"{prop.Name}_shadow", prop.GetValue(ModelItemShadow));
                }

                result = await DbConnection.ExecuteAsync(mSqlUpdateCommand, @params, Transaction, CommandTimeout, CommandType.Text);

                if (result > 0)
                {
                    _ModelItemShadow = ModelItem;
                }

                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }
            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Updates the items asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemsAsync(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsAsync()");
            bool esito = false;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                esito = await DbConnection.UpdateAsync(ModelItems, Transaction, CommandTimeout);
                ER.Value = esito;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                esito = false;
            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Updates the items ex asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="ModelItemsShadow">The model items shadow.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> UpdateItemsExAsync_OLD(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsExAsync()");
            int affectedrecords = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (ModelItemsShadow == null)
            {
                ModelItemsShadow = this.ModelItemsShadow;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                DynamicParameters parameters;
                for (int i = 0; i < ModelItems.Count(); i++)
                {
                    parameters = new DynamicParameters();
                    if (!_compareFunc(ModelItems.ElementAt(i), ModelItemsShadow.ElementAt(i)))
                    {
                        foreach (var k in EntityProperties)
                        {
                            parameters.Add($"{k.Name}", ReflectionHelper.GetPropertyValue(ModelItems.ElementAt(i), k.Name));
                        }
                        foreach (var k in EntityPrimaryKeys)
                        {
                            parameters.Add($"{k.Name}_shadow", ReflectionHelper.GetPropertyValue(ModelItemsShadow.ElementAt(i), k.Name));
                        }
                        affectedrecords += await DbConnection.ExecuteAsync(mSqlUpdateCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                        _ModelItemsShadow[i] = ModelItems.ElementAt(i);
                    }
                }
                ER.Value = affectedrecords;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedrecords;
            }
            LastExecutionResult = ER;
            return ER;
        }
        public async Task<ExecutionResult> UpdateItemsExAsync(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsExAsync()");
            int affectedrecords = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (ModelItemsShadow == null)
            {
                ModelItemsShadow = this.ModelItemsShadow;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                // Converti una sola volta a IList
                var itemsList = ModelItems as IList<ModelClass> ?? ModelItems.ToList();
                var shadowsList = ModelItemsShadow as IList<ModelClass> ?? ModelItemsShadow.ToList();

                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < itemsList.Count; i++)
                {
                    // Skip se non ci sono modifiche
                    if (_compareFunc(itemsList[i], shadowsList[i]))
                    {
                        continue;
                    }

                    var parameters = new DynamicParameters();

                    // Loop ottimizzato con accesso diretto all'indice
                    for (int j = 0; j < entityPropsCount; j++)
                    {
                        var prop = EntityProperties[j];
                        parameters.Add(prop.Name, prop.GetValue(itemsList[i]));
                    }

                    for (int j = 0; j < primaryKeysCount; j++)
                    {
                        var prop = EntityPrimaryKeys[j];
                        parameters.Add($"{prop.Name}_shadow", prop.GetValue(shadowsList[i]));
                    }

                    affectedrecords += await DbConnection.ExecuteAsync(mSqlUpdateCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                    _ModelItemsShadow[i] = itemsList[i];
                }

                ER.Value = affectedrecords;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedrecords;
            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItem(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItem()");
            //ValidateConnection();
            bool result = false;

            if (Model == null)
            {
                Model = _ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                result = DbConnection.Update(Model, Transaction, CommandTimeout);
                if (result)
                {
                    _ModelItemShadow = Model;
                }

            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);

            }
            LastExecutionResult = ER;
            return ER;

        }
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _entityPropertiesCache = new();
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _entityPrimaryKeysCache = new();
        private List<PropertyInfo> _entityProperties;
        private List<PropertyInfo> _entityPrimaryKeys;
        /// <summary>
        /// The entity properties
        /// </summary>
        /// 
        public List<PropertyInfo> EntityProperties
        {
            get
            {
                if (_entityProperties == null)
                {
                    _entityProperties = _entityPropertiesCache.GetOrAdd(typeof(ModelClass), type =>
                        DapperHelper.Utilities.GetPropertiesInfo(type, true));
                }
                return _entityProperties;
            }
            set
            {
                _entityProperties = value;
                _entityPropertiesCache[typeof(ModelClass)] = value; // Aggiorna il cache
            }
        }
        //public List<PropertyInfo> EntityProperties = DapperHelper.Utilities.GetPropertiesInfo(typeof(ModelClass), true);

        /// <summary>
        /// The entity primary keys
        /// </summary>
        public List<PropertyInfo> EntityPrimaryKeys
        {
            get
            {
                if (_entityPrimaryKeys == null)
                {
                    _entityPrimaryKeys = _entityPrimaryKeysCache.GetOrAdd(typeof(ModelClass), type =>
                        DapperHelper.Utilities.GetPrimaryKeysPropertiesInfo(type));
                }
                return _entityPrimaryKeys;
            }
            set
            {
                _entityPrimaryKeys = value;
                _entityPrimaryKeysCache[typeof(ModelClass)] = value; // Aggiorna il cache
            }
        }
        //public List<PropertyInfo> EntityPrimaryKeys = DapperHelper.Utilities.GetPrimaryKeysPropertiesInfo(typeof(ModelClass));



        /// <summary>
        /// The m SQL update command
        /// </summary>
        private string mSqlUpdateCommand = DapperHelper.Utilities.GetUpdateSqlCommand(typeof(ModelClass));
        /// <summary>
        /// SQLs the update command.
        /// </summary>
        /// <param name="Refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public string SqlUpdateCommand(bool Refresh = false)
        {
            if (Refresh)
            {
                mSqlUpdateCommand = DapperHelper.Utilities.GetUpdateSqlCommand(typeof(ModelClass));
                EntityPrimaryKeys = DapperHelper.Utilities.GetPrimaryKeysPropertiesInfo(typeof(ModelClass));
                EntityProperties = DapperHelper.Utilities.GetPropertiesInfo(typeof(ModelClass), true);
            }
            return mSqlUpdateCommand;
        }

        /// <summary>
        /// Updates the item ex.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="ModelItemShadow">The model item shadow.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItemEx_OLD(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemEx()");
            //ValidateConnection();
            int result = 0;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
            }
            if (ModelItemShadow == null)
            {
                ModelItemShadow = _ModelItemShadow;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                //if (ReflectionHelper.Compare<ModelClass>(ModelItem, ModelItemShadow) == false)
                if (!_compareFunc(ModelItem, ModelItemShadow))
                {
                    DynamicParameters @params = new DynamicParameters();
                    foreach (PropertyInfo k in EntityProperties)
                    {
                        @params.Add($"{k.Name}", ReflectionHelper.GetPropertyValue(ModelItem, k.Name));
                    }
                    foreach (PropertyInfo k in EntityPrimaryKeys)
                    {
                        @params.Add($"{k.Name}_shadow", ReflectionHelper.GetPropertyValue(ModelItemShadow, k.Name));
                    }
                    result = DbConnection.Execute(mSqlUpdateCommand, @params, Transaction, CommandTimeout, CommandType.Text);
                    if (result > 0)
                    {
                        _ModelItemShadow = ModelItem;
                    }
                }

            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);

            }
            LastExecutionResult = ER;
            return ER;

        }


        public ExecutionResult UpdateItemEx(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemEx()");
            int result = 0;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
            }
            if (ModelItemShadow == null)
            {
                ModelItemShadow = _ModelItemShadow;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                // Early exit se gli oggetti sono uguali
                if (_compareFunc(ModelItem, ModelItemShadow))
                {
                    ER.Value = 0;
                    return ER;
                }

                var @params = new DynamicParameters();

                // Pre-calcolo della capacità per EntityProperties
                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                // Loop ottimizzato con accesso diretto all'indice per List<PropertyInfo>
                for (int i = 0; i < entityPropsCount; i++)
                {
                    var prop = EntityProperties[i];
                    @params.Add(prop.Name, prop.GetValue(ModelItem));
                }

                // Loop ottimizzato per primary keys
                for (int i = 0; i < primaryKeysCount; i++)
                {
                    var prop = EntityPrimaryKeys[i];
                    @params.Add($"{prop.Name}_shadow", prop.GetValue(ModelItemShadow));
                }

                result = DbConnection.Execute(mSqlUpdateCommand, @params, Transaction, CommandTimeout, CommandType.Text);

                if (result > 0)
                {
                    _ModelItemShadow = ModelItem;
                }

                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Updates the items.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItems(IEnumerable<ModelClass> ModelItems = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");
           
            bool esito = false;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                esito = DbConnection.Update(ModelItems);
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                esito = false;
            }
            LastExecutionResult = ER;
            return ER;

        }

        /// <summary>
        /// Updates the items ex.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="ModelItemsShadow">The model items shadow.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItemsEx_OLD(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");
            int affectedrecords = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (ModelItemsShadow == null)
            {
                ModelItemsShadow = this.ModelItemsShadow;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }


            try
            {
                DynamicParameters parameters;
                for (int i = 0; i < ModelItems.Count(); i++)
                {
                    parameters = new DynamicParameters();
                    //if (!ReflectionHelper.Compare<ModelClass>(ModelItems.ElementAt(i), ModelItemsShadow.ElementAt(i)))
                    if (!_compareFunc(ModelItems.ElementAt(i), ModelItemsShadow.ElementAt(i)))
                    {
                        foreach (var k in EntityProperties)
                        {
                            parameters.Add($"{k.Name}", ReflectionHelper.GetPropertyValue(ModelItems.ElementAt(i), k.Name));
                        }
                        foreach (var k in EntityPrimaryKeys)
                        {
                            parameters.Add($"{k.Name}_shadow", ReflectionHelper.GetPropertyValue(ModelItemsShadow.ElementAt(i), k.Name));
                        }
                        affectedrecords += DbConnection.Execute(mSqlUpdateCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                        _ModelItemsShadow[i] = ModelItems.ElementAt(i);
                    }
                }

            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedrecords;
            }
            LastExecutionResult = ER;
            return ER;
        }

        public ExecutionResult UpdateItemsEx(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemsEx()");
            int affectedrecords = 0;

            if (ModelItems == null)
            {
                ModelItems = this.ModelItems;
            }

            if (ModelItemsShadow == null)
            {
                ModelItemsShadow = this.ModelItemsShadow;
            }

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                // Converti una sola volta a IList
                var itemsList = ModelItems as IList<ModelClass> ?? ModelItems.ToList();
                var shadowsList = ModelItemsShadow as IList<ModelClass> ?? ModelItemsShadow.ToList();

                var entityPropsCount = EntityProperties.Count;
                var primaryKeysCount = EntityPrimaryKeys.Count;

                for (int i = 0; i < itemsList.Count; i++)
                {
                    // Skip se non ci sono modifiche
                    if (_compareFunc(itemsList[i], shadowsList[i]))
                    {
                        continue;
                    }

                    var parameters = new DynamicParameters();

                    // Loop ottimizzato con accesso diretto all'indice
                    for (int j = 0; j < entityPropsCount; j++)
                    {
                        var prop = EntityProperties[j];
                        parameters.Add(prop.Name, prop.GetValue(itemsList[i]));
                    }

                    for (int j = 0; j < primaryKeysCount; j++)
                    {
                        var prop = EntityPrimaryKeys[j];
                        parameters.Add($"{prop.Name}_shadow", prop.GetValue(shadowsList[i]));
                    }

                    affectedrecords += DbConnection.Execute(mSqlUpdateCommand, parameters, Transaction, CommandTimeout, CommandType.Text);
                    _ModelItemsShadow[i] = itemsList[i];
                }

                ER.Value = affectedrecords;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
                ER.Value = affectedrecords;
            }
            LastExecutionResult = ER;
            return ER;
        }


        /// <summary>
        /// Gets the empty model item.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItem(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItem()");

            bool _result = false;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                _result = DbConnection.Delete(ModelItem, Transaction, CommandTimeout);
                if (_result)
                {
                    _ModelItems.Remove(ModelItem);
                    //If (AutoUpdateModelItemsShadows) Then
                    _ModelItemsShadow.Remove(ModelItem);
                    //End If
                    _ModelItem = GetEmptyModelItem();
                    _ModelItemShadow = GetEmptyModelItem();
                }
                ER.Value = _result;

            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);

            }

            LastExecutionResult = ER;
            return ER;

        }



        /// <summary>
        /// Deletes the items.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItems(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            bool result = false;

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {

                //result = DbConnection.Delete<List<ModelClass>>((List<ModelClass>)ModelItems, Transaction, CommandTimeout);
                result = DbConnection.Delete(ModelItems, Transaction, CommandTimeout);
                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;

                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;

        }


        /// <summary>
        /// Deletes the item asynchronously.
        /// </summary>
        /// <param name="ModelItem">The model item.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemAsync(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemAsync()");

            bool _result = false;

            if (ModelItem == null)
            {
                ModelItem = _ModelItem;
            }
            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                _result = await DbConnection.DeleteAsync(ModelItem, Transaction, CommandTimeout);
                if (_result)
                {
                    _ModelItems.Remove(ModelItem);
                    _ModelItemsShadow.Remove(ModelItem);
                    _ModelItem = GetEmptyModelItem();
                    _ModelItemShadow = GetEmptyModelItem();
                }
                ER.Value = _result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Deletes the items asynchronously.
        /// </summary>
        /// <param name="ModelItems">The model items.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the execution result.</returns>
        public async Task<ExecutionResult> DeleteItemsAsync(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemsAsync()");
            bool result = false;

            if (Transaction == null)
            {
                Transaction = DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = DbCommandTimeout;
            }

            try
            {
                result = await DbConnection.DeleteAsync(ModelItems, Transaction, CommandTimeout);
                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                HandleException(ER);
            }

            LastExecutionResult = ER;
            return ER;
        }

        /// <summary>
        /// Gets or sets the default SQL query.
        /// </summary>
        /// <value>
        /// The default SQL query.
        /// </value>
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
            Parameters = DapperHelper.Utilities.GetDynamicParameters(parameters);
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

        public async Task<ExecutionResult> ExecuteInTransactionScope(
       Func<IDbTransaction, Task> operation,
       IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
       int retryCount = 3)
        {
            var er = new ExecutionResult($"{mClassName}.ExecuteInTransactionScope()");
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    //ValidateConnection();
                    using (var transaction = DbConnection.BeginTransaction(isolationLevel))
                    {
                        await operation(transaction);
                        transaction.Commit();
                        return er;
                    }
                }
                catch (Exception ex) when (i < retryCount - 1)
                {
                    await Task.Delay(100 * (i + 1)); // Ritenta con un breve delay
                }
                catch (Exception ex)
                {
                    er.Exception = ex;
                    er.ResultMessage = ex.Message;
                    HandleException(er);
                    return er;
                }
            }
            return er;
        }



    }

    //public class TransactionScope : IDisposable
    //{
    //    public IDbTransaction Transaction { get; } // Proprietà pubblica per accedere alla transazione
    //    private bool _committed;

    //    public TransactionScope(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    //    {
    //        Transaction = connection.BeginTransaction(isolationLevel);
    //    }

    //    public void Commit()
    //    {
    //        Transaction.Commit();
    //        _committed = true;
    //    }

    //    public void Dispose()
    //    {
    //        if (!_committed)
    //            Transaction.Rollback();
    //        Transaction.Dispose();
    //    }
    //}

}