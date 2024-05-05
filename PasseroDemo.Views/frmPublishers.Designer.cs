namespace PasseroDemo.Views
{
    partial class frmPublishers
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

        #region Wisej Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.bsPublishers = new Wisej.Web.BindingSource(this.components);
            this.txt_Publishers_pub_id = new Wisej.Web.TextBox();
            this.txt_Publishers_pub_name = new Wisej.Web.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bsPublishers)).BeginInit();
            this.SuspendLayout();
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 228);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(746, 70);
            this.dataNavigator1.TabIndex = 0;
            this.dataNavigator1.eMoveNextCompleted += new Passero.Framework.Controls.DataNavigator.eMoveNextCompletedEventHandler(this.dataNavigator1_eMoveNextCompleted);
            // 
            // bsPublishers
            // 
            this.bsPublishers.DataSource = typeof(PasseroDemo.Models.Publisher);
            // 
            // txt_Publishers_pub_id
            // 
            this.txt_Publishers_pub_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Publishers_pub_id.LabelText = "Publisher ID";
            this.txt_Publishers_pub_id.Location = new System.Drawing.Point(28, 57);
            this.txt_Publishers_pub_id.Name = "txt_Publishers_pub_id";
            this.txt_Publishers_pub_id.Size = new System.Drawing.Size(100, 53);
            this.txt_Publishers_pub_id.TabIndex = 1;
            // 
            // txt_Publishers_pub_name
            // 
            this.txt_Publishers_pub_name.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "pub_name", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Publishers_pub_name.LabelText = "Publisher Name";
            this.txt_Publishers_pub_name.Location = new System.Drawing.Point(134, 57);
            this.txt_Publishers_pub_name.Name = "txt_Publishers_pub_name";
            this.txt_Publishers_pub_name.Size = new System.Drawing.Size(264, 53);
            this.txt_Publishers_pub_name.TabIndex = 2;
            // 
            // frmPublishers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 298);
            this.Controls.Add(this.txt_Publishers_pub_name);
            this.Controls.Add(this.txt_Publishers_pub_id);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmPublishers";
            this.Text = "Publishers";
            this.Load += new System.EventHandler(this.frmPublishers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsPublishers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.BindingSource bsPublishers;
        private Wisej.Web.TextBox txt_Publishers_pub_id;
        private Wisej.Web.TextBox txt_Publishers_pub_name;
    }
}