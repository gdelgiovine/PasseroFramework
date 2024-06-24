using System;
using System.Data;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;
using Passero.Framework.SSRSReports;
using System.IO;
using System.Data.SqlClient;
using FastReport.Data;
using System.Threading;
using Passero.Framework.FRReports;


namespace PasseroDemo.Views
{
    public partial class frmAuthors : Form
    {

        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }
        // Cosi di usa il viewmodel esteso
        //public PasseroDemo.ViewModels.Author vmAuthor = new ViewModels.Author();

        // cosi quello base
        public Passero.Framework.ViewModel<Models.Author> vmAuthor = new Passero.Framework.ViewModel<Models.Author>();
        Passero.Framework .Controls.QBEForm<Models.Author> QBEForm_Author = new Passero.Framework.Controls.QBEForm<Models.Author>();
        Passero.Framework.SSRSReports.ReportManager xQBEReport = new Passero.Framework.SSRSReports.ReportManager();
        Passero.Framework.DbLookUp<Models.Author> dblAuthor = new DbLookUp<Models.Author>();

        public frmAuthors()
        {
            InitializeComponent();
        }


        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            vmAuthor.Init(this.DbConnection);
            vmAuthor.DataBindControlsAutoSetMaxLenght = true;
            vmAuthor.AutoWriteControls = true;
            vmAuthor.AutoReadControls = true;
            vmAuthor.AutoFitColumnsLenght = true;
            //vmAuthor.UseModelData = Passero.Framework.Base.UseModelData.InternalRepository  ;

            vmAuthor.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmAuthor.BindingSource = this.bsAuthors;

            vmAuthor.CreatePasseroBindingFromBindingSource();
            
            
            // Questo Metodo sta nel ViewModel Customizzato
            //this.vmAuthor.GetAuthors();
            // Questo Metodo invece sta nella classe base ViewModel
           // this.vmAuthor.GetAllItems();

            //this.dbLookUpTextBox1.DbConnection = this.DbConnection;
            //this.dbLookUpTextBox1.ModelClass = typeof(Models.Author);
            //this.dbLookUpTextBox1.DisplayMember = "au_fullname";
            //this.dbLookUpTextBox1.ValueMember = "au_id";
            //this.dbLookUpTextBox1.SelectClause = "SELECT *, TRIM(au_fname)+' '+TRIM(au_lname) as au_fullname";
            this.dataNavigator1.ViewModels["Authors"] = new DataNavigatorViewModel(this.vmAuthor,"Authors");
            this.dataNavigator1.SetActiveViewModel("Authors");
            this.dataNavigator1.InitDataNavigator(true);

            this.dataGridView1.DataSource = this.vmAuthor.ModelItems;
            
        }

        public void Reload()
        {
            //this.vmAuthor.GetAllItems();
            this.dataNavigator1.InitDataNavigator(true);

        }

        private void QBE_Authors()
        {

            QBEForm_Author = new QBEForm<Models.Author>(this.DbConnection);

            QBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
            QBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
            QBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true, 20);
            QBEForm_Author.QBEColumns.Add(nameof(Models.Author.contract), "Have contract", "", "", true, true, Passero.Framework.Controls.QBEColumnsTypes.CheckBox, 20);
            ////xQBEForm_Author.QBEColumns["au_id"].ForeColor = System.Drawing.Color.Red;
            //QBEForm_Author.QBEColumns["au_id"].FontStyle = System.Drawing.FontStyle.Bold;
            ////xQBEForm_Author.QBEColumns["au_id"].FontSize = 10;
            ////xQBEForm_Author.QBEColumns[nameof(Models.Author.contract)].Aligment = DataGridViewContentAlignment.MiddleCenter ;

            //QBEForm_Author.SetupQBEForm();
            //            xQBEForm_Author .ResultGrid.Columns["au_id"].DefaultCellStyle .BackColor = System.Drawing.Color.Magenta ;

            //xQBEForm_Author.QBEResultMode = QBEResultMode.BoundControls;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.AllRowsItems;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.MultipleRowsItems;
            QBEForm_Author.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
            QBEForm_Author.Owner = this;
            QBEForm_Author.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEForm_Author.CallBackAction = () => { this.Reload(); };
            QBEForm_Author.SetTargetRepository(this.vmAuthor.Repository,() => { this.Reload(); });
            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);


            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");
            QBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));
            QBEForm_Author.AutoLoadData =true;

            QBEForm_Author.ShowQBE();

        }


        private void FR_QBEReport_Authors()
        {
            Passero.Framework.FRReports.ReportManager  xQBEReport = new Passero.Framework.FRReports.ReportManager ();
            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.FRX", "REPORT UNO");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("Authors", this.vmAuthor.Repository.DbConnection);
            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            xQBEReport.CallBackAction = () => { this.Reload(); };
            xQBEReport.ShowQBEReport();
        }

        private void SSRS_QBEReport_Authors()
        {
            xQBEReport = new Passero.Framework.SSRSReports.ReportManager();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT UNO");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection);
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSets["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSets["DataSet2"].Parameters.Add("@au_id", "1123");


            xQBEReport.QBEReports.Add("REPORT2", @"C:\Reports\REPORT2.RDL", "REPORT DUE");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet2", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");

            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];

            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT2", nameof(Models.Author.au_id), "CODICE", "");

            xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            xQBEReport.CallBackAction = () => { this.Reload(); };
            xQBEReport.ShowQBEReport();
        }
        private void frmAuthors_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void dataNavigator1_ePrint()
        {
            this.SSRS_QBEReport_Authors(); 
        }

        private void dataNavigator1_eFind()
        {
            this.QBE_Authors();
        }

        private void dataNavigator1_eUndo()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string reportsFolder = @"C:\REPORTS";
            MsSqlDataConnection sqlConnection = new MsSqlDataConnection();
            sqlConnection.ConnectionString = this.vmAuthor.DbConnection.ConnectionString;
            sqlConnection.CreateAllTables();
            FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            FastReport.Report xreport = new FastReport.Report();
            //xreport.Dictionary.ClearRegisteredData();
            //xreport.Dictionary.Connections.Clear();
            //xreport.Dictionary.Connections.Add(sqlConnection);
            xreport.Load(Path.Combine(reportsFolder, "REPORT1.frx"));
            xreport.Dictionary.ClearRegisteredData();
            xreport.Dictionary.Connections.Clear();
            xreport.Dictionary.Connections.Add(sqlConnection);

            TableDataSource table = xreport.GetDataSource("authors") as TableDataSource;


            table.SelectCommand = this.vmAuthor.Repository.SQLQuery;
            
            foreach (var name in this.vmAuthor.Repository.Parameters.ParameterNames)
            {
                CommandParameter parameter = new CommandParameter();
                parameter.Name = name;
                parameter.DefaultValue = "";
                parameter.Value = this.vmAuthor.Repository.Parameters.Get<dynamic>(name);
                parameter.DataType = (int)SqlDbType.VarChar;
                
                
                table.Parameters.Add(parameter);
            }
            
            

            //xreport.RegisterData(this.vmAuthor .ModelItems, "Authors");

            xreport.Prepare();
            FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();

            pdfExport.Export(xreport, @"C:\REPORTS\XREPORT1.pdf");





#if NET
            FastReport.Web.WebReport report = new FastReport.Web.WebReport();
            report.Report.Dictionary.Connections.Add(sqlConnection);
            report.Report.Load(Path.Combine(reportsFolder, "REPORT1.frx"));
            report.Report.Prepare();
            pdfExport.Export(report.Report, @"C:\REPORTS\REPORT1.pdf");


#else
      
#endif


        }

        private void button2_Click(object sender, EventArgs e)
        {


            this.FR_QBEReport_Authors();

        }
    }
}
