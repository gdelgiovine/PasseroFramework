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



//namespace Passero.Framework.Base
namespace Passero.Framework
{

    /// <summary>
    /// <typeparam name="ModelClass">The type of the odel class.</typeparam>
    /// Base class for models that supports property change notifications and cloning
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanging" />

    public class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass :  class 
    {
        public ViewModelTypes ViewModelType = ViewModelTypes.Base;
        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.Base.ViewModel<T>";

        private Dictionary<string, dynamic> _ViewModels = new(StringComparer.InvariantCultureIgnoreCase);


        #region INotifyPropertyChanged and INotifyPropertyChanging  
        /// <summary>
        /// Generato quando il valore di una proprietà cambia.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Generato quando il valore di una proprietà sta per cambiare.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">The propertyName.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies that a property is about to change.
        /// </summary>
        /// <param name="propertyName">The propertyName.</param>
        protected virtual void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of the current instance</returns>
        public ViewModel<ModelClass> Clone()
        {
            return Utilities.Clone(this);
        }
       
        /// <summary>
        /// Sets the property value and raises the appropriate change notifications.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="field">Reference to the backing field</param>
        /// <param name="value">The new value</param>
        /// <param name="propertyName">Name of the property (automatically set by the compiler)</param>
        /// <returns>True if the value was changed, false if the value was the same</returns>
        protected bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;

            NotifyPropertyChanging(propertyName);
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion


        #region Gestion ViewModels aggiuntivi
        public void AddViewModel<ModelClass>(string Key, ViewModel<ModelClass> viewModel) where ModelClass : class
        {
            string methodName = "AddViewModel<ModelClass>";
            try
            {
                if (viewModel == null)
                {
                    throw new ArgumentNullException(nameof(viewModel), "The view model instance cannot be null.");
                }
               
                if(Key.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException(Key, "The ViewModel Key be empty or null.");
                }   
                if (_ViewModels.ContainsKey(Key))
                {
                    throw new ArgumentException($"A view model with the key '{Key}' already exists in the collection.");
                }
                _ViewModels[Key] = viewModel;
                
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }
        public dynamic GetViewModel(string Key)
        {
            string methodName = "GetViewModel";
            try
            {
                if (Key.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException(Key, "The ViewModel Key be empty or null.");
                }
                if (_ViewModels.ContainsKey(Key))
                {
                    return _ViewModels[Key];
                }
                else
                {
                    throw new ArgumentException($"A ViewModel with the Key '{Key}' did not exists in the ViewModels collection.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }
        public void UpdateViewModel<ModelType>(string Key, ViewModel<ModelType> viewModel) where ModelType : class
        {
            string methodName = "UpdateViewModel<ModelClass>";
            try
            {
                if (viewModel == null)
                {
                    throw new ArgumentNullException(nameof(viewModel), "The ViewModel instance cannot be null.");
                }
                
                if (_ViewModels.ContainsKey(Key))
                {
                    _ViewModels[Key] = viewModel;
                }
                else
                {
                    throw new ArgumentException($"A ViewModel with the Key '{Key}' did not exists in the ViewModels collection.", nameof(viewModel));

                }

            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }

        public void RemoveViewModel<ModelType>(string Key) where ModelType : class
        {
            string methodName = "RemoveViewModel<ModelType>";
            try
            {
                if (Key == null)
                {
                    throw new ArgumentNullException("The ViewModel Key cannot be null.");
                }
               
                if (_ViewModels.ContainsKey(Key))
                {
                    _ViewModels.Remove(Key);
                }
                else
                {
                    throw new ArgumentException($"A ViewModel with the Key '{Key}' did not exists in the ViewModels collection.");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }

     

        #endregion


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

        /// <summary>
        /// Sets the binding source.
        /// </summary>
        /// <param name="bindingSource">The binding source.</param>
        /// <param name="setdatabindingmode">if set to <c>true</c> [setdatabindingmode].</param>
        public void SetBindingSource(BindingSource bindingSource, bool setdatabindingmode = true)
        {
            BindingSource = bindingSource;
            if (setdatabindingmode == true)
                DataBindingMode = DataBindingMode.BindingSource;
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type? ModelType
        {
            get
            {

                return GetEmptyModelItem().GetType();
                //return ModelItem.GetType(); 

            }

        }

        /// <summary>
        /// Resets the model item.
        /// </summary>
        /// <param name="ResetModelItems">if set to <c>true</c> [reset model items].</param>
        public void ResetModelItem(bool ResetModelItems = true)
        {
            ModelItem = NewModeltem;
            if (ResetModelItems == true)
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
        /// Creates new modeltem.
        /// </summary>
        /// <value>
        /// The new modeltem.
        /// </value>
        public ModelClass? NewModeltem
        {
            get { return GetEmptyModelItem(); }
            set { NewModeltem = value; }
        }
        /// <summary>
        /// Gets or sets the model item.
        /// </summary>
        /// <value>
        /// The model item.
        /// </value>
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

        /// <summary>
        /// Gets or sets the model items.
        /// </summary>
        /// <value>
        /// The model items.
        /// </value>
        public IList<ModelClass> ModelItems
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



        /// <summary>
        /// Gets the model items count.
        /// </summary>
        /// <value>
        /// The model items count.
        /// </value>
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


        /// <summary>
        /// Creates the passero binding from binding source.
        /// </summary>
        /// <param name="Form">The form.</param>
        /// <param name="BindingSource">The binding source.</param>
        /// <returns></returns>
        public int CreatePasseroBindingFromBindingSource(Form Form = null, BindingSource BindingSource = null)
        {
            //if (this.DataBindingMode == DataBindingMode.BindingSource)
            //    return 0;

            if (Form == null)
                Form = OwnerView;

            if (Form == null)
                return 0;

            if (BindingSource == null)
                BindingSource = this.BindingSource;

            if (BindingSource == null)
                return 0;

            DataBindControls.Clear();

            foreach (Control item in Form.Controls)
            {
                if (item.HasDataBindings)
                {
                    foreach (Binding binding in item.DataBindings)
                    {
                        if (binding.DataSource == BindingSource)
                        {
                            AddControl(item, binding.PropertyName, binding.BindingMemberInfo.BindingField, bindingBehaviour);
                        }
                    }
                }
            }

            return DataBindControls.Count();
        }

        /// <summary>
        /// Externals the get model items at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Gets the model items at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets or sets the model item shadow.
        /// </summary>
        /// <value>
        /// The model item shadow.
        /// </value>
        public ModelClass ModelItemShadow
        {
            get
            {
                if (UseModelData == UseModelData.External)
                {
                    return ExternalModelShadow;
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
                    ExternalModelShadow = value;
                }
                else
                {
                    Repository.ModelItemShadow = value;
                }
            }
        }

        /// <summary>
        /// Externals the model data changed.
        /// </summary>
        /// <param name="ModelShadow">The model shadow.</param>
        /// <returns></returns>
        private bool ExternalModelDataChanged(ModelClass ModelShadow = null)
        {
            if (ModelShadow is null)
            {
                ModelShadow = ExternalModelShadow;
            }
            return !Utilities.ObjectsEquals(mModelItemShadow, ModelShadow);
        }

        /// <summary>
        /// Determines whether [is model data changed] [the specified model shadow].
        /// </summary>
        /// <param name="ModelShadow">The model shadow.</param>
        /// <returns>
        ///   <c>true</c> if [is model data changed] [the specified model shadow]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsModelDataChanged(ModelClass ModelShadow = null)
        {
            if (UseModelData == UseModelData.InternalRepository)
                return Repository.IsModelDataChanged(ModelShadow);
            else
                return ExternalModelDataChanged(ModelShadow);
        }

        /// <summary>
        /// Datas the navigator raise event bound compled.
        /// </summary>
        public void DataNavigatorRaiseEventBoundCompled()
        {
            if (mDataNavigator != null)
            {
                //ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "RaiseEventBoundCompleted");
                mDataNavigator.RaiseEventBoundCompleted();
            }
        }
        /// <summary>
        /// Sets the model item shadow.
        /// </summary>
        public void SetModelItemShadow()
        {
            ModelItemShadow = Utilities.Clone(ModelItem);
        }



        private ExecutionResult MoveToPosition(int newPosition, NavigationOperation operation)
        {
            var ERContext = $"{mClassName}.{operation}()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (UseModelData == UseModelData.InternalRepository)
                {
                    ER = operation switch
                    {
                        NavigationOperation.MoveFirst => Repository.MoveFirstItem(),
                        NavigationOperation.MoveLast => Repository.MoveLastItem(),
                        NavigationOperation.MovePrevious => Repository.MovePreviousItem(),
                        NavigationOperation.MoveNext => Repository.MoveNextItem(),
                        NavigationOperation.MoveAt => Repository.MoveAtItem(newPosition),
                        _ => throw new ArgumentException($"Invalid operation: {operation}")
                    };

                    if (ER.Success && operation == NavigationOperation.MoveLast)
                    {
                        mCurrentModelItemIndex = Repository.CurrentModelItemIndex;
                        mAddNewCurrentModelItemIndex = Repository.AddNewCurrentModelItemIndex;
                    }
                }
                else
                {
                    if (mModelItems?.Count > 0)
                    {
                        bool isValidPosition = operation switch
                        {
                            NavigationOperation.MoveFirst => true,
                            NavigationOperation.MoveLast => true,
                            NavigationOperation.MovePrevious => mCurrentModelItemIndex > 0,
                            NavigationOperation.MoveNext => mCurrentModelItemIndex < mModelItems.Count - 1,
                            NavigationOperation.MoveAt => newPosition >= 0 && newPosition < mModelItems.Count,
                            _ => false
                        };

                        if (isValidPosition)
                        {
                            mCurrentModelItemIndex = operation switch
                            {
                                NavigationOperation.MoveFirst => 0,
                                NavigationOperation.MoveLast => mModelItems.Count - 1,
                                NavigationOperation.MovePrevious => mCurrentModelItemIndex - 1,
                                NavigationOperation.MoveNext => mCurrentModelItemIndex + 1,
                                NavigationOperation.MoveAt => newPosition,
                                _ => mCurrentModelItemIndex
                            };

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
                        return ER;
                    }
                }

                if (ER.Success)
                {
                    SetModelItemShadow();
                    HandleDataBindingMode(operation);
                }

                return ER;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                return ER;
            }
        }

        private void HandleDataBindingMode(NavigationOperation operation)
        {
            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoWriteControls)
                    {
                        WriteControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    switch (operation)
                    {
                        case NavigationOperation.MoveFirst:
                            mBindingSource.MoveFirst();
                            DataNavigatorRaiseEventBoundCompled();
                            break;
                        case NavigationOperation.MoveLast:
                            mBindingSource.MoveLast();
                            DataNavigatorRaiseEventBoundCompled();
                            break;
                        case NavigationOperation.MovePrevious:
                            mBindingSource.MovePrevious();
                            DataNavigatorRaiseEventBoundCompled();
                            break;
                        case NavigationOperation.MoveNext:
                            mBindingSource.MoveNext();
                            DataNavigatorRaiseEventBoundCompled();
                            break;
                        case NavigationOperation.MoveAt:
                            mBindingSource.Position = CurrentModelItemIndex;
                            DataNavigatorRaiseEventBoundCompled();
                            break;
                    }
                    break;
            }
        }


        public ExecutionResult MoveFirstItem() => MoveToPosition(0, NavigationOperation.MoveFirst);
        public ExecutionResult MoveLastItem() => MoveToPosition(-1, NavigationOperation.MoveLast);
        public ExecutionResult MovePreviousItem() => MoveToPosition(-1, NavigationOperation.MovePrevious);
        public ExecutionResult MoveNextItem() => MoveToPosition(-1, NavigationOperation.MoveNext);
        public ExecutionResult MoveAtItem(int index) => MoveToPosition(index, NavigationOperation.MoveAt);










        /// <summary>
        /// Moves the first item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveFirstItem_OLD()
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
                    case DataBindingMode.Passero:
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


        /// <summary>
        /// Moves the last item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveLastItem_OLD()
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
                    case DataBindingMode.Passero:
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


        /// <summary>
        /// Moves the previous item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MovePreviousItem_OLD()
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
                    case DataBindingMode.Passero:
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



        /// <summary>
        /// Moves the next item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveNextItem_OLD()
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
                    case DataBindingMode.Passero:
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

        /// <summary>
        /// Moves at item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns></returns>
        public ExecutionResult MoveAtItem_OLD(int Index)
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
                    case DataBindingMode.Passero:
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


        /// <summary>
        /// The m add new state
        /// </summary>
        private bool mAddNewState;
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
        /// <param name="newItem">The new item.</param>
        /// <returns></returns>
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
                            //ReflectionHelper.CallByName( mDataNavigator, "UpdateRecordLabel", CallType.Method);
                            mDataNavigator.UpdateRecordLabel();
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


        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; } = $"ViewModel<{typeof(ModelClass).FullName}> description.";

        /// <summary>
        /// Gets or sets the data binding mode.
        /// </summary>
        /// <value>
        /// The data binding mode.
        /// </value>
        public DataBindingMode DataBindingMode
        {
            get
            {
                return mDataBindingMode;
            }

            set
            {
                mDataBindingMode = value;
                if (mDataBindingMode == DataBindingMode.BindingSource)
                {
                    if (mBindingSource == null)
                    {
                        mBindingSource = new BindingSource();
                        mBindingSource.DataSource = ModelItem;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection
        {
            get { return Repository.DbConnection; }
            set { Repository.DbConnection = value; }
        }
        /// <summary>
        /// Gets or sets the data bind controls.
        /// </summary>
        /// <value>
        /// The data bind controls.
        /// </value>
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets or sets the database transaction.
        /// </summary>
        /// <value>
        /// The database transaction.
        /// </value>
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
        /// <summary>
        /// Gets or sets the database command timeout.
        /// </summary>
        /// <value>
        /// The database command timeout.
        /// </value>
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

        /// <summary>
        /// Gets the empty model item.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        /// <summary>
        /// Gets or sets a value indicating whether [automatic write controls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic write controls]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoWriteControls { get; set; } = false;
        /// <summary>
        /// Gets or sets a value indicating whether [automatic read controls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic read controls]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoReadControls { get; set; } = false;
        /// <summary>
        /// The m data bind controls automatic set maximum lenght
        /// </summary>
        private bool mDataBindControlsAutoSetMaxLenght = true;
        /// <summary>
        /// Gets or sets a value indicating whether [data bind controls automatic set maximum lenght].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data bind controls automatic set maximum lenght]; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// The m automatic fit columns lenght
        /// </summary>
        private bool mAutoFitColumnsLenght = false;
        /// <summary>
        /// Gets or sets a value indicating whether [automatic fit columns lenght].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic fit columns lenght]; otherwise, <c>false</c>.
        /// </value>
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


        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public Repository<ModelClass> Repository { get; set; }

        //public ModelClass GetEmptyModel()
        //{
        //    return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        //}

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
                mDefaultSQLQueryParameters = Repository.DefaultSQLQueryParameters;
                return mDefaultSQLQueryParameters;
            }
            set
            {
                mDefaultSQLQueryParameters = value;
                Repository.DefaultSQLQueryParameters = value;
            }
        }
        /// <summary>
        /// The m default SQL query
        /// </summary>
        private string mDefaultSQLQuery;
        /// <summary>
        /// Gets or sets the default SQL query.
        /// </summary>
        /// <value>
        /// The default SQL query.
        /// </value>
        public string DefaultSQLQuery
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

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public DynamicParameters Parameters
        {
            get
            {
                return Repository.Parameters;

            }
            set
            {
                Repository.Parameters = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{ModelClass}"/> class.
        /// </summary>
        /// <param name="Name">ViewModel Name</param>
        /// <param name="Description">ViewModel Description</param>
        public ViewModel(string Name = "", string FriendlyName ="", string Description  = "")
        {
            Repository = new Repository<ModelClass>();
            DefaultSQLQuery = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
            DefaultSQLQueryParameters = new DynamicParameters();
            if (Name != "")
                this.Name = Name;
            else
                this.Name = $"{typeof(ModelClass).Name}_{this.GetHashCode():X}";
            
            if (FriendlyName  != "")
                this.FriendlyName = FriendlyName;
            else
                this.FriendlyName = Name;

            if (Description != "")
                this.Description = Description;
            else
                this.Description = Name;

            Repository.ViewModel = this;
            Repository.Name = $"Repository<{typeof(ModelClass).Name}>";
            Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
            Repository.ErrorNotificationMode = ErrorNotificationMode;
            mModelItemShadow = GetEmptyModelItem();


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{ModelClass}"/> class.
        /// </summary>
        /// <param name="Repository">The repository.</param>
        /// <param name="Name">The name.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        public ViewModel(ref Repository<ModelClass> Repository, string Name = "", string FriendlyName="", string Description = "")
        {

            if (Name != "")
                this.Name = Name;
            else
                this.Name = nameof(ModelClass);
            if (FriendlyName != "")
                this.FriendlyName = FriendlyName;
            else
                this.FriendlyName = Name;

            if (Description != "")
                this.Description = Description;
            else
                this.Description = Name;

            mModelItemShadow = GetEmptyModelItem();
            this.Repository = Repository;
        }
        
        /// <summary>
        /// Initializes the specified database connection.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        /// <param name="DataBindingMode">The data binding mode.</param>
        public virtual void Init(IDbConnection DbConnection, DataBindingMode DataBindingMode = DataBindingMode.Passero, string Name="", string Description="")
        {
            mDataBindingMode = DataBindingMode;
            this.DbConnection = DbConnection;
            this.Name = Name;   
            this.Description = Description; 
            Repository.DbConnection = DbConnection;
       
        }

       
        /// <summary>
        /// Reloads the items.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ReloadItems()
        {
            var ERContenxt = $"{mClassName}.ReloadItems()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);
            ER = Repository.ReloadItems();
            ER.Context = ERContenxt;
            return ER;
        }


        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetItems(string SqlQuery, object Parameters = null, IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            string ERContenxt = $"{mClassName}.GetItems()";
            ExecutionResult<IList<ModelClass>> ER = new ExecutionResult<IList<ModelClass>>(ERContenxt);


            IList<ModelClass> x = null;
            try
            {
                mCurrentModelItemIndex = -1;
                Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
                Repository.ErrorNotificationMode = ErrorNotificationMode;

                Repository.SQLQuery = SqlQuery;

                //this.Repository.Parameters = (DynamicParameters)Parameters;
                Repository.Parameters = DapperHelper.Utilities.GetDynamicParameters(Parameters);
                ER = Repository.GetItems(SqlQuery, Parameters, Transaction, Buffered, CommandTimeout);

                if (ER.Success)
                {
                    x = ER.Value;
                    mCurrentModelItemIndex = 0;
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
            MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            //DataNavigatorRaiseEventBoundCompled();
            return ER;
        }
        /// <summary>
        /// Sets the binding source.
        /// </summary>
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
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="Transaction">The transaction.</param>
        /// <param name="Buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="CommandTimeout">The command timeout.</param>
        /// <returns></returns>
        public ExecutionResult<IList<ModelClass>> GetAllItems(IDbTransaction Transaction = null, bool Buffered = true, int? CommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.GetAllItems()";
            ExecutionResult<IList<ModelClass>> ER = new ExecutionResult<IList<ModelClass>>(ERContenxt);
            IList<ModelClass> x = null;
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

            MoveFirstItem();
            LastExecutionResult = ER.ToExecutionResult();
            //DataNavigatorRaiseEventBoundCompled()
            return ER;
        }



        /// <summary>
        /// Undoes the changes.
        /// </summary>
        /// <param name="AllItems">if set to <c>true</c> [all items].</param>
        /// <returns></returns>
        public ExecutionResult UndoChanges(bool AllItems = false)
        {
            var ERContenxt = $"{mClassName}.UndoChanges()";
            var ER = new ExecutionResult(ERContenxt);
            try
            {
                if (AddNewState == false)
                {
                    ModelItem = Utilities.Clone<ModelClass>(ModelItemShadow);
                    if (ModelItems.Count >= CurrentModelItemIndex)
                    {
                        ModelItems[CurrentModelItemIndex] = Utilities.Clone<ModelClass>(ModelItemShadow);
                    }
                }
                else
                {
                    if (ModelItemsCount > 0)
                    {
                        ModelItems.RemoveAt(ModelItemsCount - 1);
                    }
                }


                //If AllItems And AutoUpdateModelItemsShadows = True Then
                ModelItems = Utilities.Clone<IList<ModelClass>>(ModelItemsShadow);
                //End If

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
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
                                //ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "UpdateRecordLabel");
                                mDataNavigator.UpdateRecordLabel();
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


        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
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
                mAddNewState = false;
                // carica il nuovo item
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        if (AutoWriteControls == true)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:

                        this.BindingSource.ResetCurrentItem();
                        break;
                    default:
                        break;
                }


            }

            LastExecutionResult = ER;



            return ER;

        }

        /// <summary>
        /// Inserts the items.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItems(List<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.InsertItems()";
            var ER = new ExecutionResult(ERContenxt);
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
            AddNewState = false;

            return ER;
        }





        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERcontext = $"{mClassName}.UpdateItem()";
            var ER = new ExecutionResult(ERcontext);
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



        /// <summary>
        /// Updates the item ex.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="ItemShadow">The item shadow.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItemEx(ModelClass Item = null, ModelClass ItemShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERcontext = $"{mClassName}.UpdateItemEx()";
            var ER = new ExecutionResult();


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

            ER = Repository.UpdateItemEx(Item, ItemShadow, DbTransaction, DbCommandTimeout);

            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Item;
            }

            ER.Context = ERcontext;
            LastExecutionResult = ER;
            return ER;

        }
        /// <summary>
        /// Updates the items.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItems(IList<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
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


        /// <summary>
        /// Updates the items ex.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="ItemsShadow">The items shadow.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItemsEx(IList<ModelClass> Items = null, IList<ModelClass> ItemsShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.UpdateItemsEx()");
            string Context = ER.Context;

            if (Items == null)
            {
                Items = ModelItems.Clone();
            }
            if (ItemsShadow == null)
            {
                ItemsShadow = ModelItemsShadow.Clone();
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


        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
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
                case DataBindingMode.Passero:
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
                    CurrentModelItemIndex = mBindingSource.CurrencyManager.Position;
                    break;
                default:
                    break;
            }

            ER.Context = Context;
            LastExecutionResult = ER;
            return ER;

        }


        /// <summary>
        /// Deletes the items.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItems(List<ModelClass> Items, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            string Context = ER.Context;

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

                for (int  i = 0;  i < Items.Count;  i++)
                {
                    ModelItems.Remove(Items[i]);
                }
                //foreach (var item in Items)
                //{
                //    ModelItems.Remove(item);
                //}

                ModelItem = ModelItems.ElementAt(0);

            }
            ER.Context = Context;
            LastExecutionResult = ER;
            return ER;

        }





        /// <summary>
        /// Gets the bound control key.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        private string GetBoundControlKey(Control Control, string PropertyName)
        {
            //string objname = Conversions.ToString(Microsoft.VisualBasic.Interaction.CallByName(Control, "Name", CallType.Get, null));
            //return (objname + "|" + PropertyName.Trim()).ToLower();


            // Versione ottimizzata compatibile con .NET Framework 4.8
            if (Control == null)
                throw new ArgumentNullException(nameof(Control));
            if (string.IsNullOrEmpty(PropertyName))
                throw new ArgumentException("Property name cannot be null or empty", nameof(PropertyName));

            var controlName = Control.Name ?? string.Empty;
            var trimmedPropertyName = PropertyName.Trim();

            // Utilizzo StringBuilder per concatenazioni multiple
            var sb = new StringBuilder(controlName.Length + trimmedPropertyName.Length + 1);
            sb.Append(controlName)
              .Append('|')
              .Append(trimmedPropertyName);

            return sb.ToString().ToLowerInvariant();

        }


        /// <summary>
        /// Adds the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <param name="BindingBehaviour">The binding behaviour.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
        public int RemoveControl(Control Control, string ControlPropertyName)
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);
            return RemoveControl(Key);
        }

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <returns></returns>
        public int RemoveControl(Control Control)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, null));
            string keytofind = (objname + "|").ToLower();
            return _RemoveControl(keytofind);
        }

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="keytofind">The keytofind.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Writes the control.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
        /// 
        public int WriteControl(ModelClass Model, Control Control, string ControlPropertyName = "")
        {
            if (mDataBindingMode == DataBindingMode.None || Control is null || Model is null)
            {
                return 0;
            }

            int _writedcontrols = 0;
            string keytofind = GetBoundControlKey(Control, ControlPropertyName);

            foreach (var key in DataBindControls.Keys.Where(k => k.StartsWith(keytofind)))
            {
                if (DataBindControls.TryGetValue(key, out var DataBindControl))
                {
                    var Value = Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Get, null);
                    Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, Value ?? "");
                    _writedcontrols++;
                }
            }

            return _writedcontrols;
        }
        public int WriteControl_OLD(ModelClass Model, Control Control, string ControlPropertyName = "")
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
                    var Value = Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Get, null);
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


        /// <summary>
        /// Writes the controls.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        public int WriteControls(ModelClass Model = null)
        {
            int _writedcontrols = 0;

            if (Model is null)
            {
                Model = ModelItem;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        //_writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                        _writedcontrols += WriteControl(Model, Control.Control);
                    }
                    break;
                case DataBindingMode.BindingSource:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        //_writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                        _writedcontrols += WriteControl(Model, Control.Control);
                    }
                    break;
                default:
                    break;
            }
            return _writedcontrols;

        }

        // Scrive il valore della proprietà del Controllo nella proprietà del Model
        /// <summary>
        /// Reads the control.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
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
                            var Value = Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Get, null);
                            Value = CheckControlValue(Value, DataBindControl.ModelPropertyName);
                            Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Set, Value);
                            _readedcontrols += 1;
                        }
                        else if ((int)(DataBindControl.BindingBehaviour & BindingBehaviour.Insert) > 0)
                        {
                            var Value = Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Get, null);
                            Value = CheckControlValue(Value, DataBindControl.ModelPropertyName);
                            Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Set, Value);
                            _readedcontrols += 1;
                        }
                    }

                }
            }
            return _readedcontrols;

        }
        /// <summary>
        /// Checks the control value.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <returns></returns>
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
                                        //s = s.Substring(0, maxlength);
                                        // Sostituzione di Substring con AsSpan
                                        s = s.AsSpan(0, maxlength).ToString();

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

        /// <summary>
        /// Sets the model shadow.
        /// </summary>
        /// <returns></returns>
        public ModelClass SetModelShadow()
        {
            ModelItemShadow = Utilities.Clone(ModelItem);

            return ModelItemShadow;
        }

        /// <summary>
        /// Sets the model items shadow.
        /// </summary>
        /// <returns></returns>
        public IList<ModelClass> SetModelItemsShadow()
        {
            mModelItemsShadow = Utilities.Clone(mModelItems);

            return mModelItemsShadow;
        }
        /// <summary>
        /// Reads the controls.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        public int ReadControls(ModelClass Model = null)
        {

            if (mDataBindingMode == DataBindingMode.BindingSource | mDataBindingMode == DataBindingMode.None)
            {
                return 0;
            }

            if (Model is null)
            {
                Model = ModelItem;
            }

            int _readedcontrols = 0;
            foreach (DataBindControl Control in DataBindControls.Values)
                _readedcontrols = _readedcontrols + ReadControl(Model, Control.Control);
            return _readedcontrols;

        }

        /// <summary>
        /// Clears the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Clears the controls.
        /// </summary>
        /// <returns></returns>
        public int ClearControls()
        {
            int writedproperties = 0;
            foreach (DataBindControl Control in DataBindControls.Values)
                writedproperties = writedproperties + ClearControl(Control.Control);
            return writedproperties;
        }





    }





}