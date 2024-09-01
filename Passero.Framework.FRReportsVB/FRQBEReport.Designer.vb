Namespace Passero.Framework.FRReports
    Partial Class ReportManager
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <paramname="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Wisej.NET Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(ReportManager))
            Dim dataGridViewCellStyle1 As Wisej.Web.DataGridViewCellStyle = New Wisej.Web.DataGridViewCellStyle()
            Dim dataGridViewCellStyle2 As Wisej.Web.DataGridViewCellStyle = New Wisej.Web.DataGridViewCellStyle()
            Dim dataGridViewCellStyle3 As Wisej.Web.DataGridViewCellStyle = New Wisej.Web.DataGridViewCellStyle()
            SplitContainer = New Wisej.Web.SplitContainer()
            PanelReportViewer = New Wisej.Web.Panel()
            txtRenderError = New Wisej.Web.TextBox()
            AspNetPanel = New Wisej.Web.AspNetPanel()
            WebBrowser = New Wisej.Web.WebBrowser()
            PdfViewer = New Wisej.Web.PdfViewer()
            TabControl = New Wisej.Web.TabControl()
            TabPageReports = New Wisej.Web.TabPage()
            ReportGrid = New Wisej.Web.DataGridView()
            dgvcReportName = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvcReportDescription = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvcReportFileName = New Wisej.Web.DataGridViewTextBoxColumn()
            TabPageReportQuery = New Wisej.Web.TabPage()
            QueryGrid = New Wisej.Web.DataGridView()
            dgvcNomeCampo = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvcValoreCampo = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvcQueryColumn = New Wisej.Web.DataGridViewButtonColumn()
            chkLikeOperator = New Wisej.Web.CheckBox()
            TabPageReportSort = New Wisej.Web.TabPage()
            btnSortDown = New Wisej.Web.Button()
            btnSortUp = New Wisej.Web.Button()
            dgv_SelectedSortColumns = New Wisej.Web.DataGridView()
            dgvc_SelectedSortColumns_position = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvc_SelectedSortColumns_name = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvc_SelectedSortColumns_friendlyname = New Wisej.Web.DataGridViewTextBoxColumn()
            dgvc_SelectedSortColumns_ascdesc = New Wisej.Web.DataGridViewComboBoxColumn()
            btnSortRemove = New Wisej.Web.Button()
            btnSortAdd = New Wisej.Web.Button()
            lstSortColumns = New Wisej.Web.ListBox()
            TabPageExport = New Wisej.Web.TabPage()
            PanelExport = New Wisej.Web.Panel()
            rbJSON = New Wisej.Web.RadioButton()
            btnExport = New Wisej.Web.Button()
            rbXML = New Wisej.Web.RadioButton()
            rbCSV = New Wisej.Web.RadioButton()
            rbExcel = New Wisej.Web.RadioButton()
            TabPageDebug = New Wisej.Web.TabPage()
            Button2 = New Wisej.Web.Button()
            txtDebug = New Wisej.Web.TextBox()
            Button1 = New Wisej.Web.Button()
            cmbRecords = New Wisej.Web.ComboBox()
            NavBar = New Wisej.Web.ToolBar()
            bFirst = New Wisej.Web.ToolBarButton()
            bPrev = New Wisej.Web.ToolBarButton()
            RecordLabel = New Wisej.Web.ToolBarButton()
            bNext = New Wisej.Web.ToolBarButton()
            bLast = New Wisej.Web.ToolBarButton()
            bRefresh = New Wisej.Web.ToolBarButton()
            bDelete = New Wisej.Web.ToolBarButton()
            Records = New Wisej.Web.ToolBarButton()
            bPrint = New Wisej.Web.ToolBarButton()
            bSave = New Wisej.Web.ToolBarButton()
            bSaveQBE = New Wisej.Web.ToolBarButton()
            bLoadQBE = New Wisej.Web.ToolBarButton()
            bClose = New Wisej.Web.ToolBarButton()
            pbEngineLogo = New Wisej.Web.PictureBox()
            PanelReportInfo = New Wisej.Web.FlowLayoutPanel()
            txtReportTitle = New Wisej.Web.TextBox()
            txtReportDescription = New Wisej.Web.TextBox()
            CType(SplitContainer, ComponentModel.ISupportInitialize).BeginInit()
            SplitContainer.Panel1.SuspendLayout()
            SplitContainer.Panel2.SuspendLayout()
            SplitContainer.SuspendLayout()
            PanelReportViewer.SuspendLayout()
            TabControl.SuspendLayout()
            TabPageReports.SuspendLayout()
            CType(ReportGrid, ComponentModel.ISupportInitialize).BeginInit()
            TabPageReportQuery.SuspendLayout()
            CType(QueryGrid, ComponentModel.ISupportInitialize).BeginInit()
            TabPageReportSort.SuspendLayout()
            CType(dgv_SelectedSortColumns, ComponentModel.ISupportInitialize).BeginInit()
            TabPageExport.SuspendLayout()
            PanelExport.SuspendLayout()
            TabPageDebug.SuspendLayout()
            CType(pbEngineLogo, ComponentModel.ISupportInitialize).BeginInit()
            PanelReportInfo.SuspendLayout()
            SuspendLayout()
            ' 
            ' SplitContainer
            ' 
            resources.ApplyResources(SplitContainer, "SplitContainer")
            SplitContainer.BackColor = Drawing.SystemColors.Window
            SplitContainer.ForeColor = Drawing.SystemColors.WindowText
            SplitContainer.Name = "SplitContainer"
            ' 
            ' SplitContainer.Panel1
            ' 
            SplitContainer.Panel1.Controls.Add(PanelReportViewer)
            ' 
            ' SplitContainer.Panel2
            ' 
            SplitContainer.Panel2.Controls.Add(TabControl)
            ' 
            ' PanelReportViewer
            ' 
            PanelReportViewer.Controls.Add(PanelReportInfo)
            PanelReportViewer.Controls.Add(txtRenderError)
            PanelReportViewer.Controls.Add(AspNetPanel)
            PanelReportViewer.Controls.Add(WebBrowser)
            PanelReportViewer.Controls.Add(PdfViewer)
            resources.ApplyResources(PanelReportViewer, "PanelReportViewer")
            PanelReportViewer.Name = "PanelReportViewer"
            AddHandler PanelReportViewer.Resize, New EventHandler(AddressOf PanelReportViewer_Resize)
            ' 
            ' txtRenderError
            ' 
            resources.ApplyResources(txtRenderError, "txtRenderError")
            txtRenderError.Name = "txtRenderError"
            ' 
            ' AspNetPanel
            ' 
            resources.ApplyResources(AspNetPanel, "AspNetPanel")
            AspNetPanel.Name = "AspNetPanel"
            ' 
            ' WebBrowser
            ' 
            resources.ApplyResources(WebBrowser, "WebBrowser")
            WebBrowser.Name = "WebBrowser"
            WebBrowser.Url = New Uri("~/BasicDalWisejCRViewer.aspx", UriKind.Relative)
            ' 
            ' PdfViewer
            ' 
            resources.ApplyResources(PdfViewer, "PdfViewer")
            PdfViewer.Name = "PdfViewer"
            ' 
            ' TabControl
            ' 
            resources.ApplyResources(TabControl, "TabControl")
            TabControl.Controls.Add(TabPageReports)
            TabControl.Controls.Add(TabPageReportQuery)
            TabControl.Controls.Add(TabPageReportSort)
            TabControl.Controls.Add(TabPageExport)
            TabControl.Controls.Add(TabPageDebug)
            TabControl.Name = "TabControl"
            TabControl.PageInsets = New Wisej.Web.Padding(0, 39, 0, 0)
            ' 
            ' TabPageReports
            ' 
            TabPageReports.Controls.Add(ReportGrid)
            resources.ApplyResources(TabPageReports, "TabPageReports")
            TabPageReports.Name = "TabPageReports"
            ' 
            ' ReportGrid
            ' 
            dataGridViewCellStyle1.BackColor = Drawing.Color.FromName("@buttonFace")
            ReportGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1
            ReportGrid.Columns.AddRange(New Wisej.Web.DataGridViewColumn() {dgvcReportName, dgvcReportDescription, dgvcReportFileName})
            resources.ApplyResources(ReportGrid, "ReportGrid")
            ReportGrid.Name = "ReportGrid"
            ReportGrid.ReadOnly = True
            ReportGrid.RowTemplate.ReadOnly = True
            AddHandler ReportGrid.Click, New EventHandler(AddressOf ReportGrid_Click)
            ' 
            ' dgvcReportName
            ' 
            dgvcReportName.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells
            dataGridViewCellStyle2.Font = New Drawing.Font("default", 13F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Pixel)
            dgvcReportName.DefaultCellStyle = dataGridViewCellStyle2
            resources.ApplyResources(dgvcReportName, "dgvcReportName")
            dgvcReportName.Name = "dgvcReportName"
            ' 
            ' dgvcReportDescription
            ' 
            dgvcReportDescription.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill
            resources.ApplyResources(dgvcReportDescription, "dgvcReportDescription")
            dgvcReportDescription.Name = "dgvcReportDescription"
            ' 
            ' dgvcReportFileName
            ' 
            dgvcReportFileName.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells
            resources.ApplyResources(dgvcReportFileName, "dgvcReportFileName")
            dgvcReportFileName.Name = "dgvcReportFileName"
            ' 
            ' TabPageReportQuery
            ' 
            TabPageReportQuery.Controls.Add(QueryGrid)
            TabPageReportQuery.Controls.Add(chkLikeOperator)
            resources.ApplyResources(TabPageReportQuery, "TabPageReportQuery")
            TabPageReportQuery.Name = "TabPageReportQuery"
            ' 
            ' QueryGrid
            ' 
            QueryGrid.AllowSortingDataSource = False
            QueryGrid.AllowUserToResizeColumns = False
            QueryGrid.AllowUserToResizeRows = False
            resources.ApplyResources(QueryGrid, "QueryGrid")
            QueryGrid.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells
            QueryGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells
            dataGridViewCellStyle3.BackColor = Drawing.Color.FromName("@buttonFace")
            QueryGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3
            QueryGrid.Columns.AddRange(New Wisej.Web.DataGridViewColumn() {dgvcNomeCampo, dgvcValoreCampo, dgvcQueryColumn})
            QueryGrid.DefaultSortMode = Wisej.Web.DataGridViewColumnsSortMode.NotSortable
            QueryGrid.Name = "QueryGrid"
            ' 
            ' dgvcNomeCampo
            ' 
            dgvcNomeCampo.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells
            resources.ApplyResources(dgvcNomeCampo, "dgvcNomeCampo")
            dgvcNomeCampo.MaxInputLength = 50
            dgvcNomeCampo.Name = "dgvcNomeCampo"
            dgvcNomeCampo.ReadOnly = True
            ' 
            ' dgvcValoreCampo
            ' 
            dgvcValoreCampo.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill
            resources.ApplyResources(dgvcValoreCampo, "dgvcValoreCampo")
            dgvcValoreCampo.MaxInputLength = 1024
            dgvcValoreCampo.Name = "dgvcValoreCampo"
            ' 
            ' dgvcQueryColumn
            ' 
            resources.ApplyResources(dgvcQueryColumn, "dgvcQueryColumn")
            dgvcQueryColumn.Name = "dgvcQueryColumn"
            dgvcQueryColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable
            ' 
            ' chkLikeOperator
            ' 
            resources.ApplyResources(chkLikeOperator, "chkLikeOperator")
            chkLikeOperator.CheckState = Wisej.Web.CheckState.Checked
            chkLikeOperator.Name = "chkLikeOperator"
            ' 
            ' TabPageReportSort
            ' 
            TabPageReportSort.Controls.Add(btnSortDown)
            TabPageReportSort.Controls.Add(btnSortUp)
            TabPageReportSort.Controls.Add(dgv_SelectedSortColumns)
            TabPageReportSort.Controls.Add(btnSortRemove)
            TabPageReportSort.Controls.Add(btnSortAdd)
            TabPageReportSort.Controls.Add(lstSortColumns)
            resources.ApplyResources(TabPageReportSort, "TabPageReportSort")
            TabPageReportSort.Name = "TabPageReportSort"
            ' 
            ' btnSortDown
            ' 
            resources.ApplyResources(btnSortDown, "btnSortDown")
            btnSortDown.Name = "btnSortDown"
            AddHandler btnSortDown.Click, New EventHandler(AddressOf btnSortDown_Click)
            ' 
            ' btnSortUp
            ' 
            resources.ApplyResources(btnSortUp, "btnSortUp")
            btnSortUp.Name = "btnSortUp"
            AddHandler btnSortUp.Click, New EventHandler(AddressOf btnSortUp_Click)
            ' 
            ' dgv_SelectedSortColumns
            ' 
            resources.ApplyResources(dgv_SelectedSortColumns, "dgv_SelectedSortColumns")
            dgv_SelectedSortColumns.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells
            dgv_SelectedSortColumns.CellBorderStyle = Wisej.Web.DataGridViewCellBorderStyle.None
            dgv_SelectedSortColumns.ColumnHeadersVisible = False
            dgv_SelectedSortColumns.Columns.AddRange(New Wisej.Web.DataGridViewColumn() {dgvc_SelectedSortColumns_position, dgvc_SelectedSortColumns_name, dgvc_SelectedSortColumns_friendlyname, dgvc_SelectedSortColumns_ascdesc})
            dgv_SelectedSortColumns.DefaultRowHeight = 24
            dgv_SelectedSortColumns.Name = "dgv_SelectedSortColumns"
            dgv_SelectedSortColumns.ScrollBars = Wisej.Web.ScrollBars.Vertical
            dgv_SelectedSortColumns.Selectable = True
            ' 
            ' dgvc_SelectedSortColumns_position
            ' 
            dgvc_SelectedSortColumns_position.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.AllCells
            dgvc_SelectedSortColumns_position.DataPropertyName = "position"
            resources.ApplyResources(dgvc_SelectedSortColumns_position, "dgvc_SelectedSortColumns_position")
            dgvc_SelectedSortColumns_position.Name = "dgvc_SelectedSortColumns_position"
            ' 
            ' dgvc_SelectedSortColumns_name
            ' 
            dgvc_SelectedSortColumns_name.DataPropertyName = "Name"
            resources.ApplyResources(dgvc_SelectedSortColumns_name, "dgvc_SelectedSortColumns_name")
            dgvc_SelectedSortColumns_name.Name = "dgvc_SelectedSortColumns_name"
            ' 
            ' dgvc_SelectedSortColumns_friendlyname
            ' 
            dgvc_SelectedSortColumns_friendlyname.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill
            dgvc_SelectedSortColumns_friendlyname.DataPropertyName = "FriendlyName"
            resources.ApplyResources(dgvc_SelectedSortColumns_friendlyname, "dgvc_SelectedSortColumns_friendlyname")
            dgvc_SelectedSortColumns_friendlyname.Name = "dgvc_SelectedSortColumns_friendlyname"
            dgvc_SelectedSortColumns_friendlyname.ReadOnly = True
            ' 
            ' dgvc_SelectedSortColumns_ascdesc
            ' 
            dgvc_SelectedSortColumns_ascdesc.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None
            dgvc_SelectedSortColumns_ascdesc.DataPropertyName = "AscDesc"
            dgvc_SelectedSortColumns_ascdesc.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList
            resources.ApplyResources(dgvc_SelectedSortColumns_ascdesc, "dgvc_SelectedSortColumns_ascdesc")
            dgvc_SelectedSortColumns_ascdesc.Items.AddRange(New Object() {"ASC", "DESC"})
            dgvc_SelectedSortColumns_ascdesc.Name = "dgvc_SelectedSortColumns_ascdesc"
            dgvc_SelectedSortColumns_ascdesc.ValueType = GetType(Object)
            ' 
            ' btnSortRemove
            ' 
            resources.ApplyResources(btnSortRemove, "btnSortRemove")
            btnSortRemove.Name = "btnSortRemove"
            AddHandler btnSortRemove.Click, New EventHandler(AddressOf btnSortRemove_Click)
            ' 
            ' btnSortAdd
            ' 
            resources.ApplyResources(btnSortAdd, "btnSortAdd")
            btnSortAdd.Name = "btnSortAdd"
            AddHandler btnSortAdd.Click, New EventHandler(AddressOf btnSortAdd_Click)
            ' 
            ' lstSortColumns
            ' 
            resources.ApplyResources(lstSortColumns, "lstSortColumns")
            lstSortColumns.Label.Font = New Drawing.Font("default", 10F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Pixel)
            lstSortColumns.Name = "lstSortColumns"
            ' 
            ' TabPageExport
            ' 
            TabPageExport.Controls.Add(PanelExport)
            resources.ApplyResources(TabPageExport, "TabPageExport")
            TabPageExport.Name = "TabPageExport"
            ' 
            ' PanelExport
            ' 
            PanelExport.Controls.Add(rbJSON)
            PanelExport.Controls.Add(btnExport)
            PanelExport.Controls.Add(rbXML)
            PanelExport.Controls.Add(rbCSV)
            PanelExport.Controls.Add(rbExcel)
            resources.ApplyResources(PanelExport, "PanelExport")
            PanelExport.Name = "PanelExport"
            PanelExport.TabStop = True
            ' 
            ' rbJSON
            ' 
            resources.ApplyResources(rbJSON, "rbJSON")
            rbJSON.Name = "rbJSON"
            ' 
            ' btnExport
            ' 
            resources.ApplyResources(btnExport, "btnExport")
            btnExport.Name = "btnExport"
            AddHandler btnExport.Click, New EventHandler(AddressOf btnExport_Click)
            ' 
            ' rbXML
            ' 
            resources.ApplyResources(rbXML, "rbXML")
            rbXML.Name = "rbXML"
            ' 
            ' rbCSV
            ' 
            resources.ApplyResources(rbCSV, "rbCSV")
            rbCSV.Name = "rbCSV"
            ' 
            ' rbExcel
            ' 
            rbExcel.Checked = True
            resources.ApplyResources(rbExcel, "rbExcel")
            rbExcel.Name = "rbExcel"
            rbExcel.TabStop = True
            ' 
            ' TabPageDebug
            ' 
            TabPageDebug.Controls.Add(Button2)
            TabPageDebug.Controls.Add(txtDebug)
            TabPageDebug.Controls.Add(Button1)
            TabPageDebug.Controls.Add(cmbRecords)
            resources.ApplyResources(TabPageDebug, "TabPageDebug")
            TabPageDebug.Name = "TabPageDebug"
            ' 
            ' Button2
            ' 
            resources.ApplyResources(Button2, "Button2")
            Button2.Name = "Button2"
            ' 
            ' txtDebug
            ' 
            resources.ApplyResources(txtDebug, "txtDebug")
            txtDebug.Name = "txtDebug"
            ' 
            ' Button1
            ' 
            resources.ApplyResources(Button1, "Button1")
            Button1.Name = "Button1"
            ' 
            ' cmbRecords
            ' 
            cmbRecords.Items.AddRange(New Object() {resources.GetString("cmbRecords.Items"), resources.GetString("cmbRecords.Items1"), resources.GetString("cmbRecords.Items2"), resources.GetString("cmbRecords.Items3"), resources.GetString("cmbRecords.Items4"), resources.GetString("cmbRecords.Items5")})
            resources.ApplyResources(cmbRecords, "cmbRecords")
            cmbRecords.Name = "cmbRecords"
            ' 
            ' NavBar
            ' 
            NavBar.BorderStyle = Wisej.Web.BorderStyle.Solid
            NavBar.Buttons.AddRange(New Wisej.Web.ToolBarButton() {bFirst, bPrev, RecordLabel, bNext, bLast, bRefresh, bDelete, Records, bPrint, bSave, bSaveQBE, bLoadQBE, bClose})
            resources.ApplyResources(NavBar, "NavBar")
            NavBar.Font = New Drawing.Font("default", 10F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Pixel)
            NavBar.Name = "NavBar"
            NavBar.TabStop = False
            ' 
            ' bFirst
            ' 
            bFirst.AllowHtml = True
            resources.ApplyResources(bFirst, "bFirst")
            bFirst.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bFirst.Name = "bFirst"
            AddHandler bFirst.Click, New EventHandler(AddressOf bFirst_Click)
            ' 
            ' bPrev
            ' 
            bPrev.AllowHtml = True
            resources.ApplyResources(bPrev, "bPrev")
            bPrev.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bPrev.Name = "bPrev"
            AddHandler bPrev.Click, New EventHandler(AddressOf bPrev_Click)
            ' 
            ' RecordLabel
            ' 
            RecordLabel.AllowHtml = True
            resources.ApplyResources(RecordLabel, "RecordLabel")
            RecordLabel.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            RecordLabel.Name = "RecordLabel"
            ' 
            ' bNext
            ' 
            bNext.AllowHtml = True
            resources.ApplyResources(bNext, "bNext")
            bNext.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bNext.Name = "bNext"
            AddHandler bNext.Click, New EventHandler(AddressOf bNext_Click)
            ' 
            ' bLast
            ' 
            bLast.AllowHtml = True
            resources.ApplyResources(bLast, "bLast")
            bLast.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bLast.Name = "bLast"
            AddHandler bLast.Click, New EventHandler(AddressOf bLast_Click)
            ' 
            ' bRefresh
            ' 
            bRefresh.AllowHtml = True
            resources.ApplyResources(bRefresh, "bRefresh")
            bRefresh.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bRefresh.Name = "bRefresh"
            AddHandler bRefresh.Click, New EventHandler(AddressOf bRefresh_Click)
            ' 
            ' bDelete
            ' 
            bDelete.AllowHtml = True
            resources.ApplyResources(bDelete, "bDelete")
            bDelete.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bDelete.Name = "bDelete"
            AddHandler bDelete.Click, New EventHandler(AddressOf bDelete_Click)
            ' 
            ' Records
            ' 
            Records.AllowHtml = True
            Records.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            Records.Name = "Records"
            Records.Style = Wisej.Web.ToolBarButtonStyle.DropDownButton
            resources.ApplyResources(Records, "Records")
            ' 
            ' bPrint
            ' 
            bPrint.AllowHtml = True
            resources.ApplyResources(bPrint, "bPrint")
            bPrint.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bPrint.Name = "bPrint"
            AddHandler bPrint.Click, New EventHandler(AddressOf bPrint_Click)
            ' 
            ' bSave
            ' 
            bSave.AllowHtml = True
            resources.ApplyResources(bSave, "bSave")
            bSave.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bSave.Name = "bSave"
            AddHandler bSave.Click, New EventHandler(AddressOf bSave_Click)
            ' 
            ' bSaveQBE
            ' 
            bSaveQBE.AllowHtml = True
            resources.ApplyResources(bSaveQBE, "bSaveQBE")
            bSaveQBE.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bSaveQBE.Name = "bSaveQBE"
            ' 
            ' bLoadQBE
            ' 
            bLoadQBE.AllowHtml = True
            resources.ApplyResources(bLoadQBE, "bLoadQBE")
            bLoadQBE.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bLoadQBE.Name = "bLoadQBE"
            ' 
            ' bClose
            ' 
            bClose.AllowHtml = True
            resources.ApplyResources(bClose, "bClose")
            bClose.Margin = New Wisej.Web.Padding(0, -5, 0, 0)
            bClose.Name = "bClose"
            AddHandler bClose.Click, New EventHandler(AddressOf bClose_Click)
            ' 
            ' pbEngineLogo
            ' 
            resources.ApplyResources(pbEngineLogo, "pbEngineLogo")
            pbEngineLogo.Name = "pbEngineLogo"
            ' 
            ' PanelReportInfo
            ' 
            resources.ApplyResources(PanelReportInfo, "PanelReportInfo")
            PanelReportInfo.BackColor = Drawing.SystemColors.MenuBar
            PanelReportInfo.Controls.Add(txtReportTitle)
            PanelReportInfo.Controls.Add(txtReportDescription)
            PanelReportInfo.Name = "PanelReportInfo"
            ' 
            ' txtReportTitle
            ' 
            txtReportTitle.Focusable = False
            txtReportTitle.Font = New Drawing.Font("Microsoft Sans Serif", 13F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Pixel)
            txtReportTitle.Label.Font = New Drawing.Font("default", 13F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Pixel)
            txtReportTitle.Label.Position = Wisej.Web.LabelPosition.Left
            resources.ApplyResources(txtReportTitle, "txtReportTitle")
            txtReportTitle.Name = "txtReportTitle"
            txtReportTitle.ReadOnly = True
            txtReportTitle.TabStop = False
            ' 
            ' txtReportDescription
            ' 
            txtReportDescription.Focusable = False
            txtReportDescription.Font = New Drawing.Font("Microsoft Sans Serif", 13F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Pixel)
            txtReportDescription.Label.Font = New Drawing.Font("default", 13F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Pixel)
            txtReportDescription.Label.Position = Wisej.Web.LabelPosition.Left
            resources.ApplyResources(txtReportDescription, "txtReportDescription")
            txtReportDescription.Name = "txtReportDescription"
            txtReportDescription.ReadOnly = True
            txtReportDescription.TabStop = False
            ' 
            ' ReportManager
            ' 
            resources.ApplyResources(Me, "$this")
            AutoScaleMode = Wisej.Web.AutoScaleMode.Font
            BackColor = Drawing.Color.FromName("@toolbar")
            Controls.Add(pbEngineLogo)
            Controls.Add(NavBar)
            Controls.Add(SplitContainer)
            Name = "ReportManager"
            ShowModalMask = True
            AddHandler Load, New EventHandler(AddressOf XQBEForm_Load)
            AddHandler Shown, New EventHandler(AddressOf XQBEReport_Shown)
            AddHandler FormClosed, New Wisej.Web.FormClosedEventHandler(AddressOf XQBEReport_FormClosed)
            SplitContainer.Panel1.ResumeLayout(False)
            SplitContainer.Panel2.ResumeLayout(False)
            CType(SplitContainer, ComponentModel.ISupportInitialize).EndInit()
            SplitContainer.ResumeLayout(False)
            PanelReportViewer.ResumeLayout(False)
            PanelReportViewer.PerformLayout()
            TabControl.ResumeLayout(False)
            TabPageReports.ResumeLayout(False)
            CType(ReportGrid, ComponentModel.ISupportInitialize).EndInit()
            TabPageReportQuery.ResumeLayout(False)
            CType(QueryGrid, ComponentModel.ISupportInitialize).EndInit()
            TabPageReportSort.ResumeLayout(False)
            CType(dgv_SelectedSortColumns, ComponentModel.ISupportInitialize).EndInit()
            TabPageExport.ResumeLayout(False)
            PanelExport.ResumeLayout(False)
            PanelExport.PerformLayout()
            TabPageDebug.ResumeLayout(False)
            TabPageDebug.PerformLayout()
            CType(pbEngineLogo, ComponentModel.ISupportInitialize).EndInit()
            PanelReportInfo.ResumeLayout(False)
            PanelReportInfo.PerformLayout()
            ResumeLayout(False)
            PerformLayout()

        End Sub

#End Region

        Friend SplitContainer As Wisej.Web.SplitContainer
        Friend PanelReportViewer As Wisej.Web.Panel
        Friend AspNetPanel As Wisej.Web.AspNetPanel
        Friend WebBrowser As Wisej.Web.WebBrowser
        Friend PdfViewer As Wisej.Web.PdfViewer
        Friend TabControl As Wisej.Web.TabControl
        Friend TabPageReports As Wisej.Web.TabPage
        Private ReportGrid As Wisej.Web.DataGridView
        Private dgvcReportName As Wisej.Web.DataGridViewTextBoxColumn
        Private dgvcReportDescription As Wisej.Web.DataGridViewTextBoxColumn
        Private dgvcReportFileName As Wisej.Web.DataGridViewTextBoxColumn
        Friend TabPageReportQuery As Wisej.Web.TabPage
        Private QueryGrid As Wisej.Web.DataGridView
        Private dgvcNomeCampo As Wisej.Web.DataGridViewTextBoxColumn
        Private dgvcValoreCampo As Wisej.Web.DataGridViewTextBoxColumn
        Private dgvcQueryColumn As Wisej.Web.DataGridViewButtonColumn
        Friend chkLikeOperator As Wisej.Web.CheckBox
        Friend TabPageReportSort As Wisej.Web.TabPage
        Friend btnSortDown As Wisej.Web.Button
        Friend btnSortUp As Wisej.Web.Button
        Friend dgv_SelectedSortColumns As Wisej.Web.DataGridView
        Friend dgvc_SelectedSortColumns_position As Wisej.Web.DataGridViewTextBoxColumn
        Friend dgvc_SelectedSortColumns_name As Wisej.Web.DataGridViewTextBoxColumn
        Friend dgvc_SelectedSortColumns_friendlyname As Wisej.Web.DataGridViewTextBoxColumn
        Friend dgvc_SelectedSortColumns_ascdesc As Wisej.Web.DataGridViewComboBoxColumn
        Friend btnSortRemove As Wisej.Web.Button
        Friend btnSortAdd As Wisej.Web.Button
        Friend lstSortColumns As Wisej.Web.ListBox
        Friend TabPageExport As Wisej.Web.TabPage
        Friend PanelExport As Wisej.Web.Panel
        Friend btnExport As Wisej.Web.Button
        Friend rbXML As Wisej.Web.RadioButton
        Friend rbCSV As Wisej.Web.RadioButton
        Friend rbExcel As Wisej.Web.RadioButton
        Friend TabPageDebug As Wisej.Web.TabPage
        Friend Button2 As Wisej.Web.Button
        Friend txtDebug As Wisej.Web.TextBox
        Friend Button1 As Wisej.Web.Button
        Friend cmbRecords As Wisej.Web.ComboBox
        Friend NavBar As Wisej.Web.ToolBar
        Friend bFirst As Wisej.Web.ToolBarButton
        Friend bPrev As Wisej.Web.ToolBarButton
        Friend RecordLabel As Wisej.Web.ToolBarButton
        Friend bNext As Wisej.Web.ToolBarButton
        Friend bLast As Wisej.Web.ToolBarButton
        Friend bRefresh As Wisej.Web.ToolBarButton
        Friend bDelete As Wisej.Web.ToolBarButton
        Friend Records As Wisej.Web.ToolBarButton
        Friend bPrint As Wisej.Web.ToolBarButton
        Friend bSave As Wisej.Web.ToolBarButton
        Friend bSaveQBE As Wisej.Web.ToolBarButton
        Friend bLoadQBE As Wisej.Web.ToolBarButton
        Friend bClose As Wisej.Web.ToolBarButton
        Friend rbJSON As Wisej.Web.RadioButton
        Private txtRenderError As Wisej.Web.TextBox
        Private pbEngineLogo As Wisej.Web.PictureBox
        Private PanelReportInfo As Wisej.Web.FlowLayoutPanel
        Friend txtReportTitle As Wisej.Web.TextBox
        Friend txtReportDescription As Wisej.Web.TextBox
    End Class
End Namespace
