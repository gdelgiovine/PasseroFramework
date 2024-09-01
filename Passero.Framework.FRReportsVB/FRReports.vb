Imports Dapper
Imports FastReport
Imports FastReport.Data
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO

Namespace Passero.Framework.FRReports

    <Serializable>
    Public Enum QBEColumnsTypes
        CheckBox = 0
        ComboBox = 1
        Image = 2
        Link = 3
        TextBox = 4


    End Enum


    Public Class QBEColumn
        'public XQBEForm QBEForm = null;
        Private mDbColumn As String
        Private mUseInQBE As Boolean
        Private mDisplayInQBEResult As Boolean
        Private mFriendlyName As String = ""
        Private mQBEValue As String
        Private mDisplayFormat As String = ""
        Private mBackColor As Drawing.Color
        Private mForeColor As Drawing.Color
        Private mQBEColumnType As QBEColumnsTypes
        Private mColumnSize As Integer = 0
        Private mOrdinalPosition As Integer = 0
        Private mFont As Drawing.Font = Nothing
        Private mAlignment As Wisej.Web.DataGridViewContentAlignment = Wisej.Web.DataGridViewContentAlignment.TopLeft
        Private mFontStyle As Drawing.FontStyle = New Drawing.FontStyle()
        Private mReportName As String
        Public Property FontSize As Single


        Public Property ReportName As String
            Get
                Dim ReportNameRet As String = Nothing
                ReportNameRet = mReportName
                Return ReportNameRet


            End Get
            Set(value As String)
                mReportName = value
            End Set
        End Property

        Public Property FontStyle As Drawing.FontStyle
            Get
                Dim FontStyleRet As Drawing.FontStyle = Nothing
                FontStyleRet = mFontStyle
                Return FontStyleRet

            End Get
            Set(value As Drawing.FontStyle)
                mFontStyle = value
            End Set
        End Property


        Public Property Aligment As Wisej.Web.DataGridViewContentAlignment
            Get
                Dim AligmentRet As Wisej.Web.DataGridViewContentAlignment = Nothing
                AligmentRet = mAlignment
                Return AligmentRet

            End Get
            Set(value As Wisej.Web.DataGridViewContentAlignment)
                mAlignment = value
            End Set
        End Property


        Public Property QBEColumnType As QBEColumnsTypes
            Get
                Dim QBEColumnTypeRet As QBEColumnsTypes = Nothing
                QBEColumnTypeRet = mQBEColumnType
                Return QBEColumnTypeRet

            End Get
            Set(value As QBEColumnsTypes)
                mQBEColumnType = value
            End Set
        End Property
        Public Property Font As Drawing.Font
            Get
                Dim FontRet As Drawing.Font = Nothing
                FontRet = mFont
                Return FontRet

            End Get
            Set(value As Drawing.Font)
                mFont = value
            End Set
        End Property

        Public Property ForeColor As Drawing.Color
            Get
                Dim ForeColorRet As Drawing.Color = Nothing
                ForeColorRet = mForeColor
                Return ForeColorRet

            End Get
            Set(value As Drawing.Color)
                mForeColor = value
            End Set
        End Property

        Public Property BackColor As Drawing.Color
            Get
                Dim BackColorRet As Drawing.Color = Nothing
                BackColorRet = mBackColor
                Return BackColorRet

            End Get
            Set(value As Drawing.Color)
                mBackColor = value
            End Set
        End Property


        Public Property DisplayFormat As String
            Get
                Dim DisplayFormatRet As String = Nothing
                DisplayFormatRet = mDisplayFormat
                Return DisplayFormatRet

            End Get
            Set(value As String)
                mDisplayFormat = value
            End Set
        End Property
        Public Property QBEValue As Object
            Get
                Dim QBEValueRet As Object = Nothing
                QBEValueRet = mQBEValue
                Return QBEValueRet
            End Get
            Set(value As Object)
                mQBEValue = value.ToString()
            End Set
        End Property

        Public Property FriendlyName As String
            Get
                Dim FriendlyNameRet As String = Nothing
                FriendlyNameRet = mFriendlyName
                If Equals(mFriendlyName, Nothing) Then mFriendlyName = DbColumn
                Return FriendlyNameRet
            End Get
            Set(value As String)
                mFriendlyName = value
            End Set
        End Property
        Public Property DisplayInQBEResult As Boolean
            Get
                Dim DisplayInQBEResultRet As Boolean = Nothing
                DisplayInQBEResultRet = mDisplayInQBEResult
                Return DisplayInQBEResultRet
            End Get
            Set(value As Boolean)
                mDisplayInQBEResult = value
            End Set
        End Property
        Public Property UseInQBE As Boolean
            Get
                Dim UseInQBERet As Boolean = Nothing
                UseInQBERet = mUseInQBE
                Return UseInQBERet
            End Get
            Set(value As Boolean)
                mUseInQBE = value
            End Set
        End Property
        Public Property DbColumn As String
            Get

                Return mDbColumn
            End Get
            Set(value As String)
                mDbColumn = value
            End Set
        End Property

        Public Property ColumnSize As Integer
            Get
                Dim ColumnWidthRet As Integer = Nothing
                ColumnWidthRet = mColumnSize
                Return ColumnWidthRet
            End Get
            Set(value As Integer)
                mColumnSize = value
            End Set
        End Property

        Public Property OrdinalPosition As Integer
            Get
                Dim OrdinalPositionRet As Integer = Nothing
                OrdinalPositionRet = mOrdinalPosition
                Return OrdinalPositionRet
            End Get
            Set(value As Integer)
                mOrdinalPosition = value
            End Set
        End Property


    End Class

    Public Class QBEColumns
        Inherits Dictionary(Of String, QBEColumn)
        'public XQBEForm QBEForm;


        Public Sub New()
            MyBase.New(StringComparer.InvariantCultureIgnoreCase)
        End Sub

        Public Function Add(DbColumn As String, Optional FriendlyName As String = "", Optional DisplayFormat As String = "", Optional QBEValue As Object = Nothing, Optional UseInQBE As Boolean = True, Optional DisplayInQBEResult As Boolean = True, Optional ColumnWidth As Integer = 0) As QBEColumn
            Dim x = New QBEColumn()
            Return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnsTypes.TextBox, ColumnWidth)

        End Function
        Public Function Add(DbColumn As String, FriendlyName As String, DisplayFormat As String, QBEValue As Object, UseInQBE As Boolean, DisplayInQBEResult As Boolean, QBEColumnType As QBEColumnsTypes, ColumnWidth As Integer) As QBEColumn
            Return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnType, ColumnWidth)
        End Function


        Public Function AddForReport(ReportName As String, DbColumn As String, FriendlyName As String, QBEValue As Object, QBEColumnType As QBEColumnsTypes) As QBEColumn
            Return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, True, False, QBEColumnType, 0)
        End Function

        Public Function AddForReport(ReportName As String, DbColumn As String, FriendlyName As String, Optional QBEValue As Object = Nothing) As QBEColumn
            Return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, True, False, QBEColumnsTypes.TextBox, 0)
        End Function

        Public Function AddForReport(Report As Object, DbColumn As String, FriendlyName As String, Optional QBEValue As Object = Nothing) As QBEColumn
            Return Add(DbColumn, FriendlyName, "", QBEValue, True, False, QBEColumnsTypes.TextBox, 0)
        End Function

        Private Function Add(ReportName As String, DbColumn As String, FriendlyName As String, DisplayFormat As String, QBEValue As Object, UseInQBE As Boolean, DisplayInQBEResult As Boolean, QBEColumnType As QBEColumnsTypes, ColumnWidth As Integer) As QBEColumn
            Dim x = New QBEColumn()

            x.ReportName = ReportName
            'x.QBEForm = QBEForm;
            x.OrdinalPosition = Count
            x.DbColumn = DbColumn
            x.UseInQBE = UseInQBE
            x.QBEValue = QBEValue
            x.FriendlyName = FriendlyName
            x.DisplayInQBEResult = DisplayInQBEResult
            x.DisplayFormat = DisplayFormat
            x.QBEColumnType = QBEColumnType
            x.ColumnSize = ColumnWidth
            x.Aligment = Wisej.Web.DataGridViewContentAlignment.TopLeft

            'if (DbColumn.IsNumeric() | DbColumn.IsDate())
            '{
            '    x.Aligment = DataGridViewContentAlignment.TopRight;
            '}
            'if (DbColumn.IsString())
            '{
            '    x.Aligment = DataGridViewContentAlignment.TopLeft;
            '}
            'if (DbColumn.IsTime() | DbColumn.IsDate())
            '{
            '    x.Aligment = DataGridViewContentAlignment.TopRight;
            '}
            'if (DbColumn.IsBoolean())
            '{
            '    x.Aligment = DataGridViewContentAlignment.MiddleCenter;
            '}

            If String.IsNullOrEmpty(Trim(x.FriendlyName)) Then x.FriendlyName = x.DbColumn
            If Not Equals(ReportName.Trim(), "") Then
                Me.Add(ReportName.Trim().ToUpper() & "|" & DbColumn.Trim().ToUpper(), x)
            Else
                Me.Add(DbColumn.Trim().ToUpper(), x)
            End If

            'List.Add(x);
            Return x
        End Function





    End Class




    <Serializable>
    Public Class QBEReportSortColumn
        Public Property Name As String
        Public Property FriendlyName As String
        Public Property Position As Integer
        Public Property AscDesc As String
    End Class

    <Serializable>
    Public Enum ReportTypes
        SSRSLocalReport = 0
        SSRSRemoteServer = 1
    End Enum



    <Serializable>
    Public Class QBEFRReport
        Public DataSets As Dictionary(Of String, DataSet) = New Dictionary(Of String, DataSet)(StringComparer.InvariantCultureIgnoreCase)
        Public ReportType As ReportTypes = ReportTypes.SSRSLocalReport
        Public SQLQueryParameters As DynamicParameters = New DynamicParameters()
        Public SortColumns As Dictionary(Of String, QBEReportSortColumn) = New Dictionary(Of String, QBEReportSortColumn)(StringComparer.InvariantCultureIgnoreCase)
        Public SelectedSortColumns As Dictionary(Of String, QBEReportSortColumn) = New Dictionary(Of String, QBEReportSortColumn)(StringComparer.InvariantCultureIgnoreCase)
        Public SQLQuery As String = ""

        Private mReportTitle As String
        Private mReportFileName As String
        Private mReportDescription As String
        Private mReportUseLike As Boolean
        Public Property DbConnection As IDbConnection
        Public Property PrimaryDataSet As DataSet

        Public Function SetPrimaryDataSet(Name As String) As Boolean
            Dim result = False
            If DataSets.ContainsKey(Name) Then
                PrimaryDataSet = DataSets(Name)
                result = True
            End If

            Return result
        End Function
        Public Function AddDataSet(Of T)(Name As String, DbConnection As IDbConnection, Optional SQLQuery As String = "", Optional Parameters As DynamicParameters = Nothing) As DataSet
            Dim ds As DataSet = New DataSet()

            ds.Name = Name
            ds.DbConnection = DbConnection

            If Not Equals(SQLQuery, "") Then ds.SQLQuery = SQLQuery
            If Parameters IsNot Nothing Then ds.Parameters = Parameters
            ds.ModelType = GetType(T)
            ds.EnsureReportDataSet()
            DataSets.Add(Name, ds)
            Return ds
        End Function


        Public Function OrderBy() As String
            Dim s = ""
            For Each item In SelectedSortColumns.Values
                s += $"{item.Name} {item.AscDesc}, "
            Next

            s = s.Trim()
            If s.EndsWith(",") Then
                s = s.Substring(0, s.Length - 1)
            End If

            If Not Equals(s, "") Then s = $" ORDER BY {s}"
            Return s
        End Function


        Public Property ReportUseLike As Boolean
            Get
                Dim ReportUseLikeRet As Boolean = Nothing
                ReportUseLikeRet = mReportUseLike
                Return ReportUseLikeRet

            End Get
            Set(value As Boolean)
                mReportUseLike = value

            End Set
        End Property
        Public Property ReportFileName As String
            Get
                Dim ReportFileNameRet As String = Nothing
                ReportFileNameRet = mReportFileName
                Return ReportFileNameRet
            End Get
            Set(value As String)
                mReportFileName = value

            End Set
        End Property



        Public Property ReportDescription As String
            Get
                Dim ReportDescriptionRet As String = Nothing
                ReportDescriptionRet = mReportDescription
                Return ReportDescriptionRet

            End Get
            Set(value As String)
                mReportDescription = value

            End Set
        End Property


        Public Property ReportTitle As String
            Get
                Dim ReportTitleRet As String = Nothing
                ReportTitleRet = mReportTitle
                Return ReportTitleRet

            End Get
            Set(value As String)
                mReportTitle = value

            End Set
        End Property
    End Class
    Public Class QBEReports
        Inherits Dictionary(Of String, QBEFRReport)

        Public Function Add(ReportTitle As String, ReportFileName As String, Optional ReportDescription As String = "", Optional DbConnection As IDbConnection = Nothing) As QBEFRReport
            Dim x = New QBEFRReport()

            x.ReportDescription = ReportDescription
            x.ReportFileName = ReportFileName
            x.ReportTitle = ReportTitle
            x.DbConnection = DbConnection
            MyBase.Add(x.ReportTitle.Trim().ToUpper(), x)
            Return x
        End Function


    End Class



    Public Enum FRRenderFormat
        XML
        NULL
        CSV
        IMAGE
        PDF
        HTML40
        HTML32
        MHTML
        EXCEL
        WORD
    End Enum
    Public Class DataSet
        Public Property Name As String = String.Empty
        Public Property DbConnection As IDbConnection
        Public Property Parameters As DynamicParameters = New DynamicParameters()
        Public Property SQLQuery As String
        'public object Repository { get; set; }
        Public Property ModelType As Type
        Public ModelProperties As Dictionary(Of String, PropertyInfo) = New Dictionary(Of String, PropertyInfo)()
        Public Property Data As Object
        Public Property Model As Object
        Public Sub New()
        End Sub
        Public Sub New(Name As String, ModelType As Type, DbConnection As IDbConnection, Optional Parameters As DynamicParameters = Nothing)
            Me.Name = Name
            Me.ModelType = ModelType
            Me.DbConnection = DbConnection
            Me.Parameters = Parameters

            EnsureReportDataSet()
        End Sub

        Public Function EnsureReportDataSet() As Boolean
            If ModelType IsNot Nothing AndAlso DbConnection IsNot Nothing Then
                Dim obj = Activator.CreateInstance(ModelType)
                Model = obj
                ReflectionHelper.SetPropertyValue(obj, "DbConnection", DbConnection)
                ReflectionHelper.SetPropertyValue(obj, "Parameters", Parameters)
                'this.Repository = obj;


                ModelProperties.Clear()
                For Each item In DapperHelper.Utilities.GetPropertiesInfo(ModelType)
                    ModelProperties.Add(item.Name, item)
                Next


                Return True
            End If
            Return False
        End Function

        Public Sub LoadData()
            Data = DbConnection.Query(SQLQuery, Parameters)

        End Sub
    End Class

    Public Class ReportRenderRequestEventArgs
        Inherits EventArgs
        Public Property Cancel As Boolean
        Public Property ReportName As String
        Public DataSets As Dictionary(Of String, DataSet) = New Dictionary(Of String, DataSet)()
    End Class

    Public Class ReportAfterRenderEventArgs
        Inherits EventArgs
        Public Property Cancel As Boolean
        Public Property Success As Boolean
        Public Property ReportName As String
    End Class

    Public Class FRReport
        Public Property ReportPath As String
        Public Property ReportFormat As FRRenderFormat = FRRenderFormat.PDF
        Public Property LastExecutionResult As ExecutionResult = New ExecutionResult("Passero.Framework.FRReports.")
        Public Property DataSets As Dictionary(Of String, DataSet) = New Dictionary(Of String, DataSet)()

        Public Property Report As Report = New Report()


        Public Sub New()
            Report = New Report()
        End Sub

        Public Event ReportRenderRequest As EventHandler

        Protected Overridable Sub OnReportRenderRequest(e As ReportRenderRequestEventArgs)
            RaiseEvent ReportRenderRequest(Me, e)
        End Sub

        Public Event ReportAfterRender As EventHandler

        Protected Overridable Sub OnReportAfterRender(e As ReportAfterRenderEventArgs)
            RaiseEvent ReportAfterRender(Me, e)
        End Sub



        Private Sub PrepareReport()

            Dim SqlConnection As MsSqlDataConnection = New MsSqlDataConnection()
            Report.Dictionary.Connections.Clear()
            For Each dataset In DataSets
                If dataset.Value.DbConnection.GetType() Is GetType(SqlClient.SqlConnection) Then
                    Utils.RegisteredObjects.AddConnection(GetType(MsSqlDataConnection), "MsSqlDataConnection")
                End If
            Next

            Report.Load(ReportPath)
            Dim datasets_validated = True
            For Each dataset In DataSets
                'TableDataSource table = Report.GetDataSource(dataset.Value.Name) as TableDataSource;
                Dim table As TableDataSource = TryCast(Report.GetDataSource("C"), TableDataSource)
                If table IsNot Nothing Then
                    table.Parameters.Clear()
                    table.Connection.ConnectionString = dataset.Value.DbConnection.ConnectionString
                    table.SelectCommand = dataset.Value.SQLQuery
                    For Each name In dataset.Value.Parameters.ParameterNames
                        Dim parameter As CommandParameter = New CommandParameter()
                        parameter.Name = name
                        parameter.DefaultValue = ""
                        parameter.Value = dataset.Value.Parameters.[Get](Of dynamic)(name)
                        parameter.DataType = SqlDbType.VarChar
                        table.Parameters.Add(parameter)
                    Next
                Else
                    datasets_validated = False
                    Exit For
                End If
            Next

            If datasets_validated = True Then
                Report.Prepare()
                Dim pdfExport As Export.PdfSimple.PDFSimpleExport = New Export.PdfSimple.PDFSimpleExport()
                pdfExport.Export(Report, "C:\REPORTS\XREPORT1.pdf")
            End If

        End Sub


        Public Function Render(Optional RenderFormat As FRRenderFormat = FRRenderFormat.PDF) As Byte()
            LastExecutionResult.Reset()
            LastExecutionResult.Context = $"Passero.Framework.FSReports.FRReport.Render({RenderFormat})"
            Dim result As Byte() = Nothing
            Dim f As String = RenderFormat.ToString()


            Dim SqlConnection As MsSqlDataConnection = New MsSqlDataConnection()
            Report.Dictionary.Connections.Clear()
            For Each dataset In DataSets
                If dataset.Value.DbConnection.GetType() Is GetType(SqlClient.SqlConnection) Then
                    Utils.RegisteredObjects.AddConnection(GetType(MsSqlDataConnection), "MsSqlDataConnection")
                End If
            Next


            Try
                Report.Load(ReportPath)


                ' Invoke OnReportRenderRequest
                'ReportRenderRequestEventArgs requestargs = new ReportRenderRequestEventArgs();
                'requestargs.DataSets = new Dictionary<string, DataSet>();
                'foreach (string DataSetName in this.DataSetNames())
                '{
                '    DataSet ds = new DataSet();
                '    ds.Name = DataSetName;
                '    requestargs.DataSets.Add(DataSetName, ds);
                '}
                'this.OnReportRenderRequest(requestargs);
                'if (requestargs.Cancel)
                '{
                '    LastExecutionResult.ResultMessage = "Cancelled by User";
                '    return null;
                '}



                Dim datasets_validated = True
                For Each dataset In DataSets
                    Dim table As TableDataSource = TryCast(Report.GetDataSource(dataset.Value.Name), TableDataSource)
                    If table IsNot Nothing Then
                        table.Parameters.Clear()
                        table.Connection.ConnectionString = dataset.Value.DbConnection.ConnectionString
                        table.SelectCommand = dataset.Value.SQLQuery
                        For Each name In dataset.Value.Parameters.ParameterNames
                            Dim parameter As CommandParameter = New CommandParameter()
                            parameter.Name = name
                            parameter.DefaultValue = ""
                            parameter.Value = dataset.Value.Parameters.[Get](Of dynamic)(name)
                            parameter.DataType = SqlDbType.VarChar
                            table.Parameters.Add(parameter)
                        Next
                    Else
                        datasets_validated = False
                        Exit For
                    End If
                Next

                If datasets_validated = True Then
                    Report.Prepare()
                    Dim stream As MemoryStream = New MemoryStream()
                    Select Case RenderFormat
                        Case FRRenderFormat.XML
                        Case FRRenderFormat.NULL
                        Case FRRenderFormat.CSV
                        Case FRRenderFormat.IMAGE
                        Case FRRenderFormat.PDF
                            Dim pdfExport As Export.PdfSimple.PDFSimpleExport = New Export.PdfSimple.PDFSimpleExport()
                            'pdfExport.Export(Report, @"C:\REPORTS\XREPORT1.pdf");
                            pdfExport.Export(Report, stream)
                            result = stream.ToArray()
                        Case FRRenderFormat.HTML40
                        Case FRRenderFormat.HTML32
                        Case FRRenderFormat.MHTML
                        Case FRRenderFormat.EXCEL
                        Case FRRenderFormat.WORD
                        Case Else
                    End Select


                End If
            Catch ex As Exception
                LastExecutionResult.Exception = ex
                LastExecutionResult.ResultMessage = ex.Message
                LastExecutionResult.ErrorCode = 1
            End Try


            Return result
        End Function

        Public Function DataSetNames() As List(Of String)
            Dim result As List(Of String) = New List(Of String)()
            Try

                For Each dataSource As DataSourceBase In Report.Dictionary.DataSources
                    Dim dataSourceName = dataSource.Name

                    ' You can also check its type if needed
                    If TypeOf dataSource Is TableDataSource Then
                        ' It's a TableDataSource
                        Dim tableDataSource As TableDataSource = TryCast(dataSource, TableDataSource)
                        ' Additional handling for TableDataSource
                        result.Add(dataSourceName)
                    End If


                Next
            Catch __unusedException1__ As Exception
                Dim x = 0
            End Try
            Return result
        End Function

        Public Function RenderAndSaveReport(FileName As String, Optional RenderFormat As FRRenderFormat = FRRenderFormat.PDF) As ExecutionResult
            Dim ER = New ExecutionResult()
            ER.Context = $"Passero.Framework.Reports.SSRSReports.RenderAndSaveReport({FileName},{RenderFormat})"
            Dim result As Byte() = Nothing

            result = Render(RenderFormat)
            If LastExecutionResult.Failed Then
                ER = LastExecutionResult
                Return ER
            End If

            If result Is Nothing Then
                ER.ErrorCode = 2
                ER.ResultMessage = "Empty Rendering."
                LastExecutionResult = ER
                Return ER
            End If

            Try
                Utilities.SaveByteArrayToFile(result, FileName)

            Catch ex As Exception
                ER.ErrorCode = 3
                ER.ResultMessage = ex.Message
                LastExecutionResult = ER
                Return ER
            End Try

            Return ER

        End Function

    End Class
End Namespace
