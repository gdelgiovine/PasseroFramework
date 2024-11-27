namespace Passero.Framework.FRReports
{
    partial class ReportManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportManager));
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            this.SplitContainer = new Wisej.Web.SplitContainer();
            this.PanelReportViewer = new Wisej.Web.Panel();
            this.htmlPanel = new Wisej.Web.HtmlPanel();
            this.PanelReportInfo = new Wisej.Web.FlowLayoutPanel();
            this.txtReportTitle = new Wisej.Web.TextBox();
            this.txtReportDescription = new Wisej.Web.TextBox();
            this.txtRenderError = new Wisej.Web.TextBox();
            this.AspNetPanel = new Wisej.Web.AspNetPanel();
            this.WebBrowser = new Wisej.Web.WebBrowser();
            this.PdfViewer = new Wisej.Web.PdfViewer();
            this.TabControl = new Wisej.Web.TabControl();
            this.TabPageReports = new Wisej.Web.TabPage();
            this.ReportGrid = new Wisej.Web.DataGridView();
            this.dgvcReportName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvcReportDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvcReportFileName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.TabPageReportQuery = new Wisej.Web.TabPage();
            this.QueryGrid = new Wisej.Web.DataGridView();
            this.dgvcNomeCampo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvcValoreCampo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvcQueryColumn = new Wisej.Web.DataGridViewButtonColumn();
            this.chkLikeOperator = new Wisej.Web.CheckBox();
            this.TabPageReportSort = new Wisej.Web.TabPage();
            this.btnSortDown = new Wisej.Web.Button();
            this.btnSortUp = new Wisej.Web.Button();
            this.dgv_SelectedSortColumns = new Wisej.Web.DataGridView();
            this.dgvc_SelectedSortColumns_position = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_SelectedSortColumns_name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_SelectedSortColumns_friendlyname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_SelectedSortColumns_ascdesc = new Wisej.Web.DataGridViewComboBoxColumn();
            this.btnSortRemove = new Wisej.Web.Button();
            this.btnSortAdd = new Wisej.Web.Button();
            this.lstSortColumns = new Wisej.Web.ListBox();
            this.TabPageExport = new Wisej.Web.TabPage();
            this.PanelExport = new Wisej.Web.Panel();
            this.rbJSON = new Wisej.Web.RadioButton();
            this.btnExport = new Wisej.Web.Button();
            this.rbXML = new Wisej.Web.RadioButton();
            this.rbCSV = new Wisej.Web.RadioButton();
            this.rbExcel = new Wisej.Web.RadioButton();
            this.TabPageDebug = new Wisej.Web.TabPage();
            this.Button2 = new Wisej.Web.Button();
            this.txtDebug = new Wisej.Web.TextBox();
            this.Button1 = new Wisej.Web.Button();
            this.cmbRecords = new Wisej.Web.ComboBox();
            this.NavBar = new Wisej.Web.ToolBar();
            this.bFirst = new Wisej.Web.ToolBarButton();
            this.bPrev = new Wisej.Web.ToolBarButton();
            this.RecordLabel = new Wisej.Web.ToolBarButton();
            this.bNext = new Wisej.Web.ToolBarButton();
            this.bLast = new Wisej.Web.ToolBarButton();
            this.bRefresh = new Wisej.Web.ToolBarButton();
            this.bDelete = new Wisej.Web.ToolBarButton();
            this.Records = new Wisej.Web.ToolBarButton();
            this.bPrint = new Wisej.Web.ToolBarButton();
            this.bSave = new Wisej.Web.ToolBarButton();
            this.bSaveQBE = new Wisej.Web.ToolBarButton();
            this.bLoadQBE = new Wisej.Web.ToolBarButton();
            this.bClose = new Wisej.Web.ToolBarButton();
            this.pbEngineLogo = new Wisej.Web.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.PanelReportViewer.SuspendLayout();
            this.PanelReportInfo.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.TabPageReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReportGrid)).BeginInit();
            this.TabPageReportQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QueryGrid)).BeginInit();
            this.TabPageReportSort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SelectedSortColumns)).BeginInit();
            this.TabPageExport.SuspendLayout();
            this.PanelExport.SuspendLayout();
            this.TabPageDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEngineLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // SplitContainer
            // 
            resources.ApplyResources(this.SplitContainer, "SplitContainer");
            this.SplitContainer.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.PanelReportViewer);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.TabControl);
            // 
            // PanelReportViewer
            // 
            this.PanelReportViewer.Controls.Add(this.htmlPanel);
            this.PanelReportViewer.Controls.Add(this.PanelReportInfo);
            this.PanelReportViewer.Controls.Add(this.txtRenderError);
            this.PanelReportViewer.Controls.Add(this.AspNetPanel);
            this.PanelReportViewer.Controls.Add(this.WebBrowser);
            this.PanelReportViewer.Controls.Add(this.PdfViewer);
            resources.ApplyResources(this.PanelReportViewer, "PanelReportViewer");
            this.PanelReportViewer.Name = "PanelReportViewer";
            this.PanelReportViewer.Resize += new System.EventHandler(this.PanelReportViewer_Resize);
            // 
            // htmlPanel
            // 
            this.htmlPanel.Focusable = false;
            resources.ApplyResources(this.htmlPanel, "htmlPanel");
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.TabStop = false;
            // 
            // PanelReportInfo
            // 
            resources.ApplyResources(this.PanelReportInfo, "PanelReportInfo");
            this.PanelReportInfo.BackColor = System.Drawing.SystemColors.MenuBar;
            this.PanelReportInfo.Controls.Add(this.txtReportTitle);
            this.PanelReportInfo.Controls.Add(this.txtReportDescription);
            this.PanelReportInfo.Name = "PanelReportInfo";
            this.PanelReportInfo.Resize += new System.EventHandler(this.PanelReportInfo_Resize);
            // 
            // txtReportTitle
            // 
            this.txtReportTitle.Focusable = false;
            this.txtReportTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportTitle.Label.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportTitle.Label.Position = Wisej.Web.LabelPosition.Left;
            resources.ApplyResources(this.txtReportTitle, "txtReportTitle");
            this.txtReportTitle.Name = "txtReportTitle";
            this.txtReportTitle.ReadOnly = true;
            this.txtReportTitle.TabStop = false;
            // 
            // txtReportDescription
            // 
            this.txtReportDescription.Focusable = false;
            this.txtReportDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportDescription.Label.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportDescription.Label.Position = Wisej.Web.LabelPosition.Left;
            resources.ApplyResources(this.txtReportDescription, "txtReportDescription");
            this.txtReportDescription.Name = "txtReportDescription";
            this.txtReportDescription.ReadOnly = true;
            this.txtReportDescription.TabStop = false;
            // 
            // txtRenderError
            // 
            resources.ApplyResources(this.txtRenderError, "txtRenderError");
            this.txtRenderError.Name = "txtRenderError";
            // 
            // AspNetPanel
            // 
            resources.ApplyResources(this.AspNetPanel, "AspNetPanel");
            this.AspNetPanel.Name = "AspNetPanel";
            // 
            // WebBrowser
            // 
            resources.ApplyResources(this.WebBrowser, "WebBrowser");
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Url = new System.Uri("~/BasicDalWisejCRViewer.aspx", System.UriKind.Relative);
            // 
            // PdfViewer
            // 
            resources.ApplyResources(this.PdfViewer, "PdfViewer");
            this.PdfViewer.Name = "PdfViewer";
            // 
            // TabControl
            // 
            resources.ApplyResources(this.TabControl, "TabControl");
            this.TabControl.Controls.Add(this.TabPageReports);
            this.TabControl.Controls.Add(this.TabPageReportQuery);
            this.TabControl.Controls.Add(this.TabPageReportSort);
            this.TabControl.Controls.Add(this.TabPageExport);
            this.TabControl.Controls.Add(this.TabPageDebug);
            this.TabControl.Name = "TabControl";
            this.TabControl.PageInsets = new Wisej.Web.Padding(0, 39, 0, 0);
            // 
            // TabPageReports
            // 
            this.TabPageReports.Controls.Add(this.ReportGrid);
            resources.ApplyResources(this.TabPageReports, "TabPageReports");
            this.TabPageReports.Name = "TabPageReports";
            // 
            // ReportGrid
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.ReportGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ReportGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvcReportName,
            this.dgvcReportDescription,
            this.dgvcReportFileName});
            resources.ApplyResources(this.ReportGrid, "ReportGrid");
            this.ReportGrid.Name = "ReportGrid";
            this.ReportGrid.ReadOnly = true;
            this.ReportGrid.RowTemplate.ReadOnly = true;
            this.ReportGrid.Click += new System.EventHandler(this.ReportGrid_Click);
            // 
            // dgvcReportName
            // 
            this.dgvcReportName.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.dgvcReportName.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvcReportName, "dgvcReportName");
            this.dgvcReportName.Name = "dgvcReportName";
            // 
            // dgvcReportDescription
            // 
            this.dgvcReportDescription.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dgvcReportDescription, "dgvcReportDescription");
            this.dgvcReportDescription.Name = "dgvcReportDescription";
            // 
            // dgvcReportFileName
            // 
            this.dgvcReportFileName.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dgvcReportFileName, "dgvcReportFileName");
            this.dgvcReportFileName.Name = "dgvcReportFileName";
            // 
            // TabPageReportQuery
            // 
            this.TabPageReportQuery.Controls.Add(this.QueryGrid);
            this.TabPageReportQuery.Controls.Add(this.chkLikeOperator);
            resources.ApplyResources(this.TabPageReportQuery, "TabPageReportQuery");
            this.TabPageReportQuery.Name = "TabPageReportQuery";
            // 
            // QueryGrid
            // 
            this.QueryGrid.AllowSortingDataSource = false;
            this.QueryGrid.AllowUserToResizeColumns = false;
            this.QueryGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.QueryGrid, "QueryGrid");
            this.QueryGrid.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells;
            this.QueryGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.QueryGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.QueryGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvcNomeCampo,
            this.dgvcValoreCampo,
            this.dgvcQueryColumn});
            this.QueryGrid.DefaultSortMode = Wisej.Web.DataGridViewColumnsSortMode.NotSortable;
            this.QueryGrid.Name = "QueryGrid";
            // 
            // dgvcNomeCampo
            // 
            this.dgvcNomeCampo.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dgvcNomeCampo, "dgvcNomeCampo");
            this.dgvcNomeCampo.MaxInputLength = 50;
            this.dgvcNomeCampo.Name = "dgvcNomeCampo";
            this.dgvcNomeCampo.ReadOnly = true;
            // 
            // dgvcValoreCampo
            // 
            this.dgvcValoreCampo.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dgvcValoreCampo, "dgvcValoreCampo");
            this.dgvcValoreCampo.MaxInputLength = 1024;
            this.dgvcValoreCampo.Name = "dgvcValoreCampo";
            // 
            // dgvcQueryColumn
            // 
            resources.ApplyResources(this.dgvcQueryColumn, "dgvcQueryColumn");
            this.dgvcQueryColumn.Name = "dgvcQueryColumn";
            this.dgvcQueryColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            // 
            // chkLikeOperator
            // 
            resources.ApplyResources(this.chkLikeOperator, "chkLikeOperator");
            this.chkLikeOperator.CheckState = Wisej.Web.CheckState.Checked;
            this.chkLikeOperator.Name = "chkLikeOperator";
            // 
            // TabPageReportSort
            // 
            this.TabPageReportSort.Controls.Add(this.btnSortDown);
            this.TabPageReportSort.Controls.Add(this.btnSortUp);
            this.TabPageReportSort.Controls.Add(this.dgv_SelectedSortColumns);
            this.TabPageReportSort.Controls.Add(this.btnSortRemove);
            this.TabPageReportSort.Controls.Add(this.btnSortAdd);
            this.TabPageReportSort.Controls.Add(this.lstSortColumns);
            resources.ApplyResources(this.TabPageReportSort, "TabPageReportSort");
            this.TabPageReportSort.Name = "TabPageReportSort";
            // 
            // btnSortDown
            // 
            resources.ApplyResources(this.btnSortDown, "btnSortDown");
            this.btnSortDown.Name = "btnSortDown";
            this.btnSortDown.Click += new System.EventHandler(this.btnSortDown_Click);
            // 
            // btnSortUp
            // 
            resources.ApplyResources(this.btnSortUp, "btnSortUp");
            this.btnSortUp.Name = "btnSortUp";
            this.btnSortUp.Click += new System.EventHandler(this.btnSortUp_Click);
            // 
            // dgv_SelectedSortColumns
            // 
            resources.ApplyResources(this.dgv_SelectedSortColumns, "dgv_SelectedSortColumns");
            this.dgv_SelectedSortColumns.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_SelectedSortColumns.CellBorderStyle = Wisej.Web.DataGridViewCellBorderStyle.None;
            this.dgv_SelectedSortColumns.ColumnHeadersVisible = false;
            this.dgv_SelectedSortColumns.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvc_SelectedSortColumns_position,
            this.dgvc_SelectedSortColumns_name,
            this.dgvc_SelectedSortColumns_friendlyname,
            this.dgvc_SelectedSortColumns_ascdesc});
            this.dgv_SelectedSortColumns.DefaultRowHeight = 24;
            this.dgv_SelectedSortColumns.Name = "dgv_SelectedSortColumns";
            this.dgv_SelectedSortColumns.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dgv_SelectedSortColumns.Selectable = true;
            // 
            // dgvc_SelectedSortColumns_position
            // 
            this.dgvc_SelectedSortColumns_position.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvc_SelectedSortColumns_position.DataPropertyName = "position";
            resources.ApplyResources(this.dgvc_SelectedSortColumns_position, "dgvc_SelectedSortColumns_position");
            this.dgvc_SelectedSortColumns_position.Name = "dgvc_SelectedSortColumns_position";
            // 
            // dgvc_SelectedSortColumns_name
            // 
            this.dgvc_SelectedSortColumns_name.DataPropertyName = "Name";
            resources.ApplyResources(this.dgvc_SelectedSortColumns_name, "dgvc_SelectedSortColumns_name");
            this.dgvc_SelectedSortColumns_name.Name = "dgvc_SelectedSortColumns_name";
            // 
            // dgvc_SelectedSortColumns_friendlyname
            // 
            this.dgvc_SelectedSortColumns_friendlyname.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvc_SelectedSortColumns_friendlyname.DataPropertyName = "FriendlyName";
            resources.ApplyResources(this.dgvc_SelectedSortColumns_friendlyname, "dgvc_SelectedSortColumns_friendlyname");
            this.dgvc_SelectedSortColumns_friendlyname.Name = "dgvc_SelectedSortColumns_friendlyname";
            this.dgvc_SelectedSortColumns_friendlyname.ReadOnly = true;
            // 
            // dgvc_SelectedSortColumns_ascdesc
            // 
            this.dgvc_SelectedSortColumns_ascdesc.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.dgvc_SelectedSortColumns_ascdesc.DataPropertyName = "AscDesc";
            this.dgvc_SelectedSortColumns_ascdesc.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.dgvc_SelectedSortColumns_ascdesc, "dgvc_SelectedSortColumns_ascdesc");
            this.dgvc_SelectedSortColumns_ascdesc.Items.AddRange(new object[] {
            "ASC",
            "DESC"});
            this.dgvc_SelectedSortColumns_ascdesc.Name = "dgvc_SelectedSortColumns_ascdesc";
            this.dgvc_SelectedSortColumns_ascdesc.ValueType = typeof(object);
            // 
            // btnSortRemove
            // 
            resources.ApplyResources(this.btnSortRemove, "btnSortRemove");
            this.btnSortRemove.Name = "btnSortRemove";
            this.btnSortRemove.Click += new System.EventHandler(this.btnSortRemove_Click);
            // 
            // btnSortAdd
            // 
            resources.ApplyResources(this.btnSortAdd, "btnSortAdd");
            this.btnSortAdd.Name = "btnSortAdd";
            this.btnSortAdd.Click += new System.EventHandler(this.btnSortAdd_Click);
            // 
            // lstSortColumns
            // 
            resources.ApplyResources(this.lstSortColumns, "lstSortColumns");
            this.lstSortColumns.Label.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lstSortColumns.Name = "lstSortColumns";
            // 
            // TabPageExport
            // 
            this.TabPageExport.Controls.Add(this.PanelExport);
            resources.ApplyResources(this.TabPageExport, "TabPageExport");
            this.TabPageExport.Name = "TabPageExport";
            // 
            // PanelExport
            // 
            this.PanelExport.Controls.Add(this.rbJSON);
            this.PanelExport.Controls.Add(this.btnExport);
            this.PanelExport.Controls.Add(this.rbXML);
            this.PanelExport.Controls.Add(this.rbCSV);
            this.PanelExport.Controls.Add(this.rbExcel);
            resources.ApplyResources(this.PanelExport, "PanelExport");
            this.PanelExport.Name = "PanelExport";
            this.PanelExport.TabStop = true;
            // 
            // rbJSON
            // 
            resources.ApplyResources(this.rbJSON, "rbJSON");
            this.rbJSON.Name = "rbJSON";
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // rbXML
            // 
            resources.ApplyResources(this.rbXML, "rbXML");
            this.rbXML.Name = "rbXML";
            // 
            // rbCSV
            // 
            resources.ApplyResources(this.rbCSV, "rbCSV");
            this.rbCSV.Name = "rbCSV";
            // 
            // rbExcel
            // 
            this.rbExcel.Checked = true;
            resources.ApplyResources(this.rbExcel, "rbExcel");
            this.rbExcel.Name = "rbExcel";
            this.rbExcel.TabStop = true;
            // 
            // TabPageDebug
            // 
            this.TabPageDebug.Controls.Add(this.Button2);
            this.TabPageDebug.Controls.Add(this.txtDebug);
            this.TabPageDebug.Controls.Add(this.Button1);
            this.TabPageDebug.Controls.Add(this.cmbRecords);
            resources.ApplyResources(this.TabPageDebug, "TabPageDebug");
            this.TabPageDebug.Name = "TabPageDebug";
            // 
            // Button2
            // 
            resources.ApplyResources(this.Button2, "Button2");
            this.Button2.Name = "Button2";
            // 
            // txtDebug
            // 
            resources.ApplyResources(this.txtDebug, "txtDebug");
            this.txtDebug.Name = "txtDebug";
            // 
            // Button1
            // 
            resources.ApplyResources(this.Button1, "Button1");
            this.Button1.Name = "Button1";
            // 
            // cmbRecords
            // 
            this.cmbRecords.Items.AddRange(new object[] {
            resources.GetString("cmbRecords.Items"),
            resources.GetString("cmbRecords.Items1"),
            resources.GetString("cmbRecords.Items2"),
            resources.GetString("cmbRecords.Items3"),
            resources.GetString("cmbRecords.Items4"),
            resources.GetString("cmbRecords.Items5")});
            resources.ApplyResources(this.cmbRecords, "cmbRecords");
            this.cmbRecords.Name = "cmbRecords";
            // 
            // NavBar
            // 
            this.NavBar.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.NavBar.Buttons.AddRange(new Wisej.Web.ToolBarButton[] {
            this.bFirst,
            this.bPrev,
            this.RecordLabel,
            this.bNext,
            this.bLast,
            this.bRefresh,
            this.bDelete,
            this.Records,
            this.bPrint,
            this.bSave,
            this.bSaveQBE,
            this.bLoadQBE,
            this.bClose});
            resources.ApplyResources(this.NavBar, "NavBar");
            this.NavBar.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.NavBar.Name = "NavBar";
            this.NavBar.TabStop = false;
            // 
            // bFirst
            // 
            this.bFirst.AllowHtml = true;
            resources.ApplyResources(this.bFirst, "bFirst");
            this.bFirst.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bFirst.Name = "bFirst";
            this.bFirst.Click += new System.EventHandler(this.bFirst_Click);
            // 
            // bPrev
            // 
            this.bPrev.AllowHtml = true;
            resources.ApplyResources(this.bPrev, "bPrev");
            this.bPrev.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrev.Name = "bPrev";
            this.bPrev.Click += new System.EventHandler(this.bPrev_Click);
            // 
            // RecordLabel
            // 
            this.RecordLabel.AllowHtml = true;
            resources.ApplyResources(this.RecordLabel, "RecordLabel");
            this.RecordLabel.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.RecordLabel.Name = "RecordLabel";
            // 
            // bNext
            // 
            this.bNext.AllowHtml = true;
            resources.ApplyResources(this.bNext, "bNext");
            this.bNext.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bNext.Name = "bNext";
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // bLast
            // 
            this.bLast.AllowHtml = true;
            resources.ApplyResources(this.bLast, "bLast");
            this.bLast.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bLast.Name = "bLast";
            this.bLast.Click += new System.EventHandler(this.bLast_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.AllowHtml = true;
            resources.ApplyResources(this.bRefresh, "bRefresh");
            this.bRefresh.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bDelete
            // 
            this.bDelete.AllowHtml = true;
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bDelete.Name = "bDelete";
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // Records
            // 
            this.Records.AllowHtml = true;
            this.Records.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.Records.Name = "Records";
            this.Records.Style = Wisej.Web.ToolBarButtonStyle.DropDownButton;
            resources.ApplyResources(this.Records, "Records");
            // 
            // bPrint
            // 
            this.bPrint.AllowHtml = true;
            resources.ApplyResources(this.bPrint, "bPrint");
            this.bPrint.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrint.Name = "bPrint";
            this.bPrint.Click += new System.EventHandler(this.bPrint_Click);
            // 
            // bSave
            // 
            this.bSave.AllowHtml = true;
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bSave.Name = "bSave";
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bSaveQBE
            // 
            this.bSaveQBE.AllowHtml = true;
            resources.ApplyResources(this.bSaveQBE, "bSaveQBE");
            this.bSaveQBE.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bSaveQBE.Name = "bSaveQBE";
            // 
            // bLoadQBE
            // 
            this.bLoadQBE.AllowHtml = true;
            resources.ApplyResources(this.bLoadQBE, "bLoadQBE");
            this.bLoadQBE.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bLoadQBE.Name = "bLoadQBE";
            // 
            // bClose
            // 
            this.bClose.AllowHtml = true;
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bClose.Name = "bClose";
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // pbEngineLogo
            // 
            resources.ApplyResources(this.pbEngineLogo, "pbEngineLogo");
            this.pbEngineLogo.Name = "pbEngineLogo";
            // 
            // ReportManager
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromName("@toolbar");
            this.Controls.Add(this.pbEngineLogo);
            this.Controls.Add(this.NavBar);
            this.Controls.Add(this.SplitContainer);
            this.Name = "ReportManager";
            this.ShowModalMask = true;
            this.Load += new System.EventHandler(this.XQBEForm_Load);
            this.Shown += new System.EventHandler(this.XQBEReport_Shown);
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.XQBEReport_FormClosed);
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.PanelReportViewer.ResumeLayout(false);
            this.PanelReportViewer.PerformLayout();
            this.PanelReportInfo.ResumeLayout(false);
            this.PanelReportInfo.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.TabPageReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReportGrid)).EndInit();
            this.TabPageReportQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.QueryGrid)).EndInit();
            this.TabPageReportSort.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SelectedSortColumns)).EndInit();
            this.TabPageExport.ResumeLayout(false);
            this.PanelExport.ResumeLayout(false);
            this.PanelExport.PerformLayout();
            this.TabPageDebug.ResumeLayout(false);
            this.TabPageDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEngineLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal Wisej.Web.SplitContainer SplitContainer;
        internal Wisej.Web.Panel PanelReportViewer;
        internal Wisej.Web.AspNetPanel AspNetPanel;
        internal Wisej.Web.WebBrowser WebBrowser;
        internal Wisej.Web.PdfViewer PdfViewer;
        internal Wisej.Web.TabControl TabControl;
        internal Wisej.Web.TabPage TabPageReports;
        private Wisej.Web.DataGridView ReportGrid;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcReportName;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcReportDescription;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcReportFileName;
        internal Wisej.Web.TabPage TabPageReportQuery;
        private Wisej.Web.DataGridView QueryGrid;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcNomeCampo;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcValoreCampo;
        private Wisej.Web.DataGridViewButtonColumn dgvcQueryColumn;
        internal Wisej.Web.CheckBox chkLikeOperator;
        internal Wisej.Web.TabPage TabPageReportSort;
        internal Wisej.Web.Button btnSortDown;
        internal Wisej.Web.Button btnSortUp;
        internal Wisej.Web.DataGridView dgv_SelectedSortColumns;
        internal Wisej.Web.DataGridViewTextBoxColumn dgvc_SelectedSortColumns_position;
        internal Wisej.Web.DataGridViewTextBoxColumn dgvc_SelectedSortColumns_name;
        internal Wisej.Web.DataGridViewTextBoxColumn dgvc_SelectedSortColumns_friendlyname;
        internal Wisej.Web.DataGridViewComboBoxColumn dgvc_SelectedSortColumns_ascdesc;
        internal Wisej.Web.Button btnSortRemove;
        internal Wisej.Web.Button btnSortAdd;
        internal Wisej.Web.ListBox lstSortColumns;
        internal Wisej.Web.TabPage TabPageExport;
        internal Wisej.Web.Panel PanelExport;
        internal Wisej.Web.Button btnExport;
        internal Wisej.Web.RadioButton rbXML;
        internal Wisej.Web.RadioButton rbCSV;
        internal Wisej.Web.RadioButton rbExcel;
        internal Wisej.Web.TabPage TabPageDebug;
        internal Wisej.Web.Button Button2;
        internal Wisej.Web.TextBox txtDebug;
        internal Wisej.Web.Button Button1;
        internal Wisej.Web.ComboBox cmbRecords;
        internal Wisej.Web.ToolBar NavBar;
        internal Wisej.Web.ToolBarButton bFirst;
        internal Wisej.Web.ToolBarButton bPrev;
        internal Wisej.Web.ToolBarButton RecordLabel;
        internal Wisej.Web.ToolBarButton bNext;
        internal Wisej.Web.ToolBarButton bLast;
        internal Wisej.Web.ToolBarButton bRefresh;
        internal Wisej.Web.ToolBarButton bDelete;
        internal Wisej.Web.ToolBarButton Records;
        internal Wisej.Web.ToolBarButton bPrint;
        internal Wisej.Web.ToolBarButton bSave;
        internal Wisej.Web.ToolBarButton bSaveQBE;
        internal Wisej.Web.ToolBarButton bLoadQBE;
        internal Wisej.Web.ToolBarButton bClose;
        internal Wisej.Web.RadioButton rbJSON;
        private Wisej.Web.TextBox txtRenderError;
        private Wisej.Web.PictureBox pbEngineLogo;
        private Wisej.Web.FlowLayoutPanel PanelReportInfo;
        internal Wisej.Web.TextBox txtReportTitle;
        internal Wisej.Web.TextBox txtReportDescription;
        private Wisej.Web.HtmlPanel htmlPanel;
    }
}