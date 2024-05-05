using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions ;
//using Dommel;
using Microsoft.ReportingServices.Diagnostics.Internal;

//namespace Passero.Framework.Base
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
        public string SQLQuery { get; set; } = "";

        public event EventHandler ModelEvents;

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
                if (_ModelItems is null)
                {
                    _CurrentModelItemIndex = -1;

                }
                else
                {
                    if (_CurrentModelItemIndex != value)
                    {
                        if (value < 0)
                            value = 0;
                        if (value > _ModelItems.Count() - 1)
                            value = _ModelItems.Count() - 1;
                        _CurrentModelItemIndex = value;
                        _Modeltem = _ModelItems.ElementAt(_CurrentModelItemIndex);
                    }
                }
            }
        }

        public ModelClass GetModelItemsAt(int index)
        {
            if (this._ModelItems is null)
                return null;
            if (index>0 && index <this._ModelItems.Count())  
                return this._ModelItems .ElementAt(index);
            return null;
        }

        public void MoveFirstItem()
        {
            if (this._ModelItems != null && this._ModelItems .Count >0)
            {
                this._CurrentModelItemIndex = 0;
                this._Modeltem = this._ModelItems.ElementAt(0);
            }
            else
            {
                this._Modeltem = null;
                this._CurrentModelItemIndex = -1;
            }
        }

        public void MoveLastItem()
        {
            if (this._ModelItems != null && this._ModelItems.Count > 0)
            {
                this._CurrentModelItemIndex = this._ModelItems .Count ()-1;
                this._Modeltem = this._ModelItems.ElementAt(this._CurrentModelItemIndex);
            }
            else
            {
                this._Modeltem = null;
                this._CurrentModelItemIndex = -1;
            }
        }

        public void MovePreviousItem()
        {
            if (this._ModelItems != null && this._ModelItems.Count > 0)
            {
                if (this._CurrentModelItemIndex>0 )
                {
                    this._CurrentModelItemIndex--;
                    this._Modeltem = this._ModelItems.ElementAt(this._CurrentModelItemIndex);
                }    
            }
            else
            {
                this._Modeltem = null;
                this._CurrentModelItemIndex = -1;
            }
        }

        public void MoveNextItem()
        {
            if (this._ModelItems != null && this._ModelItems.Count > 0)
            {
                if (this._CurrentModelItemIndex < this._ModelItems .Count()-1)
                {
                    this._CurrentModelItemIndex++;
                    this._Modeltem = this._ModelItems.ElementAt(this._CurrentModelItemIndex);
                }
            }
            else
            {
                this._Modeltem = null;
                this._CurrentModelItemIndex = -1;
            }
        }


        public void MoveAtItem(int Index)
        {
            if (this._ModelItems != null && this._ModelItems.Count > 0)
            { 
                if (Index >= 0 && Index < this._ModelItems.Count())
                {
                    this._CurrentModelItemIndex = Index;
                    this._Modeltem = this._ModelItems.ElementAt(Index);
                }
            }
            else
            {
                this._Modeltem = null;
                this._CurrentModelItemIndex = -1;
            }

        }


        private List <ModelClass>? _ModelItems { get; set; }
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
                //if (_ModelItems != null && _ModelItems.Count > 0)
                //{
                //    _Modeltem = _ModelItems.ElementAt(this._CurrentModelItemIndex );  
                //}
                return _Modeltem;
            }
            set
            {
                //if (_ModelItems != null && _ModelItems.Count > 0)
                //{
                //    if (this._CurrentModelItemIndex > -1)
                //    {
                //        _ModelItems[this._CurrentModelItemIndex] = value;
                //    }
                //}
                _Modeltem = value;
            }
        }

        private ModelClass? _ModelShadow { get; set; }
        public ModelClass? ModelItemShadow
        {
            get
            {
                return _ModelShadow;
            }
            set
            {
                _ModelShadow = value;
            }
        }

        private List<ModelClass> _ModelItemsShadow { get; set; }
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
        public IDbTransaction SqlTransaction { get; set; }
        public DbContext DbContext { get; set; }
        public DbObject<ModelClass> DbObject { get; set; }


        public ModelClass SetModelShadow()
        {
            _ModelShadow = Utilities.Clone(_Modeltem);
            if (this.ViewModel != null)
            {
                this.ViewModel.ModelItemShadow = _ModelShadow;
            }
            return _ModelShadow;
        }

        public List<ModelClass> SetModelItemsShadow()
        {
            _ModelItemsShadow = Utilities.Clone(_ModelItems);
            if (this.ViewModel != null)
            {
                this.ViewModel.ModelItemShadow = _ModelShadow;
            }
            return _ModelItemsShadow;
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
            SetModelShadow();
            this.SqlTransaction = SqlTransaction;
            this.DbConnection = SqlConnection;
            DbObject = new DbObject<ModelClass>(this.DbConnection);
            
        }

        public Repository(ModelClass Model)
        {
          
            _Modeltem = GetEmptyModel();
            SetModelShadow();
            DbObject = new DbObject<ModelClass>(DbConnection);
            

        }

        public Repository()
        {
          
            _Modeltem = GetEmptyModel();
            SetModelShadow();
            DbObject = new DbObject<ModelClass>(DbConnection);
            

        }

        public Repository(DbContext DbContext)
        {
           
            _Modeltem = GetEmptyModel();
            SetModelShadow();
            this.DbContext = DbContext;
            SqlTransaction = DbContext.SqlTransaction;
            DbConnection = DbContext.SqlConnection;
            DbObject = new DbObject<ModelClass>(DbConnection);
            

        }


        public bool IsModelDataChanged(ModelClass ModelShadow = null)
        {

            if (ModelShadow is null)
            {
                ModelShadow = _ModelShadow;
            }

            return !Utilities.ObjectsEquals(_Modeltem, ModelShadow);

        }


        public void HandleExeception(ExecutionResult executionResult )
        {
            if (this.ErrorNotificationMessageBox == null | executionResult ==null)
                return;

            
            
            StringBuilder msg= new StringBuilder();

            msg.Append($"{executionResult.Context}\n\r");
            msg.Append($"{this.Name}\n\r");
            msg.Append($"{executionResult.ResultMessage}");
            
            this.ErrorNotificationMessageBox .Show( msg.ToString() );   
            //Passero.Framework.ReflectionHelper.CallByName(this.ErrorNotificationMessageBox, "Show", Microsoft.VisualBasic.CallType.Method, msg);


        }
        public void SetSQLQuery(string SQLQuery, DynamicParameters parameters )
        {
            this.SQLQuery = SQLQuery;
            this.Parameters = parameters;   

        }
        public ModelClass GetItem(string Query, object Params = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = default)
        {
            var ER = new ExecutionResult($"{mClassName}.GetItem()");
            try
            {
                _Modeltem = DbConnection.Query<ModelClass>(Query, Params, Transaction, Buffered, CommandTimeout).Single();
                if (this.ViewModel != null)
                {
                    this.ViewModel.ModelItem = _Modeltem;
                }
                SetModelShadow();
                LastExecutionResult = ER;
                this.SQLQuery = Query;
                this.Parameters = (DynamicParameters)Params;
                return _Modeltem;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return null;
                
            }

        }


        public ModelClass GetCurrentItem()
        {
            if (this._ModelItems != null && this._CurrentModelItemIndex > -1)
            {
                return this._ModelItems[_CurrentModelItemIndex];
            }
            return null;    
        }

        public List<ModelClass> GetAllItems(System.Data.IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = default)
        {
            return GetItems (Buffered, CommandTimeout).ToList();
        }

        public List<ModelClass> GetItems(string Query, object Params = null, System.Data.IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = default)
        {
            var ER = new ExecutionResult($"{mClassName}.GetItems()");
            string sqlquery = "";
            try
            {
                this._ModelItems = DbConnection.Query<ModelClass>(Query, Params, Transaction, Buffered, CommandTimeout).ToList<ModelClass >();
                if (this.ViewModel != null)
                {
                    this.ViewModel.ModelItems = _ModelItems;
                    this.ViewModel.ModelItem = _Modeltem;
                    this.ViewModel.MoveFirstItem();
                    
                }
                if (this._ModelItems.Count() > 0)
                {
                    _Modeltem = this._ModelItems.First();
                    this.MoveFirstItem();
                    SetModelItemsShadow();
                }
                LastExecutionResult = ER;
                this.SQLQuery = sqlquery;
                this.Parameters = (DynamicParameters)Params;
                return _ModelItems;
                
            }

            catch (Exception ex)
            {
                
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"SQLQuery = {Query}";
                LastExecutionResult = ER;
                HandleExeception ( ER );    

                return null;
            }

        }

        public List<ModelClass> ReloadItems(bool Buffered = true, int? CommandTimeout = default)
        {
            var ER = new ExecutionResult($"{mClassName}.ReloadItems()");
            _ModelItems = DbConnection.Query<ModelClass>(this.SQLQuery, this.Parameters, this.SqlTransaction, Buffered, CommandTimeout).ToList<ModelClass>();
            if (this.ViewModel != null)
            {
                this.ViewModel.ModelItems = _ModelItems;
                this.ViewModel.ModelItem = _Modeltem;
                this.ViewModel.MoveFirstItem();
            }

            if (_ModelItems.Count() > 0)
            {
                _Modeltem = _ModelItems.First();
                this.MoveFirstItem();
                SetModelItemsShadow();
            }
            LastExecutionResult = ER;
            return _ModelItems; 
        }

        public List<ModelClass> GetItems(bool Buffered = true, int? CommandTimeout = default)
        {
            var ER = new ExecutionResult($"{mClassName}.GetItems()");
            try
            {
                if (this.SQLQuery =="")
                {
                    this.SQLQuery = $"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<ModelClass>()}";
                    this.Parameters = new DynamicParameters();
                }

                this._CurrentModelItemIndex = -1;
                //_ModelItems = DbConnection.GetAll<ModelClass>().ToList<ModelClass>();
                _ModelItems = DbConnection.Query<ModelClass >(this.SQLQuery,this.Parameters ,SqlTransaction ,Buffered ,CommandTimeout ).ToList<ModelClass>();

                if (this.ViewModel != null)
                {
                    this.ViewModel .ModelItems = _ModelItems;
                    this.ViewModel.ModelItem = _Modeltem;
                    this.ViewModel.MoveFirstItem();
                }

                if (_ModelItems.Count() >0)
                {
                    _Modeltem = _ModelItems.First();
                    this.MoveFirstItem();
                    SetModelItemsShadow();
                }
         
                LastExecutionResult = ER;
                
                return _ModelItems;
            }

            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return null;
            }

            
        }

       
        public Repository <ModelClass> Clone()
        {
            Repository<ModelClass> newrepository = new Repository<ModelClass>();
            newrepository .DbConnection = this.DbConnection;    
            newrepository.DbContext = this.DbContext;   
            return newrepository;   
        }

        public long InsertItem(ModelClass Model = null)
        {

            var ER = new ExecutionResult($"{mClassName}.InsertItem()");
            long x;
            if (Model is null)
            {
                Model = this.ModelItem;
            }

            try
            {
              
                x =  DbConnection.Insert(Model);
                
                this.mAddNewState = false;
                return x;
            }


            catch (Exception ex)
            {
                this.mAddNewState = false;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult = ER;
                
                HandleExeception(ER);
                this.UndoChanges();
                return 0L;
                
            }


        }

        public long InsertItems(IEnumerable<ModelClass> ModelItems)
        {
            var ER = new ExecutionResult($"{mClassName}.InsertItems()");
            long x;
            try
            {
                
                x= DbConnection.Insert(ModelItems);
                this.mAddNewState = false;
                return x;
            }

            catch (Exception ex)
            {
                this.mAddNewState = false;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return 0L;
                
            }


        }

        public bool UndoChanges()
        {
            var ER = new ExecutionResult("Passero.Framework.Base.Repository.UndoChanges()");
            bool result = false;

            this.ModelItem = this.ModelItemShadow;
            if (this.AddNewState == true)
                this.AddNewState = false;
            return result;

        }

        public bool UpdateItem(ModelClass Model = null)
        {

            var ER = new ExecutionResult("Passero.Framework.Base.Repository.UpdateItem()");
            bool result;
            if (Model is null)
            {
                Model = _Modeltem;
            }

            try
            {
                // TO DO: Creare metodo per aggiornare usando il valore delle chiavi primarie ricavato dal ModelShadow
                // Questo permetterebbe l'aggiornamento ANCHE delle colonne facenti parte della chiave primaria.
                result = DbConnection.Update(Model);
               

                result = DbConnection.Update<ModelClass>(Model);
                if (result)
                {
                    _ModelShadow = Model;
                }
                return result;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return false;
                //if (ErrorNotification)
                //{
                //    throw;
                //}
                //else
                //{
                //    return false;
                //}
            }

        }

        public bool UpdateItems(IEnumerable<ModelClass> ModelItems)
        {
            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");

            try
            {
                return DbConnection.Update(ModelItems);
            }

            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return false;
                //if (ErrorNotification)
                //{
                //    throw;
                //}
                //else
                //{
                //    return false;
                //}
            }


        }


        public bool DeleteItem(ModelClass Model = null)
        {
            var ER = new ExecutionResult("Passero.Framework.Base.ViewModelManager.Delete()");
            bool result;
            if (Model is null)
            {
                Model = _Modeltem;
            }

            try
            {
                result = DbConnection.Delete(Model);
                if (result)
                {
                    _ModelItems .Remove(Model);
                    _ModelItemsShadow.Remove(Model);

                    _Modeltem = GetEmptyModel();
                    _ModelShadow = GetEmptyModel();
                   
                }
                return result;
            }

            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return false;
                //if (ErrorNotification)
                //{
                //    throw;
                //}
                //else
                //{
                //    return false;
                //}
            }


        }

        public bool DeleteItems(IEnumerable<ModelClass> ModelItems)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");

            try
            {
                return DbConnection.Delete(ModelItems);
            }

            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                LastExecutionResult = ER;
                HandleExeception(ER);
                return false;
                //if (ErrorNotification)
                //{
                //    throw;
                //}
                //else
                //{
                //    return false;
                //}
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