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
            this.lblRuntimeAppInfo = new Wisej.Web.Label();
            this.SuspendLayout();
            // 
            // lblRuntimeAppInfo
            // 
            this.lblRuntimeAppInfo.AutoSize = true;
            this.lblRuntimeAppInfo.Location = new System.Drawing.Point(21, 70);
            this.lblRuntimeAppInfo.Name = "lblRuntimeAppInfo";
            this.lblRuntimeAppInfo.Size = new System.Drawing.Size(99, 18);
            this.lblRuntimeAppInfo.TabIndex = 1;
            this.lblRuntimeAppInfo.Text = "RuntimeAppInfo";
            // 
            // MDIWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 475);
            this.Controls.Add(this.lblRuntimeAppInfo);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Name = "MDIWindow";
            this.ShowInTaskbar = false;
            this.Text = "MDIWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.Label lblRuntimeAppInfo;
    }
}