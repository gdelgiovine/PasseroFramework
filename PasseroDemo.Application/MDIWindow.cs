using System;
using System.Drawing;
using Wisej.Web;


namespace PasseroDemo.Application
{
    public partial class MDIWindow : Form
    {
        public MDIWindow()
        {
            InitializeComponent();
            this.lblRuntimeAppInfo.Text = Passero.Framework.Utilities.GetApplicationRuntimeInfo();

            this.pbPasseroLogo.Anchor = AnchorStyles.None;
            this.lblRuntimeAppInfo.Anchor = AnchorStyles.None;

        }



        private void pbGDGLogo_DoubleClick(object sender, EventArgs e)
        {
            Wisej.Web.Application.Navigate("https://www.gabrieledelgiovine.it", "_blank");

            //Application.OpenWindow(“/”, “_blank”,””, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wisej.Web.Application.Navigate("https://www.gabrieledelgiovine.it", "_blank");
        }

   
    }
}
