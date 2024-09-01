
Imports Dapper
Imports System
Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Text
Imports Wisej.Web
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices

Namespace Passero.Framework.FRReports

    Public Partial Class ReportManager
        Inherits Form
        Private RenderError As String = ""
        Private CurrentReportName As String
        Public RaiseReportEvents As Boolean = False
        Public QBEColumns As QBEColumns = New QBEColumns()

        Public QBEReports As QBEReports = New QBEReports()
        Public Property DefaultReport As QBEFRReport
        Public Property PrintDefaultReport As Boolean = False
        Public Property ReportsPath As String = Application.StartupPath & "reports"
        Public Property ReportsPDFUrlPath As String = "\Reports\"
        Public Property ReportFileName As String = ""
        Public Property ReportName As String = ""
        Public Property SQLQuery As String = ""
        Public Property SQLQueryParameters As DynamicParameters = New DynamicParameters()

        Private FRReport As FRReport = New FRReport()

        Public Event ReportRenderRequest As EventHandler

        Protected Overridable Sub OnReportRenderRequest(e As ReportRenderRequestEventArgs)
            RaiseEvent ReportRenderRequest(Me, e)
        End Sub

        Public Event ReportAfterRender As EventHandler

        Protected Overridable Sub OnReportAfterRender(e As ReportAfterRenderEventArgs)
            RaiseEvent ReportAfterRender(Me, e)
        End Sub



        Public Property UseLikeOperator As Boolean
            Get
                Return chkLikeOperator.Checked
            End Get
            Set(value As Boolean)
                chkLikeOperator.Checked = value
            End Set
        End Property


        Public Property CallBackAction As Action


        Public Property SetFocusControlAfterClose As Control

        Public Sub New()
            InitializeComponent()
            bSaveQBE.Visible = False
            bLoadQBE.Visible = False
            cmbRecords.Visible = False
            RecordLabel.Visible = False
            Records.Visible = False
            bNext.Visible = False
            bPrev.Visible = False
            bFirst.Visible = False
            bLast.Visible = False
            bPrint.Visible = False
            bSave.Visible = False
            TabPageDebug.Hidden = True
            TabPageExport.Hidden = True
            TabPageReportQuery.Hidden = True
            TabPageReports.Hidden = True
            TabPageReportSort.Hidden = True

        End Sub

        Private Sub FRReport_ReportRenderRequest(sender As Object, e As EventArgs)
            If RaiseReportEvents Then
                Dim args = CType(e, ReportRenderRequestEventArgs)
                OnReportRenderRequest(args)
            End If
        End Sub



        Public Sub ExportResultGrid()
            Dim filename = ""
            filename = Path.GetTempPath() & "\" & Guid.NewGuid().ToString()
            Dim exportfilename = FileHelper.GetSafeFileName(Text)
            Dim expofilenameextension = ".xml"
            Dim Stream As MemoryStream = Nothing

            Dim Report = QBEReports(CurrentReportName)
            Dim format = FRRenderFormat.EXCEL


            If rbExcel.Checked Then
                format = FRRenderFormat.EXCEL
                expofilenameextension = ".xls"
            End If

            If rbCSV.Checked Then
                format = FRRenderFormat.CSV
                expofilenameextension = ".csv"
            End If

            If rbJSON.Checked Then

                expofilenameextension = ".json"
            End If
            If rbXML.Checked Then
                format = FRRenderFormat.XML
                expofilenameextension = ".xml"
            End If

            BuildQuery3()
            Dim ReportBytes As Byte() = Nothing ' this.RenderReport(Report,format );


            'switch (SSRSReport.ReportFormat)
            '{
            '    case SSRSRenderFormat.XML:
            '        break;
            '    case SSRSRenderFormat.NULL:
            '        break;
            '    case SSRSRenderFormat.CSV:
            '        break;
            '    case SSRSRenderFormat.IMAGE:
            '        break;
            '    case SSRSRenderFormat.PDF:

            '        //if (this.PdfViewer.Dock != DockStyle.Fill) this.PdfViewer.Dock = DockStyle.Fill;
            '        //this.PdfViewer.PdfStream = new MemoryStream(ReportBytes);
            '        //this.PdfViewer.Visible = true;
            '        break;
            '    case SSRSRenderFormat.HTML40:
            '        break;
            '    case SSRSRenderFormat.HTML32:
            '        break;
            '    case SSRSRenderFormat.MHTML:
            '        break;
            '    case SSRSRenderFormat.EXCEL:
            '        break;
            '    case SSRSRenderFormat.WORD:
            '        break;
            '    default:
            '        break;
            '}



            If ReportBytes IsNot Nothing AndAlso Not Equals(expofilenameextension, "") Then

                Stream = New MemoryStream(ReportBytes)
                Application.Download(Stream, exportfilename & expofilenameextension)
                Stream.Close()
            End If
            'if (System.IO.File.Exists(filename))
            '{
            '    System.IO.File.Delete(filename);
            '}

        End Sub


        'private ModelClass GetEmptyModel()
        '{
        '    return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        '}

        Public Sub ShowQBEReport()
            SetupQBEReport()
            If ReportGrid.CurrentRow IsNot Nothing Then SetupQueryGrid(ReportGrid.CurrentRow(0).Value.ToString().Trim().ToUpper())

            Show()
        End Sub


        Public Sub SetupQBEReport()

            TabPageReportQuery.Hidden = False
            TabPageReports.Hidden = False
            TabPageReportSort.Hidden = False
            TabPageExport.Hidden = False
            TabPageDebug.Hidden = True
            PanelExport.Visible = True
            PanelReportViewer.Visible = True
            PanelReportViewer.Dock = DockStyle.Fill
            QueryGrid.Visible = True
            ReportGrid.Dock = DockStyle.Fill
            ReportGrid.Visible = True

            TabControl.Visible = True
            SetupReportGrid()
            SetupQueryGrid(DefaultReport)


            If Owner Is Nothing AndAlso SetFocusControlAfterClose IsNot Nothing Then Owner = SetFocusControlAfterClose.GetParentOfType(Of Form)()

            If Owner.MdiParent IsNot Nothing Then MdiParent = Owner.MdiParent
        End Sub


        Private Sub SetupReportViewer()


            PanelReportViewer.Dock = DockStyle.Fill
            PanelReportViewer.Visible = True
            ReportGrid.Visible = True


        End Sub


        Private Sub SetupReportGrid()
            ReportGrid.Rows.Clear()
            Dim i = 0
            For Each QBEReport In QBEReports.Values
                ReportGrid.Rows.Add()
                If True Then
                    Dim row = ReportGrid.Rows(i)
                    row.Cells(dgvcReportName.Name).Value = QBEReport.ReportTitle
                    row.Cells(dgvcReportDescription.Name).Value = QBEReport.ReportDescription
                    row.Cells(dgvcReportFileName.Name).Value = QBEReport.ReportFileName
                    row.Tag = QBEReport.ReportFileName
                End If
                i = i + 1
            Next
            ReportGrid.AutoResizeColumn(0)
            If ReportGrid.Rows.Count > 0 Then
                CurrentReportName = ReportGrid.Rows(0)(0).Value.ToString().Trim().ToUpper()
            End If
        End Sub


        Private Sub LoadPDFViewer(ReportName As String)

            ReportName = ReportName.Trim().ToUpper()
            Dim report = QBEReports(ReportName)
            If report Is Nothing Then
                RenderError = "No Report!"
                MessageBox.Show("No Report!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If report.DataSets Is Nothing Then
                RenderError = "No Report DataSets!"
                MessageBox.Show("No Report DataSets!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            txtRenderError.Visible = False

            Dim ReportBytes = RenderReport(report, FRRenderFormat.PDF)

            If ReportBytes IsNot Nothing Then
                'if (this.PdfViewer.Dock != DockStyle.Fill) this.PdfViewer.Dock = DockStyle.Fill;
                PdfViewer.PdfStream = New MemoryStream(ReportBytes)
                PdfViewer.Visible = True
                Utilities.SaveByteArrayToFile(ReportBytes, "c:\REports\report1.pdf")
            Else
                txtRenderError.Text = RenderError
                txtRenderError.Dock = DockStyle.Fill
                txtRenderError.Visible = True
            End If


        End Sub


        Private Function RenderReport(ReportName As String, Optional Format As FRRenderFormat = FRRenderFormat.PDF) As Byte()

            ReportName = ReportName.Trim().ToUpper()
            Dim Report = QBEReports(ReportName)
            If Report Is Nothing Then
                RenderError = "No Report!"
                MessageBox.Show("No Report!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End If

            If Report.DataSets Is Nothing Then
                RenderError = "No Report DataSets!"
                MessageBox.Show("No Report DataSets!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End If

            Return RenderReport(Report, Format)


        End Function


        Private Function RenderReport(Report As QBEFRReport, Optional Format As FRRenderFormat = FRRenderFormat.PDF) As Byte()

            If Report Is Nothing Then
                MessageBox.Show("No Report!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End If

            If Report.DataSets Is Nothing Then
                MessageBox.Show("No Report DataSets!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End If

            'Passero.Framework.Reports.SSRSReport SSRSReport = new Reports.SSRSReport();
            FRReport = New FRReport()
            If RaiseReportEvents Then
                RemoveHandler FRReport.ReportRenderRequest, AddressOf FRReport_ReportRenderRequest
                AddHandler FRReport.ReportRenderRequest, AddressOf FRReport_ReportRenderRequest
            End If
            FRReport.ReportPath = Report.ReportFileName
            FRReport.ReportFormat = Format
            FRReport.DataSets = Report.DataSets



            Dim ReportBytes = FRReport.Render(Format)
            If FRReport.LastExecutionResult.Exception IsNot Nothing Then
                RenderError = FRReport.LastExecutionResult.ResultMessage & vbLf & FRReport.LastExecutionResult.Exception.ToString()
            End If
            Return ReportBytes

        End Function




        Private Sub SetupQueryGrid(ReportName As String)

            Dim report = QBEReports(ReportName)
            SetupQueryGrid(report)
        End Sub


        Private Sub SetupQueryGrid(QBEReport As QBEFRReport)

            If QBEReport.DataSets.Count = 0 Then Return

            Dim PrimaryDataSet = QBEReport.PrimaryDataSet
            If PrimaryDataSet Is Nothing Then
                PrimaryDataSet = QBEReport.DataSets.Values.First()
            End If
            QueryGrid.Rows.Clear()
            lstSortColumns.DataSource = Nothing
            lstSortColumns.Items.Clear()
            QBEReport.SortColumns.Clear()
            For Each QBEColumn In QBEColumns.Values

                If QBEColumn.ReportName.Trim().Equals(ReportGrid.CurrentRow(0).Value.ToString().Trim(), StringComparison.InvariantCultureIgnoreCase) AndAlso QBEColumn.UseInQBE Then
                    Dim i As Integer

                    i = QueryGrid.Rows.Add(QBEColumn.FriendlyName, QBEColumn.QBEValue)
                    ' SortColumns
                    Dim sc As QBEReportSortColumn = New QBEReportSortColumn()
                    sc.Name = QBEColumn.DbColumn
                    sc.FriendlyName = QBEColumn.FriendlyName
                    sc.Position = i
                    sc.AscDesc = "ASC"
                    QBEReport.SortColumns.Add(sc.Name, sc)

                    Dim PropertyType = PrimaryDataSet.ModelProperties(QBEColumn.DbColumn).PropertyType
                    Dim PropertyTypeIs = Utilities.GetSystemTypeIs(PropertyType)
                    If Utilities.GetSystemTypeIs(PropertyType) = EnumSystemTypeIs.Boolean Then
                        If PropertyTypeIs = EnumSystemTypeIs.Boolean Then
                            Dim ncell As DataGridViewCheckBoxCell = New DataGridViewCheckBoxCell()
                            ncell.ThreeState = True
                            ncell.IndeterminateValue = ""
                            ncell.TrueValue = True
                            ncell.FalseValue = False


                            If Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(QBEColumn.QBEValue, Nothing, False)) Then
                                If Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(QBEColumn.QBEValue, "True", False)) Then
                                    ncell.Value = True
                                Else
                                    ncell.Value = False
                                End If
                            Else
                                ncell.Value = ncell.IndeterminateValue
                            End If


                            QueryGrid.Rows(i).Cells(1) = ncell
                            QueryGrid.Rows(i).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                        End If
                    End If
                    QueryGrid.Rows(i).Tag = QBEColumn.DbColumn


                End If
            Next

            txtReportTitle.Text = QBEReport.ReportTitle
            txtReportDescription.Text = QBEReport.ReportDescription

            lstSortColumns.DataSource = QBEReport.SortColumns.Values.ToList()
            lstSortColumns.ValueMember = "Name"
            lstSortColumns.DisplayMember = "FriendlyName"
        End Sub

        Public Sub LoadData()
            DoQuery()
        End Sub

        Public Sub DoQuery()
            BuildQuery3()
            LoadPDFViewer(CurrentReportName)

        End Sub

        Private Sub MovePrevious()




            ' SendKeys.Send("{UP}")

        End Sub
        Private Sub MoveNext()



        End Sub
        Private Async Sub MoveFirst()
            ' await this.PdfViewer.EvalAsync("this.__objectEl.__element.contentWindow.PDFViewerApplication.page=1");



        End Sub
        Private Async Sub MoveLast()
            'await this.PdfViewer.EvalAsync("this.__objectEl.__element.contentWindow.PDFViewerApplication.page=this.__objectEl.__element.contentWindow.PDFViewerApplication.pagesCount");


        End Sub


        Private Sub BuildQuery3()
            If QBEReports(CurrentReportName).DataSets.Count = 0 Then Return

            Dim Report = QBEReports(CurrentReportName)

            Dim PrimaryDataSet = Report.PrimaryDataSet
            If PrimaryDataSet Is Nothing Then
                PrimaryDataSet = Report.DataSets.Values.First()
            End If

            Dim sqlwhere As StringBuilder = New StringBuilder()
            Dim _WhereAND = ""
            QueryGrid.EndEdit()
            Dim parameters As DynamicParameters = New DynamicParameters()
            For Each item In QueryGrid.Rows
                Dim sqlwhereitem As StringBuilder = New StringBuilder()
                Dim Value = ""
                If item(1).Value IsNot Nothing Then Value = item(1).Value.ToString()

                Dim _WhereItemOR = ""
                Dim Values As String()
                Dim PropertyType As Type = PrimaryDataSet.ModelProperties(item.Tag.ToString()).PropertyType
                Dim PropertyTypeIs = Utilities.GetSystemTypeIs(PropertyType)
                If Not String.IsNullOrEmpty(Trim(Value)) Or Not String.IsNullOrEmpty(Value) Then
                    Value = Value.Trim()


                    If Not Equals(Value, ";") Then
                        Values = Split(Value, ";")
                    Else
                        Values = New String(0) {}
                        Values(0) = ";"
                    End If

                    Dim i = 1
                    For Each _Value In Values
                        Dim parametername As String = $"@{item.Tag.ToString()}_{i.ToString().Trim()}"
                        If chkLikeOperator.Checked Then
                            sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()} Like {parametername} ")
                            parameters.Add(parametername, "%" & _Value & "%", Utilities.GetDbType(PropertyType))
                        Else
                            sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()}{GetComparisionOperator(_Value)}{parametername}")
                            parameters.Add(parametername, RemoveComparisionOperator(_Value), Utilities.GetDbType(PropertyType))
                        End If

                        If sqlwhereitem.Length > 0 Then
                            _WhereItemOR = " OR "
                        End If
                        i += 1
                    Next
                    If sqlwhere.Length > 0 Then
                        _WhereAND = " AND "
                    End If
                    sqlwhere.Append($" {_WhereAND} ( {sqlwhereitem.ToString()} )")
                End If
            Next


            Report.SelectedSortColumns.Clear()
            For Each row In dgv_SelectedSortColumns.Rows
                Dim column As QBEReportSortColumn = New QBEReportSortColumn()
                column.Name = CStr(row(dgvc_SelectedSortColumns_name).Value)
                column.Position = CInt(row(dgvc_SelectedSortColumns_position).Value)
                column.FriendlyName = CStr(row(dgvc_SelectedSortColumns_friendlyname).Value)
                column.AscDesc = CStr(row(dgvc_SelectedSortColumns_ascdesc).Value)
                Report.SelectedSortColumns.Add(column.Name, column)
            Next



            SQLQuery = $"SELECT * FROM {DapperHelper.Utilities.GetTableName(PrimaryDataSet.Model)}"

            If Not Equals(sqlwhere.ToString().Trim(), "") Then SQLQuery = SQLQuery & $" WHERE {sqlwhere.ToString()}"


            SQLQuery += " " & Report.OrderBy()
            SQLQueryParameters = parameters
            PrimaryDataSet.Parameters = parameters
            PrimaryDataSet.SQLQuery = SQLQuery
            PrimaryDataSet.LoadData()


            'Load Data for other datasets
            For Each DataSet In Report.DataSets.Values
                If DataSet IsNot PrimaryDataSet Then
                    If Equals(DataSet.SQLQuery, Nothing) OrElse Equals(DataSet.SQLQuery.Trim(), "") Then
                        DataSet.SQLQuery = $"SELECT * FROM {DapperHelper.Utilities.GetTableName(DataSet.Model)}"
                    End If
                    DataSet.LoadData()
                End If
            Next

        End Sub



        Private Function GetComparisionOperator(Value As String) As String

            Value = Value.ToUpper()
            Dim _operator = ""
            If Value.StartsWith("=") Then _operator = " = "
            If Value.StartsWith(">") Then _operator = " > "
            If Value.StartsWith("<") Then _operator = " < "
            If Value.StartsWith(">=") Then _operator = " >= "
            If Value.StartsWith("<=") Then _operator = " <= "
            If Value.StartsWith("<>") Then _operator = " <> "
            If Value.StartsWith("LIKE ", StringComparison.CurrentCultureIgnoreCase) Then _operator = " LIKE "
            If Value.StartsWith("NOT LIKE ", StringComparison.CurrentCultureIgnoreCase) Then _operator = " NOT LIKE "
            If Equals(_operator, "") Then _operator = " = "

            Return _operator
        End Function
        Private Function RemoveComparisionOperator(Value As String) As String
            Dim op As String = GetComparisionOperator(Value).Trim()

            If Value.StartsWith(op, StringComparison.CurrentCultureIgnoreCase) = False Then Value = op & Value
            Return Value.Substring(op.Length).Trim()
        End Function

        Private Sub XQBEForm_Load(sender As Object, e As EventArgs)

        End Sub

        Private Sub bPrev_Click(sender As Object, e As EventArgs)
            MovePrevious()
        End Sub

        Private Sub bFirst_Click(sender As Object, e As EventArgs)
            MoveFirst()
        End Sub

        Private Sub bNext_Click(sender As Object, e As EventArgs)
            MoveNext()
        End Sub

        Private Sub bLast_Click(sender As Object, e As EventArgs)
            MoveLast()
        End Sub

        Private Sub bRefresh_Click(sender As Object, e As EventArgs)
            DoQuery()
        End Sub

        Private Sub bDelete_Click(sender As Object, e As EventArgs)
            ClearFilters()
        End Sub

        Public Sub ClearFilters()
            For Each row In QueryGrid.Rows
                row(1).Value = ""
            Next
        End Sub
        Private Sub bSave_Click(sender As Object, e As EventArgs)

        End Sub




        Private Sub CloseQBEForm()
            If Owner Is Nothing AndAlso SetFocusControlAfterClose IsNot Nothing Then Owner = SetFocusControlAfterClose.GetParentOfType(Of Form)()

            If Owner IsNot Nothing AndAlso CallBackAction IsNot Nothing Then
                Try
                    CallBackAction.Invoke()
                Catch __unusedException1__ As Exception

                End Try
            End If



            If SetFocusControlAfterClose IsNot Nothing AndAlso SetFocusControlAfterClose.Focusable Then
                SetFocusControlAfterClose.Focus()
            End If
            Close()
            Dispose()
        End Sub


        Private Sub CopyDataRow(oSourceRow As DataRow, oTargetRow As DataRow)
            Dim nIndex = 0
            ' - Copy all the fields from the source row to the target row
            For Each oItem In oSourceRow.ItemArray
                oTargetRow(nIndex) = oItem
                nIndex += 1
            Next
        End Sub

        Private Sub XQBEReport_Shown(sender As Object, e As EventArgs)

            Show()
            Focus()

        End Sub

        Private Sub XQBEReport_FormClosed(sender As Object, e As FormClosedEventArgs)

            QBEReports = Nothing
            DefaultReport = Nothing
            Dispose()
            GC.Collect()
        End Sub

        Private Sub btnExport_Click(sender As Object, e As EventArgs)
            ExportResultGrid()
        End Sub

        Private Sub ReportGrid_Click(sender As Object, e As EventArgs)
            If ReportGrid.CurrentRow IsNot Nothing Then

                If Not Equals(CurrentReportName, ReportGrid.CurrentRow(0).Value.ToString().Trim().ToUpper()) Then
                    PdfViewer.PdfStream = Nothing
                    PdfViewer.Visible = False

                End If
                CurrentReportName = ReportGrid.CurrentRow(0).Value.ToString().Trim().ToUpper()
                txtReportTitle.Text = ReportGrid.CurrentRow(0).Value.ToString().Trim()
                txtReportDescription.Text = ReportGrid.CurrentRow(1).Value.ToString().Trim()


            End If

            SetupQueryGrid(CurrentReportName)
        End Sub

        Private Sub bPrint_Click(sender As Object, e As EventArgs)
            DoQuery()



        End Sub

        Private Sub btnSortAdd_Click(sender As Object, e As EventArgs)

            If lstSortColumns.SelectedItem Is Nothing Then
                Return
            End If

            Dim Report = QBEReports(CurrentReportName)
            Dim SortColumn = CType(lstSortColumns.SelectedItem, QBEReportSortColumn)
            If Report.SelectedSortColumns.ContainsKey(SortColumn.Name) = False Then
                Report.SelectedSortColumns.Add(SortColumn.Name, SortColumn)
            End If
            lstSortColumns.SelectedItem = Nothing
            dgv_SelectedSortColumns.Rows.Clear()
            For Each column In Report.SelectedSortColumns.Values
                dgv_SelectedSortColumns.Rows.Add(column.Position, column.Name, column.FriendlyName, column.AscDesc)
            Next

        End Sub

        Private Sub btnSortRemove_Click(sender As Object, e As EventArgs)

            If dgv_SelectedSortColumns.CurrentRow Is Nothing Then
                Return
            End If
            Dim Report = QBEReports(CurrentReportName)
            Report.SelectedSortColumns.Remove(dgv_SelectedSortColumns.CurrentRow.Cells(dgvc_SelectedSortColumns_name).Value.ToString())
            dgv_SelectedSortColumns.Rows.Clear()
            dgv_SelectedSortColumns.Rows.Clear()
            For Each column In Report.SelectedSortColumns.Values
                dgv_SelectedSortColumns.Rows.Add(column.Position, column.Name, column.FriendlyName, column.AscDesc)
            Next
        End Sub

        Private Sub btnSortUp_Click(sender As Object, e As EventArgs)
            ControlsUtilities.DataGridRowMoveUp(dgv_SelectedSortColumns, dgvc_SelectedSortColumns_position)
            dgv_SelectedSortColumns.EndEdit()
        End Sub

        Private Sub btnSortDown_Click(sender As Object, e As EventArgs)
            ControlsUtilities.DataGridRowMoveDown(dgv_SelectedSortColumns, dgvc_SelectedSortColumns_position)
            dgv_SelectedSortColumns.EndEdit()
        End Sub

        Private Sub PanelReportViewer_Resize(sender As Object, e As EventArgs)
            PdfViewer.Top = PanelReportInfo.Height
            PdfViewer.Left = 0
            PdfViewer.Width = PanelReportViewer.Width
            PdfViewer.Height = PanelReportViewer.Height - PanelReportInfo.Top - 35
            txtRenderError.Top = PdfViewer.Top
            txtRenderError.Left = 0
            txtRenderError.Width = PanelReportViewer.Width
            txtRenderError.Height = PdfViewer.Height


        End Sub

        Private Sub bClose_Click(sender As Object, e As EventArgs)
            Close()
        End Sub
    End Class
End Namespace
