using System;
using System.Data;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;


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
        Passero.Framework .Controls.XQBEForm<Models.Author> xQBEForm_Author = new Passero.Framework.Controls.XQBEForm<Models.Author>();
        Passero.Framework.Controls.XQBEReport xQBEReport = new Passero.Framework.Controls.XQBEReport();
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
            this.dataNavigator1.ViewModels["Employee"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmAuthor , "Employee");
            this.dataNavigator1.SetActiveViewModel("Employee");

            
            // Questo Metodo sta nel ViewModel Customizzato
            //this.vmAuthor.GetAuthors();
            // Questo Metodo invece sta nella classe base ViewModel
           // this.vmAuthor.GetAllItems();

            //this.dbLookUpTextBox1.DbConnection = this.DbConnection;
            //this.dbLookUpTextBox1.ModelClass = typeof(Models.Author);
            //this.dbLookUpTextBox1.DisplayMember = "au_fullname";
            //this.dbLookUpTextBox1.ValueMember = "au_id";
            //this.dbLookUpTextBox1.SelectClause = "SELECT *, TRIM(au_fname)+' '+TRIM(au_lname) as au_fullname";
            this.dataNavigator1.ViewModels["Author"] = new DataNavigatorViewModel(this.vmAuthor,"Authors");
            this.dataNavigator1.SetActiveViewModel("Author");
            this.dataNavigator1.InitDataNavigator();
            
        }

        public void Reload()
        {
            //this.vmAuthor.GetAllItems();
            this.dataNavigator1.InitDataNavigator(true);

        }

        private void QBE_Authors()
        {

            xQBEForm_Author = new XQBEForm<Models.Author>(this.DbConnection);

            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true, 20);
            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.contract), "Have contract", "", "", true, true, QBEColumnsTypes.CheckBox, 20);
            //xQBEForm_Author.QBEColumns["au_id"].ForeColor = System.Drawing.Color.Red;
            xQBEForm_Author.QBEColumns["au_id"].FontStyle = System.Drawing.FontStyle.Bold;
            //xQBEForm_Author.QBEColumns["au_id"].FontSize = 10;
            //xQBEForm_Author.QBEColumns[nameof(Models.Author.contract)].Aligment = DataGridViewContentAlignment.MiddleCenter ;

            xQBEForm_Author.SetupQBEForm();
            //            xQBEForm_Author .ResultGrid.Columns["au_id"].DefaultCellStyle .BackColor = System.Drawing.Color.Magenta ;

            //xQBEForm_Author.QBEResultMode = QBEResultMode.BoundControls;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.AllRowsItems;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.MultipleRowsItems;
            xQBEForm_Author.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
            //xQBEForm_Author.Owner = this;
            xQBEForm_Author.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEForm_Author.CallBackAction = () => { this.Reload(); };
            xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository,() => { this.Reload(); });
            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);


            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");
            xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));


            xQBEForm_Author.ShowQBE();

        }

        private void QBEReport_Authors()
        {
            xQBEReport = new XQBEReport();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT UNO");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection);
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].Parameters.Add("@au_id", "1123");


            xQBEReport.QBEReports.Add("REPORT2", @"C:\Reports\REPORT1.RDL", "REPORT DUE");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");

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
            this.QBEReport_Authors(); 
        }

        private void dataNavigator1_eFind()
        {
            this.QBE_Authors();
        }

        private void dataNavigator1_eUndo()
        {

        }
    }
}
