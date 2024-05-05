using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using BasicDAL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Wisej.Web;

namespace BasicDALWisejControls
{

    public partial class QBEForm
    {


        private DbObject mDbObject = new DbObject();
        private DbObject mQueryDbObject;
        private DbObject mResultsDBObject;
        private DataTable mResultsDataTable;
        private QBEColumns mQBEColumns = new QBEColumns();
        private QbeMode mMode = QbeMode.Query;
        private ReportViewerMode mReportViewerMode = ReportViewerMode.PDFStream;
        private QBEResultMode mResultMode;
        private DbColumnsMapping mDbColumnsMapping = new DbColumnsMapping();
        private QBEBoundControls mBoundControls = new QBEBoundControls();
        private bool mAutoLoadAll = false;
        private Form mReportViewerMDIParent;
        private Dictionary<string, QBEReport> mReports = new Dictionary<string, QBEReport>();
        private Control mAfterCloseFocus;
        private int mTopRecords = 500;
        private Form mCallerForm;
        private int mReportCopies = 1;
        private QBEReport mDefaultReport = null;
        private bool mPrintDefaultReport = false;
        private string mReportsPath = Application.StartupPath + "reports";
        private string mReportsPDFUrlPath = @"\Reports\";
        private string mReportFileName = "";
        private string mReportName = "";
        private bool mUseExactSearchForString = false;
        public bool ParametersLoaded = false;
        private System.Drawing.Font mResultGridFont;

        public bool DoNotAllowQBEFilterChange = false;
        public bool PreserveDbFilters = false;
        public bool AllowQBEUserColumnsSetCreation = false;
        public int FormWidth = 800;
        public int FormHeight = 600;
        public object ResultGridRowHeight = 20;
        public object QueryGridRowHeight = 20;
        public object ReportGridRowHeight = 20;
        public string CrystalReportViewerQueryStringParameter = "ID";
        public string CrystalReportViewerPage = "BasicDALWisejCRViewer.aspx";
        public string CrystalReportViewerURL;
        public CXMLSession.SessionStore.StoreModes SessionStoreMode = CXMLSession.SessionStore.StoreModes.FileSystem;
        public string SessionStoreFileSystemStorePath = System.IO.Path.GetTempPath();
        public DataGridView BoundDataGridView = null;

        private string mReportSession = "";
        private string mReportsTempDir = "";
        private string mReportsLastPDFViewerFileName = "";
        private CXMLSession.SessionStore SessionStore = new CXMLSession.SessionStore();

        public event QBEForm_ResultReturnedEventHandler QBEForm_ResultReturned;

        public delegate void QBEForm_ResultReturnedEventHandler(ref DbObject ResultDbObject);
        public event QBEForm_ClosedEventHandler QBEForm_Closed;

        public delegate void QBEForm_ClosedEventHandler();
        public event QBEForm_ReportShowRequestEventHandler QBEForm_ReportShowRequest;

        public delegate void QBEForm_ReportShowRequestEventHandler(string ReportName, ref bool Cancel);
        public event QBEForm_ReportShowedEventHandler QBEForm_ReportShowed;

        public delegate void QBEForm_ReportShowedEventHandler(ReportDocument ReportDocument);
        private DbFiltersGroup oFiltersGroup = new DbFiltersGroup();

        private string _sep = "<p style='margin-top:2px;line-height:1.0;text-align:center;'>{0}<br>{1}<br>{2}</p>";

        private Dictionary<string, SortColumn> SortColumns = new Dictionary<string, SortColumn>();
        private Dictionary<string, SortColumn> SelectedSortColumns = new Dictionary<string, SortColumn>();


        private Keys _MovePreviousFKey = Keys.F6;
        private Keys _MoveNextFKey = Keys.F7;
        private Keys _MoveFirstFKey = (Keys)((int)Keys.Shift + (int)Keys.F6);
        private Keys _MoveLastFKey = (Keys)((int)Keys.Shift + (int)Keys.F7);
        private Keys _DeleteFKey = Keys.F3;
        private Keys _SaveFKey = Keys.F10;
        private Keys _RefreshFKey = Keys.F5;
        private Keys _CloseFKey = Keys.F12;
        private Keys _FindFKey = Keys.F4;
        private Keys _PrintFKey = Keys.F11;
        private bool _FKeyEnabled = false;


        private string _RecordLabelSeparator = "of";

        public string RecordLabelSeparator
        {
            get
            {
                return _RecordLabelSeparator;
            }
            set
            {
                _RecordLabelSeparator = value;

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
        public void UpdateButtonsCaption()
        {
            MoveFirstCaption = _MoveFirstCaption;
            MoveLastCaption = _MoveLastCaption;
            MoveNextCaption = _MoveNextCaption;
            MovePreviousCaption = _MovePreviousCaption;
            SelectCaption = _SelectCaption;
            DeleteFiltersCaption = _DeleteFiltersCaption;
            RefreshCaption = _RefreshCaption;
            PrintCaption = _PrintCaption;
            CloseCaption = _CloseCaption;
        }
        public Keys PrintFKey
        {
            get
            {
                return _PrintFKey;
            }
            set
            {
                _PrintFKey = value;
            }
        }
        // Public Property FindFKey() As Keys
        // Get
        // Return _FindFKey
        // End Get
        // Set(ByVal value As Keys)
        // _FindFKey = value
        // End Set
        // End Property

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



        public System.Drawing.Font ResultGridFont
        {
            get
            {
                return mResultGridFont;
            }
            set
            {
                if (value is not null)
                {
                    mResultGridFont = value;
                    ResultGrid.Font = value;
                }

            }
        }


        public string TabQueryFiltersText
        {
            get
            {

                return TabPageCriteriRicerca.Text;
            }
            set
            {
                // _TabQueryFiltersCaption = value
                TabPageCriteriRicerca.Text = value;
            }
        }
        public string TabExportText
        {
            get
            {
                return TabPageEsporta.Text;
            }
            set
            {
                // _TabQueryFiltersCaption = value
                TabPageEsporta.Text = value;
            }
        }
        public string TabReportsText
        {
            get
            {
                return TabPageStampe.Text;
            }
            set
            {

                TabPageStampe.Text = value;
            }
        }
        public string TabDebugText
        {
            get
            {
                return TabPageDebug.Text;
            }
            set
            {
                TabPageDebug.Text = value;
            }
        }
        public string ButtonExportDataText
        {
            get
            {
                return btnEsporta.Text;
            }
            set
            {
                btnEsporta.Text = value;
            }
        }


        public string QueryGridFilterColumnHeaderText
        {
            get
            {
                return dgvcNomeCampo.HeaderText;
            }
            set
            {
                dgvcNomeCampo.HeaderText = value;
            }
        }


        public string QueryGridFilterValueHeaderText
        {
            get
            {
                return dgvcValoreCampo.HeaderText;
            }
            set
            {
                dgvcValoreCampo.HeaderText = value;
            }
        }




        public bool UseExactSearchForString
        {
            get
            {
                return mUseExactSearchForString;
            }
            set
            {
                mUseExactSearchForString = value;
            }
        }

        public DataGridView QueryGridView
        {
            get
            {
                DataGridView QueryGridViewRet = default;
                QueryGridViewRet = QueryGrid;
                return QueryGridViewRet;
            }
            set
            {
                QueryGrid = value;
            }
        }

        public DataGridView ResultGridView
        {
            get
            {
                DataGridView ResultGridViewRet = default;
                ResultGridViewRet = ResultGrid;
                return ResultGridViewRet;
            }
            set
            {
                ResultGrid = value;
            }
        }
        public DataGridView ReportGridView
        {
            get
            {
                DataGridView ReportGridViewRet = default;
                ReportGridViewRet = ReportGrid;
                return ReportGridViewRet;
            }
            set
            {
                ReportGrid = value;
            }
        }

        public string QueryGridColumNameHeaderText
        {
            get
            {
                string QueryGridColumNameHeaderTextRet = default;
                QueryGridColumNameHeaderTextRet = QueryGrid.Columns[0].HeaderText;
                return QueryGridColumNameHeaderTextRet;
            }
            set
            {
                QueryGrid.Columns[0].HeaderText = value;
            }
        }

        public string QueryGridValueHeaderText
        {
            get
            {
                string QueryGridValueHeaderTextRet = default;
                QueryGridValueHeaderTextRet = QueryGrid.Columns[1].HeaderText;
                return QueryGridValueHeaderTextRet;
            }
            set
            {
                QueryGrid.Columns[1].HeaderText = value;
            }
        }
        public string ReportGridReportNameHeaderText
        {
            get
            {
                string ReportGridReportNameHeaderTextRet = default;
                ReportGridReportNameHeaderTextRet = ReportGrid.Columns[0].HeaderText;
                return ReportGridReportNameHeaderTextRet;
            }
            set
            {
                ReportGrid.Columns[0].HeaderText = value;
            }
        }
        public string ReportGridReportDescriptionHeaderText
        {
            get
            {
                string ReportGridReportDescriptionHeaderTextRet = default;
                ReportGridReportDescriptionHeaderTextRet = ReportGrid.Columns[1].HeaderText;
                return ReportGridReportDescriptionHeaderTextRet;
            }
            set
            {
                ReportGrid.Columns[1].HeaderText = value;
            }
        }
        public string ReportGridReportFileNameHeaderText
        {
            get
            {
                string ReportGridReportFileNameHeaderTextRet = default;
                ReportGridReportFileNameHeaderTextRet = ReportGrid.Columns[2].HeaderText;
                return ReportGridReportFileNameHeaderTextRet;
            }
            set
            {
                ReportGrid.Columns[2].HeaderText = value;
            }
        }

        public string UseExactSearchForStringCaption
        {
            get
            {
                string UseExactSearchForStringCaptionRet = default;
                UseExactSearchForStringCaptionRet = chkLikeOperator.Text;
                return UseExactSearchForStringCaptionRet;
            }
            set
            {
                chkLikeOperator.Text = value;
            }
        }

        public string SearchTabText
        {
            get
            {
                string SearchTabTextRet = default;
                SearchTabTextRet = TabPageCriteriRicerca.Text;
                return SearchTabTextRet;
            }
            set
            {
                TabPageCriteriRicerca.Text = value;
            }
        }

        public string ReportTabText
        {
            get
            {
                string ReportTabTextRet = default;
                ReportTabTextRet = TabPageStampe.Text;
                return ReportTabTextRet;
            }
            set
            {
                TabPageStampe.Text = value;
            }

        }
        private string _MoveFirstCaption;

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
                bFirst.Text = Conversions.ToString(ButtonCaption(value, _MoveFirstFKey));
            }

        }
        public string MoveFirstToolTipText
        {
            get
            {
                string MoveFirstToolTipTextRet = default;
                MoveFirstToolTipTextRet = bFirst.ToolTipText;
                return MoveFirstToolTipTextRet;
            }
            set
            {
                bFirst.ToolTipText = value;
            }
        }
        private string _MoveLastCaption;
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
        public string MoveLastToolTipText
        {
            get
            {
                string MoveLastToolTipTextRet = default;
                MoveLastToolTipTextRet = bLast.ToolTipText;
                return MoveLastToolTipTextRet;
            }
            set
            {

                bLast.ToolTipText = value;
            }
        }
        private string _MovePreviousCaption;
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
        public string MovePreviousToolTipText
        {
            get
            {
                string MovePreviousToolTipTextRet = default;
                MovePreviousToolTipTextRet = bPrev.ToolTipText;
                return MovePreviousToolTipTextRet;
            }
            set
            {

                bPrev.ToolTipText = value;
            }
        }
        private string _MoveNextCaption;
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
        public string MoveNextToolTipText
        {
            get
            {
                string MoveNextToolTipTextRet = default;
                MoveNextToolTipTextRet = bNext.ToolTipText;
                return MoveNextToolTipTextRet;
            }
            set
            {
                bNext.ToolTipText = value;
            }
        }

        private string _RefreshCaption;
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
        public string RefreshToolTipText
        {
            get
            {
                string RefreshToolTipTextRet = default;
                RefreshToolTipTextRet = bRefresh.ToolTipText;
                return RefreshToolTipTextRet;
            }
            set
            {
                bRefresh.ToolTipText = value;
            }
        }

        private string _DeleteFiltersCaption;
        public string DeleteFiltersCaption
        {
            get
            {
                string DeleteFiltersCaptionRet = default;
                DeleteFiltersCaptionRet = _DeleteFiltersCaption;
                return DeleteFiltersCaptionRet;
            }
            set
            {
                _DeleteFiltersCaption = value;
                bDelete.Text = Conversions.ToString(ButtonCaption(value, _DeleteFKey));
            }
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

        public string DeleteFiltersToolTipText
        {
            get
            {
                string DeleteFiltersToolTipTextRet = default;
                DeleteFiltersToolTipTextRet = bDelete.ToolTipText;
                return DeleteFiltersToolTipTextRet;
            }
            set
            {
                bDelete.ToolTipText = value;
            }
        }

        private string _PrintCaption;
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
                bPrint.Text = Conversions.ToString(ButtonCaption(value, _PrintFKey));

            }
        }
        public string PrintToolTipText
        {
            get
            {
                string PrintToolTipTextRet = default;
                PrintToolTipTextRet = bPrint.ToolTipText;
                return PrintToolTipTextRet;
            }
            set
            {
                bPrint.ToolTipText = value;
            }
        }
        private string _SelectCaption;
        public string SelectCaption
        {
            get
            {
                string SelectCaptionRet = default;
                SelectCaptionRet = _SelectCaption;
                return SelectCaptionRet;
            }
            set
            {
                _SelectCaption = value;
                bSave.Text = Conversions.ToString(ButtonCaption(value, _SaveFKey));
            }
        }
        public string SelectToolTipText
        {
            get
            {
                string SelectToolTipTextRet = default;
                SelectToolTipTextRet = bSave.ToolTipText;
                return SelectToolTipTextRet;
            }
            set
            {
                bSave.ToolTipText = value;
            }
        }
        private string _CloseCaption;
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
        public string CloseToolTipText
        {
            get
            {
                string CloseToolTipTextRet = default;
                CloseToolTipTextRet = bClose.ToolTipText;
                return CloseToolTipTextRet;
            }
            set
            {
                bClose.ToolTipText = value;
            }
        }
        // Protected Overrides ReadOnly Property CreateParams() As Windows.Forms.CreateParams
        // Get
        // Dim cp As Windows.Forms.CreateParams = MyBase.CreateParams
        // cp.ExStyle = cp.ExStyle Or &H2000000
        // Return cp
        // End Get
        // End Property
        public string ReportsPath
        {

            get
            {
                string ReportsPathRet = default;
                ReportsPathRet = mReportsPath;
                return ReportsPathRet;
            }
            set
            {
                mReportsPath = value;
            }
        }

        public string ReportsPDFUrlPath
        {
            get
            {
                string ReportsPDFUrlPathRet = default;
                ReportsPDFUrlPathRet = mReportsPDFUrlPath;
                return ReportsPDFUrlPathRet;
            }
            set
            {
                mReportsPDFUrlPath = value;
            }
        }

        public bool PrintDefaultReport
        {
            get
            {
                bool PrintDefaultReportRet = default;
                PrintDefaultReportRet = mPrintDefaultReport;
                return PrintDefaultReportRet;
            }
            set
            {
                mPrintDefaultReport = value;
            }

        }

        public QBEReport DefaultReport
        {
            get
            {
                QBEReport DefaultReportRet = default;
                DefaultReportRet = mDefaultReport;
                return DefaultReportRet;
            }
            set
            {
                mDefaultReport = value;
            }

        }


        public int ReportCopies
        {
            get
            {
                int ReportCopiesRet = default;
                ReportCopiesRet = mReportCopies;
                return ReportCopiesRet;
            }
            set
            {
                mReportCopies = value;
            }

        }
        public Form CallerForm
        {
            get
            {
                Form CallerFormRet = default;
                CallerFormRet = mCallerForm;
                return CallerFormRet;
            }
            set
            {
                mCallerForm = value;
            }
        }
        public int TopRecords
        {
            get
            {
                int TopRecordsRet = default;
                TopRecordsRet = mTopRecords;
                return TopRecordsRet;
            }
            set
            {
                mTopRecords = value;
            }
        }
        public Control AfterCloseFocus
        {
            get
            {
                Control AfterCloseFocusRet = default;
                AfterCloseFocusRet = mAfterCloseFocus;
                return AfterCloseFocusRet;
            }
            set
            {
                mAfterCloseFocus = value;
            }
        }
        // Property Reports() As QBEReports
        // Get
        // Reports = mReports
        // End Get
        // Set(ByVal value As QBEReports)
        // mReports = value
        // End Set
        // End Property

        public Dictionary<string, QBEReport> Reports
        {
            get
            {
                Dictionary<string, QBEReport> ReportsRet = default;
                ReportsRet = mReports;
                return ReportsRet;
            }
            set
            {
                mReports = value;
            }
        }

        public ReportViewerMode ReportViewerMode
        {
            get
            {
                ReportViewerMode ReportViewerModeRet = default;
                ReportViewerModeRet = mReportViewerMode;
                return ReportViewerModeRet;
            }
            set
            {
                mReportViewerMode = value;
            }
        }


        public Form ReportViewerMDIParent
        {
            get
            {
                Form ReportViewerMDIParentRet = default;
                ReportViewerMDIParentRet = mReportViewerMDIParent;
                return ReportViewerMDIParentRet;
            }
            set
            {
                mReportViewerMDIParent = value;
            }
        }


        public QBEColumns QBEColumns
        {
            get
            {
                QBEColumns QBEColumnsRet = default;
                QBEColumnsRet = mQBEColumns;
                return QBEColumnsRet;
            }
            set
            {
                mQBEColumns = value;
            }
        }
        public bool AutoLoadAll
        {
            get
            {
                bool AutoLoadAllRet = default;
                AutoLoadAllRet = mAutoLoadAll;
                return AutoLoadAllRet;
            }
            set
            {
                mAutoLoadAll = value;
            }
        }
        public QBEBoundControls BoundControls
        {
            get
            {
                QBEBoundControls BoundControlsRet = default;
                BoundControlsRet = mBoundControls;
                return BoundControlsRet;

            }
            set
            {
                mBoundControls = value;
            }
        }

        public DbColumnsMapping DbColumnsMapping
        {
            get
            {
                DbColumnsMapping DbColumnsMappingRet = default;
                DbColumnsMappingRet = mDbColumnsMapping;
                return DbColumnsMappingRet;
            }
            set
            {

            }
        }

        public QBEResultMode ResultMode
        {
            get
            {
                QBEResultMode ResultModeRet = default;
                ResultModeRet = mResultMode;
                return ResultModeRet;
            }
            set
            {
                mResultMode = value;

            }
        }

        public DbObject ResultsDbObject
        {
            get
            {
                DbObject ResultsDbObjectRet = default;
                ResultsDbObjectRet = mResultsDBObject;
                return ResultsDbObjectRet;

            }
            set
            {
                mResultsDBObject = value;

            }
        }

        public DataTable ResultsDataTable
        {
            get
            {
                DataTable ResultsDataTableRet = default;
                ResultsDataTableRet = mResultsDataTable;
                return ResultsDataTableRet;

            }
            set
            {
                mResultsDataTable = value;

            }
        }


        public DbObject QueryDbObject
        {
            get
            {
                DbObject QueryDbObjectRet = default;
                QueryDbObjectRet = mQueryDbObject;
                return QueryDbObjectRet;

            }
            set
            {
                mQueryDbObject = value;

            }
        }
        public string Title
        {
            get
            {
                string TitleRet = default;
                TitleRet = Text;
                return TitleRet;

            }
            set
            {
                Text = value;

            }
        }


        public QbeMode Mode
        {
            get
            {
                QbeMode ModeRet = default;
                ModeRet = mMode;
                return ModeRet;
            }
            set
            {
                mMode = value;
            }
        }

        public DbObject DbObject
        {
            get
            {
                DbObject DbObjectRet = default;
                DbObjectRet = mDbObject;
                return DbObjectRet;
            }
            set
            {
                mDbObject = value;
            }
        }



        private void CalcolaSplitterDistance()
        {

            SplitContainer1.SplitterDistance = (int)Math.Round(SplitContainer1.Height * 30 / 100d);
        }

        public bool AddQBEFormReportsFromFile(string ReportsFile)
        {

            bool OK = false;
            var QBEReports = new List<QBEReport>();

            if (System.IO.File.Exists(ReportsFile))
            {
                using (var MyReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(ReportsFile))
                {
                    MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    MyReader.SetDelimiters(";");
                    string[] currentRow;
                    while (!MyReader.EndOfData)
                    {
                        try
                        {
                            currentRow = MyReader.ReadFields();
                            if (currentRow.Length == 3)
                            {

                                var QBEReport = new QBEReport();
                                QBEReport.ReportTitle = currentRow[0];
                                QBEReport.ReportDescription = currentRow[1];
                                QBEReport.ReportFileName = currentRow[2];

                                if (System.IO.Path.IsPathRooted(QBEReport.ReportFileName) == true)
                                {
                                    if (System.IO.File.Exists(QBEReport.ReportFileName))
                                    {
                                        if (QBEReports.Contains(QBEReport) == false)
                                        {
                                            QBEReports.Add(QBEReport);
                                        }
                                    }
                                }
                                else if (QBEReports.Contains(QBEReport) == false)
                                {
                                    QBEReports.Add(QBEReport);
                                }
                            }
                        }
                        catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                        {
                            // MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                        }
                    }
                }
            }

            if (QBEReports.Count == 0)
            {
                return false;
            }
            else
            {

                foreach (QBEReport QBEReport in QBEReports)
                    AddReport(QBEReport.ReportTitle, QBEReport.ReportFileName, QBEReport.ReportDescription, QBEReport.DbObject, QBEReport.ReportUseLike);
                return true;
            }

        }


        public QBEReport AddReport(string ReportTitle, string ReportFileName, string ReportDescription, DbObject DbObject = null, bool ReportUseLike = true)
        {
            var x = new QBEReport();
            x.ReportDescription = ReportDescription;
            x.ReportFileName = ReportFileName;
            x.ReportTitle = ReportTitle;
            x.ReportUseLike = ReportUseLike;
            if (DbObject is null)
            {
                x.DbObject = mDbObject;
            }
            else
            {
                x.DbObject = DbObject;
            }

            if (mReports.ContainsKey(ReportTitle) == false)
            {
                mReports.Add(ReportTitle, x);
                return x;
            }
            else
            {
                return null;
            }

        }

        public void ShowQBE(Form CallerForm = null, bool Modal = false)
        {

            int x = 0;

            Accelerators = new Keys[] { _MoveFirstFKey, _MoveLastFKey, _MoveNextFKey, _MovePreviousFKey, _DeleteFKey, _RefreshFKey, _SaveFKey, _CloseFKey };


            if (CallerForm is not null)
            {
                mCallerForm = CallerForm;
                mCallerForm.Enabled = false;
            }

            if (mCallerForm is not null)
            {
                if (mCallerForm.MdiParent is not null)
                {
                    MdiParent = mCallerForm.MdiParent;
                }
            }
            if (Icon is null & this.CallerForm is not null)
            {
                Icon = this.CallerForm.Icon;
            }


            try
            {
                SetTopLevel(true);
            }
            catch (Exception ex)
            {

            }


            if (mMode == QbeMode.Query)
            {
                TabPageDebug.Hidden = true;
                TabPageEsporta.Hidden = false;
                TabPageStampe.Hidden = true;
                PanelEsporta.Dock = DockStyle.Fill;
                TabPageOrdinamento.Hidden = true;

            }


            if (mMode == QbeMode.Report)
            {
                IconSource = "icon-print";
                TabPageStampe.Hidden = false;
                TabPageDebug.Hidden = false;
                TabPageEsporta.Hidden = false;
                TabPageOrdinamento.Hidden = false;
                PanelEsporta.Dock = DockStyle.Fill;
                if (mReportViewerMode != ReportViewerMode.WEB)
                {
                    txtDebug.Dock = DockStyle.Fill;
                }
                else
                {

                }

                if (mReports.Count == 0)
                {
                    string _text = "";
                    if (mCallerForm is not null)
                    {
                        _text = mCallerForm.Text;
                    }
                    MessageBox.Show("Non ci sono Reports associati!", _text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mCallerForm.Enabled = true;
                    return;
                }
            }


            StartPosition = FormStartPosition.CenterScreen;
            ResultGrid.AutoGenerateColumns = false;
            ResultGrid.RowTemplate.Height = Conversions.ToInteger(ResultGridRowHeight);
            QueryGrid.RowTemplate.Height = Conversions.ToInteger(QueryGridRowHeight);
            ReportGrid.RowTemplate.Height = Conversions.ToInteger(ReportGridRowHeight);
            ResultGrid.DefaultRowHeight = Conversions.ToInteger(ResultGridRowHeight);
            QueryGrid.DefaultRowHeight = Conversions.ToInteger(QueryGridRowHeight);
            ReportGrid.DefaultRowHeight = Conversions.ToInteger(ReportGridRowHeight);



            chkLikeOperator.Checked = !UseExactSearchForString;
            bSaveQBE.Visible = AllowQBEUserColumnsSetCreation;
            bLoadQBE.Visible = AllowQBEUserColumnsSetCreation;


            if (ParametersLoaded == false)
            {
                LoadParameters();
            }


            if (Mode == QbeMode.Report)
            {
                if (DefaultReport is null)
                {
                    DefaultReport = Reports.FirstOrDefault().Value;
                }
                mDbObject = DefaultReport.DbObject;



                LoadReportParameters(DefaultReport.ReportTitle);
                txtReportTitle.Text = DefaultReport.ReportTitle;
                txtReportDescription.Text = DefaultReport.ReportDescription;
            }

            Records.Text = mTopRecords.ToString();


            SetControlsSize();
            // ----------------------------------------------
            if (mMode == QbeMode.Report)
            {
                Records.Visible = false;
                RecordLabel.Visible = false;

            }

            if (Mode == QbeMode.Query)
            {

                oFiltersGroup = mDbObject.FiltersGroup;
                if (string.IsNullOrEmpty(mDbObject.OrderBy))
                {
                    mDbObject.OrderBy = OrderByRules();
                }
                BuildQuery3();
                if (mAutoLoadAll == true)
                {
                    mDbObject.DoQuery();
                    ResultGrid.DataSource = mDbObject.GetDataTable();
                }
            }



            if (Mode == QbeMode.Report)
            {

                // Me.mReportSession = System.IO.Path.GetRandomFileName()
                mReportSession = Guid.NewGuid().ToString();
                PanelReportInfo.Visible = true;
                PanelReportViewer.Visible = true;
                PanelReportViewer.Dock = DockStyle.Fill;

                oFiltersGroup = mDbObject.FiltersGroup;
                if (string.IsNullOrEmpty(mDbObject.OrderBy))
                {
                    mDbObject.OrderBy = OrderByRules();
                }
                BuildQuery3();
                mDbObject.DoQuery();


                int i;
                var loopTo = ReportGrid.RowCount - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(UCase(ReportGrid.Rows[i].Cells[0].Value), Strings.UCase(DefaultReport.ReportFileName), false)))
                    {
                        mReportFileName = Conversions.ToString(ReportGrid.Rows[i].Cells[2].Value);
                        mReportName = Conversions.ToString(ReportGrid.Rows[i].Cells[0].Value);
                        break;
                    }
                }

                if (mPrintDefaultReport == true)
                {
                    if (!string.IsNullOrEmpty(mReportFileName))
                    {
                        switch (ReportViewerMode)
                        {
                            case ReportViewerMode.WEB:
                                {
                                    ShowReportWEB(Reports[mReportName]);
                                    break;
                                }
                            case ReportViewerMode.PDFStream:
                            case ReportViewerMode.PDFUrl:
                                {
                                    ShowReportPDF(Reports[mReportName]);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }
                    }
                }

            }

            if (Modal == true)
            {
                ShowDialog();
            }

            Visible = true;
            try
            {
                SetTopLevel(true);
            }
            catch (Exception ex)
            {

            }

            Select();




        }

        private void PrintReport(bool Print)
        {



            if (ReportGrid.CurrentRow is null)
            {
                return;
            }

            string ReportFileName = mReportFileName;
            string ReportName = mReportName;

            bool Cancel = false;


            if (ReportGrid.CurrentRow is null & string.IsNullOrEmpty(DefaultReport.ReportFileName))
                return;

            if (ReportGrid.CurrentRow is not null)
            {
                ReportFileName = Conversions.ToString(ReportGrid.CurrentRow.Tag);
                ReportName = Conversions.ToString(ReportGrid.CurrentRow.Cells[0].Value);
            }


            if (!string.IsNullOrEmpty(ReportFileName.Trim()))
            {

                QBEForm_ReportShowRequest?.Invoke(ReportFileName, ref Cancel);
                if (Cancel == false)
                {
                    switch (ReportViewerMode)
                    {
                        case ReportViewerMode.PDFUrl:
                        case ReportViewerMode.PDFStream:
                            {
                                ShowReportPDF(Reports[ReportName]);
                                break;
                            }
                        case ReportViewerMode.WEB:
                            {
                                ShowReportWEB(Reports[ReportName]);
                                break;
                            }
                    }

                    if (Print == true)
                    {
                        // Me.PdfViewer.Select()
                        // Me.PdfViewer.Focus()

                    }
                }

            }


        }
        public void ShowReportWEB(QBEReport Report)
        {

            var ShowReportRequest = new ShowReportRequest();
            string URL = "";
            string GUID = Guid.NewGuid().ToString();

            string TipoSessione = "SESS";
            // Me.WebBrowser.Dock = DockStyle.Fill
            // Me.WebBrowser .Height = Me.SplitContainer1.Panel1.Height

            AspNetPanel.Dock = DockStyle.Fill;
            AspNetPanel.Height = SplitContainer1.Panel1.Height;

            ShowReportRequest.GUID = GUID;
            ShowReportRequest.ReportFileName = ReportsPath + @"\" + Report.ReportFileName;
            ShowReportRequest.ServerName = DbObject.DbConfig.ServerName;
            ShowReportRequest.DataBaseName = DbObject.DbConfig.DataBaseName;
            ShowReportRequest.UserName = DbObject.DbConfig.UserName;
            ShowReportRequest.Password = DbObject.DbConfig.Password;
            ShowReportRequest.RecordSelectionFormula = GetCrystalRecordSelectionFormula();
            ShowReportRequest.AuthenticationMode = DbObject.DbConfig.AuthenticationMode;
            // .ViewerHeight = Me.WebBrowser.Height
            ShowReportRequest.ViewerHeight = AspNetPanel.Height;

            switch (TipoSessione ?? "")
            {

                case "CXML":
                    {
                        SessionStore.StoreMode = SessionStoreMode;
                        SessionStore.StoreMode = CXMLSession.SessionStore.StoreModes.MemoryMappedFile;
                        SessionStore.FileSystemStorePath = SessionStoreFileSystemStorePath;
                        SessionStore.ID = GUID;
                        SessionStore.Namespace = "BasicDALWisejCRViewer";
                        SessionStore.AddObject<ShowReportRequest>("ShowReportRequest", ShowReportRequest);
                        SessionStore.Write();
                        URL = CrystalReportViewerURL + "?" + CrystalReportViewerQueryStringParameter + "=CXML_" + GUID;
                        break;
                    }

                case "SESS":
                    {
                        URL = CrystalReportViewerURL + "?" + CrystalReportViewerQueryStringParameter + "=SESS_" + GUID;
                        Application.Session(GUID) = ShowReportRequest;
                        break;
                    }

            }

            // Me.WebBrowser.Url = New Uri(URL)
            // Me.WebBrowser .Visible =true
            AspNetPanel.Url = URL;
            AspNetPanel.Visible = true;

        }


        public static string SerializeObjectToString<T>(T toSerialize)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(toSerialize.GetType());
            using (var textWriter = new System.IO.StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static string DeSerializeStringToObject<T>(string toDeserialize)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(toDeserialize.GetType());
            using (var textWriter = new System.IO.StringWriter())
            {
                var reader = new System.IO.StreamReader(toDeserialize);
                xmlSerializer.Deserialize(reader);
                return textWriter.ToString();
            }
        }



        // Public Sub ShowReportPDF(ByVal ReportName As String, ByVal FileReportName As String)
        public void ShowReportPDF(QBEReport Report)
        {


            string _ReportsPath = "";
            var objReport = new ReportDocument();
            string ReportFileName = ""; // Application.StartupPath & ReportsPath & Report.ReportFileName
            string ReportMsg = "";
            string ErrMsg2 = "";
            string Where = "";
            string RecordSelectionFormula = "";
            string RecordOrderby = "";
            var Debug = new System.Text.StringBuilder();

            txtDebug.Dock = DockStyle.Fill;

            if (System.IO.Path.IsPathRooted(ReportsPath) == true)
            {
                _ReportsPath = ReportsPath;
            }
            else
            {
                _ReportsPath = Application.StartupPath + ReportsPath;
            }

            // controllare per Report.Filename PATHROOTED

            if (System.IO.Directory.Exists(_ReportsPath))
            {
                ReportFileName = _ReportsPath + @"\" + Report.ReportFileName;
            }
            else
            {
                ReportMsg = "Report: " + Report.ReportTitle + Constants.vbCrLf + "ReportFile: " + Report.ReportFileName + Constants.vbCrLf;

                MessageBox.Show(ReportMsg + ". ReportPath did not exist!", Text, (MessageBoxButtons)MessageBoxIcon.Error);
                return;
            }

            Debug.AppendLine("ReporViewertMode: " + ReportViewerMode.ToString());
            Debug.AppendLine("Report: " + Report.ReportTitle);
            Debug.AppendLine("ReportFile: " + ReportFileName);

            ReportMsg = "Report: " + Report.ReportTitle + Constants.vbCrLf + "ReportFile: " + ReportFileName + Constants.vbCrLf;

            if (System.IO.File.Exists(ReportFileName) == false)
            {
                MessageBox.Show(ReportMsg + ". The report file did not exist!", Text, (MessageBoxButtons)MessageBoxIcon.Error);
                return;
            }

            try
            {
                objReport.Load(ReportFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ReportMsg + Constants.vbCrLf + "Error loading report: " + ex.Message, Text, (MessageBoxButtons)MessageBoxIcon.Error);
                return;
            }


            // Wisej.Web.MessageBox.Show(ReportMsg & ". ConnectionInfo", Me.Text)
            var connectionInfo = new ConnectionInfo();
            connectionInfo.DatabaseName = DbObject.DbConfig.DataBaseName;
            connectionInfo.ServerName = DbObject.DbConfig.ServerName;

            // Wisej.Web.MessageBox.Show(ReportMsg & ". Authetication", Me.Text)
            if (DbObject.DbConfig.AuthenticationMode == 1)
            {
                connectionInfo.IntegratedSecurity = true;
            }
            else
            {
                connectionInfo.IntegratedSecurity = false;
                connectionInfo.UserID = DbObject.DbConfig.UserName;
                connectionInfo.Password = DbObject.DbConfig.Password;
                // objReport.SetDatabaseLogon(dbConfig.UserName, dbConfig.Password)
            }

            var crtableLogoninfos = new TableLogOnInfos();
            var crtableLogoninfo = new TableLogOnInfo();
            var CrTables = objReport.Database.Tables;

            // Wisej.Web.MessageBox.Show(ReportMsg & ". Authentication CrTables", Me.Text)
            // First we assign the connection to all tables in the main report
            try
            {
                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = connectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ReportMsg + Constants.vbCrLf + "Error on DB connection setup: " + ex.Message, Text, (MessageBoxButtons)MessageBoxIcon.Error);
                return;
            }

            // Now loop through all the sections and its objects to do the same for the subreports
            // 
            // Wisej.Web.MessageBox.Show(ReportMsg & ". Authentication Subreports", Me.Text)
            try
            {
                foreach (Section section in objReport.ReportDefinition.Sections)
                {
                    // In each section we need to loop through all the reporting objects
                    foreach (ReportObject reportObject in section.ReportObjects)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subReport = (SubreportObject)reportObject;
                            var subDocument = subReport.OpenSubreport(subReport.SubreportName);
                            var xcrtableLogoninfos = new TableLogOnInfos();
                            var xcrtableLogoninfo = new TableLogOnInfo();
                            var xCrTables = objReport.Database.Tables;
                            // First we assign the connection to all tables in the main report
                            foreach (Table xCrTable in CrTables)
                            {
                                xcrtableLogoninfo = xCrTable.LogOnInfo;
                                xcrtableLogoninfo.ConnectionInfo = connectionInfo;
                                xCrTable.ApplyLogOnInfo(crtableLogoninfo);
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ReportMsg + Constants.vbCrLf + "Error on DB connection setup for subreport: " + ex.Message, Text, (MessageBoxButtons)MessageBoxIcon.Error);
                return;
            }


            try
            {
                SetCrystalReportSortFields(ref objReport);

                ErrMsg2 = "GetCrystalRecordSelectionFormula";
                RecordSelectionFormula = GetCrystalRecordSelectionFormula();
                if (!string.IsNullOrEmpty(objReport.RecordSelectionFormula))
                {
                    RecordSelectionFormula = RecordSelectionFormula + " And " + objReport.RecordSelectionFormula;
                }
                ErrMsg2 = "Set RecordSelectionFormula";
                objReport.RecordSelectionFormula = RecordSelectionFormula + " " + RecordOrderby;

                ErrMsg2 = " objReport.Refresh()";
                objReport.SummaryInfo.ReportTitle = Report.ReportTitle;
                objReport.SummaryInfo.ReportSubject = Report.ReportDescription;
                objReport.Refresh();

                switch (mReportViewerMode)
                {

                    case ReportViewerMode.PDFUrl:
                        {


                            ErrMsg2 = "objReport.ExportToDisk()";
                            string ReportsPDFUrlPath = mReportsPDFUrlPath;
                            string LocalReportTempDir = Application.MapPath(ReportsPDFUrlPath);
                            LocalReportTempDir = LocalReportTempDir.Replace("/", @"\");
                            Debug.AppendLine("ReportsSitePath = " + ReportsPDFUrlPath);
                            Debug.AppendLine("LocalReportTempDir = " + LocalReportTempDir);

                            string PDFViewerFileName = LocalReportTempDir + @"\" + BasicDAL.Utilities.FileHelper.GetSafeFileName(Report.ReportTitle + ".pdf");
                            PDFViewerFileName = BasicDAL.Utilities.FileHelper.GetUniqueFileName(PDFViewerFileName);
                            objReport.ExportToDisk(ExportFormatType.PortableDocFormat, PDFViewerFileName);
                            if (System.IO.File.Exists(mReportsLastPDFViewerFileName))
                            {
                                System.IO.File.Delete(mReportsLastPDFViewerFileName);
                            }
                            mReportsLastPDFViewerFileName = PDFViewerFileName;
                            string PDFViewerFileNameURL = BasicDAL.Utilities.GetURLWithoutFileName(Application.Url) + ReportsPDFUrlPath + "/" + System.Web.HttpUtility.HtmlEncode(BasicDAL.Utilities.FileHelper.GetFileName(PDFViewerFileName));
                            Debug.AppendLine("PDFViewerFileNameURL = " + PDFViewerFileNameURL);

                            PdfViewer.PdfSource = PDFViewerFileNameURL;
                            mReportsTempDir = LocalReportTempDir;
                            PdfViewer.Visible = true;
                            break;
                        }


                    case ReportViewerMode.PDFStream:
                        {

                            // Dim ExportOptions As New ExportOptions
                            // Dim PDFFormatOptions As PdfFormatOptions = ExportOptions.CreatePdfFormatOptions()

                            ErrMsg2 = "objReport.ExportToStream()";
                            PdfViewer.ViewerType = PdfViewerType.Auto;
                            PdfViewer.PdfStream = objReport.ExportToStream(ExportFormatType.PortableDocFormat);
                            Debug.AppendLine("PDFViewer.PdfStream.Lenght = " + PdfViewer.PdfStream.Length);

                            PdfViewer.Visible = true;
                            break;
                        }

                    default:
                        {
                            return;
                        }
                }
                txtDebug.Text = Debug.ToString();
            }


            catch (Exception ex)
            {
                MessageBox.Show(ReportMsg + Constants.vbCrLf + ErrMsg2 + Constants.vbCrLf + "Error loading data!" + ex.Message + Constants.vbCrLf + ex.StackTrace, Text, (MessageBoxButtons)MessageBoxIcon.Error);
                return;
            }




        }



        private string OrderByRules()
        {

            string OrderBy = "";

            foreach (QBEColumn Col in QBEColumns)
            {
                // If Col.DisplayInQBEResult And Col.DbCOlumn.IsSortable() = True Then
                // OrderBy = OrderBy & Col.DbCOlumn.DbColumnName & ", "
                // End If

                if (Col.DisplayInQBEResult & Col.DbCOlumn.IsPrimaryKey == true)
                {
                    OrderBy = OrderBy + Col.DbCOlumn.DbColumnName + ", ";
                }
            }

            OrderBy = OrderBy.Trim();

            if (!string.IsNullOrEmpty(OrderBy))
            {
                OrderBy = Strings.Mid(OrderBy, 1, OrderBy.Length - 1);
            }

            return OrderBy;

        }

        public void LoadParameters()
        {
            var DbColumns = new DbColumns();

            int i = 0;
            int x = 0;
            int RowCount = 100;
            int charw = 0;
            int cellw = 0;

            // If Me.Icon Is Nothing And Me.CallerForm IsNot Nothing Then
            // Me.Icon = Me.CallerForm.Icon
            // End If

            TabControl.Dock = DockStyle.Top;
            ReportGrid.Visible = false;
            ResultGrid.Columns.Clear();
            QueryGrid.Rows.Clear();

            switch (mMode)
            {
                case var @case when @case == QbeMode.Report:
                    {
                        bPrint.Visible = true;
                        ResultGrid.Visible = false;
                        bSave.Visible = false;
                        break;
                    }

                default:
                    {

                        TabPageCriteriRicerca.Visible = true;
                        bPrint.Visible = false;
                        ResultGrid.Visible = true;
                        ResultGrid.Dock = DockStyle.Fill;
                        PdfViewer.Visible = false;
                        break;
                    }
            }

            RowCount = mQBEColumns.Count;

            if (ResultMode == QBEResultMode.SingleRow)
            {
                ResultGrid.MultiSelect = false;
            }
            else
            {
                ResultGrid.MultiSelect = true;
            }


            QueryGrid.Columns[dgvcNomeCampo.Name].ReadOnly = true;
            QueryGrid.Columns[dgvcNomeCampo.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            QueryGrid.Columns[dgvcValoreCampo.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            QueryGrid.Columns[dgvcQueryCampo.Name].Visible = false;

            // controllo per verifica collection QBEColumns
            if (mQBEColumns.Count == 0)
            {
                mQBEColumns.Clear();
                DbColumns = mDbObject.GetDbColumns();
                foreach (var DbColumn in DbColumns)
                    mQBEColumns.Add(DbColumn, DbColumn.FriendlyName, DbColumn.DisplayFormat, DbColumn.QBEValue, DbColumn.UseInQBE, DbColumn.DisplayInQBEResult);
            }


            double QueryColumnWidth = 0d;
            foreach (QBEColumn _QBEColumn in mQBEColumns)
            {
                // For Each entry As DictionaryEntry In Me.mQBEColumns
                // Dim _QBEColumn As QBEColumn = entry.Value

                if (mMode == QbeMode.Query)
                {

                    if (_QBEColumn.DbCOlumn == null == true)
                    {
                        _QBEColumn.UseInQBE = false;

                        switch (_QBEColumn.QBEColumnType)
                        {

                            case var case1 when case1 == DbType.Boolean:
                                {

                                    var ncol = new DataGridViewCheckBoxColumn();
                                    ncol.Name = Guid.NewGuid().ToString();
                                    ncol.HeaderText = _QBEColumn.FriendlyName;
                                    x = ResultGrid.Columns.Add(ncol);
                                    break;
                                }

                            default:
                                {

                                    var ncol = new DataGridViewTextBoxColumn();
                                    ncol.Name = Guid.NewGuid().ToString();
                                    ncol.HeaderText = _QBEColumn.FriendlyName;
                                    x = ResultGrid.Columns.Add(ncol);
                                    break;
                                }

                        }
                    }
                    else
                    {
                        // modifica per tipo colonne diverse in QBEResult
                        switch (_QBEColumn.DbCOlumn.DbType)
                        {
                            case var case2 when case2 == DbType.Boolean:
                                {
                                    var ncol = new DataGridViewCheckBoxColumn();
                                    ncol.Name = _QBEColumn.DbCOlumn.DbColumnName;
                                    ncol.HeaderText = _QBEColumn.FriendlyName;
                                    x = ResultGrid.Columns.Add(ncol);
                                    break;
                                }

                            default:
                                {

                                    var ncol = new DataGridViewTextBoxColumn();
                                    ncol.Name = _QBEColumn.DbCOlumn.DbColumnName;
                                    ncol.HeaderText = _QBEColumn.FriendlyName;
                                    x = ResultGrid.Columns.Add(ncol);
                                    break;
                                }

                        }
                        ResultGrid.Columns[x].Visible = false;

                        if (DbObject.DbObjectType == DbObjectTypeEnum.Join)
                        {
                            ResultGrid.Columns[x].DataPropertyName = _QBEColumn.DbCOlumn.DbColumnNameAlias;
                        }
                        else
                        {
                            ResultGrid.Columns[x].DataPropertyName = _QBEColumn.DbCOlumn.DbColumnNameE;
                        }

                        if (_QBEColumn.DisplayInQBEResult == true)
                        {

                            {
                                var withBlock = ResultGrid.Columns[x];
                                // modifica per export
                                withBlock.Tag = _QBEColumn.DbCOlumn.Name;
                                // ------
                                withBlock.Visible = true;
                                charw = 8;
                                switch (_QBEColumn.DbCOlumn.DbType)
                                {
                                    case var case3 when case3 == DbType.Date:
                                    case DbType.DateTime:
                                    case DbType.Time:
                                        {
                                            switch (_QBEColumn.ColumnWidth)
                                            {
                                                case var case4 when case4 > 0:
                                                    {
                                                        withBlock.Width = _QBEColumn.ColumnWidth;
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.None;
                                                        break;
                                                    }
                                                case var case5 when case5 == -2:
                                                    {
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
                                                        break;
                                                    }
                                                case -1:
                                                    {
                                                        withBlock.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                                        break;
                                                    }

                                                default:
                                                    {
                                                        withBlock.Width = charw * 10;
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.None;
                                                        break;
                                                    }
                                            }

                                            break;
                                        }

                                    case var case6 when case6 == DbType.Byte:
                                    case DbType.Currency:
                                    case DbType.Decimal:
                                    case DbType.Double:
                                    case DbType.Int16:
                                    case DbType.Int32:
                                    case DbType.Int64:
                                    case DbType.SByte:
                                    case DbType.Single:
                                    case var case7 when case7 == DbType.Time:
                                    case DbType.UInt16:
                                    case DbType.UInt32:
                                    case DbType.UInt64:
                                    case DbType.VarNumeric:
                                        {
                                            switch (_QBEColumn.ColumnWidth)
                                            {
                                                case var case8 when case8 > 0:
                                                    {
                                                        withBlock.Width = _QBEColumn.ColumnWidth;
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.None;
                                                        break;
                                                    }
                                                case var case9 when case9 == -2:
                                                    {
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
                                                        break;
                                                    }
                                                case -1:
                                                    {
                                                        withBlock.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                                        break;
                                                    }

                                                default:
                                                    {
                                                        withBlock.Width = charw * 10;
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.None;
                                                        break;
                                                    }
                                            }

                                            break;
                                        }


                                    case var case10 when case10 == DbType.Binary:
                                    case DbType.Object:
                                        {
                                            switch (_QBEColumn.ColumnWidth)
                                            {
                                                case var case11 when case11 > 0:
                                                    {
                                                        withBlock.Width = _QBEColumn.ColumnWidth;
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.None;
                                                        break;
                                                    }
                                                case var case12 when case12 == -1:
                                                    {
                                                        withBlock.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                                        break;
                                                    }
                                                case var case13 when case13 == -2:
                                                    {
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
                                                        break;
                                                    }

                                                default:
                                                    {
                                                        withBlock.Width = charw * 20;
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.None;
                                                        break;
                                                    }
                                            }

                                            break;
                                        }

                                    default:
                                        {
                                            switch (_QBEColumn.ColumnWidth)
                                            {
                                                case var case14 when case14 == -1:
                                                    {
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.AllCells;
                                                        break;
                                                    }
                                                case var case15 when case15 == -2:
                                                    {
                                                        withBlock.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
                                                        break;
                                                    }
                                                case var case16 when case16 > 0:
                                                    {
                                                        withBlock.Width = _QBEColumn.ColumnWidth;
                                                        withBlock.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                                        break;
                                                    }

                                                default:
                                                    {
                                                        if (_QBEColumn.DbCOlumn.Size < 30)
                                                        {
                                                            cellw = charw * _QBEColumn.DbCOlumn.Size;
                                                        }
                                                        else
                                                        {
                                                            cellw = charw * 30;
                                                        }
                                                        if (cellw < ResultGrid.Columns[x].HeaderText.Length * charw)
                                                        {
                                                            cellw = ResultGrid.Columns[x].HeaderText.Length * charw;
                                                        }
                                                        withBlock.Width = cellw;
                                                        withBlock.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                                        break;
                                                    }
                                            }

                                            break;
                                        }



                                }

                                {
                                    var withBlock1 = withBlock.DefaultCellStyle;
                                    withBlock1.Format = _QBEColumn.DisplayFormat;
                                    withBlock1.Alignment = _QBEColumn.Aligment;
                                    if (!string.IsNullOrEmpty(_QBEColumn.BackColor.ToString()))
                                    {
                                        withBlock1.BackColor = _QBEColumn.BackColor;
                                    }
                                    if (!string.IsNullOrEmpty(_QBEColumn.ForeColor.ToString()))
                                    {
                                        withBlock1.ForeColor = _QBEColumn.ForeColor;
                                    }
                                    if (_QBEColumn.Font is not null)
                                    {
                                        withBlock1.Font = _QBEColumn.Font;
                                    }

                                    if (withBlock1.Font is null)
                                    {
                                        withBlock1.Font = (System.Drawing.Font)ResultGrid.Font.Clone();
                                    }
                                    withBlock1.Font = withBlock1.Font.Change(_QBEColumn.FontStyle);


                                }

                            }

                        }
                    }

                }







                if (Conversions.ToBoolean(Operators.OrObject(Conversions.ToInteger(_QBEColumn.UseInQBE) == (int)UseInQBEEnum.UseInQUE, Operators.ConditionalCompareObjectNotEqual(_QBEColumn.QBEValue, "", false))))
                {
                    QueryGrid.Rows.Add();

                    // modifica
                    switch (_QBEColumn.DbCOlumn.DbType)
                    {
                        case var case17 when case17 == DbType.Boolean:
                            {
                                var ncell = new DataGridViewCheckBoxCell();
                                ncell.ThreeState = true;
                                ncell.IndeterminateValue = "";
                                ncell.TrueValue = true;
                                ncell.FalseValue = false;
                                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(_QBEColumn.QBEValue, null, false)))
                                {
                                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(_QBEColumn.QBEValue, "True", false)))
                                    {
                                        ncell.Value = true;
                                    }
                                    else
                                    {
                                        ncell.Value = false;
                                    }
                                }
                                else
                                {
                                    ncell.Value = ncell.IndeterminateValue;
                                }

                                QueryGrid.Rows[i].Cells[1] = ncell;
                                break;
                            }

                        default:
                            {
                                var ncell = new DataGridViewTextBoxCell();
                                ncell.Value = _QBEColumn.QBEValue;
                                QueryGrid.Rows[i].Cells[1] = ncell;
                                break;
                            }
                    }



                    {
                        var withBlock2 = QueryGrid.Rows[i];
                        withBlock2.Cells[0].Value = _QBEColumn.FriendlyName;
                        withBlock2.Cells[0].Tag = _QBEColumn.DbCOlumn.DbColumnNameE;
                        if (!string.IsNullOrEmpty(Strings.Trim(Conversions.ToString(_QBEColumn.QBEValue))))
                        {
                            withBlock2.Cells[1].ReadOnly = true;
                            withBlock2.Cells[1].Style.ForeColor = System.Drawing.Color.Red;
                        }
                        withBlock2.Tag = _QBEColumn.DbCOlumn.DbColumnName;
                        if (_QBEColumn.UseInQBE == false)
                        {
                            QueryGrid.Rows[i].Visible = false;
                        }
                    }
                    i = i + 1;
                }
            }

            QueryGrid.Width = TabPageCriteriRicerca.Width - 5;

            // If i <= 6 Then
            // Me.QueryGrid.Height = i * 23 + 25 - Me.chkLikeOperator.Height - 5
            // Else
            // Me.QueryGrid.Height = 6 * 23 + 25 - Me.chkLikeOperator.Height - 5
            // End If


            if (Mode == QbeMode.Report)
            {
                i = 0;
                foreach (var QBEReport in mReports.Values)
                {
                    ReportGrid.Rows.Add();
                    {
                        var withBlock3 = ReportGrid.Rows[i];
                        withBlock3.Cells[dgvcNomeReport.Name].Value = QBEReport.ReportTitle;
                        withBlock3.Cells[dgvcDescrizioneReport.Name].Value = QBEReport.ReportDescription;
                        withBlock3.Cells[dgvcReportFileName.Name].Value = QBEReport.ReportFileName;
                        withBlock3.Tag = QBEReport.ReportFileName;
                    }
                    i = i + 1;
                }
                ReportGrid.AutoResizeColumn(0);
            }

            QueryGrid.AutoResizeColumn(0);
            ParametersLoaded = true;

        }

        private void LoadReportParameters(string ReportName)
        {

            double QueryColumnWidth = 0d;
            int i = 0;
            QueryGrid.Rows.Clear();

            // controllo per verifica collection QBEColumns



            foreach (QBEColumn QBEColumn in QBEColumns)
            {
                if ((QBEColumn.ReportName.Trim().ToLower() ?? "") == (ReportName.Trim().ToLower() ?? ""))
                {
                    i = i + 1;
                }
            }

            if (i == 0)
            {
                var DbColumns = mDbObject.GetDbColumns();
                foreach (var DbColumn in DbColumns)
                    mQBEColumns.AddForReport(DefaultReport.ReportTitle, DbColumn, DbColumn.FriendlyName, DbColumn.QBEValue);

            }

            i = 1;
            // Me.lstSortColumns.Items.Clear()
            SortColumns.Clear();

            foreach (QBEColumn _QBEColumn in QBEColumns)
            {
                if ((_QBEColumn.ReportName.Trim().ToLower() ?? "") == (ReportName.Trim().ToLower() ?? ""))
                {
                    if (Conversions.ToBoolean(Operators.OrObject(Conversions.ToInteger(_QBEColumn.UseInQBE) == (int)UseInQBEEnum.UseInQUE, Operators.ConditionalCompareObjectNotEqual(_QBEColumn.QBEValue, "", false))))
                    {
                        var sc = new SortColumn();
                        sc.Name = _QBEColumn.DbCOlumn.DbColumnNameE;
                        sc.FriendlyName = _QBEColumn.FriendlyName;
                        sc.Position = i;
                        sc.AscDesc = "ASC";
                        SortColumns.Add(_QBEColumn.DbCOlumn.DbColumnNameE, sc);
                        i = i + 1;
                    }
                }
            }

            lstSortColumns.DataSource = SortColumns.Values.ToList();
            lstSortColumns.ValueMember = "Name";
            lstSortColumns.DisplayMember = "FriendlyName";

            i = 0;
            foreach (QBEColumn _QBEColumn in QBEColumns)
            {
                if ((_QBEColumn.ReportName.Trim().ToLower() ?? "") == (ReportName.Trim().ToLower() ?? ""))
                {
                    if (Conversions.ToBoolean(Operators.OrObject(Conversions.ToInteger(_QBEColumn.UseInQBE) == (int)UseInQBEEnum.UseInQUE, Operators.ConditionalCompareObjectNotEqual(_QBEColumn.QBEValue, "", false))))
                    {
                        QueryGrid.Rows.Add();

                        switch (_QBEColumn.DbCOlumn.DbType)
                        {
                            case var @case when @case == DbType.Boolean:
                                {
                                    var ncell = new DataGridViewCheckBoxCell();
                                    ncell.ThreeState = true;
                                    ncell.IndeterminateValue = "";
                                    ncell.TrueValue = true;
                                    ncell.FalseValue = false;
                                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(_QBEColumn.QBEValue, null, false)))
                                    {
                                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(_QBEColumn.QBEValue, "True", false)))
                                        {
                                            ncell.Value = true;
                                        }
                                        else
                                        {
                                            ncell.Value = false;
                                        }
                                    }
                                    else
                                    {
                                        ncell.Value = ncell.IndeterminateValue;
                                    }

                                    QueryGrid.Rows[i].Cells[1] = ncell;
                                    break;
                                }

                            default:
                                {
                                    var ncell = new DataGridViewTextBoxCell();
                                    ncell.Value = _QBEColumn.QBEValue;
                                    QueryGrid.Rows[i].Cells[1] = ncell;
                                    break;
                                }
                        }

                        {
                            var withBlock = QueryGrid.Rows[i];
                            withBlock.Cells[0].Value = _QBEColumn.FriendlyName;
                            withBlock.Cells[0].Tag = _QBEColumn.DbCOlumn.DbColumnNameE;
                            if (!string.IsNullOrEmpty(Strings.Trim(Conversions.ToString(_QBEColumn.QBEValue))))
                            {
                                withBlock.Cells[1].ReadOnly = true;
                                withBlock.Cells[1].Style.ForeColor = System.Drawing.Color.Red;
                            }
                            withBlock.Tag = _QBEColumn.DbCOlumn.DbColumnName;
                            if (_QBEColumn.UseInQBE == false)
                            {
                                QueryGrid.Rows[i].Visible = false;
                            }
                        }
                        i = i + 1;
                    }
                }
            }




        }



        private void BuildQuery3()
        {

            var FiltersGroup = new DbFiltersGroup();

            var Filters = new DbFilters();
            var Filter = new DbFilter();
            string dbColumnName = "";
            DbColumn dbColumn;
            string Value = "";
            string CompOp = "";
            int i = 0;
            int item = 0;
            string FriendlyName = "";
            string[] array;
            string SQLWhere = "";

            QueryGrid.EndEdit();

            FiltersGroup.Clear();

            foreach (var row in QueryGrid.Rows)
            {
                FriendlyName = Conversions.ToString(row.Cells[dgvcNomeCampo.Name].Value);
                // MsgBox(row.Cells(1).ValueType.ToString)


                if (row.Cells[1] is DataGridViewCheckBoxCell)
                {
                    DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)row.Cells[dgvcValoreCampo.Name];
                    Value = Conversions.ToString(c.Value);
                }
                else
                {
                    Value = Conversions.ToString(row.Cells[1].Value);
                }





                if (i >= 0)
                {
                    dbColumn = GetDbColumnFromFriendlyName(FriendlyName);
                    // dbColumn = GetDbColumnFromDbColumnName(row.Cells(Me.dgvcNomeCampo.Name).Tag)
                    if (!string.IsNullOrEmpty(Strings.Trim(Value)) | !string.IsNullOrEmpty(Value))
                    {
                        Value = Value.Trim();
                        if (Value != ";")
                        {
                            array = Strings.Split(Value, ";");
                        }
                        else
                        {
                            array = new string[1];
                            array[0] = ";";
                        }
                        Filters = xBuildFilters(dbColumn, array);
                        if (Filters is not null)
                        {
                            // Filters.Item(Filters.Count - 1).LogicOperator = BasicDAL.LogicOperator.None
                            Filters.ElementAtOrDefault(Filters.Count - 1).LogicOperator = LogicOperator.None;
                            Filters.LogicOperator = LogicOperator.AND;
                            FiltersGroup.Add(Filters);
                        }
                        item = item + 1;
                    }
                }

            }



            if (Conversions.ToBoolean(FiltersGroup.Count))
            {
                FiltersGroup[FiltersGroup.Count - 1].LogicOperator = LogicOperator.None;
            }


            string argSQL = "";
            FiltersGroup.BuildSQLFilter(ref argSQL);
            mDbObject.TopRecords = mTopRecords;

            if (PreserveDbFilters == false)
            {
                mDbObject.FiltersGroup = FiltersGroup;
            }
            else
            {
                foreach (DbFilters _Filters in FiltersGroup)
                    mDbObject.FiltersGroup.AddFilters(_Filters, LogicOperator.AND);
            }



        }
        private DbFilters xBuildFilters(DbColumn DbColumn, string[] FilterConditions)
        {
            var Filters = new DbFilters();
            var Filter = new DbFilter();
            object value = null;
            int p = 0;


            foreach (var Element in FilterConditions)
            {

                SearchNE(ref Element, DbColumn, Filters);
                SearchGTE(ref Element, DbColumn, Filters);
                SearchLTE(ref Element, DbColumn, Filters);
                SearchGT(ref Element, DbColumn, Filters);
                SearchLT(ref Element, DbColumn, Filters);
                SearchRange(ref Element, DbColumn, Filters);
                SearchISNULL(ref Element, DbColumn, Filters);
                SearchISNOTNULL(ref Element, DbColumn, Filters);

                if (!string.IsNullOrEmpty(Element))
                {
                    // DbColumn.IsDate()

                    if (DbColumn.IsString() & mUseExactSearchForString == false)
                    {
                        if (Element.StartsWith("[") == false & Element.EndsWith("]") == false)
                        {
                            Element = "%" + Element + "%";
                        }
                        else
                        {
                            Element = Element.Substring(1, Element.Length - 2);
                        }
                    }

                    Filter = new DbFilter();
                    Filter.DbColumn = DbColumn;

                    // .DbColumn.DBColumnName = DbColumn.DBColumnName
                    if (Conversions.ToBoolean(Strings.InStr(Element, "%")))
                    {
                        Filter.ComparisionOperator = ComparisionOperator.Like;
                    }
                    else
                    {
                        Filter.ComparisionOperator = ComparisionOperator.Equal;
                    }
                    Filter.Value = Cast(Element, DbColumn.DbType);

                    Filter.LogicOperator = LogicOperator.OR;
                    Filters.Add(Filter);
                }

            }

            return Filters;


        }


        private void SearchGT(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object Value = null;


            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, ">");

            if (Strings.Mid(Inputs, 1, 1) == ">")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {

                            Value = Cast(Array[1], DbColumn.DbType);

                            Filters.Add(DbColumn, ComparisionOperator.GreaterThan, Value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }






        }

        private void SearchGTE(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object value = null;

            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, ">=");

            if (Strings.Mid(Inputs, 1, 2) == ">=")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.GreaterThanOrEqualTo, value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }



        }


        private void SearchLT(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object value = null;
            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, "<");

            if (Strings.Mid(Inputs, 1, 1) == "<")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.LessThan, value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }



        }

        private void SearchISNULL(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object value = null;
            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, "ISNULL");

            if (Strings.Mid(Inputs, 1, 6).ToUpper() == "ISNULL")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.IsNull, value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }
        }


        private void SearchISNOTNULL(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object value = null;
            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, "ISNOTNULL");

            if (Strings.Mid(Inputs, 1, 6).ToUpper() == "ISNOTNULL")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.IsNotNull, value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }
        }



        private void SearchLTE(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object value = null;
            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, "<=");

            if (Strings.Mid(Inputs, 1, 2) == "<=")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.LessThanOrEqualTo, value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }


        }


        private void SearchNE(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;

            object value = null;

            if (string.IsNullOrEmpty(Inputs))
                return;
            Array = Strings.Split(Inputs, "<>");


            if (Strings.Mid(Inputs, 1, 2) == "<>")
            {
                switch (Array.Length)
                {
                    case var @case when @case == 0:
                        {
                            break;
                        }

                    default:
                        {
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.NotEqualTo, value, LogicOperator.OR);
                            Inputs = "";
                            break;
                        }
                }
            }



        }
        private void SearchRange(ref string Inputs, DbColumn DbColumn, DbFilters Filters)
        {
            string[] Array;
            object value = null;

            if (string.IsNullOrEmpty(Inputs))
                return;
            if (Strings.InStr(Inputs, "...") == Conversions.ToInteger(false))
                return;

            Array = Strings.Split(Inputs, "...");

            switch (Array.Length)
            {
                case var @case when @case == 0:
                    {
                        break;
                    }
                case var case1 when case1 == 1:
                    {
                        value = Cast(Array[0], DbColumn.DbType);
                        Filters.Add(DbColumn, ComparisionOperator.GreaterThanOrEqualTo, value, LogicOperator.OR);
                        break;
                    }

                default:
                    {
                        value = Cast(Array[0], DbColumn.DbType);
                        Filters.Add(DbColumn, ComparisionOperator.GreaterThanOrEqualTo, value, LogicOperator.AND);
                        if (!string.IsNullOrEmpty(Array[1]))
                        {
                            value = null;
                            value = Cast(Array[1], DbColumn.DbType);
                            Filters.Add(DbColumn, ComparisionOperator.LessThanOrEqualTo, value, LogicOperator.OR);
                        }
                        Inputs = "";
                        break;
                    }
            }

        }


        private void ReturnQueryResult()
        {

            if (mMode == QbeMode.Report)
                return;


            switch (ResultMode)
            {
                case var @case when @case == QBEResultMode.BoundControls:
                    {
                        ReturnBoundControls();
                        break;
                    }
                case var case1 when case1 == QBEResultMode.SingleRow:
                    {
                        ReturnSelectedRows();
                        break;
                    }
                case var case2 when case2 == QBEResultMode.AllRows:
                    {
                        ReturnSelectedRows();
                        break;
                    }
                case var case3 when case3 == QBEResultMode.SelectedRows:
                    {
                        ReturnSelectedRows();
                        break;
                    }
                case var case4 when case4 == QBEResultMode.DataTable:
                    {
                        ReturnResultsDataTable();
                        break;
                    }
                case var case5 when case5 == QBEResultMode.DBObject:
                    {
                        ReturnResultsDBObject();
                        break;
                    }

            }




            mDbObject.FiltersGroup = oFiltersGroup;


            // mCallerForm.Focus()
            CallerForm.Enabled = true;
            var argResultDbObject = DbObject;
            QBEForm_ResultReturned?.Invoke(ref argResultDbObject);
            DbObject = argResultDbObject;
            GC.Collect();
            Close();

        }


        private object GetQueryGridRowValue(string DbColumnName)
        {

            foreach (var row in QueryGrid.Rows)
            {
                if ((Strings.UCase(Strings.Trim(Conversions.ToString(row.Tag))) ?? "") == (Strings.UCase(Strings.Trim(DbColumnName)) ?? ""))
                {
                    return row.Cells[2].Value;
                }
            }
            return default;

        }

        private void ReturnResultsDataTable()
        {
            if (ResultGrid.SelectedRows.Count == 0)
                return;

            mResultsDataTable = new DataTable();
            mResultsDataTable = mDbObject.GetDataTable().Clone();
            mResultsDataTable.Clear();
            DataRow oSourceDataRow;
            DataRow oDataRow;
            foreach (var oRow in ResultGrid.SelectedRows)
            {
                oSourceDataRow = (oRow.DataBoundItem as DataRowView).Row;
                oDataRow = mResultsDataTable.NewRow();
                CopyDataRow(oSourceDataRow, oDataRow);
                mResultsDataTable.Rows.Add(oDataRow);
            }

        }

        private void CopyDataRow(DataRow oSourceRow, DataRow oTargetRow)
        {
            int nIndex = 0;
            // - Copy all the fields from the source row to the target row
            foreach (var oItem in oSourceRow.ItemArray)
            {
                oTargetRow[nIndex] = oItem;
                nIndex += 1;
            }
        }
        private void ReturnResultsDBObject()
        {


            if (ResultGrid.SelectedRows.Count == 0)
                return;
            if (mDbColumnsMapping.Count == 0)
                return;

            DbFilters Filters = null;
            var FiltersGroup = new DbFiltersGroup();

            object value;



            foreach (var row in ResultGrid.SelectedRows)
            {
                Filters = new DbFilters();

                foreach (DbColumnMapping DbColumnMapping in mDbColumnsMapping)
                {
                    value = row.Cells[DbColumnMapping.QBEDbColumn.DbColumnName].Value;
                    if (!ReferenceEquals(value, DBNull.Value))
                    {
                        Filters.Add(DbColumnMapping.TargetDbColumn, ComparisionOperator.Equals, value, LogicOperator.AND);
                    }
                }


                // Filters.Item(Filters.Count - 1).LogicOperator = BasicDAL.LogicOperator.None
                Filters.ElementAtOrDefault(Filters.Count - 1).LogicOperator = LogicOperator.None;
                Filters.LogicOperator = LogicOperator.OR;
                FiltersGroup.Add(Filters);
            }


            // Filters.Item(Filters.Count - 1).LogicOperator = BasicDAL.LogicOperator.None
            Filters.ElementAtOrDefault(Filters.Count - 1).LogicOperator = LogicOperator.None;
            Filters.LogicOperator = LogicOperator.OR;
            FiltersGroup.Add(Filters);
            FiltersGroup[FiltersGroup.Count - 1].LogicOperator = LogicOperator.None;
            mResultsDBObject.FiltersGroup = FiltersGroup;
            mResultsDBObject.DoQuery();
            // mResultsDBObject.MoveFirst()


        }
        private void ReturnSelectedRows()
        {


            if (ResultGrid.SelectedRows.Count == 0)
                return;

            DbFilters Filters = null;
            var FiltersGroup = new DbFiltersGroup();

            object value;


            foreach (var row in ResultGrid.SelectedRows)
            {
                Filters = new DbFilters();
                foreach (DbColumnMapping DbColumnMapping in mDbColumnsMapping)
                {
                    value = row.Cells[DbColumnMapping.QBEDbColumn.DbColumnName].Value;
                    if (!ReferenceEquals(value, DBNull.Value))
                    {
                        Filters.Add(DbColumnMapping.TargetDbColumn, ComparisionOperator.Equals, value, LogicOperator.AND);
                    }
                }
                if (Conversions.ToBoolean(Filters.Count))
                {
                    // Filters.Item(Filters.Count - 1).LogicOperator = BasicDAL.LogicOperator.None
                    Filters.ElementAtOrDefault(Filters.Count - 1).LogicOperator = LogicOperator.None;
                    Filters.LogicOperator = LogicOperator.OR;
                    FiltersGroup.Add(Filters);
                }

            }
            if (Conversions.ToBoolean(Filters.Count))
            {
                FiltersGroup[FiltersGroup.Count - 1].LogicOperator = LogicOperator.None;
            }

            // modifica per colonna di ordinamento risultato
            string orderby = "";
            string sortorder = " ASC ";
            if (ResultGrid.SortedColumn is not null)
            {

                if (ResultGrid.SortOrder != SortOrder.Ascending)
                {
                    sortorder = " DESC ";
                }

                orderby = QueryDbObject.GetDbColumnByDbColumnNameE(ResultGrid.SortedColumn.DataPropertyName).DbColumnNameE + sortorder;
            }
            else
            {
                foreach (DbColumn Dbcolumn in QueryDbObject.GetPrimaryKeyDbColumns())
                {
                    if (Dbcolumn.DbColumnNameE.Contains(" "))
                    {
                        orderby = orderby + Dbcolumn.DbColumnName + ", ";
                    }
                    else
                    {
                        orderby = orderby + Dbcolumn.DbColumnNameE + ", ";
                    }

                }
                orderby = orderby.Trim();
                if (orderby.EndsWith(","))
                {
                    orderby = Strings.Mid(orderby, 1, orderby.Length - 1);
                }
            }

            QueryDbObject.OrderBy = orderby;
            QueryDbObject.FiltersGroup = FiltersGroup;
            QueryDbObject.DoQuery();

            if (BoundDataGridView is not null)
            {
                BoundDataGridView.DataSource = QueryDbObject.DataTable;
            }


        }

        private void ReturnBoundControls()
        {
            object value;

            foreach (QBEBoundControl QBEBoundControl in mBoundControls)
            {
                try
                {

                    value = ResultGrid.CurrentRow.Cells[QBEBoundControl.DbColumn.DbColumnName].Value;

                    // If (QBEBoundControl.Control.GetType.FullName.StartsWith("Wisej.Web.DataGridView")) Then
                    // Dim isInEditMode As Boolean = False
                    // isInEditMode = CallByName(QBEBoundControl.Control, "IsInEditMode", CallType.Get, False)
                    // If isInEditMode = True Then
                    // CallByName(QBEBoundControl.Control, "IsInEditMode", CallType.Set, False)
                    // End If

                    // CallByName(QBEBoundControl.Control, QBEBoundControl.PropertyName, CallType.Set, value)

                    // If isInEditMode = True Then
                    // CallByName(QBEBoundControl.Control, "IsInEditMode", CallType.Set, True)
                    // End If


                    // Else


                    Microsoft.VisualBasic.Interaction.CallByName(QBEBoundControl.Control, QBEBoundControl.PropertyName, CallType.Set, value);
                }

                // End If





                catch (Exception ex)
                {
                    int x = 0;

                }

            }
            // RaiseEvent QBEForm_ResultReturned(Me.DbObject)
        }




        private string GetDbColumnNameFromFriendlyName(string FriendlyName)
        {

            foreach (QBEColumn QBEColumn in mQBEColumns)
            {

                if ((Strings.UCase(QBEColumn.FriendlyName) ?? "") == (Strings.UCase(FriendlyName) ?? ""))
                {
                    return QBEColumn.DbCOlumn.DbColumnName;
                }
            }
            return "";


        }
        private DbColumn GetDbColumnFromFriendlyName(string FriendlyName)
        {
            foreach (QBEColumn QBEColumn in QBEColumns)
            {
                // For Each entry As DictionaryEntry In Me.mQBEColumns
                // QBEColumn = entry.Value
                if ((Strings.UCase(QBEColumn.FriendlyName) ?? "") == (Strings.UCase(FriendlyName) ?? ""))
                {
                    return QBEColumn.DbCOlumn;
                }
            }
            return null;


        }
        private DbColumn GetDbColumnFromDbColumnName(string DbColumnName)
        {

            foreach (QBEColumn QBEColumn in mQBEColumns)
            {
                // For Each entry As DictionaryEntry In Me.mQBEColumns
                // QBEColumn = entry.Value
                if ((Strings.UCase(QBEColumn.DbCOlumn.DbColumnName) ?? "") == (Strings.UCase(DbColumnName) ?? ""))
                {
                    return QBEColumn.DbCOlumn;
                }
            }
            return null;



        }

        private object Cast(object Value, DbType dbType)
        {
            object CastRet = default;
            try
            {
                switch (dbType)
                {
                    case var @case when @case == DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.String:
                    case DbType.StringFixedLength:
                        {
                            CastRet = Convert.ToString(Value);
                            break;
                        }
                    case var case1 when case1 == DbType.Byte:
                        {
                            CastRet = Convert.ToByte(Value);
                            break;
                        }
                    case var case2 when case2 == DbType.Boolean:
                        {
                            CastRet = Convert.ToBoolean(Value);
                            break;
                        }
                    case var case3 when case3 == DbType.Currency:
                    case DbType.Decimal:
                    case DbType.VarNumeric:
                        {
                            CastRet = Convert.ToDecimal(Value);
                            break;
                        }
                    case var case4 when case4 == DbType.Single:
                        {
                            CastRet = Convert.ToSingle(Value);
                            break;
                        }
                    case var case5 when case5 == DbType.Date:
                    case DbType.DateTime:
                    case DbType.Time:
                        {
                            CastRet = Convert.ToDateTime(Value);
                            break;
                        }
                    case var case6 when case6 == DbType.Double:
                        {
                            CastRet = Convert.ToDouble(Value);
                            break;
                        }
                    case var case7 when case7 == DbType.Guid:
                        {
                            CastRet = Convert.ToString(Value);
                            break;
                        }
                    case var case8 when case8 == DbType.SByte:
                        {
                            CastRet = Convert.ToSByte(Value);
                            break;
                        }
                    case var case9 when case9 == DbType.Int16:
                        {
                            CastRet = Convert.ToInt16(Value);
                            break;
                        }
                    case var case10 when case10 == DbType.Int32:
                        {
                            CastRet = Convert.ToInt32(Value);
                            break;
                        }
                    case var case11 when case11 == DbType.Int64:
                        {
                            CastRet = Convert.ToInt64(Value);
                            break;
                        }
                    case var case12 when case12 == DbType.UInt16:
                        {
                            CastRet = Convert.ToUInt16(Value);
                            break;
                        }
                    case var case13 when case13 == DbType.UInt32:
                        {
                            CastRet = Convert.ToUInt32(Value);
                            break;
                        }
                    case var case14 when case14 == DbType.UInt64:
                        {
                            CastRet = Convert.ToUInt64(Value);
                            break;
                        }

                    default:
                        {
                            CastRet = Value;
                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                return DBNull.Value;
            }

            return CastRet;

        }
        private void ClearConditions()
        {
            ResultGrid.Select();
            foreach (var row in QueryGrid.Rows)
            {
                if (row.Cells[1].ReadOnly == false)
                {
                    row.Cells[1].Value = "";
                }
            }
            DoQuery();
            QueryGrid.Select();
        }

        public QBEForm()
        {
            CrystalReportViewerURL = CrystalReportViewerPage;
            // This call is required by the Windows Form Designer.

            try
            {
                InitializeComponent();
            }
            // SetLanguage("it-IT")

            catch (Exception ex)
            {

            }
            mResultGridFont = ResultGrid.Font;
            QBEColumns.QBEForm = this;
            ResultGrid.AutoGenerateColumns = false;
            ReportGrid = _ReportGrid;
            QueryGrid = _QueryGrid;
            ResultGrid = _ResultGrid;
            _ReportGrid.Name = "ReportGrid";
            _QueryGrid.Name = "QueryGrid";
            _ResultGrid.Name = "ResultGrid";
            // Add any initialization after the InitializeComponent() call.

        }

        public void SetLanguage(string Language)
        {


            switch (Language.ToLower().Trim() ?? "")
            {
                case "it-it":
                    {
                        MovePreviousCaption = "Prec.";
                        MoveNextCaption = "Succ.";
                        MoveFirstCaption = "Inizo";
                        MoveLastCaption = "Fine";
                        RefreshCaption = "Ricarica";
                        CloseCaption = "Chiudi";
                        DeleteFiltersCaption = "Azzera Filtri";
                        PrintCaption = "Stampa";
                        SelectCaption = "Seleziona";
                        TabQueryFiltersText = "Filtri Ricerca";
                        TabReportsText = "Reports Disponibili";
                        TabExportText = "Esporta Dati";
                        TabDebugText = "Debug Info";
                        ButtonExportDataText = "Esporta";
                        QueryGridFilterColumnHeaderText = "Nome Colonna";
                        QueryGridFilterValueHeaderText = "Valore Filtro";
                        ReportGridReportNameHeaderText = "Report";
                        ReportGridReportDescriptionHeaderText = "Descrizione Report";
                        ReportGridReportFileNameHeaderText = "Nome File";
                        UseExactSearchForStringCaption = "Usa sempre CONTIENE (%)";
                        break;
                    }

                default:
                    {
                        MovePreviousCaption = "Prev.";
                        MoveNextCaption = "Next";
                        MoveFirstCaption = "First";
                        MoveLastCaption = "Last";
                        RefreshCaption = "Refresh";
                        CloseCaption = "Close";
                        DeleteFiltersCaption = "Clear Filters";
                        PrintCaption = "Print";
                        SelectCaption = "Select";
                        TabQueryFiltersText = "Query Filters";
                        TabReportsText = "Avalaible Reports";
                        TabExportText = "Export Data";
                        TabDebugText = "Debug Info";
                        ButtonExportDataText = "Export Data";
                        QueryGridFilterColumnHeaderText = "Column Name";
                        QueryGridFilterValueHeaderText = "Filter value";
                        ReportGridReportNameHeaderText = "Report";
                        ReportGridReportDescriptionHeaderText = "Report Description";
                        ReportGridReportFileNameHeaderText = "Report Name";
                        UseExactSearchForStringCaption = "Always use LIKE (%)";
                        break;
                    }

            }

        }




        private void DoQuery()
        {
            UseExactSearchForString = !chkLikeOperator.Checked;

            BuildQuery3();
            mDbObject.DoQuery();
            ResultGrid.DataSource = mDbObject.GetDataTable();
            if (ResultGrid.Rows.Count > 0)
            {
                ResultGrid.Rows[0].Selected = true;
            }
            if (Mode == QbeMode.Report)
            {
                PrintReport(false);
            }
            ResultGrid.Focus();

        }
        private void MovePrevious()
        {
            if (mMode == QbeMode.Report)
                return;
            ResultGrid.Focus();
            if (ResultGrid.CurrentRow.ClientIndex > 0)
            {

                int CurrentRowClientIndex = ResultGrid.CurrentRow.ClientIndex - 1;
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, CurrentRowClientIndex];
            }



            // SendKeys.Send("{UP}")

        }
        private void MoveNext()
        {
            if (mMode == QbeMode.Report)
                return;
            ResultGrid.Focus();
            if (ResultGrid.CurrentRow.ClientIndex < ResultGrid.Rows.Count - 1)
            {
                int CurrentRowClientIndex = ResultGrid.CurrentRow.ClientIndex + 1;
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, CurrentRowClientIndex];
            }


        }
        private void MoveFirst()
        {
            if (mMode == QbeMode.Report)
                return;
            ResultGrid.Focus();
            if (Conversions.ToBoolean(ResultGrid.Rows.Count))
            {
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, 0];
            }


        }
        private void MoveLast()
        {
            if (mMode == QbeMode.Report)
                return;
            ResultGrid.Focus();
            if (Conversions.ToBoolean(ResultGrid.Rows.Count))
            {
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, ResultGrid.Rows.Count - 1];
            }


        }


        private void SplitContainer1_Resize(object sender, EventArgs e)
        {
            // ResizeSplitter(True)
        }

        private void ResizeSplitter(bool Mode)
        {
            if (mMode == QbeMode.Report & mReportViewerMode == ReportViewerMode.WEB)
            {
                // Me.Eval("App.QBEForm.aspNetPanel.getDocument().getElementById(""CrystalReportViewer"").Height = """ & Me.AspNetPanel.Height & """;")
            }

        }

        private void bLast_Click(object sender, EventArgs e)
        {
            MoveLast();
        }

        private void bPrev_Click(object sender, EventArgs e)
        {
            MovePrevious();
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            MoveNext();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            ReturnQueryResult();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            // mDbObject.FiltersGroup = Me.oFiltersGroup
            if (mCallerForm is not null)
            {
                try
                {
                    mCallerForm.Enabled = true;
                }
                catch (Exception ex)
                {

                }

            }
            GC.Collect();

            QBEForm_Closed?.Invoke();

            Close();
        }



        private void ResultGrid_DoubleClick(object sender, EventArgs e)
        {
            ReturnQueryResult();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {

            DoQuery();

        }


        private void bDelete_Click(object sender, EventArgs e)
        {
            ClearConditions();

        }

        private void bFirst_Click(object sender, EventArgs e)
        {
            MoveFirst();
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            DoQuery();
            // PrintReport(True)
        }



        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {



            ReportGrid.Visible = false;
            txtDebug.Visible = false;
            PanelEsporta.Visible = false;

            switch (e.TabPage.Name ?? "")
            {

                case var @case when @case == TabPageCriteriRicerca.Name:
                    {
                        break;
                    }


                case var case1 when case1 == TabPageStampe.Name:
                    {
                        ReportGrid.Visible = true;
                        break;
                    }

                case var case2 when case2 == TabPageDebug.Name:
                    {
                        txtDebug.Visible = true;
                        break;
                    }

                case var case3 when case3 == TabPageEsporta.Name:
                    {
                        PanelEsporta.Visible = true;
                        break;
                    }

            }
        }

        private void QueryGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (DoNotAllowQBEFilterChange == true)
                return;


            if (QueryGrid.CurrentCell is null)
            {
                return;
            }

            if (e.ColumnIndex == 1)
            {

                if (QueryGrid.CurrentCell.ReadOnly == true)
                {
                    if ((int)MessageBox.Show("Modificare il parametro di ricerca?", Text, MessageBoxButtons.YesNo) == (int)MessageBoxButtons.Yes)
                    {
                        QueryGrid.CurrentCell.ReadOnly = false;
                    }
                }

            }
        }



        // Private Sub QueryGrid_CellContentClick(sender As System.Object, e As DataGridViewCellEventArgs) Handles QueryGrid.CellContentClick
        // If e.ColumnIndex = 2 Then
        // Me.LoadQBESearchValue(Me.QueryGrid.CurrentRow.Tag, Me.QueryGrid.CurrentRow.Cells(0).Value)
        // End If
        // End Sub


        // Private Sub LoadQBESearchValue(ByVal DBColumnName As String, ByVal FriendlyName As String)


        // Dim SQL As String = ""
        // Dim datatable As New System.Data.DataTable

        // QBESearchValue.SearchDBColumn = Me.mDbObject.GetDbColumn(DBColumnName)
        // QBESearchValue.SearchDBColumnFriendlyName = FriendlyName
        // QBESearchValue.DoQuery()


        // End Sub
        private void CloseQBEForm()
        {
            try
            {
                if (System.IO.File.Exists(mReportsLastPDFViewerFileName))
                {
                    System.IO.File.Delete(mReportsLastPDFViewerFileName);
                }
            }
            catch (Exception ex)
            {

            }
            GC.Collect();
            if (CallerForm is not null)
            {
                CallerForm.Enabled = true;
                CallerForm.Focus();
                CallerForm.Select();
                if (mAfterCloseFocus is not null)
                {
                    mAfterCloseFocus.Focus();
                }
            }


        }


        private void QueryGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // non cancellare
        }

        private void QBEForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            CloseQBEForm();

        }

        private void ResultGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                // Me.RecordLabel.Text = Me.ResultGrid.CurrentCell.RowIndex + 1 & vbCrLf & Me._RecordLabelSeparator & Me.DbObject.RowCount
                RecordLabel.Text = string.Format(_sep, ResultGrid.CurrentCell.RowIndex + 1, _RecordLabelSeparator, DbObject.RowCount);
            }
            catch (Exception ex)
            {

            }
        }

        private void ResultGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReturnQueryResult();
            }
        }

        private void QBEForm_Load(object sender, EventArgs e)
        {
            PdfViewer.Visible = false;
            WebBrowser.Visible = false;

            ReportGrid.Dock = DockStyle.Fill;
            PanelEsporta.Dock = DockStyle.Fill;
        }

        private void QBEForm_Resize(object sender, EventArgs e)
        {

            // If (Me.QueryGrid.Rows.Count = 0) Then
            // Return
            // End If
            // Dim x As Int16 = 20
            // Select Case Me.QueryGrid.Rows.Count

            // Case < 5
            // Me.QueryGrid.Height = QueryGrid.ColumnHeadersHeight + (Me.QueryGrid.RowCount * Me.QueryGrid.DefaultRowHeight + x)
            // Case Else
            // Me.QueryGrid.Height = QueryGrid.ColumnHeadersHeight + (5 * Me.QueryGrid.DefaultRowHeight + x)
            // End Select

            // Me.TabControl.Height = Me.chkLikeOperator.Height + Me.QueryGrid.Height + Me.chkLikeOperator.Top + (x * 2)
            // Me.SplitContainer1.SplitterDistance = Me.SplitContainer1.Height - Me.TabControl.Height
            // 'ResizeSplitter(True)
        }
        private void SetControlsSize()
        {
            if (QueryGrid.Rows.Count == 0)
            {
                return;
            }
            short x = 25;
            QueryGrid.Top = chkLikeOperator.Top + chkLikeOperator.Height;
            QueryGrid.Width = TabPageCriteriRicerca.Width - 5;
            switch (QueryGrid.Rows.Count)
            {
                case var @case when @case < 5:
                    {
                        QueryGrid.Height = QueryGrid.ColumnHeadersHeight + QueryGrid.RowCount * QueryGrid.DefaultRowHeight + x;
                        break;
                    }

                default:
                    {
                        QueryGrid.Height = QueryGrid.ColumnHeadersHeight + 5 * QueryGrid.DefaultRowHeight + x;
                        break;
                    }
            }
            TabControl.Height = chkLikeOperator.Height + QueryGrid.Height + chkLikeOperator.Top + x * 2;
            SplitContainer1.SplitterDistance = SplitContainer1.Height - TabControl.Height;
            QueryGrid.Anchor = (AnchorStyles)((int)AnchorStyles.Bottom + (int)AnchorStyles.Left + (int)AnchorStyles.Right + (int)AnchorStyles.Top);
            TabControl.Anchor = (AnchorStyles)((int)AnchorStyles.Bottom + (int)AnchorStyles.Left + (int)AnchorStyles.Right + (int)AnchorStyles.Top);
            txtDebug.Top = 0;
            txtDebug.Left = 0;
            txtDebug.Width = TabPageDebug.Width;
            txtDebug.Height = TabPageDebug.Height - 10;
            txtDebug.Anchor = (AnchorStyles)((int)AnchorStyles.Bottom + (int)AnchorStyles.Left + (int)AnchorStyles.Right + (int)AnchorStyles.Top);
        }

        public void SetCrystalReportSortFields(ref ReportDocument ObjReport, bool RemoveSchemaName = true)
        {

            if (dgv_SelectedSortColumns.Rows.Count == 0)
            {
                return;
            }

            int i;
            string c = "";
            string p = "";
            string t = "";
            string n = "";
            string orderby = "";
            // primo passo modifica dei nomi campi
            t = DbObject.DbTableName.ToString();
            t = Strings.Replace(t, "]", "");
            t = Strings.Replace(t, "[", "");

            // If (RemoveSchemaName) Then
            // t = Replace(t, "dbo.", "")
            // End If



            // SE ESISTE
            // Dim fieldDef As DatabaseFieldDefinition = ObjReport.Database.Tables("REPORTS").Fields("FIRSTNAME")
            // targetSortField(0).Field = fieldDef
            // targetSortField(0).SortDirection = CrystalDecisions.[Shared].SortDirection.AscendingOrder

            // SE NON ESISTE 
            foreach (DataGridViewRow row in dgv_SelectedSortColumns.Rows)
            {
                DatabaseFieldDefinition fieldDef = (DatabaseFieldDefinition)ObjReport.Database.Tables[t].Fields(row[dgvc_SelectedSortColumns_name].Value);
                bool localAddSortField() { FieldDefinition argfd = fieldDef; var ret = AddSortField(ref ObjReport, ref argfd); fieldDef = (DatabaseFieldDefinition)argfd; return ret; }

                if (localAddSortField())
                {
                    int lastindex = ObjReport.DataDefinition.SortFields.Count - 1;
                    var sf = ObjReport.DataDefinition.SortFields[lastindex];
                    sf.Field = fieldDef;
                    if (Operators.ConditionalCompareObjectEqual(row[dgvc_SelectedSortColumns_ascdesc].Value, "ASC", false))
                    {
                        sf.SortDirection = SortDirection.AscendingOrder;
                    }
                    if (Operators.ConditionalCompareObjectEqual(row[dgvc_SelectedSortColumns_ascdesc].Value, "DESC", false))
                    {
                        sf.SortDirection = SortDirection.DescendingOrder;
                    }
                }
                else
                {
                    // istruzioni insuccesso
                }
            }


            ClearSortFields(ref ObjReport);

        }

        public bool AddSortField(ref ReportDocument reportdoc, ref FieldDefinition fd)
        {
            bool result;
            SortFields sfs;
            object rassort;
            object rassorts;
            object rasfield;
            MethodInfo getrassorts;
            MethodInfo addsort;
            MethodInfo setsortfield;
            MethodInfo getrasfield;
            ConstructorInfo cirassort;
            Assembly rasassembly;
            result = false;
            if (reportdoc is not null || fd is not null)
            {
                sfs = reportdoc.DataDefinition.SortFields;
                getrassorts = sfs.GetType().GetMethod("get_RasSorts", BindingFlags.NonPublic | BindingFlags.Instance);
                rassorts = getrassorts.Invoke(sfs, Type.EmptyTypes);
                addsort = rassorts.GetType().GetMethod("Add");
                rasassembly = getrassorts.ReturnType.Assembly;
                cirassort = rasassembly.GetType("CrystalDecisions.ReportAppServer.DataDefModel.SortClass").GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                rassort = cirassort.Invoke(Type.EmptyTypes);
                setsortfield = rassort.GetType().GetMethod("set_SortField", BindingFlags.Public | BindingFlags.Instance);
                getrasfield = fd.GetType().GetMethod("get_RasField", BindingFlags.NonPublic | BindingFlags.Instance);
                rasfield = getrasfield.Invoke(fd, Type.EmptyTypes);
                setsortfield.Invoke(rassort, new object[] { rasfield });
                addsort.Invoke(rassorts, new object[] { rassort });
                result = true;
            }
            return result;
        }
        public bool ClearSortFields(ref ReportDocument reportdoc)
        {

            if (reportdoc is null)
            {
                return false;
            }


            bool result;
            SortFields sfs;
            object rassort;
            object rassorts;
            object rasfield;
            MethodInfo getrassorts;
            MethodInfo removesort;
            MethodInfo setsortfield;
            MethodInfo getrasfield;
            ConstructorInfo cirassort;
            Assembly rasassembly;
            result = false;
            object[] aiParam = new object[] { new object() };

            int t = reportdoc.DataDefinition.SortFields.Count;

            for (int i = 0, loopTo = t - 1; i <= loopTo; i++)
            {
                sfs = reportdoc.DataDefinition.SortFields;
                getrassorts = sfs.GetType().GetMethod("get_RasSorts", BindingFlags.NonPublic | BindingFlags.Instance);
                rassorts = getrassorts.Invoke(sfs, Type.EmptyTypes);
                removesort = rassorts.GetType().GetMethod("Remove");
                rasassembly = getrassorts.ReturnType.Assembly;
                cirassort = rasassembly.GetType("CrystalDecisions.ReportAppServer.DataDefModel.SortClass").GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                rassort = cirassort.Invoke(Type.EmptyTypes);
                setsortfield = rassort.GetType().GetMethod("set_SortField", BindingFlags.Public | BindingFlags.Instance);
                aiParam[0] = 0;
                removesort.Invoke(rassorts, aiParam);
            }
            return result;

        }


        public string GetCrystalRecordSelectionFormula(bool RemoveSchemaName = true)
        {

            int i;
            string where = DbObject.SQLWhereConditions;
            string c = "";
            string p = "";
            string t = "";
            object v;
            object ov;
            DateTime dv;
            string n = "";
            DbColumns DbColumns;
            // Dim DbColumn As BasicDAL.DbColumn



            // primo passo modifica dei nomi campi
            t = DbObject.DbTableName.ToString();
            t = Strings.Replace(t, "]", "");
            t = Strings.Replace(t, "[", "");

            // If (RemoveSchemaName) Then
            // t = Replace(t, "dbo.", "")
            // End If

            DbColumns = DbObject.GetDbColumns();

            foreach (DbColumn xDbColumn in DbColumns)
            {
                c = xDbColumn.DbColumnNameE + " ";
                n = "{" + t + "." + c + "}";
                where = Regex.Replace(where, c, n, RegexOptions.IgnoreCase);
                // where = where.Replace(c, n)
                // where = where + " AND " + "{" + t + "." + c + "}"
                // where = Me.ReplaceWholeWord(where, c, "{" + t + "." + c + "}")
            }


            where = where.Replace("]", "");
            where = where.Replace("[", "");

            // secondo passo modifica dei valori parametri

            var loopTo = DbObject.Command.Parameters.Count - 1;
            for (i = 0; i <= loopTo; i++)
            {
                ov = DbObject.Command.Parameters[i].Value;
                p = DbObject.Command.Parameters[i].ParameterName;
                // Select Me.DbObject.GetDbColumn(Me.DbObject.Command.Parameters(i).SourceColumn).DBType
                switch (DbObject.Command.Parameters[i].DbType)
                {
                    case var @case when @case == DbType.AnsiString:
                    case DbType.AnsiString:
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Xml:
                        {
                            v = Operators.AddObject(Operators.AddObject("'", ov), "'");
                            // v = Chr(34) + ov + Chr(34)
                            v = Strings.Replace(Conversions.ToString(v), "%", "*");
                            break;
                        }

                    case var case1 when case1 == DbType.Date:
                        {
                            dv = Conversions.ToDate(ov);
                            v = "Datetime(" + dv.Year.ToString() + "," + dv.Month.ToString() + "," + dv.Day.ToString() + ")";
                            break;
                        }
                    case var case2 when case2 == DbType.DateTime:
                        {
                            dv = Conversions.ToDate(ov);
                            v = "Datetime(" + dv.Year.ToString() + "," + dv.Month.ToString() + "," + dv.Day.ToString() + "," + dv.Hour.ToString() + "," + dv.Minute.ToString() + "," + dv.Second.ToString() + ")";
                            break;
                        }
                    case var case3 when case3 == DbType.Time:
                        {
                            dv = Conversions.ToDate(ov);
                            v = "Datetime(" + dv.Year.ToString() + "," + dv.Month.ToString() + "," + dv.Day.ToString() + "," + dv.Hour.ToString() + "," + dv.Minute.ToString() + "," + dv.Second.ToString() + ")";
                            break;
                        }
                    case var case4 when case4 == DbType.Boolean:
                        {
                            v = ov;
                            break;
                        }

                    default:
                        {
                            v = ov;
                            break;
                        }
                }

                where = ReplaceWholeWord(where, p, Conversions.ToString(v));

            }

            return where;


        }
        public string ReplaceWholeWord(string s, string word, string bywhat)
        {
            char firstLetter = word[0];
            var sb = new System.Text.StringBuilder();
            bool previousWasLetterOrDigit = false;
            int i = 0;
            while (i < s.Length - word.Length + 1)
            {
                bool wordFound = false;
                char c = s[i];
                if (c == firstLetter)
                {
                    if (!previousWasLetterOrDigit)
                    {
                        if (s.Substring(i, word.Length).Equals(word))
                        {
                            wordFound = true;
                            bool wholeWordFound = true;
                            if (s.Length > i + word.Length)
                            {
                                if (char.IsLetterOrDigit(s[i + word.Length]))
                                {
                                    wholeWordFound = false;
                                }
                            }

                            if (wholeWordFound)
                            {
                                sb.Append(bywhat);
                            }
                            else
                            {
                                sb.Append(word);
                            }

                            i += word.Length;
                        }
                    }
                }

                if (!wordFound)
                {
                    previousWasLetterOrDigit = char.IsLetterOrDigit(c);
                    sb.Append(c);
                    i += 1;
                }
            }

            if (s.Length - i > 0)
            {
                sb.Append(s.Substring(i));
            }

            return sb.ToString();
        }

        private void SplitContainer1_DoubleClick(object sender, EventArgs e)
        {
            ResizeSplitter(true);
        }

        private void ContextMenuRecords_MenuItemClicked(object sender, MenuItemEventArgs e)
        {
            TopRecords = Conversions.ToInteger(e.MenuItem.Tag);
            Records.Text = e.MenuItem.Text;
        }

        private void ResultGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ResultGrid.CurrentCell is null == true)
            {
                return;
            }

            try
            {

                RecordLabel.Text = string.Format(_sep, ResultGrid.CurrentCell.RowIndex + 1, _RecordLabelSeparator, DbObject.RowCount);
            }
            catch (Exception ex)
            {

            }
        }

        private void bSaveQBE_Click(object sender, EventArgs e)
        {


            string Path = Application.CommonAppDataPath;
            string FileName = @"D:\TEST.XML";
            SaveQBEUserColumnSet(FileName);

        }

        private void bLoadQBE_Click(object sender, EventArgs e)
        {
            string FileName = @"D:\TEST.XML";
            LoadQBEUserColumnSet(FileName);

        }
        private void LoadQBEUserColumnSet(string FileName)
        {
            var QBEUserColumnsSet = new QBEUserColumnsSet();
            var QBEUserColumns = new List<QBEUserColumn>();

            if (System.IO.File.Exists(FileName) == false)
            {
                return;
            }

            object argObject = QBEUserColumnsSet;
            BasicDAL.Utilities.SerializationHelper.DeserializeObjectFromFile(FileName, ref argObject);
            QBEUserColumnsSet = (QBEUserColumnsSet)argObject;
            if (QBEUserColumnsSet is not null)
            {
                if ((QBEUserColumnsSet.DBObjectName.Trim().ToLower() ?? "") == (DbObject.Name.Trim().ToLower() ?? ""))
                {
                    QBEColumns.Clear();
                    foreach (QBEUserColumn N in QBEUserColumnsSet.QBEUserColumns)
                    {
                        QBEColumn Q;
                        var DbColumn = mDbObject.GetDbColumn(N.DBColumnName);
                        Q = QBEColumns.Add(DbColumn, N.FriendlyName, N.DisplayFormat, N.QBEValue, N.UseInQBE, N.DisplayInQBEResult, (int)N.QBEColumnType);
                        Q.BackColor = N.BackColor;
                        Q.ForeColor = N.ForeColor;
                    }
                    LoadParameters();
                }
            }
        }
        private void SaveQBEUserColumnSet(string FileName)
        {
            QueryGrid.EndEdit();
            var QBEUserColumnsSet = new QBEUserColumnsSet();
            var QBEUserColumns = new List<QBEUserColumn>();
            foreach (QBEColumn Q in mQBEColumns)
            {
                var N = new QBEUserColumn();
                N.BackColor = Q.BackColor;
                N.DBColumnName = Q.DbCOlumn.DbColumnNameE;
                N.DisplayFormat = Q.DisplayFormat;
                N.DisplayInQBEResult = Q.DisplayInQBEResult;
                N.ForeColor = Q.ForeColor;
                N.FriendlyName = Q.FriendlyName;
                N.QBEColumnType = Q.QBEColumnType;
                N.UseInQBE = Q.UseInQBE;
                // recupera il Value
                foreach (DataGridViewRow row in QueryGrid.Rows)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(row[0].Tag, N.DBColumnName, false)))
                    {
                        N.QBEValue = Conversions.ToString(row[dgvcValoreCampo.Name].Value);
                        break;
                    }
                }
                QBEUserColumns.Add(N);
            }
            QBEUserColumnsSet.Name = "Filtro Tipologie";
            QBEUserColumnsSet.Description = "Filtro Tipologie";
            QBEUserColumnsSet.UserName = Application.User.Identity.Name;
            QBEUserColumnsSet.QBEUserColumns = QBEUserColumns;
            QBEUserColumnsSet.DBObjectName = mDbObject.Name;
            // Dim xml As String = ""
            // BasicDAL.Utilities.SerializeObjectToFile(FileName, QBEUserColumnsSet)

            var frmx = new QBEFormSaveUserColumnSet();
            frmx.QBEUserColumnsSet = QBEUserColumnsSet;
            frmx.ShowDialog();

        }

        private void AspNetPanel_Resize(object sender, EventArgs e)
        {
            // Dim fullcontrolname As String = BasicDALWisejControls.Utilities.GetClientControlFullName(Me.AspNetPanel)
            // Dim style As String = """border-style:None;height:" & (Me.AspNetPanel.Height - 50).ToString() + "px;width:" & Me.AspNetPanel.Width.ToString() + "px;overflow:auto"""
            // Dim visualStyle As String = """height:" & (Me.AspNetPanel.Height - 50).ToString() + "px;width:" & Me.AspNetPanel.Width.ToString() + "px"""
            // Dim cmdDiv As String = fullcontrolname + ".getDocument().getElementById('CrystalReportViewerDiv').style=" + style
            // Dim cmdCrystalReportViewer As String = fullcontrolname + ".getDocument().getElementById('CrystalReportViewer').style=" + style
            // Dim cmdCrystalReportViewer__UI As String = fullcontrolname + ".getDocument().getElementById('CrystalReportViewer__UI').visualStyle=" + visualStyle


            // cmdCrystalReportViewer__UI = fullcontrolname + ".getDocument().getElementById('CrystalReportViewer__UI').visible='false'"
            // 'Me.Eval(cmdDiv)
            // 'Me.Eval(cmdCrystalReportViewer)
            // Me.Eval(cmdCrystalReportViewer__UI)

        }

        private void btnEsporta_Click(object sender, EventArgs e)
        {

            string filename = "";
            filename = System.IO.Path.GetTempFileName();
            string exportfilename = BasicDAL.Utilities.FileHelper.GetSafeFileName(Text);

            var DbColumns = new DbColumns();

            if (Mode == QbeMode.Query)
            {
                foreach (DataGridViewColumn Column in ResultGrid.Columns)
                {
                    if (Column.Visible == true)
                    {
                        DbColumns.Add(DbObject.GetDbColumnByDbColumnPropertyName(Conversions.ToString(Column.Tag)));
                    }
                }
            }

            if (Mode == QbeMode.Report)
            {
                DbColumns = DbObject.DbColumns;
            }

            System.IO.FileStream Stream;
            if (rbExcel.Checked)
            {
                DbObject.SaveAsCSV(filename, DBColumns: DbColumns);
                Stream = BasicDAL.Utilities.FileHelper.FileToFileStream(filename);
                Application.Download(Stream, exportfilename + ".csv");
                Stream.Close();
            }

            if (rbCSV.Checked)
            {
                DbObject.SaveAsCSV(filename);
                Stream = BasicDAL.Utilities.FileHelper.FileToFileStream(filename);
                Application.Download(Stream, exportfilename + ".csv");
                Stream.Close();
            }

            if (rbXML.Checked)
            {
                DbObject.SaveAsXML(filename);
                Stream = BasicDAL.Utilities.FileHelper.FileToFileStream(filename);
                Application.Download(Stream, exportfilename + ".xml");
                Stream.Close();
            }

            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }



        }

        private void Button1_Click(object sender, EventArgs e)
        {

            string fullcontrolname = Utilities.GetClientControlFullName(AspNetPanel);
            string par = AspNetPanel.Height + ";" + AspNetPanel.Width;
            string x = fullcontrolname + ".CallingServerSideFunction()";

        }


        private void QueryGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if ((QueryGrid.Columns[QueryGrid.CurrentCell.ColumnIndex].Name ?? "") == (dgvcValoreCampo.Name ?? ""))
            {
                e.Control.KeyDown -= dgvcValoreCampo_KeyDown;
                e.Control.KeyDown += dgvcValoreCampo_KeyDown;
            }
        }

        private void dgvcValoreCampo_KeyDown(object sender, KeyEventArgs e)
        {
            // F4 Pressed
            if ((QueryGrid.Columns[QueryGrid.CurrentCell.ColumnIndex].Name ?? "") == (dgvcValoreCampo.Name ?? ""))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    DoQuery();
                }
            }
        }

        private void SplitContainer1_Panel1_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void ReportGrid_Click(object sender, EventArgs e)
        {
            if (ReportGrid.CurrentRow is null)
            {
                return;
            }
            string NomeReport = Conversions.ToString(ReportGrid.CurrentRow[dgvcNomeReport].Value);
            mDbObject = Reports[NomeReport].DbObject;
            LoadReportParameters(NomeReport);
            txtReportTitle.Text = Reports[NomeReport].ReportTitle;
            txtReportDescription.Text = Reports[NomeReport].ReportDescription;
            chkLikeOperator.Checked = Reports[NomeReport].ReportUseLike;
            switch (ReportViewerMode)
            {
                case ReportViewerMode.PDFStream:
                    {
                        PdfViewer.PdfStream = null;
                        break;
                    }
                case ReportViewerMode.PDFUrl:
                    {
                        PdfViewer.PdfSource = "";
                        break;
                    }
                case ReportViewerMode.WEB:
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }


            }
            // Me.ShowReportPDF(Me.Reports(NomeReport))


        }

        ~QBEForm()
        {
        }

        private void QBEForm_Accelerator(object sender, AcceleratorEventArgs e)
        {
            HandleUserInput(sender, e);
        }


        private void HandleUserInput(object sender, object e)
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

                case var case4 when case4 == (int)_DeleteFKey:
                    {

                        ClearConditions();
                        break;
                    }
                case var case5 when case5 == (int)_SaveFKey:
                    {
                        ReturnQueryResult();
                        break;
                    }

                case var case6 when case6 == (int)_RefreshFKey:
                    {

                        DoQuery();
                        break;
                    }

                case var case7 when case7 == (int)_PrintFKey:
                    {

                        DoQuery();
                        PrintReport(true);
                        break;
                    }

                case var case8 when case8 == (int)_CloseFKey:
                    {

                        CloseQBEForm();
                        break;
                    }

                default:
                    {
                        break;
                    }

            }
        }

        private void PanelReportViewer_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void PanelReportViewer_Resize(object sender, EventArgs e)
        {
            PanelReportInfo.Top = 0;
            PanelReportInfo.Left = 0;
            PanelReportInfo.Width = PanelReportViewer.Width;

            switch (ReportViewerMode)
            {
                case ReportViewerMode.WEB:
                    {

                        WebBrowser.Top = PanelReportInfo.Height;
                        WebBrowser.Left = 0;
                        WebBrowser.Width = PanelReportInfo.Width;
                        WebBrowser.Height = PanelReportViewer.Height - PanelReportInfo.Height;

                        AspNetPanel.Top = PanelReportInfo.Height;
                        AspNetPanel.Left = 0;
                        AspNetPanel.Width = PanelReportInfo.Width;
                        AspNetPanel.Height = PanelReportViewer.Height - PanelReportInfo.Height;
                        break;
                    }

                case ReportViewerMode.PDFStream:
                case ReportViewerMode.PDFUrl:
                    {
                        PdfViewer.Top = PanelReportInfo.Height;
                        PdfViewer.Left = 0;
                        PdfViewer.Width = PanelReportInfo.Width;
                        PdfViewer.Height = PanelReportViewer.Height - PanelReportInfo.Height;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }




        }

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

            // Me.QueryGrid.Height = Me.SplitContainer1.Panel2.Height - Me.TabControl.Height - Me.chkLikeOperator.Height - Me.chkLikeOperator.Top - 10
            // Me.QueryGrid.Height = Me.TabPageCriteriRicerca.Height - Me.chkLikeOperator.Height '- Me.chkLikeOperator.Top - 10
            // Me.QueryGrid.Width = Me.TabPageCriteriRicerca.Width - 5

        }

        private void btnSortAdd_Click(object sender, EventArgs e)
        {

            if (lstSortColumns.SelectedItem is null)
            {
                return;
            }

            SortColumn x = (SortColumn)lstSortColumns.SelectedItem;
            if (SelectedSortColumns.ContainsKey(x.Name) == false)
            {
                SelectedSortColumns.Add(x.Name, x);
            }
            lstSortColumns.SelectedItem = null;
            dgv_SelectedSortColumns.Rows.Clear();
            foreach (SortColumn r in SelectedSortColumns.Values)
            {
                var row = new DataGridViewRow(dgv_SelectedSortColumns);
                dgv_SelectedSortColumns.Rows.Add(r.Position, r.Name, r.FriendlyName, r.AscDesc);
            }
        }

        private void btnSortRemove_Click(object sender, EventArgs e)
        {

            if (dgv_SelectedSortColumns.CurrentRow is null)
            {
                return;
            }
            SelectedSortColumns.Remove(Conversions.ToString(dgv_SelectedSortColumns.CurrentRow.Cells[dgvc_SelectedSortColumns_name].Value));
            dgv_SelectedSortColumns.Rows.Clear();
            foreach (SortColumn r in SelectedSortColumns.Values)
            {
                var row = new DataGridViewRow(dgv_SelectedSortColumns);
                dgv_SelectedSortColumns.Rows.Add(r.Position, r.Name, r.FriendlyName, r.AscDesc);
            }
        }

        private void btnSortDown_Click(object sender, EventArgs e)
        {
            DataGridRowMoveDown(dgv_SelectedSortColumns, dgvc_SelectedSortColumns_position);

        }

        private void ListBoxMoveSelectedItem(ListBox listBox, int direction)
        {
            if (listBox.SelectedItem is null || listBox.SelectedIndex < 0)
                return;
            int newIndex = listBox.SelectedIndex + direction;
            if (newIndex < 0 || newIndex >= listBox.Items.Count)
                return;
            var selected = listBox.SelectedItem;
            listBox.Items.Remove(selected);
            listBox.Items.Insert(newIndex, selected);
            listBox.SetSelected(newIndex, true);

        }


        public static void DataGridRowMoveUp(DataGridView dgv, DataGridViewColumn dgvc)
        {
            if (dgv.RowCount <= 0)
                return;
            if (dgv.SelectedRows.Count <= 0)
                return;
            int index = dgv.SelectedRows[0].Index;
            if (index == 0)
                return;
            var rows = dgv.Rows;
            var prevRow = rows[index - 1];
            rows.Remove(prevRow);
            prevRow.Frozen = false;
            rows.Insert(index, prevRow);
            dgv.ClearSelection();
            dgv.Rows[index - 1].Selected = true;

            for (int i = 0, loopTo = dgv.Rows.Count - 1; i <= loopTo; i++)
                dgv.Rows[i][dgvc].Value = i + 1;
        }

        public static void DataGridRowMoveDown(DataGridView dgv, DataGridViewColumn dgvc)
        {
            if (dgv.RowCount <= 0)
                return;
            if (dgv.SelectedRows.Count <= 0)
                return;
            int rowCount = dgv.Rows.Count;
            int index = dgv.SelectedRows[0].Index;
            if (index == rowCount - 1)
                return;
            var rows = dgv.Rows;
            var nextRow = rows[index + 1];
            rows.Remove(nextRow);
            nextRow.Frozen = false;
            rows.Insert(index, nextRow);
            dgv.ClearSelection();
            dgv.Rows[index + 1].Selected = true;

            for (int i = 0, loopTo = dgv.Rows.Count - 1; i <= loopTo; i++)
                dgv.Rows[i][dgvc].Value = i + 1;
        }





        private void btnSortUp_Click(object sender, EventArgs e)
        {

            DataGridRowMoveUp(dgv_SelectedSortColumns, dgvc_SelectedSortColumns_position);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var o = new JsonSerializerSettings();
            o.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string s = JsonConvert.SerializeObject(Reports, Formatting.Indented, o);
        }
    }

    [Serializable]
    public class QBEUserColumnsSet
    {
        public string Name = "";
        public string Description = "";
        public string UserName = "";
        public string ApplyToUsersOrGroups = "everyone";
        public string DBObjectName = "";
        public List<QBEUserColumn> QBEUserColumns;
    }


    [Serializable]
    public class QBEUserColumn
    {
        public string DBColumnName = "";
        public bool UseInQBE = true;
        public bool DisplayInQBEResult = true;
        public string FriendlyName = "";
        public string QBEValue = "";
        public string DisplayFormat = "";
        public System.Drawing.Color BackColor;
        public System.Drawing.Color ForeColor;
        public QBEColumnsTypes QBEColumnType;
        public int ColumnWidth = 0;
    }


    public class QBEColumn
    {
        public QBEForm QBEForm = null;
        private DbColumn mDBColumn;
        private bool mUseInQBE;
        private bool mDisplayInQBEResult;
        private string mFriendlyName = "";
        private string mQBEValue;
        private string mDisplayFormat = "";
        private System.Drawing.Color mBackColor;
        private System.Drawing.Color mForeColor;
        private QBEColumnsTypes mQBEColumnType;
        private int mColumnWidth = 0;
        private int mOrdinalPosition = 0;
        private System.Drawing.Font mFont = null;
        private DataGridViewContentAlignment mAlignment = DataGridViewContentAlignment.TopLeft;
        private System.Drawing.FontStyle mFontStyle = new System.Drawing.FontStyle();
        private string mReportName;


        public string ReportName
        {
            get
            {
                string ReportNameRet = default;
                ReportNameRet = mReportName;
                return ReportNameRet;

            }
            set
            {
                mReportName = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                System.Drawing.FontStyle FontStyleRet = default;
                FontStyleRet = mFontStyle;
                return FontStyleRet;

            }
            set
            {
                mFontStyle = value;
            }
        }


        public DataGridViewContentAlignment Aligment
        {
            get
            {
                DataGridViewContentAlignment AligmentRet = default;
                AligmentRet = mAlignment;
                return AligmentRet;

            }
            set
            {
                mAlignment = value;
            }
        }


        public QBEColumnsTypes QBEColumnType
        {
            get
            {
                QBEColumnsTypes QBEColumnTypeRet = default;
                QBEColumnTypeRet = mQBEColumnType;
                return QBEColumnTypeRet;

            }
            set
            {
                mQBEColumnType = value;
            }
        }
        public System.Drawing.Font Font
        {
            get
            {
                System.Drawing.Font FontRet = default;
                FontRet = mFont;
                return FontRet;

            }
            set
            {
                mFont = value;
            }
        }

        public System.Drawing.Color ForeColor
        {
            get
            {
                System.Drawing.Color ForeColorRet = default;
                ForeColorRet = mForeColor;
                return ForeColorRet;

            }
            set
            {
                mForeColor = value;
            }
        }

        public System.Drawing.Color BackColor
        {
            get
            {
                System.Drawing.Color BackColorRet = default;
                BackColorRet = mBackColor;
                return BackColorRet;

            }
            set
            {
                mBackColor = value;
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
        public object QBEValue
        {
            get
            {
                object QBEValueRet = default;
                QBEValueRet = mQBEValue;
                return QBEValueRet;
            }
            set
            {
                mQBEValue = Conversions.ToString(value);
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
        public bool DisplayInQBEResult
        {
            get
            {
                bool DisplayInQBEResultRet = default;
                DisplayInQBEResultRet = mDisplayInQBEResult;
                return DisplayInQBEResultRet;
            }
            set
            {
                mDisplayInQBEResult = value;
            }
        }
        public bool UseInQBE
        {
            get
            {
                bool UseInQBERet = default;
                UseInQBERet = mUseInQBE;
                return UseInQBERet;
            }
            set
            {
                mUseInQBE = value;
            }
        }
        public DbColumn DbCOlumn
        {
            get
            {
                DbColumn DbCOlumnRet = default;
                DbCOlumnRet = mDBColumn;
                return DbCOlumnRet;
            }
            set
            {
                mDBColumn = value;
            }
        }

        public int ColumnWidth
        {
            get
            {
                int ColumnWidthRet = default;
                ColumnWidthRet = mColumnWidth;
                return ColumnWidthRet;
            }
            set
            {
                mColumnWidth = value;
            }
        }

        public int OrdinalPosition
        {
            get
            {
                int OrdinalPositionRet = default;
                OrdinalPositionRet = mOrdinalPosition;
                return OrdinalPositionRet;
            }
            set
            {
                mOrdinalPosition = value;
            }
        }


    }

    [Serializable]
    public class QBEReport
    {
        private string mReportTitle;
        private string mReportFileName;
        private string mReportDescription;
        private bool mReportUseLike;
        private DbObject mDbObject;


        public DbObject DbObject
        {
            get
            {
                DbObject DbObjectRet = default;
                DbObjectRet = mDbObject;
                return DbObjectRet;

            }
            set
            {
                mDbObject = value;
            }
        }



        public bool ReportUseLike
        {
            get
            {
                bool ReportUseLikeRet = default;
                ReportUseLikeRet = mReportUseLike;
                return ReportUseLikeRet;

            }
            set
            {
                mReportUseLike = value;

            }
        }
        public string ReportFileName
        {
            get
            {
                string ReportFileNameRet = default;
                ReportFileNameRet = mReportFileName;
                return ReportFileNameRet;

            }
            set
            {
                mReportFileName = value;

            }
        }



        public string ReportDescription
        {
            get
            {
                string ReportDescriptionRet = default;
                ReportDescriptionRet = mReportDescription;
                return ReportDescriptionRet;

            }
            set
            {
                mReportDescription = value;

            }
        }


        public string ReportTitle
        {
            get
            {
                string ReportTitleRet = default;
                ReportTitleRet = mReportTitle;
                return ReportTitleRet;

            }
            set
            {
                mReportTitle = value;

            }
        }
    }
    public class QBEReports : CollectionBase
    {
        public QBEReport Add(string ReportName, string ReportFileName, string ReportDescription, DbObject DbObject = null)
        {
            var x = new QBEReport();
            x.ReportDescription = ReportDescription;
            x.ReportFileName = ReportFileName;
            x.ReportTitle = ReportName;
            x.DbObject = DbObject;
            List.Add(x);
            return x;
        }

        public QBEReport get_Item(int Index)
        {
            QBEReport ItemRet = default;
            ItemRet = (QBEReport)List[Index];
            return ItemRet;
        }

        public void set_Item(int Index, QBEReport value)
        {
            List[Index] = value;
        }


    }


    // Inherits DictionaryBase
    public class QBEColumns : CollectionBase
    {
        public QBEForm QBEForm;
        public QBEColumn Add(DbColumn DbColumn, string FriendlyName = "", string DisplayFormat = "", object QBEValue = "", bool UseInQBE = true, bool DisplayInQBEResult = true, int ColumnWidth = 0)
        {
            var x = new QBEColumn();
            return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnsTypes.TextBox, ColumnWidth);

        }
        public QBEColumn Add(DbColumn DbColumn, string FriendlyName, string DisplayFormat, object QBEValue, bool UseInQBE, bool DisplayInQBEResult, QBEColumnsTypes QBEColumnType, int ColumnWidth)
        {
            return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnsTypes.TextBox, ColumnWidth);
        }

        public QBEColumn AddForReport(string ReportName, DbColumn DbColumn, string FriendlyName, object QBEValue, QBEColumnsTypes QBEColumnType)
        {
            return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnType, 0);
        }
        public QBEColumn AddForReport(string ReportName, DbColumn DbColumn, string FriendlyName, object QBEValue = null)
        {
            return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnsTypes.TextBox, 0);
        }
        public QBEColumn AddForReport(QBEReport Report, DbColumn DbColumn, string FriendlyName, object QBEValue = null)
        {
            return Add(Report.ReportTitle, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnsTypes.TextBox, 0);
        }

        private QBEColumn Add(string ReportName, DbColumn DbColumn, string FriendlyName, string DisplayFormat, object QBEValue, bool UseInQBE, bool DisplayInQBEResult, QBEColumnsTypes QBEColumnType, int ColumnWidth)
        {
            var x = new QBEColumn();

            x.ReportName = ReportName;
            x.QBEForm = QBEForm;
            x.OrdinalPosition = Count;
            x.DbCOlumn = DbColumn;
            x.UseInQBE = UseInQBE;
            x.QBEValue = QBEValue;
            x.FriendlyName = FriendlyName;
            x.DisplayInQBEResult = DisplayInQBEResult;
            x.DisplayFormat = DisplayFormat;
            x.QBEColumnType = QBEColumnType;
            x.ColumnWidth = ColumnWidth;
            x.Aligment = DataGridViewContentAlignment.TopLeft;
            if (DbColumn.IsNumeric() | DbColumn.IsDate())
            {
                x.Aligment = DataGridViewContentAlignment.TopRight;
            }
            if (DbColumn.IsString())
            {
                x.Aligment = DataGridViewContentAlignment.TopLeft;
            }
            if (DbColumn.IsTime() | DbColumn.IsDate())
            {
                x.Aligment = DataGridViewContentAlignment.TopRight;
            }
            if (DbColumn.IsBoolean())
            {
                x.Aligment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (string.IsNullOrEmpty(Strings.Trim(x.FriendlyName)))
                x.FriendlyName = x.DbCOlumn.FriendlyName;
            // Dictionary.Add(DbColumn.Name, x)
            List.Add(x);
            return x;
        }


        public QBEColumn GetItem(int Index)
        {

            if (Index < 0 | Index > List.Count - 1)
            {
                return null;
            }

            return (QBEColumn)List[Index];

        }

        public QBEColumn GetItem(DbColumn DbColumn)
        {

            foreach (QBEColumn _QBEColumn in List)
            {
                if ((_QBEColumn.DbCOlumn.Name ?? "") == (DbColumn.Name ?? ""))
                {
                    return _QBEColumn;
                }
            }
            return null;
        }

        public QBEColumn GetItem(string Name)
        {

            foreach (QBEColumn _QBEColumn in List)
            {
                if ((_QBEColumn.DbCOlumn.Name ?? "") == (Name ?? ""))
                {
                    return _QBEColumn;
                }
            }
            return null;

        }

        public int GetItemIndex(QBEColumn QBEColumn)
        {
            if (List.Contains(QBEColumn))
            {
                return List.IndexOf(QBEColumn);
            }
            else
            {
                return -1;
            }
        }

        public int GetItemIndex(string Name)
        {
            QBEColumn _QBEColumn;
            for (short i = 0, loopTo = (short)(Count - 1); i <= loopTo; i++)
            {
                _QBEColumn = (QBEColumn)List[i];
                if ((_QBEColumn.DbCOlumn.Name ?? "") == (Name ?? ""))
                {
                    return i;
                }
            }
            return -1;
        }
        public QBEColumn get_Item(int Index)
        {
            QBEColumn ItemRet = default;
            ItemRet = (QBEColumn)List[Index];
            return ItemRet;

        }

        public void set_Item(int Index, QBEColumn value)
        {
            List[Index] = value;
        }

        // Property QBEColumn(ByVal Index As Integer) As QBEColumn
        // Get
        // Return List.Item(Index)

        // End Get
        // Set(ByVal value As QBEColumn)
        // List.Item(Index) = value

        // End Set
        // End Property



    }

    public class DbColumnMapping
    {
        private DbColumn mQBEDbColumn;
        private DbColumn mTargetDbColumn;

        public DbColumn QBEDbColumn
        {
            get
            {
                DbColumn QBEDbColumnRet = default;
                QBEDbColumnRet = mQBEDbColumn;
                return QBEDbColumnRet;
            }
            set
            {
                mQBEDbColumn = value;

            }
        }

        public DbColumn TargetDbColumn
        {
            get
            {
                DbColumn TargetDbColumnRet = default;
                TargetDbColumnRet = mTargetDbColumn;
                return TargetDbColumnRet;
            }
            set
            {
                mTargetDbColumn = value;
            }
        }

        public DbColumnMapping()
        {

        }
        public DbColumnMapping(DbColumn QBEDbColumn, DbColumn TargetDbColumn)
        {
            mQBEDbColumn = QBEDbColumn;
            mTargetDbColumn = TargetDbColumn;

        }
    }

    public class DbColumnsMapping : CollectionBase
    {

        public DbColumnMapping Add(DbColumn QBEDbColumn, DbColumn TargetDbColumn)
        {
            var x = new DbColumnMapping();
            x.QBEDbColumn = QBEDbColumn;
            x.TargetDbColumn = TargetDbColumn;
            List.Add(x);
            return x;
        }

        public DbColumnMapping get_Item(int Index)
        {
            DbColumnMapping ItemRet = default;
            ItemRet = (DbColumnMapping)List[Index];
            return ItemRet;
        }

        public void set_Item(int Index, DbColumnMapping value)
        {
            List[Index] = value;
        }


    }

    #region QBEBoundControl Class

    public class QBEBoundControl
    {
        private object mControl;
        private DbColumn mDbColumn;
        private string mPropertyName;

        public string PropertyName
        {
            get
            {
                string PropertyNameRet = default;
                PropertyNameRet = mPropertyName;
                return PropertyNameRet;

            }
            set
            {
                mPropertyName = value;
            }
        }
        public DbColumn DbColumn
        {
            get
            {
                DbColumn DbColumnRet = default;
                DbColumnRet = mDbColumn;
                return DbColumnRet;

            }
            set
            {
                mDbColumn = value;

            }
        }
        public object Control
        {
            get
            {
                object ControlRet = default;
                ControlRet = mControl;
                return ControlRet;

            }
            set
            {

                mControl = value;
            }
        }


    }

    public class QBEBoundControls : CollectionBase
    {

        public bool Add(QBEBoundControl QBEBoundControl)
        {
            try
            {
                List.Add(QBEBoundControl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool Add(DbColumn DbColumn, object Control, string PropertyName)
        {

            var QBEc = new QBEBoundControl();
            QBEc.Control = Control;
            QBEc.DbColumn = DbColumn;
            QBEc.PropertyName = PropertyName;
            try
            {
                List.Add(QBEc);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }


    }


    #endregion


    [Serializable]
    public class SortColumn
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public int Position { get; set; }
        public string AscDesc { get; set; }
    }
    // <Serializable>
    // Public Class QBEFormReport

    // Public ReportTitle As String
    // Public ReportDescription As String
    // Public ReportFileName As String

    // Public DbObject As BasicDAL.DbObject

    // End Class
    [Serializable]
    public enum QbeMode
    {
        Query = 0,
        Report = 1
    }

    [Serializable]
    public enum ReportViewerMode
    {
        WEB = 0,
        PDFUrl = 1,
        PDFStream = 2
    }

    public enum UseInQBEEnum
    {
        UseInQUE = true,
        DoNotUseInQBE = false
    }

    [Serializable]
    public enum QBEResultMode
    {
        BoundControls = 0,
        AllRows = 2,
        SingleRow = 1,
        SelectedRows = 3,
        DataTable = 4,
        DBObject = 5
    }

    [Serializable]
    public enum QBEColumnsTypes
    {
        CheckBox = 0,
        ComboBox = 1,
        Image = 2,
        Link = 3,
        TextBox = 4
    }
}