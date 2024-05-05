using System;
using Wisej.Web;


namespace PasseroDemo.Application
{
    public partial class MDIWindow : Form
    {
        public MDIWindow()
        {
            InitializeComponent();
            this.lblRuntimeAppInfo.Text = Passero.Framework.Utilities.GetApplicationRuntimeInfo();
        }
    }
}
