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
        public void DataNavigatorRaiseEventBoundCompleted()
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
                        if (!ER.Success)
                            HandleExeception(ER);
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
    }
}
