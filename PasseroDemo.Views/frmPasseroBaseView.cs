using System;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;
using Wisej.Web.Data;
using System.CodeDom;
using Passero.Framework.SSRSReports;


namespace PasseroDemo.Views
{
    public partial class frmPasseroBaseView : Wisej.Web.Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public System.Data.IDbConnection DbConnection { get; set; }
        // The ViewModels can be created from scratch or from customized ViewModel defined in custom classes
        // In this case we create a generic ViewModel for a model
        // REPLACE <ModelStub> with your Model
        private Passero.Framework.ViewModel<ModelStub> myViewModel = new Passero.Framework.ViewModel<ModelStub >();

        // Here we create an XQBEForm object for Query/Search Model Items if you want manage class level. Otherwise you can create local instances
        // REPLACE <ModelStub> with your Model
        //private Passero.Framework.Controls.XQBEForm<ModelStub> xQBEForm_myModel = new Passero.Framework.Controls.XQBEForm<ModelStub>();
        // Here we create an XQBEReport object for SSRS Report Display if you want manage class level. Otherwise you can create local instances
        //private Passero.Framework.Controls.XQBEReport xQBEReport = new Passero.Framework.Controls.XQBEReport();

        public frmPasseroBaseView()
        {
            InitializeComponent();
        }

        public void Init()
        {
            // Set DbConnection with you DbConnection Object
            // You can store it inside the ConfigurationManager 
            //this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["DbConnectionName"];
            myViewModel.Init(this.DbConnection);
            myViewModel.DataBindControlsAutoSetMaxLenght = true;
            myViewModel.AutoWriteControls = true;
            myViewModel.AutoReadControls = true;
            myViewModel.AutoFitColumnsLenght = true;
            // here we setting the Data Bind using a BindingSource so we can use a DataSource and visual binding at design time
            myViewModel.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            myViewModel.BindingSource = this.bindingSource1;

            // Set the DataNavigator1 ViewMode 
            
            // Questo Metodo invece sta nella classe base ViewModel
            // .GetAllItems() is a built-in methos of ViewModel Class
            this.myViewModel.GetAllItems();
            this.dataNavigator1.UpdateRecordLabel();

        }
        public void Reload()
        {
            this.myViewModel.GetAllItems();
            this.dataNavigator1.UpdateRecordLabel();

        }

        private void QBE_myModel( )
        {

            QBEForm<ModelStub>  xQBEForm_myModel = new QBEForm<ModelStub >(this.DbConnection);


            // Here we customize the Result Grid and the Query Grid adding Columns to display in Result and in Query Criteria
            //xQBEForm_myModel.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
            //xQBEForm_myModel.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
            //xQBEForm_myModel.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true, 20);
            //xQBEForm_myModel.QBEColumns.Add(nameof(Models.Author.contract), "Have contract", "", "", true, true, QBEColumnsTypes.CheckBox, 20);

            // Here we customize aspect of Result Grid
            //xQBEForm_myModel.QBEColumns["au_id"].ForeColor = System.Drawing.Color.Red;
            //xQBEForm_myModel.QBEColumns["au_id"].FontStyle = System.Drawing.FontStyle.Bold;
            //xQBEForm_myModel.QBEColumns["au_id"].FontSize = 10;
            //xQBEForm_myModel.QBEColumns[nameof(Models.Author.contract)].Aligment = DataGridViewContentAlignment.MiddleCenter ;

            // Here we Build the Result Grid and the Query Grid.
            xQBEForm_myModel.SetupQBEForm();
            // After the SetupQEBForm we have the ResultGrid Object (a DataGridView) and we can access/change any property
            //xQBEForm_myModel .ResultGrid.Columns["au_id"].DefaultCellStyle .BackColor = System.Drawing.Color.Magenta ;

            // Setting the QBEResultMode for XQBEForm
            //xQBEForm_myModel.QBEResultMode = QBEResultMode.BoundControls;
            //xQBEForm_myModel.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            //xQBEForm_myModel.QBEResultMode = QBEResultMode.AllRowsItems;
            //xQBEForm_myModel.QBEResultMode = QBEResultMode.MultipleRowsItems;

            // Setting the Owner Form 
            //xQBEForm_myModel.Owner = this;
            //xQBEForm_myModel.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEForm_myModel.CallBackAction = () => { methodtocall(); };

            // The TargetRepository must be defined when we use QBEResultMode other than BoundControls
            //xQBEForm_myModel.SetTargetRepository(this.myViewModel.Repository);
            // We can use even an Action to invoke a Method inside the View to reload the data in a custom mode. 
            //xQBEForm_myModel.SetTargetRepository(this.myViewModel.Repository,() => { methodtocall(); });


            // When whe return SQLQuery or Items  we need to map Model Properties from XQUEForm Model and the Target ViewModel
            //xQBEForm_myModel.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //xQBEForm_myModel.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));


            // If whe have a QBEResultMode.BoundControls  we need to map XQBEForm Model whit Controls inside the View Form
            //xQBEForm_myModel.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");


            // We can invoke the ShowQBE Method. This method can be Sync or Async using the Wait parameter
            // When Wait is true the flow control remain inside the current method 
            xQBEForm_myModel.ShowQBE();
            //xQBEForm_myModel.ShowQBE(true);
            //xQBEForm_myModel .ShowQBEWait() is a shortcut to ShowQBE(true)
            
            


        }

        private void QBEReport_base()
        {
            ReportManager xQBEReport = new ReportManager();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT ONE");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.myViewModel .Repository.DbConnection);
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.myViewModel .Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].Parameters.Add("@au_id", "1123");


            xQBEReport.QBEReports.Add("REPORT2", @"C:\Reports\REPORT1.RDL", "REPORT TWO");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.myViewModel.Repository.DbConnection, "SELECT * FROM Authors");

            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];

            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT2", nameof(Models.Author.au_id), "", "");

            //xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEReport.CallBackAction = () => { methodtocall(); };
            xQBEReport.ShowQBEReport(); 
        }

        private void frmPasseroBaseView_Load(object sender, EventArgs e)
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
