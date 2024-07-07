using Passero.Framework;
using Passero.Framework.Controls;
using Passero.Framework.SSRSReports;
using System;
using Wisej.Web;


namespace PasseroDemo.Views
{
    public partial class frmDiscount : Wisej.Web.Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public System.Data.IDbConnection DbConnection { get; set; }
        private Passero.Framework.ViewModel<Models.Discount  > vmDiscount = new Passero.Framework.ViewModel<Models.Discount>("Discounts");
        //private Passero.Framework.ViewModel<ModelStub> myViewModelStub = new Passero.Framework.ViewModel<ModelStub>();
        private Passero.Framework.Controls.QBEForm<Models.Discount > QBEForm_Discount = new Passero.Framework.Controls.QBEForm<Models.Discount >();
        private Passero.Framework.SSRSReports.ReportManager xQBEReport = new Passero.Framework.SSRSReports.ReportManager();

        public frmDiscount()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            vmDiscount.Init(this.DbConnection);
            vmDiscount.OwnerView = this;
            vmDiscount.DataBindControlsAutoSetMaxLenght = true;
            vmDiscount.AutoWriteControls = true;
            vmDiscount.AutoReadControls = true;
            vmDiscount.AutoFitColumnsLenght = true;
            vmDiscount.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmDiscount.BindingSource = this.bsDiscount;
            
            
            this.txt_Discount_store_id.DbConnection = this.DbConnection;
            this.txt_Discount_store_id.ModelClass = typeof(Models.Store);
            //this.txt_Discount_store_id.DisplayMember = nameof(Models.Store.stor_id);
            //this.txt_Discount_store_id.DisplayMember = nameof(Models.Store.mcode);
            this.txt_Discount_store_id.ValueMember = nameof(Models.Store.stor_id);
            this.txt_Discount_store_id.LookUpMode = DbLookUpTextBox.LookUpModes.Standard  ;
            this.txt_Discount_store_id.LookUpOnEdit = false;    
            this.txt_Discount_store_id.AddControl(txt_Store_stor_name, "text", nameof(Models.Store.stor_name));
            this.txt_Discount_store_id.AddControl(txt_Store_stor_city, "text", nameof(Models.Store.city));
            ////this.txt_Discount_store_id.SelectClause = "SELECT *, TRIM(au_fname)+' '+TRIM(au_lname) as au_fullname";


            
            //vmDiscount.DataBindingMode = Passero.Framework.DataBindingMode.Passero;
            //Passero.Framework.ControlsUtilities.BindingSourceControls(this, this.bsDiscount);

            this.vmDiscount.CreatePasseroBindingFromBindingSource(this);


            // Questo Metodo sta nel ViewModel Customizzato
            //this.vmAuthor.GetAuthors();
            // Questo Metodo invece sta nella classe base ViewModel
            this.vmDiscount.GetAllItems();
            
            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ViewModels["Discount"] = new DataNavigatorViewModel(this.vmDiscount,"Discount","Discount");
            this.dataNavigator1.SetActiveViewModel("Discount");

            
           
        }


        private void QBE_Discount()
        {

            QBEForm_Discount = new QBEForm<Models.Discount >(this.DbConnection);

            
            
            QBEForm_Discount.SetupQBEForm();
            
            //xQBEForm_Author.QBEResultMode = QBEResultMode.BoundControls;
            //xQBEForm_Discount.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.AllRowsItems;
            QBEForm_Discount.QBEResultMode = QBEResultMode.MultipleRowsItems;
            QBEForm_Discount.Owner = this;
            //xQBEForm_Author.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEForm_Author.CallBackAction = () => { this.Reload(); };

            QBEForm_Discount.SetTargetRepository(this.vmDiscount.Repository,() => { this.Reload(); });
            //xQBEForm_Discount.SetTargetRepository(this.vmDiscount.Repository);


            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");
            QBEForm_Discount.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));

            QBEForm_Discount.Text = "Search Discount";
            QBEForm_Discount.ShowQBE();

        }
        public void Reload()
        {
            this.vmDiscount.GetAllItems();
            this.dataNavigator1.UpdateRecordLabel();

        }
        private void QBEReport_base()
        {
            ReportManager xQBEReport = new ReportManager();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT ONE");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.vmDiscount.Repository.DbConnection);
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.vmDiscount.Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].Parameters.Add("@au_id", "1123");


            xQBEReport.QBEReports.Add("REPORT2", @"C:\Reports\REPORT1.RDL", "REPORT TWO");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.vmDiscount.Repository.DbConnection, "SELECT * FROM Authors");

            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];

            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT2", nameof(Models.Author.au_id), "", "");
            xQBEReport.Owner = this;
            //xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            //xQBEReport.CallBackAction = () => { this.Reload(); };
            xQBEReport.ShowQBEReport();
        }

        private void frmPasseroBaseView_Load(object sender, EventArgs e)
        {
            this.Init();
        }


        #region dataNavigator1_eEvents
        private void dataNavigator1_eAddNew()
        {

         
                this.vmDiscount.AddNew();
         
        }

        private void dataNavigator1_eBoundCompleted()
        {

        }

        private void dataNavigator1_eClose()
        {

        }

        private void dataNavigator1_eDelete()
        {
            this.vmDiscount.DeleteItem();
        }

        private void dataNavigator1_eFind()
        {
            this.QBE_Discount();
        }

        private void dataNavigator1_eMoveFirst()
        {
            this.vmDiscount.MoveFirstItem();

        }

        private void dataNavigator1_eMoveLast()
        {
            this.vmDiscount.MoveLastItem();
        }

        private void dataNavigator1_eMoveNext()
        {
            this.vmDiscount.MoveNextItem();
        }

        private void dataNavigator1_eMovePrevious()
        {
            this.vmDiscount.MovePreviousItem();
        }

        private void dataNavigator1_ePrint()
        {

        }

        private void dataNavigator1_eRefresh()
        {

        }

        private void dataNavigator1_eSave()
        {
           
                this.vmDiscount.UpdateItem();

        }

        private void dataNavigator1_eUndo()
        {
           
                this.vmDiscount.UndoChanges();
           
        }
        #endregion

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void txt_Discount_store_id_TextChanged(object sender, EventArgs e)
        {
            int i = 0;

        }

        private void bsDiscount_AddingNew(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            int i = 0;
            //this.txt_Discount_store_id.Value = "TOCA";
            //this.txt_Discount_store_id.Text = "STOCAZZ";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataNavigator1.AddNew();
            //this.txt_Discount_store_id.ClearControls();
            //this.txt_Discount_store_id.LookUp();

        }

        private void dataNavigator1_eDeleteRequest(ref bool Cancel)
        {
            MessageBox.Show($"Richiesta Cancellazione!! Item {this.dataNavigator1.CurrentModelItemIndex+1} of {this.dataNavigator1.ModelItemsCount}");
        }

        private void dataNavigator1_eAddNewCompleted()
        {
            MessageBox.Show("AddNew Completed");
        }
    }
}
