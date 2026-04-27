using Passero.Framework;
using Passero.Framework.Controls;
using Passero.Framework.SSRSReports;
using System;
using System.CodeDom;
using Wisej.Web;
using Wisej.Web.Data;


namespace PasseroDemo.Views
{
    public partial class frmEmployee : Wisej.Web.Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public System.Data.IDbConnection DbConnection { get; set; }
        // The ViewModels can be created from scratch or from customized ViewModel defined in custom classes
        // In this case we create a generic ViewModel for a model
        // REPLACE <ModelStub> with your Model
        private Passero.Framework.ViewModel<Models.Employee > vmEmployee = new Passero.Framework.ViewModel<Models.Employee>();
        private Passero.Framework.ViewModel<Models.Publisher > vmPublisher = new Passero.Framework.ViewModel<Models.Publisher >();
        private Passero.Framework.ViewModel<Models.Job > vmJob = new Passero.Framework.ViewModel<Models.Job>();
        //// Here we create an XQBEForm object for Query/Search Model Items
        //// REPLACE <ModelStub> with your Model
        //private Passero.Framework.Controls.XQBEForm<Models.Employee > xQBEForm_Employee = new Passero.Framework.Controls.XQBEForm<  Models .Employee >();
        //// Here we create an XQBEReport object for SSRS Report Display
        //private Passero.Framework.Controls.XQBEReport xQBEReport = new Passero.Framework.Controls.XQBEReport();

        public frmEmployee()
        {
            InitializeComponent();
        }

        public void Init()
        {
            // Set DbConnection with you DbConnection Object
            // You can store it inside the ConfigurationManager 
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            vmEmployee.Init(this.DbConnection);
            vmEmployee.DataBindControlsAutoSetMaxLenght = true;
            vmEmployee.AutoWriteControls = true;
            vmEmployee.AutoReadControls = true;
            vmEmployee.AutoFitColumnsLenght = true;
            // here we setting the Data Bind using a BindingSource so we can use a DataSource and visual binding at design time
            vmEmployee.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmEmployee.BindingSource = this.bsEmployee ;


            vmPublisher.Init(this.DbConnection);
            vmJob.Init(this.DbConnection);

            this.cmb_job_id.DataSource = vmJob.GetAllItems().Value; 
            this.cmb_job_id.DisplayMember = nameof(Models.Job.job_fullinfo );
            this.cmb_job_id.ValueMember = nameof(Models.Job.job_id);

            this.cmb_pub_id .DataSource = vmPublisher.GetAllItems().Value;
            this.cmb_pub_id .DisplayMember = nameof(Models.Publisher.pub_fullinfo  );    
            this.cmb_pub_id .ValueMember = nameof(Models.Publisher .pub_id);    

          
            //this.dataNavigator1.ViewModels["Employee"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmEmployee, "Employee");
            //this.dataNavigator1.SetActiveViewModel("Employee");

            this.dataNavigator1.AddViewModel(this.vmEmployee , "Employees");
            this.dataNavigator1.SetActiveViewModel(this.vmEmployee );


            this.dataNavigator1.Init(true);

        }


        private void QBE_Employee()
        {

            QBEForm <Models.Employee> QBE = new QBEForm<Models.Employee >(this.DbConnection);


            // Here we customize the Result Grid and the Query Grid adding Columns to display in Result and in Query Criteria
            QBE.QBEColumns.Add(nameof(Models.Employee .emp_id), "Employee ID", "Employee Identification");
            QBE.QBEColumns.Add(nameof(Models.Employee .fname), "First Name", "Employee First Name");
            QBE.QBEColumns.Add(nameof(Models.Employee .lname), "Last Name", "Employee Last Name");  
            QBE.QBEColumns.Add(nameof(Models.Employee .hire_date), "Hire Date", "Employee Hire Date");  
            QBE.QBEColumns.Add(nameof(Models.Employee .job_id), "Job", "Employee Job"); 
            QBE.QBEColumns.Add(nameof(Models.Employee .pub_id), "Publisher", "Employee Publisher"); 

            // Here we customize aspect of Result Grid
            //QBE.QBEColumns["au_id"].ForeColor = System.Drawing.Color.Red;
            

            // Here we Build the Result Grid and the Query Grid.
            QBE.SetupQBEForm();
            // After the SetupQEBForm we have the ResultGrid Object (a DataGridView) and we can access/change any property
            //QBE.ResultGrid.Columns["au_id"].DefaultCellStyle .BackColor = System.Drawing.Color.Magenta ;

            // Setting the QBEResultMode for XQBEForm
            //QBE.QBEResultMode = QBEResultMode.BoundControls;
            //QBE.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            QBE.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
            //QBE.QBEResultMode = QBEResultMode.AllRowsItems;
            //QBE.QBEResultMode = QBEResultMode.MultipleRowsItems;

            // Setting the Owner Form 
            QBE.Owner = this;
            //QBE.SetFocusControlAfterClose = this.txt_au_id;
            //QBE.CallBackAction = () => { this.dataNavigator1.Init(true); };

            // The TargetRepository must be defined when we use QBEResultMode other than BoundControls
            //QBE.SetTargetRepository(this.myViewModel.Repository);
            // We can use even an Action to invoke a Method inside the View to reload the data in a custom mode. 
            //QBE.SetTargetRepository(this.myViewModel.Repository,() => { methodtocall(); });
            QBE.SetTargetRepository (this.vmEmployee.Repository, () => { this.dataNavigator1.Init(true); });

            // When whe return SQLQuery or Items  we need to map Model Properties from XQUEForm Model and the Target ViewModel
            //QBE.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //QBE.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));
            QBE.QBEModelPropertiesMapping.Add(nameof(Models.Employee .emp_id), nameof(Models.Employee .emp_id));

            // If whe have a QBEResultMode.BoundControls  we need to map XQBEForm Model whit Controls inside the View Form
            //xQBEForm_myModel.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");


            // We can invoke the ShowQBE Method. This method can be Sync or Async using the Wait parameter
            // When Wait is true the flow control remain inside the current method 
            
            QBE.ShowQBE(true);
            //QBE.ShowQBEWait() is a shortcut to ShowQBE(true)




        }

        private void QBEReport_base()
        {
            ReportManager xQBEReport = new ReportManager();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT ONE");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.vmEmployee.Repository.DbConnection);
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.vmEmployee.Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].Parameters.Add("@au_id", "1123");


            xQBEReport.QBEReports.Add("REPORT2", @"C:\Reports\REPORT1.RDL", "REPORT TWO");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.vmEmployee.Repository.DbConnection, "SELECT * FROM Authors");

            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];

            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT2", nameof(Models.Author.au_id), "", "");

            //xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEReport.CallBackAction = () => { methodtocall(); };
            xQBEReport.ShowQBEReport();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            this.Init();
        }


        #region dataNavigator1_eEvents
        private void dataNavigator1_eAddNew()
        {



        }

        private void dataNavigator1_eBoundCompleted()
        {

        }

        private void dataNavigator1_eClose()
        {

        }

        private void dataNavigator1_eDelete()
        {

        }

        private void dataNavigator1_eFind()
        {
            this.QBE_Employee();
        }

        private void dataNavigator1_eMoveFirst()
        {

        }

        private void dataNavigator1_eMoveLast()
        {

        }

        private void dataNavigator1_eMoveNext()
        {

        }

        private void dataNavigator1_eMovePrevious()
        {

        }

        private void dataNavigator1_ePrint()
        {

        }

        private void dataNavigator1_eRefresh()
        {

        }

        private void dataNavigator1_eSave()
        {

        }

        private void dataNavigator1_eUndo()
        {

        }
        #endregion

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
