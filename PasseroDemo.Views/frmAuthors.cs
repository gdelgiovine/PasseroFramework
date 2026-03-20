using Passero.Framework;
using Passero.Framework.Base;
using Passero.Framework.Controls;
using System;
using System.Data;

using System.Linq;
using Wisej.Web;



namespace PasseroDemo.Views
{
    public partial class frmAuthors : Form
    {

        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }
        public ViewModels .vmAuthor vmAuthor = new ViewModels .vmAuthor ();

        private Passero.Framework.Base.IPasseroDbContext _dbContext;

        public frmAuthors()
        {
            InitializeComponent();
        }

        public async void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];

            // ORMContextFactory sceglie EF6 (net48) o EF Core (net8.0+) automaticamente.
            // ViewModel riceve il DbContext e crea internamente il repository corretto.
            //_dbContext = Passero.Framework.Base.ORMContextFactory.Create(
            //    Passero.Framework.Base.ORMType.EntityFramework      ,
            //    DbConnection.ConnectionString,
            //    new[] { typeof(Models.Author) });


            _dbContext = Passero.Framework.Base.ORMContextFactory.Create(
             Passero.Framework.Base.ORMType.Dapper,
             DbConnection.ConnectionString);


            vmAuthor = new ViewModels.vmAuthor();

            //vmAuthor = new ViewModels.vmAuthor();
            //vmAuthor.Init();
            //vmAuthor.Init(this.DbConnection);

            vmAuthor.Init(this._dbContext);
           

            vmAuthor.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmAuthor.BindingSource = this.bsAuthors;

            this.Accelerators = this.dataNavigator1.GetAccelerators();

            this.dataNavigator1.AddViewModel(this.vmAuthor, "Authors", null, null);
            this.dataNavigator1.SetActiveViewModel(this.vmAuthor);
            this.dataNavigator1.Init(true);

            //this._dbContext.DiscardChanges(vmAuthor)    ;

            //IQueryable<Models.Author> query = _dbContext.Set<Models.Author>();
            //var list = await _dbContext.ToListAsync(query.Where(a => a.contract ));
        }


        private void QBE_Authors()
        {

            //QBEForm<Models.Author> QBE = new QBEForm<Models.Author>(this.DbConnection);
            // Costruttore con DbContext: usa EfRepository o DapperRepository automaticamente
            QBEForm<Models.Author> QBE = new QBEForm<Models.Author>(_dbContext);


            QBE.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
            QBE.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
            QBE.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true, 20);
            QBE.QBEColumns.Add(nameof(Models.Author.contract), "Have contract", "", "", true, true, Passero.Framework.Controls.QBEColumnsTypes.CheckBox, 20);
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
            QBE.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
            QBE.Owner = this;
            QBE.SetFocusControlAfterClose = this.txt_au_id;
          
            QBE.SetTargetViewModel(this.vmAuthor,() => { this.dataNavigator1.Init(true); });
            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);


            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");
            QBE.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));
            QBE.AutoLoadData =true;
            QBE.ResultGrid.RowHeaderColumn.Visible = false;
            QBE.Text = "Authors QBE";
            QBE.ShowQBE();

        }


        private void FR_QBEReport_Authors()
        {

            Passero.Framework.FRReports.ReportManager  xQBEReport = new Passero.Framework.FRReports.ReportManager ();
            this._dbContext.EnsureConnectionOpen();

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.FRX", "REPORT UNO");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("Authors", this.vmAuthor.Repository.DbConnection);
            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            xQBEReport.CallBackAction = () => { this.dataNavigator1.Init(true); };
            xQBEReport.ShowQBEReport();
        }

        private void SSRS_QBEReport_Authors()
        {
            Passero.Framework.SSRSReports.ReportManager SSRSReportManager = new Passero.Framework.SSRSReports.ReportManager();
            SSRSReportManager = new Passero.Framework.SSRSReports.ReportManager();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            SSRSReportManager.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT UNO");
            SSRSReportManager.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection);
            SSRSReportManager.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSets["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSets["DataSet2"].Parameters.Add("@au_id", "1123");


            SSRSReportManager.QBEReports.Add("REPORT2", @"C:\Reports\REPORT2.RDL", "REPORT DUE");
            SSRSReportManager.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");
            SSRSReportManager.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet2", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");

            SSRSReportManager.DefaultReport = SSRSReportManager.QBEReports["REPORT1"];

            SSRSReportManager.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            SSRSReportManager.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            SSRSReportManager.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            SSRSReportManager.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.contract ), "", "");

            SSRSReportManager.QBEColumns.AddForReport("REPORT2", nameof(Models.Author.au_id), "CODICE", "");

            SSRSReportManager.SetFocusControlAfterClose = this.txt_au_id;
            SSRSReportManager.CallBackAction = () => { this.dataNavigator1.Init(true); };
            SSRSReportManager.Text = "Authors Reports";
            SSRSReportManager.ShowQBEReport();
        }
        private void frmAuthors_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void dataNavigator1_ePrint()
        {
           //this.SSRS_QBEReport_Authors();
           this.FR_QBEReport_Authors();

        }

        private void dataNavigator1_eFind()
        {
            this.QBE_Authors();
        }

        private void frmAuthors_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
            //this.dataNavigator1.HandleUserInput(sender , e);    
        }

        

        private void frmAuthors_Accelerator(object sender, AcceleratorEventArgs e)
        {
            e.Handled = true;
            this.dataNavigator1.HandleUserInput(sender, e);
        }
    }
}
