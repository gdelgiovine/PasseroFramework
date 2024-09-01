using Dapper;
using Microsoft.VisualBasic;

#if NET
#else
using Microsoft.Reporting.WebForms;
#endif

using Passero.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Passero.Framework.SSRSReports
{

    [Serializable]
    public enum QBEColumnsTypes
    {
        CheckBox = 0,
        ComboBox = 1,
        Image = 2,
        Link = 3,
        TextBox = 4


    }


    public class QBEColumn
    {
        //public XQBEForm QBEForm = null;
        private string mDbColumn;
        private bool mUseInQBE;
        private bool mDisplayInQBEResult;
        private string mFriendlyName = "";
        private string mQBEValue;
        private string mDisplayFormat = "";
        private System.Drawing.Color mBackColor;
        private System.Drawing.Color mForeColor;
        private QBEColumnsTypes mQBEColumnType;
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
                mQBEValue = value.ToString() ;
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




    [Serializable]
    public class QBEReportSortColumn
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public int Position { get; set; }
        public string AscDesc { get; set; }
    }

    [Serializable]
    public enum ReportTypes
    {
        SSRSLocalReport = 0,
        SSRSRemoteServer = 1
    }



    [Serializable]
    public class QBESSRSReport
    {
        public Dictionary<string, Passero.Framework.SSRSReports.ReportDataSet> DataSets = new Dictionary<string, SSRSReports.ReportDataSet>(StringComparer.InvariantCultureIgnoreCase);
        public ReportTypes ReportType = ReportTypes.SSRSLocalReport;
        public Dapper.DynamicParameters SQLQueryParameters = new Dapper.DynamicParameters();
        public Dictionary<string, QBEReportSortColumn> SortColumns = new Dictionary<string, QBEReportSortColumn>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, QBEReportSortColumn> SelectedSortColumns = new Dictionary<string, QBEReportSortColumn>(StringComparer.InvariantCultureIgnoreCase);
        public string SQLQuery = "";

        private string mReportTitle;
        private string mReportFileName;
        private string mReportDescription;
        private bool mReportUseLike;
        public IDbConnection DbConnection { get; set; }
        public SSRSReports.ReportDataSet PrimaryDataSet { get; set; }

        public bool SetPrimaryDataSet(string Name)
        {
            bool result = false;
            if (this.DataSets.ContainsKey(Name))
            {
                this.PrimaryDataSet = this.DataSets[Name];
                result = true;
            }

            return result;
        }
        public Passero.Framework.SSRSReports.ReportDataSet AddDataSet<T>(string Name, IDbConnection DbConnection, string SQLQuery = "", DynamicParameters Parameters = null)
        {
            Passero.Framework.SSRSReports.ReportDataSet ds = new SSRSReports.ReportDataSet();

            ds.Name = Name;
            ds.DbConnection = DbConnection;

            if (SQLQuery != "")
                ds.SQLQuery = SQLQuery;
            if (Parameters != null)
                ds.Parameters = Parameters;
            ds.ModelType = typeof(T);
            ds.EnsureReportDataSet();
            this.DataSets.Add(Name, ds);
            return ds;
        }


        public string OrderBy()
        {
            string s = "";
            foreach (var item in this.SelectedSortColumns.Values)
            {
                s += $"{item.Name} {item.AscDesc}, ";
            }

            s = s.Trim();
            if (s.EndsWith(","))
            {
                s = s.Substring(0, s.Length - 1);
            }

            if (s != "")
                s = $" ORDER BY {s}";
            return s;
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
    public class QBEReports : Dictionary<string, QBESSRSReport>
    {

        public QBESSRSReport Add(string ReportTitle, string ReportFileName, string ReportDescription = "", IDbConnection DbConnection = null)
        {
            var x = new QBESSRSReport();

            x.ReportDescription = ReportDescription;
            x.ReportFileName = ReportFileName;
            x.ReportTitle = ReportTitle;
            x.DbConnection = DbConnection;
            Add(x.ReportTitle.Trim().ToUpper(), x);
            return x;
        }


    }



    public enum RenderFormat
    {


    //HTML4.0 / HTML5 / MHTML
    //PDF (*)
    //IMAGE (TIFF/EMF) (*)
    //EXCEL (Microsoft Excel 97/2003) (*)
    //EXCELOPENXML (Microsoft Excel Open XML)
    //WORD (Microsoft Word 97/2003) (*)
    //WORDOPENXML (Microsoft Word Open XML)

       
        TIFF,
        EMF,
        PDF,
        HTML40,
        HTML5,
        MHTML,
        EXCEL,
        EXCELOPENXML,
        WORD,
        WORDOPENXML
    }
    public class ReportDataSet
    {
        public string Name { get; set; } = string.Empty;
        public System.Data.IDbConnection DbConnection { get; set; }
        public Dapper.DynamicParameters Parameters { get; set; }= new DynamicParameters (); 
        public string SQLQuery { get; set; }    
        //public object Repository { get; set; }
        public Type ModelType { get; set; }
        public Dictionary<string, System.Reflection.PropertyInfo> ModelProperties= new Dictionary<string, System.Reflection.PropertyInfo>();
        public object Data { get; set; }    
        public object Model { get; set; }
        public ReportDataSet() 
        {
        }   
        public ReportDataSet(string Name, Type ModelType, IDbConnection DbConnection, Dapper .DynamicParameters Parameters=null)
        {
            this.Name = Name;   
            this.ModelType= ModelType;  
            this.DbConnection = DbConnection;   
            this.Parameters = Parameters;
            
            EnsureReportDataSet ();
        }   

        public bool EnsureReportDataSet()
        {
            if (this.ModelType != null && this.DbConnection != null)
            {
                object obj = Activator.CreateInstance(ModelType);
                this.Model = obj;   
                Passero.Framework.ReflectionHelper.SetPropertyValue(ref obj, "DbConnection", DbConnection);
                Passero.Framework.ReflectionHelper.SetPropertyValue(ref obj, "Parameters", Parameters);
                //this.Repository = obj;

                
                this.ModelProperties.Clear();
                foreach (var item in Passero.Framework.DapperHelper.Utilities.GetPropertiesInfo(ModelType))
                {
                    this.ModelProperties.Add(item.Name, item);
                }
                

                return true;
            }
            return false;   
        }

        public void LoadData()
        {
            this.Data = this.DbConnection.Query(this.SQLQuery, this.Parameters);
            
        }
    }

    public class ReportRenderRequestEventArgs : EventArgs
    {
        public bool Cancel {  get; set; }   
        public string ReportName { get; set; }  
        public Dictionary <string,ReportDataSet> DataSets = new Dictionary<string, ReportDataSet> ();
    }

    public class ReportAfterRenderEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public bool Success { get; set; }   
        public string ReportName { get; set; }
    }
    
    public class SSRSReport
    {
        public string ReportPath { get; set; }
        public RenderFormat ReportFormat { get; set; } = RenderFormat.PDF;
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult("Passero.Framework.Reports.SSRSReport.");
        public Dictionary<string, ReportDataSet> DataSets { get; set; } = new Dictionary<string, ReportDataSet>();


#if NET
        public Microsoft.Reporting.NETCore.LocalReport Report { get; set; } 
      
#else
        public Microsoft.Reporting.WebForms.LocalReport Report { get; set; }
      
#endif
     
        public SSRSReport()
        {
        
#if NET
            Report = new Microsoft.Reporting.NETCore.LocalReport();
#else
            Report = new Microsoft.Reporting.WebForms.LocalReport();
#endif

        }


        public event EventHandler ReportRenderRequest;

        protected virtual void OnReportRenderRequest(ReportRenderRequestEventArgs e)
        {
            ReportRenderRequest?.Invoke(this, e);
        }

        public event EventHandler ReportAfterRender;

        protected virtual void OnReportAfterRender(ReportAfterRenderEventArgs  e)
        {
            ReportAfterRender?.Invoke(this, e);
        }

        public byte[] Render(string Format,string deviceinfo=null)
        {
            LastExecutionResult.Reset();
            LastExecutionResult.Context = $"Passero.Framework.Reports.SSRSReports.Render({Format})";
            byte[] result = null;

            try
            {
                Report.ReportPath = this.ReportPath;
                IList<string> DataSetNames = Report.GetDataSourceNames();

                // Invoke OnReportRenderRequest
                ReportRenderRequestEventArgs  requestargs = new ReportRenderRequestEventArgs();
                requestargs.DataSets = new Dictionary<string, ReportDataSet>();
                foreach (string DataSetName in this.DataSetNames() )
                {
                    ReportDataSet ds = new ReportDataSet();
                    ds.Name= DataSetName;
                    requestargs.DataSets.Add(DataSetName, ds);
                }
                this.OnReportRenderRequest (requestargs);   
                if (requestargs .Cancel ) 
                {
                    LastExecutionResult.ResultMessage = "Cancelled by User";
                    return null;
                }

                Report.DataSources.Clear();

                if (requestargs .DataSets != null && requestargs .DataSets.Count > 0) 
                {
                    bool ok = true;
                    foreach (var item in requestargs .DataSets .Values )
                    {
                        if (item.Data ==null) 
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok) this.DataSets = requestargs.DataSets;                
                }

                foreach (var item in this.DataSets.Values)
                {
                   item.EnsureReportDataSet();
#if NET
                   Microsoft.Reporting.NETCore.ReportDataSource  ds = new Microsoft.Reporting.NETCore .ReportDataSource(item.Name ,item.Data);
                   Report.DataSources.Add(ds);
#else
                   Microsoft.Reporting.WebForms.ReportDataSource ds = new Microsoft.Reporting.WebForms.ReportDataSource(item.Name ,item.Data);
                   Report.DataSources.Add(ds);
#endif
                }

                result = Report.Render(Format,deviceinfo);
                
            }
            catch (Exception ex)
            {
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultMessage = ex.Message;
                LastExecutionResult.ErrorCode = 1;
            }
            return result;
        }

        public List<string> DataSetNames()
        {
            List<string> result = new List<string>();   
            try
            {
                result= Report.GetDataSourceNames().ToList();
            }
            catch (Exception)
            {

            }
            return result;
        }

    public ExecutionResult RenderAndSaveReport(string FileName, string RenderFormat = "pdf")
        {
            var ER = new ExecutionResult();
            ER.Context = $"Passero.Framework.Reports.SSRSReports.RenderAndSaveReport({FileName},{RenderFormat})";
            byte[] result = null;

            result = Render(RenderFormat);
            if (LastExecutionResult.Failed)
            {
                ER = LastExecutionResult;
                return ER;
            }

            if (result is null)
            {
                ER.ErrorCode = 2;
                ER.ResultMessage = "Empty Rendering.";
                LastExecutionResult = ER;
                return ER;
            }

            try
            {
                Utilities.SaveByteArrayToFile(result, FileName);
            }

            catch (Exception ex)
            {
                ER.ErrorCode = 3;
                ER.ResultMessage = ex.Message;
                LastExecutionResult = ER;
                return ER;
            }

            return ER;

        }

    }
}