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
            this.txt_Publishers_pub_id = new Wisej.Web.TextBox();
            this.bsPublishers = new Wisej.Web.BindingSource(this.components);
            this.txt_Publishers_pub_name = new Wisej.Web.TextBox();
            this.txt_Publishers_city = new Wisej.Web.TextBox();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.txt_Publishers_state = new Wisej.Web.TextBox();
            this.txt_Publishers_country = new Wisej.Web.TextBox();
            this.txt_Publishers_phone = new Wisej.Web.TextBox();
            this.txt_Publishers_email = new Wisej.Web.TextBox();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.bsPublishers)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Publishers_pub_id
            // 
            this.txt_Publishers_pub_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_Publishers_pub_id, 20);
            this.txt_Publishers_pub_id.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_pub_id.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_pub_id.LabelText = "Publisher ID";
            this.txt_Publishers_pub_id.Location = new System.Drawing.Point(3, 3);
            this.txt_Publishers_pub_id.Name = "txt_Publishers_pub_id";
            this.txt_Publishers_pub_id.Size = new System.Drawing.Size(126, 48);
            this.txt_Publishers_pub_id.TabIndex = 0;
            // 
            // bsPublishers
            // 
            this.bsPublishers.DataSource = typeof(PasseroDemo.Models.Publisher);
            // 
            // txt_Publishers_pub_name
            // 
            this.txt_Publishers_pub_name.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "pub_name", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_Publishers_pub_name, 80);
            this.flowLayoutPanel1.SetFlowBreak(this.txt_Publishers_pub_name, true);
            this.txt_Publishers_pub_name.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_pub_name.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_pub_name.LabelText = "Publisher Name";
            this.txt_Publishers_pub_name.Location = new System.Drawing.Point(135, 3);
            this.txt_Publishers_pub_name.Name = "txt_Publishers_pub_name";
            this.txt_Publishers_pub_name.Size = new System.Drawing.Size(506, 48);
            this.txt_Publishers_pub_name.TabIndex = 1;
            // 
            // txt_Publishers_city
            // 
            this.txt_Publishers_city.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "city", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_Publishers_city, 40);
            this.txt_Publishers_city.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_city.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_city.LabelText = "City";
            this.txt_Publishers_city.Location = new System.Drawing.Point(3, 57);
            this.txt_Publishers_city.Name = "txt_Publishers_city";
            this.txt_Publishers_city.Size = new System.Drawing.Size(263, 48);
            this.txt_Publishers_city.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_pub_id);
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_pub_name);
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_city);
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_state);
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_country);
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_phone);
            this.flowLayoutPanel1.Controls.Add(this.txt_Publishers_email);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(644, 373);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // txt_Publishers_state
            // 
            this.txt_Publishers_state.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "state", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Publishers_state.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_state.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_state.LabelText = "State";
            this.txt_Publishers_state.Location = new System.Drawing.Point(272, 57);
            this.txt_Publishers_state.Name = "txt_Publishers_state";
            this.txt_Publishers_state.Size = new System.Drawing.Size(100, 48);
            this.txt_Publishers_state.TabIndex = 3;
            // 
            // txt_Publishers_country
            // 
            this.txt_Publishers_country.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "country", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_Publishers_country, 40);
            this.flowLayoutPanel1.SetFlowBreak(this.txt_Publishers_country, true);
            this.txt_Publishers_country.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_country.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_country.LabelText = "Country";
            this.txt_Publishers_country.Location = new System.Drawing.Point(378, 57);
            this.txt_Publishers_country.Name = "txt_Publishers_country";
            this.txt_Publishers_country.Size = new System.Drawing.Size(263, 48);
            this.txt_Publishers_country.TabIndex = 4;
            // 
            // txt_Publishers_phone
            // 
            this.txt_Publishers_phone.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "phone", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_Publishers_phone, 30);
            this.txt_Publishers_phone.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_phone.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_phone.LabelText = "Phone";
            this.txt_Publishers_phone.Location = new System.Drawing.Point(3, 111);
            this.txt_Publishers_phone.Name = "txt_Publishers_phone";
            this.txt_Publishers_phone.Size = new System.Drawing.Size(190, 48);
            this.txt_Publishers_phone.TabIndex = 5;
            // 
            // txt_Publishers_email
            // 
            this.txt_Publishers_email.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPublishers, "email", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_Publishers_email, 70);
            this.txt_Publishers_email.Label.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Publishers_email.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Publishers_email.LabelText = "Email";
            this.txt_Publishers_email.Location = new System.Drawing.Point(199, 111);
            this.txt_Publishers_email.Name = "txt_Publishers_email";
            this.txt_Publishers_email.Size = new System.Drawing.Size(442, 48);
            this.txt_Publishers_email.TabIndex = 6;
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "Publishers";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 382);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(650, 57);
            this.dataNavigator1.TabIndex = 0;
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
            this.dataNavigator1.eAddNewCompleted += new Passero.Framework.Controls.DataNavigator.eAddNewCompletedEventHandler(this.dataNavigator1_eAddNewCompleted);
            this.dataNavigator1.eDelete += new Passero.Framework.Controls.DataNavigator.eDeleteEventHandler(this.dataNavigator1_eDelete);
            this.dataNavigator1.eDeleteCompleted += new Passero.Framework.Controls.DataNavigator.eDeleteCompletedEventHandler(this.dataNavigator1_eDeleteCompleted);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eSave += new Passero.Framework.Controls.DataNavigator.eSaveEventHandler(this.dataNavigator1_eSave);
            this.dataNavigator1.eSaveCompleted += new Passero.Framework.Controls.DataNavigator.eSaveCompletedEventHandler(this.dataNavigator1_eSaveCompleted);
            this.dataNavigator1.eUndo += new Passero.Framework.Controls.DataNavigator.eUndoEventHandler(this.dataNavigator1_eUndo);
            this.dataNavigator1.eUndoCompleted += new Passero.Framework.Controls.DataNavigator.eUndoCompletedEventHandler(this.dataNavigator1_eUndoCompleted);
            this.dataNavigator1.eAddNewRequest += new Passero.Framework.Controls.DataNavigator.eAddNewRequestEventHandler(this.dataNavigator1_eAddNewRequest);
            this.dataNavigator1.eDeleteRequest += new Passero.Framework.Controls.DataNavigator.eDeleteRequestEventHandler(this.dataNavigator1_eDeleteRequest);
            this.dataNavigator1.eSaveRequest += new Passero.Framework.Controls.DataNavigator.eSaveRequestEventHandler(this.dataNavigator1_eSaveRequest);
            this.dataNavigator1.eMoveNextCompleted += new Passero.Framework.Controls.DataNavigator.eMoveNextCompletedEventHandler(this.dataNavigator1_eMoveNextCompleted);
            this.dataNavigator1.eUndoRequest += new Passero.Framework.Controls.DataNavigator.eUndoRequestEventHandler(this.dataNavigator1_eUndoRequest);
            // 
            // frmPublishers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 439);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmPublishers";
            this.Text = "Publishers";
            this.Load += new System.EventHandler(this.frmPublishers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsPublishers)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.BindingSource bsPublishers;
        private Wisej.Web.TextBox txt_Publishers_pub_id;
        private Wisej.Web.TextBox txt_Publishers_pub_name;
        private Wisej.Web.TextBox txt_Publishers_city;
        private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
        private Wisej.Web.TextBox txt_Publishers_state;
        private Wisej.Web.TextBox txt_Publishers_country;
        private Wisej.Web.TextBox txt_Publishers_phone;
        private Wisej.Web.TextBox txt_Publishers_email;
    }
}