using FastDeepCloner;
using System;
using Wisej.Web;


namespace PasseroDemo.Application
{
    public partial class LoginPage : Page
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public string ID = "";
        public LoginPage()
        {
            InitializeComponent();
            panel1.CenterToParent();
            panel1.Anchor = AnchorStyles.None;
            panel1.Visible = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

           // this.Text = this.ID;

            ConfigurationManager.AddSessionConfigurationKeyValue("General", "CurrentUser", this.txtUserName.Text.Trim());
            Wisej .Web .Application .Session["ConfigurationManager"]= ConfigurationManager;    
            MainPage maiPage = new MainPage();
            maiPage.ConfigurationManager = ConfigurationManager;
            maiPage.SetDesktop();
            maiPage.Show();
            this.Dispose (); // chiude la form corrente
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            Wisej.Web.Application.Navigate("https://www.gabrieledelgiovine.it", "_blank");
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

         


            
        }
    }
}
