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
            panel1.CenterToParent();
            panel1.Anchor = AnchorStyles.None;
            panel1.Visible = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ConfigurationManager.AddSessionConfigurationKeyValue("General", "CurrentUser", this.txtUserName.Text.Trim());
            MainPage maiPage = new MainPage();
            maiPage .ConfigurationManager = ConfigurationManager;
            maiPage.SetDesktop();
            maiPage.Show(); 

        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            Wisej.Web.Application.Navigate("https://www.gabrieledelgiovine.it", "_blank");
        }
    }
}
