using FastDeepCloner;
using System;
using Passero.Framework.Base;
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
            btnMicrosoft.Visible = IsFederatedProviderConfigured("Microsoft");
            btnGoogle.Visible = IsFederatedProviderConfigured("Google");
        }

        private void btnMicrosoft_Click(object sender, EventArgs e)
        {
            StartFederatedSignIn("microsoft");
        }

        private void btnGoogle_Click(object sender, EventArgs e)
        {
            StartFederatedSignIn("google");
        }

        private static void StartFederatedSignIn(string provider)
        {
            Wisej.Web.Application.Navigate($"/auth/{provider}", "_self");
        }

        private static bool IsFederatedProviderConfigured(string provider)
        {
#if NET
            return !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable($"Authentication__{provider}__ClientId"));
#else
            return false;
#endif
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var identityFilePath = ResolveIdentityFilePath();
                var authenticationProvider = new JsonSecurityPrincipalProvider(identityFilePath, () => AuthenticatedSecuritySession.Current?.UserName);
                var identity = authenticationProvider.Authenticate(this.txtUserName.Text, this.txtPassword.Text);
                if (identity == null)
                {
                    this.txtPassword.Text = string.Empty;
                    this.txtPassword.Focus();
                    MessageBox.Show("Nome utente o password non validi.", "Autenticazione non riuscita", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AuthenticatedSecuritySession.SignIn(identity);
                Wisej.Web.Application.Session["PASSERO_AUTHENTICATION_STORE"] = authenticationProvider;
                ConfigurationManager.AddSessionConfigurationKeyValue("General", "CurrentUser", identity.UserName);
                Wisej.Web.Application.Session["ConfigurationManager"] = ConfigurationManager;
                MainPage maiPage = new MainPage();
                maiPage.ConfigurationManager = ConfigurationManager;
                maiPage.SetDesktop();
                maiPage.Show();
                this.Dispose();
            }
            catch (Exception ex)
            {
                this.txtPassword.Text = string.Empty;
                this.txtPassword.Focus();
                MessageBox.Show($"Errore durante l'accesso: {ex.Message}", "Autenticazione non riuscita", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ResolveIdentityFilePath()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var candidates = new[]
            {
                System.IO.Path.Combine(baseDirectory, "Security", "UsersAndRoles.json"),
                System.IO.Path.Combine(baseDirectory, "bin", "Security", "UsersAndRoles.json")
            };

            foreach (var candidate in candidates)
            {
                if (System.IO.File.Exists(candidate))
                    return candidate;
            }

            throw new System.IO.FileNotFoundException(
                $"File identità UsersAndRoles.json non trovato. Percorsi verificati: {string.Join("; ", candidates)}");
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
