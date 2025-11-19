using System;
using System.Data;
using Wisej.Web;
using Passero.Framework;
using Passero.Framework.Controls;
namespace PasseroDemo.Views 
{
    public partial class frmPubsInfo : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }
        private ViewModel<PasseroDemo.Models.Pub_Info> vmPubIsnfo = new ViewModel<PasseroDemo.Models.Pub_Info>();
        private ViewModel<PasseroDemo.Models.Publisher > vmPublisherIntrinsic = new ViewModel<PasseroDemo.Models.Publisher>();
        private ViewModels .vmPublisher  vmPublisher = new ViewModels .vmPublisher ();
        private bool FormInit = false;
        private bool DataNavigatorHandlerInitialized = false;
        
        public frmPubsInfo()
        {
            InitializeComponent();
        }


        private void Init(bool ForceFormInit = false)
        {

            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            this.vmPubIsnfo.Init(this.DbConnection);
            this.vmPubIsnfo.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            this.vmPubIsnfo.BindingSource = this.bsPubsInfo ;
            this.vmPublisher.Init(this.DbConnection);

            this.txt_pub_id.DbConnection = this.DbConnection;
            this.txt_pub_id.LookUpOnEdit = true;
            this.txt_pub_id.DynamicDataSource = () => this.vmPublisher.GetPublisher(this.txt_pub_id.Text);
            this.txt_pub_id.DisplayMember = nameof(Models.Publisher.pub_id);
            this.txt_pub_id.ValueMember = nameof(Models.Publisher.pub_id);
            this.txt_pub_id.AddControl(this.txt_pub_name, "text", nameof(Models.Publisher.pub_name));



            this.dataNavigator1 .ManageNavigation = true;   
            this.dataNavigator1 .ManageChanges = true;  
            this.dataNavigator1.ViewModels["Authors"] = new DataNavigatorViewModel(this.vmPubIsnfo , "Authors");
            this.dataNavigator1.SetActiveViewModel("Authors");
            this.dataNavigator1.Init(true);
        }

       
      


        private void ManageTabDataNavigator(string Item)
        {
            switch (Item.ToLower())
            {
                case "tabform":
                    this.dataNavigator1.DataGridActive = false;
                    break;

                case "tablist":
                    this.dataNavigator1.DataGridActive = true;
                  
                    break;

                default:
                    break;
            }
        }
        private void dataNavigator1_eFind()
        {

            this.qbe_PubInfo();
            

        }

        private void dataNavigator1_ePrint()
        {

            this.qbePrint_PubsInfo();
            //DbT_DbObjectToPrint QBEDbObject = new DbT_DbObjectToPrint();
            //QBEDbObject.Init(this.DbConfig);

            //BasicDALWisejControls.QBEForm QBEForm = new BasicDALWisejControls.QBEForm();
            //QBEForm.Mode = BasicDALWisejControls.QbeMode.Report;
            //QBEForm.ReportsPath = @"<pathofreports>" // ex: @"C:\WisejDemo\WisejDemo\Reports";
            ////QBEForm.AddReport("Report","Report.rpt", "Descrizione del report");

            ////QBEForm.DefaultReport= QBEForm .Reports["Report"];
            ////QBEForm.Text = "Stampa " + this.Text;
            ////QBEForm.ReportViewerMode = BasicDALWisejControls.ReportViewerMode.PDFStream;
            //QBEForm.CrystalReportViewerPage = Application.StartupUrl + "CrystalReportViewer.aspx";
            //QBEForm.DbObject = QBEDbObject;

            //QBEForm.ShowQBE(this);

        }

        private void qbe_PubInfo()
        {
            Passero.Framework.Controls.QBEForm<Models.vpub_info_publisher> QBE = new Passero.Framework.Controls.QBEForm<Models.vpub_info_publisher>(this.DbConnection);

            QBE.QBEColumns.Add(nameof(Models.vpub_info_publisher.pub_id ), "Id", "", "", true, true, -1);
            QBE.QBEColumns.Add(nameof(Models.vpub_info_publisher.pub_name ), "Name", "", "", true, true, -1);
            QBE.QBEColumns.Add(nameof(Models.vpub_info_publisher.state ), "State", "", "", true, true, -2);
            QBE.QBEColumns.Add(nameof(Models.vpub_info_publisher.city), "City", "", "", true, true, -2);
            QBE.QBEColumns.Add(nameof(Models.vpub_info_publisher.country ), "Country", "", "", true, true, -2);


            QBE.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
            QBE.Owner = this;
            QBE.SetFocusControlAfterClose = this.txt_pub_id;
            QBE.SetTargetViewModel(this.vmPubIsnfo , () => { this.dataNavigator1 .Init(true); });
            QBE.QBEModelPropertiesMapping.Add(nameof(Models.vpub_info_publisher .pub_id), nameof(Models.Pub_Info .pub_id));
            QBE.AutoLoadData = true;
            QBE.ShowQBE();




        }

        private void qbePrint_PubsInfo()
        {
           

        }
        private void dataNavigator1_eAddNew()
        {
            
        }

        private void dataNavigator1_eClose()
        {
            this.Close();
        }

        private void dataNavigator1_eDelete()
        {

        
        }

        private void dataNavigator1_eMoveFirst()
        {
            

        }

        private void dataNavigator1_eMoveLast()
        {

            
        }

        private void dataNavigator1_eMoveNext()
        {
            
        }

        private void dataNavigator1_eMovePrevious()
        {
            
        }

        private void dataNavigator1_eRefresh()
        {
            
        }

        private void dataNavigator1_eSave()
        {

            
        }

        private void dataNavigator1_eUndo()
        {
            
        }

        private void dataNavigator1_eSaveRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eDeleteRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eAddNewRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eCloseRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eUndoRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_ePrintRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eRefreshRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eMovePreviousRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eMoveNextRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eMoveLastRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eMoveFirstRequest(ref bool Cancel)
        {

        }

        private void dataNavigator1_eFindRequest(ref bool Cancel)
        {

        }

        private void tabDataNavigator_SelectedIndexChanged(object sender, EventArgs e)
        {

           
        }



        private void dataNavigator1_eBoundCompleted()
        {

        }

        private void frmPubsInfo_Load(object sender, EventArgs e)
        {
            this.Init();

        }

       


        private void txt_pub_id_TextChanged(object sender, EventArgs e)
        {
           //this.dbl_pub_id.LookUpByFilters();

           //if (this.dbT_Dbo_Pub_Info.DbC_logo.IsDBNull()==false)
           // {
           //     this.pict_logo.Image = BasicDAL.Utilities.ByteArrayHelper.byteArrayToImage((byte[])this.dbT_Dbo_Pub_Info.DbC_logo.Value);
           // }
           // else
           // {
           //     this.pict_logo.Image = null;
           // }
        }

      

        private void panelLogo_ToolClick(object sender, ToolClickEventArgs e)
        {

            if (e.Tool.Name.ToUpper() == "LOGO_CLEAR")
            {
                this.panelLogo .Image = null;
            }

            if (e.Tool.Name.ToUpper() == "LOGO_UPLOAD")
            {
                this.uploadLogo.UploadFiles();
            }

        }

        private void uploadLogo_Uploaded(object sender, UploadedEventArgs e)
        {

           
            this.pbLOGO .Image   = ControlsUtilities.UploadPictureBoxImage(e);
            //Passero .Framework.ControlsUtilities .UploadPictureBoxImage (e, this.pict_logo);
        }

        private void txt_pub_id_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "Search")
                this.qbe_PubInfo();
        }

        private void frmPubsInfo_Resize(object sender, EventArgs e)
        {
            this.panelLogo.Height = this.txt_pr_info.Height;
        }
    }
}

