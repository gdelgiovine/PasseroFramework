namespace PasseroDemo.Views
{
    partial class frmAuthorDEV
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAuthorDEV));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool2 = new Wisej.Web.ComponentTool();
            this.txt_email = new Wisej.Web.TextBox();
            this.bsAuthors = new Wisej.Web.BindingSource(this.components);
            this.txt_zip = new Wisej.Web.TextBox();
            this.txt_state = new Wisej.Web.TextBox();
            this.txt_city = new Wisej.Web.TextBox();
            this.txt_address = new Wisej.Web.TextBox();
            this.txt_au_id = new Wisej.Web.TextBox();
            this.txt_phone = new Wisej.Web.TextBox();
            this.txt_au_fname = new Wisej.Web.TextBox();
            this.txt_au_lname = new Wisej.Web.TextBox();
            this.chk_Contract = new Wisej.Web.CheckBox();
            this.button1 = new Wisej.Web.Button();
            this.textBox1 = new Wisej.Web.TextBox();
            this.textBox2 = new Wisej.Web.TextBox();
            this.textBox3 = new Wisej.Web.TextBox();
            this.button2 = new Wisej.Web.Button();
            this.textBox4 = new Wisej.Web.TextBox();
            this.comboBox1 = new Wisej.Web.ComboBox();
            this.button3 = new Wisej.Web.Button();
            this.bindingSource1 = new Wisej.Web.BindingSource(this.components);
            this.dbLookUpTextBox1 = new Passero.Framework.Controls.DbLookUpTextBox(this.components);
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.bsAuthors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_email
            // 
            this.txt_email.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "email", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_email.InputType.Mode = Wisej.Web.TextBoxMode.Email;
            this.txt_email.InvalidMessage = "Please insert a valid Email address";
            this.txt_email.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_email.LabelText = "Author Email";
            this.txt_email.Location = new System.Drawing.Point(165, 110);
            this.txt_email.Margin = new Wisej.Web.Padding(0);
            this.txt_email.Name = "txt_email";
            this.txt_email.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_email.ResponsiveProfiles"))));
            this.txt_email.Size = new System.Drawing.Size(210, 46);
            this.txt_email.TabIndex = 8;
            // 
            // bsAuthors
            // 
            this.bsAuthors.DataSource = typeof(PasseroDemo.Models.Author);
            this.bsAuthors.CurrentChanged += new System.EventHandler(this.bsAuthors_CurrentChanged);
            // 
            // txt_zip
            // 
            this.txt_zip.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "zip", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_zip.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_zip.LabelText = "Author Zip Code";
            this.txt_zip.Location = new System.Drawing.Point(472, 61);
            this.txt_zip.Margin = new Wisej.Web.Padding(0);
            this.txt_zip.Name = "txt_zip";
            this.txt_zip.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_zip.ResponsiveProfiles"))));
            this.txt_zip.Size = new System.Drawing.Size(83, 46);
            this.txt_zip.TabIndex = 5;
            // 
            // txt_state
            // 
            this.txt_state.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "state", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_state.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_state.LabelText = "Author State";
            this.txt_state.Location = new System.Drawing.Point(561, 61);
            this.txt_state.Margin = new Wisej.Web.Padding(0);
            this.txt_state.Name = "txt_state";
            this.txt_state.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_state.ResponsiveProfiles"))));
            this.txt_state.Size = new System.Drawing.Size(62, 46);
            this.txt_state.TabIndex = 6;
            // 
            // txt_city
            // 
            this.txt_city.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "city", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_city.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_city.LabelText = "Author City";
            this.txt_city.Location = new System.Drawing.Point(241, 61);
            this.txt_city.Margin = new Wisej.Web.Padding(0);
            this.txt_city.Name = "txt_city";
            this.txt_city.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_city.ResponsiveProfiles"))));
            this.txt_city.Size = new System.Drawing.Size(225, 46);
            this.txt_city.TabIndex = 4;
            // 
            // txt_address
            // 
            this.txt_address.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "address", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_address.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_address.LabelText = "Author Address";
            this.txt_address.Location = new System.Drawing.Point(10, 61);
            this.txt_address.Margin = new Wisej.Web.Padding(0);
            this.txt_address.Name = "txt_address";
            this.txt_address.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_address.ResponsiveProfiles"))));
            this.txt_address.Size = new System.Drawing.Size(225, 46);
            this.txt_address.TabIndex = 3;
            // 
            // txt_au_id
            // 
            this.txt_au_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_au_id.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_au_id.LabelText = "Author ID";
            this.txt_au_id.Location = new System.Drawing.Point(10, 12);
            this.txt_au_id.Margin = new Wisej.Web.Padding(0);
            this.txt_au_id.Name = "txt_au_id";
            this.txt_au_id.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_au_id.ResponsiveProfiles"))));
            this.txt_au_id.Size = new System.Drawing.Size(110, 46);
            this.txt_au_id.TabIndex = 0;
            this.txt_au_id.TextChanged += new System.EventHandler(this.txt_au_id_TextChanged);
            this.txt_au_id.Validated += new System.EventHandler(this.txt_au_id_Validated);
            // 
            // txt_phone
            // 
            this.txt_phone.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "phone", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_phone.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_phone.LabelText = "Author Phone Number";
            this.txt_phone.Location = new System.Drawing.Point(10, 110);
            this.txt_phone.Margin = new Wisej.Web.Padding(0);
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_phone.ResponsiveProfiles"))));
            this.txt_phone.Size = new System.Drawing.Size(149, 46);
            this.txt_phone.TabIndex = 7;
            // 
            // txt_au_fname
            // 
            this.txt_au_fname.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_fname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_au_fname.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_au_fname.LabelText = "Author First Name";
            this.txt_au_fname.Location = new System.Drawing.Point(325, 12);
            this.txt_au_fname.Margin = new Wisej.Web.Padding(0);
            this.txt_au_fname.Name = "txt_au_fname";
            this.txt_au_fname.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_au_fname.ResponsiveProfiles"))));
            this.txt_au_fname.Size = new System.Drawing.Size(193, 46);
            this.txt_au_fname.TabIndex = 2;
            // 
            // txt_au_lname
            // 
            this.txt_au_lname.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_lname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_au_lname.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_au_lname.LabelText = "Author Last Name";
            this.txt_au_lname.Location = new System.Drawing.Point(126, 12);
            this.txt_au_lname.Margin = new Wisej.Web.Padding(0);
            this.txt_au_lname.Name = "txt_au_lname";
            this.txt_au_lname.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("txt_au_lname.ResponsiveProfiles"))));
            this.txt_au_lname.Size = new System.Drawing.Size(193, 46);
            this.txt_au_lname.TabIndex = 1;
            // 
            // chk_Contract
            // 
            this.chk_Contract.DataBindings.Add(new Wisej.Web.Binding("Checked", this.bsAuthors, "contract", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.chk_Contract.Location = new System.Drawing.Point(10, 171);
            this.chk_Contract.Name = "chk_Contract";
            this.chk_Contract.Size = new System.Drawing.Size(110, 23);
            this.chk_Contract.TabIndex = 19;
            this.chk_Contract.Text = "With Contract";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(123, 215);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 37);
            this.button1.TabIndex = 20;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.LabelText = "Author Address";
            this.textBox1.Location = new System.Drawing.Point(226, 206);
            this.textBox1.Margin = new Wisej.Web.Padding(0);
            this.textBox1.Name = "textBox1";
            this.textBox1.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("textBox1.ResponsiveProfiles"))));
            this.textBox1.Size = new System.Drawing.Size(149, 46);
            this.textBox1.TabIndex = 21;
            // 
            // textBox2
            // 
            this.textBox2.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox2.LabelText = "Author ID";
            this.textBox2.Location = new System.Drawing.Point(10, 206);
            this.textBox2.Margin = new Wisej.Web.Padding(0);
            this.textBox2.Name = "textBox2";
            this.textBox2.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("textBox2.ResponsiveProfiles"))));
            this.textBox2.Size = new System.Drawing.Size(110, 46);
            this.textBox2.TabIndex = 22;
            this.textBox2.Validated += new System.EventHandler(this.textBox2_Validated);
            // 
            // textBox3
            // 
            this.textBox3.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox3.LabelText = "Author Address";
            this.textBox3.Location = new System.Drawing.Point(375, 206);
            this.textBox3.Margin = new Wisej.Web.Padding(0);
            this.textBox3.Name = "textBox3";
            this.textBox3.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("textBox3.ResponsiveProfiles"))));
            this.textBox3.Size = new System.Drawing.Size(143, 46);
            this.textBox3.TabIndex = 23;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(190, 302);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 37);
            this.button2.TabIndex = 25;
            this.button2.Text = "button2";
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsAuthors, "au_fullname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.textBox4.InputType.Mode = Wisej.Web.TextBoxMode.Email;
            this.textBox4.InvalidMessage = "Please insert a valid Email address";
            this.textBox4.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox4.LabelText = "Author Email";
            this.textBox4.Location = new System.Drawing.Point(387, 110);
            this.textBox4.Margin = new Wisej.Web.Padding(0);
            this.textBox4.Name = "textBox4";
            this.textBox4.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("textBox4.ResponsiveProfiles"))));
            this.textBox4.Size = new System.Drawing.Size(210, 46);
            this.textBox4.TabIndex = 26;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoSize = false;
            this.comboBox1.DataBindings.Add(new Wisej.Web.Binding("SelectedValue", this.bsAuthors, "au_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.comboBox1.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.comboBox1.LabelText = "Combo";
            this.comboBox1.Location = new System.Drawing.Point(356, 302);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(257, 52);
            this.comboBox1.TabIndex = 27;
            componentTool1.ImageSource = "icon-search";
            componentTool1.Name = "search";
            this.comboBox1.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(356, 360);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 37);
            this.button3.TabIndex = 28;
            this.button3.Text = "button3";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(PasseroDemo.Models.Author);
            // 
            // dbLookUpTextBox1
            // 
            this.dbLookUpTextBox1.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsAuthors, "au_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dbLookUpTextBox1.LabelText = "DbLookUp";
            this.dbLookUpTextBox1.Location = new System.Drawing.Point(10, 286);
            this.dbLookUpTextBox1.Name = "dbLookUpTextBox1";
            this.dbLookUpTextBox1.Size = new System.Drawing.Size(174, 53);
            this.dbLookUpTextBox1.TabIndex = 24;
            componentTool2.ImageSource = "icon-search";
            componentTool2.Name = "search";
            this.dbLookUpTextBox1.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool2});
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.AddNewCaption = "Nuovo";
            this.dataNavigator1.DelegateCurrencyManager = false;
            this.dataNavigator1.DeleteCaption = "Elimina";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.FindCaption = "Trova";
            this.dataNavigator1.Location = new System.Drawing.Point(0, 460);
            this.dataNavigator1.ManageNavigation = false;
            this.dataNavigator1.MoveFirstCaption = "Inizio";
            this.dataNavigator1.MoveLastCaption = "Fine";
            this.dataNavigator1.MoveNextCaption = "Succ.";
            this.dataNavigator1.MovePreviousCaption = "Prec.";
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.PrintCaption = "Stampa";
            this.dataNavigator1.RefreshCaption = "Ricarica";
            this.dataNavigator1.SaveCaption = "Salva";
            this.dataNavigator1.SaveMessage = "Confermi salvataggio dati?";
            this.dataNavigator1.Size = new System.Drawing.Size(694, 70);
            this.dataNavigator1.TabIndex = 18;
            this.dataNavigator1.UndoCaption = "Annulla";
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
            this.dataNavigator1.ePrint += new Passero.Framework.Controls.DataNavigator.ePrintEventHandler(this.dataNavigator1_ePrint);
            this.dataNavigator1.eDelete += new Passero.Framework.Controls.DataNavigator.eDeleteEventHandler(this.dataNavigator1_eDelete);
            this.dataNavigator1.eRefresh += new Passero.Framework.Controls.DataNavigator.eRefreshEventHandler(this.dataNavigator1_eRefresh);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eSave += new Passero.Framework.Controls.DataNavigator.eSaveEventHandler(this.dataNavigator1_eSave);
            this.dataNavigator1.eMovePrevious += new Passero.Framework.Controls.DataNavigator.eMovePreviousEventHandler(this.dataNavigator1_eMovePrevious);
            this.dataNavigator1.eMoveFirst += new Passero.Framework.Controls.DataNavigator.eMoveFirstEventHandler(this.dataNavigator1_eMoveFirst);
            this.dataNavigator1.eMoveLast += new Passero.Framework.Controls.DataNavigator.eMoveLastEventHandler(this.dataNavigator1_eMoveLast);
            this.dataNavigator1.eMoveNext += new Passero.Framework.Controls.DataNavigator.eMoveNextEventHandler(this.dataNavigator1_eMoveNext);
            this.dataNavigator1.eUndo += new Passero.Framework.Controls.DataNavigator.eUndoEventHandler(this.dataNavigator1_eUndo);
            this.dataNavigator1.Click += new System.EventHandler(this.dataNavigator1_Click);
            // 
            // frmAuthorDEV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 530);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dbLookUpTextBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chk_Contract);
            this.Controls.Add(this.dataNavigator1);
            this.Controls.Add(this.txt_email);
            this.Controls.Add(this.txt_zip);
            this.Controls.Add(this.txt_state);
            this.Controls.Add(this.txt_city);
            this.Controls.Add(this.txt_address);
            this.Controls.Add(this.txt_au_id);
            this.Controls.Add(this.txt_phone);
            this.Controls.Add(this.txt_au_fname);
            this.Controls.Add(this.txt_au_lname);
            this.Name = "frmAuthorDEV";
            this.Text = "Autori";
            this.Load += new System.EventHandler(this.frmAuthor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsAuthors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal Wisej.Web.TextBox txt_email;
        internal Wisej.Web.TextBox txt_zip;
        internal Wisej.Web.TextBox txt_state;
        internal Wisej.Web.TextBox txt_city;
        internal Wisej.Web.TextBox txt_address;
        internal Wisej.Web.TextBox txt_au_id;
        internal Wisej.Web.TextBox txt_phone;
        internal Wisej.Web.TextBox txt_au_fname;
        internal Wisej.Web.TextBox txt_au_lname;
        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.BindingSource bsAuthors;
        private Wisej.Web.CheckBox chk_Contract;
        private Wisej.Web.Button button1;
        internal Wisej.Web.TextBox textBox1;
        private Wisej.Web.BindingSource bindingSource1;
        internal Wisej.Web.TextBox textBox2;
        internal Wisej.Web.TextBox textBox3;
        private Passero.Framework.Controls.DbLookUpTextBox dbLookUpTextBox1;
        private Wisej.Web.Button button2;
        internal Wisej.Web.TextBox textBox4;
        private Wisej.Web.ComboBox comboBox1;
        private Wisej.Web.Button button3;
    }
}