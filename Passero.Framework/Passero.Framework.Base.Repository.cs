using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions ;
using Microsoft.Ajax.Utilities;
namespace Passero.Framework

{
  


    [Serializable]
    public class Repository<ModelClass> where ModelClass : class
    {
        private const string mClassName = "Passero.Framework.Base.Repository";
        private Dictionary<string, System.Reflection.PropertyInfo> ModelProperties;
        public string Name { get; set; } = $"Repository<{typeof(ModelClass).FullName}>";
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult(mClassName);
        public ViewModel<ModelClass> ViewModel { get; set; }
        public ErrorNotificationModes ErrorNotificationMode { get; set; } = ErrorNotificationModes.ThrowException;
        
        public ErrorNotificationMessageBox ErrorNotificationMessageBox { get; set; }
        public DynamicParameters Parameters { get; set; }
        
        public event EventHandler ModelEvents;
        private string mSQLQuery = "";
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

        public void ResetModelItem(bool ResetModelItems=true)
        {
            ModelItem = GetEmptyModelItem();
            if (ResetModelItems )
                ModelItems = new List<ModelClass>();
        }
        public void ResetModelItems()
        {
            ModelItems = new List<ModelClass>();
        }

        protected virtual void OnModelEvents(EventArgs e)
        {
            ModelEvents?.Invoke(this, e);
        }

        private int _AddNewCurrentModelItemIndex = -1;

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


        private int _CurrentModelItemIndex = -1;

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
                        _Modeltem = _ModelItems.ElementAt(_CurrentModelItemIndex);
                    }
                }
            }
        }


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


        public ExecutionResult MoveFirstItem()
        {
            var ERContext = $"{mClassName}.MoveFirstItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                _CurrentModelItemIndex = 0;
                _Modeltem = _ModelItems.ElementAt(0);
            }
            else
            {
                _Modeltem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Invalid Index Position.";
            }
            return ER;
        }


        public ExecutionResult MoveLastItem()
        {
            var ERContext = $"{mClassName}.MoveLastItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                _CurrentModelItemIndex = _ModelItems.Count() - 1;
                _Modeltem = _ModelItems.ElementAt(_CurrentModelItemIndex);
            }
            else
            {
                _Modeltem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Invalid Index Position.";
            }
            return ER;
        }


        public ExecutionResult MovePreviousItem()
        {
            var ERContext = $"{mClassName}.MovePreviousItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                if (_CurrentModelItemIndex > 0)
                {
                    _CurrentModelItemIndex -= 1;
                    _Modeltem = _ModelItems.ElementAt(_CurrentModelItemIndex);
                }
            }
            else
            {
                _Modeltem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Invalid Index Position.";
            }
            return ER;
        }


        public ExecutionResult MoveNextItem()
        {
            var ERContext = $"{mClassName}.MoveNextItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                if (_CurrentModelItemIndex < _ModelItems.Count() - 1)
                {
                    _CurrentModelItemIndex += 1;
                    _Modeltem = _ModelItems.ElementAt(_CurrentModelItemIndex);
                }
            }
            else
            {
                _Modeltem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Invalid Index Position.";
            }
            return ER;
        }



        public ExecutionResult MoveAtItem(int Index)
        {
            var ERContext = $"{mClassName}.MoveAtItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                if (Index >= 0 && Index < _ModelItems.Count())
                {
                    _CurrentModelItemIndex = Index;
                    _Modeltem = _ModelItems.ElementAt(Index);
                }
            }
            else
            {
                _Modeltem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Invalid Index Position.";
            }
            return ER;
        }


        private List <ModelClass>? _ModelItems { get; set; }= new List<ModelClass> ();  
        public List<ModelClass>? ModelItems
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


        private ModelClass? _Modeltem { get; set; } 
        public ModelClass? ModelItem
        {
            get
            {
                return _Modeltem;
            }
            set
            {
                _Modeltem = value;
            }
        }

#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        private ModelClass? _ModelItemShadow { get; set; }
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
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

        private List<ModelClass> _ModelItemsShadow { get; set; } = new List<ModelClass>();
        public List<ModelClass> ModelItemsShadow
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

        private bool mAddNewState { get; set; }
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
         public void AddNew()
        {
            if (this.AddNewState == false) 
            {
                AddNewState = true; 
            }
        }

        public string Description { get; set; }
        //public SqlConnection SqlConnection { get; set; }
        public IDbConnection DbConnection { get; set; }
        //public SqlTransaction SqlTransaction { get; set; }
        public IDbTransaction DbTransaction { get; set; }
        public int DbCommandTimeout { get; set; } = 30;

        public DbContext DbContext { get; set; }
        public DbObject<ModelClass> DbObject { get; set; }

        public ModelClass GetModelItemClone()
        {
            return Utilities.Clone(_Modeltem);
        }

        public List<ModelClass> GetModelItemsClone()
        {
            return Utilities.Clone(_ModelItems);
        }


        public void SetModelItemShadow()
        {
            _ModelItemShadow = Utilities.Clone(_Modeltem);
            //TBD: verifica se è superfluo
            if (ViewModel != null)
            {
                ViewModel.ModelItemShadow = _ModelItemShadow;
            }
        }

        public void SetModelItemsShadow()
        {
            _ModelItemsShadow = Utilities.Clone(_ModelItems);
            if (this.ViewModel != null)
            {
                this.ViewModel.ModelItemShadow = _ModelItemShadow;
            }
        }

        public ModelClass GetEmptyModel()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }

                
        private void CreateDbObject()
        {
            DbObject = new DbObject<ModelClass>(DbConnection);
        }
              
        public Repository(IDbConnection  SqlConnection, IDbTransaction SqlTransaction = null)
        {
           
            _Modeltem = GetEmptyModel();
            SetModelItemShadow();
            this.DbTransaction = SqlTransaction;
            this.DbConnection = SqlConnection;
            DbObject = new DbObject<ModelClass>(this.DbConnection);
            
        }

        public Repository(ModelClass Model)
        {
          
            _Modeltem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            DbObject = new DbObject<ModelClass>(DbConnection);
            

        }

        public Repository()
        {
          
            _Modeltem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            DbObject = new DbObject<ModelClass>(DbConnection);
            

        }

        public Repository(DbContext DbContext)
        {
           
            _Modeltem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemShadow();
            this.DbContext = DbContext;
            DbTransaction = DbContext.SqlTransaction;
            DbConnection = DbContext.SqlConnection;
            DbObject = new DbObject<ModelClass>(DbConnection);
            

        }


        public bool IsModelDataChanged(ModelClass ModelShadow = null)
        {

            if (ModelShadow is null)
            {
                ModelShadow = _ModelItemShadow;
            }

            return !Utilities.ObjectsEquals(_Modeltem, ModelShadow);

        }


        public void HandleExeception(ExecutionResult ER)
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


        public void SetSQLQuery(string SQLQuery, DynamicParameters parameters )
        {
            this.SQLQuery = SQLQuery;
            this.Parameters = parameters;   

        }

      


        public ExecutionResult<ModelClass> GetItem(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult<ModelClass>($"{mClassName}.GetItem()");
            ER.Value = null;
            try
            {
                _Modeltem = DbConnection.Query<ModelClass>(Query, Params, Transaction, Buffered, CommandTimeout).SingleOrDefault();
                if (ViewModel != null)
                {
                    ViewModel.ModelItem = _Modeltem;
                }
                SetModelItemShadow();
                LastExecutionResult = ER.ToExecutionResult();
                mSQLQuery = Query;
                Parameters = DapperHelper.Utilities.GetDynamicParameters(Params);
                ER.Value = _Modeltem;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"SQLQuery = {Query}";
                LastExecutionResult = ER.ToExecutionResult();
                HandleExeception(ER.ToExecutionResult());
            }
            return ER;
        }



        public ModelClass GetCurrentItem()
        {
            if (this._ModelItems != null && this._CurrentModelItemIndex > -1)
            {
                return this._ModelItems[_CurrentModelItemIndex];
            }
            return null;    
        }

        public ExecutionResult<List<ModelClass>> GetAllItems(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            return GetItems(this.mSQLQuery, this.Parameters, Transaction, Buffered, CommandTimeout);
        }


        public ExecutionResult<List<ModelClass>> GetItems(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult<List<ModelClass>>($"{mClassName}.GetItems()");
            if (Equals(Query, ""))
            {
                Query = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
                Parameters = new DynamicParameters();
            }
            this._CurrentModelItemIndex = -1;
            try
            {
                _ModelItemsShadow = new List<ModelClass>();
                //_ModelItemsShadow.Clear();
                _ModelItems = DbConnection.Query<ModelClass>(Query, Params, Transaction, Buffered, CommandTimeout).ToList();
                if (_ModelItems.Count() > 0)
                {
                    _Modeltem = _ModelItems.First();
                    _CurrentModelItemIndex = 0;
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _Modeltem;
                    ViewModel.ModelItemsShadow = _ModelItemsShadow;
                    ViewModel.MoveFirstItem();
                    _CurrentModelItemIndex = 0;
                }
                this.SQLQuery = Query;
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
                HandleExeception(ER.ToExecutionResult());
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }

        public ExecutionResult ReloadItems(bool Buffered = true, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.ReloadItems()");
            try
            {
                if (this.mSQLQuery.IsNullOrWhiteSpace() == false)
                {
                    _ModelItems = DbConnection.Query<ModelClass>(mSQLQuery, Parameters, DbTransaction , Buffered, CommandTimeout).ToList();
                }

                if (_ModelItems.Count() > 0)
                {
                    _Modeltem = _ModelItems.First();
                    MoveFirstItem();
                    SetModelItemsShadow();
                }
                if (ViewModel != null)
                {
                    ViewModel.ModelItems = _ModelItems;
                    ViewModel.ModelItem = _Modeltem;
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
                ER.DebugInfo = $"Query = {this.mSQLQuery}";
                HandleExeception(ER);
            }

            LastExecutionResult = ER;
            return ER;

        }

        

       
       
        public Repository <ModelClass> Clone()
        {
            Repository<ModelClass> newrepository = new Repository<ModelClass>();
            newrepository .DbConnection = this.DbConnection;    
            newrepository.DbContext = this.DbContext;   
            return newrepository;   
        }

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
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
            }

            try
            {

                x = DbConnection.Insert(Model, Transaction, CommandTimeout);

                ModelItem = Model;
                ModelItemShadow = Model;
                if (ModelItems == null)
                {
                    ModelItems = new List<ModelClass>();
                    ModelItems.Add(Model);
                }
                if (ModelItemsShadow == null)
                {
                    ModelItemsShadow = new List<ModelClass>();
                    ModelItemsShadow.Add(Model);
                }
                //ModelItems.Add(Model)
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
                HandleExeception(ER);

            }
            LastExecutionResult = ER;
            return ER;
        }


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
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
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
                HandleExeception(ER);

            }

            LastExecutionResult = (ER);
            return ER;

        }


        public bool UndoChanges()
        {
            //var ER = new ExecutionResult($"{mClassName}.UndoChanges()");
            var result = false;
            ModelItem = ModelItemShadow;
            if (AddNewState == true)
            {
                AddNewState = false;
            }
            return result;
        }

        public ExecutionResult UpdateItem(ModelClass Model = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItem()");
            bool result = false;

            if (Model == null)
            {
                Model = _Modeltem;
            }
            if (Transaction == null)
            {
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
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
                HandleExeception(ER);

            }
            LastExecutionResult = ER;
            return ER;

        }

        public List<PropertyInfo> EntityProperties = DapperHelper.Utilities.GetPropertiesInfo(typeof(ModelClass), true);
        public List<PropertyInfo> EntityPrimaryKeys = DapperHelper.Utilities.GetPrimaryKeysPropertiesInfo(typeof(ModelClass));
        private string mSqlUpdateCommand = DapperHelper.Utilities.GetUpdateSqlCommand(typeof(ModelClass));
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

        public ExecutionResult UpdateItemEx(ModelClass ModelItem = null, ModelClass ModelItemShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItemEx()");
            int result = 0;

            if (ModelItem == null)
            {
                ModelItem = _Modeltem;
            }
            if (ModelItemShadow == null)
            {
                ModelItemShadow = _ModelItemShadow;
            }
            if (Transaction == null)
            {
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
            }

            try 
            { 
                if (ReflectionHelper.Compare<ModelClass>(ModelItem, ModelItemShadow) == false)
                {
                    DynamicParameters @params = new DynamicParameters();
                    foreach (PropertyInfo k in this.EntityProperties)
                    {
                        @params.Add($"{k.Name}", ReflectionHelper.GetPropertyValue(ModelItem, k.Name));
                    }
                    foreach (PropertyInfo k in this.EntityPrimaryKeys)
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
                HandleExeception(ER);

            }
            LastExecutionResult = ER;
            return ER;

        }


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
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
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
                HandleExeception(ER);
                esito = false;
            }
            LastExecutionResult = ER;
            return ER;

        }

        public ExecutionResult UpdateItemsEx(IEnumerable<ModelClass> ModelItems = null, IEnumerable<ModelClass> ModelItemsShadow = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
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
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
            }


            try
            {
                DynamicParameters parameters;
                for (int i = 0; i < ModelItems.Count(); i++)
                {
                    parameters = new DynamicParameters();
                    if (!ReflectionHelper.Compare<ModelClass>(ModelItems.ElementAt(i), ModelItemsShadow.ElementAt(i)))
                    {
                        foreach (var k in this.EntityProperties)
                        {
                            parameters.Add($"{k.Name}", ReflectionHelper.GetPropertyValue(ModelItems.ElementAt(i), k.Name));
                        }
                        foreach (var k in this.EntityPrimaryKeys)
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
                HandleExeception(ER);
                ER.Value = affectedrecords;
            }
            LastExecutionResult = ER;
            return ER;
        }

        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }

        public ExecutionResult DeleteItem(ModelClass ModelItem = null, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItem()");

            bool _result = false;

            if (ModelItem == null)
            {
                ModelItem = _Modeltem;
            }
            if (Transaction == null)
            {
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
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
                    _Modeltem = GetEmptyModelItem();
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
                HandleExeception(ER);

            }

            LastExecutionResult = ER;
            return ER;

        }



        public ExecutionResult DeleteItems(IEnumerable<ModelClass> ModelItems, IDbTransaction Transaction = null, int? CommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            bool result = false;

            if (Transaction == null)
            {
                Transaction = this.DbTransaction;
            }
            if (CommandTimeout == null)
            {
                CommandTimeout = this.DbCommandTimeout;
            }

            try
            {

                result = DbConnection.Delete(ModelItems, Transaction, CommandTimeout);
                ER.Value = result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;

                HandleExeception(ER);
            }

            LastExecutionResult = ER;
            return ER;

        }
        public string DefaultSQLQuery { get; set; } = "";
        private DynamicParameters mDefaultSQLQueryParameters;
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




        public string GetTableName()
        {
            string tableName = "";
            var type = typeof(ModelClass);
            var tableAttr = type.GetCustomAttribute<Dapper .Contrib .Extensions .TableAttribute>();

            if (tableAttr is not null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name; // & "s"
        }

        public bool SetTableName(string TableName)
        {


            var type = typeof(ModelClass);
            var tableAttr = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();

            var n = new System.ComponentModel.DataAnnotations.Schema.TableAttribute(TableName);
            n = tableAttr;

            Utilities.Assign(ref tableAttr, n);
            return default;

        }

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

        public string GetPropertyNames(bool excludeKey = false)
        {
            // Dim properties = GetType(ModelClass).GetProperties().Where(Function(p) Not excludeKey OrElse p.GetCustomAttribute(Of Dapper.Contrib.Extensions.KeyAttribute)() Is Nothing)
            var properties = typeof(ModelClass).GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return values;
        }

        public IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            // Dim properties = GetType(ModelClass).GetProperties().Where(Function(p) Not excludeKey OrElse p.GetCustomAttribute(Of Dapper.Contrib.Extensions.KeyAttribute)() Is Nothing)
            var properties = typeof(ModelClass).GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            return properties;
        }

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

    }
}