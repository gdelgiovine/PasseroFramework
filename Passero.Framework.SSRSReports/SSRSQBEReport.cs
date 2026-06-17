
using Dapper;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;
using Wisej.Web;

namespace Passero.Framework.SSRSReports
{

    public partial class ReportManager : Form
    {


        /// <summary>
        /// Shows or hides the Save/Load buttons for QBE queries.   
        /// </summary>
        public bool ShowSaveLoadButtons
        {
            get
            {
                return bSaveQBE.Visible && bLoadQBE.Visible;
            }
            set
            {
                bSaveQBE.Visible = value;
                bLoadQBE.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the QueryGridQuerySave call back action.
        /// </summary>
        /// <value>
        /// The call back action.
        /// </value>
        public Action<string> QueryGridQuerySaveCallBackAction { get; set; }
        /// <summary>
        /// Gets or sets the QueryGridQueryLoad call back action.
        /// </summary>
        /// <value>
        /// The call back action.
        /// </value>
        public Action QueryGridQueryLoadCallBackAction { get; set; }

        private string RenderError_NoReport = "No Report!";
        private string RenderError_NoDataSets = "No Report DataSets!";
        private string RenderError = "";
        private string CurrentReportName;
        public bool RaiseReportEvents= false;
        public Passero.Framework.SSRSReports.QBEColumns QBEColumns = new QBEColumns();

        public QBEReports QBEReports = new QBEReports();    
        public QBESSRSReport DefaultReport { get; set; }
        public bool PrintDefaultReport { get; set; } = false;
        public string ReportsPath { get; set; } = Wisej.Web.Application.StartupPath + "reports";
        public string ReportsPDFUrlPath { get; set; } = @"\Reports\";
        public string ReportFileName { get; set; } = "";
        public string ReportName { get; set; } = "";
        public string SQLQuery { get; set; } = "";
        public DynamicParameters SQLQueryParameters { get; set; } = new DynamicParameters();

        private  SSRSReport SSRSReport = new SSRSReport();

        public event EventHandler ReportRenderRequest;

        protected virtual void OnReportRenderRequest(ReportRenderRequestEventArgs e)
        {
            ReportRenderRequest?.Invoke(this, e);
        }

        public event EventHandler ReportAfterRender;

        protected virtual void OnReportAfterRender(ReportAfterRenderEventArgs e)
        {
            ReportAfterRender?.Invoke(this, e);
        }



        public bool UseLikeOperator
        {
            get { return this.chkLikeOperator.Checked; }
            set { this.chkLikeOperator.Checked = value; }
        }

       
        public Action CallBackAction { get; set; }
       

        public Control SetFocusControlAfterClose { get; set; }

        public ReportManager()
        {
            InitializeComponent();
            this.bSaveQBE.Visible = false;
            this.bLoadQBE.Visible = false; 
            this.cmbRecords.Visible = false;   
            this.RecordLabel.Visible = false;
            this.Records.Visible = false;
            this.bNext.Visible = false; 
            this.bPrev.Visible = false;    
            this.bFirst.Visible = false;    
            this.bLast .Visible = false;    
            this.bPrint .Visible = false;
            this.bSave .Visible = false;    
            this.TabPageDebug.Hidden = true;
            this.TabPageExport.Hidden = true;
            this.TabPageReportQuery.Hidden = true;
            this.TabPageReports.Hidden = true;
            this.TabPageReportSort.Hidden = true;
         
        }

        private void SSRSReport_ReportRenderRequest(object sender, EventArgs e)
        {
            if (this.RaiseReportEvents)
            {
                ReportRenderRequestEventArgs args = (ReportRenderRequestEventArgs)e;
                this.OnReportRenderRequest(args);
            }
        }



        public void ExportResultGrid()
        {
            string filename = "";
            filename = System.IO.Path.GetTempPath() + @"\" + System.Guid.NewGuid().ToString();
            string exportfilename = Passero.Framework.FileHelper.GetSafeFileName(this.Text);
            string exportfilenameextension = ".xml";
            System.IO.MemoryStream Stream = null;
            QBESSRSReport Report = this.QBEReports[CurrentReportName];
            RenderFormat format = RenderFormat.EXCEL ;
            bool NativeRenderFormat = false;
          
            if (rbHTML.Checked)
            {
                format = RenderFormat.HTML5;
                exportfilenameextension = ".html";
                NativeRenderFormat = true;
            }

            if (rbMHTML.Checked)
            {
                format = RenderFormat.MHTML ;
                exportfilenameextension = ".mhtml";
                NativeRenderFormat = true;
            }

            if (rbIMAGETIFF .Checked)
            {
                format = RenderFormat.TIFF  ;
                exportfilenameextension = ".tiff";
                NativeRenderFormat = true;
                
            }

            if (rbIMAGEEMF .Checked)
            {
                format = RenderFormat.EMF ;
                exportfilenameextension = ".emf";
                NativeRenderFormat = true;

            }


            if (rbExcel.Checked)
            {
                format = RenderFormat.EXCEL;
                exportfilenameextension = ".xls";
                NativeRenderFormat = true;
            }
            if (rbEXCELXML .Checked)
            {
                format = RenderFormat.EXCELOPENXML ;
                exportfilenameextension = ".xlsx";
                NativeRenderFormat = true;
            }

                   
            if (rbWord.Checked)
            {
                format = RenderFormat.WORD ;
                exportfilenameextension = ".doc";
                NativeRenderFormat = true;
            }
            if (rbWORDXML.Checked)
            {
                format = RenderFormat.WORDOPENXML ;
                exportfilenameextension = ".docx";
                NativeRenderFormat = true;
            }

            BuildQuery3();
            byte[] ReportBytes = null;

            if (NativeRenderFormat)
            {
                ReportBytes = this.RenderReport(Report, format);
            }
            else
            {
                
                

            }
            
            if (ReportBytes != null && exportfilenameextension != "")
            {

                Stream = new MemoryStream(ReportBytes);
                Wisej.Web.Application.Download(Stream, exportfilename + exportfilenameextension);
                Stream.Close();
            }

        }
        public void ShowQBEReport(bool ShowModalMask=false)
        {
            this.SetupQBEReport();
            if (this.ReportGrid.CurrentRow != null)
                this.SetupQueryGrid(this.ReportGrid.CurrentRow[0].Value.ToString().Trim().ToUpper());

            this.ShowModalMask = ShowModalMask;
            this.Show();
        }


        public void SetupQBEReport()
        {

            this.TabPageReportQuery.Hidden = false;
            this.TabPageReports.Hidden = false;
            this.TabPageReportSort.Hidden = false;
            this.TabPageExport.Hidden = false;
            this.TabPageDebug.Hidden = true;
            this.PanelExport.Visible = true;
            this.PanelReportViewer.Visible = true;
            this.PanelReportViewer.Dock = DockStyle.Fill;
            this.QueryGrid.Visible = true;
            this.ReportGrid.Dock = DockStyle.Fill;
            this.ReportGrid.Visible = true;

            this.TabControl.Visible = true;
            SetupReportGrid();
            //SetupQueryGrid();


            if (this.Owner == null && this.SetFocusControlAfterClose != null)
                this.Owner = Passero.Framework.Utilities.GetParentOfType<Form>(this.SetFocusControlAfterClose);

            if (this.Owner.MdiParent != null)
                this.MdiParent = this.Owner.MdiParent;
        }


        private void SetupReportViewer()
        {
            this.PanelReportViewer.Dock = DockStyle.Fill;
            this.PanelReportViewer.Visible = true;
            this.ReportGrid.Visible = true;
        }


        private void SetupReportGrid()
        {
            this.ReportGrid.Rows.Clear();
            int i = 0;
            foreach (var QBEReport in this.QBEReports.Values)
            {
                ReportGrid.Rows.Add();
                {
                    var row = ReportGrid.Rows[i];
                    row.Cells[dgvcReportName.Name].Value = QBEReport.ReportTitle;
                    row.Cells[dgvcReportDescription.Name].Value = QBEReport.ReportDescription;
                    row.Cells[dgvcReportFileName.Name].Value = QBEReport.ReportFileName;
                    row.Tag = QBEReport.ReportFileName;
                }
                i = i + 1;
            }
            ReportGrid.AutoResizeColumn(0);
            if (ReportGrid .Rows.Count >0)
            {
                this.CurrentReportName = ReportGrid.Rows[0][0].Value.ToString().Trim().ToUpper ();  
            }    
        }


        private void LoadPDFViewer(string ReportName)
        {
            ReportName = ReportName.Trim().ToUpper();
            QBESSRSReport report = this.QBEReports[ReportName];
            if (report==null)
            {
                this.RenderError =this.RenderError_NoReport ;
                MessageBox.Show(this.RenderError, this.Text,MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (report.DataSets == null )
            {
                this.RenderError = this.RenderError_NoDataSets;
                MessageBox.Show(this.RenderError, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.txtRenderError.Visible = false;
          
            byte[] ReportBytes = this.RenderReport(ReportName, RenderFormat.PDF);
          
            if (ReportBytes != null)
            {
                //if (this.PdfViewer.Dock != DockStyle.Fill) this.PdfViewer.Dock = DockStyle.Fill;
                this.PdfViewer.PdfStream = new MemoryStream(ReportBytes);
                this.PdfViewer.Visible = true;
                //Passero.Framework.Utilities.SaveByteArrayToFile(ReportBytes, @"c:\REports\report1.pdf");
                
            }
            else
            {
                this.txtRenderError.Text = this.RenderError;
                this.txtRenderError.Dock = DockStyle.Fill;
                this.txtRenderError.Visible = true;
            }
          

        }


        private byte[] RenderReport(string ReportName, RenderFormat Format = RenderFormat.PDF)
        {

            ReportName = ReportName.Trim().ToUpper();
            QBESSRSReport Report = this.QBEReports[ReportName];
            if (Report == null)
            {
                this.RenderError = this.RenderError_NoReport;
                MessageBox.Show(this.RenderError , this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (Report.DataSets == null)
            {
                this.RenderError = this.RenderError_NoDataSets;
                MessageBox.Show(this.RenderError , this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return RenderReport(Report, Format);
            

        }


        private byte[] RenderReport(QBESSRSReport Report, RenderFormat Format = RenderFormat.PDF)
        {

            if (Report == null)
            {
                MessageBox.Show(this.RenderError_NoReport , this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (Report.DataSets == null)
            {
                MessageBox.Show(this.RenderError_NoDataSets , this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

       
            SSRSReport = new SSRSReports.SSRSReport();
            if (RaiseReportEvents)
            {
                this.SSRSReport.ReportRenderRequest -= SSRSReport_ReportRenderRequest;
                this.SSRSReport.ReportRenderRequest += SSRSReport_ReportRenderRequest;
            }
            SSRSReport.ReportPath = Report.ReportFileName;
            SSRSReport.ReportFormat = Format;
            SSRSReport.DataSets = Report.DataSets;
            string DeviceInfo = null;
            string _Format = Format.ToString();
            switch (Format)
            {

                case RenderFormat.TIFF:
                    _Format = "IMAGE";
                    DeviceInfo = "<DeviceInfo><OutputFormat>TIFF</OutputFormat><DpiX>300</DpiX><DpiY>300</DpiY></DeviceInfo>";
                    break;
                case RenderFormat.EMF:
                    _Format = "IMAGE";
                    DeviceInfo = "<DeviceInfo><OutputFormat>EMF</OutputFormat></DeviceInfo>";
                    break;
                case RenderFormat.PDF:
                    break;
                case RenderFormat.HTML40:
                    break;
                case RenderFormat.HTML5:
                    break;
                case RenderFormat.MHTML:
                    break;
                case RenderFormat.EXCEL:
                    break;
                case RenderFormat.EXCELOPENXML:
                    break;
                case RenderFormat.WORD:
                    break;
                case RenderFormat.WORDOPENXML:
                    break;
                default:
                    break;
            }

            byte[] ReportBytes = SSRSReport.Render(_Format,DeviceInfo);
            if (SSRSReport.LastExecutionResult.Exception != null)
            {
                this.RenderError = SSRSReport.LastExecutionResult.ResultMessage + "\n" + SSRSReport.LastExecutionResult.Exception.ToString();
            }
            return ReportBytes;

        }

      
     

        private void SetupQueryGrid(string ReportName)
        {

            QBESSRSReport report = this.QBEReports[ReportName];
            SetupQueryGrid(report);
        }


        private void SetupQueryGrid(QBESSRSReport QBEReport)
        {

            if (QBEReport.DataSets.Count == 0)
                return;

            SSRSReports.ReportDataSet PrimaryDataSet = QBEReport.PrimaryDataSet;
            if (PrimaryDataSet == null)
            {
                PrimaryDataSet = QBEReport.DataSets.Values.First();
            }
            this.QueryGrid.Rows.Clear();
            this.lstSortColumns.DataSource = null;
            this.lstSortColumns .Items .Clear();
            QBEReport.SortColumns.Clear();
            foreach (var QBEColumn in this.QBEColumns.Values)
            {

                if (QBEColumn.ReportName.Trim().Equals(this.ReportGrid.CurrentRow[0].Value.ToString().Trim(), StringComparison.InvariantCultureIgnoreCase) && QBEColumn.UseInQBE)
                {
                    int i;

                    i = this.QueryGrid.Rows.Add(QBEColumn.FriendlyName, QBEColumn.QBEValue);
                    // SortColumns
                    QBEReportSortColumn  sc  = new QBEReportSortColumn();
                    sc.Name = QBEColumn.DbColumn;
                    sc.FriendlyName = QBEColumn.FriendlyName;
                    sc.Position = i;
                    sc.AscDesc = "ASC";
                    QBEReport.SortColumns.Add(sc.Name, sc);
        
                    Type PropertyType = PrimaryDataSet.ModelProperties[QBEColumn.DbColumn].PropertyType;
                    Framework.EnumSystemTypeIs PropertyTypeIs = Framework.Utilities.GetSystemTypeIs(PropertyType);
                    //if (Passero.Framework.Utilities.GetSystemTypeIs(PropertyType) == EnumSystemTypeIs.Boolean)
                    if (PropertyTypeIs == EnumSystemTypeIs.Boolean)
                    {
                        DataGridViewCheckBoxCell ncell = new DataGridViewCheckBoxCell();
                        ncell.ThreeState = true;
                        ncell.IndeterminateValue = "";
                        ncell.TrueValue = true;
                        ncell.FalseValue = false;
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(QBEColumn.QBEValue, null, false)))
                        {
                            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(QBEColumn.QBEValue, "True", false)))
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
                        this.QueryGrid.Rows[i].Cells[1] = ncell;
                        this.QueryGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    this.QueryGrid.Rows[i].Tag = QBEColumn.DbColumn;


                }
            }

            this.txtReportTitle.Width = this.PanelReportInfo.Width * 40 / 100;
            this.txtReportDescription   .Width = this.PanelReportInfo.Width * 60 / 100;
            this.txtReportTitle.Text = QBEReport.ReportTitle  ;
                this.txtReportDescription.Text = QBEReport.ReportDescription;
            
            this.lstSortColumns.DataSource = QBEReport.SortColumns.Values.ToList();
            this.lstSortColumns.ValueMember = "Name";
            this.lstSortColumns.DisplayMember = "FriendlyName";
        }

        public void LoadData()
        {
            this.DoQuery();
        }

        public void DoQuery()
        {
            BuildQuery3();
            this.LoadPDFViewer(this.CurrentReportName);
            
        }

        private void MovePrevious()
        {




            // SendKeys.Send("{UP}")

        }
        private void MoveNext()
        {



        }
        private async void MoveFirst()
        {
          // await this.PdfViewer.EvalAsync("this.__objectEl.__element.contentWindow.PDFViewerApplication.page=1");
          


        }
        private async void MoveLast()
        {
           //await this.PdfViewer.EvalAsync("this.__objectEl.__element.contentWindow.PDFViewerApplication.page=this.__objectEl.__element.contentWindow.PDFViewerApplication.pagesCount");


        }


        private void BuildQuery3_OLD()
        {
            if (this.QBEReports[this.CurrentReportName].DataSets.Count == 0)
                return;

            QBESSRSReport Report = this.QBEReports[this.CurrentReportName];

            Passero.Framework.SSRSReports.ReportDataSet PrimaryDataSet = Report.PrimaryDataSet;
            if (PrimaryDataSet == null)
            {
                PrimaryDataSet = Report.DataSets.Values.First();
            }

            StringBuilder sqlwhere = new StringBuilder();
            this.QueryGrid.EndEdit();
            DynamicParameters parameters = new DynamicParameters();

                
                string _WhereAND = "";
              
                foreach (var item in this.QueryGrid.Rows)
                {
                    StringBuilder sqlwhereitem = new StringBuilder();
                    string Value = "";
                    if (item[1].Value != null)
                        Value = item[1].Value.ToString();

                    string _WhereItemOR = "";
                    string[] Values;
                    Type PropertyType = PrimaryDataSet.ModelProperties[item.Tag.ToString()].PropertyType;
                    Passero.Framework.EnumSystemTypeIs PropertyTypeIs = Passero.Framework.Utilities.GetSystemTypeIs(PropertyType);
                    if (!string.IsNullOrEmpty(Strings.Trim(Value)) | !string.IsNullOrEmpty(Value))
                    {
                        Value = Value.Trim();


                        if (Value != ";")
                        {
                            Values = Strings.Split(Value, ";");
                        }
                        else
                        {
                            Values = new string[1];
                            Values[0] = ";";
                        }

                        int i = 1;
                        foreach (var _Value in Values)
                        {
                            string parametername = $"@{item.Tag.ToString()}_{i.ToString().Trim()}";
                            // GESTIONE PRIORITARIA PER COLONNE BOOLEAN/BIT
                            if (PropertyTypeIs == Passero.Framework.EnumSystemTypeIs.Boolean)
                            {
                                // Converte il valore stringa in boolean
                                bool boolValue;
                                if (bool.TryParse(_Value, out boolValue))
                                {
                                    sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()} = {parametername}");
                                    parameters.Add(parametername, boolValue, System.Data.DbType.Boolean);
                                }
                                // Se il valore non è un boolean valido, salta questa condizione
                            }
                            else if (this.chkLikeOperator.Checked)
                            {
                                sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()} Like {parametername} ");
                                parameters.Add(parametername, "%" + _Value + "%", Passero.Framework.Utilities.GetDbType(PropertyType));
                            }
                            else
                            {
                                sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()}{GetComparisionOperator(_Value)}{parametername}");
                                parameters.Add(parametername, RemoveComparisionOperator(_Value), Passero.Framework.Utilities.GetDbType(PropertyType));
                            }

                            if (sqlwhereitem.Length > 0)
                            {
                                _WhereItemOR = " OR ";
                            }
                            i++;
                        }
                        if (sqlwhere.Length > 0)
                        {
                            _WhereAND = " AND ";
                        }
                        sqlwhere.Append($" {_WhereAND} ( {sqlwhereitem.ToString()} )");
                    }
                }

                if (PrimaryDataSet.SQLQuery == null || PrimaryDataSet.SQLQuery.Trim() == "")
                {
                    this.SQLQuery = $"SELECT * FROM {Passero.Framework.Utilities.GetModelTableName(PrimaryDataSet.Model)}";
                    if (sqlwhere.ToString().Trim() != "")
                        this.SQLQuery = this.SQLQuery + $" WHERE {sqlwhere.ToString()}";
                    this.SQLQueryParameters = parameters;
                }
                else
                {
                    
                    this.SQLQuery = $"SELECT * FROM ({PrimaryDataSet.SQLQuery}) AS X";
                    if (sqlwhere.ToString().Trim() != "")
                        this.SQLQuery = this.SQLQuery + $" WHERE {sqlwhere.ToString()}";
                    parameters.AddDynamicParams(PrimaryDataSet.Parameters); 
                    
                    this.SQLQueryParameters = parameters;
                }
 
                 
                this.SQLQuery += " " + Report.OrderBy();

            Report.SelectedSortColumns.Clear();
            foreach (DataGridViewRow row in this.dgv_SelectedSortColumns.Rows)
            {
                QBEReportSortColumn column = new QBEReportSortColumn();
                column.Name = (string)row[this.dgvc_SelectedSortColumns_name].Value;
                column.Position = (int)row[this.dgvc_SelectedSortColumns_position].Value;
                column.FriendlyName = (string)row[this.dgvc_SelectedSortColumns_friendlyname].Value;
                column.AscDesc = (string)row[this.dgvc_SelectedSortColumns_ascdesc].Value;
                Report.SelectedSortColumns.Add(column.Name, column);
            }



            
            PrimaryDataSet.LoadData(SQLQuery, SQLQueryParameters);


            //Load Data for other datasets
            foreach (Passero.Framework.SSRSReports.ReportDataSet DataSet in Report.DataSets.Values )
            {
                if (DataSet != PrimaryDataSet )
                {
                    if (DataSet.SQLQuery ==null || DataSet.SQLQuery.Trim()== "")
                    {
                        DataSet.SQLQuery = $"SELECT * FROM {Passero.Framework.Utilities.GetModelTableName(DataSet.Model)}";
                    }
                    DataSet.LoadData (DataSet.SQLQuery,DataSet.Parameters );    
                }
            }
           
        }


        private void BuildQuery3()
        {
            if (this.QBEReports[this.CurrentReportName].DataSets.Count == 0)
                return;

            QBESSRSReport Report = this.QBEReports[this.CurrentReportName];

            Passero.Framework.SSRSReports.ReportDataSet PrimaryDataSet = Report.PrimaryDataSet;
            if (PrimaryDataSet == null)
            {
                PrimaryDataSet = Report.DataSets.Values.First();
            }

            StringBuilder sqlWhere = new StringBuilder();
            this.QueryGrid.EndEdit();
            DynamicParameters parameters = new DynamicParameters();

            foreach (DataGridViewRow row in this.QueryGrid.Rows)
            {
                string value = null;
                object cellValue = row?.Cells?[1]?.Value;

                // Gestisci i diversi tipi di celle
                if (cellValue == null || cellValue.Equals(""))
                {
                    // Valore nullo o stringa vuota (indeterminato) - salta il filtro
                    continue;
                }
                else if (cellValue is bool boolValue)
                {
                    // Converti bool a "true" o "false"
                    value = boolValue ? "true" : "false";
                }
                else if (cellValue is int intValue)
                {
                    // Converti int (da checkbox 0/1) a stringa
                    value = intValue.ToString();
                }
                else
                {
                    // Per altri tipi, converti a stringa
                    value = cellValue.ToString();
                }

                // Salta se il valore è vuoto
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                string columnName = row.Tag?.ToString();
                if (string.IsNullOrWhiteSpace(columnName))
                    continue;

                if (!PrimaryDataSet.ModelProperties.ContainsKey(columnName))
                    continue;

                Type propertyType = PrimaryDataSet.ModelProperties[columnName].PropertyType;
                bool isCodeColumn = CodeColumns.Contains(columnName);

                var build = Framework.Controls.NavFilterSqlEngine.BuildColumnPredicate(
                    columnName: columnName,
                    propertyType: propertyType,
                    filterText: value.Trim(),
                    isCodeColumn: isCodeColumn,
                    parameterPrefix: $"f_{columnName}_{row.Index}",
                    parameters: parameters,
                    options: new Passero.Framework.Controls.NavFilterSqlOptions
                    {
                        Culture = FilterCulture,
                        CaseInsensitiveText = FilterCaseInsensitiveText,
                        AllowRelativeDateTokens = EnableRelativeDateTokens,
                        AllowTextRelationalOperators = AllowTextRelationalOperators,
                        UseLikeOperator = UseLikeOperator
                    });

                if (build.Errors.Count > 0)
                {
                    Passero.Framework.Controls.NavFilterError first = build.Errors[0];
                    string technical = $"{first.Code}: {first.TechnicalMessage}";
                    string user = first.UserMessage;
                    MessageBox.Show($"{user}\n{technical}", "Filtro non valido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new InvalidOperationException(technical);
                }

                if (!string.IsNullOrWhiteSpace(build.Sql))
                {
                    if (sqlWhere.Length > 0)
                        sqlWhere.Append(" AND ");

                    sqlWhere.Append("(");
                    sqlWhere.Append(build.Sql);
                    sqlWhere.Append(")");
                }
            }

            Report.SelectedSortColumns.Clear();
            foreach (DataGridViewRow row in this.dgv_SelectedSortColumns.Rows)
            {
                QBEReportSortColumn column = new QBEReportSortColumn();
                column.Name = (string)row[this.dgvc_SelectedSortColumns_name].Value;
                column.Position = (int)row[this.dgvc_SelectedSortColumns_position].Value;
                column.FriendlyName = (string)row[this.dgvc_SelectedSortColumns_friendlyname].Value;
                column.AscDesc = (string)row[this.dgvc_SelectedSortColumns_ascdesc].Value;
                Report.SelectedSortColumns.Add(column.Name, column);
            }

            if (PrimaryDataSet.SQLQuery == null || PrimaryDataSet.SQLQuery.Trim() == "")
            {
                this.SQLQuery = $"SELECT * FROM {Passero.Framework.Utilities.GetModelTableName(PrimaryDataSet.Model)}";
                if (sqlWhere.ToString().Trim() != "")
                    this.SQLQuery = this.SQLQuery + $" WHERE {sqlWhere.ToString()}";
                this.SQLQueryParameters = parameters;
            }
            else
            {
                this.SQLQuery = $"SELECT * FROM ({PrimaryDataSet.SQLQuery}) AS X";
                if (sqlWhere.ToString().Trim() != "")
                    this.SQLQuery = this.SQLQuery + $" WHERE {sqlWhere.ToString()}";
                parameters.AddDynamicParams(PrimaryDataSet.Parameters);

                this.SQLQueryParameters = parameters;
            }

            this.SQLQuery += " " + Report.OrderBy();

            PrimaryDataSet.LoadData(SQLQuery, SQLQueryParameters);

            //Load Data for other datasets
            foreach (Passero.Framework.SSRSReports.ReportDataSet DataSet in Report.DataSets.Values)
            {
                if (DataSet != PrimaryDataSet)
                {
                    if (DataSet.SQLQuery == null || DataSet.SQLQuery.Trim() == "")
                    {
                        DataSet.SQLQuery = $"SELECT * FROM {Passero.Framework.Utilities.GetModelTableName(DataSet.Model)}";
                    }
                    DataSet.LoadData(DataSet.SQLQuery, DataSet.Parameters);
                }
            }
        }



        private string GetComparisionOperator(string Value)
        {

            Value = Value.ToUpper();
            string _operator = "";
            if (Value.StartsWith("="))
                _operator = " = ";
            if (Value.StartsWith(">"))
                _operator = " > ";
            if (Value.StartsWith("<"))
                _operator = " < ";
            if (Value.StartsWith(">="))
                _operator = " >= ";
            if (Value.StartsWith("<="))
                _operator = " <= ";
            if (Value.StartsWith("<>"))
                _operator = " <> ";
            if (Value.StartsWith("LIKE ", StringComparison.CurrentCultureIgnoreCase))
                _operator = " LIKE ";
            if (Value.StartsWith("NOT LIKE ", StringComparison.CurrentCultureIgnoreCase))
                _operator = " NOT LIKE ";
            if (_operator == "")
                _operator = " = ";

            return _operator;
        }
        private string RemoveComparisionOperator(string Value)
        {
            string op = GetComparisionOperator(Value).Trim();

            if (Value.StartsWith(op, StringComparison.CurrentCultureIgnoreCase) == false)
                Value = op + Value;
            return Value.Substring(op.Length).Trim();
        }

        private void XQBEForm_Load(object sender, EventArgs e)
        {

        }

        private void bPrev_Click(object sender, EventArgs e)
        {
            this.MovePrevious();
        }

        private void bFirst_Click(object sender, EventArgs e)
        {
            this.MoveFirst();
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            this.MoveNext();
        }

        private void bLast_Click(object sender, EventArgs e)
        {
            this.MoveLast();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            this.DoQuery();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        public void ClearFilters()
        {
            foreach (var row in this.QueryGrid.Rows)
            {
                row[1].Value = "";
            }
        }
        private void bSave_Click(object sender, EventArgs e)
        {
           
        }

        private void bSaveQBE_Click(object sender, EventArgs e)
        {
            string jsonData = QueryGridToJson();

            if (QueryGridQuerySaveCallBackAction != null)
            {
                QueryGridQuerySaveCallBackAction?.Invoke(jsonData);
            }
            else
            {
                var saveEventArgs = new QueryGridSaveEventArgs { QueryGridJson = jsonData };
                OnQueryGridSaving(saveEventArgs);

                if (!saveEventArgs.Cancel)
                {

                }
            }

        }

        private void bLoadQBE_Click(object sender, EventArgs e)
        {
            if (QueryGridQueryLoadCallBackAction != null)
            {
                QueryGridQueryLoadCallBackAction?.Invoke();
            }
            else
            {
                var loadEventArgs = new QueryGridLoadEventArgs();
                OnQueryGridLoading(loadEventArgs);
                if (!loadEventArgs.Cancel)
                {
                    // Esegui azioni predefinite se necessario
                    //System.Diagnostics.Debug.WriteLine("Caricamento QueryGrid completato.");
                }
            }
        }



        private void CloseQBEForm()
        {
            if (this.Owner == null && this.SetFocusControlAfterClose != null)
                this.Owner = Passero.Framework.Utilities.GetParentOfType<Form>(this.SetFocusControlAfterClose);

            if (this.Owner != null && this.CallBackAction != null)
            {
                try
                {
                    this.CallBackAction.Invoke();
                }
                catch (Exception)
                {

                }
            }



            if (this.SetFocusControlAfterClose != null && this.SetFocusControlAfterClose.Focusable)
            {
                this.SetFocusControlAfterClose.Focus();
            }
            this.Close();
            this.Dispose();
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

        private void XQBEReport_Shown(object sender, EventArgs e)
        {

            this.Show();
            this.Focus();

        }

        private void XQBEReport_FormClosed(object sender, FormClosedEventArgs e)
        {
           
            this.QBEReports=null;
            this.DefaultReport = null;
            this.Dispose();
            GC.Collect();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.ExportResultGrid();
        }

        private void ReportGrid_Click(object sender, EventArgs e)
        {
            if (this.ReportGrid.CurrentRow != null)
            {

                if (this.CurrentReportName != this.ReportGrid.CurrentRow[0].Value.ToString().Trim().ToUpper())
                {
                    this.PdfViewer.PdfStream = null;
                    this.PdfViewer.Visible = false;
                   
                }
                this.CurrentReportName = this.ReportGrid.CurrentRow[0].Value.ToString().Trim().ToUpper();
                this.txtReportTitle.Text = this.ReportGrid.CurrentRow[0].Value.ToString().Trim();
                this.txtReportDescription.Text = this.ReportGrid.CurrentRow[1].Value.ToString().Trim();


            }

            this.SetupQueryGrid(this.CurrentReportName);
        }

        private   void bPrint_Click(object sender, EventArgs e)
        {
            this.DoQuery();
          

            
        }

        private void btnSortAdd_Click(object sender, EventArgs e)
        {
            SortListAdd();

        }


        private void SortListRemove()
        {
            if (this.dgv_SelectedSortColumns.CurrentRow is null)
            { return; }
            QBESSRSReport Report = QBEReports[CurrentReportName];
            Report.SelectedSortColumns.Remove(this.dgv_SelectedSortColumns.CurrentRow.Cells[this.dgvc_SelectedSortColumns_name].Value.ToString());
            this.dgv_SelectedSortColumns.Rows.Clear();
            this.dgv_SelectedSortColumns.Rows.Clear();
            foreach (QBEReportSortColumn column in Report.SelectedSortColumns.Values)
            {
                this.dgv_SelectedSortColumns.Rows.Add(column.Position, column.Name, column.FriendlyName, column.AscDesc);
            }
        }

        private void SortListAdd()
        {
            if (this.lstSortColumns.SelectedItem == null)
            {
                return;
            }

            QBESSRSReport Report = QBEReports[CurrentReportName];
            QBEReportSortColumn SortColumn = (QBEReportSortColumn)this.lstSortColumns.SelectedItem;
            if (Report.SelectedSortColumns.ContainsKey(SortColumn.Name) == false)
            {
                Report.SelectedSortColumns.Add(SortColumn.Name, SortColumn);
            }
            this.lstSortColumns.SelectedItem = null;
            this.dgv_SelectedSortColumns.Rows.Clear();
            foreach (QBEReportSortColumn column in Report.SelectedSortColumns.Values)
            {
                this.dgv_SelectedSortColumns.Rows.Add(column.Position, column.Name, column.FriendlyName, column.AscDesc);
            }
        }

        private void SortListUp()
        {
            Passero.Framework.ControlsUtilities.DataGridRowMoveUp(this.dgv_SelectedSortColumns, this.dgvc_SelectedSortColumns_position);
            this.dgv_SelectedSortColumns.EndEdit();
        }
        private void SortListDown()
        {
            Passero.Framework.ControlsUtilities.DataGridRowMoveDown(this.dgv_SelectedSortColumns, this.dgvc_SelectedSortColumns_position);
            this.dgv_SelectedSortColumns.EndEdit();
        }
        private void btnSortRemove_Click(object sender, EventArgs e)
        {

            SortListRemove();
        }

        private void btnSortUp_Click(object sender, EventArgs e)
        {
            SortListUp();
        }

        private void btnSortDown_Click(object sender, EventArgs e)
        {
            SortListDown();
        }

        private void SetReportViewer()
        {
            this.PdfViewer.Top = this.PanelReportInfo.Height;
            this.PdfViewer.Left = 0;
            this.PdfViewer.Width = this.PanelReportViewer.Width;
            this.PdfViewer.Height = this.PanelReportViewer.Height - this.PanelReportInfo.Top - 35;
            this.txtRenderError.Top = this.PdfViewer.Top;
            this.txtRenderError.Left = 0;
            this.txtRenderError.Width = this.PanelReportViewer.Width;
            this.txtRenderError.Height = this.PdfViewer.Height;
            this.PanelReportInfo.Width = this.PanelReportViewer.Width;
        }
        private void PanelReportViewer_Resize(object sender, EventArgs e)
        {
            SetReportViewer();

        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstSortColumns_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool .Name=="add")
            {
                SortListAdd();
            }
            if (e.Tool.Name == "remove")
            {
                SortListRemove();
            }
        }

        private void dgv_SelectedSortColumns_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "moveup")
            {
               SortListUp();
            }
            if (e.Tool.Name == "movedown")
            {
                SortListDown ();    
            }
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            this.lstSortColumns.Height = this.flowLayoutPanel1.ClientSize.Height - 5;
            this.dgv_SelectedSortColumns.Height = this.flowLayoutPanel1.ClientSize.Height - 5;
        }

        private void PanelReportInfo_Resize(object sender, EventArgs e)
        {
            this.txtReportTitle.Width = this.PanelReportInfo.Width * 30 / 100;
            this.txtReportDescription.Width = this.PanelReportInfo.Width * 60 / 100;
        }

     


        /// <summary>
        /// Proprietà per le impostazioni di filtro.
        /// </summary>
        public System.Globalization.CultureInfo FilterCulture { get; set; } = System.Globalization.CultureInfo.CurrentCulture;
        public bool EnableRelativeDateTokens { get; set; } = true;
        public bool FilterCaseInsensitiveText { get; set; } = true;
        public bool AllowTextRelationalOperators { get; set; } = false;
        public int TopRows { get; set; } = 500;

        /// <summary>
        /// Colonne con codice (per filtri speciali).
        /// </summary>
        public System.Collections.Generic.ISet<string> CodeColumns { get; } = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// Legge le righe della QueryGrid e serializza i valori delle colonne 0 e 1 in un oggetto JSON.
        /// </summary>
        /// <returns>Stringa JSON contenente i dati della QueryGrid.</returns>
        public string QueryGridToJson()
        {
            var rowDataList = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow row in QueryGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var rowData = new Dictionary<string, object>();

                // Leggi il valore e il tag della colonna 0
                object column0Value = row.Cells[0].Value;
                object column0Tag = row.Cells[0].Tag;

                // Leggi il valore e il tag della colonna 1
                object column1Value = row.Cells[1].Value;
                object column1Tag = row.Cells[1].Tag;

                // Costruisci il dizionario per la riga
                rowData["column0"] = new
                {
                    value = column0Value,
                    tag = column0Tag
                };

                rowData["column1"] = new
                {
                    value = column1Value,
                    tag = column1Tag
                };

                rowData["rowTag"] = row.Tag;

                rowDataList.Add(rowData);
            }

            // Serializza in JSON usando Newtonsoft.Json (Nuget package già presente nel progetto)
            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(rowDataList, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

        /// <summary>
        /// Riconfigura la QueryGrid a partire da una stringa JSON.
        /// </summary>
        /// <param name="jsonData">Stringa JSON contenente i dati della QueryGrid.</param>
        public void JsonToQueryGrid(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                return;

            try
            {
                var rowDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);

                if (rowDataList == null || rowDataList.Count == 0)
                    return;

                // Itera su tutti i dati JSON
                foreach (var rowData in rowDataList)
                {
                    // Estrai il rowTag per identificare la riga corretta
                    string rowTag = rowData.ContainsKey("rowTag") ? rowData["rowTag"]?.ToString() : null;

                    if (string.IsNullOrEmpty(rowTag))
                        continue;

                    // Trova la riga nella QueryGrid corrispondente al rowTag
                    DataGridViewRow targetRow = null;
                    foreach (DataGridViewRow row in QueryGrid.Rows)
                    {
                        if (row.Tag?.ToString() == rowTag)
                        {
                            targetRow = row;
                            break;
                        }
                    }

                    if (targetRow == null)
                        continue;

                    // Ripristina il valore della colonna 1 (il valore del filtro)
                    if (rowData.ContainsKey("column1"))
                    {
                        var column1Data = rowData["column1"];

                        if (column1Data is Newtonsoft.Json.Linq.JObject jObject)
                        {
                            object column1Value = jObject["value"];

                            // Gestisci i diversi tipi di celle (TextBox vs CheckBox)
                            if (targetRow.Cells[1] is DataGridViewCheckBoxCell checkBoxCell)
                            {
                                // Per le checkbox, converti il valore appropriatamente
                                if (column1Value != null)
                                {
                                    if (column1Value is bool boolValue)
                                    {
                                        checkBoxCell.Value = boolValue ? 1 : 0;
                                    }
                                    else if (column1Value is int intValue)
                                    {
                                        checkBoxCell.Value = intValue;
                                    }
                                    else if (int.TryParse(column1Value.ToString(), out int parsedValue))
                                    {
                                        checkBoxCell.Value = parsedValue;
                                    }
                                    else
                                    {
                                        checkBoxCell.Value = checkBoxCell.IndeterminateValue;
                                    }
                                }
                                else
                                {
                                    checkBoxCell.Value = checkBoxCell.IndeterminateValue;
                                }
                            }
                            else
                            {
                                // Per le TextBox, assegna il valore direttamente
                                targetRow.Cells[1].Value = column1Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error on JSON deserialization of QueryGrid: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Evento generato quando la QueryGrid viene salvata in JSON.
        /// </summary>
        public event EventHandler<QueryGridSaveEventArgs> QueryGridSaving;

        /// <summary>
        /// Evento generato quando la QueryGrid viene caricata da JSON.
        /// </summary>
        public event EventHandler<QueryGridLoadEventArgs> QueryGridLoading;

        /// <summary>
        /// Solleva l'evento QueryGridSaving.
        /// </summary>
        protected virtual void OnQueryGridSaving(QueryGridSaveEventArgs e)
        {
            QueryGridSaving?.Invoke(this, e);
        }

        /// <summary>
        /// Solleva l'evento QueryGridLoading.
        /// </summary>
        protected virtual void OnQueryGridLoading(QueryGridLoadEventArgs e)
        {
            QueryGridLoading?.Invoke(this, e);
        }

    }
}
  
