using Dapper;
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
        private object mDataNavigator = null;
        public object DataNavigator
        {
            get { return mDataNavigator; }
            set { mDataNavigator = value; }
        }
        public Wisej.Web.Form OwnerView { get; set; }   
        public string Name { get; set; } = $"ViewModel<{typeof(ModelClass).FullName}>";
        public string FriendlyName { get; set; } = $"ViewModel<{typeof(ModelClass).FullName}>";
        public DateTime MinDateTime { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0);
        public DateTime MaxDateTime { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59, 999);
        public UseModelData UseModelData { get; set; } = UseModelData.InternalRepository;
        public BindingBehaviour bindingBehaviour { get; set; }=BindingBehaviour.SelectInsertUpdate;
        private ErrorNotificationMessageBox mErrorNotificationMessageBox = new ErrorNotificationMessageBox();   
        private ErrorNotificationModes mErrorNotificationMode = ErrorNotificationModes.ThrowException;
        private ModelClass mModelItemShadow;
        private ModelClass ExternalModelShadow;
        private int mAddNewCurrentModelItemIndex = -1;
        private int mCurrentModelItemIndex=-1;
        private List<ModelClass> mModelItems;
        private List<ModelClass> mModelItemsShadow;
        private BindingSource mBindingSource;
        private Dictionary<string, Wisej.Web.Control> BindingSourceControls = new Dictionary<string, Control>();
        private DataBindingMode mDataBindingMode = DataBindingMode.Passero;
        
        
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult(mClassName);
        public ErrorNotificationModes  ErrorNotificationMode
        {
            get { return mErrorNotificationMode; }
            set { mErrorNotificationMode = value; 
                this.Repository.ErrorNotificationMode = value; }
        }
        public ErrorNotificationMessageBox ErrorNotificationMessageBox        {
            get { return mErrorNotificationMessageBox; }
            set { this.mErrorNotificationMessageBox = value; 
                this.Repository.ErrorNotificationMessageBox = value; }
        }

        public void HandleExeception(ExecutionResult executionResult)
        {
            if (executionResult == null)
            {
                return;
            }

            switch (this.ErrorNotificationMode)
            {
                case ErrorNotificationModes.ShowDialog:
                    StringBuilder msg = new StringBuilder();
                    msg.Append($"{executionResult.Context}");
                    msg.Append($"{executionResult.ResultMessage}");
                    msg.Append($"{Name}");
                    ReflectionHelper.CallByName(ErrorNotificationMessageBox, "Show", Microsoft.VisualBasic.CallType.Method, msg);
                    break;
                case ErrorNotificationModes.Silent:
                    break;
                case ErrorNotificationModes.ThrowException:
                    throw executionResult.Exception;
            }

        }
        private string mSQLQuery;

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

        public string ResolvedSQLQuery(string SQLQuery="", DynamicParameters Parameters=null)
        {
            if (SQLQuery!=null && Parameters !=null)
                return Passero.Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, Parameters);
            else
                return Passero.Framework.DapperHelper.Utilities.ResolveSQL(this.SQLQuery, this.Parameters);
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

        public void SetBindingSource(BindingSource bindingSource, bool setdatabindingmode=true )
        {
            this.BindingSource = bindingSource;
            if (setdatabindingmode == true)
                this.DataBindingMode = DataBindingMode.BindingSource;
        }

        public Type? ModelType
        {
            get 
            {

                return GetEmptyModelItem().GetType ();
                //return ModelItem.GetType(); 
            
            }
            
        }
        
        public void ResetModelItem(bool ResetModelItems=true)
        {
            ModelItem = NewModeltem; 
            if (ResetModelItems==true)
                ModelItems = new List<ModelClass>();
        }
        public void ResetModelItems()
        {
            ModelItems =  new List<ModelClass>();
        }


        public ModelClass? NewModeltem
        {
            get { return GetEmptyModelItem(); }
            set { NewModeltem = value; }
        }
        public ModelClass ModelItem
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return mModelItemShadow;
                    default:
                        return Repository.ModelItem;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mModelItemShadow = value;
                        Repository.ModelItem = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.ModelItem = value;
                        break;
                }
            }


        }

        public List<ModelClass> ModelItems
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return mModelItems;
                    default:
                        return Repository.ModelItems;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mModelItems = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.ModelItems = value;
                        break;
                }
                
                if (mBindingSource != null)
                {
                    mBindingSource.DataSource = value;
                }
                
            }
        }

        public List<ModelClass> ModelItemsShadow
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return mModelItemsShadow;
                    default:
                        return Repository.ModelItemsShadow;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mModelItemsShadow = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.ModelItemsShadow = value;
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
                        return mModelItems.Count;

                    default:
                        return Repository.ModelItems.Count;
                }
            }
        }
        public int CurrentModelItemIndex
        {
            get
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        return mCurrentModelItemIndex;
                    default:
                        return Repository.CurrentModelItemIndex;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mCurrentModelItemIndex = value;
                        break;
                    case UseModelData.InternalRepository:
                        Repository.CurrentModelItemIndex = value;
                        mCurrentModelItemIndex = value;
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
                        return mAddNewCurrentModelItemIndex;
                    default:
                        return Repository.AddNewCurrentModelItemIndex;
                }
            }
            set
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mAddNewCurrentModelItemIndex = value;
                        break;

                    case UseModelData.InternalRepository:
                        Repository.AddNewCurrentModelItemIndex = value;
                        mAddNewCurrentModelItemIndex = value;
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

        private ExecutionResult<ModelClass> ExternalGetModelItemsAt(int index)
        {

            var ERContext = $"{mClassName}.ExternalGetModelItemsAt()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContext);
            if (mModelItems == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "Invalid Index!";
                ER.ErrorCode = 0;
            }
            if (index > 0 && index < mModelItems.Count())
            {
                ER.Value = mModelItems.ElementAt(index);
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }


        public ExecutionResult<ModelClass> GetModelItemsAt(int index)
        {
            var ERContext = $"{mClassName}.GetModelItemsAt()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContext);

            if (UseModelData == UseModelData.External)
            {
                ER = ExternalGetModelItemsAt(index);
            }
            else
            {
                ER = Repository.GetModelItemsAt(index);
            }

            ER.Context = ERContext;
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }

        public ModelClass ModelItemShadow
        {
            get
            {
                if (UseModelData == UseModelData.External)
                {
                    return this.ExternalModelShadow;
                }
                else
                {
                    return Repository.ModelItemShadow;
                }
            }
            set
            {
                if (UseModelData == UseModelData.External)
                {
                    this.ExternalModelShadow = value;
                }
                else
                {
                    Repository.ModelItemShadow = value;
                }
            }
        }

        private bool ExternalModelDataChanged(ModelClass ModelShadow = null)
        {
            if (ModelShadow is null)
            {
                ModelShadow = ExternalModelShadow;
            }
            return !Utilities.ObjectsEquals(mModelItemShadow, ModelShadow);
        }

        public bool IsModelDataChanged(ModelClass ModelShadow = null)
        {
            if (UseModelData == UseModelData.InternalRepository)
                return Repository.IsModelDataChanged(ModelShadow);
            else
                return ExternalModelDataChanged(ModelShadow);
        }

        public void DataNavigatorRaiseEventBoundCompled()
        {
            if (mDataNavigator != null)
            {
                ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "RaiseEventBoundCompleted");
            }
        }
        public void SetModelItemShadow()
        {
            ModelItemShadow = Utilities.Clone(ModelItem);
        }

        public ExecutionResult MoveFirstItem()
        {
            var ERContext = $"{mClassName}.MoveFirstItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            if (UseModelData == UseModelData.InternalRepository)
            {
                ER = Repository.MoveFirstItem();
            }
            else
            {
                if (mModelItemShadow != null && mModelItems.Count > 0)
                {
                    mCurrentModelItemIndex = 0;
                    mModelItemShadow = mModelItems.ElementAt(0);
                }
                else
                {
                    mModelItemShadow = null;
                    mCurrentModelItemIndex = -1;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                }
            }

            if (ER.Success)
            {
                SetModelItemShadow();
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero :
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.MoveFirst();
                        DataNavigatorRaiseEventBoundCompled();
                        break;
                    default:
                        break;
                }
            }

            return ER;

        }


        public ExecutionResult MoveLastItem()
        {
            var ERContext = $"{mClassName}.MoveLastItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            if (UseModelData == UseModelData.InternalRepository)
            {
                ER = Repository.MoveLastItem();
                if (ER.Success)
                {
                    mCurrentModelItemIndex = Repository.CurrentModelItemIndex;
                    mAddNewCurrentModelItemIndex = Repository.AddNewCurrentModelItemIndex;
                }
            }
            else
            {
                if (mModelItemShadow != null && ModelItemsCount > 0)
                {
                    mCurrentModelItemIndex = mModelItems.Count() - 1;
                }
                else
                {
                    mCurrentModelItemIndex = -1;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                }
            }

            if (ER.Success)
            {
                SetModelItemShadow();
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero :
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.MoveLast();
                        break;
                    default:
                        break;
                }
            }

            return ER;

        }


        public ExecutionResult MovePreviousItem()
        {
            var ERContext = $"{mClassName}.MovePreviousItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (UseModelData == UseModelData.InternalRepository)
            {
                ER = Repository.MovePreviousItem();
            }
            else
            {
                if (mModelItems != null && mModelItems.Count > 0)
                {
                    if (mCurrentModelItemIndex > 0)
                    {
                        mCurrentModelItemIndex -= 1;
                        mModelItemShadow = mModelItems.ElementAt(mCurrentModelItemIndex);
                    }
                }
                else
                {
                    mModelItemShadow = null;
                    mCurrentModelItemIndex = -1;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                }
            }


            if (ER.Success)
            {
                SetModelItemShadow();
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero :
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.MovePrevious();
                        break;
                    default:
                        break;
                }
            }

            return ER;

        }



        public ExecutionResult MoveNextItem()
        {
            var ERContext = $"{mClassName}.MoveNextItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            if (UseModelData == UseModelData.InternalRepository)
            {
                ER = Repository.MoveNextItem();
            }
            else
            {
                if (mModelItems != null && mModelItems.Count > 0)
                {
                    if (mCurrentModelItemIndex < mModelItems.Count() - 1)
                    {
                        mCurrentModelItemIndex += 1;
                        mModelItemShadow = mModelItems.ElementAt(mCurrentModelItemIndex);
                    }
                }
                else
                {
                    mModelItemShadow = null;
                    mCurrentModelItemIndex = -1;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                }
            }

            if (ER.Success)
            {
                SetModelItemShadow();
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero :
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.MoveNext();
                        break;
                    default:
                        break;
                }
            }

            return ER;
        }

        public ExecutionResult MoveAtItem(int Index)
        {
            var ERContext = $"{mClassName}.MoveAtItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (UseModelData == UseModelData.InternalRepository)
            {
                ER = Repository.MoveAtItem(Index);
            }
            else
            {
                if (mModelItems != null && mModelItems.Count > 0)
                {
                    if (Index > 0 && Index < mModelItems.Count())
                    {
                        mCurrentModelItemIndex = Index;
                        mModelItemShadow = mModelItems.ElementAt(Index);
                    }
                }
                else
                {
                    mModelItemShadow = null;
                    mCurrentModelItemIndex = -1;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                    return ER;
                }
            }

            if (ER.Success)
            {
                SetModelItemShadow();
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero :
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.Position = CurrentModelItemIndex;
                        break;
                    default:
                        break;
                }
            }

            return ER;
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
        public ExecutionResult AddNew(object newItem = null)
        {
            var ER = new ExecutionResult($"{mClassName}.AddNew()");
            if (mAddNewState == false)
            {
                try
                {
                    ER.Context = "Checking newItem";
                    if (newItem == null)
                    {
                        newItem = (ModelClass)Activator.CreateInstance(typeof(ModelClass));
                    }
                    ModelItem = (ModelClass)newItem;
                    ModelItemShadow = (ModelClass)newItem;

                    ER.Context = "DataBinding";
                    switch (mDataBindingMode)
                    {
                        case DataBindingMode.None:
                            break;
                        case DataBindingMode.Passero:
                            WriteControls(ModelItemShadow);
                            break;
                        case DataBindingMode.BindingSource:
                            AddNewCurrentModelItemIndex = mBindingSource.CurrencyManager.Position;
                            mBindingSource.AddNew();
                            ModelItems[mBindingSource.CurrencyManager.Position] = ModelItem;
                            mBindingSource.CurrencyManager.UpdateBindings();
                            CurrentModelItemIndex = mBindingSource.CurrencyManager.Position;
                            ReflectionHelper.CallByName( mDataNavigator, "UpdateRecordLabel", CallType.Method);
                            break;
                        default:
                            break;
                    }
                    Repository.AddNewState = mAddNewState;
                    mAddNewState = true;

                }
                catch (Exception ex)
                {
                    ER.Exception = ex;
                    ER.ResultMessage = ex.Message;
                    ER.ErrorCode = 1;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                }
            }
            LastExecutionResult = ER;
            return ER;
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

        public IDbTransaction DbTransaction
        {
            get
            {
                return Repository.DbTransaction;
            }
            set
            {
                Repository.DbTransaction = value;
            }
        }
        public int DbCommandTimeout
        {
            get
            {
                return Repository.DbCommandTimeout;
            }
            set
            {
                Repository.DbCommandTimeout = value;
            }
        }

        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


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

        //public ModelClass GetEmptyModel()
        //{
        //    return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        //}

        private DynamicParameters mDefaultSQLQueryParameters;
        public DynamicParameters DefaultSQLQueryParameters
        {
            get
            {
                mDefaultSQLQueryParameters = Repository.DefaultSQLQueryParameters;
                return mDefaultSQLQueryParameters;
            }
            set
            {
                mDefaultSQLQueryParameters = value;
                Repository.DefaultSQLQueryParameters = value;
            }
        }
        private string mDefaultSQLQuery;
        public  string DefaultSQLQuery
        {
            get
            {
                mDefaultSQLQuery = Repository.DefaultSQLQuery;
                return mDefaultSQLQuery;
            }
            set
            {
                mDefaultSQLQuery = value;
                Repository.DefaultSQLQuery = value;
            }
        }

        public DynamicParameters Parameters
        {
            get
            {
                return Repository.Parameters;
                
            }
        }

        public ViewModel(string Name="", string FriendlyName="")
        {
            this.Repository = new Repository<ModelClass>();
            this.DefaultSQLQuery = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
            this.DefaultSQLQueryParameters = new DynamicParameters();
            if (Name != "")
                this.Name = Name;
            else
                this.Name=nameof (ModelClass);

            if (FriendlyName != "")
                this.FriendlyName = FriendlyName;
            else
                this.FriendlyName = Name;

            this.Repository.ViewModel = this;
            this.Repository.Name = $"Repository<{typeof(ModelClass).FullName}>";
            this.Repository.ErrorNotificationMessageBox = this.ErrorNotificationMessageBox;
            this.Repository.ErrorNotificationMode = this.ErrorNotificationMode;
            this.mModelItemShadow = GetEmptyModelItem();
           

        }

        public ViewModel(ref Repository<ModelClass> Repository, string Name = "", string FriendlyName = "")
        {
            if (Name != "")
                this.Name = Name;
            if (FriendlyName != "")
                this.FriendlyName = FriendlyName;

            mModelItemShadow = GetEmptyModelItem();
            this.Repository = Repository;
        }
        public void Init(ModelClass Model, string Name, string Description, DataBindingMode DataBindingMode = DataBindingMode.Passero)
        {
            this.Name = Name;
            this.Description = Description;
            this.mDataBindingMode = DataBindingMode;
            //Repository.DbObject.DbConnection = Repository.DbConnection;
            //Repository.DbObject.GetSchema();
        }


        public virtual void Init(IDbConnection DbConnection, DataBindingMode DataBindingMode = DataBindingMode.Passero)
        {
            this.mDataBindingMode = DataBindingMode;
            this.DbConnection = DbConnection;   
            this.Repository.DbConnection = DbConnection;
            //Repository.DbObject.DbConnection = Repository.DbConnection;
            //Repository.DbObject.GetSchema();
        }

        public void Init(DataBindingMode DataBindingMode = DataBindingMode.Passero)
        {

            this.mDataBindingMode = DataBindingMode;

        }

        public ExecutionResult ReloadItems()
        {
            var ERContenxt = $"{mClassName}.ReloadItems()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);
            ER = Repository.ReloadItems();
            ER.Context = ERContenxt;
            return ER;
        }


        public ExecutionResult<ModelClass> GetItem(string SqlQuery, object Parameters, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.GetItems()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContenxt);
            ER.Value = null;
            ER = Repository.GetItem(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);
            switch (UseModelData)
            {
                case UseModelData.External:
                    mModelItemShadow = ER.Value;
                    break;
                case UseModelData.InternalRepository:

                    Repository.ModelItem = ER.Value;
                    break;

                default:
                    break;
            }
            ModelItem = ER.Value;
            //if (ModelItem == null)
            //{
            //    ER.ResultCode = ExecutionResultCodes.Failed;
            //    ER.ResultMessage = $"No data for query\n{Framework .DapperHelper .Utilities .ResolveSQL (SqlQuery,(DynamicParameters)Parameters)}";
            //}
            DataNavigatorRaiseEventBoundCompled();
            return ER;
        }


        public ExecutionResult<List<ModelClass>> GetItems(string SqlQuery, object Parameters = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            string ERContenxt = $"{mClassName}.GetItems()";
            ExecutionResult<List<ModelClass>> ER = new ExecutionResult<List<ModelClass>>(ERContenxt);


            List<ModelClass> x = null;
            try
            {
                this.mCurrentModelItemIndex = -1;
                this.Repository.ErrorNotificationMessageBox = this.ErrorNotificationMessageBox;
                this.Repository.ErrorNotificationMode = this.ErrorNotificationMode;

                this.Repository.SQLQuery = SqlQuery;

                //this.Repository.Parameters = (DynamicParameters)Parameters;
                this.Repository.Parameters = DapperHelper.Utilities .GetDynamicParameters (Parameters);
                ER = Repository.GetItems(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);

                if (ER.Success)
                {
                    x = ER.Value;
                    this.mCurrentModelItemIndex = 0;
                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            mModelItemShadow = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            mModelItems = x;
                            //If (AutoUpdateModelItemsShadows) Then
                            SetModelItemsShadow();
                            //End If
                            SetModelItemShadow();
                            break;
                        case UseModelData.InternalRepository:
                            Repository.ModelItems = x;
                            Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            //If (AutoUpdateModelItemsShadows) Then
                            Repository.SetModelItemsShadow();
                            //End If
                            Repository.SetModelItemShadow();
                            Repository.CurrentModelItemIndex = 0;
                            break;
                        default:
                            break;
                    }

                    if (mDataBindingMode == DataBindingMode.BindingSource)
                    {
                        mBindingSource.DataSource = ModelItems;
                    }
                }
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query\n{Framework.DapperHelper.Utilities.ResolveSQL(SqlQuery, (DynamicParameters)Parameters)}";
                HandleExeception(ER.ToExecutionResult());
            }
            this.MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            //DataNavigatorRaiseEventBoundCompled();
            return ER;
        }
        public void SetBindingSource()
        {
            if (mDataBindingMode == DataBindingMode.BindingSource)
            {
                switch (UseModelData)
                {
                    case UseModelData.External:
                        mBindingSource.DataSource = ModelItems;
                        break;
                    case UseModelData.InternalRepository:
                        mBindingSource.DataSource = Repository.ModelItems;
                        break;
                    default:
                        break;
                }
            }
        }
        public ExecutionResult<List<ModelClass>> GetAllItems(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.GetAllItems()";
            ExecutionResult<List<ModelClass>> ER = new ExecutionResult<List<ModelClass>>(ERContenxt);
            List<ModelClass> x = null;
            try
            {
                ER = Repository.GetAllItems(Transaction, Buffered, CommandTimeout);
                if (ER.Success)
                {
                    x = ER.Value;
                    switch (UseModelData)
                    {
                        case UseModelData.External:
                            mModelItemShadow = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            mModelItems = x;
                            if (mDataBindingMode == DataBindingMode.BindingSource)
                            {
                                mBindingSource.DataSource = ModelItems;

                            }
                            break;
                        case UseModelData.InternalRepository:
                            Repository.ModelItem = x.DefaultIfEmpty(GetEmptyModelItem()).First();
                            Repository.ModelItems = x;
                            if (mDataBindingMode == DataBindingMode.BindingSource)
                            {
                                mBindingSource.DataSource = Repository.ModelItems;

                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.DebugInfo = $"Query = {SQLQuery}";
                HandleExeception(ER.ToExecutionResult());
            }

            //if (this.DataNavigator != null)
            //{
            //    ReflectionHelper.CallByName(this.DataNavigator, "InitDataNavigator", Microsoft.VisualBasic.CallType.Method, null);
            //}

            this.MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            //DataNavigatorRaiseEventBoundCompled()
            return ER;
        }



        public ExecutionResult UndoChanges(bool AllItems = false)
        {
            var ERContenxt = $"{mClassName}.UndoChanges()";
            var ER = new ExecutionResult(ERContenxt);
            var result = false;
            try
            {
                if (this.AddNewState == false)
                {
                    this.ModelItem = Utilities.Clone<ModelClass>(this.ModelItemShadow);
                    if (this.ModelItems.Count >= this.CurrentModelItemIndex)
                    {
                        this.ModelItems[this.CurrentModelItemIndex] =Utilities.Clone<ModelClass>(this.ModelItemShadow);
                    }
                }
                else
                {
                    if (this.ModelItemsCount > 0)
                    {
                        this.ModelItems.RemoveAt(this.ModelItemsCount - 1);
                    }
                }


                //If AllItems And AutoUpdateModelItemsShadows = True Then
                this.ModelItems = Utilities.Clone<List<ModelClass>>(this.ModelItemsShadow);
                //End If

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero :
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        if (AddNewState)
                        {
                            mBindingSource.Position = AddNewCurrentModelItemIndex;
                            MoveAtItem(AddNewCurrentModelItemIndex);
                            if (mDataNavigator != null)
                            {
                                ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "UpdateRecordLabel");
                            }
                        }
                        else
                        {
                            mBindingSource.CancelEdit();
                        }
                        break;

                    default:
                        break;
                }
                if (mAddNewState == true)
                {
                    mAddNewState = false;
                }

            }
            catch (Exception ex)
            {

                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;

            }

            return ER;

        }


        public ExecutionResult InsertItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.InsertItem()";
            var ER = new ExecutionResult(ERContext);
            if (Item == null)
            {
                Item = ModelItem;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            long x = 0;


            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero :
                    if (AutoReadControls == true)
                    {
                        ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    //BindingSource.CurrencyManager.EndCurrentEdit()
                    BindingSource.EndEdit();
                    Item = (ModelClass)BindingSource.Current;
                    ModelItem = Item;
                    break;
                //Item = CType(mBindingSource.Current, ModelClass)

                default:
                    break;
            }

            ER = Repository.InsertItem(Item, DbTransaction, DbCommandTimeout);
            ER.Context = ERContext;


            if (ER.Success)
            {
                this.mAddNewState = false;

            }

            LastExecutionResult = ER;



            return ER;

        }

        public ExecutionResult InsertItems(List<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.InsertItems()";
            var ER = new ExecutionResult(ERContenxt );
            if (ModelItem == null)
            {
                ModelItem = ModelItem;
            }
            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            long x = 0;
            ER = Repository.InsertItems(Items, DbTransaction, DbCommandTimeout);
            ER.Context = ERContenxt;
            x = Convert.ToInt64(ER.Value);
            if (x > 0)
            {
                ModelItem = Items.ElementAt(0);
                ModelItems = Items;
                CurrentModelItemIndex = 0;
            }
            LastExecutionResult = ER;
            this.AddNewState = false;

            return ER;
        }





        public ExecutionResult UpdateItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERcontext = $"{mClassName}.UpdateItem()";
            var ER = new ExecutionResult(ERcontext);
            var x = false;
            if (mAddNewState == true)
            {
                //long r= this.Repository.InsertItem(Item);
                //Dim r = InsertItem(Item)
                ER = InsertItem(Item, DbTransaction, DbCommandTimeout);
                ER.Context = ERcontext;
                LastExecutionResult = ER;
                return ER;
            }


            if (Item == null)
            {
                Item = ModelItem;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoReadControls == true)
                    {
                        ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    mBindingSource.EndEdit();
                    ModelItem = Item;
                    break;
                default:
                    break;
            }

            ER = Repository.UpdateItem(Item, DbTransaction, DbCommandTimeout);

            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Item;
            }

            ER.Context = ERcontext;
            LastExecutionResult = ER;
            return ER;

        }

       

        public ExecutionResult UpdateItemEx(ModelClass Item = null, ModelClass ItemShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERcontext = $"{mClassName}.UpdateItemEx()";
            var ER = new ExecutionResult();
            var x = false;


            if (mAddNewState == true)
            {
                //long r= this.Repository.InsertItem(Item);
                //Dim r = InsertItem(Item)
                ER = InsertItem(Item);
                ER.Context = ERcontext;
                LastExecutionResult = ER;
                return ER;
            }


            if (Item == null)
            {
                Item = ModelItem;
            }
            if (ItemShadow == null)
            {
                ItemShadow = ModelItemShadow;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero :
                    if (AutoReadControls == true)
                    {
                        ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    mBindingSource.EndEdit();
                    ModelItem = Item;
                    break;
                default:
                    break;
            }

            ER = Repository.UpdateItemEx(Item, ItemShadow, DbTransaction, DbCommandTimeout);

            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Item;
            }

            ER.Context = ERcontext;
            LastExecutionResult = ER;
            return ER;

        }
        public ExecutionResult UpdateItems(List<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");
            string Context = ER.Context;

            if (Items == null)
            {
                Items = ModelItems;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }


            ER = Repository.UpdateItems(Items, DbTransaction, DbCommandTimeout);
            LastExecutionResult = Repository.LastExecutionResult;
            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Items.ElementAt(0);
                ModelItems = Items;
            }

            return ER;

        }


        public ExecutionResult UpdateItemsEx(List<ModelClass> Items = null, List<ModelClass> ItemsShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.UpdateItemsEx()");
            string Context = ER.Context;

            if (Items == null)
            {
                Items = ModelItems;
            }
            if (ItemsShadow == null)
            {
                ItemsShadow = ModelItemsShadow;
            }


            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }


            ER = Repository.UpdateItemsEx(Items, ItemsShadow, DbTransaction, DbCommandTimeout);
            LastExecutionResult = Repository.LastExecutionResult;
            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Items.ElementAt(0);
                ModelItems = Items;
            }

            return ER;

        }


        public ExecutionResult DeleteItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItem()");
            string Context = ER.Context;
            if (Item == null)
            {
                Item = ModelItem;
            }
            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    ER = Repository.DeleteItem(Item, DbTransaction, DbCommandTimeout);
                    break;
                case DataBindingMode.Passero :
                    ER = Repository.DeleteItem(Item, DbTransaction, DbCommandTimeout);

                    if (Convert.ToBoolean(ER.Value))
                    {
                        ModelItem = ModelItems.ElementAt(0);
                        if (AutoReadControls == true)
                        {
                            ReadControls();
                        }
                    }
                    break;
                case DataBindingMode.BindingSource:

                    mBindingSource.Remove(Item);
                    mBindingSource.EndEdit();
                    ER = Repository.DeleteItem(Item, DbTransaction, DbCommandTimeout);
                    this.CurrentModelItemIndex = mBindingSource.CurrencyManager.Position;
                    break;
                default:
                    break;
            }

            ER.Context = Context;
            LastExecutionResult = ER;
            return ER;

        }


        public ExecutionResult DeleteItems(List<ModelClass> Items, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            string Context = ER.Context;
            var x = false;

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }
            ER = Repository.DeleteItems(Items, DbTransaction, DbCommandTimeout);
            if (Convert.ToBoolean(ER.Value))
            {

                foreach (var item in Items)
                {
                    ModelItems.Remove(item);
                }

                ModelItem = Items.ElementAt(0);

            }
            ER.Context = Context;
            LastExecutionResult = ER;
            return ER;

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
            mModelItemsShadow = Utilities.Clone(mModelItems);
            
            return mModelItemsShadow;
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