
using Dapper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Passero.Framework.FRReports;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Wisej.Web;

namespace Passero.Framework.FRReports
{

    public partial class ReportManager : Form
    {
        private string RenderError = "";
        private string CurrentReportName;
        public bool RaiseReportEvents= false;
        public FRReports.QBEColumns QBEColumns = new FRReports.QBEColumns();

        public FRReports.QBEReports QBEReports = new FRReports.QBEReports();    
        public FRReports.QBEFRReport DefaultReport { get; set; }
        public bool PrintDefaultReport { get; set; } = false;
        public string ReportsPath { get; set; } = Wisej.Web.Application.StartupPath + "reports";
        public string ReportsPDFUrlPath { get; set; } = @"\Reports\";
        public string ReportFileName { get; set; } = "";
        public string ReportName { get; set; } = "";
        public string SQLQuery { get; set; } = "";
        public DynamicParameters SQLQueryParameters { get; set; } = new DynamicParameters();

        private FRReport FRReport = new FRReport();

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

        private void FRReport_ReportRenderRequest(object sender, EventArgs e)
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
            string expofilenameextension = ".xml";
            System.IO.MemoryStream  Stream = null;

            QBEFRReport  Report = this.QBEReports[CurrentReportName];
            FRRenderFormat format = FRRenderFormat.EXCEL ;
            
            
            if (rbExcel.Checked)
            {
                format = FRRenderFormat.EXCEL;
                expofilenameextension = ".xls";
            }

            if (rbCSV.Checked)
            {
                format = FRRenderFormat.CSV ;
                expofilenameextension = ".csv";
            }

            if (rbJSON.Checked)
            {
                
                expofilenameextension = ".json";
            }
            if (rbXML.Checked)
            {
                format = FRRenderFormat.XML;
                expofilenameextension = ".xml";
            }

            BuildQuery3();
            byte[] ReportBytes = null;// this.RenderReport(Report,format );


            //switch (SSRSReport.ReportFormat)
            //{
            //    case SSRSRenderFormat.XML:
            //        break;
            //    case SSRSRenderFormat.NULL:
            //        break;
            //    case SSRSRenderFormat.CSV:
            //        break;
            //    case SSRSRenderFormat.IMAGE:
            //        break;
            //    case SSRSRenderFormat.PDF:

            //        //if (this.PdfViewer.Dock != DockStyle.Fill) this.PdfViewer.Dock = DockStyle.Fill;
            //        //this.PdfViewer.PdfStream = new MemoryStream(ReportBytes);
            //        //this.PdfViewer.Visible = true;
            //        break;
            //    case SSRSRenderFormat.HTML40:
            //        break;
            //    case SSRSRenderFormat.HTML32:
            //        break;
            //    case SSRSRenderFormat.MHTML:
            //        break;
            //    case SSRSRenderFormat.EXCEL:
            //        break;
            //    case SSRSRenderFormat.WORD:
            //        break;
            //    default:
            //        break;
            //}



            if (ReportBytes!=null && expofilenameextension != "" )
            {
                
                Stream = new MemoryStream(ReportBytes);
               Wisej.Web.Application.Download(Stream, exportfilename + expofilenameextension);
                Stream.Close();
            }
            //if (System.IO.File.Exists(filename))
            //{
            //    System.IO.File.Delete(filename);
            //}

        }


        //private ModelClass GetEmptyModel()
        //{
        //    return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        //}

        public void ShowQBEReport()
        {
            this.SetupQBEReport();
            if (this.ReportGrid.CurrentRow != null)
                this.SetupQueryGrid(this.ReportGrid.CurrentRow[0].Value.ToString().Trim().ToUpper());

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
            SetupQueryGrid(this.DefaultReport );


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
            QBEFRReport  report = this.QBEReports[ReportName];
            if (report==null)
            {
                this.RenderError = "No Report!";
                MessageBox.Show("No Report!", this.Text,MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (report.DataSets == null )
            {
                this.RenderError = "No Report DataSets!";
                MessageBox.Show("No Report DataSets!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.txtRenderError.Visible = false;
          
            byte[] ReportBytes = this.RenderReport(report, FRRenderFormat.PDF);
          
            if (ReportBytes != null)
            {
                //if (this.PdfViewer.Dock != DockStyle.Fill) this.PdfViewer.Dock = DockStyle.Fill;
                this.PdfViewer.PdfStream = new MemoryStream(ReportBytes);
                this.PdfViewer.Visible = true;
                Passero.Framework.Utilities.SaveByteArrayToFile(ReportBytes, @"c:\REports\report1.pdf");
                
            }
            else
            {
                this.txtRenderError.Text = this.RenderError;
                this.txtRenderError.Dock = DockStyle.Fill;
                this.txtRenderError.Visible = true;
            }
          

        }


        private byte[] RenderReport(string ReportName, FRRenderFormat Format = FRRenderFormat.PDF)
        {

            ReportName = ReportName.Trim().ToUpper();
            QBEFRReport  Report = this.QBEReports[ReportName];
            if (Report == null)
            {
                this.RenderError = "No Report!";
                MessageBox.Show("No Report!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (Report.DataSets == null)
            {
                this.RenderError = "No Report DataSets!";
                MessageBox.Show("No Report DataSets!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
            return RenderReport(Report , Format);
            

        }


        private byte[] RenderReport(QBEFRReport Report, FRRenderFormat  Format = FRRenderFormat.PDF)
        {

            if (Report == null)
            {
                MessageBox.Show("No Report!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (Report.DataSets == null)
            {
                MessageBox.Show("No Report DataSets!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            //Passero.Framework.Reports.SSRSReport SSRSReport = new Reports.SSRSReport();
            FRReport = new FRReports.FRReport();
            if (RaiseReportEvents)
            {
                this.FRReport.ReportRenderRequest -= FRReport_ReportRenderRequest;
                this.FRReport.ReportRenderRequest += FRReport_ReportRenderRequest;
            }
            FRReport.ReportPath = Report.ReportFileName;
            FRReport.ReportFormat = Format;
            FRReport.DataSets = Report.DataSets;

         

            byte[] ReportBytes = FRReport.Render(Format );
            if (FRReport.LastExecutionResult.Exception != null)
            {
                this.RenderError = FRReport.LastExecutionResult.ResultMessage + "\n" + FRReport.LastExecutionResult.Exception.ToString();
            }
            return ReportBytes;

        }

      
     

        private void SetupQueryGrid(string ReportName)
        {

            QBEFRReport  report = this.QBEReports[ReportName];
            SetupQueryGrid(report);
        }


        private void SetupQueryGrid(QBEFRReport  QBEReport)
        {

            if (QBEReport.DataSets.Count == 0)
                return;

            Passero.Framework.FRReports.DataSet PrimaryDataSet = QBEReport.PrimaryDataSet;
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
                    Passero.Framework.EnumSystemTypeIs PropertyTypeIs = Passero.Framework.Utilities.GetSystemTypeIs(PropertyType);
                    if (Passero.Framework.Utilities.GetSystemTypeIs(PropertyType) == EnumSystemTypeIs.Boolean)
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


        private void BuildQuery3()
        {
            if (this.QBEReports[this.CurrentReportName].DataSets.Count == 0)
                return;

            QBEFRReport Report = this.QBEReports[this.CurrentReportName];

            Passero.Framework.FRReports.DataSet PrimaryDataSet = Report.PrimaryDataSet;
            if (PrimaryDataSet == null)
            {
                PrimaryDataSet = Report.DataSets.Values.First();
            }
            
            StringBuilder sqlwhere = new StringBuilder();
            string _WhereAND = "";
            this.QueryGrid.EndEdit();
            DynamicParameters parameters = new DynamicParameters();
            foreach (var item in this.QueryGrid.Rows)
            {
                StringBuilder sqlwhereitem = new StringBuilder();
                string Value = "";
                if (item[1].Value != null)
                    Value = item[1].Value.ToString();

                string _WhereItemOR = "";
                string[] Values;
                Type PropertyType = PrimaryDataSet .ModelProperties[item.Tag.ToString()].PropertyType;
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
                        if (this.chkLikeOperator.Checked)
                        {
                            sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()} Like {parametername} ");
                            parameters.Add(parametername, "%"+_Value+"%", Passero.Framework.Utilities.GetDbType(PropertyType));
                        }
                        else
                        {
                            sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()}{GetComparisionOperator(_Value)}{parametername}");
                            parameters.Add(parametername,RemoveComparisionOperator(_Value), Passero.Framework.Utilities.GetDbType(PropertyType));
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



            this.SQLQuery = $"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName(PrimaryDataSet .Model)}";

            if (sqlwhere.ToString ().Trim () != "")
                this.SQLQuery=this.SQLQuery + $" WHERE {sqlwhere.ToString()}";


            this.SQLQuery += " "+Report.OrderBy() ;
            this.SQLQueryParameters = parameters;
            PrimaryDataSet.Parameters = parameters;
            PrimaryDataSet.SQLQuery = SQLQuery;
            PrimaryDataSet.LoadData();


            //Load Data for other datasets
            foreach (Passero.Framework.FRReports.DataSet DataSet in Report.DataSets.Values )
            {
                if (DataSet != PrimaryDataSet )
                {
                    if (DataSet.SQLQuery ==null || DataSet.SQLQuery.Trim()== "")
                    {
                        DataSet.SQLQuery = $"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName(DataSet.Model)}";
                    }
                    DataSet.LoadData ();    
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

            if (this.lstSortColumns .SelectedItem ==null)
            {
                return;
            }

            QBEFRReport  Report = QBEReports[CurrentReportName];
            QBEReportSortColumn SortColumn= (QBEReportSortColumn )this.lstSortColumns.SelectedItem;    
            if(Report .SelectedSortColumns .ContainsKey (SortColumn.Name)==false)
            {
                Report.SelectedSortColumns.Add(SortColumn.Name, SortColumn);
            }
            this.lstSortColumns.SelectedItem = null;
            this.dgv_SelectedSortColumns.Rows.Clear();
            foreach ( QBEReportSortColumn column in Report.SelectedSortColumns.Values  )
            {
                this.dgv_SelectedSortColumns.Rows.Add(column.Position, column.Name, column.FriendlyName, column.AscDesc);
            }

        }

        private void btnSortRemove_Click(object sender, EventArgs e)
        {
    
            if (this.dgv_SelectedSortColumns.CurrentRow  is null)
            { return; }
            QBEFRReport  Report = QBEReports[CurrentReportName];
            Report.SelectedSortColumns.Remove(this.dgv_SelectedSortColumns.CurrentRow.Cells[this.dgvc_SelectedSortColumns_name].Value.ToString());
            this.dgv_SelectedSortColumns.Rows.Clear();
            this.dgv_SelectedSortColumns.Rows.Clear();
            foreach (QBEReportSortColumn column in Report.SelectedSortColumns.Values)
            {
                this.dgv_SelectedSortColumns.Rows.Add(column.Position, column.Name, column.FriendlyName, column.AscDesc);
            }
        }

        private void btnSortUp_Click(object sender, EventArgs e)
        {
            Passero.Framework.ControlsUtilities.DataGridRowMoveUp(this.dgv_SelectedSortColumns, this.dgvc_SelectedSortColumns_position);
            this.dgv_SelectedSortColumns.EndEdit();
        }

        private void btnSortDown_Click(object sender, EventArgs e)
        {
            Passero.Framework.ControlsUtilities.DataGridRowMoveDown(this.dgv_SelectedSortColumns, this.dgvc_SelectedSortColumns_position);
            this.dgv_SelectedSortColumns.EndEdit();
        }

        private void PanelReportViewer_Resize(object sender, EventArgs e)
        {
            this.PdfViewer.Top = this.PanelReportInfo .Height + 1;
            this.PdfViewer.Left = 0;
            this.PdfViewer.Width = this.PanelReportViewer.Width;
            this.PdfViewer.Height =  this.PanelReportViewer.Height - this.PanelReportInfo.Top - 35;
            this.txtRenderError.Top = this.PdfViewer.Top;
            this.txtRenderError.Left = 0;
            this.txtRenderError.Width = this.PanelReportViewer.Width;
            this.txtRenderError .Height =this.PdfViewer .Height;


        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
  
