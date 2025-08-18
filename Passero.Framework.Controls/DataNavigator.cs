using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
//using Passero.Framework.Base;
using Wisej.Web;

namespace Passero.Framework.Controls
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Wisej.Web.UserControl" />
    public partial class DataNavigator
    {
        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.Controls.DataNavigator";

        /// <summary>
        ///   <br />
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// The move
            /// </summary>
            Move,
            /// <summary>
            /// The move first
            /// </summary>
            MoveFirst,
            /// <summary>
            /// The move last
            /// </summary>
            MoveLast,
            /// <summary>
            /// The move next
            /// </summary>
            MoveNext,
            /// <summary>
            /// The move previous
            /// </summary>
            MovePrevious,
            /// <summary>
            /// The add new
            /// </summary>
            AddNew,
            /// <summary>
            /// The delete
            /// </summary>
            Delete,
            /// <summary>
            /// The save
            /// </summary>
            Save,
            /// <summary>
            /// The close
            /// </summary>
            Close,
            /// <summary>
            /// The undo
            /// </summary>
            Undo,
            /// <summary>
            /// The print
            /// </summary>
            Print,
            /// <summary>
            /// The find
            /// </summary>
            Find,
            /// <summary>
            /// The refresh
            /// </summary>
            Refresh
        }

        /// <summary>
        /// The ListView columns
        /// </summary>
        public ListViewColumns ListViewColumns = new ListViewColumns();
        /// <summary>
        /// The data grid ListView data table
        /// </summary>
        private DataTable _DataGridListViewDataTable;
        /// <summary>
        /// The data grid ListView default row height
        /// </summary>
        private int _DataGridListViewDefaultRowHeight = 24;
        /// <summary>
        /// The move previous caption
        /// </summary>
        private string _MovePreviousCaption = "Prev.";
        /// <summary>
        /// The move next caption
        /// </summary>
        private string _MoveNextCaption = "Next";
        /// <summary>
        /// The move first caption
        /// </summary>
        private string _MoveFirstCaption = "First";
        /// <summary>
        /// The move last caption
        /// </summary>
        private string _MoveLastCaption = "Last";
        /// <summary>
        /// The add new caption
        /// </summary>
        private string _AddNewCaption = "New";
        /// <summary>
        /// The delete caption
        /// </summary>
        private string _DeleteCaption = "Delete";
        /// <summary>
        /// The save caption
        /// </summary>
        private string _SaveCaption = "Save";
        /// <summary>
        /// The refresh caption
        /// </summary>
        private string _RefreshCaption = "Refresh";
        /// <summary>
        /// The undo caption
        /// </summary>
        private string _UndoCaption = "Undo";
        /// <summary>
        /// The close caption
        /// </summary>
        private string _CloseCaption = "Close";
        /// <summary>
        /// The find caption
        /// </summary>
        private string _FindCaption = "Find";
        /// <summary>
        /// The print caption
        /// </summary>
        private string _PrintCaption = "Print";

        /// <summary>
        /// The print f key
        /// </summary>
        private int _PrintFKey = (int)Keys.F8;

        /// <summary>
        /// Raises the event bound completed.
        /// </summary>
        public void RaiseEventBoundCompleted()
        {
            eBoundCompleted?.Invoke();
            _DataBoundCompleted = true;
            //if (eBoundCompleted != null)
            //    eBoundCompleted();
        }

        /// <summary>
        /// Gets or sets the record label HTML format.
        /// </summary>
        /// <value>
        /// The record label HTML format.
        /// </value>
        public string RecordLabelHtmlFormat { get; set; } = "<p style='margin-top:2px;line-height:1.0;text-align:center;'>{0}<br>{1}<br>{2}</p>";

        /// <summary>
        /// Gets or sets the view models.
        /// </summary>
        /// <value>
        /// The view models.
        /// </value>
        public Dictionary<string, DataNavigatorViewModel> ViewModels { get; set; } = new Dictionary<string, DataNavigatorViewModel>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// The data repeater
        /// </summary>
        private DataRepeater __DataRepeater;
        /// <summary>
        /// Gets or sets the data repeater.
        /// </summary>
        /// <value>
        /// The data repeater.
        /// </value>
        private DataRepeater _DataRepeater
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return __DataRepeater;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                __DataRepeater = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use update ex].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use update ex]; otherwise, <c>false</c>.
        /// </value>
        public bool UseUpdateEx { get; set; } = false;
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
                if (_ActiveViewModel == null)
                    return 0;
                //return (int)ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "ModelItemsCount");
                return (int)_ActiveViewModel.ModelItemsCount;
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
                if (_ActiveViewModel != null)
                    return -1;
                if (DesignMode == false)
                    //return (int)ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "CurrentModelItemIndex");
                    return (int)_ActiveViewModel.CurrentModelItemIndex;
                else
                    return -1;
            }
            set
            {
                if (DesignMode == false)
                {
                    if (_ActiveViewModel != null)
                        //ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "CurrentModelItemIndex", value);
                        _ActiveViewModel.CurrentModelItemIndex = value;
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

                if (_ActiveViewModel != null)
                    return -1;
                if (DesignMode == false)
                    //return (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "AddNewCurrentModelItemIndex");
                    return (int)_ActiveViewModel.AddNewCurrentModelItemIndex;
                else
                    return -1;
            }
            set
            {
                if (DesignMode == false)
                {
                    if (_ActiveViewModel != null)
                        //ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "AddNewCurrentModelItemIndex", value);
                        _ActiveViewModel.AddNewCurrentModelItemIndex = value;
                }
            }
        }




        /// <summary>
        /// Views the model add new.
        /// </summary>
        /// <param name="NewItem">The new item.</param>
        /// <param name="InsertAtCursor">if set to <c>true</c> [insert at cursor].</param>
        /// <returns></returns>
        public ExecutionResult ViewModel_AddNew(object NewItem = null, bool InsertAtCursor = false)
        {
            string ERContext = $"{mClassName}ViewModel_AddNew()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    ER = DataGrid_AddNew(NewItem, InsertAtCursor);
                    break;
                case ViewModelGridModes.DataRepeater:
                    ER = DataRepeater_AddNew(NewItem, InsertAtCursor);
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "AddNew", Microsoft.VisualBasic.CallType.Method, NewItem);
                    ER = (ExecutionResult)ActiveViewModel.AddNew(NewItem);
                    break;
            }


            if (ER.Success)
            {
                _AddNewState = true;
                SetButtonsForAddNew();
                if (eAddNewCompleted != null)
                    eAddNewCompleted();
            }
            else
            {
                if (eError != null)
                    eError("ViewModel_AddNew", ER);
            }

            return ER;

        }



        /// <summary>
        /// Views the model delete item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_DeleteItem()
        {
            string ERContext = $"{mClassName}.ViewModel_DeleteItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        ER = DataGrid_Delete();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        ER = DataRepeater_Delete();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "DeleteItem", Microsoft.VisualBasic.CallType.Method, null);
                    ER = (ExecutionResult)ActiveViewModel.DeleteItem(null);
                    break;
            }
            ER.Context = ERContext;
            return ER;

        }



        /// <summary>
        /// Views the model delete items.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_DeleteItems()
        {
            string ERContext = $"{mClassName}.ViewModel_DeleteItems()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "DeleteItems", Microsoft.VisualBasic.CallType.Method, null);
            ER = (ExecutionResult)ActiveViewModel.DeleteItems(null);
            ER.Context = ERContext;
            return ER;
        }



        /// <summary>
        /// Views the model undo changes.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_UndoChanges()
        {
            string ERContext = $"{mClassName}.ViewModel_UndoChanges()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        ER = DataGrid_Undo();
                        if (eUndoCompleted != null)
                            eUndoCompleted();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        DataRepeater_Undo();
                        if (eUndoCompleted != null)
                            eUndoCompleted();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "UndoChanges", Microsoft.VisualBasic.CallType.Method);
                    //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref this._ActiveViewModel, "UndoChanges", false);
                    ER = (ExecutionResult)_ActiveViewModel.UndoChanges(false);
                    if (ER.Success)
                    {
                        if (eUndoCompleted != null)
                            eUndoCompleted();
                    }
                    break;
            }
            ER.Context = ERContext;
            return ER;


        }

        /// <summary>
        /// Views the model move first item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_MoveFirstItem()
        {

            string ERContext = $"{mClassName}.ViewModel_MoveFirstItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        ER = DataGrid_MoveFirst();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        ER = DataRepeater_MoveFirst();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "MoveFirstItem", Microsoft.VisualBasic.CallType.Method);
                    ER = (ExecutionResult)ActiveViewModel.MoveFirstItem();
                    break;
            }

            ER.Context = ERContext;
            return ER;
        }

        /// <summary>
        /// Views the model move last item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_MoveLastItem()
        {

            string ERContext = $"{mClassName}.ViewModel_MoveLastItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        ER = DataGrid_MoveLast();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        ER = DataRepeater_MoveLast();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "MoveLastItem", Microsoft.VisualBasic.CallType.Method);
                    ER = (ExecutionResult)ActiveViewModel.MoveLastItem();
                    break;
            }
            ER.Context = ERContext;
            return ER;
        }




        /// <summary>
        /// Views the model move previous item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_MovePreviousItem()
        {
            string ERContext = $"{mClassName}.ViewModel_MovePreviousItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        ER = DataGrid_MovePrevious();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        ER = DataRepeater_MovePrevious();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "MovePreviousItem", Microsoft.VisualBasic.CallType.Method);
                    ER = (ExecutionResult)ActiveViewModel.MovePreviousItem();
                    break;
            }

            ER.Context = ERContext;
            return ER;

        }


        /// <summary>
        /// Views the model move next item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_MoveNextItem()
        {
            string ERContext = $"{mClassName}.ViewModel_MoveNextItem()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        ER = DataGrid_MoveNext();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        ER = DataRepeater_MoveNext();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this._ActiveViewModel, "MoveNextItem", Microsoft.VisualBasic.CallType.Method);
                    ER = (ExecutionResult)_ActiveViewModel.MoveNextItem();
                    break;
            }
            ER.Context = ERContext;
            return ER;

        }

        /// <summary>
        /// Views the model reload items.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_ReloadItems()
        {

            string ERContext = $"{mClassName}.ViewModel_ReloadItems()"; ;
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        _DataGridView.DataSource = ModelItems;
                        ER = DataGrid_MoveFirst();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {

                        if (_DataRepeater.DataSource is BindingSource bs)
                        {
                            bs.ResetBindings(false);
                        }
                        else
                        {
                            _DataRepeater.DataSource = ModelItems;
                        }
                        
                        //Me._DataRepeater.Refresh()
                        ER = DataRepeater_MoveFirst();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "ReloadItems", Microsoft.VisualBasic.CallType.Method);
                    ER = (ExecutionResult)ActiveViewModel.ReloadItems();


                    break;
            }
            ER.Context = ERContext;
            return ER;
        }


        /// <summary>
        /// Views the model udpate item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public ExecutionResult ViewModel_UdpateItem(dynamic item = null)
        {
            string ERContext = $"{mClassName}.ViewModel_UdpateItem()";

            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        DataGrid_Save();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        DataRepeater_Save();
                    }
                    break;
                default:

                    if (DataBindingMode() == Framework.DataBindingMode.BindingSource)
                    {
                    }

                    if (_AddNewState)
                    {
                        //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "InsertItem", item);
                        ER = (ExecutionResult)_ActiveViewModel.InsertItem(item);
                        if (ER.Success)
                        {
                            ViewModel_MoveLastItem();
                            _AddNewState = false;
                        }
                    }
                    else
                    {
                        if (UseUpdateEx)
                            //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "UpdateItem", item);
                            ER = (ExecutionResult)_ActiveViewModel.UpdateItem(item);
                        else
                            //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "UpdateItemEx", item);
                            ER = (ExecutionResult)_ActiveViewModel.UpdateItemEx(item);
                    }
                    break;

            }
            ER.Context = ERContext;


            if (ER.Success)
            {
                SetButtonForSave();
                if (eSaveCompleted != null)
                    eSaveCompleted();
            }
            else
            {
                if (eError != null)
                    eError("ViewModel_UdpateItem", ER);
            }

            return ER;

        }


        /// <summary>
        /// Views the model udpate items.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ViewModel_UdpateItems()
        {

            string ERContext = $"{mClassName}.ViewModel_UdpateItems()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            switch (_ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (_DataGridView != null && DataGridActive)
                    {
                        DataGrid_Save();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (_DataRepeater != null && DataGridActive)
                    {
                        DataRepeater_Save();
                    }
                    break;
                default:
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "UpdateItems", Microsoft.VisualBasic.CallType.Method, null);
                    ER = (ExecutionResult)ActiveViewModel.UpdateItems(null);
                    break;
            }

            ER.Context = ERContext;
            return ER;
        }



        /// <summary>
        /// Sets the active view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void SetActiveViewModel(DataNavigatorViewModel viewModel)
        {
            var typename = typeof(ViewModel<>).Name; // "ViewModel`1";
            if (Equals(viewModel.ViewModel.GetType().Name, typename) || Equals(viewModel.ViewModel.GetType().BaseType.Name, typename))
            {
                _ActiveViewModel = viewModel.ViewModel;
                _ActiveDataNavigatorViewModel = viewModel;
                //_ModelItems = ReflectionHelper.CallByName(viewModel.ViewModel, "ModelItems", Microsoft.VisualBasic.CallType.Get, null);
                _ModelItems = viewModel.ViewModel.ModelItems;
                try
                {
                    //_ModelItem = ReflectionHelper.CallByName(viewModel.ViewModel, "ModelItem", Microsoft.VisualBasic.CallType.Get, null);
                    _ModelItem = viewModel.ViewModel.ModelItem;
                }
                catch (Exception)
                {
                    _ModelItem = _ModelItems.GetType().GetGenericArguments()[0];
                }
                //_BindingSource = (BindingSource)ReflectionHelper.GetPropertyValue(viewModel.ViewModel, "BindingSource");
                //ReflectionHelper.CallByName(viewModel.ViewModel, "DataNavigator", CallType.Set, this);
                _BindingSource = viewModel.ViewModel.BindingSource;
                viewModel.ViewModel.DataNavigator = this;

                switch (ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridView != null)
                        {
                            _DataGridView.ReadOnly = true;
                        }
                        if (viewModel.DataGridView != null)
                        {
                            _DataGridView = viewModel.DataGridView;
                            _DataGridView.Click -= DataGridView_Click;
                            _DataGridView.Click += DataGridView_Click;
                            //_DataGridView.RowStateChanged -= _DataGridView_RowStateChanged;
                            //_DataGridView.RowStateChanged += _DataGridView_RowStateChanged;
                            _DataGridView.SelectionChanged -= _DataGridView_SelectionChanged;
                            _DataGridView.SelectionChanged += _DataGridView_SelectionChanged;
                            _DataGridActive = true;
                            _DataGridView.ReadOnly = false;
                        }
                        else
                        {
                            _DataGridView = null;
                            _DataGridActive = false;
                        }

                        if (_DataGridView != null && _DataGridActive == true)
                        {
                            _DataGridView.DataSource = _ModelItems;
                        }
                        RaiseEventBoundCompleted();
                        break;

                    case ViewModelGridModes.DataRepeater:

                        if (_DataRepeater != null)
                        {
                            _DataRepeater.Enabled = false;
                        }
                        if (viewModel.DataRepeater != null)
                        {
                            _DataRepeater = viewModel.DataRepeater;
                            _DataRepeater.Click -= DataRepeater_Click;
                            _DataRepeater.Click += DataRepeater_Click;
                            _DataGridActive = true;
                            _DataRepeater.Enabled = true;
                        }
                        else
                        {
                            _DataRepeater = null;
                            _DataGridActive = false;
                        }

                        if (_DataRepeater != null && _DataGridActive == true)
                        {
                            _DataRepeater.DataSource = _ModelItems;
                        }
                        RaiseEventBoundCompleted();
                        break;

                    default:
                        break;

                }

                if (viewModel.Name != "")
                    Caption = viewModel.Name;
                if (viewModel.FriendlyName != "")
                    Caption = viewModel.FriendlyName;

            }
            else
            {
                _ActiveViewModel = null;
                _ActiveDataNavigatorViewModel = null;
                //ReflectionHelper.CallByName(viewModel.ViewModel, "DataNavigator", CallType.Set, null);
                viewModel.ViewModel.DataNavigator = null;
            }
            if (_ModelItems != null)
            {
                EnableAllButtons();
            }
            UpdateRecordLabel();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the _DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _DataGridView_SelectionChanged(object sender, EventArgs e)
        {

            CurrentModelItemIndex = DataGridView.CurrentRow.Index;
            UpdateRecordLabel();
        }

        /// <summary>
        /// Handles the RowStateChanged event of the _DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowStateChangedEventArgs"/> instance containing the event data.</param>
        private void _DataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //if (e.StateChanged == DataGridViewElementStates.Selected)
            //{
            //    this.CurrentModelItemIndex  = this.DataGridView.CurrentRow.Index;
            //    UpdateRecordLabel();
            //}
        }

        /// <summary>
        /// Handles the Click event of the DataRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DataRepeater_Click(object sender, EventArgs e)
        {
            //if (this.DataRepeater.CurrentRow != null)
            //{
            //    this.CurrentModelItemIndex = this._DataRepeater.CurrentRow.Index;
            //    this.UpdateRecordLabel();
            //}
        }

        /// <summary>
        /// Sets the active view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void SetActiveViewModel(string viewModel)
        {
            if (ViewModels.ContainsKey(viewModel))
            {
                SetActiveViewModel(ViewModels[viewModel]);
            }
        }




        /// <summary>
        /// The active view model
        /// </summary>
        private dynamic   _ActiveViewModel = null;
        /// <summary>
        /// Gets the active view model.
        /// </summary>
        /// <value>
        /// The active view model.
        /// </value>
        public dynamic ActiveViewModel
        {
            get
            {
                return _ActiveViewModel;
            }

            
        }
        
        public bool ActiveViewModelIs(object viewModel)    
        {
            if (_ActiveViewModel.Equals(viewModel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType
        {
            get
            {
                if (_ActiveViewModel != null)
                {
                    //return (Type)ReflectionHelper.CallByName(this._ActiveViewModel, "ModelType", CallType.Get, null);
                    return _ActiveViewModel.ModelType;
                }
                return null;
            }

        }


        /// <summary>
        /// The model item
        /// </summary>
        private object _ModelItem = null;
        /// <summary>
        /// Gets or sets the model item.
        /// </summary>
        /// <value>
        /// The model item.
        /// </value>
        public object ModelItem
        {
            get
            {
                if (_ActiveViewModel != null)
                {
                    //_ModelItem = ReflectionHelper.CallByName(_ActiveViewModel, "ModelItem", Microsoft.VisualBasic.CallType.Get, null);
                    _ModelItem = _ActiveViewModel.ModelItem;
                    return _ModelItem;
                }
                return null;
            }
            set
            {
                if (_ActiveViewModel != null)
                {
                    //ReflectionHelper.CallByName(_ActiveViewModel, "ModelItem", Microsoft.VisualBasic.CallType.Set, value);
                    _ActiveViewModel.ModelItem = value;
                    _ModelItem = value;
                }
            }
        }


        /// <summary>
        /// The model items
        /// </summary>
        private dynamic _ModelItems = null;

        /// <summary>
        /// Gets or sets the model items.
        /// </summary>
        /// <value>
        /// The model items.
        /// </value>
        public dynamic ModelItems
        {
            get
            {
                if (_ActiveViewModel != null)
                {
                    //_ModelItems = ReflectionHelper.CallByName(_ActiveViewModel, "ModelItems", Microsoft.VisualBasic.CallType.Get, null);
                    _ModelItems = _ActiveViewModel.ModelItems;
                    return _ModelItems;
                }
                return null;
            }
            set
            {
                if (_ActiveViewModel != null)
                {
                    //ReflectionHelper.CallByName(_ActiveViewModel, "ModelItems", Microsoft.VisualBasic.CallType.Set, value);
                    _ActiveViewModel.ModelItems = value;
                    _ModelItems = value;
                }
            }
        }




        /// <summary>
        /// Gets or sets the model item shadow.
        /// </summary>
        /// <value>
        /// The model item shadow.
        /// </value>
        public dynamic ModelItemShadow
        {
            get
            {
                if (_ActiveViewModel != null)
                {
                    //return ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemShadow", CallType.Get, null);
                    return _ActiveViewModel.ModelItemShadow;
                }
                return null;
            }
            set
            {
                if (_ActiveViewModel != null)
                {
                    //ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemShadow", CallType.Set, value);
                    _ActiveViewModel.ModelItemShadow = value;
                }
            }
        }



        /// <summary>
        /// Gets or sets the model items shadow.
        /// </summary>
        /// <value>
        /// The model items shadow.
        /// </value>
        public dynamic ModelItemsShadow
        {
            get
            {
                if (_ActiveViewModel != null)
                {
                    //return ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemsShadow", CallType.Get, null);
                    return _ActiveViewModel.ModelItemsShadow;
                }
                return null;
            }
            set
            {
                if (_ActiveViewModel != null)
                {
                    //ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemsShadow", CallType.Set, value);
                    _ActiveViewModel.ModelItemsShadow = value;
                }
            }
        }

        /// <summary>
        /// The dataset
        /// </summary>
        private DataSet _Dataset;
        /// <summary>
        /// The data table
        /// </summary>
        private DataTable _DataTable;
        /// <summary>
        /// The currency manager
        /// </summary>
        private Wisej.Web.CurrencyManager _CurrencyManager = null;
        /// <summary>
        /// The binding source
        /// </summary>
        private Wisej.Web.BindingSource _BindingSource = new BindingSource();
        /// <summary>
        /// The data grid ListView row index column name
        /// </summary>
        private string _DataGridListViewRowIndexColumnName = "$<rowindex>$";
        /// <summary>
        /// The data grid ListView row index column index
        /// </summary>
        private int _DataGridListViewRowIndexColumnIndex = 0;
        /// <summary>
        /// The data grid
        /// </summary>
        private DataGridView __DataGrid;

        /// <summary>
        /// Gets or sets the data grid view.
        /// </summary>
        /// <value>
        /// The data grid view.
        /// </value>
        private DataGridView _DataGridView
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return __DataGrid;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                __DataGrid = value;
            }
        }
        /// <summary>
        /// The data grid ListView
        /// </summary>
        private DataGridView __DataGridListView;
        /// <summary>
        /// Gets or sets the data grid ListView.
        /// </summary>
        /// <value>
        /// The data grid ListView.
        /// </value>
        private DataGridView _DataGridListView
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return __DataGridListView;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (__DataGridListView != null)
                {
                    __DataGridListView.RowEnter -= _DataGridListView_RowEnter;
                }

                __DataGridListView = value;
                if (__DataGridListView != null)
                {
                    __DataGridListView.RowEnter += _DataGridListView_RowEnter;
                }
            }
        }



        /// <summary>
        /// The delete message
        /// </summary>
        private string _DeleteMessage = "Confirm delete data?";
        /// <summary>
        /// The save message
        /// </summary>
        private string _SaveMessage = "Confirm save data?";
        /// <summary>
        /// The add new message
        /// </summary>
        private string _AddNewMessage = "Confirm new data insert?";

        /// <summary>
        /// The manage navigation
        /// </summary>
        private bool _ManageNavigation = true;
        /// <summary>
        /// The manage changes
        /// </summary>
        private bool _ManageChanges = true;
        /// <summary>
        /// The data grid active
        /// </summary>
        private bool _DataGridActive = false;
        /// <summary>
        /// The data grid ListView active
        /// </summary>
        private bool _DataGridListViewActive = false;

        /// <summary>
        /// The data panel
        /// </summary>
        private Panel _DataPanel;

        /// <summary>
        /// Occurs when [e add new].
        /// </summary>
        public event eAddNewEventHandler eAddNew;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAddNewEventHandler();

        /// <summary>
        /// Occurs when [e add new completed].
        /// </summary>
        public event eAddNewCompletedEventHandler eAddNewCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAddNewCompletedEventHandler();

        /// <summary>
        /// Occurs when [e print].
        /// </summary>
        public event ePrintEventHandler ePrint;
        /// <summary>
        /// 
        /// </summary>
        public delegate void ePrintEventHandler();

        //public event ePrintCompletedEventHandler ePrintCompleted;
        //public delegate void ePrintCompletedEventHandler();

        /// <summary>
        /// Occurs when [e delete].
        /// </summary>
        public event eDeleteEventHandler eDelete;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eDeleteEventHandler();

        /// <summary>
        /// Occurs when [e delete completed].
        /// </summary>
        public event eDeleteCompletedEventHandler eDeleteCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eDeleteCompletedEventHandler();

        /// <summary>
        /// Occurs when [e refresh].
        /// </summary>
        public event eRefreshEventHandler eRefresh;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eRefreshEventHandler();

        /// <summary>
        /// Occurs when [e refresh completed].
        /// </summary>
        public event eRefreshCompletedEventHandler eRefreshCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eRefreshCompletedEventHandler();

        /// <summary>
        /// Occurs when [e close].
        /// </summary>
        public event eCloseEventHandler eClose;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eCloseEventHandler();

        /// <summary>
        /// Occurs when [e find].
        /// </summary>
        public event eFindEventHandler eFind;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eFindEventHandler();

        /// <summary>
        /// Occurs when [e save].
        /// </summary>
        public event eSaveEventHandler eSave;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eSaveEventHandler();

        /// <summary>
        /// Occurs when [e save completed].
        /// </summary>
        public event eSaveCompletedEventHandler eSaveCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eSaveCompletedEventHandler();


        /// <summary>
        /// Occurs when [e move previous].
        /// </summary>
        public event eMovePreviousEventHandler eMovePrevious;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMovePreviousEventHandler();

        /// <summary>
        /// Occurs when [e move first].
        /// </summary>
        public event eMoveFirstEventHandler eMoveFirst;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveFirstEventHandler();

        /// <summary>
        /// Occurs when [e move last].
        /// </summary>
        public event eMoveLastEventHandler eMoveLast;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveLastEventHandler();

        /// <summary>
        /// Occurs when [e move next].
        /// </summary>
        public event eMoveNextEventHandler eMoveNext;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveNextEventHandler();

        /// <summary>
        /// Occurs when [e move at item].
        /// </summary>
        public event eMoveAtItemEventHandler eMoveAtItem;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveAtItemEventHandler();

        /// <summary>
        /// Occurs when [e undo].
        /// </summary>
        public event eUndoEventHandler eUndo;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eUndoEventHandler();
        /// <summary>
        /// Occurs when [e undo completed].
        /// </summary>
        public event eUndoCompletedEventHandler eUndoCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eUndoCompletedEventHandler();

        /// <summary>
        /// Occurs when [e bound completed].
        /// </summary>
        public event eBoundCompletedEventHandler eBoundCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eBoundCompletedEventHandler();

        /// <summary>
        /// Occurs when [e add new request].
        /// </summary>
        public event eAddNewRequestEventHandler eAddNewRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eAddNewRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e after add new request].
        /// </summary>
        public event eAfterAddNewEventHandler eAfterAddNewRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eAfterAddNewEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e print request].
        /// </summary>
        public event ePrintRequestEventHandler ePrintRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void ePrintRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e after print].
        /// </summary>
        public event eAfterPrintEventHandler eAfterPrint;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAfterPrintEventHandler();

        /// <summary>
        /// Occurs when [e delete request].
        /// </summary>
        public event eDeleteRequestEventHandler eDeleteRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eDeleteRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e after delete].
        /// </summary>
        public event eAfterDeleteEventHandler eAfterDelete;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAfterDeleteEventHandler();

        /// <summary>
        /// Occurs when [e refresh request].
        /// </summary>
        public event eRefreshRequestEventHandler eRefreshRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eRefreshRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e after refresh].
        /// </summary>
        public event eAfterRefreshEventHandler eAfterRefresh;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAfterRefreshEventHandler();

        /// <summary>
        /// Occurs when [e close request].
        /// </summary>
        public event eCloseRequestEventHandler eCloseRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eCloseRequestEventHandler(ref bool Cancel);


        /// <summary>
        /// Occurs when [e after close].
        /// </summary>
        public event eAfterCloseEventHandler eAfterClose;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAfterCloseEventHandler();

        /// <summary>
        /// Occurs when [e find request].
        /// </summary>
        public event eFindRequestEventHandler eFindRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eFindRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e after find].
        /// </summary>
        public event eAfterFindEventHandler eAfterFind;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eAfterFindEventHandler();

        /// <summary>
        /// Occurs when [e save request].
        /// </summary>
        public event eSaveRequestEventHandler eSaveRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eSaveRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e move previous request].
        /// </summary>
        public event eMovePreviousRequestEventHandler eMovePreviousRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eMovePreviousRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e move previous completed].
        /// </summary>
        public event eMovePreviousCompletedEventHandler eMovePreviousCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMovePreviousCompletedEventHandler();

        /// <summary>
        /// Occurs when [e move first request].
        /// </summary>
        public event eMoveFirstRequestEventHandler eMoveFirstRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eMoveFirstRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e move first completed].
        /// </summary>
        public event eMoveFirstCompletedEventHandler eMoveFirstCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveFirstCompletedEventHandler();

        /// <summary>
        /// Occurs when [e move last request].
        /// </summary>
        public event eMoveLastRequestEventHandler eMoveLastRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eMoveLastRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e move last completed].
        /// </summary>
        public event eMoveLastCompletedEventHandler eMoveLastCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveLastCompletedEventHandler();

        /// <summary>
        /// Occurs when [e move next request].
        /// </summary>
        public event eMoveNextRequestEventHandler eMoveNextRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eMoveNextRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e move next completed].
        /// </summary>
        public event eMoveNextCompletedEventHandler eMoveNextCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveNextCompletedEventHandler();

        /// <summary>
        /// Occurs when [e move at item request].
        /// </summary>
        public event eMoveAtItemRequestEventHandler eMoveAtItemRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eMoveAtItemRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e move at item completed].
        /// </summary>
        public event eMoveAtItemCompletedEventHandler eMoveAtItemCompleted;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eMoveAtItemCompletedEventHandler();

        /// <summary>
        /// Occurs when [e undo request].
        /// </summary>
        public event eUndoRequestEventHandler eUndoRequest;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public delegate void eUndoRequestEventHandler(ref bool Cancel);

        /// <summary>
        /// Occurs when [e error].
        /// </summary>
        public event eErrorEventHandler eError;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Operation">The operation.</param>
        /// <param name="ExecutionResult">The execution result.</param>
        public delegate void eErrorEventHandler(string Operation, ExecutionResult ExecutionResult);

        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _ReadOnlyMode = false;
        /// <summary>
        /// The add new state
        /// </summary>
        private bool _AddNewState = false;
        /// <summary>
        /// The new item index
        /// </summary>
        private int _NewItemIndex = -1;
        /// <summary>
        /// The delete pending
        /// </summary>
        private bool _DeletePending = false;
        /// <summary>
        /// The save pending
        /// </summary>
        private bool _SavePending = false;
        /// <summary>
        /// The print pending
        /// </summary>
        private bool _PrintPending = false;
        /// <summary>
        /// The find pending
        /// </summary>
        private bool _FindPending = false;
        /// <summary>
        /// The undo pending
        /// </summary>
        private bool _UndoPending = false;
        /// <summary>
        /// The navigation enabled
        /// </summary>
        private bool _NavigationEnabled = true;
        /// <summary>
        /// The data grid row
        /// </summary>
        private int _DataGridRow = -1;
        /// <summary>
        /// The compact mode
        /// </summary>
        private bool _CompactMode = false;


        /// <summary>
        /// The move previous f key
        /// </summary>
        private Keys _MovePreviousFKey = Keys.F6;
        /// <summary>
        /// The move next f key
        /// </summary>
        private Keys _MoveNextFKey = Keys.F7;
        /// <summary>
        /// The move first f key
        /// </summary>
        private Keys _MoveFirstFKey = (Keys)((int)Keys.Shift + (int)Keys.F6);
        /// <summary>
        /// The move last f key
        /// </summary>
        private Keys _MoveLastFKey = (Keys)((int)Keys.Shift + (int)Keys.F7);
        /// <summary>
        /// The add new f key
        /// </summary>
        private Keys _AddNewFKey = Keys.F2;
        /// <summary>
        /// The delete f key
        /// </summary>
        private Keys _DeleteFKey = Keys.F3;
        /// <summary>
        /// The save f key
        /// </summary>
        private Keys _SaveFKey = Keys.F10;
        /// <summary>
        /// The refresh f key
        /// </summary>
        private Keys _RefreshFKey = Keys.F5;
        /// <summary>
        /// The undo f key
        /// </summary>
        private Keys _UndoFKey = Keys.F9;
        /// <summary>
        /// The close f key
        /// </summary>
        private Keys _CloseFKey = Keys.F12;
        /// <summary>
        /// The find key
        /// </summary>
        private Keys _FindKey = Keys.F4;

        /// <summary>
        /// The f key enabled
        /// </summary>
        private bool _FKeyEnabled = false;

        /// <summary>
        /// The record label separator
        /// </summary>
        private string _RecordLabelSeparator = "of";
        /// <summary>
        /// The record label new row
        /// </summary>
        private string _RecordLabelNewRow = "New Row";
        /// <summary>
        /// The data bound completed
        /// </summary>
        private bool _DataBoundCompleted = false;
        /// <summary>
        /// Gets or sets a value indicating whether [data bound completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data bound completed]; otherwise, <c>false</c>.
        /// </value>
        public bool DataBoundCompleted
        {
            get
            {
                return _DataBoundCompleted;
            }
            set
            {
                _DataBoundCompleted = value;
            }
        }


        /// <summary>
        /// The language
        /// </summary>
        private string _Language = "it-IT";
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language
        {
            get
            {
                return _Language;
            }
            set
            {
                _Language = value;
                SetLanguage(_Language);
            }
        }


        /// <summary>
        /// Gets or sets the record label separator.
        /// </summary>
        /// <value>
        /// The record label separator.
        /// </value>
        [Localizable(true)]
        public string RecordLabelSeparator
        {
            get
            {
                return _RecordLabelSeparator;
            }
            set
            {
                _RecordLabelSeparator = value;
                UpdateRecordLabel();
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [f key enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [f key enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool FKeyEnabled
        {
            get
            {
                return _FKeyEnabled;
            }
            set
            {
                _FKeyEnabled = value;
                UpdateButtonsCaption();
            }
        }


        /// <summary>
        /// Gets or sets the find key.
        /// </summary>
        /// <value>
        /// The find key.
        /// </value>
        public Keys FindKey
        {
            get
            {
                return _FindKey;
            }
            set
            {
                _FindKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the close f key.
        /// </summary>
        /// <value>
        /// The close f key.
        /// </value>
        public Keys CloseFKey
        {
            get
            {
                return _CloseFKey;
            }
            set
            {
                _CloseFKey = value;
            }
        }


        /// <summary>
        /// Gets or sets the undo f key.
        /// </summary>
        /// <value>
        /// The undo f key.
        /// </value>
        public Keys UndoFKey
        {
            get
            {
                return _UndoFKey;
            }
            set
            {
                _UndoFKey = value;
            }
        }


        /// <summary>
        /// Gets or sets the refresh f key.
        /// </summary>
        /// <value>
        /// The refresh f key.
        /// </value>
        public Keys RefreshFKey
        {
            get
            {
                return _RefreshFKey;
            }
            set
            {
                _RefreshFKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the save f key.
        /// </summary>
        /// <value>
        /// The save f key.
        /// </value>
        public Keys SaveFKey
        {
            get
            {
                return _SaveFKey;
            }
            set
            {
                _SaveFKey = value;
            }
        }



        /// <summary>
        /// Gets or sets the delete f key.
        /// </summary>
        /// <value>
        /// The delete f key.
        /// </value>
        public Keys DeleteFKey
        {
            get
            {
                return _DeleteFKey;
            }
            set
            {
                _DeleteFKey = value;
            }
        }


        /// <summary>
        /// Gets or sets the add new f key.
        /// </summary>
        /// <value>
        /// The add new f key.
        /// </value>
        public Keys AddNewFKey
        {
            get
            {
                return _AddNewFKey;
            }
            set
            {
                _AddNewFKey = value;
            }
        }


        /// <summary>
        /// Gets or sets the move previous f key.
        /// </summary>
        /// <value>
        /// The move previous f key.
        /// </value>
        public Keys MovePreviousFKey
        {
            get
            {
                return _MovePreviousFKey;
            }
            set
            {
                _MovePreviousFKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the move next f key.
        /// </summary>
        /// <value>
        /// The move next f key.
        /// </value>
        public Keys MoveNextFKey
        {
            get
            {
                return _MoveNextFKey;
            }
            set
            {
                _MoveNextFKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the move first f key.
        /// </summary>
        /// <value>
        /// The move first f key.
        /// </value>
        public Keys MoveFirstFKey
        {
            get
            {
                return _MoveFirstFKey;
            }
            set
            {
                _MoveFirstFKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the move last f key.
        /// </summary>
        /// <value>
        /// The move last f key.
        /// </value>
        public Keys MoveLastFKey
        {
            get
            {
                return _MoveLastFKey;
            }
            set
            {
                _MoveLastFKey = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only mode]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnlyMode
        {
            get
            {
                return _ReadOnlyMode;
            }
            set
            {
                _ReadOnlyMode = value;
                SetDataNavigator();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [compact mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [compact mode]; otherwise, <c>false</c>.
        /// </value>
        public bool CompactMode
        {
            get
            {
                return _CompactMode;
            }
            set
            {
                _CompactMode = value;

                if (_CompactMode == true)
                {

                    bClose.Text = "";
                    bClose.ToolTipText = _CloseCaption;
                    bDelete.Text = "";
                    bDelete.ToolTipText = _DeleteCaption;
                    bFind.Text = "";
                    bFind.ToolTipText = _FindCaption;
                    bFirst.Text = "";
                    bFirst.ToolTipText = _MoveFirstCaption;
                    bLast.Text = "";
                    bLast.ToolTipText = _MoveLastCaption;
                    bNew.Text = "";
                    bNew.ToolTipText = _AddNewCaption;
                    bPrev.Text = "";
                    bPrev.ToolTipText = _MovePreviousCaption;
                    bPrint.Text = "";
                    bPrint.ToolTipText = _PrintCaption;
                    bRefresh.Text = "";
                    bRefresh.ToolTipText = _RefreshCaption;
                    bSave.Text = "";
                    bSave.ToolTipText = _SaveCaption;
                    bUndo.Text = "";
                    bUndo.ToolTipText = _UndoCaption;
                    bNext.Text = "";
                    bNext.ToolTipText = _MoveNextCaption;
                }

                else
                {
                    bClose.Text = _CloseCaption;
                    bClose.ToolTipText = "";
                    bDelete.Text = _DeleteCaption;
                    bDelete.ToolTipText = "";
                    bFind.Text = _FindCaption;
                    bFind.ToolTipText = "";
                    bFirst.Text = _MoveFirstCaption;
                    bFirst.ToolTipText = "";
                    bLast.Text = _MoveLastCaption;
                    bLast.ToolTipText = "";
                    bNew.Text = _AddNewCaption;
                    bNew.ToolTipText = "";
                    bPrev.Text = _MovePreviousCaption;
                    bPrev.ToolTipText = "";
                    bPrint.Text = _PrintCaption;
                    bPrint.ToolTipText = "";
                    bRefresh.Text = _RefreshCaption;
                    bRefresh.ToolTipText = "";
                    bSave.Text = _SaveCaption;
                    bSave.ToolTipText = "";
                    bUndo.Text = _UndoCaption;
                    bUndo.ToolTipText = "";
                    bNext.Text = _MoveNextCaption;
                    bNext.ToolTipText = "";
                }

            }
        }

        /// <summary>
        /// Gets or sets the height of the data grid ListView default row.
        /// </summary>
        /// <value>
        /// The height of the data grid ListView default row.
        /// </value>
        public int DataGridListViewDefaultRowHeight
        {
            get
            {
                int DataGridListViewDefaultRowHeightRet = default;
                DataGridListViewDefaultRowHeightRet = _DataGridListViewDefaultRowHeight;
                return DataGridListViewDefaultRowHeightRet;
            }
            set
            {
                _DataGridListViewDefaultRowHeight = value;
                DataGridListView.DefaultRowHeight = _DataGridListViewDefaultRowHeight;
            }
        }

        /// <summary>
        /// Datas the binding mode.
        /// </summary>
        /// <returns></returns>
        public DataBindingMode DataBindingMode()
        {
            //return (DataBindingMode)Passero.Framework.ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "DataBindingMode");
            return _ActiveViewModel.DataBindingMode;
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
                if (_BindingSource != null)
                {
                    _CurrencyManager = _BindingSource.CurrencyManager;
                }
                return _BindingSource;
            }
            set
            {
                _BindingSource = value;
            }
        }

        /// <summary>
        /// Gets or sets the data panel.
        /// </summary>
        /// <value>
        /// The data panel.
        /// </value>
        public Panel DataPanel
        {
            get
            {
                Panel DataPanelRet = default;
                DataPanelRet = _DataPanel;
                return DataPanelRet;
            }
            set
            {
                _DataPanel = value;
            }
        }


        /// <summary>
        /// Gets or sets the data grid view.
        /// </summary>
        /// <value>
        /// The data grid view.
        /// </value>
        public DataGridView DataGridView
        {
            get
            {
                return _DataGridView;
            }
            set
            {
                _DataGridView = value;
                _DataGridView.Click -= DataGridView_Click;
                _DataGridView.Click += DataGridView_Click;
                //_DataGridView.RowStateChanged -= _DataGridView_RowStateChanged;
                //_DataGridView.RowStateChanged += _DataGridView_RowStateChanged;
                _DataGridView.SelectionChanged -= _DataGridView_SelectionChanged;
                _DataGridView.SelectionChanged += _DataGridView_SelectionChanged;


            }
        }

        /// <summary>
        /// Gets or sets the data repeater.
        /// </summary>
        /// <value>
        /// The data repeater.
        /// </value>
        public DataRepeater DataRepeater
        {
            get
            {
                return _DataRepeater;
            }
            set
            {
                _DataRepeater = value;
            }
        }

        /// <summary>
        /// Gets or sets the data grid ListView.
        /// </summary>
        /// <value>
        /// The data grid ListView.
        /// </value>
        public DataGridView DataGridListView
        {
            get
            {
                return _DataGridListView;
            }
            set
            {
                _DataGridListView = value;
                //_DataGridListView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //_DataGridListView.MultiSelect = false;
            }
        }

        
        /// <summary>
        /// The caption
        /// </summary>
        private string _Caption = "DataNavigator";
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        [Localizable(true)]
        public string Caption
        {
            get
            {

                return _Caption;

            }
            set
            {
                _Caption = value;
                Panel.Text = _Caption;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [record label visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [record label visible]; otherwise, <c>false</c>.
        /// </value>
        public bool RecordLabelVisible
        {
            get
            {
                return RecordLabel.Visible;
            }
            set
            {
                RecordLabel.Visible = value;
            }
        }



        /// <summary>
        /// Gets or sets a value indicating whether [navigation enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [navigation enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool NavigationEnabled
        {
            get
            {
                bool NavigationEnabledRet = default;
                NavigationEnabledRet = _NavigationEnabled;
                return NavigationEnabledRet;

            }
            set
            {
                _NavigationEnabled = value;
                switch (_NavigationEnabled)
                {
                    case var @case when @case == false:
                        {
                            DisableNavigation();
                            break;
                        }
                    case var case1 when case1 == true:
                        {
                            EnableNavigation();
                            break;
                        }
                }
            }
        }


        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {
            _InitDataNavigator(true);
        }

        /// <summary>
        /// Initializes the specified load data.
        /// </summary>
        /// <param name="LoadData">if set to <c>true</c> [load data].</param>
        public void Init(bool LoadData = false)
        {
            _InitDataNavigator(LoadData);
        }

        //public void InitDataNavigator(bool LoadData = false)
        //{
        //    _InitDataNavigator(LoadData);
        //}


        /// <summary>
        /// Initializes the data navigator.
        /// </summary>
        /// <param name="LoadData">if set to <c>true</c> [load data].</param>
        private void _InitDataNavigator(bool LoadData = false)
        {
            UpdateButtonsCaption();
            //ReflectionHelper.CallByName(this.ActiveViewModel, "SetBindingSource", CallType.Method);
            ActiveViewModel.SetBindingSource();

            if (LoadData)
            {
                //ReflectionHelper.CallByName(this.ActiveViewModel, "GetAllItems", CallType.Method);
                ActiveViewModel.GetAllItems();
                
                //_DbObject.Open(true);
            }

            UpdateRecordLabel();
            if (ModelItemsCount == 0)
            {
                SetButtonForEmptyModelItems();
            }
            else
            {
                EnableNavigation();
                EnableRefresh();
                if (ReadOnlyMode == false)
                {
                    SetButtonsForReadWrite();
                }
                else
                {
                    SetButtonsForReadOnly();
                }
            }

        }
        /// <summary>
        /// Datas the grid save.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataGrid_Save()
        {
            var ERContext = $"{mClassName}.DataGrid_Save()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            var argDataGridView = _DataGridView;
            ER = DataGrid_Update(ref argDataGridView);
            _DataGridView = argDataGridView;
            return ER;
        }

        /// <summary>
        /// Datas the repeater save.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Save()
        {
            var ERContext = $"{mClassName}.DataRepeater_Save()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            var argDataRepeater = _DataRepeater;
            ER = DataRepeater_Update(ref argDataRepeater);
            _DataRepeater = argDataRepeater;
            return ER;
        }


        /// <summary>
        /// Datas the grid save.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_Save(DataGridView DataGridView)
        {
            return DataGrid_Update(ref DataGridView);
        }

        /// <summary>
        /// Datas the repeater save.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Save(DataRepeater DataRepeater)
        {
            return DataRepeater_Update(ref DataRepeater);
        }

        /// <summary>
        /// Datas the grid update.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataGrid_Update()
        {
            var ERContext = $"{mClassName}.DataGrid_Update()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            var argDataGridView = _DataGridView;
            ER = DataGrid_Update(ref argDataGridView);
            _DataGridView = argDataGridView;
            return ER;

        }

        /// <summary>
        /// Datas the grid update.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_Update(ref Wisej.Web.DataGridView DataGridView)
        {
            var ERContext = $"{mClassName}.DataGrid_Update()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            int AffectedRecords = 0;
            int CurrentRowIndex = 0;
            int CurrentCellIndex = 0;
            if (DataGridView == null)
            {
                return ER;
            }
            if (DataGridView != null && DataGridView.DataSource != null && DataGridView.CurrentRow != null)
            {
                bool allowAdd = DataGridView.AllowUserToAddRows;
                CurrentRowIndex = DataGridView.CurrentRow.Index;
                CurrentCellIndex = DataGridView.CurrentCell.ColumnIndex;

                DataGridView.EndEdit();
                if (_ActiveViewModel != null)
                {


                    ModelItems = this.DataGridView.DataSource;


                    if (_AddNewState == true)
                    {
                        dynamic item = ((IList)_ModelItems)[NewItemIndex];
                        ER = (ExecutionResult)_ActiveViewModel.InsertItem(item);

                        if (ER.Success)
                        {
                            _AddNewState = false;
                        }
                    }
                    else
                    {
                        if (UseUpdateEx == false)
                            ER = (ExecutionResult)_ActiveViewModel.UpdateItems(_ModelItems);
                        else
                            ER = (ExecutionResult)_ActiveViewModel.UpdateItemsEx(_ModelItems);
                    }

                }
                try
                {
                    DataGridView.CurrentCell = DataGridView[CurrentCellIndex, CurrentRowIndex];
                }
                catch (Exception)
                {
                }
                _DataGridRow = DataGridView.CurrentRow.Index;
                DataGridView.AllowUserToAddRows = allowAdd;
            }
            if (_AddNewState == false)
            {
                foreach (Wisej.Web.DataGridViewRow _row in DataGridView.Rows)
                {
                    _row.ReadOnly = false;
                }
                SetDataNavigator();
            }
            else
            {
                SetButtonsForAddNew();
            }
            ER.Value = AffectedRecords;
            return ER;
        }

        /// <summary>
        /// Datas the repeater update.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Update()
        {
            var ERContext = $"{mClassName}.DataRepeater_Update()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            var argDataRepeater = _DataRepeater;
            ER = DataRepeater_Update(ref argDataRepeater);
            _DataRepeater = argDataRepeater;
            return ER;
        }

        /// <summary>
        /// Datas the repeater update.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Update(ref Wisej.Web.DataRepeater DataRepeater)
        {
            var ERContext = $"{mClassName}.DataRepeater_Update()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            int AffectedRecords = 0;
            int CurrentRowIndex = 0;
            if (DataRepeater == null)
            {
                ER.Value = AffectedRecords;
                return ER;
            }
            if (DataRepeater != null && DataRepeater.DataSource != null && DataRepeater.CurrentItem != null)
            {
                bool allowAdd = DataRepeater.AllowUserToAddItems;
                CurrentRowIndex = DataRepeater.CurrentItemIndex;

                if (_ActiveViewModel != null)
                {
                    ModelItems = this.DataRepeater.DataSource;

                    if (_AddNewState == true)
                    {
                        dynamic item = ((IList)_ModelItems)[NewItemIndex];
                        //ER = (ExecutionResult)ReflectionHelper.CallByName(this._ActiveViewModel, "InsertItem", Microsoft.VisualBasic.CallType.Method, item);
                        ER = (ExecutionResult)_ActiveViewModel.InsertItem(item);
                        if (ER.Success)
                        {
                            _AddNewState = false;
                        }
                    }
                    else
                    {
                        //ER = (ExecutionResult)ReflectionHelper.CallByName(this._ActiveViewModel, "UpdateItems", Microsoft.VisualBasic.CallType.Method, this._ModelItems);
                        ER = (ExecutionResult)_ActiveViewModel.UpdateItems(_ModelItems);
                    }
                }


                DataRepeater.AllowUserToAddItems = allowAdd;
            }
            if (_AddNewState == false)
            {
                SetDataNavigator();
            }
            else
            {
                SetButtonsForAddNew();
            }
            return ER;
        }


        /// <summary>
        /// Finds the first editable column.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
        private int FindFirstEditableColumn(DataGridView DataGridView)
        {

            for (int I = 0, loopTo = DataGridView.Columns.Count - 1; I <= loopTo; I++)
            {
                var c = DataGridView.Columns[I];
                if (c.Visible == true)
                {
                    if (c.ReadOnly == false)
                    {
                        return I;
                    }
                }
            }

            return -1;
        }
        /// <summary>
        /// Datas the grid enable row change.
        /// </summary>
        public void DataGrid_EnableRowChange()
        {
            _DataGridRow = -1;
        }

        /// <summary>
        /// Datas the grid add new.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="InsertAtCursor">if set to <c>true</c> [insert at cursor].</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_AddNew(object Item = null, bool InsertAtCursor = false)
        {
            var argDataGridView = _DataGridView;
            return DataGrid_AddNew(ref argDataGridView, Item, InsertAtCursor);

        }

        /// <summary>
        /// Datas the grid add new.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <param name="Item">The item.</param>
        /// <param name="InsertAtCurrentRow">if set to <c>true</c> [insert at current row].</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_AddNew(ref DataGridView DataGridView, object Item = null, bool InsertAtCurrentRow = false)
        {
            string ERContext = $"{mClassName}.DataGrid_AddNew()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            int ColumnIndex = 0;
            int RowIndex = 0;
            int GridRowIndex = 0;
            int NewRowIndex = -1;

            if (DataGridView == null)
            {
                ER.ResultMessage = "Null DataGridView";
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                return ER;
            }

            if (DataGridView.DataSource != null)
            {
                try
                {
                    var items = (IList)DataGridView.DataSource;
                    var T = ReflectionHelper.GetListType(items);
                    _AddNewState = true;
                    ColumnIndex = FindFirstEditableColumn(DataGridView);
                    if (Item == null)
                    {
                        Item = Activator.CreateInstance(T);
                    }

                    if (DataGridView.RowCount == 0)
                    {
                        items.Add(Item);
                        _AddNewState = true;
                        RowIndex = 0;
                        GridRowIndex = 0;
                        NewRowIndex = 0;
                        _NewItemIndex = 0;
                        _DataGridView.DataSource = null;
                        _DataGridView.DataSource = items;
                    }
                    else
                    {
                        GridRowIndex = DataGridView.CurrentRow.Index;
                        if (InsertAtCurrentRow == true)
                        {
                            // ------- INSERISCE NELLA POSIZIONE DEL CURSORE ----
                            // INSTANT C# NOTE: The following VB 'Select Case' included either a non-ordinal switch expression or non-ordinal, range-type, or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
                            //					 Select Case DataGridView.SortOrder
                            // ORIGINAL LINE: Case = SortOrder.None
                            if (DataGridView.SortOrder == SortOrder.None)
                            {
                                RowIndex = DataGridView.CurrentRow.Index;
                                items.Insert(RowIndex, Item);
                                NewRowIndex = RowIndex;
                            }
                            // ORIGINAL LINE: Case = SortOrder.Descending
                            else if (DataGridView.SortOrder == SortOrder.Descending)
                            {
                                RowIndex = DataGridView.Rows.Count - 1;
                                NewRowIndex = RowIndex;
                            }
                            // ORIGINAL LINE: Case = SortOrder.Ascending
                            else if (DataGridView.SortOrder == SortOrder.Ascending)
                            {
                                RowIndex = 0;
                                items.Insert(RowIndex, Item);
                                NewRowIndex = 0;
                            }
                        }
                        else
                        {

                            DataGridView.CurrentCell = DataGridView[ColumnIndex, DataGridView.Rows.Count - 1];
                            // ------- INSERISCE ALLA FINE O ALL'INIZIO-----------

                            // INSTANT C# NOTE: The following VB 'Select Case' included either a non-ordinal switch expression or non-ordinal, range-type, or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
                            //					 Select Case DataGridView.SortOrder
                            // ORIGINAL LINE: Case = SortOrder.Descending
                            if (DataGridView.SortOrder == SortOrder.Descending)
                            {
                                RowIndex = DataGridView.Rows.Count - 1;
                                items.Insert(RowIndex, Item);
                                NewRowIndex = RowIndex;
                            }
                            // ORIGINAL LINE: Case = SortOrder.Ascending
                            else if (DataGridView.SortOrder == SortOrder.Ascending)
                            {
                                RowIndex = 0;
                                NewRowIndex = RowIndex;
                                items.Insert(RowIndex, Item);
                            }
                            // ORIGINAL LINE: Case = SortOrder.None
                            else if (DataGridView.SortOrder == SortOrder.None)
                            {
                                RowIndex = items.Add(Item);
                                NewRowIndex = RowIndex;
                                //DataGridView.Rows.Insert(RowIndex, newrow);
                            }
                            // ----------------------------------------------------
                        }
                        ColumnIndex = DataGridView.CurrentCell.ColumnIndex;
                        _DataGridView.DataSource = null;
                        _DataGridView.DataSource = items;
                    }

                    foreach (var _row in DataGridView.Rows)
                    {
                        _row.ReadOnly = true;
                    }
                    DataGridView.Rows[RowIndex].ReadOnly = false;
                    DataGridView.CurrentCell = DataGridView[ColumnIndex, RowIndex];
                    _DataGridRow = DataGridView.CurrentCell.RowIndex;

                    _AddNewState = true;
                    _NewItemIndex = NewRowIndex;
                    //ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "AddNewState", true);
                    _ActiveViewModel.AddNewState = true;
                    UpdateRecordLabel();
                    SetButtonsForAddNew();

                }
                catch (Exception ex)
                {
                    ER.Exception = ex;
                    ER.ResultMessage = ex.Message;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 2;
                    _AddNewState = false;
                }
            }
            ER.Value = _DataGridRow;
            return ER;
        }

        /// <summary>
        /// Datas the grid delete.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataGrid_Delete()
        {
            return DataGrid_Delete(_DataGridView);

        }

        /// <summary>
        /// Datas the grid delete.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_Delete(DataGridView DataGridView)
        {
            string ERContext = $"{mClassName}.DataGrid_Delete()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            int RowIndex = 0;
            if (DataGridView == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "DataGridView is null!";
                ER.Value = 0;
                return ER; // Exit Function
            }

            if (DataGridView.DataSource != null)
            {
                //If Me._ManageChanges = True Then
                if (DataGridView.CurrentRow != null)
                {
                    RowIndex = DataGridView.CurrentRow.Index;
                    if (!DataGridView.CurrentRow.IsNewRow)
                    {
                        if (_ActiveViewModel != null)
                        {
                            object item = ((IList)_ModelItems)[RowIndex];
                            ER = (ExecutionResult)Microsoft.VisualBasic.Interaction.CallByName(_ActiveViewModel, "DeleteItem", Microsoft.VisualBasic.CallType.Method, item);
                            DataGridView.DataSource = null;
                            DataGridView.DataSource = _ModelItems;
                        }
                    }
                }

                int xRec = 1;
                if (RowIndex != 0)
                {
                    if (RowIndex < DataGridView.Rows.Count)
                    {
                        foreach (DataGridViewColumn Column in DataGridView.Columns)
                        {
                            if (Column.Visible)
                            {
                                DataGridView.CurrentCell = DataGridView.Rows[RowIndex].Cells[Column.Index];
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataGridViewColumn Column in DataGridView.Columns)
                        {
                            if (Column.Visible)
                            {
                                DataGridView.CurrentCell = DataGridView.Rows[RowIndex - 1].Cells[Column.Index];
                                break;
                            }
                        }
                    }
                }
                ER.Value = xRec;
            }
            else
            {
                SetDataNavigator();
                ER.Value = 0;
            }
            return ER;

        }



        /// <summary>
        /// Datas the repeater delete.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Delete()
        {
            return DataRepeater_Delete(_DataRepeater);
        }

        /// <summary>
        /// Datas the repeater delete.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Delete(DataRepeater DataRepeater)
        {

            string ERContext = $"{mClassName}.DataRepeater_Delete()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            int RowIndex = 0;
            if (DataRepeater == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "DataGridView is null!";
                ER.Value = 0;
                return ER; // Exit Function
            }
            if (DataRepeater.DataSource != null)
            {
                if (DataRepeater.CurrentItem != null)
                {
                    RowIndex = DataRepeater.CurrentItemIndex;
                    if (_AddNewState == false)
                    {
                        if (_ActiveViewModel != null)
                        {
                            object item = ((IList)_ModelItems)[RowIndex];

                            ER = (ExecutionResult)Microsoft.VisualBasic.Interaction.CallByName(_ActiveViewModel, "DeleteItem", Microsoft.VisualBasic.CallType.Method, item);
                            DataRepeater.Refresh();
                            if (RowIndex < DataRepeater.ItemCount)
                            {
                                DataRepeater.CurrentItemIndex = RowIndex;
                            }
                        }
                    }
                }

                int xRec = 1;
                if (RowIndex != 0)
                {
                    if (RowIndex < DataRepeater.ItemCount)
                    {
                        //For Each Column As DataGridViewColumn In DataGridView.Columns
                        //    If Column.Visible Then
                        //        DataGridView.CurrentCell = DataGridView.Rows(RowIndex).Cells(Column.Index)
                        //        Exit For
                        //    End If
                        //Next
                    }
                    else
                    {
                        //For Each Column As DataGridViewColumn In DataGridView.Columns
                        //    If Column.Visible Then
                        //        DataGridView.CurrentCell = DataGridView.Rows(RowIndex - 1).Cells(Column.Index)
                        //        Exit For
                        //    End If
                        //Next
                    }
                }
                ER.Value = xRec;

            }
            else
            {
                SetDataNavigator();
                ER.Value = 0;
            }
            return ER;
        }



        /// <summary>
        /// The active data navigator view model
        /// </summary>
        private DataNavigatorViewModel _ActiveDataNavigatorViewModel = null;

        /// <summary>
        /// Gets the active data navigator view model.
        /// </summary>
        /// <value>
        /// The active data navigator view model.
        /// </value>
        public DataNavigatorViewModel ActiveDataNavigatorViewModel
        {
            get
            {
                return _ActiveDataNavigatorViewModel;
            }

        }

        /// <summary>
        /// Datas the repeater move next.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        private ExecutionResult DataRepeater_MoveNext(bool IgnoreManageNavigation = false)
        {

            var ERContenxt = $"{mClassName}.DataRepeater_MoveNext()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            //if (!IgnoreManageNavigation && _ManageNavigation || _DataRepeater == null)
            //{
            //    return ER;
            //}


            if (DataGridActive)
            {
                if (_DataRepeater != null)
                {
                    //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveNextItem");
                    ER = (ExecutionResult)_ActiveViewModel.MoveNextItem();
                    _DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }

            ER.Context = ERContenxt;
            return ER;


        }


        /// <summary>
        /// Datas the repeater move last.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        private ExecutionResult DataRepeater_MoveLast(bool IgnoreManageNavigation = false)
        {
            var ERContenxt = $"{mClassName}.DataRepeater_MoveLast()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);
            //if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            //{
            //    return ER;
            //}

            if (DataGridActive)
            {
                if (_DataRepeater != null)
                {
                    //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveLastItem");
                    ER = (ExecutionResult)_ActiveViewModel.MoveLastItem();
                    _DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }
            ER.Context = ERContenxt;
            return ER;

        }


        /// <summary>
        /// Datas the repeater move previous.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        private ExecutionResult DataRepeater_MovePrevious(bool IgnoreManageNavigation = false)
        {
            var ERContenxt = $"{mClassName}.DataRepeater_MovePrevious()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            //if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            //{
            //    return ER;
            //}

            if (DataGridActive)
            {
                if (_DataRepeater != null)
                {
                    //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MovePreviousItem");
                    ER = (ExecutionResult)_ActiveViewModel.MovePreviousItem();
                    _DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }

            ER.Context = ERContenxt;
            return ER;


        }



        /// <summary>
        /// Datas the repeater move first.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        private ExecutionResult DataRepeater_MoveFirst(bool IgnoreManageNavigation = false)
        {
            var ERContenxt = $"{mClassName}.DataRepeater_MoveFirst()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            //if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            //{
            //    return ER;
            //}

            if (DataGridActive)
            {
                if (_DataRepeater != null)
                {
                    //Framework.ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveFirstItem");
                    _ActiveViewModel.MoveFirstItem();
                    _DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }

            ER.Context = ERContenxt;
            return ER;

        }




        /// <summary>
        /// Datas the repeater undo.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Undo()
        {
            return DataRepeater_Undo(_DataRepeater);
        }

        /// <summary>
        /// Datas the repeater undo.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <returns></returns>
        public ExecutionResult DataRepeater_Undo(DataRepeater DataRepeater)
        {
            var ERContext = $"{mClassName}.DataGrid_Undo()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (DataRepeater == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "DataRepeater is Null!";
                return ER;
            }

            if (DataRepeater.DataSource == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 2;
                ER.ResultMessage = "DataRepeater.DataSource is Null!";
                return ER;
            }

            try
            {
                if (DataRepeater.DataSource != null)
                {
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(_ActiveViewModel, "UndoChanges", Microsoft.VisualBasic.CallType.Method, true);
                    ER = (ExecutionResult)_ActiveViewModel.UndoChanges(true);

                    if (DataRepeater.DataSource is BindingSource bs)
                    {
                        bs.ResetBindings(false);
                    }
                    else
                    {
                        DataRepeater.DataSource = ModelItems;
                    }
                }

            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 3;
            }

            _AddNewState = false;
            SetDataNavigator();
            ER.Context = ERContext;
            return ER;

        }


        /// <summary>
        /// Datas the repeater add new.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="InsertAtCursor">if set to <c>true</c> [insert at cursor].</param>
        /// <returns></returns>
        public ExecutionResult DataRepeater_AddNew(object Item = null, bool InsertAtCursor = false)
        {

            var argDataRepeater = _DataRepeater;
            return DataRepeater_AddNew(ref argDataRepeater, Item, InsertAtCursor);

        }

        /// <summary>
        /// Datas the repeater add new.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <param name="NewItem">The new item.</param>
        /// <param name="InsertAtCurrentRow">if set to <c>true</c> [insert at current row].</param>
        /// <returns></returns>
        public ExecutionResult DataRepeater_AddNew(ref DataRepeater DataRepeater, object NewItem = null, bool InsertAtCurrentRow = false)
        {
            string ERContext = $"{mClassName}.DataRepeater_AddNew()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            int NewRowIndex = -1;
            if (ActiveViewModel == null)
            {
                ER.ResultMessage = "ActiveViewModel is null";
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                return ER;
            }

            if (DataRepeater == null)
            {
                ER.ResultMessage = "DataRepeater is null";
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 2;
                return ER;
            }

            if (DataRepeater.DataSource == null)
            {
                ER.ResultMessage = "DataRepeater.DataSource is null";
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 3;
                return ER;
            }

            if (DataRepeater.DataSource != null)
            {
                try
                {
                                      

                    IList items = (IList)DataRepeater.DataSource;

                    if (DataRepeater.DataSource is BindingSource bs)
                    {
                        // If it is a BindingSource, get its DataSource
                        if (bs.DataSource is IList)
                        {
                            items = (IList)bs.DataSource;
                        }
                    }
                    else if (DataRepeater.DataSource is IList)
                    {
                        items = (IList)DataRepeater.DataSource;
                    }

                    Type T = ReflectionHelper.GetListType(items);
                    _AddNewState = true;
                    if (NewItem == null)
                    {
                        NewItem = Activator.CreateInstance(T);
                    }
                    if (items.Count == 0)
                    {

                        items = ReflectionHelper.GetBindingListOfType(T);
                        DataRepeater.DataSource = items;
                        _AddNewState = true;
                        DataRepeater.AddNew();
                        NewRowIndex = 0;
                        items[0] = NewItem;
                        DataRepeater.CurrentItemIndex = NewRowIndex;
                        DataRepeater.Refresh();
                        DataRepeater.Select();
                        if (ReflectionHelper.IsBindingList(items))
                        {
                            items = (IList)ReflectionHelper.ConvertBindingListToList(items, T);
                        }
                        ModelItems = items;
                        ModelItem = NewItem;
                    }
                    else
                    {
                        if (InsertAtCurrentRow == true)
                        {

                        }
                        else
                        {
                            DataRepeater.AddNew();
                            items[DataRepeater.ItemCount - 1] = NewItem;
                            NewRowIndex = DataRepeater.ItemCount - 1;
                            DataRepeater.CurrentItemIndex = NewRowIndex;
                        }
                        DataRepeater.Refresh();

                    }
                    _AddNewState = true;
                    _NewItemIndex = NewRowIndex;
                    //ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "AddNewState", true);
                    _ActiveViewModel.AddNewState = true;
                    UpdateRecordLabel();
                    SetButtonsForAddNew();

                }
                catch (Exception ex)
                {
                    ER.ResultMessage = ex.Message;
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.Exception = ex;
                    ER.ErrorCode = 10;
                    _AddNewState = false;
                }
            }

            ER.Value = DataRepeater.CurrentItemIndex;
            return ER;

        }

        /// <summary>
        /// Datas the grid undo.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult DataGrid_Undo()
        {
            return DataGrid_Undo(_DataGridView);
        }
        /// <summary>
        /// Datas the grid undo.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_Undo(DataGridView DataGridView)
        {
            string ERContext = $"{mClassName}.DataGrid_Undo()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (DataGridView == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "DataGridView is Null!";
                return ER;
            }

            if (DataGridView.DataSource == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 2;
                ER.ResultMessage = "DataGridView.DataSource is Null!";
                return ER;
            }

            try
            {

                if (DataGridView.DataSource != null)
                {
                    DataGridView.CancelEdit();
                    DataGridView.DataSource = null;
                    //ER = (ExecutionResult)ReflectionHelper.CallByName(_ActiveViewModel, "UndoChanges", Microsoft.VisualBasic.CallType.Method, true);
                    ER = (ExecutionResult)_ActiveViewModel.UndoChanges(true);
                    DataGridView.DataSource = ModelItems;
                    //DataGridView.DataSource = this.ModelItemsShadow;
                    DataGridViewColumn c = Framework.ControlsUtilities.GetFirstVisibleColumnForDataGridView(DataGridView);
                    if (c != null)
                    {
                        DataGridView.SetCurrentCell(c.Index, CurrentModelItemIndex);
                    }
                }
                foreach (DataGridViewRow _row in DataGridView.Rows)
                {
                    _row.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {

                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 3;

            }


            _AddNewState = false;
            SetDataNavigator();
            ER.Context = ERContext;
            return ER;

        }



        /// <summary>
        /// Cancels the find.
        /// </summary>
        public void CancelFind()
        {
            _FindPending = false;
        }
        /// <summary>
        /// Cancels the print.
        /// </summary>
        public void CancelPrint()
        {
            _PrintPending = false;
        }
        /// <summary>
        /// Cancels the save.
        /// </summary>
        public void CancelSave()
        {
            _SavePending = false;
        }
        /// <summary>
        /// Cancels the add new.
        /// </summary>
        public void CancelAddNew()
        {
            // _AddNewPending = False
            //_DbObject.UndoChanges();
        }

        /// <summary>
        /// Cancels the undo.
        /// </summary>
        public void CancelUndo()
        {
            _UndoPending = false;

        }
        /// <summary>
        /// Cancels the delete.
        /// </summary>
        public void CancelDelete()
        {
            _DeletePending = false;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [undo pending].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [undo pending]; otherwise, <c>false</c>.
        /// </value>
        public bool UndoPending
        {
            get
            {
                bool UndoPendingRet = default;
                UndoPendingRet = _UndoPending;
                return UndoPendingRet;
            }
            set
            {
                _UndoPending = UndoPending;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [find pending].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [find pending]; otherwise, <c>false</c>.
        /// </value>
        public bool FindPending
        {
            get
            {
                bool FindPendingRet = default;
                FindPendingRet = _FindPending;
                return FindPendingRet;
            }
            set
            {
                _FindPending = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [print pending].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [print pending]; otherwise, <c>false</c>.
        /// </value>
        public bool PrintPending
        {
            get
            {
                bool PrintPendingRet = default;
                PrintPendingRet = _PrintPending;
                return PrintPendingRet;
            }
            set
            {
                _PrintPending = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [save pending].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [save pending]; otherwise, <c>false</c>.
        /// </value>
        public bool SavePending
        {
            get
            {
                bool SavePendingRet = default;
                SavePendingRet = _SavePending;
                return SavePendingRet;
            }
            set
            {
                _SavePending = value;
            }
        }

        /// <summary>
        /// Creates new itemindex.
        /// </summary>
        /// <value>
        /// The new index of the item.
        /// </value>
        public int NewItemIndex
        {
            get { return _NewItemIndex; }
            set { _NewItemIndex = value; }
        }
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

                return _AddNewState;
                //if (this._ViewModel != null)
                //    return (bool)Passero.Framework.ReflectionHelper.CallByName(this._ViewModel, "AddNewState", CallType.Get);
                //else
                //    return false;   
                //bool AddNewPendingRet = default;
                //AddNewPendingRet = false;
                //switch (DataGridActive)
                //{
                //    case false:
                //        {
                //            AddNewPendingRet = _AddNewState;
                //            break;
                //        }

                //    default:
                //        {
                //            if (DataGridView is not null)
                //            {
                //                AddNewPendingRet = _AddNewState;
                //            }

                //            break;
                //        }
                //}

                //return AddNewPendingRet;

            }

            set
            {
                _AddNewState = value;
            }

        }

        /// <summary>
        /// Gets or sets a value indicating whether [delete pending].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [delete pending]; otherwise, <c>false</c>.
        /// </value>
        public bool DeletePending
        {
            get
            {
                bool DeletePendingRet = default;
                DeletePendingRet = _DeletePending;
                return DeletePendingRet;
            }
            set
            {
                _DeletePending = value;
            }
        }

        /// <summary>
        /// Gets or sets the delete message.
        /// </summary>
        /// <value>
        /// The delete message.
        /// </value>
        [Localizable(true)]
        public string DeleteMessage
        {
            get
            {
                string DeleteMessageRet = default;
                DeleteMessageRet = _DeleteMessage;
                return DeleteMessageRet;

            }
            set
            {
                _DeleteMessage = value;

            }
        }


        /// <summary>
        /// Gets or sets the add new message.
        /// </summary>
        /// <value>
        /// The add new message.
        /// </value>
        [Localizable(true)]
        public string AddNewMessage
        {
            get
            {
                string AddNewMessageRet = default;
                AddNewMessageRet = _AddNewMessage;
                return AddNewMessageRet;

            }
            set
            {
                _AddNewMessage = value;

            }
        }

        /// <summary>
        /// Gets or sets the save message.
        /// </summary>
        /// <value>
        /// The save message.
        /// </value>
        [Localizable(true)]
        public string SaveMessage
        {
            get
            {
                string SaveMessageRet = default;
                SaveMessageRet = _SaveMessage;
                return SaveMessageRet;

            }
            set
            {
                _SaveMessage = value;

            }
        }
        /// <summary>
        /// Gets or sets the data set.
        /// </summary>
        /// <value>
        /// The data set.
        /// </value>
        public DataSet DataSet
        {
            get
            {
                DataSet DataSetRet = default;
                DataSetRet = _Dataset;
                return DataSetRet;

            }
            set
            {
                _Dataset = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [data grid active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data grid active]; otherwise, <c>false</c>.
        /// </value>
        public bool DataGridActive
        {
            get
            {
                return _DataGridActive;
            }
            set
            {
                if (_DataGridView == null)
                {
                    _DataGridActive = false;
                }
                else
                {
                    _DataGridActive = value;
                }
                UpdateRecordLabel();
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [data grid ListView active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data grid ListView active]; otherwise, <c>false</c>.
        /// </value>
        public bool DataGridListViewActive
        {
            get
            {
                bool DataGridListViewActiveRet = default;

                DataGridListViewActiveRet = _DataGridListViewActive;
                return DataGridListViewActiveRet;


            }
            set
            {


                _DataGridListViewActive = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [save visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [save visible]; otherwise, <c>false</c>.
        /// </value>
        public bool SaveVisible
        {
            get
            {
                bool SaveVisibleRet = default;
                SaveVisibleRet = bSave.Visible;
                return SaveVisibleRet;
            }
            set
            {
                bSave.Visible = value;

            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [save enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [save enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool SaveEnabled
        {
            get
            {
                bool SaveEnabledRet = default;
                SaveEnabledRet = bSave.Enabled;
                return SaveEnabledRet;
            }
            set
            {
                bSave.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [undo visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [undo visible]; otherwise, <c>false</c>.
        /// </value>
        public bool UndoVisible
        {
            get
            {
                bool UndoVisibleRet = default;
                UndoVisibleRet = bUndo.Visible;
                return UndoVisibleRet;
            }
            set
            {
                bUndo.Visible = value;

            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [undo enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [undo enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool UndoEnabled
        {
            get
            {
                bool UndoEnabledRet = default;
                UndoEnabledRet = bUndo.Enabled;
                return UndoEnabledRet;
            }
            set
            {
                bUndo.Enabled = value;
            }
        }




        /// <summary>
        /// Gets or sets a value indicating whether [find visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [find visible]; otherwise, <c>false</c>.
        /// </value>
        public bool FindVisible
        {
            get
            {
                bool FindVisibleRet = default;
                FindVisibleRet = bFind.Visible;
                return FindVisibleRet;
            }
            set
            {
                bFind.Visible = value;

            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [find enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [find enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool FindEnabled
        {
            get
            {
                bool FindEnabledRet = default;
                FindEnabledRet = bFind.Enabled;
                return FindEnabledRet;
            }
            set
            {
                bFind.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [delete enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [delete enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteEnabled
        {
            get
            {
                bool DeleteEnabledRet = default;
                DeleteEnabledRet = bDelete.Enabled;
                return DeleteEnabledRet;
            }
            set
            {
                bDelete.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [print visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [print visible]; otherwise, <c>false</c>.
        /// </value>
        public bool PrintVisible
        {
            get
            {
                bool PrintVisibleRet = default;
                PrintVisibleRet = bPrint.Visible;
                return PrintVisibleRet;
            }
            set
            {
                bPrint.Visible = value;

            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [print enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [print enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool PrintEnabled
        {
            get
            {
                bool PrintEnabledRet = default;
                PrintEnabledRet = bPrint.Enabled;
                return PrintEnabledRet;
            }
            set
            {
                bPrint.Enabled = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [add new visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add new visible]; otherwise, <c>false</c>.
        /// </value>
        public bool AddNewVisible
        {
            get
            {
                bool AddNewVisibleRet = default;
                AddNewVisibleRet = bNew.Visible;
                return AddNewVisibleRet;
            }
            set
            {
                bNew.Visible = value;

            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [close visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [close visible]; otherwise, <c>false</c>.
        /// </value>
        public bool CloseVisible
        {
            get
            {
                bool CloseVisibleRet = default;
                CloseVisibleRet = bClose.Visible;
                return CloseVisibleRet;
            }
            set
            {
                bClose.Visible = value;

            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [close enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [close enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CloseEnabled
        {
            get
            {
                bool CloseEnabledRet = default;
                CloseEnabledRet = bClose.Enabled;
                return CloseEnabledRet;
            }
            set
            {
                bClose.Enabled = value;

            }
        }
        /// <summary>
        /// Shows all buttons.
        /// </summary>
        public void ShowAllButtons()
        {
            CloseVisible = true;
            PrintVisible = true;
            DeleteVisible = true;
            RefreshVisible = true;
            FindVisible = true;
            SaveVisible = true;
            UndoVisible = true;
            AddNewVisible = true;
            NavigationVisible(true);
            RecordLabelVisible = true;

        }
        /// <summary>
        /// Hides all buttons.
        /// </summary>
        public void HideAllButtons()
        {
            CloseVisible = false;
            PrintVisible = false;
            DeleteVisible = false;
            RefreshVisible = false;
            FindVisible = false;
            SaveVisible = false;
            UndoVisible = false;
            AddNewVisible = false;
            NavigationVisible(false);
            RecordLabelVisible = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show header].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show header]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowHeader
        {
            get
            {
                return Panel.ShowHeader;
            }
            set
            {
                Panel.ShowHeader = value;
            }
        }
        /// <summary>
        /// Gets or sets the color of the header back.
        /// </summary>
        /// <value>
        /// The color of the header back.
        /// </value>
        public System.Drawing.Color HeaderBackColor
        {
            get
            {
                return Panel.HeaderBackColor;
            }
            set
            {
                Panel.HeaderBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the header alignment.
        /// </summary>
        /// <value>
        /// The header alignment.
        /// </value>
        public HorizontalAlignment HeaderAlignment
        {
            get
            {
                return Panel.HeaderAlignment;
            }
            set
            {
                Panel.HeaderAlignment = value;
            }
        }
        /// <summary>
        /// Gets or sets the color of the header fore.
        /// </summary>
        /// <value>
        /// The color of the header fore.
        /// </value>
        public System.Drawing.Color HeaderForeColor
        {
            get
            {
                return Panel.HeaderForeColor;
            }
            set
            {
                Panel.HeaderForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the header position.
        /// </summary>
        /// <value>
        /// The header position.
        /// </value>
        public HeaderPosition HeaderPosition
        {
            get
            {
                return Panel.HeaderPosition;
            }
            set
            {
                Panel.HeaderPosition = value;
            }
        }
        /// <summary>
        /// Gets or sets the size of the header.
        /// </summary>
        /// <value>
        /// The size of the header.
        /// </value>
        public int HeaderSize
        {
            get
            {
                return Panel.HeaderSize;
            }
            set
            {
                Panel.HeaderSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [delete visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [delete visible]; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteVisible
        {
            get
            {
                bool DeleteVisibleRet = default;
                DeleteVisibleRet = bDelete.Visible;
                return DeleteVisibleRet;
            }
            set
            {
                bDelete.Visible = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [add new enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add new enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AddNewEnabled
        {
            get
            {
                bool AddNewEnabledRet = default;
                AddNewEnabledRet = bNew.Enabled;
                return AddNewEnabledRet;
            }
            set
            {
                bNew.Enabled = value;


            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [manage navigation].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [manage navigation]; otherwise, <c>false</c>.
        /// </value>
        public bool ManageNavigation
        {
            get
            {
                bool ManageNavigationRet = default;
                ManageNavigationRet = _ManageNavigation;
                return ManageNavigationRet;
            }
            set
            {
                _ManageNavigation = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [manage changes].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [manage changes]; otherwise, <c>false</c>.
        /// </value>
        public bool ManageChanges
        {
            get
            {
                bool ManageChangesRet = default;
                ManageChangesRet = _ManageChanges;
                return ManageChangesRet;
            }
            set
            {
                _ManageChanges = value;
            }
        }

        //public BasicDAL.DbObject DbObject
        //{

        //    get
        //    {
        //        BasicDAL.DbObject DbObjectRet = default;
        //        DbObjectRet = _DbObject;
        //        return DbObjectRet;
        //    }

        //    set
        //    {
        //        _DbObject = value;

        //        try
        //        {
        //            SetDataNavigator();
        //            UpdateRecordLabel();
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether [delegate currency manager].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [delegate currency manager]; otherwise, <c>false</c>.
        /// </value>
        public bool DelegateCurrencyManager
        {
            get
            {
                bool DelegateCurrencyManagerRet = default;
                DelegateCurrencyManagerRet = _ManageNavigation;
                return DelegateCurrencyManagerRet;
            }
            set
            {
                _ManageNavigation = value;
            }
        }


        /// <summary>
        /// Gets or sets the add new caption.
        /// </summary>
        /// <value>
        /// The add new caption.
        /// </value>
        [Localizable(true)]
        public string AddNewCaption
        {
            get
            {
                string AddNewCaptionRet = default;
                AddNewCaptionRet = _AddNewCaption;
                return AddNewCaptionRet;
            }
            set
            {
                _AddNewCaption = value;
                bNew.Text = Conversions.ToString(ButtonCaption(value, _AddNewFKey));
            }
        }


        /// <summary>
        /// Gets or sets the find caption.
        /// </summary>
        /// <value>
        /// The find caption.
        /// </value>
        [Localizable(true)]
        public string FindCaption
        {
            get
            {
                string FindCaptionRet = default;
                FindCaptionRet = _FindCaption;
                return FindCaptionRet;

            }
            set
            {
                _FindCaption = value;
                bFind.Text = Conversions.ToString(ButtonCaption(value, _FindKey));
            }
        }
        /// <summary>
        /// Gets or sets the close caption.
        /// </summary>
        /// <value>
        /// The close caption.
        /// </value>
        [Localizable(true)]
        public string CloseCaption
        {
            get
            {
                string CloseCaptionRet = default;
                CloseCaptionRet = _CloseCaption;
                return CloseCaptionRet;

            }
            set
            {
                _CloseCaption = value;
                bClose.Text = Conversions.ToString(ButtonCaption(value, _CloseFKey));
            }
        }
        /// <summary>
        /// Gets or sets the print caption.
        /// </summary>
        /// <value>
        /// The print caption.
        /// </value>
        [Localizable(true)]
        public string PrintCaption
        {
            get
            {
                string PrintCaptionRet = default;
                PrintCaptionRet = _PrintCaption;
                return PrintCaptionRet;

            }
            set
            {
                _PrintCaption = value;
                bPrint.Text = Conversions.ToString(ButtonCaption(value, (Keys)_PrintFKey));
            }
        }


        /// <summary>
        /// Gets or sets the delete caption.
        /// </summary>
        /// <value>
        /// The delete caption.
        /// </value>
        [Localizable(true)]
        public string DeleteCaption
        {
            get
            {
                string DeleteCaptionRet = default;
                DeleteCaptionRet = _DeleteCaption;
                return DeleteCaptionRet;
            }
            set
            {
                _DeleteCaption = value;
                bDelete.Text = Conversions.ToString(ButtonCaption(value, _DeleteFKey));
            }

        }
        /// <summary>
        /// Gets or sets the save caption.
        /// </summary>
        /// <value>
        /// The save caption.
        /// </value>
        [Localizable(true)]
        public string SaveCaption
        {
            get
            {
                string SaveCaptionRet = default;
                SaveCaptionRet = _SaveCaption;
                return SaveCaptionRet;
            }
            set
            {
                _SaveCaption = value;
                bSave.Text = Conversions.ToString(ButtonCaption(value, _SaveFKey));
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [refresh enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [refresh enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshEnabled
        {
            get
            {
                bool RefreshEnabledRet = default;
                RefreshEnabledRet = bRefresh.Enabled;
                return RefreshEnabledRet;
            }
            set
            {
                bRefresh.Enabled = value;

            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [refresh visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [refresh visible]; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshVisible
        {
            get
            {
                bool RefreshVisibleRet = default;
                RefreshVisibleRet = bRefresh.Visible;
                return RefreshVisibleRet;
            }
            set
            {
                bRefresh.Visible = value;
            }
        }
        /// <summary>
        /// Gets or sets the refresh caption.
        /// </summary>
        /// <value>
        /// The refresh caption.
        /// </value>
        [Localizable(true)]
        public string RefreshCaption
        {
            get
            {
                string RefreshCaptionRet = default;
                RefreshCaptionRet = _RefreshCaption;
                return RefreshCaptionRet;

            }
            set
            {
                _RefreshCaption = value;
                bRefresh.Text = Conversions.ToString(ButtonCaption(value, _RefreshFKey));
            }
        }

        /// <summary>
        /// Gets or sets the undo caption.
        /// </summary>
        /// <value>
        /// The undo caption.
        /// </value>
        [Localizable(true)]
        public string UndoCaption
        {
            get
            {
                string UndoCaptionRet = default;
                UndoCaptionRet = _UndoCaption;
                return UndoCaptionRet;

            }
            set
            {
                _UndoCaption = value;
                bUndo.Text = Conversions.ToString(ButtonCaption(value, _UndoFKey));
            }
        }
        /// <summary>
        /// Gets or sets the move previous caption.
        /// </summary>
        /// <value>
        /// The move previous caption.
        /// </value>
        [Localizable(true)]
        public string MovePreviousCaption
        {
            get
            {
                string MovePreviousCaptionRet = default;
                MovePreviousCaptionRet = _MovePreviousCaption;
                return MovePreviousCaptionRet;
            }
            set
            {
                _MovePreviousCaption = value;
                bPrev.Text = Conversions.ToString(ButtonCaption(value, _MovePreviousFKey));
            }
        }
        /// <summary>
        /// Gets or sets the move next caption.
        /// </summary>
        /// <value>
        /// The move next caption.
        /// </value>
        [Localizable(true)]
        public string MoveNextCaption
        {
            get
            {
                string MoveNextCaptionRet = default;
                MoveNextCaptionRet = _MoveNextCaption;
                return MoveNextCaptionRet;
            }
            set
            {
                _MoveNextCaption = value;
                bNext.Text = Conversions.ToString(ButtonCaption(value, _MoveNextFKey));
            }
        }
        /// <summary>
        /// Gets or sets the move last caption.
        /// </summary>
        /// <value>
        /// The move last caption.
        /// </value>
        public string MoveLastCaption
        {
            get
            {
                string MoveLastCaptionRet = default;
                MoveLastCaptionRet = _MoveLastCaption;
                return MoveLastCaptionRet;
            }
            set
            {
                _MoveLastCaption = value;
                bLast.Text = Conversions.ToString(ButtonCaption(value, _MoveLastFKey));
            }
        }
        /// <summary>
        /// Gets or sets the move first caption.
        /// </summary>
        /// <value>
        /// The move first caption.
        /// </value>
        public string MoveFirstCaption
        {
            get
            {
                string MoveFirstCaptionRet = default;
                MoveFirstCaptionRet = _MoveFirstCaption;
                return MoveFirstCaptionRet;

            }
            set
            {
                _MoveFirstCaption = value;
                bFirst.Text = Conversions.ToString(ButtonCaption(value, _MoveLastFKey));
            }
        }

        /// <summary>
        /// Locks the parent form update.
        /// </summary>
        /// <param name="TrueFalse">if set to <c>true</c> [true false].</param>
        public void LockParentFormUpdate(bool TrueFalse)
        {
            // LockWindow(Me.ParentForm.Handle, TrueFalse)
        }

        // Public Sub HandleUserInput(ByVal sender As Object, ByVal e As Object)

        // Dim Key As KeyEventArgs = e

        // Select Case CInt(Key.KeyData)
        // Case _MovePreviousFKey
        // MovePrevious()
        // Case _MoveNextFKey
        // e.suppresskeypress = True
        // MoveNext()
        // Case _MoveFirstFKey
        // e.suppresskeypress = True
        // MoveFirst()
        // Case _MoveLastFKey
        // e.suppresskeypress = True
        // MoveLast()
        // Case _AddNewFKey
        // e.suppresskeypress = True
        // AddNew()
        // Case _DeleteFKey
        // e.suppresskeypress = True
        // Delete()
        // Case _SaveFKey
        // e.suppresskeypress = True
        // Save()
        // Case _RefreshFKey
        // e.suppresskeypress = True
        // RefreshData()
        // Case _PrintFKey
        // e.suppresskeypress = True

        // If Me.PrintEnabled = True And Me.PrintVisible = True Then
        // RaiseEvent ePrint()
        // End If

        // Case _UndoFKey
        // e.suppresskeypress = True
        // Undo()
        // Case _FindKey
        // e.suppressKeypress = True
        // If Me.FindEnabled = True And Me.FindVisible = True Then
        // RaiseEvent eFind()
        // End If

        // Case _CloseFKey
        // e.suppresskeypress = True
        // RaiseEvent eClose()
        // Case Else

        // End Select




        // End Sub
        /// <summary>
        /// Navigations the visible.
        /// </summary>
        /// <param name="Visible">if set to <c>true</c> [visible].</param>
        public void NavigationVisible(bool Visible)
        {
            bPrev.Visible = Visible;
            bFirst.Visible = Visible;
            bLast.Visible = Visible;
            bNext.Visible = Visible;
            RecordLabel.Visible = Visible;
        }
        /// <summary>
        /// Checks the data change.
        /// </summary>
        private void CheckDataChange()
        {
            //if (_ManageNavigation == true & _DbObject is not null)
            //{

            //    if (_DbObject.DataChanged)
            //    {
            //        Save();
            //    }
            //}
        }

        /// <summary>
        /// Deletes the specified override managed changes.
        /// </summary>
        /// <param name="OverrideManagedChanges">if set to <c>true</c> [override managed changes].</param>
        public void Delete(bool OverrideManagedChanges = false)
        {

            // Me._AddNewPending = False
            bool Cancel = false;
            _DataBoundCompleted = false;
            eDeleteRequest?.Invoke(ref Cancel);

            if (Cancel == true)
                return;
            if (DeleteVisible == false)
                return;

            if (OverrideManagedChanges == false && _ManageChanges == true)
            {
                if (MessageBox.Show(_DeleteMessage, ParentForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (_DataGridActive == true & _DataGridView is not null)
                    {
                        DataGrid_Delete();
                        eDeleteCompleted?.Invoke();
                    }
                    else
                    {
                        //Passero.Framework.ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "DeleteItem");
                        _ActiveViewModel.DeleteItem();
                        //int index = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this.ActiveViewModel, "CurrentModelItemIndex");
                        int index = ActiveViewModel.CurrentModelItemIndex;
                        MoveLast();
                        eDeleteCompleted?.Invoke();

                    }
                }
            }
            else
            {
                eDelete?.Invoke();
            }
            UpdateRecordLabel();

        }


        /// <summary>
        /// Saves the specified override managed changes.
        /// </summary>
        /// <param name="OverrideManagedChanges">if set to <c>true</c> [override managed changes].</param>
        public void Save(bool OverrideManagedChanges = false)
        {
            var ERContext = $"{mClassName}.DataNavigator.Save()";
            ExecutionResult ER = new ExecutionResult(ERContext);
            bool Cancel = false;
            if (eSaveRequest != null)
                eSaveRequest(ref Cancel);
            if (Cancel == true)
            {
                return;
            }
            if (SaveVisible == false || SaveEnabled == false)
            {
                return;
            }
            if (DataGridListView != null)
            {
                //DataGridListView.DataSource = _DbObject.DataTable
            }


            //this.ModelItems = this.DataGridView.DataSource;

            if (OverrideManagedChanges == false && _ManageChanges == true)
            {
                if (MessageBox.Show(_SaveMessage, ParentForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    ER = ViewModel_UdpateItem();
                }
            }
            else
            {
                SetButtonForSave();
                if (eSave != null)
                    eSave();
            }
            UpdateRecordLabel();
        }



        /// <summary>
        /// Checks the data panel.
        /// </summary>
        public void CheckDataPanel()
        {


            if (_DataPanel is not null)
            {
                //if (_DbObject.RowCount == 0)
                //{
                //    _DataPanel.Enabled = false;
                //    DisableNavigation();
                //    DisableSave();
                //    DisableDelete();
                //    DisableUndo();
                //    DisableRefresh();
                //}
                //else if (_DbObject.AddNewStatus == false)
                //{
                //    _DataPanel.Enabled = true;
                //    EnableNavigation();
                //    EnableDelete();
                //    EnableUndo();
                //    EnableSave();
                //    EnableRefresh();
                //    EnablePrint();
                //}

                //else
                //{
                //    _DataPanel.Enabled = true;
                //    DisableNavigation();
                //    DisableDelete();
                //    EnableUndo();
                //    EnableSave();
                //    DisableRefresh();
                //    DisablePrint();
                //    DisableFind();


                //}
            }
        }
        /// <summary>
        /// Handles the Click event of the bFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bFirst_Click(object sender, EventArgs e)
        {
            MoveFirst();
        }

        /// <summary>
        /// Handles the Click event of the bPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bPrev_Click(object sender, EventArgs e)
        {
            MovePrevious();
        }

        /// <summary>
        /// Handles the Click event of the bNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bNext_Click(object sender, EventArgs e)
        {
            MoveNext();
        }

        /// <summary>
        /// Handles the Click event of the bLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bLast_Click(object sender, EventArgs e)
        {

            MoveLast();

        }

        /// <summary>
        /// Handles the Click event of the bRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// Handles the Click event of the bNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void bNew_Click(object sender, EventArgs e)
        {
            AddNew();
        }

        /// <summary>
        /// Handles the Click event of the bDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        /// <summary>
        /// Handles the Click event of the bUndo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// Handles the Click event of the bSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Handles the Click event of the bFind control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bFind_Click(object sender, EventArgs e)
        {

            bool Cancel = false;
            eFindRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            eFind?.Invoke();

        }


        /// <summary>
        /// Handles the Click event of the bPrint control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bPrint_Click(object sender, EventArgs e)
        {
            bool Cancel = false;
            ePrintRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            ePrint?.Invoke();
        }

        /// <summary>
        /// Handles the Click event of the bClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bClose_Click(object sender, EventArgs e)
        {
            bool Cancel = false;
            eCloseRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            eClose?.Invoke();
        }

        /// <summary>
        /// Datas the grid row change.
        /// </summary>
        public void DataGrid_RowChange()
        {

            if (_DataGridRow != -1)
            {
                if (_DataGridView.CurrentRow.Index != _DataGridRow)
                {
                    int CurrentColumn = _DataGridView.CurrentCell.ColumnIndex;
                    int CurrentRow = _DataGridView.CurrentCell.RowIndex;

                    _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridRow].Cells[CurrentColumn];

                }
            }


        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataNavigator"/> class.
        /// </summary>
        public DataNavigator()
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();
            ToolBar.Height = 70;
            Panel.Dock = DockStyle.Fill;
            ToolBar.Dock = DockStyle.Fill;
            // Add any initialization after the InitializeComponent() call.



            CompactMode = false;
            bLast.Visible = true;
            bFirst.Visible = true;
            bNew.Visible = true;
            bNext.Visible = true;
            bPrev.Visible = true;
            bSave.Visible = true;
            bDelete.Visible = true;
            bUndo.Visible = true;
            bPrint.Visible = true;
            bRefresh.Visible = true;
            bFind.Visible = true;
            bClose.Visible = true;
            ToolBar = _ToolBar;
            _ToolBar.Name = "ToolBar";
            //HeaderSize = _HeaderSize;
            //ShowHeader = _ShowHeader;
        }

        /// <summary>
        /// Handles the Click event of the DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DataGridView_Click(object sender, EventArgs e)
        {
            if (_DataGridView.CurrentRow != null)
            {
                CurrentModelItemIndex = _DataGridView.CurrentRow.Index;
                UpdateRecordLabel();
            }
        }

        /// <summary>
        /// Adds the new.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="OverrideManagedChanges">if set to <c>true</c> [override managed changes].</param>
        /// <returns></returns>
        public ExecutionResult AddNew(object item = null, bool OverrideManagedChanges = false)
        {
            string ERContext = $"{mClassName}.AddNew()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (ActiveViewModel == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "ActiveViewModel is null.";
                return ER;
            }

            if (item == null)
            {
                try
                {
                    item = Activator.CreateInstance(ModelType);
                }
                catch (Exception)
                {
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 2;
                    ER.ResultMessage = "ModelType is null.";
                    return ER;
                }
            }

            bool Cancel = false;
            _DataBoundCompleted = false;

            if (eAddNewRequest != null)
                eAddNewRequest(ref Cancel);

            if (Cancel)
            {
                ER.ResultCode = ExecutionResultCodes.Warning;
                ER.ErrorCode = 3;
                ER.ResultMessage = "Operation Cancelled by User.";
                return ER;
            }

            //int index = Convert.ToInt32(Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "CurrentModelItemIndex", Microsoft.VisualBasic.CallType.Get));
            int index = _ActiveViewModel.CurrentModelItemIndex;
            int newIndex = index;

            if (!OverrideManagedChanges && _ManageChanges)
            {
                ER = ViewModel_AddNew(item);
                SetButtonsForAddNew();
            }
            else
            {
                _AddNewState = true;
                //ReflectionHelper.CallByName(this._ActiveViewModel, "AddNewCurrentModelItemIndex", Microsoft.VisualBasic.CallType.Set, index);
                _ActiveViewModel.AddNewCurrentModelItemIndex = index;
                SetButtonsForAddNew();
                if (eAddNew != null)
                    eAddNew();
            }
            UpdateRecordLabel();
            Cancel = false;
            if (eAfterAddNewRequest != null)
                eAfterAddNewRequest(ref Cancel);


            return null;
        }

        /// <summary>
        /// Updates the record label.
        /// </summary>
        public void UpdateRecordLabel()
        {
            if (ActiveViewModel == null)
            {
                return;
            }
            var itemscount = 0;
            //int index = (int)ReflectionHelper.GetPropertyValue(ActiveViewModel, "CurrentModelItemIndex");
            //itemscount = (int)ReflectionHelper.GetPropertyValue(ActiveViewModel, "ModelItemsCount");
            int index = ActiveViewModel.CurrentModelItemIndex;
            itemscount = ActiveViewModel.ModelItemsCount;
            RecordLabel.Text = string.Format(RecordLabelHtmlFormat, index + 1, RecordLabelSeparator, itemscount);
            if (itemscount == 0 && AddNewState == false)
            {
                SetButtonForEmptyModelItems();
            }

        }



        /// <summary>
        /// Moves the first.
        /// </summary>
        /// <param name="OverrideManagedNavigation">if set to <c>true</c> [override managed navigation].</param>
        /// <returns></returns>
        public ExecutionResult MoveFirst(bool OverrideManagedNavigation = false)
        {
            var ERContenxt = $"{mClassName}.MoveFirst()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);
            bool Cancel = false;

            _DataBoundCompleted = false;
            if (eMoveFirstRequest != null)
                eMoveFirstRequest(ref Cancel);
            if (Cancel)
            {
                return ER;
            }
            if (!OverrideManagedNavigation && _ManageNavigation)
            {
                ViewModel_MoveFirstItem();
            }
            else
            {
                if (eMoveFirst != null)
                    eMoveFirst();
            }
            RaiseEventBoundCompleted();
            MoveFirstDataGridListView();
            UpdateRecordLabel();
            ER.Context = ERContenxt;
            return ER;

        }


        /// <summary>
        /// Moves the first data grid ListView.
        /// </summary>
        private void MoveFirstDataGridListView()
        {
            if (_DataGridListView is not null)
            {
                _DataGridListView.CurrentCell = _DataGridListView.Rows[0].Cells[_DataGridListView.CurrentCell.ColumnIndex];
            }

        }


        /// <summary>
        /// Datas the grid move previous.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_MovePrevious(bool IgnoreManageNavigation = false)
        {
            var ERContenxt = $"{mClassName}.DataGrid_MovePrevious()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            if (_DataGridActive == true && _DataGridView != null && _DataGridView.CurrentRow != null)
            {
                if (_DataGridView.CurrentRow.Index > 0)
                {
                    _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index - 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                    //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MovePreviousItem");
                    ER = (ExecutionResult)_ActiveViewModel.MovePreviousItem();
                }
            }

            ER.Context = ERContenxt;
            return ER;

        }




        /// <summary>
        /// Moves the previous data grid ListView.
        /// </summary>
        private void MovePreviousDataGridListView()
        {
            if (_DataGridListView is not null)
            {
                if (_DataGridListView.CurrentRow.Index > 0)
                {
                    _DataGridListView.CurrentCell = _DataGridListView.Rows[_DataGridListView.CurrentRow.Index - 1].Cells[_DataGridListView.CurrentCell.ColumnIndex];
                }
            }
        }

        /// <summary>
        /// Itemses the move previous.
        /// </summary>
        public void ItemsMovePrevious()
        {
            if (_DataGridActive == true & _DataGridView is not null)
            {
                if (_DataGridView.CurrentRow.Index > 0)
                {
                    _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index - 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                }
            }
            else
            {
                //Passero.Framework.ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MovePreviousItem");
                _ActiveViewModel.MovePreviousItem();
                MovePreviousDataGridListView();
            }
            UpdateRecordLabel();
        }

        /// <summary>
        /// Updates the grid position.
        /// </summary>
        private void UpdateGridPosition()
        {
            if (_DataGridActive)
            {
                switch (_ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridView != null)
                        {
                            if (CurrentModelItemIndex > -1)
                            {
                                _DataGridView.CurrentCell = _DataGridView.Rows[CurrentModelItemIndex].Cells[_DataGridView.CurrentCell.ColumnIndex];
                            }

                        }
                        break;
                    case ViewModelGridModes.DataRepeater:
                        if (_DataRepeater != null)
                        {
                            if (CurrentModelItemIndex > -1)
                            {
                                _DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                            }
                        }
                        break;
                    default:
                        break;

                }
            }

        }


        /// <summary>
        /// Moves the previous.
        /// </summary>
        /// <param name="OverrideManagedNavigation">if set to <c>true</c> [override managed navigation].</param>
        /// <returns></returns>
        public ExecutionResult MovePrevious(bool OverrideManagedNavigation = false)
        {
            var ERContenxt = $"{mClassName}.MovePrevious()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMovePreviousRequest != null)
                eMovePreviousRequest(ref Cancel);
            if (Cancel == true)
            {
                return ER;
            }
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                ViewModel_MovePreviousItem();
            }
            else
            {
                if (eMovePrevious != null)
                    eMovePrevious();
            }
            RaiseEventBoundCompleted();
            MovePreviousDataGridListView();
            UpdateRecordLabel();

            ER.Context = ERContenxt;
            return ER;

        }

        /// <summary>
        /// Datas the grid move next.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_MoveNext(bool IgnoreManageNavigation = false)
        {
            string ERContenxt = $"{mClassName}.DataGrid_MoveNext()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            //if (!IgnoreManageNavigation && _ManageNavigation || _DataGridView == null)
            //{
            //    return ER;
            //}

            if (DataGridActive)
            {
                if (_DataGridView != null && _DataGridView.CurrentRow != null)
                {
                    if (_DataGridView.CurrentRow.Index < _DataGridView.Rows.Count - 1)
                    {
                        _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index + 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                        //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveNextItem");
                        ER = _ActiveViewModel.MoveNextItem();
                    }
                }
            }

            ER.Context = ERContenxt;
            return ER;

        }


        /// <summary>
        /// Moves the next data grid ListView.
        /// </summary>
        private void MoveNextDataGridListView()
        {
            if (_DataGridListView is not null)
            {
                if (_DataGridListView.CurrentRow.Index < _DataGridListView.Rows.Count - 1)
                {
                    _DataGridListView.CurrentCell = _DataGridListView.Rows[_DataGridListView.CurrentRow.Index + 1].Cells[_DataGridListView.CurrentCell.ColumnIndex];
                }
            }
        }

        /// <summary>
        /// Moves at item data grid ListView.
        /// </summary>
        /// <param name="ItemIndex">Index of the item.</param>
        private void MoveAtItemDataGridListView(int ItemIndex)
        {
            if (_DataGridListView is not null)
            {
                if (ItemIndex < _DataGridListView.Rows.Count - 1)
                {
                    _DataGridListView.CurrentCell = _DataGridListView.Rows[ItemIndex].Cells[_DataGridListView.CurrentCell.ColumnIndex];
                }
            }
        }

        /// <summary>
        /// Moves at item.
        /// </summary>
        /// <param name="ItemIndex">Index of the item.</param>
        /// <param name="OverrideManagedNavigation">if set to <c>true</c> [override managed navigation].</param>
        /// <returns></returns>
        private ExecutionResult MoveAtItem(int ItemIndex, bool OverrideManagedNavigation = false)
        {
            var ERContenxt = $"{mClassName}.MoveAtItem()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMoveAtItemRequest != null)
                eMoveAtItemRequest(ref Cancel);
            if (Cancel == true)
            {
                return ER;
            }
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                if (_DataGridActive == true && _DataGridView != null)
                {
                    if (_DataGridView.CurrentRow.Index < _DataGridView.Rows.Count - 1)
                    {
                        _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index + 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                    }
                }
                else
                {
                    //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveAtItem", ItemIndex);
                    ER = _ActiveViewModel.MoveAtItem();
                    MoveAtItemDataGridListView(ItemIndex);
                    if (eUndoCompleted != null)
                        eUndoCompleted();
                }
            }
            else
            {
                MoveNextDataGridListView();
                if (eMoveAtItem != null)
                    eMoveAtItem();
            }
            UpdateRecordLabel();

            ER.Context = ERContenxt;
            return ER;

        }


        /// <summary>
        /// Moves the next.
        /// </summary>
        /// <param name="OverrideManagedNavigation">if set to <c>true</c> [override managed navigation].</param>
        /// <returns></returns>
        public ExecutionResult MoveNext(bool OverrideManagedNavigation = false)
        {
            var ERContenxt = $"{mClassName}.MoveNext()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMoveNextRequest != null)
                eMoveNextRequest(ref Cancel);
            if (Cancel == true)
            {
                return ER;
            }
            
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                ViewModel_MoveNextItem();
            }
            else
            {
                if (eMoveNext != null)
                    eMoveNext();
            }
            RaiseEventBoundCompleted();
            MoveNextDataGridListView();
            UpdateRecordLabel();
                        ER.Context = ERContenxt;
            return ER;

        }



        /// <summary>
        /// Datas the grid move first.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_MoveFirst(bool IgnoreManageNavigation = false)
        {
            var ERContenxt = $"{mClassName}.DataGrid_MoveFirst()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            //if (IgnoreManageNavigation == false && _ManageNavigation == false || _DataGridView == null)
            //{
            //    return ER;
            //}
            if (_DataGridActive && _DataGridView != null && _DataGridView.CurrentRow != null)
            {
                _DataGridView.CurrentCell = _DataGridView.Rows[0].Cells[_DataGridView.CurrentCell.ColumnIndex];
                //ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveFirstItem");
                _ActiveViewModel.MoveFirstItem();
            }
            ER.Context = ERContenxt;
            return ER;

        }

        /// <summary>
        /// Datas the grid move last.
        /// </summary>
        /// <param name="IgnoreManageNavigation">if set to <c>true</c> [ignore manage navigation].</param>
        /// <returns></returns>
        public ExecutionResult DataGrid_MoveLast(bool IgnoreManageNavigation = false)
        {
            var ERContenxt = $"{mClassName}.DataGrid_MoveLast()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);
            //if (IgnoreManageNavigation == false && _ManageNavigation == false || _DataGridView == null)
            //{
            //    return ER;
            //}
            if (_DataGridActive == true && _DataGridView != null && _DataGridView.CurrentRow != null)
            {
                _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.Rows.Count - 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                //ER = (ExecutionResult)ReflectionHelper.InvokeMethodByName(ref _ActiveViewModel, "MoveAtItem", _DataGridView.CurrentRow.Index);
                ER = _ActiveViewModel.MoveAtItem(_DataGridView.CurrentRow.Index);
            }
            ER.Context = ERContenxt;
            return ER;

        }




        /// <summary>
        /// Moves the last data grid ListView.
        /// </summary>
        private void MoveLastDataGridListView()
        {
            if (_DataGridListView is not null)
            {
                _DataGridListView.CurrentCell = _DataGridListView.Rows[_DataGridListView.RowCount - 1].Cells[_DataGridListView.CurrentCell.ColumnIndex];
            }
        }

        /// <summary>
        /// Moves the last.
        /// </summary>
        /// <param name="OverrideManagedNavigation">if set to <c>true</c> [override managed navigation].</param>
        /// <returns></returns>
        public ExecutionResult MoveLast(bool OverrideManagedNavigation = false)
        {
            var ERContenxt = $"{mClassName}.MoveLast()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMoveLastRequest != null)
                eMoveLastRequest(ref Cancel);
            if (Cancel == true)
            {
                return ER;
            }
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                ViewModel_MoveLastItem();

            }
            else
            {

                if (eMoveLast != null)
                    eMoveLast();
            }
            RaiseEventBoundCompleted();
            MoveLastDataGridListView();
            UpdateRecordLabel();
            ER.Context = ERContenxt;
            return ER;
        }




        /// <summary>
        /// Refreshes the data.
        /// </summary>
        /// <returns></returns>
        private ExecutionResult RefreshData()
        {

            var ERContenxt = $"{mClassName}.RefreshData()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eRefreshRequest != null)
                eRefreshRequest(ref Cancel);
            if (Cancel)
            {
                return ER;
            }
            if (!RefreshVisible || !RefreshEnabled)
            {
                return ER;
            }
            CheckDataChange();


            if (_ManageNavigation)
            {
                switch (_ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridView != null && DataGridActive)
                        {

                        }
                        break;
                    case ViewModelGridModes.DataRepeater:
                        if (_DataRepeater != null && DataGridActive)
                        {

                        }
                        break;
                    default:
                        //ER = (ExecutionResult)ReflectionHelper.CallByName(this.ActiveViewModel, "ReloadItems", Microsoft.VisualBasic.CallType.Method);
                        ER = (ExecutionResult)ActiveViewModel.ReloadItems();
                        break;
                }


                if (eRefreshCompleted != null)
                    eRefreshCompleted();
            }
            else
            {
                if (eRefresh != null)
                    eRefresh();
            }
            UpdateRecordLabel();
            ER.Context = ERContenxt;
            return ER;
        }






        /// <summary>
        /// Undoes the specified override managed changes.
        /// </summary>
        /// <param name="OverrideManagedChanges">if set to <c>true</c> [override managed changes].</param>
        /// <returns></returns>
        public ExecutionResult Undo(bool OverrideManagedChanges = false)
        {
            var ERContenxt = $"{mClassName}.Undo()";
            ExecutionResult ER = new ExecutionResult(ERContenxt);

            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eUndoRequest != null)
                eUndoRequest(ref Cancel);
            if (Cancel)
            {
                return ER;
            }
            SetButtonForUndo();
            if (DataGridListView != null)
            {
                // DataGridListView.DataSource = _DbObject.DataTable
            }
            if (!OverrideManagedChanges && _ManageChanges)
            {

                ER = ViewModel_UndoChanges();

            }
            else
            {
                if (eUndo != null)
                    eUndo();
            }
            _AddNewState = false;
            UpdateRecordLabel();


            ER.Context = ERContenxt;
            return ER;

        }


        /// <summary>
        /// Adds the view model.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="ViewModel">The view model.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="DataGridView">The data grid view.</param>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <returns></returns>
        public bool AddViewModel(string Name, object ViewModel, string FriendlyName = "DataNavigator", DataGridView DataGridView = null, DataRepeater DataRepeater = null)
        {

            DataNavigatorViewModel DataNavigatorViewModel = null;
            try
            {
                DataNavigatorViewModel = new DataNavigatorViewModel(ViewModel, Name, FriendlyName, DataGridView, DataRepeater);
                ViewModels[Name] = DataNavigatorViewModel;
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        /// <summary>
        /// Buttons the caption.
        /// </summary>
        /// <param name="ButtonText">The button text.</param>
        /// <param name="FKey">The f key.</param>
        /// <returns></returns>
        private object ButtonCaption(string ButtonText, Keys FKey)
        {
            if (_FKeyEnabled)
            {


                string _f = FKey.ToString().Replace(",", "+");
                _f = _f.Replace(" ", "");
                // Return "<p style='text-align:center;font-size:70%;'>" + _f + "</p><br><p style='font-size:100%;'>" + ButtonText + "</p>"
                return "<p style='margin-top:0px;line-height:1.0;text-align:center;'><b>" + ButtonText + "<b><br><small>" + _f + "</small></p>";
            }
            else
            {
                return ButtonText;
            }
        }
        /// <summary>
        /// Handles the user input.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public void HandleUserInput(object sender, object e)
        {

            if (_FKeyEnabled == false)
            {
                return;
            }

            KeyEventArgs Key = (KeyEventArgs)e;

            switch ((int)Key.KeyData)
            {
                case var @case when @case == (int)_MovePreviousFKey:
                    {
                        MovePrevious();
                        break;
                    }
                case var case1 when case1 == (int)_MoveNextFKey:
                    {
                        MoveNext();
                        break;
                    }
                case var case2 when case2 == (int)_MoveFirstFKey:
                    {

                        MoveFirst();
                        break;
                    }
                case var case3 when case3 == (int)_MoveLastFKey:
                    {

                        MoveLast();
                        break;
                    }
                case var case4 when case4 == (int)_AddNewFKey:
                    {

                        AddNew();
                        break;
                    }
                case var case5 when case5 == (int)_DeleteFKey:
                    {

                        Delete();
                        break;
                    }
                case var case6 when case6 == (int)_SaveFKey:
                    {

                        Save();
                        break;
                    }
                case var case7 when case7 == (int)_RefreshFKey:
                    {

                        RefreshData();
                        break;
                    }

                case var case8 when case8 == _PrintFKey:
                    {

                        if (PrintEnabled == true & PrintVisible == true)
                        {
                            ePrint?.Invoke();
                        }

                        break;
                    }

                case var case9 when case9 == (int)_UndoFKey:
                    {

                        Undo();
                        break;
                    }
                case var case10 when case10 == (int)_FindKey:
                    {

                        if (FindEnabled == true & FindVisible == true)
                        {
                            eFind?.Invoke();
                        }

                        break;
                    }

                case var case11 when case11 == (int)_CloseFKey:
                    {

                        eClose?.Invoke();
                        break;
                    }

                default:
                    {
                        break;
                    }

            }
        }

        /// <summary>
        /// Sets the button for empty model items.
        /// </summary>
        public void SetButtonForEmptyModelItems()
        {
            DisableNavigation();
            bFind.Enabled = true;
            bRefresh.Enabled = false;
            bNew.Enabled = true;
            bClose.Enabled = true;
            bDelete.Enabled = false;
            bSave.Enabled = false;
            bUndo.Enabled = false;
            switch (DataGridActive)
            {
                case false:
                    break;
                case true:
                    break;
            }
        }

        /// <summary>
        /// Sets the buttons for read write.
        /// </summary>
        public void SetButtonsForReadWrite()
        {
            bUndo.Enabled = true;
            bSave.Enabled = true;
            bNew.Enabled = true;
            bDelete.Enabled = true;
        }

        /// <summary>
        /// Sets the buttons for read only.
        /// </summary>
        public void SetButtonsForReadOnly()
        {
            bUndo.Enabled = false;
            bSave.Enabled = false;
            bNew.Enabled = false;
            bDelete.Enabled = false;
        }
        /// <summary>
        /// Sets the buttons for add new.
        /// </summary>
        public void SetButtonsForAddNew()
        {

            DisableNavigation();

            bUndo.Enabled = true;
            bSave.Enabled = true;
            bFind.Enabled = false;
            bPrint.Enabled = false;
            bRefresh.Enabled = false;
            bPrint.Enabled = false;
            bNew.Enabled = false;
            bClose.Enabled = false;
            bDelete.Enabled = false;

            switch (DataGridActive)
            {
                case false:
                    {
                        break;
                    }
                case true:
                    {
                        break;
                    }
            }
            _AddNewState = true;
        }

        /// <summary>
        /// Datas the grid start edit.
        /// </summary>
        /// <param name="CurrentRow">The current row.</param>
        public void DataGrid_StartEdit(int CurrentRow)
        {

            _DataGridRow = CurrentRow;
            SetButtonForEdit();
        }

        /// <summary>
        /// Datas the grid start edit.
        /// </summary>
        public void DataGrid_StartEdit()
        {
            if (_DataGridView is not null)
            {

                SetButtonForEdit();
            }

        }

        /// <summary>
        /// Sets the button for edit.
        /// </summary>
        public void SetButtonForEdit()
        {

            DisableNavigation();

            bFind.Enabled = false;
            bPrint.Enabled = false;
            bRefresh.Enabled = false;
            bPrint.Enabled = false;
            bNew.Enabled = false;
            bClose.Enabled = false;
            bDelete.Enabled = false;

            switch (DataGridActive)
            {
                case false:
                    {
                        break;
                    }
                case true:
                    {
                        break;
                    }
            }

        }
        /// <summary>
        /// Sets the button for save.
        /// </summary>
        public void SetButtonForSave()
        {


            EnableAllButtons();
            switch (DataGridActive)
            {
                case false:
                    {
                        break;
                    }
                case true:
                    {
                        break;
                    }
            }

        }
        /// <summary>
        /// Sets the button for undo.
        /// </summary>
        private void SetButtonForUndo()
        {

            EnableAllButtons();

            switch (DataGridActive)
            {
                case false:
                    {
                        break;
                    }
                case true:
                    {
                        break;
                    }
            }

        }

        /// <summary>
        /// Disables the navigation.
        /// </summary>
        public void DisableNavigation()
        {
            bFirst.Enabled = false;
            bNext.Enabled = false;
            bPrev.Enabled = false;
            bLast.Enabled = false;
            _NavigationEnabled = false;
        }
        /// <summary>
        /// Enables the navigation.
        /// </summary>
        public void EnableNavigation()
        {
            bFirst.Enabled = true;
            bNext.Enabled = true;
            bPrev.Enabled = true;
            bLast.Enabled = true;
            _NavigationEnabled = true;
        }

        /// <summary>
        /// Disables the refresh.
        /// </summary>
        public void DisableRefresh()
        {
            bRefresh.Enabled = false;

        }
        /// <summary>
        /// Enables the refresh.
        /// </summary>
        public void EnableRefresh()
        {
            bRefresh.Enabled = true;
        }
        /// <summary>
        /// Disables the delete.
        /// </summary>
        public void DisableDelete()
        {
            bDelete.Enabled = false;
        }

        /// <summary>
        /// Disables the new.
        /// </summary>
        public void DisableNew()
        {
            bNew.Enabled = false;
        }

        /// <summary>
        /// Enables the delete.
        /// </summary>
        public void EnableDelete()
        {
            bDelete.Enabled = true;
        }

        /// <summary>
        /// Disables the save.
        /// </summary>
        public void DisableSave()
        {
            bSave.Enabled = false;
        }
        /// <summary>
        /// Disables the undo.
        /// </summary>
        public void DisableUndo()
        {
            bUndo.Enabled = false;
        }
        /// <summary>
        /// Enables the undo.
        /// </summary>
        public void EnableUndo()
        {
            bUndo.Enabled = true;
        }

        /// <summary>
        /// Enables the save.
        /// </summary>
        public void EnableSave()
        {
            bSave.Enabled = true;
        }
        /// <summary>
        /// Disables the find.
        /// </summary>
        public void DisableFind()
        {
            bFind.Enabled = false;
        }
        /// <summary>
        /// Enables the find.
        /// </summary>
        public void EnableFind()
        {
            bFind.Enabled = true;
        }

        /// <summary>
        /// Disables the print.
        /// </summary>
        public void DisablePrint()
        {
            bPrint.Enabled = false;
        }
        /// <summary>
        /// Enables the print.
        /// </summary>
        public void EnablePrint()
        {
            bPrint.Enabled = true;
        }

        /// <summary>
        /// Enables all buttons.
        /// </summary>
        public void EnableAllButtons()
        {

            bFirst.Enabled = true;
            bNext.Enabled = true;
            bPrev.Enabled = true;
            bLast.Enabled = true;
            bClose.Enabled = true;
            bDelete.Enabled = true;
            bNew.Enabled = true;

            bRefresh.Enabled = true;

            bFind.Enabled = true;
            bPrint.Enabled = true;

            bSave.Enabled = true;
            bUndo.Enabled = true;
            _NavigationEnabled = true;
        }



        /// <summary>
        /// Sets the data navigator.
        /// </summary>
        public void SetDataNavigator()
        {
            Accelerators = new Keys[] { _MoveFirstFKey, _MoveLastFKey, _MoveNextFKey, _MovePreviousFKey, _AddNewFKey, _DeleteFKey, _RefreshFKey, _FindKey, _UndoFKey, _SaveFKey, _CloseFKey };
            bool RowExist = false;
            if (DataGridActive)
            {
                switch (_ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridView != null)
                        {
                            if (_DataGridView.RowCount > 0)
                            {
                                RowExist = true;
                            }
                        }
                        break;
                    case ViewModelGridModes.DataRepeater:
                        if (_DataRepeater != null)
                        {
                            if (_DataRepeater.ItemCount > 0)
                            {
                                RowExist = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            if (!RowExist)
            {
                bLast.Enabled = false;
                bFirst.Enabled = false;
                bNext.Enabled = false;
                bPrev.Enabled = false;
                if (_ReadOnlyMode)
                {
                    bNew.Enabled = false;
                }
                else
                {
                    bNew.Enabled = true;
                }
                bSave.Enabled = false;
                bDelete.Enabled = false;
                bUndo.Enabled = false;
                bPrint.Enabled = true;
                bRefresh.Enabled = true;
                bFind.Enabled = true;
                bClose.Enabled = true;
            }
            else
            {
                bLast.Enabled = true;
                bFirst.Enabled = true;
                bNext.Enabled = true;
                bPrev.Enabled = true;
                if (!_ReadOnlyMode)
                {
                    bNew.Enabled = true;
                    bSave.Enabled = true;
                    bDelete.Enabled = true;
                    bUndo.Enabled = true;
                }
                else
                {
                    bNew.Enabled = false;
                    bSave.Enabled = false;
                    bDelete.Enabled = false;
                    bUndo.Enabled = false;
                }
                bPrint.Enabled = true;
                bRefresh.Enabled = true;
                bFind.Enabled = true;
                bClose.Enabled = true;
            }

            UpdateButtonsCaption();
            UpdateRecordLabel();
        }



        /// <summary>
        /// Updates the buttons caption.
        /// </summary>
        public void UpdateButtonsCaption()
        {
            MoveFirstCaption = _MoveFirstCaption;
            MoveLastCaption = _MoveLastCaption;
            MoveNextCaption = _MoveNextCaption;
            MovePreviousCaption = _MovePreviousCaption;
            AddNewCaption = _AddNewCaption;
            DeleteCaption = _DeleteCaption;
            FindCaption = _FindCaption;
            RefreshCaption = _RefreshCaption;
            PrintCaption = _PrintCaption;
            CloseCaption = _CloseCaption;
            SaveCaption = _SaveCaption;
            UndoCaption = _UndoCaption;
        }

        /// <summary>
        /// Databases the object bound completed.
        /// </summary>
        private void _DbObject_BoundCompleted()
        {

            RaiseEventBoundCompleted();
            //eBoundCompleted?.Invoke();
            //_DataBoundCompleted = true;
        }


        /// <summary>
        /// Handles the RowEnter event of the _DataGridListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void _DataGridListView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (_DataGridListView.Focused)
            {
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
                try
                {
                    //_DbObject.MoveTo(Conversions.ToInteger(_DataGridListView.CurrentRow.Cells[_DataGridListViewRowIndexColumnName].Value));
                }
                catch (Exception ex)
                {

                }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata

            }

        }


        /// <summary>
        /// Sets the language.
        /// </summary>
        /// <param name="Language">The language.</param>
        public void SetLanguage(string Language)
        {
            switch (Language.ToLower().Trim() ?? "")
            {
                case "it-it":
                    {
                        _Language = "it";
                        //_MovePreviousCaption = "Prec.";
                        //_MoveNextCaption = "Succ.";
                        //_MoveFirstCaption = "Inizio";
                        //_MoveLastCaption = "Fine";
                        //_AddNewCaption = "Nuovo";
                        //_DeleteCaption = "Elimina";
                        //_SaveCaption = "Salva";
                        //_RefreshCaption = "Ricarica";
                        //_UndoCaption = "Annulla";
                        //_CloseCaption = "Chiudi";
                        //_FindCaption = "Trova";
                        //_PrintCaption = "Stampa";
                        //_DeleteMessage = "Confermi la cancellazione dei dati?";
                        //_SaveMessage = "Confermi il salvataggio dei dati?";
                        //_RecordLabelSeparator = "di";
                        //_RecordLabelNewRow = "Nuovo elemento";
                        break;
                    }

                default:
                    {
                        _Language = "en";
                        //_MovePreviousCaption = "Prev.";
                        //_MoveNextCaption = "Next";
                        //_MoveFirstCaption = "First";
                        //_MoveLastCaption = "Last";
                        //_AddNewCaption = "New";
                        //_DeleteCaption = "Delete";
                        //_SaveCaption = "Save";
                        //_RefreshCaption = "Refresh";
                        //_UndoCaption = "Undo";
                        //_CloseCaption = "Close";
                        //_FindCaption = "Find";
                        //_PrintCaption = "Print";
                        //_DeleteMessage = "Ok to delete data?";
                        //_SaveMessage = "Ok to save data ?";
                        //_RecordLabelSeparator = "of";
                        //_RecordLabelNewRow = "New Row";
                        break;
                    }
            }

        }




        /// <summary>
        /// Datas the grid ListView initialize.
        /// </summary>
        public void DataGridListViewInit()
        {

            int ncolIndex;


            if (_DataGridListView is null)
                return;
            _DataGridListView.DataSource = null;

            _DataGridListView.DefaultRowHeight = _DataGridListViewDefaultRowHeight;

            if (ListViewColumns.Count > 0)
            {
                _DataGridListView.DataSource = null;
                _DataGridListView.AutoGenerateColumns = false;
                _DataGridListView.Columns.Clear();
                _DataGridListView.Rows.Clear();

                foreach (ListViewColumn c in ListViewColumns)
                {

                    //string _Name = c.DbColumn.DbColumnNameE;
                    //string _FriendlyName = c.FriendlyName.Trim();
                    //string _DisplayFormat = c.DisplayFormat;
                    //if (string.IsNullOrEmpty(_FriendlyName))
                    //    _FriendlyName = c.DbColumn.FriendlyName;
                    //if (string.IsNullOrEmpty(_FriendlyName))
                    //    _FriendlyName = _Name;
                    //if (string.IsNullOrEmpty(_DisplayFormat.Trim()))
                    //    _DisplayFormat = c.DbColumn.DisplayFormat;


                    if (c.ColumnType == DataGridListViewColumnType.Undefined)
                    {
                        //switch (c.DbColumn.DbType)
                        //{
                        //    case var @case when @case == DbType.Boolean:
                        //        {
                        //            var ncol = new DataGridViewCheckBoxColumn();
                        //            ncolIndex = _DataGridListView.Columns.Add(ncol);
                        //            break;
                        //        }

                        //    default:
                        //        {
                        //            var ncol = new DataGridViewTextBoxColumn();
                        //            ncolIndex = _DataGridListView.Columns.Add(ncol);
                        //            break;
                        //        }
                        //}
                    }
                    else
                    {
                        switch (c.ColumnType)
                        {
                            case DataGridListViewColumnType.CheckBox:
                                {
                                    var ncol = new DataGridViewCheckBoxColumn();
                                    ncolIndex = _DataGridListView.Columns.Add(ncol);
                                    break;
                                }
                            case DataGridListViewColumnType.Image:
                                {
                                    var ncol = new DataGridViewImageColumn();
                                    ncol.CellImageLayout = DataGridViewImageCellLayout.BestFit;
                                    ncolIndex = _DataGridListView.Columns.Add(ncol);
                                    break;
                                }

                            case DataGridListViewColumnType.Link:
                                {
                                    var ncol = new DataGridViewLinkColumn();
                                    ncolIndex = _DataGridListView.Columns.Add(ncol);
                                    break;
                                }

                            default:
                                {
                                    var ncol = new DataGridViewTextBoxColumn();
                                    ncolIndex = _DataGridListView.Columns.Add(ncol);
                                    break;
                                }
                        }
                    }

                    //_DataGridListView.Columns[ncolIndex].Name = _Name;
                    //_DataGridListView.Columns[ncolIndex].HeaderText = _FriendlyName;
                    //_DataGridListView.Columns[ncolIndex].DataPropertyName = _Name;
                    //_DataGridListView.Columns[ncolIndex].DefaultCellStyle.Format = _DisplayFormat;
                    switch (c.Width)
                    {
                        case var case1 when case1 == 0:
                            {
                                //_DataGridListView.Columns[ncolIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                break;
                            }

                        default:
                            {
                                //_DataGridListView.Columns[ncolIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                //_DataGridListView.Columns[ncolIndex].Width = c.Width;
                                break;
                            }
                    }



                }
            }
            else
            {
                _DataGridListView.Columns.Clear();

                //foreach (BasicDAL.DbColumn c in DbObject.GetDbColumns())
                //{

                //    string _Name = c.DbColumnNameE;
                //    string _FriendlyName = c.FriendlyName.Trim();
                //    string _DisplayFormat = c.DisplayFormat;
                //    if (string.IsNullOrEmpty(_FriendlyName))
                //        _FriendlyName = _Name;


                //    switch (c.DbType)
                //    {
                //        case var case2 when case2 == DbType.Boolean:
                //            {
                //                var ncol = new DataGridViewCheckBoxColumn();
                //                ncolIndex = _DataGridListView.Columns.Add(ncol);
                //                break;
                //            }

                //        default:
                //            {
                //                var ncol = new DataGridViewTextBoxColumn();
                //                ncolIndex = _DataGridListView.Columns.Add(ncol);
                //                break;
                //            }
                //    }
                //    // Me._DataGridListView.Columns(ncolIndex).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                //    _DataGridListView.Columns[ncolIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //    _DataGridListView.Columns[ncolIndex].Name = _Name;
                //    _DataGridListView.Columns[ncolIndex].HeaderText = _FriendlyName;
                //    _DataGridListView.Columns[ncolIndex].DataPropertyName = _Name;
                //    _DataGridListView.Columns[ncolIndex].DefaultCellStyle.Format = _DisplayFormat;

                //}
            }



            //var hcol = new DataGridViewTextBoxColumn();
            //ncolIndex = _DataGridListView.Columns.Add(hcol);
            //_DataGridListViewRowIndexColumnIndex = ncolIndex;
            //_DataGridListView.Columns[ncolIndex].Name = _DataGridListViewRowIndexColumnName;
            //_DataGridListView.Columns[ncolIndex].DataPropertyName = _DataGridListViewRowIndexColumnName;
            //_DataGridListView.Columns[ncolIndex].Visible = false;
            //_DataGridListView.Columns[ncolIndex].ShowInVisibilityMenu = false;
            // _DataGridListViewDataTable = _DbObject.DataTable.Copy();
            //_DataGridListViewDataTable.Columns.Add(_DataGridListViewRowIndexColumnName);

            //for (int i = 0, loopTo = _DataGridListViewDataTable.Rows.Count - 1; i <= loopTo; i++)
            //{
            //    _DataGridListViewDataTable.Rows[i][_DataGridListViewRowIndexColumnName] = i;
            //}
            //_DataGridListView.Columns[ncolIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;



            this._DataGridListView.DataSource = this.ActiveDataNavigatorViewModel.ViewModel.ModelItems;
        }

        /// <summary>
        /// Handles the Accelerator event of the DataNavigator control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AcceleratorEventArgs"/> instance containing the event data.</param>
        private void DataNavigator_Accelerator(object sender, AcceleratorEventArgs e)
        {

        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ListViewColumn
    {
        //private BasicDAL.DbColumn mDBColumn;
        /// <summary>
        /// The m friendly name
        /// </summary>
        private string mFriendlyName;
        /// <summary>
        /// The m display format
        /// </summary>
        private string mDisplayFormat;
        /// <summary>
        /// The m column type
        /// </summary>
        private DataGridListViewColumnType mColumnType;
        /// <summary>
        /// The m width
        /// </summary>
        private int mWidth = 100;
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get
            {
                int WidthRet = default;
                WidthRet = mWidth;
                return WidthRet;

            }
            set
            {
                mWidth = value;
            }
        }
        /// <summary>
        /// Gets or sets the display format.
        /// </summary>
        /// <value>
        /// The display format.
        /// </value>
        public string DisplayFormat
        {
            get
            {
                string DisplayFormatRet = default;
                DisplayFormatRet = mDisplayFormat;
                return DisplayFormatRet;

            }
            set
            {
                mDisplayFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName
        {
            get
            {
                string FriendlyNameRet = default;
                FriendlyNameRet = mFriendlyName;
                return FriendlyNameRet;
            }
            set
            {
                mFriendlyName = value;
            }
        }
        //public BasicDAL.DbColumn DbColumn
        //{
        //    get
        //    {
        //        BasicDAL.DbColumn DbColumnRet = default;
        //        DbColumnRet = mDBColumn;
        //        return DbColumnRet;
        //    }
        //    set
        //    {
        //        mDBColumn = value;
        //    }
        //}
        /// <summary>
        /// Gets or sets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public DataGridListViewColumnType ColumnType
        {
            get
            {
                DataGridListViewColumnType ColumnTypeRet = default;
                ColumnTypeRet = mColumnType;
                return ColumnTypeRet;
            }
            set
            {
                mColumnType = value;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public enum DataGridListViewColumnType : int
    {
        /// <summary>
        /// The undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The text box
        /// </summary>
        TextBox = 1,
        /// <summary>
        /// The CheckBox
        /// </summary>
        CheckBox = 2,
        /// <summary>
        /// The image
        /// </summary>
        Image = 3,
        /// <summary>
        /// The link
        /// </summary>
        Link = 4
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.CollectionBase" />
    public class ListViewColumns : CollectionBase
    {



        //public void Add(BasicDAL.DbColumn DbColumn, string FriendlyName, string DisplayFormat = "", DataGridListViewColumnType ColumnType = DataGridListViewColumnType.Undefined, int Width = 0)
        //{
        //    var x = new ListViewColumn();

        //    x.DbColumn = DbColumn;
        //    x.FriendlyName = FriendlyName;
        //    x.DisplayFormat = DisplayFormat;
        //    x.ColumnType = ColumnType;
        //    x.Width = Width;

        //    if (string.IsNullOrEmpty(Strings.Trim(x.FriendlyName)))
        //        x.FriendlyName = x.DbColumn.FriendlyName;

        //    List.Add(x);

        //}


        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns></returns>
        public ListViewColumn get_Item(int Index)
        {
            ListViewColumn ItemRet = default;
            ItemRet = (ListViewColumn)List[Index];
            return ItemRet;
        }

        /// <summary>
        /// Sets the item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <param name="value">The value.</param>
        public void set_Item(int Index, ListViewColumn value)
        {
            List[Index] = value;
        }


    }
}