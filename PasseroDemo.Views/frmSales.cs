using System;
using Wisej.Web;
using System.Data;
using Passero.Framework.Controls;
using Passero.Framework;
using PasseroDemo.ViewModels;
using System.IdentityModel.Tokens;

namespace PasseroDemo.Views
{
    public partial class frmSales : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }
        private ViewModels.vmSalesmaster vmSalesmaster = new ViewModels.vmSalesmaster();
        private ViewModels.vmSalesdetail vmSalesdetail = new ViewModels.vmSalesdetail();
        private ViewModels.vmStore vmStore = new ViewModels.vmStore();
        private ViewModels.vmTitle vmTitle = new ViewModels.vmTitle();  
        
        private DbLookUp<Models.Store> dbLookUpStores = new DbLookUp<Models.Store>();
        public frmSales()
        {
            InitializeComponent();
        }

        private void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            vmSalesmaster.Init(this.DbConnection);
            vmSalesdetail.Init(this.DbConnection);
            vmStore.Init(this.DbConnection);
            vmTitle.Init(this.DbConnection);
            vmSalesmaster.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmSalesmaster.BindingSource = this.bsSalesmaster;

            dbLookUpStores.DbConnection = this.DbConnection;
            // qui possimao usare una lamba expression (LookUpFunction) per ottenere il valore della chiave esterna
            dbLookUpStores.LookUpFunction = () => this.vmStore.GetStore(this.txt_stor_id .Text);


            // il data binding mode puo essere Passero o BindingSource  
            dbLookUpStores.DataBindingMode = DataBindingMode.Passero;
            //dblAuthor.BindingSource = this.bindingSource1;
            dbLookUpStores.AddControl(this.txt_stor_name, "text", nameof(dbLookUpStores.Model.stor_name));



            this.dgv_SalesDetails.ReadOnly = true;
            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            this.dataNavigator1.ViewModels["Salesmaster"] = new DataNavigatorViewModel(this.vmSalesmaster, "Salesmaster");
            this.dataNavigator1.ViewModels["Salesdetail"] = new DataNavigatorViewModel(this.vmSalesdetail, "Salesdetail", "", this.dgv_SalesDetails);
            this.dataNavigator1.SetActiveViewModel("Salesmaster");
            this.dataNavigator1.Init(true);

        }

        public void Reload()
        {
            //this.vmSalesmaster.GetAllItems();
            this.dataNavigator1.Init(true);
        }
        private void QBE_Salesmaster()
        {
            QBEForm<Models.Salesmaster> QBEForm = new QBEForm<Models.Salesmaster>(this.DbConnection);
            QBEForm.QBEColumns.Add(nameof(Models.Salesmaster.ord_num), "Order Number", "", "", true, true, 20);
            QBEForm.QBEColumns.Add(nameof(Models.Salesmaster.ord_date), "Order Date", "", "", true, true, 10);
            QBEForm.QBEColumns.Add(nameof(Models.Salesmaster.stor_id), "Store ID", "", "", true, true, 10);

            QBEForm.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;

            QBEForm.Owner = this;
            QBEForm.SetFocusControlAfterClose = this.dataNavigator1;
            QBEForm.SetTargetRepository(this.vmSalesmaster.Repository, () => { this.Reload(); });
            //QBEForm.SetTargetRepository(this.vmSalesmaster.Repository );

            // Da usare quando QBEResultMode = QBEResultMode .BoundControls 
            //QBEForm.QBEBoundControls.Add(nameof(Models.Title .title_id ), this.txt_title_id, nameof (txt_title_id .Text) );

            // Da usare quando QBEResultMode implica SQL
            QBEForm.QBEModelPropertiesMapping.Add(nameof(Models.Salesmaster.ord_num), nameof(Models.Salesmaster.ord_num));
            QBEForm.AutoLoadData = true;
            QBEForm.ShowQBE();


        }

        private void QBE_Title()
        {
            QBEForm<Models.Title> QBEForm = new QBEForm<Models.Title>(this.DbConnection);

            QBEForm.QBEColumns.Add(nameof(Models.Title.title_id), "Title Id", "", "", true, true, 20);
            QBEForm.QBEColumns.Add(nameof(Models.Title.title), "Title", "", "", true, true, 0);
            QBEForm.QBEColumns.Add(nameof(Models.Title.pub_id), "Pub Id", "", "", true, true, 10);
            //xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.pubdate), "Pub Date", "", "", true, true, 10);


            QBEForm.SetupQBEForm();
            QBEForm.QBEResultMode = QBEResultMode.BoundControls;

            //QBEForm.SetFocusControlAfterClose = this.txt_title_id;
            //QBEForm.SetTargetViewModel(this.vmTitle, () => { this.dataNavigator1.Init(true); });


            // Da usare quando QBEResultMode = QBEResultMode .BoundControls 
            QBEForm.QBEBoundControls.Add(nameof(Models.Title.title_id), this.dgv_SalesDetails.CurrentRow[this.dgvc_title_id], "Value");
            QBEForm.QBEBoundControls.Add(nameof(Models.Title.title ), this.dgv_SalesDetails.CurrentRow[this.dgvc_title], "Value");

            // Da usare quando QBEResultMode implica SQL
            //QBEForm.QBEModelPropertiesMapping.Add(nameof(Models.Title.title_id), nameof(Models.Title.title_id));


            QBEForm.ShowQBE(false);
        }


        private void QBE_Store()
        {
            QBEForm<Models.Store> QBEForm = new QBEForm<Models.Store>(this.DbConnection);

            QBEForm.QBEColumns.Add(nameof(Models.Store .stor_id ), "Store Id", "", "", true, true, -1);
            QBEForm.QBEColumns.Add(nameof(Models.Store.stor_name ), "Store Name", "", "", true, true, -1);
            QBEForm.QBEColumns.Add(nameof(Models.Store.city), "City", "", "", true, true, -1);
            QBEForm.QBEColumns.Add(nameof(Models.Store.stor_address), "Address", "", "", true, true,-1);
            QBEForm.QBEColumns.Add(nameof(Models.Store.zip ), "Zip", "", "", true, true, -1);
            QBEForm.QBEColumns.Add(nameof(Models.Store.state ), "State", "", "", true, true, -1);

            //xQBEForm_Title.QBEColumns.Add(nameof(Models.Title.pubdate), "Pub Date", "", "", true, true, 10);


            QBEForm.SetupQBEForm();
            QBEForm.QBEResultMode = QBEResultMode.BoundControls;

            //QBEForm.SetFocusControlAfterClose = this.txt_title_id;
            //QBEForm.SetTargetViewModel(this.vmTitle, () => { this.dataNavigator1.Init(true); });


            // Da usare quando QBEResultMode = QBEResultMode .BoundControls 
            QBEForm.QBEBoundControls.Add(nameof(Models.Store.stor_id), this.txt_stor_id,"text");


            // Da usare quando QBEResultMode implica SQL
            //QBEForm.QBEModelPropertiesMapping.Add(nameof(Models.Title.title_id), nameof(Models.Title.title_id));


            QBEForm.ShowQBE(false);
        }

        private void frmSales_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void dataNavigator1_eBoundCompleted()
        {
            this.vmSalesdetail.GetSalesdetail(this.vmSalesmaster.ModelItem.ord_num);
            //this.dbLookUpStores.Lookup();
            this.dgv_SalesDetails.DataSource = this.vmSalesdetail.ModelItems;

        }

        private void dataNavigator1_eFind()
        {
            if (this.dataNavigator1.ActiveViewModelIs(this.vmSalesmaster))
            {
                this.QBE_Salesmaster();
            }
        }

        private void frmSales_Resize(object sender, EventArgs e)
        {
            //this.tabSales.Width = this.Width - 8;
            //this.tabSales.Height = this.dataNavigator1.Top - this.tabSales.Top;
            this.pnl_SalesMaster.Height = this.cmb_payterms.Top + this.cmb_payterms.Height + 4;
            //this.dgv_SalesDetails .Top=this.pnl_SalesMaster.Top + this.pnl_SalesMaster.Height +4;   

        }

        private void tabSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Parent = this.tabSales.SelectedTab;
            //this.pnl_SalesMaster.Parent = this.tabSales.SelectedTab;
            //this.dgv_SalesDetails .Parent = this.tabSales .SelectedTab;

            if (this.tabSales.SelectedTab == this.tabPageSalesDetails)
            {
                this.dataNavigator1.SetActiveViewModel("Salesdetail");
                this.pnl_SalesMaster.Enabled = false;
                this.dgv_SalesDetails.ReadOnly = false;
            }
            else
            {
                this.pnl_SalesMaster.Enabled = true;
                this.dgv_SalesDetails.ReadOnly = true;
                this.dataNavigator1.SetActiveViewModel("Salesmaster");
            }
        }

        private void dataNavigator1_eAfterAddNewRequest(ref bool Cancel)
        {
            if (this.dataNavigator1.ActiveViewModelIs(this.vmSalesdetail))
            {
                this.dgv_SalesDetails.CurrentRow[this.dgvc_ord_num].Value = this.vmSalesmaster.ModelItem.ord_num;
            }
            if (this.dataNavigator1.ActiveViewModelIs(this.vmSalesmaster))
            {
                this.txt_ord_num.Text = "";
                this.dtp_ord_date .Value = DateTime.Now;    
                this.dtp_stor_ord_date.Value = DateTime.Now;  
            }
        }

        private void dgv_SalesDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == this.dgvc_qbe_titles.Index)
            {
                this.QBE_Title();
            }
        }

        private void dataNavigator1_eAddNewCompleted()
        {
            if (this.dataNavigator1.ActiveViewModelIs(this.vmSalesmaster))
            {
                this.tabPageSalesDetails.Enabled = false;
            }
        }

        private void dataNavigator1_eSaveCompleted()
        {
            if (this.dataNavigator1.ActiveViewModelIs(this.vmSalesmaster))
            {
                this.tabPageSalesDetails.Enabled = true;
            }
        }

        private void dataNavigator1_eUndoCompleted()
        {
            if (this.dataNavigator1.ActiveViewModelIs(this.vmSalesmaster))
            {
                this.tabPageSalesDetails.Enabled = true;
            }
        }

        private void txt_stor_id_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "search")
            {
                QBE_Store();    
            }
        }

        private void txt_stor_id_TextChanged(object sender, EventArgs e)
        {
            this.dbLookUpStores.Lookup();   
        }

        private void dataNavigator1_eSaveRequest(ref bool Cancel)
        {
            // Forza la validazione di tutti i controlli con regole di validazione
            if (!this.ValidateChildren())
            {
                Cancel = true;  // Annulla il salvataggio se la validazione fallisce
            }
        }

        private void dgv_SalesDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow cRow in this.dgv_SalesDetails .Rows)
            {
                if (cRow[this.dgvc_title_id].Value != null)
                {
                    Models.Title title= this.vmTitle.GetTitle(cRow[this.dgvc_title_id].Value.ToString());
                    if (title != null)
                        cRow[this.dgvc_title].Value = this.vmTitle.GetTitle(cRow[this.dgvc_title_id].Value.ToString()).title;
                    else
                        cRow[this.dgvc_title].Value = "";
                }
            }
        }
    }
}
