namespace PasseroDemo.Views
{
    partial class frmAuthors
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
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.chk_Contract = new Wisej.Web.CheckBox();
            this.bsAuthors = new Wisej.Web.BindingSource(this.components);
            this.txt_email = new Wisej.Web.TextBox();
            this.txt_zip = new Wisej.Web.TextBox();
            this.txt_state = new Wisej.Web.TextBox();
            this.txt_city = new Wisej.Web.TextBox();
            this.txt_address = new Wisej.Web.TextBox();
            this.txt_au_id = new Wisej.Web.TextBox();
            this.txt_phone = new Wisej.Web.TextBox();
            this.txt_au_fname = new Wisej.Web.TextBox();
            this.txt_au_lname = new Wisej.Web.TextBox();
            this.flpAuthors = new Wisej.Web.FlowLayoutPanel();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.bsAuthors)).BeginInit();
            this.flpAuthors.SuspendLayout();
            this.SuspendLayout();
            // 
            // chk_Contract
            // 
            this.chk_Contract.AutoSize = false;
            this.chk_Contract.DataBindings.Add(new Wisej.Web.Binding("Checked", this.bsAuthors, "contract", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.chk_Contract.Dock = Wisej.Web.DockStyle.Bottom;
            this.flpAuthors.SetFlowBreak(this.chk_Contract, true);
            this.chk_Contract.Location = new System.Drawing.Point(377, 124);
            this.chk_Contract.Name = "chk_Contract";
            this.chk_Contract.Size = new System.Drawing.Size(110, 32);
            this.chk_Contract.TabIndex = 9;
            this.chk_Contract.Text = "With Contract";
            // 
            // bsAuthors
            // 
            this.bsAuthors.DataSource = typeof(PasseroDemo.Models.Author);
            // 
            // txt_email
            // 
            this.txt_email.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "email", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_email.InputType.Mode = Wisej.Web.TextBoxMode.Email;
            this.txt_email.InvalidMessage = "Please insert a valid Email address";
            this.txt_email.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_email.LabelText = "Author Email";
            this.txt_email.Location = new System.Drawing.Point(161, 110);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(210, 46);
            this.txt_email.TabIndex = 8;
            // 
            // txt_zip
            // 
            this.txt_zip.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "zip", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_zip.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_zip.LabelText = "Author Zip Code";
            this.txt_zip.Location = new System.Drawing.Point(468, 58);
            this.txt_zip.Name = "txt_zip";
            this.txt_zip.Size = new System.Drawing.Size(83, 46);
            this.txt_zip.TabIndex = 5;
            // 
            // txt_state
            // 
            this.txt_state.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "state", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flpAuthors.SetFlowBreak(this.txt_state, true);
            this.txt_state.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_state.LabelText = "Author State";
            this.txt_state.Location = new System.Drawing.Point(557, 58);
            this.txt_state.Name = "txt_state";
            this.txt_state.Size = new System.Drawing.Size(62, 46);
            this.txt_state.TabIndex = 6;
            // 
            // txt_city
            // 
            this.txt_city.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "city", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_city.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_city.LabelText = "Author City";
            this.txt_city.Location = new System.Drawing.Point(237, 58);
            this.txt_city.Name = "txt_city";
            this.txt_city.Size = new System.Drawing.Size(225, 46);
            this.txt_city.TabIndex = 4;
            // 
            // txt_address
            // 
            this.txt_address.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "address", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_address.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_address.LabelText = "Author Address";
            this.txt_address.Location = new System.Drawing.Point(6, 58);
            this.txt_address.Name = "txt_address";
            this.txt_address.Size = new System.Drawing.Size(225, 46);
            this.txt_address.TabIndex = 3;
            // 
            // txt_au_id
            // 
            this.txt_au_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_au_id.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_au_id.LabelText = "Author ID";
            this.txt_au_id.Location = new System.Drawing.Point(6, 6);
            this.txt_au_id.Name = "txt_au_id";
            this.txt_au_id.Size = new System.Drawing.Size(110, 46);
            this.txt_au_id.TabIndex = 0;
            // 
            // txt_phone
            // 
            this.txt_phone.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "phone", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_phone.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_phone.LabelText = "Author Phone Number";
            this.txt_phone.Location = new System.Drawing.Point(6, 110);
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Size = new System.Drawing.Size(149, 46);
            this.txt_phone.TabIndex = 7;
            // 
            // txt_au_fname
            // 
            this.txt_au_fname.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_fname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flpAuthors.SetFlowBreak(this.txt_au_fname, true);
            this.txt_au_fname.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_au_fname.LabelText = "Author First Name";
            this.txt_au_fname.Location = new System.Drawing.Point(321, 6);
            this.txt_au_fname.Name = "txt_au_fname";
            this.txt_au_fname.Size = new System.Drawing.Size(193, 46);
            this.txt_au_fname.TabIndex = 2;
            // 
            // txt_au_lname
            // 
            this.txt_au_lname.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_lname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_au_lname.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_au_lname.LabelText = "Author Last Name";
            this.txt_au_lname.Location = new System.Drawing.Point(122, 6);
            this.txt_au_lname.Name = "txt_au_lname";
            this.txt_au_lname.Size = new System.Drawing.Size(193, 46);
            this.txt_au_lname.TabIndex = 1;
            // 
            // flpAuthors
            // 
            this.flpAuthors.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.flpAuthors.AutoScroll = true;
            this.flpAuthors.Controls.Add(this.txt_au_id);
            this.flpAuthors.Controls.Add(this.txt_au_lname);
            this.flpAuthors.Controls.Add(this.txt_au_fname);
            this.flpAuthors.Controls.Add(this.txt_address);
            this.flpAuthors.Controls.Add(this.txt_city);
            this.flpAuthors.Controls.Add(this.txt_zip);
            this.flpAuthors.Controls.Add(this.txt_state);
            this.flpAuthors.Controls.Add(this.txt_phone);
            this.flpAuthors.Controls.Add(this.txt_email);
            this.flpAuthors.Controls.Add(this.chk_Contract);
            this.flpAuthors.Location = new System.Drawing.Point(3, 7);
            this.flpAuthors.Name = "flpAuthors";
            this.flpAuthors.Padding = new Wisej.Web.Padding(3);
            this.flpAuthors.Size = new System.Drawing.Size(790, 384);
            this.flpAuthors.TabIndex = 1;
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "Authors";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.FKeyEnabled = true;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 397);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(798, 74);
            this.dataNavigator1.TabIndex = 0;
            this.dataNavigator1.ePrint += new Passero.Framework.Controls.DataNavigator.ePrintEventHandler(this.dataNavigator1_ePrint);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            // 
            // frmAuthors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 471);
            this.Controls.Add(this.flpAuthors);
            this.Controls.Add(this.dataNavigator1);
            this.IconSource = "table-row-editing";
            this.Name = "frmAuthors";
            this.Text = "Authors";
            componentTool1.ImageSource = "menu-overflow";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.frmAuthors_Load);
            this.Accelerator += new Wisej.Web.AcceleratorEventHandler(this.frmAuthors_Accelerator);
            ((System.ComponentModel.ISupportInitialize)(this.bsAuthors)).EndInit();
            this.flpAuthors.ResumeLayout(false);
            this.flpAuthors.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.BindingSource bsAuthors;
        private Wisej.Web.CheckBox chk_Contract;
        internal Wisej.Web.TextBox txt_email;
        internal Wisej.Web.TextBox txt_zip;
        internal Wisej.Web.TextBox txt_state;
        internal Wisej.Web.TextBox txt_city;
        internal Wisej.Web.TextBox txt_address;
        internal Wisej.Web.TextBox txt_au_id;
        internal Wisej.Web.TextBox txt_phone;
        internal Wisej.Web.TextBox txt_au_fname;
        internal Wisej.Web.TextBox txt_au_lname;
        private Wisej.Web.FlowLayoutPanel flpAuthors;
    }
}