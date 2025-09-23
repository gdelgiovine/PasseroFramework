using System;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;
using PasseroDemo.ViewModels;

namespace PasseroDemo.Views
{
    public partial class frmJobs : Form
    {

        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public Passero.Framework.ViewModel<Models.Job> vmJobs2 = new Passero.Framework.ViewModel<Models.Job>();
        //public vmTEST vmJobs = new vmTEST();
        public ViewModels.vmJobs vmJobs = new vmJobs();
        private System.Data.SqlClient.SqlConnection DbConnection;

        public frmJobs()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];

            vmJobs.Init(this.DbConnection);
            vmJobs.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs.BindingSource = this.bsJobs;

            vmJobs.Jobs.Init(this.DbConnection);
            vmJobs.Jobs.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs.Jobs.BindingSource = this.bsJobs;

            vmJobs2.Init(this.DbConnection);
            vmJobs2.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs2.BindingSource = this.bsJobs;

            vmJobs.AddViewModel<Models.Job>("vmJobs2",vmJobs2);

            
            ViewModel<Models.Job> vm = vmJobs.GetViewModel("vmJobs2");

            //this.dataNavigator1.ViewModels["Jobs"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmJobs.Jobs , "Jobs");

            this.dataNavigator1.ViewModels["Jobs"] = new Passero.Framework.Controls.DataNavigatorViewModel(vm, "Jobs");

            this.dataNavigator1.SetActiveViewModel("Jobs");
            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            this.dataNavigator1.Init(true);

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
            //QBE.SetTargetRepository(this.vmJobs.DefaultViewModel.Repository, () => { this.Reload(); });
            QBE.SetTargetRepository((Repository<Models.Job>)this.vmJobs.Repository, () => { this.Reload(); });
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
