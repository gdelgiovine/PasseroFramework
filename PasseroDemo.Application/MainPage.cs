using System;
using Wisej.Web;



namespace PasseroDemo.Application
{
    public partial class MainPage : Page
    {

       public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager ();

       MDIWindow MDIWindow = new MDIWindow();
        public MainPage()
        {
            InitializeComponent();
            SetDesktop();
            this.MDIWindow.FormBorderStyle = FormBorderStyle.None;
            this.MDIWindow.Show();
        }


        public void Init()
        {
            Passero.Framework.ExecutionResult ER = new Passero.Framework.ExecutionResult();
            ER.Context = "PasseroDemo.Application.Init()";
            this.ConfigurationManager.FileName = AppDomain.CurrentDomain.BaseDirectory + @"\PasseroDemo.ini";
            this.ConfigurationManager.Syntax = Passero.Framework.ConfigurationSyntax.INI;
            this.ConfigurationManager.ReadConfiguration();
            if (this.ConfigurationManager.ReadConfiguration() == false)
            {
                ER = this.ConfigurationManager.LastExecutionResult;
                MessageBox.Show($"Errore {ER.ResultCode}\n{ER.ResultMessage}", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.ConfigurationManager.DBConnections.Clear();
            this.ConfigurationManager.SetSessionConfigurationKeyValue("General", 
                                                                      "DBConnectionString", 
                                                                      this.ConfigurationManager.GetConfigurationKeyValue("General", "DBConnectionString"));

            System.Data.SqlClient.SqlConnection DBConnectionPasseroDemo = new System.Data.SqlClient.SqlConnection(
            this.ConfigurationManager.GetSessionConfigurationKeyValue("General", "DBConnectionString"));
            this.ConfigurationManager.DBConnections.Add("PasseroDemo", DBConnectionPasseroDemo);
        }

        public void SetMDIWindow()
        {
            MDIWindow.Top = this.ToolBar.Height;
            MDIWindow.Height = this.Height - this.ToolBar.Height;
            if (this.Width < 500)
            {
                NavigationBar.CompactView = true;
            }
            else
            {
                NavigationBar.CompactView = false;
            }
            if (NavigationBar.Visible)
            {
                MDIWindow.Left = NavigationBar.Width;
                MDIWindow.Width = this.Width - NavigationBar.Width;
            }
            else
            {
                MDIWindow.Left = 0;
                MDIWindow.Width = this.Width;
            }
        }


        public void SetDesktop()
        {
            this.NavigationBar.Left = 0;
            this.NavigationBar.Top= this.ToolBar.Height;
            this.NavigationBar.Height = this.ClientSize.Height - this.ToolBar.Height;
            if (this.Width < 500)
            {
                NavigationBar.CompactView = true;
            }
            else
            {
                NavigationBar.CompactView = false;
            }
            this.pbAppLogo.Height = this.ToolBar.Height;
            this.pbAppLogo.Width = this.ToolBar.Height;
            SetMDIWindow ();

        }

        
        private void ManageNavigationBar(string ItemName)
        {



            if (ItemName == mnuSales.Name )
            {
          
                PasseroDemo.Views.frmSales  frmSales = Passero.Framework.ControlsUtilities.FormExist<Views.frmSales>("frmSales");
                if (frmSales is null)
                {
                    frmSales = new Views .frmSales ();
                    frmSales.Name = "frmSales";
                    frmSales.ConfigurationManager = this.ConfigurationManager;
                    frmSales.MdiParent = this.MDIWindow;
                    frmSales.Show();
                }   
                frmSales.Activate();
                
            }

            if (ItemName == mnuRoyalties.Name)
            {

                PasseroDemo.Views.frmRoySched  frmRoySched = Passero.Framework.ControlsUtilities.FormExist<Views.frmRoySched>("frmRoySched");
                if(frmRoySched is null)
                {
                    frmRoySched = new Views .frmRoySched ();
                    frmRoySched.Name = "frmRoySched";
                    frmRoySched.ConfigurationManager = this.ConfigurationManager;
                    frmRoySched.MdiParent = this.MDIWindow;
                    frmRoySched.Show();
                }
                frmRoySched.Activate();

            }


            if (ItemName == mnuPubInfo .Name)
            {

                PasseroDemo.Views.frmPubsInfo  frmPubsInfo   = Passero.Framework.ControlsUtilities.FormExist<Views.frmPubsInfo>("frmPubsInfo");
                if (frmPubsInfo is null)
                {
                    frmPubsInfo = new Views .frmPubsInfo ();
                    frmPubsInfo.Name = "frmPubsInfo";
                    frmPubsInfo.ConfigurationManager = this.ConfigurationManager;
                    frmPubsInfo.MdiParent = this.MDIWindow;
                    frmPubsInfo.Show();
                }
                frmPubsInfo.Activate();

            }


            if (ItemName == mnuAuthors.Name  )
            {

                PasseroDemo.Views.frmAuthors frmAuthors =Passero.Framework.ControlsUtilities.FormExist<Views.frmAuthors>("frmAuthors");
                if (frmAuthors is null)
                {
                    frmAuthors = new Views.frmAuthors();
                    frmAuthors.Name = "frmAuthors";
                    frmAuthors.ConfigurationManager = this.ConfigurationManager;
                    frmAuthors.MdiParent = this.MDIWindow;
                    frmAuthors.Show();
                }
                frmAuthors.Activate();

              

            }

            if (ItemName == mnuDiscounts.Name)
            {

                PasseroDemo.Views.frmDiscount frmDiscount = Passero.Framework.ControlsUtilities.FormExist <Views.frmDiscount>("frmDiscount");
                
                if (frmDiscount is null)
                {
                    frmDiscount = new Views .frmDiscount ();
                    frmDiscount.Name = "frmDiscount";
                    frmDiscount.ConfigurationManager = this.ConfigurationManager;
                    frmDiscount.MdiParent = this.MDIWindow;
                    frmDiscount.Show();
                }   
                frmDiscount.Activate();

            }
            if (ItemName == mnuEmployees .Name)
            {

                PasseroDemo.Views.frmEmployee frmEmployee = Passero.Framework.ControlsUtilities.FormExist<Views.frmEmployee>("frmEmployee");
                if (frmEmployee is null)
                {
                    frmEmployee = new Views .frmEmployee ();
                    frmEmployee.Name = "frmEmployee";
                    frmEmployee.ConfigurationManager = this.ConfigurationManager;
                    frmEmployee.MdiParent = this.MDIWindow;
                    frmEmployee.Show();
                }
                frmEmployee.Activate();

            }

            if (ItemName == mnuTitles.Name)
            {
                PasseroDemo.Views.frmTitle frmTitle = Passero.Framework.ControlsUtilities.FormExist <Views.frmTitle>("frmTitle");
                if (frmTitle is null)
                {
                    frmTitle = new Views .frmTitle ();
                    frmTitle.Name = "frmTitle";
                    frmTitle.ConfigurationManager = this.ConfigurationManager;
                    frmTitle.MdiParent = this.MDIWindow;
                    frmTitle.Show();
                }   
                frmTitle.Activate();    
            }

            if (ItemName == mnuJobs.Name)
            {
                PasseroDemo.Views.frmJobs frmJobs = Passero.Framework.ControlsUtilities.FormExist<Views.frmJobs>("frmJobs");
                if (frmJobs is null)
                {
                    frmJobs = new Views .frmJobs ();
                    frmJobs.Name = "frmJobs";
                    frmJobs.ConfigurationManager = this.ConfigurationManager;
                    frmJobs.MdiParent = this.MDIWindow;
                    frmJobs.Show();
                }   
                frmJobs.Activate(); 

            }

            if (ItemName == mnuPublishers.Name)
            {
                PasseroDemo.Views.frmPublishers frmPublishers = Passero.Framework.ControlsUtilities.FormExist<Views.frmPublishers>("frmPublishers");
                if(frmPublishers is null)
                {
                    frmPublishers = new Views.frmPublishers();
                    frmPublishers.Name = "frmPublishers";
                    frmPublishers.ConfigurationManager = this.ConfigurationManager;
                    frmPublishers.MdiParent = this.MDIWindow;
                    frmPublishers.Show();
                }   
                frmPublishers.Activate();



            }


            if (ItemName == mnuItaliano .Name)
            {
                Wisej.Web.Application.CurrentCulture = new System.Globalization.CultureInfo("it-IT");
            }

            if (ItemName == mnuInglese.Name)
            {
                Wisej.Web.Application.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }

          

        }


        private void MaiPage_Load(object sender, EventArgs e)
        {
            Init();
            //SetDesktop();
        }

        private void MaiPage_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout ();  
            SetDesktop();
            this.ResumeLayout(true);
        }

      
    
        private void NavigationBar_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout();
            SetDesktop();
            this.ResumeLayout(true);
        }

        private void NavigationBar_ItemClick(object sender, Wisej.Web.Ext.NavigationBar.NavigationBarItemClickEventArgs e)
        {
            ManageNavigationBar(e.Item.Name);
        }

        private void ToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button.Name == this.tbMenuOwerFlow .Name)
            {
                if (this.NavigationBar.Visible)
                {
                    this.NavigationBar.Visible = false;
                }
                else
                {
                    this.NavigationBar.Visible = true;
                }

                SetMDIWindow();

            }

        }

        private void pbGDGLogo_DoubleClick(object sender, EventArgs e)
        {
            Wisej.Web.Application.Navigate("https://www.gabrieledelgiovine.it", "_blank");
        }
    }
}
