using System;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;

namespace PasseroDemo.Views
{
    public partial class frmJobs : Form
    {

        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public Passero.Framework.ViewModel<Models.Job> vmJobs = new Passero.Framework.ViewModel<Models.Job>();
        private System.Data.SqlClient.SqlConnection DbConnection;

        public frmJobs()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];

            vmJobs.Init(this.DbConnection);
            //vmPublishers.DataBindControlsAutoSetMaxLenght = true;
            //vmPublishers.AutoWriteControls = true;
            //vmPublishers.AutoReadControls = true;
            //vmPublishers.AutoFitColumnsLenght = true;
            vmJobs.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs.BindingSource = this.bsJobs ;

            this.dataNavigator1.ViewModels["Jobs"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmJobs, "Jobs");
            this.dataNavigator1.SetActiveViewModel("Jobs");
            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            this.dataNavigator1.Init(false);

        }

        public void Reload()
        {
            this.dataNavigator1.Init(true);
        }
        private void frmJobs_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void QBEJobs()
        {
            QBEForm<Models.Job > QBE = new QBEForm<Models.Job>(this.DbConnection);


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
            QBE.SetFocusControlAfterClose = this.txt_job_id ;
            //xQBEForm_Author.CallBackAction = () => { this.Reload(); };
            QBE.SetTargetRepository(this.vmJobs.Repository, () => { this.Reload(); });
            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);


            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");
            QBE.QBEModelPropertiesMapping.Add(nameof(Models.Job .job_id), nameof(Models.Job.job_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));
            QBE.AutoLoadData = true;

            QBE.ShowQBE();
        }

        private void dataNavigator1_eMoveNextCompleted()
        {
            MessageBox.Show("a");

        }

        private void dataNavigator1_eFind()
        {
            this.QBEJobs ();

        }

      
    }
}
