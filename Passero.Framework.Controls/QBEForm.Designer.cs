using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Wisej.Web;

namespace BasicDALWisejControls
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class QBEForm : Form
    {

        // UserControl overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Wisej Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Wisej Designer
        // It can be modified using the Wisej Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var DataGridViewCellStyle1 = new DataGridViewCellStyle();
            var DataGridViewCellStyle2 = new DataGridViewCellStyle();
            var DataGridViewCellStyle3 = new DataGridViewCellStyle();
            var DataGridViewCellStyle4 = new DataGridViewCellStyle();
            ContextMenuRecords = new ContextMenu(components);
            ContextMenuRecords.MenuItemClicked += new MenuItemEventHandler(ContextMenuRecords_MenuItemClicked);
            menuRecords100 = new MenuItem();
            menuRecords500 = new MenuItem();
            menuRecords1000 = new MenuItem();
            menuRecords2000 = new MenuItem();
            menuRecords5000 = new MenuItem();
            menuRecords10000 = new MenuItem();
            menuRecordsALL = new MenuItem();
            SaveFileDialog = new SaveFileDialog(components);
            NavBar = new ToolBar();
            bFirst = new ToolBarButton();
            bFirst.Click += new EventHandler(bFirst_Click);
            bPrev = new ToolBarButton();
            bPrev.Click += new EventHandler(bPrev_Click);
            RecordLabel = new ToolBarButton();
            bNext = new ToolBarButton();
            bNext.Click += new EventHandler(bNext_Click);
            bLast = new ToolBarButton();
            bLast.Click += new EventHandler(bLast_Click);
            bRefresh = new ToolBarButton();
            bRefresh.Click += new EventHandler(bRefresh_Click);
            bDelete = new ToolBarButton();
            bDelete.Click += new EventHandler(bDelete_Click);
            Records = new ToolBarButton();
            bPrint = new ToolBarButton();
            bPrint.Click += new EventHandler(bPrint_Click);
            bSave = new ToolBarButton();
            bSave.Click += new EventHandler(bSave_Click);
            bSaveQBE = new ToolBarButton();
            bSaveQBE.Click += new EventHandler(bSaveQBE_Click);
            bLoadQBE = new ToolBarButton();
            bLoadQBE.Click += new EventHandler(bLoadQBE_Click);
            bClose = new ToolBarButton();
            bClose.Click += new EventHandler(bClose_Click);
            TabControl = new TabControl();
            TabControl.Selected += new TabControlEventHandler(TabControl_Selected);
            TabControl.SelectedIndexChanged += new EventHandler(TabControl_SelectedIndexChanged);
            TabPageStampe = new TabPage();
            _ReportGrid = new DataGridView();
            _ReportGrid.Click += new EventHandler(ReportGrid_Click);
            dgvcNomeReport = new DataGridViewTextBoxColumn();
            dgvcDescrizioneReport = new DataGridViewTextBoxColumn();
            dgvcReportFileName = new DataGridViewTextBoxColumn();
            TabPageCriteriRicerca = new TabPage();
            _QueryGrid = new DataGridView();
            dgvcNomeCampo = new DataGridViewTextBoxColumn();
            dgvcValoreCampo = new DataGridViewTextBoxColumn();
            dgvcQueryCampo = new DataGridViewButtonColumn();
            chkLikeOperator = new CheckBox();
            TabPageOrdinamento = new TabPage();
            btnSortDown = new Button();
            btnSortDown.Click += new EventHandler(btnSortDown_Click);
            btnSortUp = new Button();
            btnSortUp.Click += new EventHandler(btnSortUp_Click);
            dgv_SelectedSortColumns = new DataGridView();
            dgvc_SelectedSortColumns_position = new DataGridViewTextBoxColumn();
            dgvc_SelectedSortColumns_name = new DataGridViewTextBoxColumn();
            dgvc_SelectedSortColumns_friendlyname = new DataGridViewTextBoxColumn();
            dgvc_SelectedSortColumns_ascdesc = new DataGridViewComboBoxColumn();
            btnSortRemove = new Button();
            btnSortRemove.Click += new EventHandler(btnSortRemove_Click);
            btnSortAdd = new Button();
            btnSortAdd.Click += new EventHandler(btnSortAdd_Click);
            lstSortColumns = new ListBox();
            TabPageEsporta = new TabPage();
            PanelEsporta = new Panel();
            btnEsporta = new Button();
            btnEsporta.Click += new EventHandler(btnEsporta_Click);
            rbXML = new RadioButton();
            rbCSV = new RadioButton();
            rbExcel = new RadioButton();
            TabPageDebug = new TabPage();
            Button2 = new Button();
            Button2.Click += new EventHandler(Button2_Click);
            txtDebug = new TextBox();
            Button1 = new Button();
            Button1.Click += new EventHandler(Button1_Click);
            cmbRecords = new ComboBox();
            _ResultGrid = new DataGridView();
            _ResultGrid.DoubleClick += new EventHandler(ResultGrid_DoubleClick);
            _ResultGrid.CellEnter += new DataGridViewCellEventHandler(ResultGrid_CellEnter);
            _ResultGrid.KeyDown += new KeyEventHandler(ResultGrid_KeyDown);
            _ResultGrid.RowEnter += new DataGridViewCellEventHandler(ResultGrid_RowEnter);
            PdfViewer = new PdfViewer();
            WebBrowser = new WebBrowser();
            AspNetPanel = new AspNetPanel();
            AspNetPanel.Resize += new EventHandler(AspNetPanel_Resize);
            SplitContainer1 = new SplitContainer();
            SplitContainer1.Resize += new EventHandler(SplitContainer1_Resize);
            SplitContainer1.DoubleClick += new EventHandler(SplitContainer1_DoubleClick);
            SplitContainer1.SplitterMoved += new SplitterEventHandler(SplitContainer1_SplitterMoved);
            PanelReportViewer = new Panel();
            PanelReportViewer.PanelCollapsed += new EventHandler(PanelReportViewer_PanelCollapsed);
            PanelReportViewer.Resize += new EventHandler(PanelReportViewer_Resize);
            PanelReportInfo = new Panel();
            txtReportDescription = new TextBox();
            txtReportTitle = new TextBox();
            TabControl.SuspendLayout();
            TabPageStampe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_ReportGrid).BeginInit();
            TabPageCriteriRicerca.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_QueryGrid).BeginInit();
            TabPageOrdinamento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_SelectedSortColumns).BeginInit();
            TabPageEsporta.SuspendLayout();
            PanelEsporta.SuspendLayout();
            TabPageDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_ResultGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SplitContainer1).BeginInit();
            SplitContainer1.Panel1.SuspendLayout();
            SplitContainer1.Panel2.SuspendLayout();
            SplitContainer1.SuspendLayout();
            PanelReportViewer.SuspendLayout();
            PanelReportInfo.SuspendLayout();
            SuspendLayout();
            // 
            // ContextMenuRecords
            // 
            ContextMenuRecords.MenuItems.AddRange(new MenuItem[] { menuRecords100, menuRecords500, menuRecords1000, menuRecords2000, menuRecords5000, menuRecords10000, menuRecordsALL });
            ContextMenuRecords.Name = "ContextMenuRecords";
            // 
            // menuRecords100
            // 
            menuRecords100.Index = 0;
            menuRecords100.Name = "menuRecords100";
            menuRecords100.Tag = "100";
            menuRecords100.Text = "100";
            // 
            // menuRecords500
            // 
            menuRecords500.Index = 1;
            menuRecords500.Name = "menuRecords500";
            menuRecords500.Tag = "500";
            menuRecords500.Text = "500";
            // 
            // menuRecords1000
            // 
            menuRecords1000.Index = 2;
            menuRecords1000.Name = "menuRecords1000";
            menuRecords1000.Tag = "1000";
            menuRecords1000.Text = "1000";
            // 
            // menuRecords2000
            // 
            menuRecords2000.Index = 3;
            menuRecords2000.Name = "menuRecords2000";
            menuRecords2000.Tag = "2000";
            menuRecords2000.Text = "2000";
            // 
            // menuRecords5000
            // 
            menuRecords5000.Index = 4;
            menuRecords5000.Name = "menuRecords5000";
            menuRecords5000.Tag = "5000";
            menuRecords5000.Text = "5000";
            // 
            // menuRecords10000
            // 
            menuRecords10000.Index = 5;
            menuRecords10000.Name = "menuRecords10000";
            menuRecords10000.Tag = "10000";
            menuRecords10000.Text = "10000";
            // 
            // menuRecordsALL
            // 
            menuRecordsALL.Index = 6;
            menuRecordsALL.Name = "menuRecordsALL";
            menuRecordsALL.Tag = "0";
            menuRecordsALL.Text = "0";
            // 
            // SaveFileDialog
            // 
            SaveFileDialog.FileName = "SaveFileDialog";
            // 
            // NavBar
            // 
            NavBar.BorderStyle = BorderStyle.Solid;
            NavBar.Buttons.AddRange(new ToolBarButton[] { bFirst, bPrev, RecordLabel, bNext, bLast, bRefresh, bDelete, Records, bPrint, bSave, bSaveQBE, bLoadQBE, bClose });
            NavBar.Dock = DockStyle.Bottom;
            NavBar.Font = new System.Drawing.Font("default", 10.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            NavBar.Location = new System.Drawing.Point(0, 649);
            NavBar.Margin = new Padding(0);
            NavBar.Name = "NavBar";
            NavBar.Size = new System.Drawing.Size(898, 44);
            NavBar.TabIndex = 7;
            NavBar.TabStop = false;
            // 
            // bFirst
            // 
            bFirst.AllowHtml = true;
            bFirst.ImageSource = "icon-first";
            bFirst.Margin = new Padding(0, -5, 0, 0);
            bFirst.Name = "bFirst";
            bFirst.Text = "Inizio";
            // 
            // bPrev
            // 
            bPrev.AllowHtml = true;
            bPrev.ImageSource = "icon-left";
            bPrev.Margin = new Padding(0, -5, 0, 0);
            bPrev.Name = "bPrev";
            bPrev.Text = "Prec.";
            // 
            // RecordLabel
            // 
            RecordLabel.AllowHtml = true;
            RecordLabel.Enabled = false;
            RecordLabel.Margin = new Padding(0, -5, 0, 0);
            RecordLabel.Name = "RecordLabel";
            RecordLabel.Text = "0<br>0";
            // 
            // bNext
            // 
            bNext.AllowHtml = true;
            bNext.ImageSource = "icon-right";
            bNext.Margin = new Padding(0, -5, 0, 0);
            bNext.Name = "bNext";
            bNext.Text = "Succ.";
            // 
            // bLast
            // 
            bLast.AllowHtml = true;
            bLast.ImageSource = "icon-last";
            bLast.Margin = new Padding(0, -5, 0, 0);
            bLast.Name = "bLast";
            bLast.Text = "Fine";
            // 
            // bRefresh
            // 
            bRefresh.AllowHtml = true;
            bRefresh.ImageSource = "icon-redo";
            bRefresh.Margin = new Padding(0, -5, 0, 0);
            bRefresh.Name = "bRefresh";
            bRefresh.Text = "Aggiorna";
            // 
            // bDelete
            // 
            bDelete.AllowHtml = true;
            bDelete.ImageSource = "tab-close";
            bDelete.Margin = new Padding(0, -5, 0, 0);
            bDelete.Name = "bDelete";
            bDelete.Text = "Elimina Filtro";
            // 
            // Records
            // 
            Records.AllowHtml = true;
            Records.DropDownMenu = ContextMenuRecords;
            Records.Margin = new Padding(0, -5, 0, 0);
            Records.Name = "Records";
            Records.Style = ToolBarButtonStyle.DropDownButton;
            Records.Text = "Max Rows";
            // 
            // bPrint
            // 
            bPrint.AllowHtml = true;
            bPrint.ImageSource = "icon-print";
            bPrint.Margin = new Padding(0, -5, 0, 0);
            bPrint.Name = "bPrint";
            bPrint.Text = "Stampa";
            // 
            // bSave
            // 
            bSave.AllowHtml = true;
            bSave.ImageSource = "icon-check";
            bSave.Margin = new Padding(0, -5, 0, 0);
            bSave.Name = "bSave";
            bSave.Text = "Seleziona";
            // 
            // bSaveQBE
            // 
            bSaveQBE.AllowHtml = true;
            bSaveQBE.ImageSource = "icon-save";
            bSaveQBE.Margin = new Padding(0, -5, 0, 0);
            bSaveQBE.Name = "bSaveQBE";
            bSaveQBE.Text = "Salva Filtro";
            // 
            // bLoadQBE
            // 
            bLoadQBE.AllowHtml = true;
            bLoadQBE.ImageSource = "icon-open";
            bLoadQBE.Margin = new Padding(0, -5, 0, 0);
            bLoadQBE.Name = "bLoadQBE";
            bLoadQBE.Text = "Carica Filtro";
            // 
            // bClose
            // 
            bClose.AllowHtml = true;
            bClose.ImageSource = "icon-exit";
            bClose.Margin = new Padding(0, -5, 0, 0);
            bClose.Name = "bClose";
            bClose.Text = "Chiudi";
            // 
            // TabControl
            // 
            TabControl.Controls.Add(TabPageStampe);
            TabControl.Controls.Add(TabPageCriteriRicerca);
            TabControl.Controls.Add(TabPageOrdinamento);
            TabControl.Controls.Add(TabPageEsporta);
            TabControl.Controls.Add(TabPageDebug);
            TabControl.Dock = DockStyle.Top;
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Name = "TabControl";
            TabControl.PageInsets = new Padding(1, 40, 1, 1);
            TabControl.Size = new System.Drawing.Size(896, 199);
            TabControl.TabIndex = 0;
            // 
            // TabPageStampe
            // 
            TabPageStampe.Controls.Add(_ReportGrid);
            TabPageStampe.Hidden = true;
            TabPageStampe.Location = new System.Drawing.Point(1, 40);
            TabPageStampe.Name = "TabPageStampe";
            TabPageStampe.Size = new System.Drawing.Size(894, 158);
            TabPageStampe.Text = "Report Disponibili";
            // 
            // ReportGrid
            // 
            _ReportGrid.AutoGenerateColumns = false;
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromName("@buttonFace");
            _ReportGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1;
            _ReportGrid.Columns.AddRange(new DataGridViewColumn[] { dgvcNomeReport, dgvcDescrizioneReport, dgvcReportFileName });
            _ReportGrid.Location = new System.Drawing.Point(3, 5);
            _ReportGrid.Name = "_ReportGrid";
            _ReportGrid.ReadOnly = true;
            _ReportGrid.RowHeadersWidth = 24;
            _ReportGrid.RowTemplate.ReadOnly = true;
            _ReportGrid.Size = new System.Drawing.Size(377, 105);
            _ReportGrid.TabIndex = 2;
            // 
            // dgvcNomeReport
            // 
            dgvcNomeReport.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DataGridViewCellStyle2.Font = new System.Drawing.Font("default", 13.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            dgvcNomeReport.DefaultCellStyle = DataGridViewCellStyle2;
            dgvcNomeReport.HeaderText = "Nome Report";
            dgvcNomeReport.Name = "dgvcNomeReport";
            // 
            // dgvcDescrizioneReport
            // 
            dgvcDescrizioneReport.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvcDescrizioneReport.HeaderText = "Descrizione";
            dgvcDescrizioneReport.Name = "dgvcDescrizioneReport";
            // 
            // dgvcReportFileName
            // 
            dgvcReportFileName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvcReportFileName.HeaderText = "Report FileName";
            dgvcReportFileName.Name = "dgvcReportFileName";
            // 
            // TabPageCriteriRicerca
            // 
            TabPageCriteriRicerca.Controls.Add(_QueryGrid);
            TabPageCriteriRicerca.Controls.Add(chkLikeOperator);
            TabPageCriteriRicerca.Location = new System.Drawing.Point(1, 40);
            TabPageCriteriRicerca.Name = "TabPageCriteriRicerca";
            TabPageCriteriRicerca.Size = new System.Drawing.Size(894, 158);
            TabPageCriteriRicerca.Text = "Criteri Ricerca";
            // 
            // QueryGrid
            // 
            _QueryGrid.AllowSortingDataSource = false;
            _QueryGrid.AllowUserToResizeColumns = false;
            _QueryGrid.AllowUserToResizeRows = false;
            _QueryGrid.AutoGenerateColumns = false;
            _QueryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _QueryGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromName("@buttonFace");
            _QueryGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3;
            _QueryGrid.Columns.AddRange(new DataGridViewColumn[] { dgvcNomeCampo, dgvcValoreCampo, dgvcQueryCampo });
            _QueryGrid.DefaultSortMode = DataGridViewColumnsSortMode.NotSortable;
            _QueryGrid.Location = new System.Drawing.Point(3, 30);
            _QueryGrid.Name = "_QueryGrid";
            _QueryGrid.RowHeadersWidth = 24;
            _QueryGrid.Size = new System.Drawing.Size(520, 65);
            _QueryGrid.TabIndex = 3;
            // 
            // dgvcNomeCampo
            // 
            dgvcNomeCampo.HeaderText = "Campo";
            dgvcNomeCampo.MaxInputLength = 50;
            dgvcNomeCampo.Name = "dgvcNomeCampo";
            // 
            // dgvcValoreCampo
            // 
            dgvcValoreCampo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvcValoreCampo.HeaderText = "Valore";
            dgvcValoreCampo.MaxInputLength = 1024;
            dgvcValoreCampo.Name = "dgvcValoreCampo";
            // 
            // dgvcQueryCampo
            // 
            dgvcQueryCampo.HeaderText = "?";
            dgvcQueryCampo.Name = "dgvcQueryCampo";
            dgvcQueryCampo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // chkLikeOperator
            // 
            chkLikeOperator.AutoSize = false;
            chkLikeOperator.Location = new System.Drawing.Point(3, 3);
            chkLikeOperator.Name = "chkLikeOperator";
            chkLikeOperator.Size = new System.Drawing.Size(194, 21);
            chkLikeOperator.TabIndex = 2;
            chkLikeOperator.Text = "Usa operatore \"CONTIENE\"";
            // 
            // TabPageOrdinamento
            // 
            TabPageOrdinamento.Controls.Add(btnSortDown);
            TabPageOrdinamento.Controls.Add(btnSortUp);
            TabPageOrdinamento.Controls.Add(dgv_SelectedSortColumns);
            TabPageOrdinamento.Controls.Add(btnSortRemove);
            TabPageOrdinamento.Controls.Add(btnSortAdd);
            TabPageOrdinamento.Controls.Add(lstSortColumns);
            TabPageOrdinamento.Location = new System.Drawing.Point(1, 40);
            TabPageOrdinamento.Name = "TabPageOrdinamento";
            TabPageOrdinamento.Size = new System.Drawing.Size(894, 158);
            TabPageOrdinamento.Text = "Ordinamento Report";
            // 
            // btnSortDown
            // 
            btnSortDown.ImageSource = "scrollbar-arrow-down";
            btnSortDown.Location = new System.Drawing.Point(766, 59);
            btnSortDown.Name = "btnSortDown";
            btnSortDown.Size = new System.Drawing.Size(36, 25);
            btnSortDown.TabIndex = 7;
            // 
            // btnSortUp
            // 
            btnSortUp.ImageSource = "scrollbar-arrow-up";
            btnSortUp.Location = new System.Drawing.Point(766, 27);
            btnSortUp.Name = "btnSortUp";
            btnSortUp.Size = new System.Drawing.Size(36, 25);
            btnSortUp.TabIndex = 6;
            // 
            // dgv_SelectedSortColumns
            // 
            dgv_SelectedSortColumns.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgv_SelectedSortColumns.AutoGenerateColumns = false;
            dgv_SelectedSortColumns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_SelectedSortColumns.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv_SelectedSortColumns.ColumnHeadersVisible = false;
            dgv_SelectedSortColumns.Columns.AddRange(new DataGridViewColumn[] { dgvc_SelectedSortColumns_position, dgvc_SelectedSortColumns_name, dgvc_SelectedSortColumns_friendlyname, dgvc_SelectedSortColumns_ascdesc });
            dgv_SelectedSortColumns.DefaultRowHeight = 24;
            dgv_SelectedSortColumns.Location = new System.Drawing.Point(294, 27);
            dgv_SelectedSortColumns.Name = "dgv_SelectedSortColumns";
            dgv_SelectedSortColumns.ScrollBars = ScrollBars.Vertical;
            dgv_SelectedSortColumns.Selectable = true;
            dgv_SelectedSortColumns.Size = new System.Drawing.Size(463, 120);
            dgv_SelectedSortColumns.TabIndex = 5;
            // 
            // dgvc_SelectedSortColumns_position
            // 
            dgvc_SelectedSortColumns_position.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvc_SelectedSortColumns_position.DataPropertyName = "position";
            dgvc_SelectedSortColumns_position.HeaderText = "Position";
            dgvc_SelectedSortColumns_position.Name = "dgvc_SelectedSortColumns_position";
            // 
            // dgvc_SelectedSortColumns_name
            // 
            dgvc_SelectedSortColumns_name.DataPropertyName = "Name";
            dgvc_SelectedSortColumns_name.HeaderText = "Name";
            dgvc_SelectedSortColumns_name.Name = "dgvc_SelectedSortColumns_name";
            dgvc_SelectedSortColumns_name.Visible = false;
            // 
            // dgvc_SelectedSortColumns_friendlyname
            // 
            dgvc_SelectedSortColumns_friendlyname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvc_SelectedSortColumns_friendlyname.DataPropertyName = "FriendlyName";
            dgvc_SelectedSortColumns_friendlyname.HeaderText = "FriendlyName";
            dgvc_SelectedSortColumns_friendlyname.MinimumWidth = 200;
            dgvc_SelectedSortColumns_friendlyname.Name = "dgvc_SelectedSortColumns_friendlyname";
            dgvc_SelectedSortColumns_friendlyname.ReadOnly = true;
            dgvc_SelectedSortColumns_friendlyname.Width = 200;
            // 
            // dgvc_SelectedSortColumns_ascdesc
            // 
            dgvc_SelectedSortColumns_ascdesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvc_SelectedSortColumns_ascdesc.DataPropertyName = "AscDesc";
            dgvc_SelectedSortColumns_ascdesc.DropDownStyle = ComboBoxStyle.DropDownList;
            dgvc_SelectedSortColumns_ascdesc.HeaderText = "AscDesc";
            dgvc_SelectedSortColumns_ascdesc.Items.AddRange(new object[] { "ASC", "DESC" });
            dgvc_SelectedSortColumns_ascdesc.MinimumWidth = 100;
            dgvc_SelectedSortColumns_ascdesc.Name = "dgvc_SelectedSortColumns_ascdesc";
            dgvc_SelectedSortColumns_ascdesc.ValueType = typeof(object);
            // 
            // btnSortRemove
            // 
            btnSortRemove.ImageSource = "scrollbar-arrow-left";
            btnSortRemove.Location = new System.Drawing.Point(252, 58);
            btnSortRemove.Name = "btnSortRemove";
            btnSortRemove.Size = new System.Drawing.Size(36, 25);
            btnSortRemove.TabIndex = 3;
            // 
            // btnSortAdd
            // 
            btnSortAdd.ImageSource = "scrollbar-arrow-right";
            btnSortAdd.Location = new System.Drawing.Point(252, 27);
            btnSortAdd.Name = "btnSortAdd";
            btnSortAdd.Size = new System.Drawing.Size(36, 25);
            btnSortAdd.TabIndex = 2;
            // 
            // lstSortColumns
            // 
            lstSortColumns.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstSortColumns.Label.Font = new System.Drawing.Font("default", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            lstSortColumns.LabelText = "Colonne disponibili";
            lstSortColumns.Location = new System.Drawing.Point(6, 7);
            lstSortColumns.Name = "lstSortColumns";
            lstSortColumns.Size = new System.Drawing.Size(240, 140);
            lstSortColumns.TabIndex = 0;
            // 
            // TabPageEsporta
            // 
            TabPageEsporta.Controls.Add(PanelEsporta);
            TabPageEsporta.Hidden = true;
            TabPageEsporta.Location = new System.Drawing.Point(1, 40);
            TabPageEsporta.Name = "TabPageEsporta";
            TabPageEsporta.Size = new System.Drawing.Size(894, 158);
            TabPageEsporta.Text = "Esporta";
            // 
            // PanelEsporta
            // 
            PanelEsporta.Controls.Add(btnEsporta);
            PanelEsporta.Controls.Add(rbXML);
            PanelEsporta.Controls.Add(rbCSV);
            PanelEsporta.Controls.Add(rbExcel);
            PanelEsporta.Location = new System.Drawing.Point(15, 0);
            PanelEsporta.Name = "PanelEsporta";
            PanelEsporta.Size = new System.Drawing.Size(207, 78);
            PanelEsporta.TabIndex = 6;
            PanelEsporta.TabStop = true;
            PanelEsporta.Visible = false;
            // 
            // btnEsporta
            // 
            btnEsporta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEsporta.Location = new System.Drawing.Point(100, 9);
            btnEsporta.Name = "btnEsporta";
            btnEsporta.Size = new System.Drawing.Size(100, 27);
            btnEsporta.TabIndex = 3;
            btnEsporta.Text = "Esporta";
            // 
            // rbXML
            // 
            rbXML.Location = new System.Drawing.Point(14, 51);
            rbXML.Name = "rbXML";
            rbXML.Size = new System.Drawing.Size(79, 23);
            rbXML.TabIndex = 2;
            rbXML.Text = "File XML";
            // 
            // rbCSV
            // 
            rbCSV.Location = new System.Drawing.Point(14, 30);
            rbCSV.Name = "rbCSV";
            rbCSV.Size = new System.Drawing.Size(77, 23);
            rbCSV.TabIndex = 1;
            rbCSV.Text = "File CSV";
            // 
            // rbExcel
            // 
            rbExcel.Checked = true;
            rbExcel.Location = new System.Drawing.Point(14, 9);
            rbExcel.Name = "rbExcel";
            rbExcel.Size = new System.Drawing.Size(82, 23);
            rbExcel.TabIndex = 0;
            rbExcel.TabStop = true;
            rbExcel.Text = "File Excel";
            // 
            // TabPageDebug
            // 
            TabPageDebug.Controls.Add(Button2);
            TabPageDebug.Controls.Add(txtDebug);
            TabPageDebug.Controls.Add(Button1);
            TabPageDebug.Controls.Add(cmbRecords);
            TabPageDebug.Hidden = true;
            TabPageDebug.Location = new System.Drawing.Point(1, 40);
            TabPageDebug.Name = "TabPageDebug";
            TabPageDebug.Padding = new Padding(3);
            TabPageDebug.Size = new System.Drawing.Size(894, 158);
            TabPageDebug.Text = "Debug";
            // 
            // Button2
            // 
            Button2.Location = new System.Drawing.Point(359, 6);
            Button2.Name = "Button2";
            Button2.Size = new System.Drawing.Size(100, 28);
            Button2.TabIndex = 8;
            Button2.Text = "Button2";
            Button2.Visible = false;
            // 
            // txtDebug
            // 
            txtDebug.AutoSize = false;
            txtDebug.Location = new System.Drawing.Point(15, 6);
            txtDebug.Multiline = true;
            txtDebug.Name = "txtDebug";
            txtDebug.Size = new System.Drawing.Size(100, 23);
            txtDebug.TabIndex = 5;
            txtDebug.Text = "Debug";
            txtDebug.Visible = false;
            // 
            // Button1
            // 
            Button1.Location = new System.Drawing.Point(253, 4);
            Button1.Name = "Button1";
            Button1.Size = new System.Drawing.Size(100, 28);
            Button1.TabIndex = 7;
            Button1.Text = "Button1";
            Button1.Visible = false;
            // 
            // cmbRecords
            // 
            cmbRecords.Items.AddRange(new object[] { "100", "500", "1000", "2000", "5000", "10000" });
            cmbRecords.Location = new System.Drawing.Point(179, 6);
            cmbRecords.Name = "cmbRecords";
            cmbRecords.Size = new System.Drawing.Size(68, 30);
            cmbRecords.TabIndex = 4;
            cmbRecords.Visible = false;
            // 
            // ResultGrid
            // 
            _ResultGrid.AutoGenerateColumns = false;
            _ResultGrid.DefaultRowHeight = 24;
            _ResultGrid.KeepSameRowHeight = true;
            _ResultGrid.Location = new System.Drawing.Point(16, 17);
            _ResultGrid.Name = "_ResultGrid";
            _ResultGrid.ReadOnly = true;
            DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromName("@buttonFace");
            _ResultGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle4;
            _ResultGrid.RowHeadersWidth = 24;
            _ResultGrid.RowTemplate.Height = 20;
            _ResultGrid.Size = new System.Drawing.Size(200, 177);
            _ResultGrid.TabIndex = 0;
            _ResultGrid.Visible = false;
            // 
            // PdfViewer
            // 
            PdfViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            PdfViewer.Location = new System.Drawing.Point(323, 43);
            PdfViewer.Name = "PdfViewer";
            PdfViewer.Size = new System.Drawing.Size(98, 64);
            PdfViewer.TabIndex = 1;
            PdfViewer.ViewerType = PdfViewerType.Mozilla;
            PdfViewer.Visible = false;
            // 
            // WebBrowser
            // 
            WebBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            WebBrowser.Location = new System.Drawing.Point(222, 43);
            WebBrowser.Name = "WebBrowser";
            WebBrowser.Size = new System.Drawing.Size(95, 64);
            WebBrowser.TabIndex = 3;
            WebBrowser.Url = new Uri("~/BasicDalWisejCRViewer.aspx", UriKind.Relative);
            WebBrowser.Visible = false;
            // 
            // AspNetPanel
            // 
            AspNetPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            AspNetPanel.Location = new System.Drawing.Point(58, 43);
            AspNetPanel.Name = "AspNetPanel";
            AspNetPanel.Size = new System.Drawing.Size(158, 64);
            AspNetPanel.TabIndex = 4;
            AspNetPanel.Text = "AspNetPanel";
            AspNetPanel.Visible = false;
            // 
            // SplitContainer1
            // 
            SplitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            SplitContainer1.BackColor = System.Drawing.SystemColors.Window;
            SplitContainer1.ForeColor = System.Drawing.SystemColors.WindowText;
            SplitContainer1.Location = new System.Drawing.Point(0, 0);
            SplitContainer1.Margin = new Padding(0);
            SplitContainer1.Name = "SplitContainer1";
            SplitContainer1.Orientation = Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            SplitContainer1.Panel1.Controls.Add(PanelReportViewer);
            SplitContainer1.Panel1.Controls.Add(_ResultGrid);
            // 
            // SplitContainer1.Panel2
            // 
            SplitContainer1.Panel2.Controls.Add(TabControl);
            SplitContainer1.Size = new System.Drawing.Size(898, 644);
            SplitContainer1.SplitterDistance = 432;
            SplitContainer1.TabIndex = 0;
            // 
            // PanelReportViewer
            // 
            PanelReportViewer.Controls.Add(PanelReportInfo);
            PanelReportViewer.Controls.Add(AspNetPanel);
            PanelReportViewer.Controls.Add(WebBrowser);
            PanelReportViewer.Controls.Add(PdfViewer);
            PanelReportViewer.Location = new System.Drawing.Point(222, 17);
            PanelReportViewer.Name = "PanelReportViewer";
            PanelReportViewer.Size = new System.Drawing.Size(548, 120);
            PanelReportViewer.TabIndex = 6;
            PanelReportViewer.Visible = false;
            // 
            // PanelReportInfo
            // 
            PanelReportInfo.BackColor = System.Drawing.Color.FromName("@toolbar");
            PanelReportInfo.Controls.Add(txtReportDescription);
            PanelReportInfo.Controls.Add(txtReportTitle);
            PanelReportInfo.Location = new System.Drawing.Point(3, 1);
            PanelReportInfo.Name = "PanelReportInfo";
            PanelReportInfo.Size = new System.Drawing.Size(542, 36);
            PanelReportInfo.TabIndex = 5;
            PanelReportInfo.Visible = false;
            // 
            // txtReportDescription
            // 
            txtReportDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtReportDescription.Focusable = false;
            txtReportDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            txtReportDescription.Label.Font = new System.Drawing.Font("default", 13.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            txtReportDescription.Label.Position = LabelPosition.Left;
            txtReportDescription.LabelText = "Descrizione";
            txtReportDescription.Location = new System.Drawing.Point(303, 2);
            txtReportDescription.Name = "txtReportDescription";
            txtReportDescription.ReadOnly = true;
            txtReportDescription.Size = new System.Drawing.Size(230, 30);
            txtReportDescription.TabIndex = 1;
            txtReportDescription.TabStop = false;
            // 
            // txtReportTitle
            // 
            txtReportTitle.Focusable = false;
            txtReportTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            txtReportTitle.Label.Font = new System.Drawing.Font("default", 13.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            txtReportTitle.Label.Position = LabelPosition.Left;
            txtReportTitle.LabelText = "Report";
            txtReportTitle.Location = new System.Drawing.Point(5, 2);
            txtReportTitle.Name = "txtReportTitle";
            txtReportTitle.ReadOnly = true;
            txtReportTitle.Size = new System.Drawing.Size(292, 30);
            txtReportTitle.TabIndex = 0;
            txtReportTitle.TabStop = false;
            // 
            // QBEForm
            // 
            ClientSize = new System.Drawing.Size(898, 693);
            Controls.Add(SplitContainer1);
            Controls.Add(NavBar);
            IconSource = "icon-search";
            Name = "QBEForm";
            Text = "QBEForm";
            TabControl.ResumeLayout(false);
            TabPageStampe.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_ReportGrid).EndInit();
            TabPageCriteriRicerca.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_QueryGrid).EndInit();
            TabPageOrdinamento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv_SelectedSortColumns).EndInit();
            TabPageEsporta.ResumeLayout(false);
            PanelEsporta.ResumeLayout(false);
            PanelEsporta.PerformLayout();
            TabPageDebug.ResumeLayout(false);
            TabPageDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_ResultGrid).EndInit();
            SplitContainer1.Panel1.ResumeLayout(false);
            SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer1).EndInit();
            SplitContainer1.ResumeLayout(false);
            PanelReportViewer.ResumeLayout(false);
            PanelReportInfo.ResumeLayout(false);
            PanelReportInfo.PerformLayout();
            FormClosed += new FormClosedEventHandler(QBEForm_FormClosed);
            Accelerator += new AcceleratorEventHandler(QBEForm_Accelerator);
            Load += new EventHandler(QBEForm_Load);
            Resize += new EventHandler(QBEForm_Resize);
            ResumeLayout(false);
            PerformLayout();

        }
        internal ContextMenu ContextMenuRecords;
        internal MenuItem menuRecords100;
        internal MenuItem menuRecords500;
        internal MenuItem menuRecords1000;
        internal MenuItem menuRecords2000;
        internal MenuItem menuRecords5000;
        internal MenuItem menuRecords10000;
        internal MenuItem menuRecordsALL;
        internal SaveFileDialog SaveFileDialog;
        internal ToolBar NavBar;
        internal ToolBarButton bFirst;
        internal ToolBarButton bPrev;
        internal ToolBarButton RecordLabel;
        internal ToolBarButton bNext;
        internal ToolBarButton bLast;
        internal ToolBarButton bRefresh;
        internal ToolBarButton bDelete;
        internal ToolBarButton Records;
        internal ToolBarButton bPrint;
        internal ToolBarButton bSave;
        internal ToolBarButton bSaveQBE;
        internal ToolBarButton bLoadQBE;
        internal ToolBarButton bClose;
        internal TabControl TabControl;
        internal TabPage TabPageCriteriRicerca;
        internal TabPage TabPageEsporta;
        internal TabPage TabPageStampe;
        internal TabPage TabPageDebug;
        private DataGridViewTextBoxColumn dgvcReportFileName;
        private DataGridViewTextBoxColumn dgvcDescrizioneReport;
        private DataGridViewTextBoxColumn dgvcNomeReport;
        private DataGridView _ReportGrid;

        internal virtual DataGridView ReportGrid
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ReportGrid;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ReportGrid != null)
                {
                    _ReportGrid.Click -= ReportGrid_Click;
                }

                _ReportGrid = value;
                if (_ReportGrid != null)
                {
                    _ReportGrid.Click += ReportGrid_Click;
                }
            }
        }
        internal ComboBox cmbRecords;
        internal TextBox txtDebug;
        internal Panel PanelEsporta;
        internal Button btnEsporta;
        internal RadioButton rbXML;
        internal RadioButton rbCSV;
        internal RadioButton rbExcel;
        internal Button Button1;
        private DataGridView _ResultGrid;

        internal virtual DataGridView ResultGrid
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ResultGrid;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ResultGrid != null)
                {
                    _ResultGrid.DoubleClick -= ResultGrid_DoubleClick;
                    _ResultGrid.CellEnter -= ResultGrid_CellEnter;
                    _ResultGrid.KeyDown -= ResultGrid_KeyDown;
                    _ResultGrid.RowEnter -= ResultGrid_RowEnter;
                }

                _ResultGrid = value;
                if (_ResultGrid != null)
                {
                    _ResultGrid.DoubleClick += ResultGrid_DoubleClick;
                    _ResultGrid.CellEnter += ResultGrid_CellEnter;
                    _ResultGrid.KeyDown += ResultGrid_KeyDown;
                    _ResultGrid.RowEnter += ResultGrid_RowEnter;
                }
            }
        }
        internal PdfViewer PdfViewer;
        internal WebBrowser WebBrowser;
        internal AspNetPanel AspNetPanel;
        internal SplitContainer SplitContainer1;
        internal Panel PanelReportInfo;
        internal TextBox txtReportTitle;
        internal TextBox txtReportDescription;
        internal Panel PanelReportViewer;
        internal TabPage TabPageOrdinamento;
        internal Button btnSortRemove;
        internal Button btnSortAdd;
        internal ListBox lstSortColumns;
        private DataGridView _QueryGrid;

        internal virtual DataGridView QueryGrid
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _QueryGrid;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _QueryGrid = value;
            }
        }
        private DataGridViewTextBoxColumn dgvcNomeCampo;
        private DataGridViewTextBoxColumn dgvcValoreCampo;
        private DataGridViewButtonColumn dgvcQueryCampo;
        internal CheckBox chkLikeOperator;
        internal DataGridView dgv_SelectedSortColumns;
        internal DataGridViewTextBoxColumn dgvc_SelectedSortColumns_name;
        internal DataGridViewTextBoxColumn dgvc_SelectedSortColumns_friendlyname;
        internal DataGridViewComboBoxColumn dgvc_SelectedSortColumns_ascdesc;
        internal Button btnSortDown;
        internal Button btnSortUp;
        internal DataGridViewTextBoxColumn dgvc_SelectedSortColumns_position;
        internal Button Button2;
    }
}