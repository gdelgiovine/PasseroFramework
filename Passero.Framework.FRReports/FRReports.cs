using Dapper;
using FastReport;
using FastReport.Data;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Passero.Framework.FRReports
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
    public class QBEFRReport
    {
        public Dictionary<string, Passero.Framework.FRReports.DataSet> DataSets = new Dictionary<string,FRReports.DataSet>(StringComparer.InvariantCultureIgnoreCase);
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
        public FRReports.DataSet PrimaryDataSet { get; set; }

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
        public Passero.Framework.FRReports.DataSet AddDataSet<T>(string Name, IDbConnection DbConnection, string SQLQuery = "", DynamicParameters Parameters = null)
        {
            Passero.Framework.FRReports.DataSet ds = new FRReports.DataSet();

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
    public class QBEReports : Dictionary<string, QBEFRReport >
    {

        public QBEFRReport  Add(string ReportTitle, string ReportFileName, string ReportDescription = "", IDbConnection DbConnection = null)
        {
            var x = new QBEFRReport();

            x.ReportDescription = ReportDescription;
            x.ReportFileName = ReportFileName;
            x.ReportTitle = ReportTitle;
            x.DbConnection = DbConnection;
            Add(x.ReportTitle.Trim().ToUpper(), x);
            return x;
        }


    }



    public enum FRRenderFormat
    {
        XML,
        NULL,
        CSV,
        IMAGE,
        PDF,
        HTML40,
        HTML32,
        MHTML,
        EXCEL,
        WORD
    }
    public class DataSet
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
        public DataSet() 
        {
        }   
        public DataSet(string Name, Type ModelType, IDbConnection DbConnection, Dapper .DynamicParameters Parameters=null)
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
        public Dictionary <string,DataSet> DataSets = new Dictionary<string, DataSet> ();
    }

    public class ReportAfterRenderEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public bool Success { get; set; }   
        public string ReportName { get; set; }
    }
    
    public class FRReport
    {
        public string ReportPath { get; set; }
        public FRRenderFormat ReportFormat { get; set; } = FRRenderFormat.PDF;
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult("Passero.Framework.FRReports.");
        public Dictionary<string, DataSet> DataSets { get; set; } = new Dictionary<string, DataSet>();

        public FastReport.Report Report { get; set; } = new FastReport.Report();

     
        public FRReport()
        {
            Report = new FastReport.Report();
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



      

        public byte[] Render(FRRenderFormat RenderFormat = FRRenderFormat.PDF)
        {
            LastExecutionResult.Reset();
            LastExecutionResult.Context = $"FRReport.Render({RenderFormat})";
            byte[] result = null;
            string f = RenderFormat.ToString();

            
            MsSqlDataConnection SqlConnection = new MsSqlDataConnection();
            Report.Dictionary.Connections.Clear();
            foreach (var dataset in this.DataSets)
            {
                if (dataset.Value.DbConnection.GetType() == typeof(System.Data.SqlClient.SqlConnection))
                {
                    FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection), "MsSqlDataConnection");
                }
            }


            try
            {
                Report.Load(this.ReportPath);


                //Invoke OnReportRenderRequest
                ReportRenderRequestEventArgs requestargs = new ReportRenderRequestEventArgs();
                requestargs.DataSets = new Dictionary<string, DataSet>();
                foreach (string DataSetName in this.DataSetNames())
                {
                    DataSet ds = new DataSet();
                    ds.Name = DataSetName;
                    requestargs.DataSets.Add(DataSetName, ds);
                }
                this.OnReportRenderRequest(requestargs);
                if (requestargs.Cancel)
                {
                    LastExecutionResult.ResultMessage = "Cancelled by User";
                    return null;
                }



                bool datasets_validated = true;
                foreach (var dataset in this.DataSets)
                {
                    TableDataSource table = Report.GetDataSource(dataset.Value.Name) as TableDataSource;
                    if (table != null)
                    {
                        table.Parameters.Clear();
                        table.Connection.ConnectionString = dataset.Value.DbConnection.ConnectionString;
                        table.SelectCommand = dataset.Value.SQLQuery;
                        foreach (var name in dataset.Value.Parameters.ParameterNames)
                        {
                            CommandParameter parameter = new CommandParameter();
                            parameter.Name = name;
                            parameter.DefaultValue = "";
                            parameter.Value = dataset.Value.Parameters.Get<dynamic>(name);
                            parameter.DataType = (int)SqlDbType.VarChar;
                            table.Parameters.Add(parameter);
                        }
                    }
                    else
                    {
                        datasets_validated = false;
                        break;
                    }
                }

                if (datasets_validated == true)
                {
                    Report.Prepare();
                    MemoryStream stream = new MemoryStream();
                    switch (RenderFormat)
                    {
                        case FRRenderFormat.XML:
                            break;
                        case FRRenderFormat.NULL:
                            break;
                        case FRRenderFormat.CSV:
                            break;
                        case FRRenderFormat.IMAGE:
                            break;
                        case FRRenderFormat.PDF:
                            FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                            //pdfExport.Export(Report, @"C:\REPORTS\XREPORT1.pdf");
                            pdfExport.Export(Report,stream );
                            result = stream.ToArray();    
                            break;
                        case FRRenderFormat.HTML40:
                            break;
                        case FRRenderFormat.HTML32:
                            break;
                        case FRRenderFormat.MHTML:
                            break;
                        case FRRenderFormat.EXCEL:
                            
                            break;
                        case FRRenderFormat.WORD:
                            break;
                        default:
                            break;
                    }

                   
                }


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

                foreach (DataSourceBase dataSource in Report.Dictionary.DataSources)
                {
                    string dataSourceName = dataSource.Name;

                    // You can also check its type if needed
                    if (dataSource is TableDataSource)
                    {
                        // It's a TableDataSource
                        TableDataSource tableDataSource = dataSource as TableDataSource;
                        // Additional handling for TableDataSource
                        result.Add(dataSourceName);
                    }
                  
                    
                }


                
            }
            catch (Exception)
            {
                int x = 0;
            }
            return result;
        }

    public ExecutionResult RenderAndSaveReport(string FileName, FRRenderFormat RenderFormat = FRRenderFormat.PDF)
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