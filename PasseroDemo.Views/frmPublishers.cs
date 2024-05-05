using System;
using Wisej.Web;
using System.Data;

namespace PasseroDemo.Views
{
    public partial class frmPublishers : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public Passero.Framework.ViewModel<Models.Publisher> vmPublishers = new Passero.Framework.ViewModel<Models.Publisher>();
        private System.Data.SqlClient.SqlConnection DbConnection;

        public frmPublishers()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];

            vmPublishers.Init(this.DbConnection);
            vmPublishers.DataBindControlsAutoSetMaxLenght = true;
            vmPublishers.AutoWriteControls = true;
            vmPublishers.AutoReadControls = true;
            vmPublishers.AutoFitColumnsLenght = true;
            vmPublishers.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmPublishers.BindingSource = this.bsPublishers;

            this.dataNavigator1.ViewModels["Publishers"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmPublishers, "Publishers");
            this.dataNavigator1.SetActiveViewModel("Publishers");
            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            this.dataNavigator1.Init(true);

        }

        private void frmPublishers_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void dataNavigator1_eMoveNextCompleted()
        {
            MessageBox.Show("a");
        }
    }
}
