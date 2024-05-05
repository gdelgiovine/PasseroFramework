using System;
using Wisej.Web;


namespace PasseroDemo.Application
{
    public partial class LoginPage : Page
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();

        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ConfigurationManager.AddSessionConfigurationKeyValue("General", "CurrentUser", this.txtUserName.Text.Trim());
            MainPage maiPage = new MainPage();
            maiPage .ConfigurationManager = ConfigurationManager;
            maiPage.SetDesktop();
            maiPage.Show(); 

        }
    }
}
