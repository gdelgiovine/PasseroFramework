using Dapper;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Wisej.Web;



//namespace Passero.Framework.Base
namespace Passero.Framework
{
   

    public class ViewModel<ModelClass> where ModelClass : class
    {

        private const string mClassName = "Passero.Framework.Base.ViewModel";
        //public event EventHandler WriteControlsCompleted;
        //protected virtual void OnWriteControlsdCompleted(EventArgs e)
        //{
        //    WriteControlsCompleted?.Invoke(this, e);
        //}

        //public event EventHandler ReadControlsCompleted;
        //protected virtual void OnReadControlsdCompleted(EventArgs e)
        //{
        //    ReadControlsCompleted?.Invoke(this, e);
        //}
        private object _DataNavigator = null;
        public object DataNavigator
        {
            get { return _DataNavigator; }
            set { _DataNavigator = value; }
        }
        public Wisej.Web.Form OwnerView { get; set; }   
        public string Name { get; set; } = $"ViewModel<{typeof(ModelClass).FullName}>";
        public DateTime MinDateTime { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0);
        public DateTime MaxDateTime { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59, 999);
        public UseModelData UseModelData { get; set; } = UseModelData.InternalRepository;
        public BindingBehaviour bindingBehaviour { get; set; }=BindingBehaviour.SelectInsertUpdate;
        private ErrorNotificationMessageBox _ErrorNotificationMessageBox = new ErrorNotificationMessageBox();   
        private ErrorNotificationModes _ErrorNotificationMode = ErrorNotificationModes.ThrowException;
        private ModelClass _ModelItemShadow;
        private ModelClass ExternalModelShadow;
        private int _AddNewCurrentModelItemIndex = -1;
        private int _CurrentModelItemIndex=-1;
        private List<ModelClass> _ModelItems;
        private List<ModelClass> _ModelItemsShadow;
        private BindingSource mBindingSource;
        private Dictionary<string, Wisej.Web.Control> BindingSourceControls = new Dictionary<string, Control>();
        private DataBindingMode mDataBindingMode = DataBindingMode.Passero;
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult(mClassName);
        public ErrorNotificationModes  ErrorNotificationMode
        {
            get { return _ErrorNotificationMode; }
            set { _ErrorNotificationMode = value; this.Repository.ErrorNotificationMode = value; }
        }
        public ErrorNotificationMessageBox ErrorNotificationMessageBox        {
            get { return _ErrorNotificationMessageBox; }
            set { this._ErrorNotificationMessageBox = value; this.Repository.ErrorNotificationMessageBox = value; }
        } 

        public void HandleExeception(ExecutionResult executionResult)
        {
            if (this.ErrorNotificationMessageBox == null | executionResult == null)
                return;

            StringBuilder msg = new StringBuilder();

            msg.Append($"{executionResult.Context}");
            msg.Append($"{executionResult.ResultMessage}");
            msg.Append($"{this.Name}");

            Passero.Framework.ReflectionHelper.CallByName(this.ErrorNotificationMessageBox, "Show", Microsoft.VisualBasic.CallType.Method, msg);


        }


        public BindingSource BindingSource
        {
            get
            {
                return mBindingSource;  
            }
            set
            {
                this.mBindingSource = value;
            }
        }


        public Type? ModelType
        {
            get { return ModelItem.GetType(); }
            
        }


        public ModelClass? NewModeltem
        {
            get { return GetEmptyModel(); }
            set { NewModeltem = value; }
        }

        public ModelClass? ModelItem
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return _ModelItemShadow;
                    default:
                        return Repository.ModelItem;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        this._ModelItemShadow = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.ModelItem = value;
                        break;
                }
            }


        }
        public List<ModelClass>? ModelItems
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return _ModelItems;

                    default:
                        return Repository.ModelItems;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        this._ModelItems = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.ModelItems = value;
                        break;
                }
                //
                if (this.mBindingSource !=null )
                    this.mBindingSource .DataSource = value; 
                //
            }
        }
        public List<ModelClass> ModelItemsShadow
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return _ModelItemsShadow;

                    default:
                        return Repository.ModelItemsShadow;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        this._ModelItemsShadow = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.ModelItemsShadow  = value;
                        break;
                }
            }
        }
     

        public int ModelItemsCount
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return _ModelItems.Count;

                    default:
                        return Repository.ModelItems.Count;
                }
            }
        }
        public int CurrentModelItemIndex
        {
            get {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return _CurrentModelItemIndex ;

                    default:
                        return Repository.CurrentModelItemIndex;
                }
            }
            set {

                switch (UseModelData)
                {
                    case UseModelData.External:
                        _CurrentModelItemIndex = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.CurrentModelItemIndex = value;
                        break;
                }

            }  
        }

        public int AddNewCurrentModelItemIndex
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return _AddNewCurrentModelItemIndex;

                    default:
                        return Repository.AddNewCurrentModelItemIndex;
                }
            }
            set
            {

                switch (UseModelData)
                {
                    case UseModelData.External:
                        _AddNewCurrentModelItemIndex = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.AddNewCurrentModelItemIndex = value;
                        break;
                }

            }
        }

      
        public int CreatePasseroBindingFromBindingSource(Form Form=null, BindingSource BindingSource =null)
        {
            //if (this.DataBindingMode == DataBindingMode.BindingSource)
            //    return 0;

            if (Form == null )
                Form = this.OwnerView;
            
            if (Form == null)
                return 0;

            if (BindingSource == null)
                BindingSource = this.BindingSource;
            
            if (BindingSource == null)
                return 0;

            this.DataBindControls.Clear();

            foreach (Control item in Form.Controls)
            {
                if (item.HasDataBindings)
                {
                    foreach (Binding binding in item.DataBindings)
                    {
                        if (binding.DataSource == BindingSource)
                        {
                            this.AddControl(item, binding.PropertyName, binding.BindingMemberInfo.BindingField, this.bindingBehaviour);
                        }
                    }
                }
            }

            return this.DataBindControls.Count();
        }

        private ModelClass ExternalGetModelItemsAt(int index)
        {
            if (this._ModelItems is null)
                return null;
            if (index > 0 && index < this._ModelItems.Count())
                return this._ModelItems.ElementAt(index);
            return null;
        }

        public ModelClass GetModelItemsAt(int index)
        {
            if (UseModelData ==  UseModelData.External  )
                return ExternalGetModelItemsAt(index);  
            else
                return Repository.GetModelItemsAt(index);
        }

        public ModelClass ModelItemShadow
        {
            get
            {
                if (UseModelData == UseModelData.External  ) 
                    return this.ExternalModelShadow ;
                else
                    return Repository.ModelItemShadow;  
            }
            set
            {
                if (UseModelData == UseModelData.External  )
                    this.ExternalModelShadow = value;   
                else
                    Repository.ModelItemShadow = value;
                // Me.BindingSource = New BindingSource(_Model)
            }
        }

        private bool ExternalModelDataChanged(ModelClass ModelShadow = null)
        {
            if (ModelShadow is null)
            {
                ModelShadow = ExternalModelShadow;
            }
            return !Utilities.ObjectsEquals(_ModelItemShadow, ModelShadow);
        }

        public bool IsModelDataChanged(ModelClass ModelShadow = null)
        {
            if (UseModelData == UseModelData.InternalRepository)
                return Repository.IsModelDataChanged(ModelShadow);
            else
                return ExternalModelDataChanged(ModelShadow);
        }

        public void MoveFirstItem()
        {
            if (UseModelData == UseModelData.InternalRepository)
            {
                this.Repository.MoveFirstItem();   
            }
            else
            {
                if (this._ModelItemShadow  != null && this._ModelItems.Count > 0)
                {
                    this._CurrentModelItemIndex = 0;
                    this._ModelItemShadow = this._ModelItems.ElementAt(0);
                }
                else
                {
                    this._ModelItemShadow = null;
                    this._CurrentModelItemIndex = -1;
                }
            }
            this.SetModelShadow();

            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoWriteControls)
                        this.WriteControls();
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource.MoveFirst ();
                    break;
                default:
                    break;
            }

        }

        public void MoveLastItem()
        {
            if (UseModelData == UseModelData.InternalRepository)
            {
                this.Repository.MoveLastItem();
            }
            else
            {
                if (this._ModelItemShadow != null && this._ModelItems.Count > 0)
                {
                    this._CurrentModelItemIndex = this._ModelItems.Count() - 1;
                    this._ModelItemShadow = this._ModelItems.ElementAt(this._CurrentModelItemIndex);
                }
                else
                {
                    this._ModelItemShadow = null;
                    this._CurrentModelItemIndex = -1;
                }
            }
            this.SetModelShadow();

            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoWriteControls)
                        this.WriteControls();
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource.MoveLast();
                    break;
                default:
                    break;
            }

          
        }

        public void MovePreviousItem()
        {
            if (UseModelData == UseModelData.InternalRepository)
            {
                this.Repository.MovePreviousItem();
            }
            else
            {
                if (this._ModelItems != null && this._ModelItems.Count > 0)
                {
                    if (this._CurrentModelItemIndex > 0)
                    {
                        this._CurrentModelItemIndex--;
                        this._ModelItemShadow = this._ModelItems.ElementAt(this._CurrentModelItemIndex);
                    }
                }
                else
                {
                    this._ModelItemShadow = null;
                    this._CurrentModelItemIndex = -1;
                }
            }
            
            this.SetModelShadow();

            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if(AutoWriteControls)
                        this.WriteControls();
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource.MovePrevious();
                    break;
                default:
                    break;
            }

            
        }

        public void MoveNextItem()
        {

            if (UseModelData == UseModelData.InternalRepository)
            {
                this.Repository.MoveNextItem();
            }
            else
            {
                if (this._ModelItems != null && this._ModelItems.Count > 0)
                {
                    if (this._CurrentModelItemIndex < this._ModelItems.Count()-1)
                    {
                        this._CurrentModelItemIndex++;
                        this._ModelItemShadow = this._ModelItems.ElementAt(this._CurrentModelItemIndex);
                    }
                }
                else
                {
                    this._ModelItemShadow = null;
                    this._CurrentModelItemIndex = -1;
                }
            }
            this.SetModelShadow();

            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoWriteControls)
                        this.WriteControls();
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource.MoveNext();
                    break;
                default:
                    break;
            }

        }

        public void MoveAtItem(int Index)
        {
            if (UseModelData == UseModelData.InternalRepository)
            {           
                this.Repository.MoveAtItem(Index);
            }
            else
            {
                if (this._ModelItems != null && this._ModelItems.Count > 0)
                {
                    if (Index > 0 && Index < this._ModelItems.Count())
                    {
                        this._CurrentModelItemIndex = Index;
                        this._ModelItemShadow = this._ModelItems.ElementAt(Index);
                    }
                }
                else
                {
                    this._ModelItemShadow = null;
                    this._CurrentModelItemIndex = -1;
                }
            }
            this.SetModelShadow();
   
            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoWriteControls)
                        this.WriteControls();
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource.Position =this.CurrentModelItemIndex ;
                    break;
                default:
                    break;
            }
        }

        private bool mAddNewState;
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
        public bool AddNew()
        {
            if (mAddNewState == false) 
            {
                    this._ModelItemShadow = (ModelClass)Activator.CreateInstance(typeof(ModelClass));

                    switch (this.mDataBindingMode)
                    {
                        case DataBindingMode.None:
                            break;
                        case DataBindingMode.Passero:
                            WriteControls(this._ModelItemShadow);
                            break;
                        case DataBindingMode.BindingSource:
                            this.AddNewCurrentModelItemIndex = this.CurrentModelItemIndex;
                            this.BindingSource.AddNew();
                            this.MoveLastItem();
                            
                            Passero.Framework.ReflectionHelper.InvokeMethod2(ref _DataNavigator, "UpdateRecordLabel", null);
                            break;
                        default:
                            break;
                    }

                    //WriteControls(this.Model);
                    
                this.Repository.AddNewState = mAddNewState;
                mAddNewState = true;
                return true;    
            }
            return false;   
        }

        public string Description { get; set; }

        public DataBindingMode DataBindingMode
        {
            get
            {
                return mDataBindingMode;
            }

            set 
            {
                this.mDataBindingMode = value; 
                if (this.mDataBindingMode == DataBindingMode.BindingSource )
                {
                    if (this.mBindingSource == null)
                    {
                        this.mBindingSource =new BindingSource();
                        this.mBindingSource.DataSource = this.ModelItem;
                    }
                }
            } 
        }

        public IDbConnection DbConnection 
        { 
            get { return this.Repository.DbConnection; } 
            set { this.Repository.DbConnection = value; }
        }
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);

        public bool AutoWriteControls { get; set; } = false;
        public bool AutoReadControls { get; set; } = false;
        private bool mDataBindControlsAutoSetMaxLenght = true;
        public bool DataBindControlsAutoSetMaxLenght
        {
            get
            {
                return mDataBindControlsAutoSetMaxLenght;
            }
            set
            {
                mDataBindControlsAutoSetMaxLenght = value;
                if (value == true)
                {
                    if (Repository.DbObject.DbColumns.Count == 0)
                    {
                        Repository.DbObject.DbConnection = Repository.DbConnection;
                        Repository.DbObject.GetSchema();
                    }
                }

            }
        }

        private bool mAutoFitColumnsLenght = false;
        public bool AutoFitColumnsLenght
        {
            get
            {
                return mAutoFitColumnsLenght;
            }
            set
            {
                mAutoFitColumnsLenght = value;
                if (value == true)
                {
                    if (Repository.DbObject.DbColumns.Count == 0)
                    {
                        Repository.DbObject.DbConnection = Repository.DbConnection;
                        Repository.DbObject.GetSchema();
                    }
                }

            }
        }
        public Repository<ModelClass> Repository { get; set; }

        public ModelClass GetEmptyModel()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }

        public ViewModel()
        {
           
            Repository = new Repository<ModelClass>(); // (Model)
            
            Repository.ViewModel = this;
            Repository.Name = $"Repository<{typeof(ModelClass).FullName}>";
            _ModelItemShadow = GetEmptyModel();
        }

        public ViewModel(ref Repository<ModelClass> Repository)
        {
           
            _ModelItemShadow = GetEmptyModel();
            this.Repository = Repository;
        }
        public void Init(ModelClass Model, string Name, string Description, DataBindingMode DataBindingMode = DataBindingMode.Passero)
        {
            this.Name = Name;
            this.Description = Description;
            this.mDataBindingMode = DataBindingMode;
            Repository.DbObject.DbConnection = Repository.DbConnection;
            Repository.DbObject.GetSchema();
        }


        public virtual void Init(IDbConnection DbConnection, DataBindingMode DataBindingMode = DataBindingMode.Passero)
        {
            this.mDataBindingMode = DataBindingMode;
            Repository.DbConnection = DbConnection;
            Repository.DbObject.DbConnection = Repository.DbConnection;
            Repository.DbObject.GetSchema();
        }

        public void Init(DataBindingMode DataBindingMode = DataBindingMode.Passero)
        {

            this.mDataBindingMode = DataBindingMode;

        }

        public List<ModelClass> ReloadItems()
        {
            return this.Repository.ReloadItems();
        }

        public ModelClass GetItem(string SqlQuery, object Parameters, IDbTransaction Transaction=null, bool Buffered=true,int? CommandTimeout = null)
        {
            //ModelClass  x= this.Repository.SqlConnection.Query<ModelClass>(sqlquery, parameters, transaction).Single<ModelClass>();
            ModelClass x = this.Repository.GetItem(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);
            switch (UseModelData)
            {
                case UseModelData.External:
                    this._ModelItemShadow = x;
                    break;
                case UseModelData.InternalRepository:
                    this.Repository.ModelItem = x;
                    
                    break;
                default:
                    break;
            }
            this.ModelItem = x; 
            return x;
        }

        public List <ModelClass> GetItems(string SqlQuery, object Parameters=null, System.Data.IDbTransaction Transaction = null, bool Buffered = true,int? CommandTimeout = null)
        {
            List<ModelClass> x = null;
            try
            {
                x = this.Repository.DbConnection.Query<ModelClass>(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout).ToList<ModelClass>();
                if (x != null)
                {
                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            this._ModelItemShadow = x.DefaultIfEmpty(GetEmptyModel()).First();
                            this._ModelItems = x;
                            SetModelItemsShadow();
                            this.SetModelShadow();
                            break;
                        case UseModelData.InternalRepository:
                            this.Repository.ModelItems = x;
                            this.Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModel()).First();
                            this.Repository.SetModelItemsShadow();
                            this.Repository.SetModelShadow();
                            this.Repository.CurrentModelItemIndex = 0;
                            break;
                        default:
                            break;
                    }
                    if (this.mDataBindingMode == DataBindingMode.BindingSource)
                    {
                         this.mBindingSource.DataSource = this.ModelItems;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return x;   
        }

        public List<ModelClass> GetAllItems(System.Data.IDbTransaction Transaction = null, bool Buffered = true,int ? CommandTimeout = null)
        {
            List<ModelClass> x = null;
          
                try
                {
                    x = this.Repository.GetAllItems(Transaction, Buffered ,CommandTimeout);

                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            this._ModelItemShadow = x.DefaultIfEmpty(GetEmptyModel()).First();
                            this._ModelItems = x;

                            break;
                        case UseModelData.InternalRepository:
                            this.Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModel()).First();
                            this.Repository.ModelItems = x;
                            break;
                        default:
                            break;
                    }

                    
                    if (this.mDataBindingMode == DataBindingMode.BindingSource)
                    {
                        this.mBindingSource.DataSource = this.ModelItems;
                    }

                //this.ModelItem = x.FirstOrDefault<ModelClass>(GetEmptyModel());
                }
                catch (Exception)
                {

                    throw;
                }

               
            return x;   
        }

      
        public bool UndoChanges(bool AllItems=false)
        {
            var ER = new ExecutionResult("Passero.Framework.Base.ViewModel.UndoChanges()");
            bool result = false;

            if (this.AddNewState == false)
            {
                this.ModelItem = Passero.Framework.Utilities.Clone<ModelClass>(this.ModelItemShadow);
                if (this.ModelItems.Count >= this.CurrentModelItemIndex)
                    this.ModelItems[this.CurrentModelItemIndex] = Passero.Framework.Utilities.Clone<ModelClass>(this.ModelItemsShadow[this.CurrentModelItemIndex]);
            }
            if (AllItems)
                this.ModelItems = Passero.Framework.Utilities.Clone<List<ModelClass>>(this.ModelItemsShadow);

            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoWriteControls)
                        this.WriteControls();
                    break;
                case DataBindingMode.BindingSource:
                    
                    this.mBindingSource.CancelEdit ();  

                    if (this.AddNewState)
                    {
                        
                        this.mBindingSource.Position = this.AddNewCurrentModelItemIndex;
                        this.MoveAtItem(this.AddNewCurrentModelItemIndex);

                        if (DataNavigator != null)
                        {
                            object x = this.DataNavigator;
                            Passero.Framework.ReflectionHelper.InvokeMethod2(ref x, "UpdateRecordLabel", null);
                        }
                    }
                    //this.mBindingSource.CancelEdit();
                    break;
                default:
                    break;
            }
            if (this.mAddNewState == true)
                this.mAddNewState = false;


            return result;

        }

        public long InsertItem(ModelClass Item = null)
        {

            var ER = new ExecutionResult("Passero.Framework.Base.ViewModel.InsertItem()");
            if (Item is null)
            {
                Item = this.ModelItem;
            }
            long  x = 0;


            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (this.AutoReadControls == true)
                    {
                        this.ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource.EndEdit();
                    break;
                default:
                    break;
            }


            //if (this.AutoReadControls == true)
            //{
            //    this.ReadControls();
            //}

            x = this.Repository.InsertItem (Item);
            if (x>0)
            {
                this.ModelItem = Item;
            }
            this.LastExecutionResult = this.Repository.LastExecutionResult;
            return x;

        }

        public long InsertItems(List<ModelClass> Items = null)
        {
            var ER = new ExecutionResult("Passero.Framework.Base.ViewModel.UpdateItems()");
            if (ModelItem is null)
            {
                ModelItem = this.ModelItem;
            }
            long x = 0;
            x = this.Repository.InsertItems(Items);
            if (x > 0   )
            {
                this.ModelItem = Items.ElementAt(0);
                this.ModelItems = Items;
            }
            this.LastExecutionResult = this.Repository.LastExecutionResult;
            return x;
        }

   
        public bool UpdateItem(ModelClass Item=null)
        {
            var ER = new ExecutionResult("Passero.Framework.Base.ViewModel.UpdateItem()");
            bool x = false;

            if (Item==null)
                Item=this.ModelItem;

            if (this.mAddNewState == true)
            {
                
                //long r= this.Repository.InsertItem(Item);
                long r = this.InsertItem(Item);
                return r> 0;
            }

            //this.LastExecutionResult = this.Repository.LastExecutionResult;
            this.LastExecutionResult = this.LastExecutionResult;
            if (Item is null)
            {
                Item = this.ModelItem;
            }
            


            switch (this.mDataBindingMode)  
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (this.AutoReadControls == true)
                    {
                        this.ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    this.mBindingSource .EndEdit ();
              
                    break;
                default:
                    break;
            }
           
            x = this.Repository.UpdateItem(Item);
            if (x)
            {
                this.ModelItem = Item;
            }
            return x;
            
        }
        public bool UpdateItems()
        {
            return UpdateItems(null);
        }
        public bool UpdateItems(List<ModelClass> Items )
        {

           
            var ER = new ExecutionResult("Passero.Framework.Base.ViewModel.UpdateItems()");
            if (Items is null)
            {
                Items = this.ModelItems;
            }
            bool x = false;
            x = this.Repository.UpdateItems(Items);
            this.LastExecutionResult = this.Repository.LastExecutionResult;
            if (x)
            {
                this.ModelItem = Items.ElementAt(0);
                this.ModelItems = Items;
            }
            return x;
        }

        public bool DeleteItem(ModelClass Item=null)
        {
            var ER = new ExecutionResult("Passero.Framework.Base.ViewModel.DeleteItem()");
            if (Item is null)
            {
                Item = this.ModelItem;
            }

            bool x = false;


            switch (this.mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    x = this.Repository.DeleteItem(Item);
                    if (x == false)
                        return x;
                    this.ModelItem = ModelItems.ElementAt(0);
                        if (this.AutoReadControls == true)
                        {
                            this.ReadControls();
                        }
                    break;
                case DataBindingMode.BindingSource:
                   
                    this.mBindingSource.Remove(Item);
                    this.mBindingSource.EndEdit(); 
                    x = this.Repository.DeleteItem(Item);
                    break;
                default:
                    break;
            }

            
            return x;

        }

        public bool DeleteItems(List<ModelClass> Items)
        {
            bool x = false;
            x = this.Repository.DeleteItems (Items);
            if (x)
            {

                foreach (var item in Items)
                {
                    this.ModelItems.Remove(item);
                }

                this.ModelItem = Items.ElementAt(0);
                
            }
            return x;
        }




        private string GetBoundControlKey(Control Control, string PropertyName)
        {
            string objname = Conversions.ToString(Microsoft.VisualBasic.Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            return (objname + "|" + PropertyName.Trim()).ToLower();
        }


        public bool AddControl(Control Control, string ControlPropertyName, string ModelPropertyName, BindingBehaviour BindingBehaviour = (BindingBehaviour)((int)BindingBehaviour.Insert + (int)BindingBehaviour.Update + (int)BindingBehaviour.Select))
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);

            var _DataBindControl = new DataBindControl();
            _DataBindControl.Control = Control;
            _DataBindControl.ControlPropertyName = ControlPropertyName;
            _DataBindControl.ModelPropertyName = ModelPropertyName;
            _DataBindControl.BindingBehaviour = BindingBehaviour;

            DataBindControls[Key] = _DataBindControl;

            if (DataBindControlsAutoSetMaxLenght == true)
            {
                if (Utilities.ObjectPropertyExist(_DataBindControl.Control, "MaxLength"))
                {
                    if (Repository.DbObject.DbColumns.ContainsKey(ModelPropertyName))
                    {
                        int maxlength = Repository.DbObject.DbColumns[ModelPropertyName].DataColumn.MaxLength;
                        Interaction.CallByName(_DataBindControl.Control, "MaxLength", CallType.Set, maxlength);
                    }
                }
            }

            //if (mDataBindingMode == DataBindingMode.BindingSource)
            //{
            //    Control.DataBindings.Add(ControlPropertyName, mBindingSource , ModelPropertyName);
            //}

            return true;
            // Else
            // Return False
            // End If
        }

        public int RemoveControl(Control Control, string ControlPropertyName)
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);
            return RemoveControl(Key);
        }

        public int RemoveControl(string Key)
        {
            if (DataBindControls.ContainsKey(Key) == true)
            {
                DataBindControls.Remove(Key);

                if (mDataBindingMode == DataBindingMode.BindingSource)
                {
                    DataBindControls[Key].Control.DataBindings.Clear();
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int RemoveControl(Control Control)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            string keytofind = (objname + "|").ToLower();
            return _RemoveControl(keytofind);
        }

        private int _RemoveControl(string keytofind)
        {
            int removedobjects = 0;
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {

                foreach (string key in keys)
                {
                    if (mDataBindingMode == DataBindingMode.BindingSource)
                    {
                        DataBindControls[key].Control.DataBindings.Clear();
                    }
                    DataBindControls.Remove(key);
                    removedobjects += 1;
                }
            }

            return removedobjects;
        }


    


        // Scrive il valore della proprietà del Model nella proprietà del controllo
        public int WriteControl(ModelClass Model, Control Control, string ControlPropertyName = "")
        {
            //if (mDataBindingMode == DataBindingMode.BindingSource | mDataBindingMode == DataBindingMode.None)
            if (mDataBindingMode == DataBindingMode.None)
            {
                return 0;
            }
            int _writedcontrols = 0;

            if (Control is null | Model is null)
            {
                return _writedcontrols;
            }
            string keytofind = GetBoundControlKey(Control, ControlPropertyName);
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    var DataBindControl = DataBindControls[key];
                    var Value = Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Get, (object[])null);
                    if (Value is not null)
                    {
                        Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, Value);
                    }
                    else
                    {
                        Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, "");
                    }

                    _writedcontrols += 1;
                }
            }
            //this.OnWriteControlsdCompleted(new EventArgs());
            return _writedcontrols;

        }


        public int WriteControls(ModelClass Model = null)
        {
            int _writedcontrols = 0;

            if (Model is null)
            {
                Model = this.ModelItem;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        _writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                    }
                    break;
                case DataBindingMode.BindingSource:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        _writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                    }
                    break;
                default:
                    break;
            }
            return _writedcontrols;

        }

        // Scrive il valore della proprietà del Controllo nella proprietà del Model
        public int ReadControl(ModelClass Model, Control Control, string ControlPropertyName = "")
        {

            if (mDataBindingMode == DataBindingMode.BindingSource)
            {
                return 0;
            }

            int _readedcontrols = 0;
            if (Control is null)
            {
                return _readedcontrols;
            }


            string keytofind = GetBoundControlKey(Control, ControlPropertyName);
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    var DataBindControl = DataBindControls[key];
                    if (((int)DataBindControl.BindingBehaviour & (int)BindingBehaviour.Insert + (int)BindingBehaviour.Update) > 0)
                    {
                        if (AddNewState == false)
                        {
                            var Value = Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Get, (object[])null);
                            Value = CheckControlValue(Value, DataBindControl.ModelPropertyName);
                            Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Set, Value);
                            _readedcontrols += 1;
                        }
                        else if ((int)(DataBindControl.BindingBehaviour & BindingBehaviour.Insert) > 0)
                        {
                            var Value = Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Get, (object[])null);
                            Value = CheckControlValue(Value, DataBindControl.ModelPropertyName);
                            Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Set, Value);
                            _readedcontrols += 1;
                        }
                    }

                }
            }
            return _readedcontrols;

        }
        private object CheckControlValue(object Value, string ModelPropertyName)
        {
            if (Value is not null)
            {
                switch (Value.GetType())
                {
                    case var @case when @case == typeof(DateTime):
                        {
                            DateTime d = Conversions.ToDate(Value);
                            if (d < MinDateTime | d > MaxDateTime)
                            {
                                Value = new DateTime?();
                            }

                            break;
                        }
                    case var case1 when case1 == typeof(string):
                        {
                            if (mAutoFitColumnsLenght == true)
                            {
                                if (Repository.DbObject.DbColumns.ContainsKey(ModelPropertyName))
                                {
                                    string s = Conversions.ToString(Value);
                                    int maxlength = Repository.DbObject.DbColumns[ModelPropertyName].DataColumn.MaxLength;
                                    if (s.Length > maxlength)
                                    {
                                        s = s.Substring(0, maxlength);
                                        Value = s;
                                    }
                                }
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }

                }
            }

            return Value;
        }
        //public ModelClass SetModelShadow()
        //{
        //    return Repository.SetModelShadow();
        //}

        public ModelClass SetModelShadow()
        {
            this.ModelItemShadow = Utilities.Clone(this.ModelItem);
            
            return ModelItemShadow;
        }

        public List<ModelClass> SetModelItemsShadow()
        {
            _ModelItemsShadow = Utilities.Clone(_ModelItems);
            
            return _ModelItemsShadow;
        }
        public int ReadControls(ModelClass Model = null)
        {

            if (mDataBindingMode == DataBindingMode.BindingSource | mDataBindingMode == DataBindingMode.None)
            {
                return 0;
            }

            if (Model is null)
            {
                Model = this.ModelItem;
            }

            int _readedcontrols = 0;
            foreach (DataBindControl Control in DataBindControls.Values)
                _readedcontrols = _readedcontrols + ReadControl(Model, Control.Control);
            return _readedcontrols;

        }

        public int ClearControl(Control Control, string ControlPropertyName = "")
        {

            int writedproperties = 0;

            if (Control is null)
            {
                return writedproperties;
            }

            string keytofind = GetBoundControlKey(Control, ControlPropertyName);
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    var DataBindControl = DataBindControls[key];
                    object Value = null;
                    Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, Value);
                    writedproperties += 1;
                }
            }
            return writedproperties;

        }
        
        public int ClearControls()
        {
            int writedproperties = 0;
            foreach (DataBindControl Control in DataBindControls.Values)
                writedproperties = writedproperties + ClearControl(Control.Control);
            return writedproperties;
        }




        
    }




  
}