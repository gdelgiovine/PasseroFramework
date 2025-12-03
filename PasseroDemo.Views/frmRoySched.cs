using System;
using System.Data;
using Passero.Framework;
using Passero.Framework.Controls;
using Wisej.Web;

namespace PasseroDemo.Views
{
    public partial class frmRoySched : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }
        private ViewModels.vmTitle vmTitle = new  ViewModels.vmTitle ();    
        private ViewModels.vmRoysched  vmRoysched = new  ViewModels.vmRoysched ();

        public frmRoySched()
        {
            InitializeComponent();
        }
        private void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            vmTitle.Init(this.DbConnection);
            vmRoysched .Init(this.DbConnection);

            vmTitle.DataBindingMode = DataBindingMode.BindingSource;
            vmTitle .BindingSource = this.bsTitle ;


            this.dgvRoyalties.ReadOnly = true;

            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            //this.dataNavigator1.ViewModels["Title"] = new DataNavigatorViewModel(this.vmTitle , "Titles");
            //this.dataNavigator1.ViewModels["RoySched"] = new DataNavigatorViewModel(this.vmRoysched, "RoySched","",this.dgvRoyalties);
            //this.dataNavigator1.SetActiveViewModel("Title");


            this.dataNavigator1.AddViewModel(this.vmTitle , "Title");
            this.dataNavigator1.AddViewModel(this.vmRoysched , "Royalties", this.dgvRoyalties );
            this.dataNavigator1.SetActiveViewModel(this.vmTitle);


            this.dataNavigator1.Init(true);
            this.dataNavigator1.SetButtonsForReadOnly();
        }   


        private void Qbe_Title()
        {
            QBEForm<Models.Title> QBEForm = new QBEForm<Models.Title>(this.DbConnection);

            QBEForm.QBEColumns.Add(nameof(Models.Title.title_id), "Title Id", "", "", true, true, 20);
            QBEForm.QBEColumns.Add(nameof(Models.Title.title), "Title", "", "", true, true, 0);
            QBEForm.QBEColumns.Add(nameof(Models.Title.pub_id), "Pub Id", "", "", true, true, 10);
            //xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.pubdate), "Pub Date", "", "", true, true, 10);


            QBEForm.SetupQBEForm();
            QBEForm.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery ; 

            QBEForm.SetFocusControlAfterClose = this.txt_title_id;
            //QBEForm.CallBackAction = () => { this.dataNavigator1.Init(true); };
            QBEForm.SetTargetRepository(this.vmTitle.Repository, () => { this.dataNavigator1.Init(true); });


            // Da usare quando QBEResultMode = QBEResultMode .BoundControls 
            //QBEForm.QBEBoundControls.Add(nameof(Models.Title .title_id ), this.txt_title_id, nameof (txt_title_id .Text) );


            // Da usare quando QBEResultMode implica SQL
            QBEForm.QBEModelPropertiesMapping.Add(nameof(Models.Title.title_id), nameof(Models.Title.title_id));
            


            QBEForm.ShowQBE(false);


        }


        private void ManageTabSelection()
        {
    
            if (this.tabRoyalties.SelectedTab == this.tabPageRoyalties )
            {
                this.flowLayoutPanel1.Parent = this.tabPageRoyalties;
                this.dgvRoyalties.ReadOnly = false;
                this.dataNavigator1.SetActiveViewModel("RoySched");
                this.dataNavigator1.SetButtonsForReadWrite    ();  
            }
            else
            {
                this.flowLayoutPanel1.Parent = this.tabPageTitles;
                this.dataNavigator1.SetActiveViewModel("Title");
                this.dgvRoyalties.ReadOnly = true;
                this.dataNavigator1.SetButtonsForReadOnly ();
            }
        }   

        private void tabRoyalties_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManageTabSelection();
        }

        private void frmRoySched_Load(object sender, EventArgs e)
        {
            this.Init();    
        }

        private void txt_title_id_ToolClick(object sender, ToolClickEventArgs e)
        {
            Qbe_Title();

        }

        private void dataNavigator1_eBoundCompleted()
        {
            if (this.dataNavigator1.ActiveViewModelIs(vmTitle )  )
            {
                
               this.vmRoysched.GetRoysched(this.txt_title_id.Text);
               this.dgvRoyalties.DataSource = this.vmRoysched.ModelItems;
            }
        }

        private void dataNavigator1_eFind()
        {
            if (this.dataNavigator1.ActiveViewModelIs(vmTitle))
            {
                Qbe_Title();
            }   
        }

        private void dataNavigator1_eAddNewRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eAfterAddNewRequest(ref bool Cancel)
        {
            if (this.dataNavigator1.ActiveViewModelIs(vmRoysched))
            {
                this.dgvRoyalties.CurrentRow[dgvc_title_id ].Value   = this.txt_title_id.Text;
            }   
        }
    }
}
