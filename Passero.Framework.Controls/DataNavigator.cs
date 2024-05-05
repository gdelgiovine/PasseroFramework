using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Dapper.Contrib.Extensions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
//using Passero.Framework.Base;
using Passero.Framework;
using Wisej.Web;
using Wisej.Web.Data;

namespace Passero.Framework.Controls
{

    public partial class DataNavigator
    {

        public enum EventType
        {
            Move,
            MoveFirst,
            MoveLast,
            MoveNext,
            MovePrevious,
            AddNew,
            Delete,
            Save,
            Close,
            Undo,
            Print,
            Find,
            Refresh
        }

        public ListViewColumns ListViewColumns = new ListViewColumns();
        private DataTable _DataGridListViewDataTable;
        private int _DataGridListViewDefaultRowHeight = 24;
        private string _MovePreviousCaption = "Prev.";
        private string _MoveNextCaption = "Next";
        private string _MoveFirstCaption = "First";
        private string _MoveLastCaption = "Last";
        private string _AddNewCaption = "New";
        private string _DeleteCaption = "Delete";
        private string _SaveCaption = "Save";
        private string _RefreshCaption = "Refresh";
        private string _UndoCaption = "Undo";
        private string _CloseCaption = "Close";
        private string _FindCaption = "Find";
        private string _PrintCaption = "Print";
      
        private int _PrintFKey = (int)Keys.F8;

        //public string RecordLabelSeparator { get; set; } = "of";
        public string RecordLabelHtmlFormat { get; set; } = "<p style='margin-top:2px;line-height:1.0;text-align:center;'>{0}<br>{1}<br>{2}</p>";

        public Dictionary <string, DataNavigatorViewModel > ViewModels { get; set; } = new Dictionary<string, DataNavigatorViewModel> ();

        private DataRepeater __DataRepeater;
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


        public int ModelItemsCount
        {
            get
            {
                if(_ActiveViewModel ==null)
                    return 0;

                return (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "ModelItemsCount");
            }
        }

        public  int CurrentModelItemIndex
        {
            get {
                if (this._ActiveViewModel != null)
                    return -1;
                if (DesignMode == false)
                    return (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "CurrentModelItemIndex");
                else
                    return -1;
            }
            set {
                if (DesignMode == false)
                {
                    if (_ActiveViewModel != null)
                        Passero.Framework.ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "CurrentModelItemIndex", value);
                }
            }
        }
        public int AddNewCurrentModelItemIndex
        {
            get
            {
                if (this._ActiveViewModel != null)
                    return -1;
                if (DesignMode ==false)
                    return (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "AddNewCurrentModelItemIndex");
                else
                   return -1;
                
            }
            set
            {
                if (DesignMode == false)
                {
                    if (_ActiveViewModel != null)
                        Passero.Framework.ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "AddNewCurrentModelItemIndex", value);
                }
            }
        }



        public void ViewModel_AddNew(object NewItem = null, bool InsertAtCursor = false)
        {

            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    this.DataGrid_AddNew(NewItem, InsertAtCursor);
                    break;
                case ViewModelGridModes.DataRepeater:
                    this.DataRepeater_AddNew(NewItem, InsertAtCursor);
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this.ActiveViewModel, "AddNew", Microsoft.VisualBasic.CallType.Method, NewItem);
                    break;
            }
        }


        public void ViewModel_DeleteItem()
        {
            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (this._DataGridView != null && this.DataGridActive)
                    {
                        this.DataGrid_Delete();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (this._DataRepeater != null && this.DataGridActive)
                    {
                        this.DataRepeater_Delete();
                    }
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this.ActiveViewModel, "DeleteItem", Microsoft.VisualBasic.CallType.Method, null);
                    break;
            }

        }


        public void ViewModel_DeleteItems()
        {
            Passero.Framework.ReflectionHelper.CallByName(ActiveViewModel, "DeleteItems", CallType.Method);
        }

        public void ViewModel_UndoChanges()
        {
            if (DataGridView != null && DataGridActive)
            {
                DataGrid_Undo();
            }
            else
            {
                Passero.Framework.ReflectionHelper.CallByName(ActiveViewModel, "UndoChanges", CallType.Method);
            }
        }

        public void ViewModel_MoveLastItem()
        {

            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (this._DataGridView != null && this.DataGridActive)
                    {
                        this.DataGrid_MoveLast();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (this._DataRepeater != null && this.DataGridActive)
                    {
                        this.DataRepeater_MoveLast();
                    }
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "MoveLastItem", Microsoft.VisualBasic.CallType.Method);
                    break;
            }
        }


        public void ViewModel_MoveFirstItem()
        {
            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (this._DataGridView != null && this.DataGridActive)
                    {
                        this.DataGrid_MoveFirst();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (this._DataRepeater != null && this.DataGridActive)
                    {
                        this.DataRepeater_MoveFirst();
                    }
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "MoveFirstItem", Microsoft.VisualBasic.CallType.Method);
                    break;
            }


            //if (this._DataGridView != null && this._DataGridActive)
            //{
            //    this.DataGrid_MoveFirst();
            //}
            //else
            //{
            //    Framework.ReflectionHelper.CallByName(this.ActiveViewModel, "MoveFirstItem", Microsoft.VisualBasic.CallType.Method, null);
            //}
        }


        public void ViewModel_MovePreviousItem()
        {
            if (DataGridView != null && DataGridActive)
            {
                DataGrid_MovePrevious();
            }
            else
            {
                Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "MovePreviousItem", CallType.Method);
            }
        }

        public void ViewModel_MoveNextItem()
        {
            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (this._DataGridView != null && this.DataGridActive)
                    {
                        this.DataGrid_MoveNext();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (this._DataRepeater != null && this.DataGridActive)
                    {
                        this.DataRepeater_MoveNext();
                    }
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "MoveNextItem", CallType.Method);
                    break;
            }
        }

        public void ViewModel_UdpateItem()
        {
            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (this._DataGridView != null && this.DataGridActive)
                    {
                        this.DataGrid_Save();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (this._DataRepeater != null && this.DataGridActive)
                    {
                        this.DataRepeater_Save();
                    }
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "UpdateItem", Microsoft.VisualBasic.CallType.Method);
                    break;
            }
        }


        public void ViewModel_UdpateItems()
        {
            switch (this._ActiveDataNavigatorViewModel.GridMode)
            {
                case ViewModelGridModes.DataGridView:
                    if (this._DataGridView != null && this.DataGridActive)
                    {
                        this.DataGrid_Save();
                    }
                    break;
                case ViewModelGridModes.DataRepeater:
                    if (this._DataRepeater != null && this.DataGridActive)
                    {
                        this.DataRepeater_Save();
                    }
                    break;
                default:
                    Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "UpdateItems", Microsoft.VisualBasic.CallType.Method);
                    break;
            }
        }



        public void SetActiveViewModel(DataNavigatorViewModel viewModel)
        {
            var typename = typeof(ViewModel<>).Name; // "ViewModel`1";
            if (Equals(viewModel.ViewModel.GetType().Name, typename) || Equals(viewModel.ViewModel.GetType().BaseType.Name, typename))
            {
                _ActiveViewModel = viewModel.ViewModel;
                _ActiveDataNavigatorViewModel = viewModel;
                _ModelItems = ReflectionHelper.CallByName(viewModel.ViewModel, "ModelItems", Microsoft.VisualBasic.CallType.Get, null);
                try
                {
                    _ModelItem = ReflectionHelper.CallByName(viewModel.ViewModel, "ModelItem", Microsoft.VisualBasic.CallType.Get, null);
                }
                catch (Exception __unusedException1__)
                {
                    _ModelItem = _ModelItems.GetType().GetGenericArguments()[0];
                }
                _BindingSource = (BindingSource)ReflectionHelper.GetPropertyValue(viewModel.ViewModel, "BindingSource");
                //ReflectionHelper.SetPropertyValue(ref viewModel.ViewModel, "DataNavigator", this)
                ReflectionHelper.CallByName( viewModel.ViewModel, "DataNavigator", CallType.Set,this);

                switch (this.ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridView != null)
                        {
                            _DataGridView.ReadOnly = true;
                        }
                        if (viewModel.DataGridView != null)
                        {
                            _DataGridView = viewModel.DataGridView;
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
                        break;

                    case ViewModelGridModes.DataRepeater:

                        if (this._DataRepeater != null)
                        {
                            _DataRepeater.Enabled = false;
                        }
                        if (viewModel.DataRepeater != null)
                        {
                            _DataRepeater = viewModel.DataRepeater;
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
                        break;

                    default:
                        break;

                }
                Caption = viewModel.FriendlyName;

            }
            else
            {
                _ActiveViewModel = null;
                _ActiveDataNavigatorViewModel = null;
                //ReflectionHelper.SetPropertyValue(ref viewModel.ViewModel, "DataNavigator", null);
                ReflectionHelper.CallByName (viewModel.ViewModel, "DataNavigator", CallType.Set,null);
            }
            if (_ModelItems != null)
            {
                EnableAllButtons();
            }
            UpdateRecordLabel();
        }



        public void SetActiveViewModel(string viewModel)
        {
            if (this.ViewModels .ContainsKey(viewModel)) 
            {
                SetActiveViewModel(this.ViewModels[viewModel]);
            }
        }
        private object?  _ActiveViewModel  = null;

        public object? ActiveViewModel
        {
            get
            {
                return _ActiveViewModel;
            }
          
        }
        


        public Type? ModelType
        {
            get
            {
                //if (_ModelItems != null)
                //    return Passero.Framework.ReflectionHelper.GetListType(_ModelItems);

                if (this._ActiveViewModel != null)
                {
                    return (Type)Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelType", CallType.Get, null);
                }
               
                return null;    
            }
            
        }

        private object? _ModelItem = null;

        public object? ModelItem
        {
            get
            {
                if (this._ActiveViewModel != null)
                {
                    return Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "Model", CallType.Get ,null);
                }
                return null;    
            }
            set
            {
                if (this._ActiveViewModel != null)
                {
                    Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "Model", CallType.Set, value );
                }
                

            }
        }

        private object? _ModelItems = null;

        public object? ModelItems
        {
            get
            {

                if (this._ActiveViewModel != null)
                {
                    return Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItems", CallType.Get, null);
                }
                return null;
               
            }
            set
            {

                if (this._ActiveViewModel != null)
                {
                    Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItems", CallType.Set, value);
                }


            }
        }

        //private object? _ModelItemShadow = null;

        public object? ModelItemShadow
        {
            get
            {

                if (this._ActiveViewModel != null)
                {
                    return Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemShadow", CallType.Get, null);
                }
                return null;

            }
            set
            {

                if (this._ActiveViewModel != null)
                {
                    Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemShadow", CallType.Set, value);
                }


            }
        }

        //private object? _ModelItemsShadow = null;

        public object? ModelItemsShadow
        {
            get
            {

                if (this._ActiveViewModel != null)
                {
                    return Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemsShadow", CallType.Get, null);
                }
                return null;

            }
            set
            {

                if (this._ActiveViewModel != null)
                {
                    Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "ModelItemsShadow", CallType.Set, value);
                }


            }
        }

        private DataSet _Dataset;
        private DataTable _DataTable;
        private Wisej.Web .BindingSource _BindingSource;
        private string _DataGridListViewRowIndexColumnName = "$<rowindex>$";
        private int _DataGridListViewRowIndexColumnIndex = 0;

        private DataGridView __DataGrid;

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
        private DataGridView __DataGridListView;

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



        private string _DeleteMessage = "Confirm delete data?";
        private string _SaveMessage = "Confirm save data?";
        private string _AddNewMessage = "Confirm new data insert?";

        private bool _ManageNavigation = true;
        private bool _ManageChanges = true;
        private bool _DataGridActive = false;
        private bool _DataGridListViewActive = false;

        private Panel _DataPanel;


        public event eAddNewEventHandler eAddNew;
        public delegate void eAddNewEventHandler();

        public event eAddNewCompletedEventHandler eAddNewCompleted;
        public delegate void eAddNewCompletedEventHandler();

        public event ePrintEventHandler ePrint;
        public delegate void ePrintEventHandler();

        //public event ePrintCompletedEventHandler ePrintCompleted;
        //public delegate void ePrintCompletedEventHandler();

        public event eDeleteEventHandler eDelete;
        public delegate void eDeleteEventHandler();

        public event eDeleteCompletedEventHandler eDeleteCompleted;
        public delegate void eDeleteCompletedEventHandler();

        public event eRefreshEventHandler eRefresh;
        public delegate void eRefreshEventHandler();

        public event eRefreshCompletedEventHandler eRefreshCompleted;
        public delegate void eRefreshCompletedEventHandler();

        public event eCloseEventHandler eClose;
        public delegate void eCloseEventHandler();

        public event eFindEventHandler eFind;
        public delegate void eFindEventHandler();

        public event eSaveEventHandler eSave;
        public delegate void eSaveEventHandler();

        public event eSaveCompletedEventHandler eSaveCompleted;
        public delegate void eSaveCompletedEventHandler();


        public event eMovePreviousEventHandler eMovePrevious;
        public delegate void eMovePreviousEventHandler();

        public event eMoveFirstEventHandler eMoveFirst;
        public delegate void eMoveFirstEventHandler();

        public event eMoveLastEventHandler eMoveLast;
        public delegate void eMoveLastEventHandler();

        public event eMoveNextEventHandler eMoveNext;
        public delegate void eMoveNextEventHandler();

        public event eMoveAtItemEventHandler eMoveAtItem;
        public delegate void eMoveAtItemEventHandler();

        public event eUndoEventHandler eUndo;
        public delegate void eUndoEventHandler();
        public event eUndoCompletedEventHandler eUndoCompleted;
        public delegate void eUndoCompletedEventHandler();

        public event eBoundCompletedEventHandler eBoundCompleted;
        public delegate void eBoundCompletedEventHandler();

        public event eAddNewRequestEventHandler eAddNewRequest;
        public delegate void eAddNewRequestEventHandler(ref bool Cancel);

        public event ePrintRequestEventHandler ePrintRequest;
        public delegate void ePrintRequestEventHandler(ref bool Cancel);

        public event eDeleteRequestEventHandler eDeleteRequest;
        public delegate void eDeleteRequestEventHandler(ref bool Cancel);

        public event eRefreshRequestEventHandler eRefreshRequest;
        public delegate void eRefreshRequestEventHandler(ref bool Cancel);

        public event eCloseRequestEventHandler eCloseRequest;
        public delegate void eCloseRequestEventHandler(ref bool Cancel);

        public event eFindRequestEventHandler eFindRequest;
        public delegate void eFindRequestEventHandler(ref bool Cancel);

        public event eSaveRequestEventHandler eSaveRequest;
        public delegate void eSaveRequestEventHandler(ref bool Cancel);

        public event eMovePreviousRequestEventHandler eMovePreviousRequest;
        public delegate void eMovePreviousRequestEventHandler(ref bool Cancel);

        public event eMoveFirstRequestEventHandler eMoveFirstRequest;
        public delegate void eMoveFirstRequestEventHandler(ref bool Cancel);

        public event eMoveLastRequestEventHandler eMoveLastRequest;
        public delegate void eMoveLastRequestEventHandler(ref bool Cancel);

        public event eMoveNextRequestEventHandler eMoveNextRequest;
        public delegate void eMoveNextRequestEventHandler(ref bool Cancel);

        public event eMoveAtItemRequestEventHandler eMoveAtItemRequest;
        public delegate void eMoveAtItemRequestEventHandler(ref bool Cancel);


        public event eUndoRequestEventHandler eUndoRequest;
        public delegate void eUndoRequestEventHandler(ref bool Cancel);

        private bool _ReadOnlyMode = false;
        private bool _AddNewState = false;
        private int _NewItemIndex = -1;
        private bool _DeletePending = false;
        private bool _SavePending = false;
        private bool _PrintPending = false;
        private bool _FindPending = false;
        private bool _UndoPending = false;
        private bool _NavigationEnabled = true;
        private int _DataGridRow = -1;
        private bool _CompactMode = false;


        private Keys _MovePreviousFKey = Keys.F6;
        private Keys _MoveNextFKey = Keys.F7;
        private Keys _MoveFirstFKey = (Keys)((int)Keys.Shift + (int)Keys.F6);
        private Keys _MoveLastFKey = (Keys)((int)Keys.Shift + (int)Keys.F7);
        private Keys _AddNewFKey = Keys.F2;
        private Keys _DeleteFKey = Keys.F3;
        private Keys _SaveFKey = Keys.F10;
        private Keys _RefreshFKey = Keys.F5;
        private Keys _UndoFKey = Keys.F9;
        private Keys _CloseFKey = Keys.F12;
        private Keys _FindKey = Keys.F4;

        private bool _FKeyEnabled = false;

        private string _RecordLabelSeparator = "of";
        private string _RecordLabelNewRow = "New Row";

        private bool _DataBoundCompleted = false;
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


        private string _Language = "it-IT";
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

        public DataBindingMode DataBindingMode()
        {
            return (DataBindingMode)Passero.Framework.ReflectionHelper.GetPropertyValue(this._ActiveViewModel, "DataBindingMode");
        }
        public BindingSource BindingSource
        {
            get
            {
                BindingSource BindingSourceRet = default;
                BindingSourceRet = _BindingSource;
                return BindingSourceRet;
            }
            set
            {
                _BindingSource = value;
            }
        }

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
        public int DataGridRow
        {
            get
            {
                int DataGridRowRet = default;
                DataGridRowRet = _DataGridRow;
                return DataGridRowRet;
            }
            set
            {
                _DataGridRow = value;
            }
        }
        public DataGridView DataGridView
        {
            get
            {
                DataGridView DataGridRet = default;
                DataGridRet = _DataGridView;
                return DataGridRet;
            }
            set
            {
                _DataGridView = value;
                //if (DbObject is not null & _DataGrid is not null)
                //{
                //    _DataGrid.DataSource = DbObject.DataTable;
                //}

            }
        }

        public DataGridView DataGridListView
        {
            get
            {
                DataGridView DataGridListViewRet = default;
                DataGridListViewRet = _DataGridListView;
                return DataGridListViewRet;
            }
            set
            {
                _DataGridListView = value;
                _DataGridListView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _DataGridListView.MultiSelect = false;


            }
        }
        private string _Caption="DataNavigator";
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

        public void DataNavigatorInit(bool LoadData = false)
        {
            _InitDataNavigator(LoadData);
        }

        public void Init(bool LoadData = false)
        {
            _InitDataNavigator(LoadData);
        }

        public void InitDataNavigator(bool LoadData = false)
        {
            _InitDataNavigator(LoadData);
        }


        private void _InitDataNavigator(bool LoadData = false)
        {
            UpdateButtonsCaption();
            if (LoadData)
            {
                Passero.Framework.ReflectionHelper.CallByName(this.ActiveViewModel, "GetAllItems", CallType.Method );
                //_DbObject.Open(true);
            }

            //if (_DbObject.IsReadOnly)
            //{
            //    AddNewVisible = false;
            //    AddNewEnabled = false;
            //    DisableNew();
            //    DeleteEnabled = false;
            //    DeleteVisible = false;
            //    DisableDelete();
            //    SaveEnabled = false;
            //    SaveVisible = false;
            //    DisableSave();
            //    UndoEnabled = false;
            //    UndoVisible = false;
            //    DisableUndo();
            //}
            //else
            //{
            //    AddNewVisible = true;
            //    DeleteVisible = true;
            //    SaveVisible = true;
            //    UndoVisible = true;
            //}
            UpdateRecordLabel();
            if (ModelItems == null)
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
        public int DataGrid_Save()
        {
            int i = 0;
            var argDataGridView = _DataGridView;
            i= DataGrid_Update(ref argDataGridView);
            _DataGridView = argDataGridView;
            return i;
        }

        public int DataGrid_Save(DataGridView DataGridView)
        {
            return DataGrid_Update(ref DataGridView);
        }

        public int DataGrid_Update()
        {
            int i = 0;
            var argDataGridView = _DataGridView;
            i= DataGrid_Update(ref argDataGridView);
            _DataGridView = argDataGridView;
            return i;
        }

  
        public int DataGrid_Update(ref DataGridView DataGridView)
        {
            int AffectedRecords = 0;
            int CurrentRowIndex = 0;
            int CurrentCellIndex = 0;

            if (DataGridView is null)
            {
                return AffectedRecords;
            }

            if (DataGridView is not null && DataGridView.DataSource is not null && DataGridView.CurrentRow is not null)
            {
                bool allowAdd = DataGridView.AllowUserToAddRows;
                CurrentRowIndex = DataGridView.CurrentRow.Index;
                CurrentCellIndex = this.DataGridView.CurrentCell.ColumnIndex;

                //try
                //{
                    DataGridView.EndEdit();

                    //Type dsType = DataGridView.DataSource.GetType().GetGenericArguments().Single();
                    //IList dt= (IList)DataGridView.DataSource;
                    //    object instance = (object)Activator.CreateInstance(dsType);


                    if (this._ActiveViewModel != null)
                    {
                        if (this._AddNewState  == true)
                        {

                            object item=((IList)this._ModelItems)[this.NewItemIndex];
                            var result = Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "InsertItem", CallType.Method, item);
                            var eresult = (Passero.Framework .ExecutionResult)Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "LastExecutionResult", CallType.Get );
                            if (eresult.Success)
                            {
                                _AddNewState = false;   
                            }
                        }

                        else
                        {
                            var result = Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "UpdateItems", CallType.Method, this._ModelItems );
                            var eresult = Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "LastExecutionResult", CallType.Get);

                        }  

                    }
           
                    //DataGridView.Refresh();
                //}
                //catch (Exception ex)
                //{
                //    throw;
                //}

                try
                {
                    DataGridView.CurrentCell = DataGridView[CurrentCellIndex, CurrentRowIndex];
                }
                catch (Exception )
                {
                }

                _DataGridRow = DataGridView.CurrentRow.Index;
                DataGridView.AllowUserToAddRows = allowAdd;




            }
            
            if (_AddNewState ==false)
            {
                foreach (DataGridViewRow _row in DataGridView.Rows)
                    _row.ReadOnly = false;
                SetDataNavigator();
            }
            else
            {
                SetButtonsForAddNew();
            }


            return AffectedRecords;


        }


        public int DataRepeater_Update()
        {
            int i = 0;
            var argDataRepeater = _DataRepeater;
            i = this.DataRepeater_Update(ref argDataRepeater);
            _DataRepeater = argDataRepeater;
            return i;
        }

        public int DataRepeater_Update(ref Wisej.Web.DataRepeater DataRepeater)
        {
            int AffectedRecords = 0;
            int CurrentRowIndex = 0;
            int CurrentCellIndex = 0;
            if (DataRepeater == null)
            {
                return AffectedRecords;
            }
            if (DataRepeater != null && DataRepeater.DataSource != null && DataRepeater.CurrentItem != null)
            {
                bool allowAdd = DataRepeater.AllowUserToAddItems;
                CurrentRowIndex = DataRepeater.CurrentItemIndex;
                CurrentCellIndex = 0;

                if (this._ActiveViewModel != null)
                {
                    if (this._AddNewState == true)
                    {
                        object item = ((IList)this._ModelItems)[this.NewItemIndex];
                        object result = ReflectionHelper.CallByName(this._ActiveViewModel, "InsertItem", CallType.Method, new[] { item });
                        ExecutionResult eresult = (ExecutionResult)ReflectionHelper.CallByName(this._ActiveViewModel, "LastExecutionResult", CallType.Get);
                        if (eresult.Success)
                        {
                            this._AddNewState = false;
                        }
                    }
                    else
                    {
                        object result = ReflectionHelper.CallByName(this._ActiveViewModel, "UpdateItems", CallType.Method, new[] { this._ModelItems });
                        object eresult = ReflectionHelper.CallByName(this._ActiveViewModel, "LastExecutionResult", CallType.Get);
                    }
                }
                try
                {
                    // DataGridView.CurrentCell = DataGridView(CurrentCellIndex, CurrentRowIndex);
                }
                catch (Exception ex)
                {
                }
                // this._DataGridRow = DataGridView.CurrentRow.Index;
                DataRepeater.AllowUserToAddItems = allowAdd;
            }
            if (this._AddNewState == false)
            {
                // foreach (Wisej.Web.DataGridViewRow _row in DataGridView.Rows)
                // {
                //     _row.ReadOnly = false;
                // }
                this.SetDataNavigator();
            }
            else
            {
                this.SetButtonsForAddNew();
            }
            return AffectedRecords;
        }
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
        public void DataGrid_EnableRowChange()
        {
            _DataGridRow = -1;
        }
        public int DataGrid_AddNew(object Item=null, bool InsertAtCursor = false)
        {
            var argDataGridView = _DataGridView;
            return DataGrid_AddNew(ref argDataGridView, Item, InsertAtCursor);
            
        }

        public int DataRepeater_Delete()
        {
            return this.DataRepeater_Delete(_DataRepeater);

        }

        public int DataRepeater_Delete(DataRepeater DataRepeater)
        {
            int RowIndex = 0;
            if (DataRepeater == null)
            {
                return 0; // Exit Function
            }
            if (DataRepeater.DataSource != null)
            {
                if (DataRepeater.CurrentItem != null)
                {
                    RowIndex = DataRepeater.CurrentItemIndex;
                    if (_AddNewState == false)
                    {
                        if (this._ActiveViewModel != null)
                        {
                            object item = ((IList)this._ModelItems)[RowIndex];

                            object result = Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "DeleteItem", Microsoft.VisualBasic.CallType.Method, item);
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
                return xRec;
            }
            else
            {
                SetDataNavigator();
                return 0;
            }
        }

        private DataNavigatorViewModel _ActiveDataNavigatorViewModel = null;

        public DataNavigatorViewModel ActiveDataNavigatorViewModel
        {
            get
            {
                return _ActiveDataNavigatorViewModel;
            }

        }

     
      


        private void DataRepeater_MoveNext(bool IgnoreManageNavigation = false)
        {
            if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            {
                return;
            }

            if (DataGridActive)
            {
                if (this._DataRepeater != null)
                {
                    Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MoveNextItem");
                    this._DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }

        }

        private void DataRepeater_MoveLast(bool IgnoreManageNavigation = false)
        {
            if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            {
                return;
            }

            if (DataGridActive)
            {
                if (this._DataRepeater != null)
                {
                    Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MoveLastItem");
                    this._DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }
        }
        private void DataRepeater_MovePrevious(bool IgnoreManageNavigation = false)
        {
            if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            {
                return;
            }

            if (DataGridActive)
            {
                if (this._DataRepeater != null)
                {
                    Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MovePreviousItem");
                    this._DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }
        }

        private void DataRepeater_MoveFirst(bool IgnoreManageNavigation = false)
        {
            if (IgnoreManageNavigation == false && _ManageNavigation == true || _DataRepeater == null)
            {
                return;
            }

            if (DataGridActive)
            {
                if (this._DataRepeater != null)
                {
                    Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MoveFirstItem");
                    this._DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                }
            }
        }

        public int DataRepeater_Save()
        {
            int i = 0;
            var argDataRepeater = _DataRepeater;
            i = this.DataRepeater_Update(ref argDataRepeater);
            _DataRepeater = argDataRepeater;
            return i;
        }

        public int DataRepeater_Save(DataRepeater DataRepeater)
        {
            return this.DataRepeater_Update(ref DataRepeater);
        }

        public void DataRepeater_Undo()
        {
            this.DataRepeater_Undo(this._DataRepeater);
        }

        public void DataRepeater_Undo(DataRepeater DataRepeater)
        {
            if (DataRepeater == null)
            {
                return;
            }
            if (DataRepeater.DataSource != null)
            {
                Microsoft.VisualBasic.Interaction.CallByName(_ActiveViewModel, "UndoChanges", Microsoft.VisualBasic.CallType.Method, true);
                DataRepeater.DataSource = this.ModelItems;
            }
            _AddNewState = false;
            SetDataNavigator();
        }

        public int DataRepeater_AddNew(object Item = null, bool InsertAtCursor = false)
        {

            var argDataRepeater = _DataRepeater;
            return DataRepeater_AddNew(ref argDataRepeater, Item, InsertAtCursor);
            
        }

        public int DataRepeater_AddNew(ref DataRepeater DataRepeater, object NewItem = null, bool InsertAtCurrentRow = false)
        {
            int NewRowIndex = -1;
            if (DataRepeater == null)
                return 0;

            if (DataRepeater.DataSource != null)
            {
                try
                {
                    IList items = (IList)DataRepeater.DataSource;
                    Type T = ReflectionHelper.GetListType(items);
                    _AddNewState = true;
                    if (NewItem == null)
                    {
                        NewItem = Activator.CreateInstance(T);
                    }

                    if (items.Count == 0)
                    {
                        items = Framework.ReflectionHelper.GetBindingListOfType(T);
                        DataRepeater.DataSource = items;
                        _AddNewState = true;
                        DataRepeater.AddNew();
                        NewRowIndex = 0;
                        items[0] = NewItem;
                        DataRepeater.CurrentItemIndex = NewRowIndex;
                        DataRepeater.Refresh();
                        DataRepeater.Select();
                        if (Framework.ReflectionHelper.IsBindingList(items))
                        {
                            items = (IList)ReflectionHelper.ConvertBindingListToList(items,T);
                        }

                        this.ModelItems = items;
                        this.ModelItem = NewItem;
                    }
                    else
                    {
                        if (InsertAtCurrentRow)
                        {
                            // GridRowIndex = DataRepeater.CurrentItemIndex
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
                    Framework.ReflectionHelper.SetPropertyValue(ref this._ActiveViewModel, "AddNewState", true);
                    UpdateRecordLabel();
                    SetButtonsForAddNew();
                }
                catch (Exception ex)
                {
                    _AddNewState = false;
                    throw;
                }
            }

            return DataRepeater.CurrentItemIndex;
        }




        public int DataGrid_AddNew(ref DataGridView DataGridView, object Item = null, bool InsertAtCurrentRow = false)
        {

            int ColumnIndex = 0;
            int RowIndex = 0;
            int GridRowIndex = 0;
            int NewRowIndex = -1;
            if (DataGridView is null)
                return 0;
            if (DataGridView.DataSource  != null)
            {
                try
                {   
                    
                    IList items = (IList)DataGridView.DataSource;
                    Type T = Passero.Framework.ReflectionHelper.GetListType(items );
                    _AddNewState = true;
                    ColumnIndex = FindFirstEditableColumn(DataGridView);
                    if (Item==null)
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
                        this._NewItemIndex = 0;
                        this._DataGridView.DataSource = null;
                        this._DataGridView.DataSource = items;
                        
                    }

                    else
                    {
                        GridRowIndex = DataGridView.CurrentRow.Index;
                        if (InsertAtCurrentRow == true)
                        {
                            // ------- INSERISCE NELLA POSIZIONE DEL CURSORE ----
                            switch (DataGridView.SortOrder)
                            {
                                case var @case when @case == SortOrder.None:
                                    {
                                        RowIndex = DataGridView.CurrentRow.Index;
                                        items.Insert(RowIndex, Item);
                                        NewRowIndex = RowIndex;
                                        break;
                                    }
                                case var case1 when case1 == SortOrder.Descending:
                                    {
                                        RowIndex = DataGridView.Rows.Count - 1;
                                        NewRowIndex = RowIndex;
                                        break;
                                    }
                                case var case2 when case2 == SortOrder.Ascending:
                                    {
                                        RowIndex = 0;
                                        items.Insert(RowIndex, Item);
                                        NewRowIndex = 0;
                                        break;
                                    }
                            }
                        }

                        // ----------------------------------------------------
                        else
                        {

                            DataGridView.CurrentCell = DataGridView[ColumnIndex, DataGridView.Rows.Count - 1];
                            // ------- INSERISCE ALLA FINE O ALL'INIZIO-----------

                            switch (DataGridView.SortOrder)
                            {
                                case var case3 when case3 == SortOrder.Descending:
                                    {

                                        RowIndex = DataGridView.Rows.Count - 1;
                                        items.Insert(RowIndex, Item);
                                        NewRowIndex = RowIndex;
                                        break;
                                    }
                                case var case4 when case4 == SortOrder.Ascending:
                                    {

                                        RowIndex = 0;
                                        NewRowIndex = RowIndex;
                                        items.Insert(RowIndex, Item);
                                        break;
                                    }
                                case var case5 when case5 == SortOrder.None:
                                    {

                                        RowIndex= items.Add(Item);
                                        NewRowIndex = RowIndex;
                                        //DataGridView.Rows.Insert(RowIndex, newrow);
                                        break;
                                    }
                            }
                            // ----------------------------------------------------
                        }
                        ColumnIndex = DataGridView.CurrentCell.ColumnIndex;
                        this._DataGridView.DataSource = null;
                        this._DataGridView.DataSource = items;
                    }


                    foreach (DataGridViewRow _row in DataGridView.Rows)
                    {
                        _row.ReadOnly = true;
                       
                    }
                    DataGridView.Rows[RowIndex].ReadOnly = false;
                    DataGridView.CurrentCell = DataGridView[ColumnIndex, RowIndex];
                    _DataGridRow = DataGridView.CurrentCell.RowIndex;

                    bool ok = true;
                    //bool ok=(bool)Passero.Framework.ReflectionHelper.CallByName(this._ViewModel, "AddNew", CallType.Method);
                    
                    if (ok==true)
                    {
                        _AddNewState = true;
                        this._NewItemIndex = NewRowIndex;

                        //Passero.Framework.ReflectionHelper.CallByName(this._ViewModel, "AddNewCurrentModelItemIndex", CallType.Set, _NewItemIndex);
                        UpdateRecordLabel();
                        SetButtonsForAddNew();
                    }
                    else
                    {
                        _AddNewState = false;
                        MessageBox.Show("ERROR ViewModel.AddNew");
                    }
                    
                }
                catch (Exception ex)
                {
            
                    _AddNewState = false;
                    throw ;
                }


            }
            
            return _DataGridRow;
        }




        public int DataGrid_AddNew2(ref DataGridView DataGridView, bool InsertAtCursor = false)
        {

            int ColumnIndex = 0;
            int RowIndex = 0;
            int GridRowIndex = 0;
            int xRowIndex = 0;
            if (DataGridView is null)
                return 0;
            if (DataGridView.DataSource is not null)
            {
                try
                {
                    DataTable dt;
                    dt = Passero.Framework.DapperHelper.Utilities.ObjectListToDataTable((object)this.DataGridView.DataSource);
                    //DataTable dt = (DataTable)DataGridView.DataSource;
                    
                    string Rf = dt.DefaultView.RowFilter;
                    string Rs = dt.DefaultView.Sort;
                    _AddNewState = true;
                    dt.DefaultView.RowFilter = "";
                    ColumnIndex = FindFirstEditableColumn(DataGridView);
                    // GridRowIndex = DataGridView.CurrentRow.Index

                    if (DataGridView.RowCount == 0)
                    {
                        RowIndex = 0;
                       
                        dt.Rows.Add();
                        GridRowIndex = DataGridView.CurrentRow.Index;
                    }

                    else
                    {
                        GridRowIndex = DataGridView.CurrentRow.Index;
                        if (InsertAtCursor == true)
                        {
                            // ------- INSERISCE NELLA POSIZIONE DEL CURSORE ----
                            // RowIndex = DataGridView.CurrentRow.Index

                            switch (DataGridView.SortOrder)
                            {
                                case var @case when @case == SortOrder.None:
                                    {
                                        xRowIndex = ((DataTable)DataGridView.DataSource).Rows.IndexOf(((DataRowView)DataGridView.CurrentRow.DataBoundItem).Row) + 1;
                                        DataRow nRow;
                                        nRow = dt.NewRow();
                                        dt.Rows.InsertAt(nRow, xRowIndex);
                                        RowIndex = xRowIndex;
                                        break;
                                    }
                                case var case1 when case1 == SortOrder.Descending:
                                    {
                                        dt.Rows.Add();
                                        RowIndex = DataGridView.Rows.Count - 1;
                                        break;
                                    }
                                case var case2 when case2 == SortOrder.Ascending:
                                    {
                                        dt.Rows.Add();
                                        RowIndex = 0;
                                        break;
                                    }
                            }
                        }

                        // ----------------------------------------------------
                        else
                        {

                            DataGridView.CurrentCell = DataGridView[ColumnIndex, DataGridView.Rows.Count - 1];
                            // ------- INSERISCE ALLA FINE O ALL'INIZIO-----------

                            switch (DataGridView.SortOrder)
                            {
                                case var case3 when case3 == SortOrder.Descending:
                                    {
                                        dt.Rows.Add();
                                        RowIndex = DataGridView.Rows.Count - 1;
                                        break;
                                    }
                                case var case4 when case4 == SortOrder.Ascending:
                                    {
                                        dt.Rows.Add();
                                        RowIndex = 0;
                                        break;
                                    }
                                case var case5 when case5 == SortOrder.None:
                                    {
                                        dt.Rows.Add();
                                        RowIndex = DataGridView.Rows.Count - 1;
                                        break;
                                    }
                            }
                            // ----------------------------------------------------
                        }
                        ColumnIndex = DataGridView.CurrentCell.ColumnIndex;
                    }


                    foreach (DataGridViewRow _row in DataGridView.Rows)
                        _row.ReadOnly = true;
                    DataGridView.Rows[RowIndex].ReadOnly = false;
                    DataGridView.CurrentCell = DataGridView[ColumnIndex, RowIndex];
                    _DataGridRow = DataGridView.CurrentCell.RowIndex;
                    _AddNewState = true;
                    UpdateRecordLabel();
                    SetButtonsForAddNew();
                }
                catch (Exception ex)
                {
                    // LockWindow(Me.ParentForm.Handle, False)
                    _AddNewState = false;
                }


            }
            // LockWindow(Me.ParentForm.Handle, False)
            return _DataGridRow;
        }

        public int DataGrid_Delete()
        {
            return DataGrid_Delete(_DataGridView);
        }

        public int DataGrid_Delete(DataGridView DataGridView)
        {
            int RowIndex = 0;
            if (DataGridView is null)
                return 0; // Exit Function
            if (DataGridView.DataSource is not null)
            {

                // If Me._ManageNavigation = True Then
                if (DataGridView.CurrentRow is not null)
                {
                    RowIndex = DataGridView.CurrentRow.Index;
                    if (DataGridView.CurrentRow.IsNewRow == false)
                    {
                        if (this._ActiveViewModel != null)
                        {
                            object item = ((IList)this._ModelItems)[RowIndex];
                            var result =    Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "DeleteItem", CallType.Method, item);
                            DataGridView.DataSource = null;
                            DataGridView.DataSource = this._ModelItems;
                        }
                    }
                }
                // endif
                int xRec = 1;
                if (RowIndex != 0)
                {
                    if (RowIndex < DataGridView.Rows.Count)
                    {
                        foreach (DataGridViewColumn Column in DataGridView.Columns)
                        {
                            if (Column.Visible == true)
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
                            if (Column.Visible == true)
                            {
                                DataGridView.CurrentCell = DataGridView.Rows[RowIndex - 1].Cells[Column.Index];
                                break;
                            }
                        }
                    }
                }
                return xRec;
            }
            else
            {
                SetDataNavigator();
                return 0;
            }

        }

        public void DataGrid_Undo()
        {
            DataGrid_Undo(_DataGridView);

        }

        public void DataGrid_Undo(DataGridView DataGridView)
        {

            if (DataGridView is null)
                return;
            if (DataGridView.DataSource is not null)
            {
                _DataGridView.CancelEdit();
                _DataGridView.DataSource = null;
                Passero.Framework.ReflectionHelper.CallByName(_ActiveViewModel, "UndoChanges", CallType.Method, true);
                _DataGridView.DataSource = this.ModelItems;
                
            }
            foreach (DataGridViewRow _row in DataGridView.Rows)
                _row.ReadOnly = false;

            _AddNewState = false;
            SetDataNavigator();


        }
        public void CancelFind()
        {
            _FindPending = false;
        }
        public void CancelPrint()
        {
            _PrintPending = false;
        }
        public void CancelSave()
        {
            _SavePending = false;
        }
        public void CancelAddNew()
        {
            // _AddNewPending = False
            //_DbObject.UndoChanges();
        }

        public void CancelUndo()
        {
            _UndoPending = false;

        }
        public void CancelDelete()
        {
            _DeletePending = false;
        }
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

        public int NewItemIndex
        {
            get { return _NewItemIndex; }
            set { _NewItemIndex = value; }
        }
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
        public void NavigationVisible(bool Visible)
        {
            bPrev.Visible = Visible;
            bFirst.Visible = Visible;
            bLast.Visible = Visible;
            bNext.Visible = Visible;
            RecordLabel.Visible = Visible;
        }
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
                    }
                    else
                    {
                        Passero.Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "DeleteItem");
                        int index = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this.ActiveViewModel, "CurrentModelItemIndex");
                        this.MoveLast();
                        eDeleteCompleted?.Invoke();
                    }
                }
            }
            else
            {
                eDelete?.Invoke();
            }
            this.UpdateRecordLabel ();  

        }

      
        public void Save(bool OverrideManagedChanges = false)
        {

            bool Cancel = false;
            eSaveRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;


            if (SaveVisible == false | SaveEnabled == false)
                return;

            if (DataGridListView is not null)
            {
                //DataGridListView.DataSource = _DbObject.DataTable;
            }

            if (OverrideManagedChanges == false && _ManageChanges == true)
            {
                if (MessageBox.Show(_SaveMessage, ParentForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_DataGridActive == true & _DataGridView is not null)
                    {
                        DataGrid_Update();
                        SetButtonForSave();
                    }
                    else
                    {
                        if (_AddNewState == true)
                        {
                            Passero.Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "InsertItem");
                            var eresult = (Passero.Framework.ExecutionResult)Passero.Framework.ReflectionHelper.CallByName(this._ActiveViewModel, "LastExecutionResult", CallType.Get);
                            if (eresult.Success)
                            {
                                _AddNewState = false;
                            }
                        }
                        else
                        {
                            Passero.Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "UpdateItem");
                        }
                        SetButtonForSave();
                    }
                    SetButtonForSave();
                    eSaveCompleted?.Invoke();
                }
            }
            else
            {
                SetButtonForSave();
                eSave?.Invoke();
            }

            this.UpdateRecordLabel();
        }
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
        private void bFirst_Click(object sender, EventArgs e)
        {
            MoveFirst();
        }

        private void bPrev_Click(object sender, EventArgs e)
        {
            MovePrevious();
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            MoveNext();
        }

        private void bLast_Click(object sender, EventArgs e)
        {

            MoveLast();

        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            _RefreshData();
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            AddNew();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void bUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void bFind_Click(object sender, EventArgs e)
        {

            bool Cancel = false;
            eFindRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            eFind?.Invoke();

        }


        private void bPrint_Click(object sender, EventArgs e)
        {
            bool Cancel = false;
            ePrintRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            ePrint?.Invoke();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            bool Cancel = false;
            eCloseRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            eClose?.Invoke();
        }

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
        public DataNavigator()
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();
            ToolBar.Height = 70;
            // Me.Panel.Width = Me.NavBar.Width
            // Me.Panel.Height = Me.NavBar.Height
            Panel.Dock = DockStyle.Fill;
            // Me.Width = Me.Panel.Width
            // Me.Height = Me.Panel.Height

            // Me.Width = Me.NavBar.Width
            // Me.Height = Me.NavBar.Height

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


        public void AddNew(object item = null, bool OverrideManagedChanges = false)
        {
            if (item == null)
            {
                item = Activator.CreateInstance(this.ModelType);
            }
            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eAddNewRequest != null)
                eAddNewRequest(ref Cancel);
            if (Cancel)
            {
                return;
            }
            if (!AddNewVisible)
            {
                return;
            }
            int index = Convert.ToInt32(Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "CurrentModelItemIndex", Microsoft.VisualBasic.CallType.Get));
            if (!OverrideManagedChanges && _ManageChanges)
            {
                
                switch (this._ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridActive && _DataGridView != null)
                        {
                            DataGrid_AddNew(item);
                        }
                        break;

                    case ViewModelGridModes.DataRepeater:
                        if (_DataGridActive && _DataRepeater != null)
                        {
                            DataRepeater_AddNew(item);
                        }
                        break;

                    default:
                        if (this.BindingSource != null && this.DataBindingMode() == Framework.DataBindingMode.BindingSource)
                        {
                            this.BindingSource.AddNew();
                            this.MoveLast();
                        }
                        break;


                }

                //if (_DataGridActive && _DataGridView != null)
                //{
                //    DataGrid_AddNew(item);
                //}
                //else
                //{
                //    if (this.BindingSource != null && this.DataBindingMode() == Framework.DataBindingMode.BindingSource)
                //    {
                //        this.BindingSource.AddNew();
                //        this.MoveLast();
                //    }
                //}

                this._AddNewState = true;
                Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "AddNewCurrentModelItemIndex", Microsoft.VisualBasic.CallType.Set, index);
                Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "AddNewState", Microsoft.VisualBasic.CallType.Set, true);
                Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "ModelItem", Microsoft.VisualBasic.CallType.Set, item);
                SetButtonsForAddNew();
                if (eAddNewCompleted != null)
                    eAddNewCompleted();
            }
            else
            {
                this._AddNewState = true;
                Microsoft.VisualBasic.Interaction.CallByName(this._ActiveViewModel, "AddNewCurrentModelItemIndex", Microsoft.VisualBasic.CallType.Set, index);
                //CallByName(Me._ViewModel, "AddNewState", CallType.Set, True)
                SetButtonsForAddNew();
                if (eAddNew != null)
                    eAddNew();
            }
            UpdateRecordLabel();
        }




        public void UpdateRecordLabel()
        {
            if (this.ActiveViewModel == null)
                return;
            int itemscount = 0;
            int index = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this.ActiveViewModel, "CurrentModelItemIndex");
            object items = Passero.Framework.ReflectionHelper.GetPropertyValue(this.ActiveViewModel, "ModelItems");
            if (items!=null)
                itemscount = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(items, "Count");
            this.RecordLabel.Text = String.Format(this.RecordLabelHtmlFormat, index + 1, this.RecordLabelSeparator, itemscount);

            //if (this._AddNewPending )
            //{
            //    int index = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this.ViewModel, "CurrentModelItemIndex");
            //    object items = Passero.Framework.ReflectionHelper.GetPropertyValue(this.ViewModel, "ModelItems");
            //    int itemscount = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(items, "Count");
            //    this.RecordLabel.Text = String.Format(this.RecordLabelHtmlFormat, index + 1, this.RecordLabelSeparator, itemscount);
            //}
            //else
            //{
               
            //    int index = (int)Passero.Framework.ReflectionHelper.GetPropertyValue(this.ViewModel, "CurrentModelItemIndex");
            //    object items=Passero.Framework.ReflectionHelper.GetPropertyValue(this.ViewModel, "ModelItems");
            //    int itemscount= (int)Passero.Framework.ReflectionHelper.GetPropertyValue(items, "Count");
            //    this.RecordLabel.Text = String.Format(this.RecordLabelHtmlFormat, index+1, this.RecordLabelSeparator, itemscount);
            //}

            
           


        }
        public void DataGrid_MoveFirst(bool IgnoreManageNavigation = false)
        {

            if (IgnoreManageNavigation == false & _ManageNavigation == true | _DataGridView is null)
                return;
            if (_DataGridActive == true & _DataGridView is not null & DataGridView.CurrentRow is not null)
            {
                _DataGridView.CurrentCell = _DataGridView.Rows[0].Cells[_DataGridView.CurrentCell.ColumnIndex];
            }
        }
        private void MoveFirstDataGridListView()
        {
            if (_DataGridListView is not null)
            {
                _DataGridListView.CurrentCell = _DataGridListView.Rows[0].Cells[_DataGridListView.CurrentCell.ColumnIndex];
            }

        }

        public void MoveFirst(bool OverrideManagedNavigation = false)
        {
            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMoveFirstRequest != null)
                eMoveFirstRequest(ref Cancel);
            if (Cancel)
            {
                return;
            }
            if (!OverrideManagedNavigation && _ManageNavigation)
            {
                Framework.ReflectionHelper.InvokeMethod (ref _ActiveViewModel, "MoveFirstItem");
                UpdateGridPosition();

            }
            else
            {

                if (eMoveFirst != null)
                    eMoveFirst();
            }
            MoveFirstDataGridListView();
            UpdateRecordLabel();
        }


        public void DataGrid_MovePrevious(bool IgnoreManageNavigation = false)
        {

            if (IgnoreManageNavigation == false & _ManageNavigation == true | _DataGridView is null)
                return;

            if (_DataGridActive == true & _DataGridView is not null & DataGridView.CurrentRow is not null)
            {
                if (_DataGridView.CurrentRow.Index > 0)
                {
                    _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index - 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                }
            }


        }
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
                Passero.Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MovePreviousItem");
                MovePreviousDataGridListView();
            }
            UpdateRecordLabel();
        }

        private void UpdateGridPosition()
        {
            if (_DataGridActive)
            {
                switch (this._ActiveDataNavigatorViewModel.GridMode)
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
                                this._DataRepeater.CurrentItemIndex = CurrentModelItemIndex;
                            }
                        }
                        break;
                    default:
                        break;

                }
            }

        }


        public void MovePrevious(bool OverrideManagedNavigation = false)
        {
            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMovePreviousRequest != null)
                eMovePreviousRequest(ref Cancel);
            if (Cancel == true)
            {
                return;
            }
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MovePreviousItem");
                UpdateGridPosition();
            }
            else
            {
                if (eMovePrevious != null)
                    eMovePrevious();
            }
            MovePreviousDataGridListView();
            UpdateRecordLabel();
        }



        public void DataGrid_MoveNext(bool IgnoreManageNavigation = false)
        {

            if (IgnoreManageNavigation == false & _ManageNavigation == true | _DataGridView is null)
                return;

            if (_DataGridActive == true & _DataGridView is not null & _DataGridView.CurrentRow is not null)
            {
                if (_DataGridView.CurrentRow.Index < _DataGridView.Rows.Count - 1)
                {
                    _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index + 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                }
            }
        }

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
        public void MoveAtItem(int ItemIndex, bool OverrideManagedNavigation = false)
        {

            bool Cancel = false;
            _DataBoundCompleted = false;
            eMoveAtItemRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;
            
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                if (_DataGridActive == true & _DataGridView is not null)
                {
                    if (_DataGridView.CurrentRow.Index < _DataGridView.Rows.Count - 1)
                    {
                        _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.CurrentRow.Index + 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                    }
                }
                else
                {
                    Passero.Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MoveAtItem",ItemIndex);
                    MoveAtItemDataGridListView(ItemIndex);
                    eUndoCompleted?.Invoke();
                }
            }
            else
            {
                MoveNextDataGridListView();
                eMoveAtItem?.Invoke();
            }

            UpdateRecordLabel();


        }

        public void MoveNext(bool OverrideManagedNavigation = false)
        {
            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMoveNextRequest != null)
                eMoveNextRequest(ref Cancel);
            if (Cancel == true)
            {
                return;
            }
         
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MoveNextItem");
                UpdateGridPosition();
            }
            else
            {
                if (eMoveNext != null)
                    eMoveNext();
            }
            MoveNextDataGridListView();
            UpdateRecordLabel();
        }


        public void DataGrid_MoveLast(bool IgnoreManageNavigation = false)
        {

            if (IgnoreManageNavigation == false & _ManageNavigation == true | _DataGridView is null)
                return;
            if (_DataGridActive == true & _DataGridView is not null & DataGridView.CurrentRow is not null)
            {
                for (int i = 0, loopTo = _DataGridView.Columns.Count - 1; i <= loopTo; i++)
                {
                    if (_DataGridView.Columns[i].ReadOnly == false)
                    {
                        _DataGridView.CurrentCell = _DataGridView.Rows[_DataGridView.RowCount - 1].Cells[_DataGridView.CurrentCell.ColumnIndex];
                        break;
                    }
                }
            }
        }

        private void MoveLastDataGridListView()
        {
            if (_DataGridListView is not null)
            {
                _DataGridListView.CurrentCell = _DataGridListView.Rows[_DataGridListView.RowCount - 1].Cells[_DataGridListView.CurrentCell.ColumnIndex];
            }
        }



        public void MoveLast(bool OverrideManagedNavigation = false)
        {
            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eMoveLastRequest != null)
                eMoveLastRequest(ref Cancel);
            if (Cancel == true)
            {
                return;
            }
            if (OverrideManagedNavigation == false && _ManageNavigation == true)
            {
                Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "MoveLastItem");
                UpdateGridPosition();
            }
            else
            {

                if (eMoveLast != null)
                    eMoveLast();
            }
            MoveLastDataGridListView();
            UpdateRecordLabel();
        }



        public void RefreshData()
        {
            _RefreshData();
            //if (_DataGridActive == true & _DataGrid is not null)
            //{

            //}
            //else
            //{
            //    Passero.Framework.ReflectionHelper.InvokeMethod(ref this._ViewModel, "ReloadItems");
            //}
            //UpdateRecordLabel();
        }
        private void _RefreshData()
        {

            bool Cancel = false;
            _DataBoundCompleted = false;
            eRefreshRequest?.Invoke(ref Cancel);
            if (Cancel == true)
                return;

            if (RefreshVisible == false | RefreshEnabled == false)
                return;

            CheckDataChange();

            if (_ManageNavigation == true)
            {
                if (_DataGridActive == true & _DataGridView is not null)
                {

                   
                }
                else
                {
                    Passero.Framework.ReflectionHelper.InvokeMethod(ref this._ActiveViewModel , "ReloadItems");
                }
                eRefreshCompleted ?.Invoke();   
            }


            else
            {
                eRefresh?.Invoke();
            }
            UpdateRecordLabel();

        }


        public void Undo(bool OverrideManagedChanges = false)
        {
            bool Cancel = false;
            _DataBoundCompleted = false;
            if (eUndoRequest != null)
                eUndoRequest(ref Cancel);
            if (Cancel)
            {
                return;
            }
            SetButtonForUndo();
            if (DataGridListView != null)
            {
                // DataGridListView.DataSource = _DbObject.DataTable
            }
            if (!OverrideManagedChanges && _ManageChanges)
            {

                switch (this._ActiveDataNavigatorViewModel.GridMode)
                {
                    case ViewModelGridModes.DataGridView:
                        if (_DataGridActive && _DataGridView != null)
                        {
                            DataGrid_Undo();
                        }
                        break;

                    case ViewModelGridModes.DataRepeater:
                        if (_DataGridActive && _DataRepeater != null)
                        {
                            DataGrid_Undo();
                        }
                        break;
                    case ViewModelGridModes.NoGridMode:
                        Framework.ReflectionHelper.InvokeMethod(ref _ActiveViewModel, "UndoChanges");
                        if (_AddNewState)
                        {
                            this.MoveAtItem(this.AddNewCurrentModelItemIndex);
                        }
                        break;
                }

                if (eUndoCompleted != null)
                    eUndoCompleted();
            }
            else
            {
                if (eUndo != null)
                    eUndo();
            }
            _AddNewState = false;
            UpdateRecordLabel();
        }
        public bool AddViewModel(string Name, object ViewModel, string FriendlyName = "DataNavigator", DataGridView DataGridView = null, DataRepeater DataRepeater = null)
        {
            DataNavigatorViewModel DataNavigatorViewModel = null;
            DataNavigatorViewModel = new DataNavigatorViewModel(ViewModel, Name, FriendlyName, DataGridView, DataRepeater);
            this.ViewModels[Name]= DataNavigatorViewModel;
            return true;
        }


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

                        _RefreshData();
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

        public void SetButtonsForReadWrite()
        {
            bUndo.Enabled = true;
            bSave.Enabled = true;
            bNew.Enabled = true;
            bDelete.Enabled = true;
        }

        public void SetButtonsForReadOnly()
        {
            bUndo.Enabled = false;
            bSave.Enabled = false;
            bNew.Enabled = false;
            bDelete.Enabled = false;
        }
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

        public void DataGrid_StartEdit(int CurrentRow)
        {

            _DataGridRow = CurrentRow;
            SetButtonForEdit();
        }

        public void DataGrid_StartEdit()
        {
            if (_DataGridView is not null)
            {

                SetButtonForEdit();
            }

        }

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

        public void DisableNavigation()
        {
            bFirst.Enabled = false;
            bNext.Enabled = false;
            bPrev.Enabled = false;
            bLast.Enabled = false;
            _NavigationEnabled = false;
        }
        public void EnableNavigation()
        {
            bFirst.Enabled = true;
            bNext.Enabled = true;
            bPrev.Enabled = true;
            bLast.Enabled = true;
            _NavigationEnabled = true;
        }

        public void DisableRefresh()
        {
            bRefresh.Enabled = false;

        }
        public void EnableRefresh()
        {
            bRefresh.Enabled = true;
        }
        public void DisableDelete()
        {
            bDelete.Enabled = false;
        }

        public void DisableNew()
        {
            bNew.Enabled = false;
        }

        public void EnableDelete()
        {
            bDelete.Enabled = true;
        }

        public void DisableSave()
        {
            bSave.Enabled = false;
        }
        public void DisableUndo()
        {
            bUndo.Enabled = false;
        }
        public void EnableUndo()
        {
            bUndo.Enabled = true;
        }

        public void EnableSave()
        {
            bSave.Enabled = true;
        }
        public void DisableFind()
        {
            bFind.Enabled = false;
        }
        public void EnableFind()
        {
            bFind.Enabled = true;
        }

        public void DisablePrint()
        {
            bPrint.Enabled = false;
        }
        public void EnablePrint()
        {
            bPrint.Enabled = true;
        }

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


        // Private Sub _DataGrid_CancelRowEdit(sender As Object, e As System.Windows.Forms.QuestionEventArgs) Handles _DataGrid.E
        // DataGrid_Undo()
        // End Sub

        //private void _mDBObject_DataEventAfter(BasicDAL.DataEventType EventType)
        //{

        //    switch (EventType)
        //    {
        //        case BasicDAL.DataEventType.AddNew:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }

        //        case BasicDAL.DataEventType.AddToDataSet:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }

        //        case BasicDAL.DataEventType.Binding:
        //            {
        //                break;
        //            }
        //        case BasicDAL.DataEventType.BindingFromDataReader:
        //            {
        //                break;
        //            }
        //        case BasicDAL.DataEventType.ControlsBinding:
        //            {
        //                break;
        //            }
        //        case BasicDAL.DataEventType.Delete:
        //        case BasicDAL.DataEventType.DeleteAll:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }
        //        case BasicDAL.DataEventType.DeleteFromDataSet:
        //        case BasicDAL.DataEventType.DeleteFromDataTable:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }
        //        case BasicDAL.DataEventType.Disposing:
        //            {
        //                break;
        //            }
        //        case BasicDAL.DataEventType.Initializing:
        //            {
        //                break;
        //            }
        //        case BasicDAL.DataEventType.Insert:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }
        //        case BasicDAL.DataEventType.MoveFirst:
        //        case BasicDAL.DataEventType.MoveLast:
        //        case BasicDAL.DataEventType.MoveNext:
        //        case BasicDAL.DataEventType.MovePrevious:
        //        case BasicDAL.DataEventType.MoveTo:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }
        //        case BasicDAL.DataEventType.Query:
        //            {
        //                DataGridListViewInit();
        //                break;
        //            }
        //        case BasicDAL.DataEventType.UndoChanges:
        //            {
        //                SetDataNavigator();
        //                break;
        //            }
        //        case BasicDAL.DataEventType.Update:
        //        case BasicDAL.DataEventType.UpdateFromDataSet:
        //        case BasicDAL.DataEventType.UpdateFromDataTable:
        //            {
        //                // Me._AddNewPending = True
        //                SetDataNavigator();
        //                break;
        //            }

        //        default:
        //            {
        //                break;
        //            }
        //            // Me._AddNewPending = False
        //    }

        //}



        //public void SetDataGridListView(BasicDAL.DbObject DbObject, DataGridView DataGridListView, bool Active = true)
        //{

        //    DataGridActive = false;
        //    this.DbObject = DbObject;
        //    this.DataGridListView = DataGridListView;
        //    this.DataGridListView.DataSource = this.DbObject.DataTable;
        //    DataGridListViewActive = Active;
        //    DataGridListViewInit();

        //}


        //public void SetDataGrid(BasicDAL.DbObject DbObject, DataGridView DataGrid, bool Active = true)
        //{
        //    this.DbObject = DbObject;
        //    this.DataGrid = DataGrid;
        //    DataGridActive = Active;
        //    DataGridListViewActive = false;
        //}

        //public void SetDataNavigator(BasicDAL.DbObject DbObject)
        //{

        //    DataGridActive = false;
        //    DataGridListViewActive = false;
        //    this.DbObject = DbObject;


        //}



        public void SetDataNavigator()
        {
            Accelerators = new Keys[] { _MoveFirstFKey, _MoveLastFKey, _MoveNextFKey, _MovePreviousFKey, _AddNewFKey, _DeleteFKey, _RefreshFKey, _FindKey, _UndoFKey, _SaveFKey, _CloseFKey };
            bool RowExist = false;
            if (DataGridActive)
            {
                switch (this._ActiveDataNavigatorViewModel.GridMode)
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

        private void _DbObject_BoundCompleted()
        {

            eBoundCompleted?.Invoke();
            _DataBoundCompleted = true;
        }


        private void _DataGridListView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (_DataGridListView.Focused)
            {
                try
                {
                    //_DbObject.MoveTo(Conversions.ToInteger(_DataGridListView.CurrentRow.Cells[_DataGridListViewRowIndexColumnName].Value));
                }
                catch (Exception ex)
                {

                }

            }

        }


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



            var hcol = new DataGridViewTextBoxColumn();
            ncolIndex = _DataGridListView.Columns.Add(hcol);
            _DataGridListViewRowIndexColumnIndex = ncolIndex;
            _DataGridListView.Columns[ncolIndex].Name = _DataGridListViewRowIndexColumnName;
            _DataGridListView.Columns[ncolIndex].DataPropertyName = _DataGridListViewRowIndexColumnName;
            _DataGridListView.Columns[ncolIndex].Visible = false;
            _DataGridListView.Columns[ncolIndex].ShowInVisibilityMenu = false;
           // _DataGridListViewDataTable = _DbObject.DataTable.Copy();
            _DataGridListViewDataTable.Columns.Add(_DataGridListViewRowIndexColumnName);

            for (int i = 0, loopTo = _DataGridListViewDataTable.Rows.Count - 1; i <= loopTo; i++)

                _DataGridListViewDataTable.Rows[i][_DataGridListViewRowIndexColumnName] = i;
            _DataGridListView.Columns[ncolIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            _DataGridListView.DataSource = _DataGridListViewDataTable;

        }

        private void DataNavigator_Accelerator(object sender, AcceleratorEventArgs e)
        {

        }

    }

    public class ListViewColumn
    {
        //private BasicDAL.DbColumn mDBColumn;
        private string mFriendlyName;
        private string mDisplayFormat;
        private DataGridListViewColumnType mColumnType;
        private int mWidth = 100;
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

    public enum DataGridListViewColumnType : int
    {
        Undefined = 0,
        TextBox = 1,
        CheckBox = 2,
        Image = 3,
        Link = 4
    }
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


        public ListViewColumn get_Item(int Index)
        {
            ListViewColumn ItemRet = default;
            ItemRet = (ListViewColumn)List[Index];
            return ItemRet;
        }

        public void set_Item(int Index, ListViewColumn value)
        {
            List[Index] = value;
        }


    }
}