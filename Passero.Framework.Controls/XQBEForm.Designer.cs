using System.ComponentModel;

namespace Passero.Framework.Controls
{ 
   
    partial class XQBEForm<ModelClass> where ModelClass : class
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XQBEForm));
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
            resources.ApplyResources(this.SplitContainer, "SplitContainer");
            this.SplitContainer.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            resources.ApplyResources(this.SplitContainer.Panel1, "SplitContainer.Panel1");
            this.SplitContainer.Panel1.Controls.Add(this.ResultGrid);
            // 
            // SplitContainer.Panel2
            // 
            resources.ApplyResources(this.SplitContainer.Panel2, "SplitContainer.Panel2");
            this.SplitContainer.Panel2.Controls.Add(this.TabControl);
            // 
            // ResultGrid
            // 
            resources.ApplyResources(this.ResultGrid, "ResultGrid");
            this.ResultGrid.DefaultRowHeight = 24;
            this.ResultGrid.KeepSameRowHeight = true;
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.ReadOnly = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromName("@buttonFace");
            resources.ApplyResources(dataGridViewCellStyle1, "dataGridViewCellStyle1");
            this.ResultGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ResultGrid.RowTemplate.DefaultCellStyle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage")));
            this.ResultGrid.RowTemplate.DefaultCellStyle.BackgroundImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.BackgroundImageAlign")));
            this.ResultGrid.RowTemplate.DefaultCellStyle.BackgroundImageLayout = ((Wisej.Web.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout")));
            this.ResultGrid.RowTemplate.DefaultCellStyle.BackgroundImageSource = resources.GetString("resource.BackgroundImageSource");
            this.ResultGrid.RowTemplate.Height = 20;
            this.ResultGrid.RowEnter += new Wisej.Web.DataGridViewCellEventHandler(this.ResultGrid_RowEnter);
            this.ResultGrid.CellDoubleClick += new Wisej.Web.DataGridViewCellEventHandler(this.ResultGrid_CellDoubleClick);
            // 
            // TabControl
            // 
            resources.ApplyResources(this.TabControl, "TabControl");
            this.TabControl.Controls.Add(this.TabPageReportQuery);
            this.TabControl.Controls.Add(this.TabPageExport);
            this.TabControl.Controls.Add(this.TabPageDebug);
            this.TabControl.Name = "TabControl";
            this.TabControl.PageInsets = new Wisej.Web.Padding(1, 40, 1, 1);
            // 
            // TabPageReportQuery
            // 
            resources.ApplyResources(this.TabPageReportQuery, "TabPageReportQuery");
            this.TabPageReportQuery.Controls.Add(this.QueryGrid);
            this.TabPageReportQuery.Controls.Add(this.chkLikeOperator);
            this.TabPageReportQuery.Name = "TabPageReportQuery";
            // 
            // QueryGrid
            // 
            resources.ApplyResources(this.QueryGrid, "QueryGrid");
            this.QueryGrid.AllowSortingDataSource = false;
            this.QueryGrid.AllowUserToResizeColumns = false;
            this.QueryGrid.AllowUserToResizeRows = false;
            this.QueryGrid.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells;
            this.QueryGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromName("@buttonFace");
            this.QueryGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.QueryGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvcNomeCampo,
            this.dgvcColumnValue,
            this.dgvcQueryColumn});
            this.QueryGrid.DefaultSortMode = Wisej.Web.DataGridViewColumnsSortMode.NotSortable;
            this.QueryGrid.Name = "QueryGrid";
            this.QueryGrid.RowTemplate.DefaultCellStyle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage1")));
            this.QueryGrid.RowTemplate.DefaultCellStyle.BackgroundImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.BackgroundImageAlign1")));
            this.QueryGrid.RowTemplate.DefaultCellStyle.BackgroundImageLayout = ((Wisej.Web.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout1")));
            this.QueryGrid.RowTemplate.DefaultCellStyle.BackgroundImageSource = resources.GetString("resource.BackgroundImageSource1");
            // 
            // dgvcNomeCampo
            // 
            resources.ApplyResources(this.dgvcNomeCampo, "dgvcNomeCampo");
            this.dgvcNomeCampo.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcNomeCampo.MaxInputLength = 50;
            this.dgvcNomeCampo.Name = "dgvcNomeCampo";
            this.dgvcNomeCampo.ReadOnly = true;
            // 
            // dgvcColumnValue
            // 
            resources.ApplyResources(this.dgvcColumnValue, "dgvcColumnValue");
            this.dgvcColumnValue.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcColumnValue.MaxInputLength = 1024;
            this.dgvcColumnValue.Name = "dgvcColumnValue";
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
            // TabPageExport
            // 
            resources.ApplyResources(this.TabPageExport, "TabPageExport");
            this.TabPageExport.Controls.Add(this.PanelExport);
            this.TabPageExport.Name = "TabPageExport";
            // 
            // PanelExport
            // 
            resources.ApplyResources(this.PanelExport, "PanelExport");
            this.PanelExport.Controls.Add(this.rbJSON);
            this.PanelExport.Controls.Add(this.btnExport);
            this.PanelExport.Controls.Add(this.rbXML);
            this.PanelExport.Controls.Add(this.rbCSV);
            this.PanelExport.Controls.Add(this.rbExcel);
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
            resources.ApplyResources(this.rbExcel, "rbExcel");
            this.rbExcel.Checked = true;
            this.rbExcel.Name = "rbExcel";
            this.rbExcel.TabStop = true;
            // 
            // TabPageDebug
            // 
            resources.ApplyResources(this.TabPageDebug, "TabPageDebug");
            this.TabPageDebug.Controls.Add(this.Button2);
            this.TabPageDebug.Controls.Add(this.txtDebug);
            this.TabPageDebug.Controls.Add(this.Button1);
            this.TabPageDebug.Controls.Add(this.cmbRecords);
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
            resources.ApplyResources(this.cmbRecords, "cmbRecords");
            this.cmbRecords.Items.AddRange(new object[] {
            resources.GetString("cmbRecords.Items"),
            resources.GetString("cmbRecords.Items1"),
            resources.GetString("cmbRecords.Items2"),
            resources.GetString("cmbRecords.Items3"),
            resources.GetString("cmbRecords.Items4"),
            resources.GetString("cmbRecords.Items5")});
            this.cmbRecords.Name = "cmbRecords";
            // 
            // NavBar
            // 
            resources.ApplyResources(this.NavBar, "NavBar");
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
            this.Records.DropDownMenu = this.ContextMenuRecords;
            resources.ApplyResources(this.Records, "Records");
            this.Records.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.Records.Name = "Records";
            this.Records.Style = Wisej.Web.ToolBarButtonStyle.DropDownButton;
            // 
            // ContextMenuRecords
            // 
            resources.ApplyResources(this.ContextMenuRecords, "ContextMenuRecords");
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
            resources.ApplyResources(this.menuRecords100, "menuRecords100");
            this.menuRecords100.Index = 0;
            this.menuRecords100.Name = "menuRecords100";
            this.menuRecords100.Tag = 100;
            // 
            // menuRecords500
            // 
            resources.ApplyResources(this.menuRecords500, "menuRecords500");
            this.menuRecords500.Index = 1;
            this.menuRecords500.Name = "menuRecords500";
            this.menuRecords500.Tag = 500;
            // 
            // menuRecords1000
            // 
            resources.ApplyResources(this.menuRecords1000, "menuRecords1000");
            this.menuRecords1000.Index = 2;
            this.menuRecords1000.Name = "menuRecords1000";
            this.menuRecords1000.Tag = 1000;
            // 
            // menuRecords2000
            // 
            resources.ApplyResources(this.menuRecords2000, "menuRecords2000");
            this.menuRecords2000.Index = 3;
            this.menuRecords2000.Name = "menuRecords2000";
            this.menuRecords2000.Tag = 2000;
            // 
            // menuRecords5000
            // 
            resources.ApplyResources(this.menuRecords5000, "menuRecords5000");
            this.menuRecords5000.Index = 4;
            this.menuRecords5000.Name = "menuRecords5000";
            this.menuRecords5000.Tag = 5000;
            // 
            // menuRecords10000
            // 
            resources.ApplyResources(this.menuRecords10000, "menuRecords10000");
            this.menuRecords10000.Index = 5;
            this.menuRecords10000.Name = "menuRecords10000";
            this.menuRecords10000.Tag = 10000;
            // 
            // menuRecordsALL
            // 
            resources.ApplyResources(this.menuRecordsALL, "menuRecordsALL");
            this.menuRecordsALL.Index = 6;
            this.menuRecordsALL.Name = "menuRecordsALL";
            this.menuRecordsALL.Tag = 0;
            // 
            // bPrint
            // 
            this.bPrint.AllowHtml = true;
            resources.ApplyResources(this.bPrint, "bPrint");
            this.bPrint.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrint.Name = "bPrint";
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
            // XQBEForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromName("@toolbar");
            this.Controls.Add(this.NavBar);
            this.Controls.Add(this.SplitContainer);
            this.Name = "XQBEForm";
            this.ShowModalMask = true;
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