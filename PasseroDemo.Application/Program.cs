using System;
using Wisej.Web;

namespace PasseroDemo.Application
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Application.LoginPage LoginPage  = new Application.LoginPage ();   
            LoginPage .Show();
        }

        //
        // You can use the entry method below
        // to receive the parameters from the URL in the args collection.
        //
        //static void Main(NameValueCollection args)
        //{
        //}
    }
}