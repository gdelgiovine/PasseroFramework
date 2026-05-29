using System;
using System.Drawing;
using Wisej.Web;


namespace PasseroDemo.Application
{
    public partial class MDIWindow : Form
    {
        public MDIWindow()
        {
            //InitializeComponent();
            //this.lblRuntimeAppInfo.Text = Passero.Framework.Utilities.GetApplicationRuntimeInfo();
            //this.pbPasseroLogo.CenterToParent();
            //this.pbPasseroLogo.Anchor = AnchorStyles.None;
            InitializeComponent();

            InitializeRuntimeInfo();
            RegisterLayoutHandlers();

        }


        private void InitializeRuntimeInfo()
        {
            this.lblRuntimeAppInfo.Text = Passero.Framework.Utilities.GetApplicationRuntimeInfo();
            this.pbPasseroLogo.Visible = true;
            this.lblRuntimeAppInfo.Visible = true;
        }

        private void RegisterLayoutHandlers()
        {
            this.Load += (sender, e) => CenterSplashContent();
            this.SizeChanged += (sender, e) => CenterSplashContent();
            this.lblRuntimeAppInfo.TextChanged += (sender, e) => CenterSplashContent();
        }

        private void CenterSplashContent()
        {
            var bounds = this.DisplayRectangle;
            const int verticalSpacing = 12;

            var contentHeight = this.pbPasseroLogo.Height + verticalSpacing + this.lblRuntimeAppInfo.Height;
            var logoX = bounds.X + Math.Max(0, (bounds.Width - this.pbPasseroLogo.Width) / 2);
            var startY = bounds.Y + Math.Max(0, (bounds.Height - contentHeight) / 2);
            var labelX = bounds.X + Math.Max(0, (bounds.Width - this.lblRuntimeAppInfo.Width) / 2);

            this.SuspendLayout();
            this.pbPasseroLogo.Location = new Point(logoX, startY);
            this.lblRuntimeAppInfo.Location = new Point(labelX, this.pbPasseroLogo.Bottom + verticalSpacing);
            this.pbPasseroLogo.BringToFront();
            this.lblRuntimeAppInfo.BringToFront();
            this.ResumeLayout();
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
