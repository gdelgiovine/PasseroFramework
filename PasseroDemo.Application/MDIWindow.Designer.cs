namespace PasseroDemo.Application
{
    partial class MDIWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIWindow));
            this.lblRuntimeAppInfo = new Wisej.Web.Label();
            this.pbPasseroLogo = new Wisej.Web.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPasseroLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRuntimeAppInfo
            // 
            this.lblRuntimeAppInfo.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Left)));
            this.lblRuntimeAppInfo.AutoSize = true;
            this.lblRuntimeAppInfo.Location = new System.Drawing.Point(13, 325);
            this.lblRuntimeAppInfo.Name = "lblRuntimeAppInfo";
            this.lblRuntimeAppInfo.Size = new System.Drawing.Size(99, 18);
            this.lblRuntimeAppInfo.TabIndex = 1;
            this.lblRuntimeAppInfo.Text = "RuntimeAppInfo";
            // 
            // pbPasseroLogo
            // 
            this.pbPasseroLogo.Anchor = Wisej.Web.AnchorStyles.None;
            this.pbPasseroLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbPasseroLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbPasseroLogo.Image")));
            this.pbPasseroLogo.Location = new System.Drawing.Point(165, 136);
            this.pbPasseroLogo.Name = "pbPasseroLogo";
            this.pbPasseroLogo.Size = new System.Drawing.Size(236, 207);
            this.pbPasseroLogo.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // MDIWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 475);
            this.Controls.Add(this.pbPasseroLogo);
            this.Controls.Add(this.lblRuntimeAppInfo);
            this.IsMdiContainer = true;
            this.Margin = new Wisej.Web.Padding(0);
            this.Name = "MDIWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = Wisej.Web.FormStartPosition.Manual;
            this.Text = "MDIWindow";
            ((System.ComponentModel.ISupportInitialize)(this.pbPasseroLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.Label lblRuntimeAppInfo;
        private Wisej.Web.PictureBox pbPasseroLogo;
    }
}