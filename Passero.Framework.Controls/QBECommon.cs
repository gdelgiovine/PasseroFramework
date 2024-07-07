using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    //internal class CustomComponentResourceManager : ComponentResourceManager
    //{
    //    public CustomComponentResourceManager(Type type, string resourceName)
    //       : base(type)
    //    {
    //        this.BaseNameField = resourceName;
    //    }
    //}

    public enum ViewModelGridModes
    {
        NoGridMode = 0,
        DataGridView = 1,
        DataRepeater = 2
    }

    public class DataNavigatorViewModel
    {
        public string Name { get; set; }
        public object ViewModel { get; set; }
        public string FriendlyName { get; set; }
        public DataGridView DataGridView { get; set; }


        public DataRepeater DataRepeater { get; set; }
        private ViewModelGridModes mGridMode = ViewModelGridModes.NoGridMode;
        public ViewModelGridModes GridMode
        {
            get { return mGridMode; }
            set { mGridMode = value; }
        }

        public DataNavigatorViewModel(object ViewModel, string Name = "", string FriendlyName = "", DataGridView DataGridView = null, DataRepeater DataRepeater = null)
        {

            if (string.IsNullOrEmpty(Name))
                Name = ReflectionHelper.GetPropertyValue(ViewModel, "Name").ToString ();

            if (string.IsNullOrEmpty(FriendlyName ))
                FriendlyName = ReflectionHelper.GetPropertyValue(ViewModel, "FriendlyName").ToString();

            this.Name = Name;
            this.FriendlyName = FriendlyName;

            this.ViewModel = ViewModel;

            this.DataRepeater = DataRepeater;
            this.DataGridView = DataGridView;
            if (this.DataRepeater != null)
            {
                this.GridMode = ViewModelGridModes.DataRepeater;
            }
            if (this.DataGridView != null)
            {
                this.GridMode = ViewModelGridModes.DataGridView;
            }
        }

    }

    public class ModelPropertyMapping
    {
        public string QBEModelProperty { get; set; }
        public string TargetModelProperty { get; set; }

        public ModelPropertyMapping()
        {

        }
        public ModelPropertyMapping(string QBEModelProperty, string TargetModelProperty)
        {
            this.TargetModelProperty = TargetModelProperty.Trim();
            this.QBEModelProperty = QBEModelProperty.Trim();
        }
    }

    public class ModelPropertiesMapping : CollectionBase
    {

        public ModelPropertyMapping Add(string QBEModelProperty, string TargetModelProperty)
        {
            var x = new ModelPropertyMapping();
            x.QBEModelProperty = QBEModelProperty;
            x.TargetModelProperty = TargetModelProperty;
            List.Add(x);
            return x;
        }

        public ModelPropertyMapping get_Item(int Index)
        {
            ModelPropertyMapping ItemRet = default;
            ItemRet = (ModelPropertyMapping)List[Index];
            return ItemRet;
        }

        public void set_Item(int Index, ModelPropertyMapping value)
        {
            List[Index] = value;
        }


    }
    // Stub Class QBEForm for Resource loading for QBEForm<ModelClass>
    public class QBEForm : Wisej.Web.Form
    {
    }


    #region QBEBoundControl Class
    public class QBEBoundControl
    {
        private object mControl;
        private string mModelPropertyName;
        private string mControlPropertyName;

        public string ControlPropertyName
        {
            get
            {
                string PropertyNameRet = default;
                PropertyNameRet = mControlPropertyName;
                return PropertyNameRet;

            }
            set
            {
                mControlPropertyName = value;
            }
        }
        public string ModelPropertyName
        {
            get
            {

                return mModelPropertyName;

            }
            set
            {
                mModelPropertyName = value;

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
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
            try
            {
                List.Add(QBEBoundControl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata

        }
        public bool Add(string ModelPropertyName, object Control, string PropertyName)
        {

            var QBEc = new QBEBoundControl();
            QBEc.Control = Control;
            QBEc.ModelPropertyName = ModelPropertyName;
            QBEc.ControlPropertyName = PropertyName;
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
            try
            {
                List.Add(QBEc);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata


        }


    }


    #endregion


    [Serializable]
    public enum QBEMode
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
    [Serializable]
    public enum UseInQBEEnum
    {
        UseInQUE = 1,
        DoNotUseInQBE = 0
    }

    [Serializable]
    public enum QBEResultMode
    {
        BoundControls = 0,
        AllRowsSQLQuery = 2,
        SingleRowSQLQuery = 1,
        MultipleRowsSQLQuery = 3,
        MultipleRowsItems = 4,
        SingleRowItem = 5,
        AllRowsItems = 6

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


    [Serializable]
    public class DbColumn
    {
        public string Name = "";
        public string FriendlyName = "";
        public bool IsBoolean = false;
        public bool IsDate = false;
        public bool IsTime = false;
        public bool IsDateTime = false;
        public bool IsString = false;
        public bool IsNumeric = false;
    }


    public class QBEColumn
    {
        //public XQBEForm QBEForm = null;
        private string mDbColumn;
        private bool mUseInQBE;
        private bool mDisplayInQBEResult = true;
        private string mFriendlyName = "";
        private string mQBEValue;
        private string mDisplayFormat = "";
        private System.Drawing.Color mBackColor;
        private System.Drawing.Color mForeColor;
        private QBEColumnsTypes mQBEColumnType = QBEColumnsTypes.TextBox;
        private int mColumnSize = 0;
        private int mOrdinalPosition = 0;
        private System.Drawing.Font mFont = null;
        private Wisej.Web.DataGridViewContentAlignment mAlignment = Wisej.Web.DataGridViewContentAlignment.TopLeft;
        private System.Drawing.FontStyle mFontStyle = new System.Drawing.FontStyle();
        private string mReportName;
        public float FontSize { get; set; }


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


        public Wisej.Web.DataGridViewContentAlignment Aligment
        {
            get
            {
                Wisej.Web.DataGridViewContentAlignment AligmentRet = default;
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
                if (mFriendlyName == null)
                    mFriendlyName = DbColumn;
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
        public string DbColumn
        {
            get
            {

                return mDbColumn;
            }
            set
            {
                mDbColumn = value;
            }
        }

        public int ColumnSize
        {
            get
            {
                int ColumnWidthRet = default;
                ColumnWidthRet = mColumnSize;
                return ColumnWidthRet;
            }
            set
            {
                mColumnSize = value;
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

    public class QBEColumns : Dictionary<string, QBEColumn>
    {
        //public XQBEForm QBEForm;


        public QBEColumns() : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        public QBEColumn Add(string DbColumn, string FriendlyName = "", string DisplayFormat = "", object QBEValue = null, bool UseInQBE = true, bool DisplayInQBEResult = true, int ColumnWidth = 0)
        {
            var x = new QBEColumn();
            return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnsTypes.TextBox, ColumnWidth);

        }
        public QBEColumn Add(string DbColumn, string FriendlyName, string DisplayFormat, object QBEValue, bool UseInQBE, bool DisplayInQBEResult, QBEColumnsTypes QBEColumnType, int ColumnWidth)
        {
            return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnType, ColumnWidth);
        }


        public QBEColumn AddForReport(string ReportName, string DbColumn, string FriendlyName, object QBEValue, QBEColumnsTypes QBEColumnType)
        {
            return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnType, 0);
        }

        public QBEColumn AddForReport(string ReportName, string DbColumn, string FriendlyName, object QBEValue = null)
        {
            return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnsTypes.TextBox, 0);
        }

        public QBEColumn AddForReport(object Report, string DbColumn, string FriendlyName, object QBEValue = null)
        {
            return Add(DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnsTypes.TextBox, 0);
        }

        private QBEColumn Add(string ReportName, string DbColumn, string FriendlyName, string DisplayFormat, object QBEValue, bool UseInQBE, bool DisplayInQBEResult, QBEColumnsTypes QBEColumnType, int ColumnWidth)
        {
            var x = new QBEColumn();

            x.ReportName = ReportName;
            //x.QBEForm = QBEForm;
            x.OrdinalPosition = Count;
            x.DbColumn = DbColumn;
            x.UseInQBE = UseInQBE;
            x.QBEValue = QBEValue;
            x.FriendlyName = FriendlyName;
            x.DisplayInQBEResult = DisplayInQBEResult;
            x.DisplayFormat = DisplayFormat;
            x.QBEColumnType = QBEColumnType;
            x.ColumnSize = ColumnWidth;
            x.Aligment = Wisej.Web.DataGridViewContentAlignment.TopLeft;

            //if (DbColumn.IsNumeric() | DbColumn.IsDate())
            //{
            //    x.Aligment = DataGridViewContentAlignment.TopRight;
            //}
            //if (DbColumn.IsString())
            //{
            //    x.Aligment = DataGridViewContentAlignment.TopLeft;
            //}
            //if (DbColumn.IsTime() | DbColumn.IsDate())
            //{
            //    x.Aligment = DataGridViewContentAlignment.TopRight;
            //}
            //if (DbColumn.IsBoolean())
            //{
            //    x.Aligment = DataGridViewContentAlignment.MiddleCenter;
            //}

            if (string.IsNullOrEmpty(Strings.Trim(x.FriendlyName)))
                x.FriendlyName = x.DbColumn;
            if (ReportName.Trim() != "")
            {
                this.Add(ReportName.Trim().ToUpper() + "|" + DbColumn.Trim().ToUpper(), x);
            }
            else
            {
                this.Add(DbColumn.Trim().ToUpper(), x);
            }

            //List.Add(x);
            return x;
        }





    }


    //[Serializable]
    //public class QBEReportSortColumn
    //{
    //    public string Name { get; set; }
    //    public string FriendlyName { get; set; }
    //    public int Position { get; set; }
    //    public string AscDesc { get; set; }
    //}

    //[Serializable ]
    //public enum ReportTypes
    //{
    //    SSRSLocalReport =0,
    //    SSRSRemoteServer=1
    //}



    //[Serializable]
    //public class QBEReport
    //{
    //    public Dictionary<string, Passero.Framework.RsReports.DataSet> DataSets = new Dictionary<string, RsReports.DataSet>(StringComparer.InvariantCultureIgnoreCase );
    //    public ReportTypes ReportType = ReportTypes.SSRSLocalReport;
    //    public Dapper.DynamicParameters SQLQueryParameters = new Dapper.DynamicParameters();
    //    public Dictionary<string, QBEReportSortColumn> SortColumns = new Dictionary<string, QBEReportSortColumn>(StringComparer.InvariantCultureIgnoreCase);
    //    public Dictionary<string, QBEReportSortColumn> SelectedSortColumns = new Dictionary<string, QBEReportSortColumn>(StringComparer.InvariantCultureIgnoreCase);
    //    public string SQLQuery = "";

    //    private string mReportTitle;
    //    private string mReportFileName;
    //    private string mReportDescription;
    //    private bool mReportUseLike;
    //    public IDbConnection DbConnection { get; set; }
    //    public RsReports.DataSet PrimaryDataSet { get; set; }

    //    public bool SetPrimaryDataSet (string Name)
    //    {
    //        bool result = false;
    //        if (this.DataSets.ContainsKey(Name))
    //        {
    //            this.PrimaryDataSet = this.DataSets[Name];
    //            result = true;  
    //        }

    //        return result;  
    //    }
    //    public Passero.Framework.RsReports.DataSet AddDataSet<T>(string Name, IDbConnection DbConnection, string SQLQuery="", DynamicParameters Parameters =null)
    //    {
    //        Passero.Framework.RsReports.DataSet ds = new RsReports.DataSet();

    //        ds.Name = Name;
    //        ds.DbConnection = DbConnection; 

    //        if (SQLQuery != "")
    //            ds.SQLQuery  = SQLQuery;
    //        if (Parameters != null)
    //            ds.Parameters = Parameters;
    //        ds.ModelType = typeof(T);
    //        ds.EnsureReportDataSet();
    //        this.DataSets.Add(Name, ds);
    //        return ds;  
    //    }


    //    public string OrderBy()
    //    {
    //        string s = "";
    //        foreach (var item in this.SelectedSortColumns .Values )
    //        {
    //            s += $"{item.Name} {item.AscDesc}, "; 
    //        }

    //        s = s.Trim();
    //        if (s.EndsWith (","))
    //        {
    //            s=s.Substring (0,s.Length - 1);
    //        }

    //        if (s != "")
    //            s = $" ORDER BY {s}";
    //        return s;
    //    }


    //    public bool ReportUseLike
    //    {
    //        get
    //        {
    //            bool ReportUseLikeRet = default;
    //            ReportUseLikeRet = mReportUseLike;
    //            return ReportUseLikeRet;

    //        }
    //        set
    //        {
    //            mReportUseLike = value;

    //        }
    //    }
    //    public string ReportFileName
    //    {
    //        get
    //        {
    //            string ReportFileNameRet = default;
    //            ReportFileNameRet = mReportFileName;
    //            return ReportFileNameRet;
    //        }
    //        set
    //        {
    //            mReportFileName = value;

    //        }
    //    }



    //    public string ReportDescription
    //    {
    //        get
    //        {
    //            string ReportDescriptionRet = default;
    //            ReportDescriptionRet = mReportDescription;
    //            return ReportDescriptionRet;

    //        }
    //        set
    //        {
    //            mReportDescription = value;

    //        }
    //    }


    //    public string ReportTitle
    //    {
    //        get
    //        {
    //            string ReportTitleRet = default;
    //            ReportTitleRet = mReportTitle;
    //            return ReportTitleRet;

    //        }
    //        set
    //        {
    //            mReportTitle = value;

    //        }
    //    }
    //}
    //public class QBEReports : Dictionary<string,QBEReport >
    //{

    //    public QBEReport Add(string ReportTitle, string ReportFileName, string ReportDescription = "", IDbConnection DbConnection=null)
    //    {
    //        var x = new QBEReport();

    //        x.ReportDescription = ReportDescription;
    //        x.ReportFileName = ReportFileName;
    //        x.ReportTitle = ReportTitle;
    //        x.DbConnection = DbConnection;  
    //        Add(x.ReportTitle.Trim().ToUpper() ,x);
    //        return x;
    //    }


    //}

}
