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
        public PasseroDemo.ViewModels.vmTitle vmTitle = new ViewModels.vmTitle();
        // Posso usare il viewmodel customizzato o usare un viewmodel generico
        //public PasseroDemo.ViewModels.TitleAuthor vmTitleAuthor = new ViewModels.TitleAuthor();
        public PasseroDemo.ViewModels.vmPublisher vmPublisher = new ViewModels.vmPublisher();

        public ViewModel<Models.Titleauthor> vmTitleAuthor = new ViewModel<Models.Titleauthor>();

        // Se devo solo fare query semplici sull'entità senza logica business e/o binding  posso anche usare direttamente un repository    
        //public Repository<Models.Publisher> rpPublisher = new Repository<Models.Publisher>();
        //public Repository<Models.Author> rpAuthor = new Repository<Models.Author>();

        //Posso usare un QBEForm qui per gestire i relativi eventi
        //QBEForm<Models.Title> xQBEForm_Title = new QBEForm<Models.Title>();

        // Oppure posso usare un ReportManager per gestire i report SSRS basati su QBE  
        Passero.Framework.SSRSReports.ReportManager xQBEReport = new Passero.Framework.SSRSReports.ReportManager();

        public frmTitle()
        {
            InitializeComponent();
            this.dgv_TitleAuthors.AutoGenerateColumns = false;
            this.dgv_TitleAuthors .ReadOnly = true;
            this.pnl_TitleInfo.Dock = DockStyle.Fill;    


        }


        public void Init()
        {
            this.DbConnection = ConfigurationManager.DBConnections["PasseroDemo"];

            vmTitle.Init(DbConnection);
            vmTitle.DataBindControlsAutoSetMaxLenght = true;
            vmTitle.AutoWriteControls = true;
            vmTitle.AutoReadControls = true;
            vmTitle.AutoFitColumnsLenght = true;
            vmTitle.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmTitle.BindingSource = this.bsTitles;
            vmTitle.ErrorNotificationMessageBox = this.ErrorNotificationMessageBox;
            vmTitle.ErrorNotificationMode = ErrorNotificationModes.ShowDialog;

            vmTitleAuthor.Init(DbConnection);
            vmTitleAuthor .ErrorNotificationMessageBox = this.ErrorNotificationMessageBox;
            vmTitleAuthor.ErrorNotificationMode = ErrorNotificationModes.ShowDialog;

            vmPublisher.Init(vmTitle.DbConnection);
            //rpPublisher.DbConnection = this.DbConnection; ;
            //rpAuthor.DbConnection = this.DbConnection;


            //this.cmb_pub_id.DataSource = rpPublisher.GetItems($"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Publisher>()}", null).Value.ToList();
            this.cmb_pub_id.DataSource = vmPublisher.GetPublishers();
            this.cmb_pub_id.ValueMember = nameof(vmPublisher.ModelItem.pub_id);
            this.cmb_pub_id.DisplayMember = nameof(vmPublisher.ModelItem.pub_name );
            
            //this.dataNavigator1.ViewModels["Title"]=new DataNavigatorViewModel(this.vmTitle,"Titles");
            //this.dataNavigator1.ViewModels["TitleAuthor"]= new DataNavigatorViewModel(this.vmTitleAuthor,"TitleAuthoe", "Title Authors", this.dgv_TitleAuthors);
            //this.dataNavigator1.SetActiveViewModel ("Title"); 


            this.dataNavigator1.AddViewModel (this.vmTitle,"Titles");
            this.dataNavigator1.AddViewModel(this.vmTitleAuthor , "Title Authors", this.dgv_TitleAuthors );
            this.dataNavigator1 .SetActiveViewModel (this.vmTitle );    

            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = false;

            this.dataNavigator1.Init(true);

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

          
            if (this.dataNavigator1.ActiveViewModelIs(this.vmTitleAuthor) )
            {
                Models.Titleauthor titleauthor =new Models.Titleauthor();
                titleauthor.title_id = this.vmTitle.ModelItem.title_id;
                this.dataNavigator1.DataGrid_AddNew(titleauthor);
            }
            else
            {
                this.vmTitle.AddNew();
            }
        }

        private void dataNavigator1_eUndo()
        {
            this.dataNavigator1.ViewModel_UndoChanges();

          
        }

        private void tabTitle_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.pnl_Title.Parent = tabctlTitle.SelectedTab;
           

            if (tabctlTitle .SelectedTab== tabPageTitles )
            {
                this.pnl_TitleInfo.Enabled = true; 
                this.dgv_TitleAuthors.ReadOnly = true;

                
                this.dataNavigator1.SetActiveViewModel (this.vmTitle);  
            }

            if (tabctlTitle.SelectedTab == tabPageTitleAuthors )
            {
                this.pnl_TitleInfo.Enabled = false;
                this.dgv_TitleAuthors.ReadOnly = false;
                this.dataNavigator1.SetActiveViewModel(this.vmTitleAuthor );
                
                
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

            this.pnl_Title.Text= $"Title: {this.txt_title_id.Text} - {this.txt_title .Text}";   
            ExecutionResult < IList <Models .Titleauthor>> ER = this.vmTitleAuthor.GetItems($"Select * FROM " +
                                                $"{Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Titleauthor>()} " +
                                                $"WHERE title_id=@title_id", new { title_id = this.txt_title_id.Text });


            //this.dgv_TitleAuthors.DataSource = this.vmTitleAuthor.ModelItems;
            this.dgv_TitleAuthors.DataSource = ER.Value;



        }

        private void cmb_pub_id_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "refresh")
            {
                string sv = this.cmb_pub_id.SelectedValue.ToString();
                this.cmb_pub_id.DataSource = vmPublisher.GetPublishers();
                this.cmb_pub_id.SelectedValue = sv;
            }
        }


        private void QBE_Title()
        {
            QBEForm<Models.Title> QBEForm = new QBEForm<Models.Title>(this.DbConnection);

            QBEForm.QBEColumns.Add(nameof(Models.Title.title_id), "Title Id", "", "", true, true, 20);
            QBEForm.QBEColumns.Add(nameof(Models.Title.title), "Title", "", "", true, true, 0);
            QBEForm.QBEColumns.Add(nameof(Models.Title.pub_id), "Pub Id", "", "", true, true, 10);
            //xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.pubdate), "Pub Date", "", "", true, true, 10);


            QBEForm.SetupQBEForm();
            QBEForm.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery ;

            QBEForm.SetFocusControlAfterClose = this.txt_title_id;
            QBEForm.SetTargetViewModel(this.vmTitle, () => { this.dataNavigator1.Init(true); });


            // Da usare quando QBEResultMode = QBEResultMode .BoundControls 
            QBEForm.QBEBoundControls.Add(nameof(Models.Title), this.txt_title_id, "text");


            // Da usare quando QBEResultMode implica SQL
            QBEForm.QBEModelPropertiesMapping.Add(nameof(Models.Title.title_id), nameof(Models.Title.title_id));
         

            QBEForm.ShowQBE(false);
        }   
        private void dataNavigator1_eFind()
        {
          QBE_Title (); 
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

       
        private void tabPageTitles_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void xResize()
        {
            this.dgv_TitleAuthors.Height = this.dgv_TitleAuthors.Parent.Height - this.dgv_TitleAuthors.Top - this.pnl_Title.HeaderSize;
        }
        private void frmTitle_Resize(object sender, EventArgs e)
        {
            
             xResize();
        }

        private void pnl_Title_ToolClick(object sender, ToolClickEventArgs e)
        {

            this.pnl_TitleInfo.Visible = !this.pnl_TitleInfo.Visible;
            xResize();

          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Update (); 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.dataNavigator1 .Visible= !this.dataNavigator1 .Visible;    
        }

        private void dataNavigator1_Minimized()
        {
            this.tabctlTitle.Height = this.Height - this.tabctlTitle.Top - this.pnl_Title.Top - this.dataNavigator1.Height;
        }

        private void dataNavigator1_Maximized()
        {
            this.tabctlTitle.Height = this.Height - this.tabctlTitle.Top - this.pnl_Title.Top - this.dataNavigator1.Height;
        }

        private void dataNavigator1_eDeleteCompleted()
        {

        }

        private void dataNavigator1_eSaveCompleted()
        {
            // Viene invocato dopo la save 

        }

        private void dataNavigator1_eSaveRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eDeleteRequest(ref bool Cancel)
        {
            //Viene invocato prima della delete 
            if (this.dataNavigator1 .ActiveViewModelIs(this.vmTitle))
            {
                if (this.vmTitleAuthor.ModelItemsCount>0)
                {
                    MessageBox.Show("Cannot delete this title because it has related Title Authors.","Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Cancel = true;  
                    return;
                }
                if (MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Cancel = true;
                }

            }
            
            if (dataNavigator1 .ActiveViewModelIs (this.vmTitleAuthor ))
            {
                if (MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Cancel = true;
                }
            }
            
        }
    }

}
