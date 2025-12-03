using System;
using Wisej.Web;
using System.Data;
using Passero.Framework.Controls;
using Passero.Framework.Base;

namespace PasseroDemo.Views
{
    public partial class frmPublishers : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public Passero.Framework.ViewModel<Models.Publisher> vmPublishers = new Passero.Framework.ViewModel<Models.Publisher>();
        private System.Data.SqlClient.SqlConnection DbConnection;
        private Microsoft.Data.SqlClient.SqlConnection DbConnectionA;
        public frmPublishers()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            this.DbConnectionA = new Microsoft.Data.SqlClient.SqlConnection(ConfigurationManager.DBConnections["PasseroDemo"].ConnectionString);


            vmPublishers.Init(this.DbConnection);
            //vmPublishers.DataBindControlsAutoSetMaxLenght = true;
            //vmPublishers.AutoWriteControls = true;
            //vmPublishers.AutoReadControls = true;
            //vmPublishers.AutoFitColumnsLenght = true;
            vmPublishers.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmPublishers.BindingSource = this.bsPublishers;

            // Old way to add ViewModel to DataNavigator    
            //this.dataNavigator1.ViewModels["vmPublishers"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmPublishers, "vmPublishers");
            //this.dataNavigator1 .AddViewModel("vmPublishers", this.vmPublishers ,"Publishers", null,null );
            //this.dataNavigator1.SetActiveViewModel("vmPublishers");
            //this.dataNavigator1.SetActiveViewModel(nameof (this.vmPublishers));

            // New way to add ViewModel to DataNavigator    
            this.dataNavigator1.AddViewModel(this.vmPublishers, "Publishers", null, null);
            this.dataNavigator1.SetActiveViewModel(this.vmPublishers);

            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            this.dataNavigator1.Init(true);
            
        }

        public void Reload()
        {
            this.dataNavigator1.Init(true);
        }
        private void frmPublishers_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void QBEPublishers()
        {
            QBEForm<Models.Publisher> QBE = new QBEForm<Models.Publisher>(this.DbConnection);
            
            
            //QBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
            //QBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
            //QBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true, 20);
            //QBEForm_Author.QBEColumns.Add(nameof(Models.Author.contract), "Have contract", "", "", true, true, Passero.Framework.Controls.QBEColumnsTypes.CheckBox, 20);
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
            QBE.SetFocusControlAfterClose = this.txt_Publishers_pub_id ;    


            //xQBEForm_Author.CallBackAction = () => { this.Reload(); };

            //QBE.SetTargetRepository(this.vmPublishers.Repository, () => { this.Reload(); });
            QBE.SetTargetViewModel(this.vmPublishers, () => { this.Reload(); });

            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);
            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");

            QBE.QBEModelPropertiesMapping.Add(nameof(Models.Publisher.pub_id ), nameof(Models.Publisher .pub_id));

            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));

            QBE.AutoLoadData = true;
            
            QBE.ShowQBE();
        }

        private void dataNavigator1_eMoveNextCompleted()
        {
                    
        }

        private void dataNavigator1_eFind()
        {
            this.QBEPublishers ();

        }

        private void dataNavigator1_eAddNew()
        {
            MessageBox.Show("AddNew");
            this.dataNavigator1.ViewModel_AddNew();  
        }

        private void dataNavigator1_eAddNewCompleted()
        {
            MessageBox.Show("AddNewCompleted");
        }

        private void dataNavigator1_eAddNewRequest(ref bool Cancel)
        {
            if (MessageBox.Show("AddNewRequest",this.Text , MessageBoxButtons.YesNo)==  DialogResult.No)
                 Cancel = true;
        }

        private void dataNavigator1_eAfterAddNewRequest()
        {
            MessageBox.Show("AfterAddNewRequest");
        }

        private void dataNavigator1_eUndo()
        {
            MessageBox.Show("Undo");
            this.dataNavigator1.ViewModel_UndoChanges ();
        }

        private void dataNavigator1_eUndoCompleted()
        {
            MessageBox.Show("UndoCompleted");
        }

        private void dataNavigator1_eUndoRequest(ref bool Cancel)
        {
            if (MessageBox.Show("UndoRequest", this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
                Cancel = true;
            
        }

        private void dataNavigator1_eSave()
        {
            MessageBox.Show("Save");
            this.dataNavigator1.ViewModel_UdpateItem();
        }

        private void dataNavigator1_eSaveCompleted()
        {
            MessageBox.Show("SaveCompleted");
        }

        private void dataNavigator1_eSaveRequest(ref bool Cancel)
        {
            if (MessageBox.Show("SaveRequest", this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
                Cancel = true;
        }

        private void dataNavigator1_eDelete()
        {
            MessageBox.Show("Delete");
            this.dataNavigator1.ViewModel_DeleteItem();
        }

        private void dataNavigator1_eDeleteCompleted()
        {
            MessageBox.Show("DeleteCompleted");
        }

        private void dataNavigator1_eDeleteRequest(ref bool Cancel)
        {
            if (MessageBox.Show("DeleteRequest", this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
                Cancel = true;
        }
    }

}
