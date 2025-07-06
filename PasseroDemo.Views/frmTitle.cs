using System;
using System.Collections.Generic;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;
using System.Linq;
using Dapper;
using System.Data;
using System.Configuration;
namespace PasseroDemo.Views
{
    public partial class frmTitle : Form
    {

        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        private IDbConnection DbConnection;
        private ErrorNotificationMessageBox ErrorNotificationMessageBox= new ErrorNotificationMessageBox();
        public PasseroDemo.ViewModels.Title vmTitle = new ViewModels.Title();
        // Posso usare il viewmodel customizzato o usare un viewmodel generico
        //public PasseroDemo.ViewModels.TitleAuthor vmTitleAuthor = new ViewModels.TitleAuthor();
        //public PasseroDemo.ViewModels.Publisher  vmPublisher = new ViewModels.Publisher();

        public ViewModel<Models.Titleauthor> vmTitleAuthor = new ViewModel<Models.Titleauthor>();

        // Se devo solo fare query semplici sull'entità senza logica business e/o binding  posso anche usare direttamente un repository    
        public Repository<Models.Publisher> rpPublisher = new Repository<Models.Publisher>();
        public Repository<Models.Author> rpAuthor = new Repository<Models.Author>();

        QBEForm<Models.Title> xQBEForm_Title = new QBEForm<Models.Title>();
        Passero.Framework.SSRSReports.ReportManager xQBEReport = new Passero.Framework.SSRSReports.ReportManager();

        //private System.Data.SqlClient.SqlConnection sqlConnection;
        //private IEnumerable<PasseroDemo.Models.Title> title ;
        //private Repository<Models.Title > rtitle;
        public frmTitle()
        {
            InitializeComponent();
            this.dgv_TitleAuthors.AutoGenerateColumns = false;
            this.dgv_TitleAuthors .ReadOnly = true; 

        }


        public void Init()
        {
            this.DbConnection = ConfigurationManager.DBConnections["PasseroDemo"];

            vmTitle.Init(DbConnection);
            vmTitle.DataBindControlsAutoSetMaxLenght = true;
            vmTitle.AutoWriteControls = true;
            vmTitle.AutoReadControls = true;
            vmTitle.AutoFitColumnsLenght = true;
            //vmTitle.UseModelData = Passero.Framework.Base.UseModelData.InternalRepository;
            vmTitle.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmTitle.BindingSource = this.bsTitles;
            vmTitle.ErrorNotificationMessageBox = this.ErrorNotificationMessageBox;
            vmTitle.ErrorNotificationMode = ErrorNotificationModes.ShowDialog;

            vmTitleAuthor.Init(DbConnection);
            vmTitleAuthor .ErrorNotificationMessageBox = this.ErrorNotificationMessageBox;
            vmTitleAuthor.ErrorNotificationMode = ErrorNotificationModes.ShowDialog;

            //vmPublisher.Init(vmTitle.DbConnection);
            rpPublisher.DbConnection = this.DbConnection; ;
            rpAuthor.DbConnection = this.DbConnection;

            //this.cmb_pub_id.DataSource = vmPublisher.GetPublishers();
            //this.cmb_pub_id.DataSource = vmPublisher.GetItems($"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Publisher>()}",null).ToList();
            //this.cmb_pub_id.ValueMember = nameof(vmPublisher.Model.pub_id);
            //this.cmb_pub_id.DisplayMember = nameof(vmPublisher.Model.pub_name);

            this.cmb_pub_id.DataSource = rpPublisher.GetItems($"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Publisher>()}", null).Value.ToList();
            this.cmb_pub_id.ValueMember = nameof(rpPublisher.ModelItem.pub_id);
            this.cmb_pub_id.DisplayMember = nameof(rpPublisher.ModelItem.pub_name);

            //this.vmTitle.GetTitles ();
            this.vmTitle.GetAllItems();
            this.dataNavigator1.ViewModels["Title"]=new DataNavigatorViewModel(this.vmTitle,"Titles");
            this.dataNavigator1.ViewModels["TitleAuthor"]= new DataNavigatorViewModel(this.vmTitleAuthor,"TitleAuthoe", "Title Authors", this.dgv_TitleAuthors);
            this.dataNavigator1.SetActiveViewModel (this.dataNavigator1 .ViewModels["Title"]); 

            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = false;

        }

        public string GetAuthorFullName(string au_id)
        {

            
            Models .Author author = this.DbConnection.Query<Models.Author>("SELECT * FROM Authors WHERE au_id=@au_id", new { au_id = au_id }).First();
            //return $"{author.au_fname.Trim()} {author.au_lname.Trim()}".Trim();
            return author.au_fullname;
        }


        private void frmTitle_Load(object sender, EventArgs e)
        {
            this.Init();
        }



        public void Reload()
        {
            this.vmTitle.GetAllItems();

        }


        private void dataNavigator1_eMoveFirst()
        {
            this.dataNavigator1.ViewModel_MoveFirstItem();

        }

        private void dataNavigator1_eMoveNext()
        {
            this.dataNavigator1.ViewModel_MoveNextItem();

        }

        private void dataNavigator1_eMovePrevious()
        {
            this.dataNavigator1.ViewModel_MovePreviousItem();
        }

        private void dataNavigator1_eMoveLast()
        {
       
            this.dataNavigator1.ViewModel_MoveLastItem();

        }

        private void dataNavigator1_eSave()
        {

            this.dataNavigator1.ViewModel_UdpateItem();

            //if (this.dataNavigator1.ActiveViewModel == this.vmTitleAuthor)
            //{
            //    this.dataNavigator1.DataGrid_Save();
            //}
            //else
            //{
                
            //    this.vmTitle.UpdateItem();
            //}
        }

        private void dataNavigator1_eAddNew()
        {

            //if (this.dataNavigator1.ActiveViewModel.Equals(this.vmTitleAuthor) )
            if (this.dataNavigator1.ActiveViewModelIs(this.vmTitleAuthor) )
            //if (this.dataNavigator1.DataGridActive)
            {
                Models.Titleauthor titleauthor =new Models.Titleauthor();
                titleauthor.title_id = this.vmTitle.ModelItem.title_id;

                this.dataNavigator1.DataGrid_AddNew(titleauthor);
                //this.vmTitleAuthor .ModelItem .title_id = this.vmTitle .ModelItem .title_id;
                
            }
            else
            {
                this.vmTitle.AddNew();
               
            }
        }

        private void dataNavigator1_eUndo()
        {
            this.dataNavigator1.ViewModel_UndoChanges();

            //if (this.dataNavigator1.ActiveViewModel.Equals(this.vmTitleAuthor))
            //{
            //    this.dataNavigator1.DataGrid_Undo();
            //}
            //else
            //{
            //    this.vmTitle.UndoChanges();
            //}
        }

        private void tabTitle_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.pnl_Title.Parent = tabTitle.SelectedTab;
            this.dgv_TitleAuthors.Parent = tabTitle.SelectedTab;
            this.dgv_TitleAuthors.BringToFront();

            if (tabTitle .SelectedTab== tabPageTitles )
            {
                this.pnl_Title.Enabled = true; 
                this.dgv_TitleAuthors.ReadOnly = true;

                this.dataNavigator1.SetActiveViewModel(this.dataNavigator1.ViewModels["Title"]);
            }

            if (tabTitle.SelectedTab == tabPageTitleAuthors )
            {
                this.pnl_Title.Enabled = false;
                this.dgv_TitleAuthors.ReadOnly = false;
                this.dataNavigator1.SetActiveViewModel(this.dataNavigator1.ViewModels["TitleAuthor"]);
                //this.dataNavigator1.DataGridActive = true;
                //this.dataNavigator1.ViewModel = vmTitleAuthor;
            }


        }

        private void txt_title_id_TextChanged(object sender, EventArgs e)
        {
            //this.dgv_TitleAuthors.DataSource = this.vmTitle.GetTitleAuthors(this.txt_title_id.Text).ToList();
        }



        private void bsTitles_PositionChanged(object sender, EventArgs e)
        {
            //this.dgv_TitleAuthors.DataSource = this.vmTitle.GetTitleAuthors(this.txt_title_id.Text).ToList();
        }

        private void bsTitles_CurrentChanged(object sender, EventArgs e)
        {

            ExecutionResult < IList <Models .Titleauthor>> ER = this.vmTitleAuthor.GetItems($"Select * FROM " +
                                                $"{Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Titleauthor>()} " +
                                                $"WHERE title_id=@title_id", new { title_id = this.txt_title_id.Text });


            //this.dgv_TitleAuthors.DataSource = this.vmTitleAuthor.ModelItems;
            this.dgv_TitleAuthors.DataSource = ER.Value;


        }

        private void cmb_pub_id_ToolClick(object sender, ToolClickEventArgs e)
        {
            string sv = this.cmb_pub_id.SelectedValue.ToString();
            //this.cmb_pub_id .DataSource = vmPublisher.GetPublishers();
            //this.cmb_pub_id.DataSource = vmPublisher.GetItems($"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Publisher>()}", null).ToList();
            this.cmb_pub_id.DataSource = rpPublisher.GetItems($"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Publisher>()}", null).Value.ToList();
            this.cmb_pub_id.SelectedValue = sv;
        }

        private void dataNavigator1_eFind()
        {
            xQBEForm_Title = new QBEForm<Models.Title>(this.vmTitle.Repository.DbConnection);

            xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.title_id), "Title Id", "", "", true, true, 20);
            xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.title), "Title", "", "", true, true, 0);
            xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.pub_id), "Pub Id", "", "", true, true, 10);
            //xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.pubdate), "Pub Date", "", "", true, true, 10);


            xQBEForm_Title.SetupQBEForm();
            xQBEForm_Title.QBEResultMode = QBEResultMode.SingleRowSQLQuery;

            xQBEForm_Title.SetFocusControlAfterClose = this.txt_title_id;
            xQBEForm_Title.CallBackAction = () => { this.Reload(); };
            xQBEForm_Title.SetTargetRepository(this.vmTitle.Repository);


            // Da usare quando QBEResultMode = QBEResultMode .BoundControls 
            xQBEForm_Title.QBEBoundControls.Add(nameof(Models.Title), this.txt_title_id, "text");


            // Da usare quando QBEResultMode implica SQL
            xQBEForm_Title.QBEModelPropertiesMapping.Add(nameof(Models.Title.title_id), nameof(Models.Title.title_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));


            xQBEForm_Title.ShowQBE(false);
        }
        private void QBEAuthor_DataGridView(DataGridView DataGridView)
        {
            QBEForm<Models.Author> xQBEForm_Author = new QBEForm<Models.Author>(this.vmTitle.Repository.DbConnection);
            //using XQBEForm<Models.Author> xQBEForm_Author = new XQBEForm<Models.Author>(this.vmTitle.Repository.DbConnection);
            {
                xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_id), "", "", "", true, true, 20);
                xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_fname), "", "", "", true, true, 20);
                xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_lname), "", "", "", true, true, 20);
                xQBEForm_Author.QBEResultMode = QBEResultMode.BoundControls;
                xQBEForm_Author.SetFocusControlAfterClose = DataGridView.CurrentCell.Control;
                
                xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), DataGridView.CurrentCell.OwningRow[this.dgvc_au_id.Name], "Value");
                xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_fullname ), DataGridView.CurrentCell.OwningRow[this.dgvc_au_fullname.Name], "Value");
                xQBEForm_Author.Owner = this;
                xQBEForm_Author.ShowQBE(true);
            }
        }


        private void dgv_TitleAuthors_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (this.dgv_TitleAuthors.ReadOnly ==false && e.ColumnIndex == this.dgvc_qbe_authors.Index )
            {
                this.QBEAuthor_DataGridView(this.dgv_TitleAuthors);

            }
        }

        private void dgv_TitleAuthors_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow cRow in this.dgv_TitleAuthors.Rows)
            {
                if (cRow[this.dgvc_au_id.Name].Value != null)
                {
                    cRow[this.dgvc_au_fullname].Value = this.GetAuthorFullName(cRow[this.dgvc_au_id.Name].Value.ToString());
                }
            }
        }

        private void dataNavigator1_eDelete()
        {
            if (this.dataNavigator1 .DataGridActive)
            {
                this.dataNavigator1.DataGrid_Delete();
            }
            else
            {
                this.vmTitle.DeleteItem();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "";

            sql = Passero.Framework.DataBaseHelper.GetUpdateSqlCommand("Select * FROM Titles", (System.Data .SqlClient .SqlConnection )this.DbConnection);

        }

        private void tabPageTitles_PanelCollapsed(object sender, EventArgs e)
        {

        }
    }

}
