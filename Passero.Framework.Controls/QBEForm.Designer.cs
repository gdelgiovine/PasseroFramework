using System.ComponentModel;

namespace Passero.Framework.Controls
{ 
   
    partial class QBEForm<ModelClass> where ModelClass : class
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool2 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool3 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool4 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool5 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool6 = new Wisej.Web.ComponentTool();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            this.SplitContainer = new Wisej.Web.SplitContainer();
            this.ResultGrid = new Wisej.Web.DataGridView();
            this.TabControl = new Wisej.Web.TabControl();
            this.TabPageReportQuery = new Wisej.Web.TabPage();
            this.QueryGrid = new Wisej.Web.DataGridView();
            this.dgvcNomeCampo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvcColumnValue = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvcQueryColumn = new Wisej.Web.DataGridViewButtonColumn();
            this.chkLikeOperator = new Wisej.Web.CheckBox();
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
            this.ContextMenuRecords = new Wisej.Web.ContextMenu(this.components);
            this.menuRecords100 = new Wisej.Web.MenuItem();
            this.menuRecords500 = new Wisej.Web.MenuItem();
            this.menuRecords1000 = new Wisej.Web.MenuItem();
            this.menuRecords2000 = new Wisej.Web.MenuItem();
            this.menuRecords5000 = new Wisej.Web.MenuItem();
            this.menuRecords10000 = new Wisej.Web.MenuItem();
            this.menuRecordsALL = new Wisej.Web.MenuItem();
            this.bPrint = new Wisej.Web.ToolBarButton();
            this.bSave = new Wisej.Web.ToolBarButton();
            this.bSaveQBE = new Wisej.Web.ToolBarButton();
            this.bLoadQBE = new Wisej.Web.ToolBarButton();
            this.bClose = new Wisej.Web.ToolBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.TabControl.SuspendLayout();
            this.TabPageReportQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QueryGrid)).BeginInit();
            this.TabPageExport.SuspendLayout();
            this.PanelExport.SuspendLayout();
            this.TabPageDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer
            // 
            this.SplitContainer.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.SplitContainer.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Margin = new Wisej.Web.Padding(0);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Orientation = Wisej.Web.Orientation.Horizontal;
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.ResultGrid);
            this.SplitContainer.Panel1.ShowHeader = true;
            this.SplitContainer.Panel1.Text = "Query Results";
            componentTool1.ImageSource = "icon-copy";
            componentTool1.Name = "selectrows";
            componentTool1.Position = Wisej.Web.LeftRightAlignment.Left;
            componentTool1.ToolTipText = "Select/Unselect all rows";
            componentTool2.ImageSource = "icon-preview";
            componentTool2.Name = "autosize";
            componentTool2.ToolTipText = "Autosize Grid";
            componentTool3.ImageSource = "icon-first";
            componentTool3.Name = "movefirst";
            componentTool3.ToolTipText = "Move to first row";
            componentTool4.ImageSource = "scrollbar-arrow-left";
            componentTool4.Name = "moveprevious";
            componentTool4.ToolTipText = "Move to previous row";
            componentTool5.ImageSource = "scrollbar-arrow-right";
            componentTool5.Name = "movenext";
            componentTool5.ToolTipText = "Move to next row";
            componentTool6.ImageSource = "icon-last";
            componentTool6.Name = "movelast";
            componentTool6.ToolTipText = "Move to last row";
            this.SplitContainer.Panel1.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1,
            componentTool2,
            componentTool3,
            componentTool4,
            componentTool5,
            componentTool6});
            this.SplitContainer.Panel1.ToolClick += new Wisej.Web.ToolClickEventHandler(this.SplitContainer_Panel1_ToolClick);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.TabControl);
            this.SplitContainer.Panel2.ShowHeader = true;
            this.SplitContainer.Panel2.Text = "Query Manager";
            this.SplitContainer.Size = new System.Drawing.Size(898, 588);
            this.SplitContainer.SplitterDistance = 394;
            this.SplitContainer.SplitterWidth = 8;
            this.SplitContainer.TabIndex = 1;
            // 
            // ResultGrid
            // 
            this.ResultGrid.DefaultRowHeight = 24;
            this.ResultGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.ResultGrid.KeepSameRowHeight = true;
            this.ResultGrid.Location = new System.Drawing.Point(0, 0);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.ReadOnly = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.ResultGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ResultGrid.RowHeadersWidth = 24;
            this.ResultGrid.RowTemplate.DefaultCellStyle.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ResultGrid.RowTemplate.Height = 20;
            this.ResultGrid.Size = new System.Drawing.Size(898, 366);
            this.ResultGrid.TabIndex = 0;
            this.ResultGrid.Visible = false;
            this.ResultGrid.RowEnter += new Wisej.Web.DataGridViewCellEventHandler(this.ResultGrid_RowEnter);
            this.ResultGrid.CellDoubleClick += new Wisej.Web.DataGridViewCellEventHandler(this.ResultGrid_CellDoubleClick);
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabPageReportQuery);
            this.TabControl.Controls.Add(this.TabPageExport);
            this.TabControl.Controls.Add(this.TabPageDebug);
            this.TabControl.Dock = Wisej.Web.DockStyle.Fill;
            this.TabControl.ItemSize = new System.Drawing.Size(0, 30);
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.PageInsets = new Wisej.Web.Padding(0, 29, 0, 0);
            this.TabControl.Size = new System.Drawing.Size(898, 158);
            this.TabControl.TabIndex = 0;
            // 
            // TabPageReportQuery
            // 
            this.TabPageReportQuery.Controls.Add(this.QueryGrid);
            this.TabPageReportQuery.Controls.Add(this.chkLikeOperator);
            this.TabPageReportQuery.Hidden = true;
            this.TabPageReportQuery.Location = new System.Drawing.Point(0, 29);
            this.TabPageReportQuery.Name = "TabPageReportQuery";
            this.TabPageReportQuery.Size = new System.Drawing.Size(898, 129);
            this.TabPageReportQuery.Text = "Query Criteria";
            // 
            // QueryGrid
            // 
            this.QueryGrid.AllowSortingDataSource = false;
            this.QueryGrid.AllowUserToResizeColumns = false;
            this.QueryGrid.AllowUserToResizeRows = false;
            this.QueryGrid.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.QueryGrid.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells;
            this.QueryGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.QueryGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.QueryGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvcNomeCampo,
            this.dgvcColumnValue,
            this.dgvcQueryColumn});
            this.QueryGrid.DefaultSortMode = Wisej.Web.DataGridViewColumnsSortMode.NotSortable;
            this.QueryGrid.Location = new System.Drawing.Point(1, 30);
            this.QueryGrid.Name = "QueryGrid";
            this.QueryGrid.RowHeadersVisible = false;
            this.QueryGrid.RowHeadersWidth = 24;
            this.QueryGrid.Size = new System.Drawing.Size(888, 87);
            this.QueryGrid.TabIndex = 3;
            this.QueryGrid.CellDoubleClick += new Wisej.Web.DataGridViewCellEventHandler(this.QueryGrid_CellDoubleClick);
            // 
            // dgvcNomeCampo
            // 
            this.dgvcNomeCampo.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcNomeCampo.HeaderText = "Column";
            this.dgvcNomeCampo.MaxInputLength = 50;
            this.dgvcNomeCampo.Name = "dgvcNomeCampo";
            this.dgvcNomeCampo.ReadOnly = true;
            // 
            // dgvcColumnValue
            // 
            this.dgvcColumnValue.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcColumnValue.HeaderText = "Value/Condition";
            this.dgvcColumnValue.MaxInputLength = 1024;
            this.dgvcColumnValue.Name = "dgvcColumnValue";
            // 
            // dgvcQueryColumn
            // 
            this.dgvcQueryColumn.HeaderText = "?";
            this.dgvcQueryColumn.Name = "dgvcQueryColumn";
            this.dgvcQueryColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.dgvcQueryColumn.Visible = false;
            // 
            // chkLikeOperator
            // 
            this.chkLikeOperator.AutoSize = false;
            this.chkLikeOperator.CheckState = Wisej.Web.CheckState.Checked;
            this.chkLikeOperator.Location = new System.Drawing.Point(3, 3);
            this.chkLikeOperator.Name = "chkLikeOperator";
            this.chkLikeOperator.Size = new System.Drawing.Size(194, 21);
            this.chkLikeOperator.TabIndex = 2;
            this.chkLikeOperator.Text = "Use CONTAINS operator";
            // 
            // TabPageExport
            // 
            this.TabPageExport.Controls.Add(this.PanelExport);
            this.TabPageExport.Hidden = true;
            this.TabPageExport.Location = new System.Drawing.Point(0, 29);
            this.TabPageExport.Name = "TabPageExport";
            this.TabPageExport.Size = new System.Drawing.Size(898, 129);
            this.TabPageExport.Text = "Export";
            // 
            // PanelExport
            // 
            this.PanelExport.Controls.Add(this.rbJSON);
            this.PanelExport.Controls.Add(this.btnExport);
            this.PanelExport.Controls.Add(this.rbXML);
            this.PanelExport.Controls.Add(this.rbCSV);
            this.PanelExport.Controls.Add(this.rbExcel);
            this.PanelExport.Location = new System.Drawing.Point(15, 0);
            this.PanelExport.Name = "PanelExport";
            this.PanelExport.Size = new System.Drawing.Size(207, 111);
            this.PanelExport.TabIndex = 6;
            this.PanelExport.TabStop = true;
            this.PanelExport.Visible = false;
            // 
            // rbJSON
            // 
            this.rbJSON.Location = new System.Drawing.Point(14, 71);
            this.rbJSON.Name = "rbJSON";
            this.rbJSON.Size = new System.Drawing.Size(85, 23);
            this.rbJSON.TabIndex = 4;
            this.rbJSON.Text = "File JSON";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(100, 9);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 27);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // rbXML
            // 
            this.rbXML.Location = new System.Drawing.Point(14, 51);
            this.rbXML.Name = "rbXML";
            this.rbXML.Size = new System.Drawing.Size(79, 23);
            this.rbXML.TabIndex = 2;
            this.rbXML.Text = "File XML";
            // 
            // rbCSV
            // 
            this.rbCSV.Location = new System.Drawing.Point(14, 30);
            this.rbCSV.Name = "rbCSV";
            this.rbCSV.Size = new System.Drawing.Size(77, 23);
            this.rbCSV.TabIndex = 1;
            this.rbCSV.Text = "File CSV";
            // 
            // rbExcel
            // 
            this.rbExcel.Checked = true;
            this.rbExcel.Location = new System.Drawing.Point(14, 9);
            this.rbExcel.Name = "rbExcel";
            this.rbExcel.Size = new System.Drawing.Size(82, 23);
            this.rbExcel.TabIndex = 0;
            this.rbExcel.TabStop = true;
            this.rbExcel.Text = "File Excel";
            // 
            // TabPageDebug
            // 
            this.TabPageDebug.Controls.Add(this.Button2);
            this.TabPageDebug.Controls.Add(this.txtDebug);
            this.TabPageDebug.Controls.Add(this.Button1);
            this.TabPageDebug.Controls.Add(this.cmbRecords);
            this.TabPageDebug.Hidden = true;
            this.TabPageDebug.Location = new System.Drawing.Point(0, 29);
            this.TabPageDebug.Name = "TabPageDebug";
            this.TabPageDebug.Padding = new Wisej.Web.Padding(3);
            this.TabPageDebug.Size = new System.Drawing.Size(898, 129);
            this.TabPageDebug.Text = "Debug";
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(359, 6);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(100, 28);
            this.Button2.TabIndex = 8;
            this.Button2.Text = "Button2";
            this.Button2.Visible = false;
            // 
            // txtDebug
            // 
            this.txtDebug.AutoSize = false;
            this.txtDebug.Location = new System.Drawing.Point(15, 6);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(100, 23);
            this.txtDebug.TabIndex = 5;
            this.txtDebug.Text = "Debug";
            this.txtDebug.Visible = false;
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(253, 4);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(100, 28);
            this.Button1.TabIndex = 7;
            this.Button1.Text = "Button1";
            this.Button1.Visible = false;
            // 
            // cmbRecords
            // 
            this.cmbRecords.Items.AddRange(new object[] {
            "100",
            "500",
            "1000",
            "2000",
            "5000",
            "10000"});
            this.cmbRecords.Location = new System.Drawing.Point(179, 6);
            this.cmbRecords.Name = "cmbRecords";
            this.cmbRecords.Size = new System.Drawing.Size(68, 30);
            this.cmbRecords.TabIndex = 4;
            this.cmbRecords.Visible = false;
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
            this.NavBar.Dock = Wisej.Web.DockStyle.Bottom;
            this.NavBar.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.NavBar.Location = new System.Drawing.Point(0, 591);
            this.NavBar.Margin = new Wisej.Web.Padding(0);
            this.NavBar.Name = "NavBar";
            this.NavBar.Size = new System.Drawing.Size(898, 44);
            this.NavBar.TabIndex = 8;
            this.NavBar.TabStop = false;
            // 
            // bFirst
            // 
            this.bFirst.AllowHtml = true;
            this.bFirst.ImageSource = "icon-first";
            this.bFirst.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bFirst.Name = "bFirst";
            this.bFirst.Text = "First";
            this.bFirst.Click += new System.EventHandler(this.bFirst_Click);
            // 
            // bPrev
            // 
            this.bPrev.AllowHtml = true;
            this.bPrev.ImageSource = "icon-left";
            this.bPrev.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrev.Name = "bPrev";
            this.bPrev.Text = "Previous";
            this.bPrev.Click += new System.EventHandler(this.bPrev_Click);
            // 
            // RecordLabel
            // 
            this.RecordLabel.AllowHtml = true;
            this.RecordLabel.Enabled = false;
            this.RecordLabel.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.RecordLabel.Name = "RecordLabel";
            this.RecordLabel.Text = "0<br>0";
            // 
            // bNext
            // 
            this.bNext.AllowHtml = true;
            this.bNext.ImageSource = "icon-right";
            this.bNext.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bNext.Name = "bNext";
            this.bNext.Text = "Next";
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // bLast
            // 
            this.bLast.AllowHtml = true;
            this.bLast.ImageSource = "icon-last";
            this.bLast.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bLast.Name = "bLast";
            this.bLast.Text = "Last";
            this.bLast.Click += new System.EventHandler(this.bLast_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.AllowHtml = true;
            this.bRefresh.ImageSource = "icon-redo";
            this.bRefresh.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Text = "Refresh";
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bDelete
            // 
            this.bDelete.AllowHtml = true;
            this.bDelete.ImageSource = "tab-close";
            this.bDelete.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bDelete.Name = "bDelete";
            this.bDelete.Text = "Delete Filters";
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // Records
            // 
            this.Records.AllowHtml = true;
            this.Records.DropDownMenu = this.ContextMenuRecords;
            this.Records.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.Records.Name = "Records";
            this.Records.Style = Wisej.Web.ToolBarButtonStyle.DropDownButton;
            this.Records.Text = "Max Rows";
            // 
            // ContextMenuRecords
            // 
            this.ContextMenuRecords.MenuItems.AddRange(new Wisej.Web.MenuItem[] {
            this.menuRecords100,
            this.menuRecords500,
            this.menuRecords1000,
            this.menuRecords2000,
            this.menuRecords5000,
            this.menuRecords10000,
            this.menuRecordsALL});
            this.ContextMenuRecords.Name = "ContextMenuRecords";
            this.ContextMenuRecords.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.ContextMenuRecords_MenuItemClicked);
            // 
            // menuRecords100
            // 
            this.menuRecords100.Index = 0;
            this.menuRecords100.Name = "menuRecords100";
            this.menuRecords100.Tag = 100;
            this.menuRecords100.Text = "100";
            // 
            // menuRecords500
            // 
            this.menuRecords500.Index = 1;
            this.menuRecords500.Name = "menuRecords500";
            this.menuRecords500.Tag = 500;
            this.menuRecords500.Text = "500";
            // 
            // menuRecords1000
            // 
            this.menuRecords1000.Index = 2;
            this.menuRecords1000.Name = "menuRecords1000";
            this.menuRecords1000.Tag = 1000;
            this.menuRecords1000.Text = "1000";
            // 
            // menuRecords2000
            // 
            this.menuRecords2000.Index = 3;
            this.menuRecords2000.Name = "menuRecords2000";
            this.menuRecords2000.Tag = 2000;
            this.menuRecords2000.Text = "2000";
            // 
            // menuRecords5000
            // 
            this.menuRecords5000.Index = 4;
            this.menuRecords5000.Name = "menuRecords5000";
            this.menuRecords5000.Tag = 5000;
            this.menuRecords5000.Text = "5000";
            // 
            // menuRecords10000
            // 
            this.menuRecords10000.Index = 5;
            this.menuRecords10000.Name = "menuRecords10000";
            this.menuRecords10000.Tag = 10000;
            this.menuRecords10000.Text = "10000";
            // 
            // menuRecordsALL
            // 
            this.menuRecordsALL.Index = 6;
            this.menuRecordsALL.Name = "menuRecordsALL";
            this.menuRecordsALL.Tag = 0;
            this.menuRecordsALL.Text = "ALL";
            // 
            // bPrint
            // 
            this.bPrint.AllowHtml = true;
            this.bPrint.ImageSource = "icon-print";
            this.bPrint.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrint.Name = "bPrint";
            this.bPrint.Text = "Print";
            // 
            // bSave
            // 
            this.bSave.AllowHtml = true;
            this.bSave.ImageSource = "icon-check";
            this.bSave.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bSave.Name = "bSave";
            this.bSave.Text = "Select";
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bSaveQBE
            // 
            this.bSaveQBE.AllowHtml = true;
            this.bSaveQBE.ImageSource = "icon-save";
            this.bSaveQBE.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bSaveQBE.Name = "bSaveQBE";
            this.bSaveQBE.Text = "Save Filters";
            // 
            // bLoadQBE
            // 
            this.bLoadQBE.AllowHtml = true;
            this.bLoadQBE.ImageSource = "icon-open";
            this.bLoadQBE.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bLoadQBE.Name = "bLoadQBE";
            this.bLoadQBE.Text = "Load Filters";
            // 
            // bClose
            // 
            this.bClose.AllowHtml = true;
            this.bClose.ImageSource = "icon-exit";
            this.bClose.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bClose.Name = "bClose";
            this.bClose.Text = "Close";
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // QBEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromName("@toolbar");
            this.ClientSize = new System.Drawing.Size(898, 635);
            this.Controls.Add(this.NavBar);
            this.Controls.Add(this.SplitContainer);
            this.Name = "QBEForm";
            this.ShowModalMask = true;
            this.StartPosition = Wisej.Web.FormStartPosition.CenterParent;
            this.Text = "QBEForm";
            this.Load += new System.EventHandler(this.XQBEForm_Load);
            this.Shown += new System.EventHandler(this.XQBEForm_Shown);
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.XQBEForm_FormClosed);
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.TabControl.ResumeLayout(false);
            this.TabPageReportQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.QueryGrid)).EndInit();
            this.TabPageExport.ResumeLayout(false);
            this.PanelExport.ResumeLayout(false);
            this.PanelExport.PerformLayout();
            this.TabPageDebug.ResumeLayout(false);
            this.TabPageDebug.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal Wisej.Web.SplitContainer SplitContainer;
        internal Wisej.Web.TabControl TabControl;
        internal Wisej.Web.TabPage TabPageReportQuery;
        private Wisej.Web.DataGridView QueryGrid;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcNomeCampo;
        private Wisej.Web.DataGridViewTextBoxColumn dgvcColumnValue;
        private Wisej.Web.DataGridViewButtonColumn dgvcQueryColumn;
        internal Wisej.Web.CheckBox chkLikeOperator;
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
        public Wisej.Web.DataGridView ResultGrid;
        internal Wisej.Web.RadioButton rbJSON;
        internal Wisej.Web.ContextMenu ContextMenuRecords;
        internal Wisej.Web.MenuItem menuRecords100;
        internal Wisej.Web.MenuItem menuRecords500;
        internal Wisej.Web.MenuItem menuRecords1000;
        internal Wisej.Web.MenuItem menuRecords2000;
        internal Wisej.Web.MenuItem menuRecords5000;
        internal Wisej.Web.MenuItem menuRecords10000;
        internal Wisej.Web.MenuItem menuRecordsALL;
    }
}