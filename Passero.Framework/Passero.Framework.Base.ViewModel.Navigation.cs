using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wisej.Web;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {

        // ─── Move Events ────────────────────────────────────────────────────────────

        /// <summary>Raised before any navigation move. Set Cancel = true to abort.</summary>
        public event MoveRequestEventHandler MoveRequest;
        public delegate void MoveRequestEventHandler(NavigationOperation operation, ref bool cancel);

        /// <summary>Raised after a successful navigation move.</summary>
        public event MoveCompletedEventHandler MoveCompleted;
        public delegate void MoveCompletedEventHandler(NavigationOperation operation);

        // ────────────────────────────────────────────────────────────────────────────


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

        private void SyncAddNewStateWithRepository()
        {
            if (Repository != null)
            {
                Repository.AddNewState = mAddNewState;
            }
        }

        private void SyncNavigationStateWithRepository()
        {
            if (Repository == null)
            {
                return;
            }

            Repository.CurrentModelItemIndex = mCurrentModelItemIndex;
            Repository.AddNewCurrentModelItemIndex = mAddNewCurrentModelItemIndex;
            Repository.AddNewState = mAddNewState;
        }

        private void SyncViewModelStateFromRepository()
        {
            if (Repository == null)
            {
                return;
            }

            mCurrentModelItemIndex = Repository.CurrentModelItemIndex;
            mAddNewCurrentModelItemIndex = Repository.AddNewCurrentModelItemIndex;
            mAddNewState = Repository.AddNewState;
        }

        private void SetAddNewState(bool value, bool syncRepository = true)
        {
            mAddNewState = value;
            if (syncRepository)
            {
                SyncAddNewStateWithRepository();
            }
        }

        private void SetCurrentItemIndexes(int currentIndex, int addNewIndex, bool syncRepository = true)
        {
            mCurrentModelItemIndex = currentIndex;
            mAddNewCurrentModelItemIndex = addNewIndex;
            if (syncRepository)
            {
                SyncNavigationStateWithRepository();
            }
        }

        private void ApplyViewModelStateAfterRepositorySync(bool syncRepository = true)
        {
            if (syncRepository)
            {
                SyncNavigationStateWithRepository();
            }
            else
            {
                SyncViewModelStateFromRepository();
            }
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

        private ModelClass _newModeltem;

        public ModelClass? NewModeltem
        {
            get { return _newModeltem ?? GetEmptyModelItem(); }
            set { _newModeltem = value; }
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

                return Repository.ModelItem;
                
            }
            set
            {
                Repository.ModelItem = value;
                
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
                return Repository.ModelItems;
            }
            set
            {
                Repository.ModelItems = value;
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
                return Repository.ModelItemsShadow;
                
            }
            set
            {
                Repository.ModelItemsShadow = value;
                
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
                return Repository.ModelItems.Count;
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
                return Repository.CurrentModelItemIndex;
            }
            set
            {
                SetCurrentItemIndexes(value, mAddNewCurrentModelItemIndex, true);
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
                return Repository.AddNewCurrentModelItemIndex;
            }
            set
            {
                SetCurrentItemIndexes(mCurrentModelItemIndex, value, true);
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
        /// Gets the model items at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ExecutionResult<ModelClass> GetModelItemsAt(int index)
        {
            var ERContext = $"{mClassName}.GetModelItemsAt()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContext);
            ER = Repository.GetModelItemsAt(index);


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
                return Repository.ModelItemShadow;
            }
            set
            {
                Repository.ModelItemShadow = value;

            }
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
            return Repository.IsModelDataChanged(ModelShadow);

        }

        /// <summary>
        /// Datas the navigator raise event bound completed.
        /// </summary>
        public void DataNavigatorRaiseEventBoundCompleted()
        {
            if (mDataNavigator != null)
            {
                //ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "RaiseEventBoundCompleted");
                mDataNavigator.RaiseEventBoundCompleted();
            }
        }


        /// <summary>
        /// Datas the navigator raise event movement completed.
        /// </summary>
        public void DataNavigatorRaiseEventMoveCompleted()
        {
            if (mDataNavigator != null)
            {
                //ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "RaiseEventMoveCompleted");
                mDataNavigator.RaiseEventMoveCompleted();
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
                //if (UseModelData == UseModelData.InternalRepository)
                // Raise MoveRequest — allow cancellation
                bool cancel = false;
                MoveRequest?.Invoke(operation, ref cancel);
                if (cancel)
                {
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ResultMessage = "Move cancelled by MoveRequest handler.";
                    ER.ErrorCode = 0;
                    return ER;
                }

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

                    if (ER.Success)
                    {
                        SyncViewModelStateFromRepository();
                    }
                }

                if (ER.Success)
                {

                    SetModelItemShadow();
                    HandleDataBindingMode(operation);
                    // Raise MoveCompleted
                    MoveCompleted?.Invoke(operation);
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
                            DataNavigatorRaiseEventBoundCompleted();
                            break;
                        case NavigationOperation.MoveLast:
                            mBindingSource.MoveLast();
                            DataNavigatorRaiseEventBoundCompleted();
                            break;
                        case NavigationOperation.MovePrevious:
                            mBindingSource.MovePrevious();
                            DataNavigatorRaiseEventBoundCompleted();
                            break;
                        case NavigationOperation.MoveNext:
                            mBindingSource.MoveNext();
                            DataNavigatorRaiseEventBoundCompleted();
                            break;
                        case NavigationOperation.MoveAt:
                            mBindingSource.Position = CurrentModelItemIndex;
                            DataNavigatorRaiseEventBoundCompleted();
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
                SetAddNewState(value, true);
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


            if (!RaiseRequest(AddNewRequest))
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "AddNew cancelled by AddNewRequest handler.";
                return ER;
            }

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
                    SetAddNewState(true, true);

                    if (ER.Success)
                        AddNewCompleted?.Invoke();

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
    }
}
