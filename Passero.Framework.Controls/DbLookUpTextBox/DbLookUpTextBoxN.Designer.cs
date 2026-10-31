using Wisej.Web;

namespace Passero.Framework.Controls
{
    public partial class DbLookUpTextBoxN : Wisej.Web.TextBox
    {
        private System.ComponentModel.IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
    }
}
