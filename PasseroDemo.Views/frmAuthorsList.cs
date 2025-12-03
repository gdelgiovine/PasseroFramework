
using Dapper;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json.Linq;
using Passero.Framework;
using PasseroDemo.Models;
using PasseroDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting;
using Wisej.Web;


namespace PasseroDemo.Views
{
    public partial class frmAuthorsList : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        private System.Data.SqlClient.SqlConnection sqlConnection;
        private IEnumerable<PasseroDemo.Models.Author> authors;
        private ViewModel<Models.Author> vmAuthor = new ViewModel<Author>();
        public frmAuthorsList()
        {
            InitializeComponent();
        }

        public void LoadAuthors()
        {
          
            string cs = this.ConfigurationManager.GetSessionConfigurationKeyValue("General", "DBConnectionString");

            sqlConnection = new System.Data.SqlClient.SqlConnection(cs);
            vmAuthor.DbConnection =(sqlConnection);

            //authors = sqlConnection.GetAll<Models .Author >();
            vmAuthor.GetAllItems();
            this.dataGridView1.AutoGenerateColumns = false;

            //this.dataGridView1.DataSource = authors;

            //this.dataNavigator1.ViewModels["Author"] = new Passero.Framework .Controls .DataNavigatorViewModel(this.vmAuthor, "Author","",this.dataGridView1,null);
            //this.dataNavigator1.SetActiveViewModel("Author");
            this.dataNavigator1.AddViewModel(this.vmAuthor,"Authors",this.dataGridView1 );
            this.dataNavigator1.SetActiveViewModel(this.vmAuthor);




        }
        private void frmAuthorsList_Load(object sender, EventArgs e)
        {

            this.LoadAuthors();

           
            
        }

        private void dataNavigator1_eAddNew()
        {

            IEnumerable<Models .Author > authors;

            authors = (IEnumerable<Author>)this.dataNavigator1.ModelItems;


            MessageBox.Show("ADDNEW");
        }

        private void dataNavigator1_eMoveFirst()
        {
            if (this.dataNavigator1.DataGridView !=null && this.dataNavigator1 .DataGridActive ) 
            {

                this.dataNavigator1.DataGrid_MoveFirst();
            }
        }

        private void dataNavigator1_eMoveLast()
        {
            if (this.dataNavigator1.DataGridView != null && this.dataNavigator1.DataGridActive)
            {

                this.dataNavigator1.DataGrid_MoveLast();
            }
        }

        private void dataNavigator1_eMovePrevious()
        {
            if (this.dataNavigator1.DataGridView != null && this.dataNavigator1.DataGridActive)
            {

                this.dataNavigator1.DataGrid_MovePrevious();
            }

        }

        private void dataNavigator1_eMoveNext()
        {
            if (this.dataNavigator1.DataGridView != null && this.dataNavigator1.DataGridActive)
            {

                this.dataNavigator1.DataGrid_MoveNext();
            }

        }

        private void dataNavigator1_eSave()
        {
            if (this.dataNavigator1.DataGridView != null && this.dataNavigator1.DataGridActive)
            {
                //this.dataGridView1.EndEdit();

                this.dataGridView1.EndEdit();
                //this.rAuthor.UpdateItem((Models.Author)this.dataGridView1.CurrentRow.DataBoundItem);
                //this.vmAuthor.UpdateItems(this.authors);

                //this.dataNavigator1.DataGrid_Save ();
                
            }

        }

        private void dataNavigator1_eRefresh()
        {
            this.LoadAuthors();
        }
    }
}
