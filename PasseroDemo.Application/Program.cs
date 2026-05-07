using System.Collections.Specialized;
using Passero.Framework.JwtAuthHandler;
using Wisej.Web;

namespace PasseroDemo.Application
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Intercetta l'autenticazione JWT quando la URL contiene /JWTAUTH.
        /// </summary>
        static void Main(NameValueCollection args)
        {
       
       
            var loginPage = new LoginPage();
            loginPage.Show();
        }
    }
}
