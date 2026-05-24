namespace Passero.Framework.Controls

{
    partial class SerialBarcodeReader
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Wisej.NET Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBarcodeReader = new Wisej.Web.Button();
            this.SuspendLayout();
            // 
            // btnBarcodeReader
            // 
            this.btnBarcodeReader.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left)
            | Wisej.Web.AnchorStyles.Right)));
            this.btnBarcodeReader.BackColor = System.Drawing.Color.Red;
            this.btnBarcodeReader.Focusable = false;
            this.btnBarcodeReader.ImageSource = "resource.wx/Wisej.Ext.BootstrapIcons/upc-scan.svg";
            this.btnBarcodeReader.Location = new System.Drawing.Point(14, 3);
            this.btnBarcodeReader.Name = "btnBarcodeReader";
            this.btnBarcodeReader.Size = new System.Drawing.Size(98, 49);
            this.btnBarcodeReader.TabIndex = 0;
            this.btnBarcodeReader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBarcodeReader.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.btnBarcodeReader.Click += new System.EventHandler(this.btnBarcodeReader_Click);
            // 
            // BarcodeReader
            // 
            this.Controls.Add(this.btnBarcodeReader);
            this.Name = "BarcodeReader";
            this.Size = new System.Drawing.Size(128, 54);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Button btnBarcodeReader;
    }
}
