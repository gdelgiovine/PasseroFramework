namespace PasseroDemo.Views
{
    partial class frmTitle
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle25 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle26 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.ComponentTool componentTool13 = new Wisej.Web.ComponentTool();
            this.tabTitle = new Wisej.Web.TabControl();
            this.tabPageTitles = new Wisej.Web.TabPage();
            this.dgv_TitleAuthors = new Wisej.Web.DataGridView();
            this.dgvc_title_id = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_au_id = new Wisej.Web.DataGridViewMaskedTextBoxColumn();
            this.dgvc_qbe_authors = new Wisej.Web.DataGridViewButtonColumn();
            this.dgvc_au_fullname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_au_ord = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.dgvc_royaltyper = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.pnl_Title = new Wisej.Web.Panel();
            this.button1 = new Wisej.Web.Button();
            this.txt_YtdSales = new Wisej.Web.NumericUpDown();
            this.bsTitles = new Wisej.Web.BindingSource(this.components);
            this.txt_notes = new Wisej.Web.TextBox();
            this.txt_Royalties = new Wisej.Web.NumericUpDown();
            this.txt_Advance = new Wisej.Web.NumericUpDown();
            this.txt_Price = new Wisej.Web.NumericUpDown();
            this.cmb_pub_id = new Wisej.Web.ComboBox();
            this.txt_title_id = new Wisej.Web.TextBox();
            this.txt_title = new Wisej.Web.TextBox();
            this.txt_type = new Wisej.Web.TextBox();
            this.dtp_pubbdate = new Wisej.Web.DateTimePicker();
            this.tabPageTitleAuthors = new Wisej.Web.TabPage();
            this.tabDataNavigator = new Wisej.Web.TabControl();
            this.tabForm = new Wisej.Web.TabPage();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.tabList = new Wisej.Web.TabPage();
            this.dgvListView = new Wisej.Web.DataGridView();
            this.tabTitle.SuspendLayout();
            this.tabPageTitles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TitleAuthors)).BeginInit();
            this.pnl_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_YtdSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTitles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Royalties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Advance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Price)).BeginInit();
            this.tabDataNavigator.SuspendLayout();
            this.tabForm.SuspendLayout();
            this.tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabTitle
            // 
            this.tabTitle.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.tabTitle.Controls.Add(this.tabPageTitles);
            this.tabTitle.Controls.Add(this.tabPageTitleAuthors);
            this.tabTitle.Location = new System.Drawing.Point(3, 0);
            this.tabTitle.Name = "tabTitle";
            this.tabTitle.PageInsets = new Wisej.Web.Padding(0, 39, 0, 0);
            this.tabTitle.Size = new System.Drawing.Size(892, 502);
            this.tabTitle.TabIndex = 0;
            this.tabTitle.SelectedIndexChanged += new System.EventHandler(this.tabTitle_SelectedIndexChanged);
            // 
            // tabPageTitles
            // 
            this.tabPageTitles.Controls.Add(this.dgv_TitleAuthors);
            this.tabPageTitles.Controls.Add(this.pnl_Title);
            this.tabPageTitles.Location = new System.Drawing.Point(0, 39);
            this.tabPageTitles.Name = "tabPageTitles";
            this.tabPageTitles.Size = new System.Drawing.Size(892, 463);
            this.tabPageTitles.Text = "Title";
            // 
            // dgv_TitleAuthors
            // 
            this.dgv_TitleAuthors.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.dgv_TitleAuthors.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvc_title_id,
            this.dgvc_au_id,
            this.dgvc_qbe_authors,
            this.dgvc_au_fullname,
            this.dgvc_au_ord,
            this.dgvc_royaltyper});
            this.dgv_TitleAuthors.DefaultRowHeight = 20;
            this.dgv_TitleAuthors.Location = new System.Drawing.Point(6, 233);
            this.dgv_TitleAuthors.Name = "dgv_TitleAuthors";
            this.dgv_TitleAuthors.ReadOnly = true;
            this.dgv_TitleAuthors.SelectionMode = Wisej.Web.DataGridViewSelectionMode.NoSelection;
            this.dgv_TitleAuthors.Size = new System.Drawing.Size(878, 225);
            this.dgv_TitleAuthors.TabIndex = 2;
            this.dgv_TitleAuthors.DataBindingComplete += new Wisej.Web.DataGridViewBindingCompleteEventHandler(this.dgv_TitleAuthors_DataBindingComplete);
            this.dgv_TitleAuthors.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.dgv_TitleAuthors_CellClick);
            // 
            // dgvc_title_id
            // 
            this.dgvc_title_id.DataPropertyName = "title_id";
            this.dgvc_title_id.HeaderText = "dgvc_title_id";
            this.dgvc_title_id.Name = "dgvc_title_id";
            // 
            // dgvc_au_id
            // 
            this.dgvc_au_id.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.dgvc_au_id.DataPropertyName = "au_id";
            this.dgvc_au_id.HeaderText = "Author ID";
            this.dgvc_au_id.Mask = "999-99-999";
            this.dgvc_au_id.Name = "dgvc_au_id";
            this.dgvc_au_id.Width = 120;
            // 
            // dgvc_qbe_authors
            // 
            this.dgvc_qbe_authors.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle25.NullValue = "...";
            this.dgvc_qbe_authors.DefaultCellStyle = dataGridViewCellStyle25;
            this.dgvc_qbe_authors.HeaderText = "";
            this.dgvc_qbe_authors.Name = "dgvc_qbe_authors";
            this.dgvc_qbe_authors.Text = "...";
            this.dgvc_qbe_authors.Width = 20;
            // 
            // dgvc_au_fullname
            // 
            this.dgvc_au_fullname.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.dgvc_au_fullname.HeaderText = "Author";
            this.dgvc_au_fullname.Name = "dgvc_au_fullname";
            this.dgvc_au_fullname.ReadOnly = true;
            this.dgvc_au_fullname.Width = 200;
            // 
            // dgvc_au_ord
            // 
            this.dgvc_au_ord.DataPropertyName = "au_ord";
            dataGridViewCellStyle26.Format = "N0";
            dataGridViewCellStyle26.NullValue = null;
            this.dgvc_au_ord.DefaultCellStyle = dataGridViewCellStyle26;
            this.dgvc_au_ord.HeaderText = "Auth.Order";
            this.dgvc_au_ord.HideUpDownButtons = true;
            this.dgvc_au_ord.Name = "dgvc_au_ord";
            this.dgvc_au_ord.ValueType = typeof(int);
            // 
            // dgvc_royaltyper
            // 
            this.dgvc_royaltyper.DataPropertyName = "royaltyper";
            this.dgvc_royaltyper.HeaderText = "Royalties%";
            this.dgvc_royaltyper.Name = "dgvc_royaltyper";
            this.dgvc_royaltyper.ValueType = typeof(int);
            // 
            // pnl_Title
            // 
            this.pnl_Title.Controls.Add(this.button1);
            this.pnl_Title.Controls.Add(this.txt_YtdSales);
            this.pnl_Title.Controls.Add(this.txt_notes);
            this.pnl_Title.Controls.Add(this.txt_Royalties);
            this.pnl_Title.Controls.Add(this.txt_Advance);
            this.pnl_Title.Controls.Add(this.txt_Price);
            this.pnl_Title.Controls.Add(this.cmb_pub_id);
            this.pnl_Title.Controls.Add(this.txt_title_id);
            this.pnl_Title.Controls.Add(this.txt_title);
            this.pnl_Title.Controls.Add(this.txt_type);
            this.pnl_Title.Controls.Add(this.dtp_pubbdate);
            this.pnl_Title.Location = new System.Drawing.Point(6, 3);
            this.pnl_Title.Name = "pnl_Title";
            this.pnl_Title.Size = new System.Drawing.Size(878, 224);
            this.pnl_Title.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(757, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 37);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_YtdSales
            // 
            this.txt_YtdSales.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "ytd_sales", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_YtdSales.HideUpDownButtons = true;
            this.txt_YtdSales.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_YtdSales.LabelText = "Ytd Sales";
            this.txt_YtdSales.Location = new System.Drawing.Point(635, 55);
            this.txt_YtdSales.Maximum = new decimal(99999999);
            this.txt_YtdSales.Name = "txt_YtdSales";
            this.txt_YtdSales.Size = new System.Drawing.Size(80, 48);
            this.txt_YtdSales.TabIndex = 8;
            this.txt_YtdSales.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // bsTitles
            // 
            this.bsTitles.DataSource = typeof(PasseroDemo.Models.Title);
            this.bsTitles.CurrentChanged += new System.EventHandler(this.bsTitles_CurrentChanged);
            this.bsTitles.PositionChanged += new System.EventHandler(this.bsTitles_PositionChanged);
            // 
            // txt_notes
            // 
            this.txt_notes.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.txt_notes.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "notes", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_notes.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_notes.LabelText = "Notes";
            this.txt_notes.Location = new System.Drawing.Point(3, 106);
            this.txt_notes.Margin = new Wisej.Web.Padding(0);
            this.txt_notes.MaxLength = 200;
            this.txt_notes.Multiline = true;
            this.txt_notes.Name = "txt_notes";
            this.txt_notes.Size = new System.Drawing.Size(855, 111);
            this.txt_notes.TabIndex = 1;
            // 
            // txt_Royalties
            // 
            this.txt_Royalties.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "royalty", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Royalties.HideUpDownButtons = true;
            this.txt_Royalties.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Royalties.LabelText = "Royalties %";
            this.txt_Royalties.Location = new System.Drawing.Point(552, 55);
            this.txt_Royalties.Name = "txt_Royalties";
            this.txt_Royalties.Size = new System.Drawing.Size(80, 48);
            this.txt_Royalties.TabIndex = 7;
            this.txt_Royalties.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txt_Advance
            // 
            this.txt_Advance.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "advance", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Advance.DecimalPlaces = 2;
            this.txt_Advance.HideUpDownButtons = true;
            this.txt_Advance.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Advance.LabelText = "Advance";
            this.txt_Advance.Location = new System.Drawing.Point(469, 55);
            this.txt_Advance.Maximum = new decimal(1000000);
            this.txt_Advance.Name = "txt_Advance";
            this.txt_Advance.Size = new System.Drawing.Size(80, 48);
            this.txt_Advance.TabIndex = 6;
            this.txt_Advance.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txt_Price
            // 
            this.txt_Price.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "price", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Price.DecimalPlaces = 2;
            this.txt_Price.HideUpDownButtons = true;
            this.txt_Price.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Price.LabelText = "Price";
            this.txt_Price.Location = new System.Drawing.Point(386, 55);
            this.txt_Price.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            this.txt_Price.Name = "txt_Price";
            this.txt_Price.Size = new System.Drawing.Size(80, 48);
            this.txt_Price.TabIndex = 5;
            this.txt_Price.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // cmb_pub_id
            // 
            this.cmb_pub_id.DataBindings.Add(new Wisej.Web.Binding("SelectedValue", this.bsTitles, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.cmb_pub_id.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmb_pub_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_pub_id.LabelText = "Publisher";
            this.cmb_pub_id.Location = new System.Drawing.Point(3, 55);
            this.cmb_pub_id.Margin = new Wisej.Web.Padding(0);
            this.cmb_pub_id.Name = "cmb_pub_id";
            this.cmb_pub_id.Size = new System.Drawing.Size(264, 48);
            this.cmb_pub_id.TabIndex = 3;
            componentTool13.ImageSource = "icon-refresh";
            componentTool13.Name = "refresh";
            componentTool13.ToolTipText = "Reload Publishers";
            this.cmb_pub_id.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool13});
            this.cmb_pub_id.ToolClick += new Wisej.Web.ToolClickEventHandler(this.cmb_pub_id_ToolClick);
            // 
            // txt_title_id
            // 
            this.txt_title_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "title_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_title_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title_id.LabelText = "Title ID";
            this.txt_title_id.Location = new System.Drawing.Point(3, 4);
            this.txt_title_id.Margin = new Wisej.Web.Padding(0);
            this.txt_title_id.Name = "txt_title_id";
            this.txt_title_id.Size = new System.Drawing.Size(100, 48);
            this.txt_title_id.TabIndex = 0;
            this.txt_title_id.TextChanged += new System.EventHandler(this.txt_title_id_TextChanged);
            // 
            // txt_title
            // 
            this.txt_title.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "title", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_title.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title.LabelText = "Title";
            this.txt_title.Location = new System.Drawing.Point(106, 4);
            this.txt_title.Margin = new Wisej.Web.Padding(0);
            this.txt_title.Name = "txt_title";
            this.txt_title.Size = new System.Drawing.Size(330, 48);
            this.txt_title.TabIndex = 1;
            // 
            // txt_type
            // 
            this.txt_type.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "type", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_type.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_type.LabelText = "Type";
            this.txt_type.Location = new System.Drawing.Point(439, 4);
            this.txt_type.Margin = new Wisej.Web.Padding(0);
            this.txt_type.Name = "txt_type";
            this.txt_type.Size = new System.Drawing.Size(277, 48);
            this.txt_type.TabIndex = 2;
            // 
            // dtp_pubbdate
            // 
            this.dtp_pubbdate.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "pubdate", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dtp_pubbdate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtp_pubbdate.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtp_pubbdate.LabelText = "Pub. Date";
            this.dtp_pubbdate.Location = new System.Drawing.Point(270, 55);
            this.dtp_pubbdate.Margin = new Wisej.Web.Padding(0);
            this.dtp_pubbdate.Name = "dtp_pubbdate";
            this.dtp_pubbdate.Size = new System.Drawing.Size(113, 48);
            this.dtp_pubbdate.TabIndex = 4;
            this.dtp_pubbdate.Value = new System.DateTime(((long)(0)));
            // 
            // tabPageTitleAuthors
            // 
            this.tabPageTitleAuthors.Location = new System.Drawing.Point(0, 39);
            this.tabPageTitleAuthors.Name = "tabPageTitleAuthors";
            this.tabPageTitleAuthors.Size = new System.Drawing.Size(892, 463);
            this.tabPageTitleAuthors.Text = "Title Authors";
            // 
            // tabDataNavigator
            // 
            this.tabDataNavigator.Controls.Add(this.tabForm);
            this.tabDataNavigator.Controls.Add(this.tabList);
            this.tabDataNavigator.Dock = Wisej.Web.DockStyle.Fill;
            this.tabDataNavigator.Location = new System.Drawing.Point(0, 0);
            this.tabDataNavigator.Name = "tabDataNavigator";
            this.tabDataNavigator.PageInsets = new Wisej.Web.Padding(0, 39, 0, 0);
            this.tabDataNavigator.Size = new System.Drawing.Size(902, 620);
            this.tabDataNavigator.TabIndex = 1;
            // 
            // tabForm
            // 
            this.tabForm.Controls.Add(this.dataNavigator1);
            this.tabForm.Controls.Add(this.tabTitle);
            this.tabForm.Location = new System.Drawing.Point(0, 39);
            this.tabForm.Name = "tabForm";
            this.tabForm.Size = new System.Drawing.Size(902, 581);
            this.tabForm.Text = "Form View";
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.DelegateCurrencyManager = false;
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 509);
            this.dataNavigator1.ManageNavigation = false;
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(900, 70);
            this.dataNavigator1.TabIndex = 22;
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
            this.dataNavigator1.eDelete += new Passero.Framework.Controls.DataNavigator.eDeleteEventHandler(this.dataNavigator1_eDelete);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eSave += new Passero.Framework.Controls.DataNavigator.eSaveEventHandler(this.dataNavigator1_eSave);
            this.dataNavigator1.eMovePrevious += new Passero.Framework.Controls.DataNavigator.eMovePreviousEventHandler(this.dataNavigator1_eMovePrevious);
            this.dataNavigator1.eMoveFirst += new Passero.Framework.Controls.DataNavigator.eMoveFirstEventHandler(this.dataNavigator1_eMoveFirst);
            this.dataNavigator1.eMoveLast += new Passero.Framework.Controls.DataNavigator.eMoveLastEventHandler(this.dataNavigator1_eMoveLast);
            this.dataNavigator1.eMoveNext += new Passero.Framework.Controls.DataNavigator.eMoveNextEventHandler(this.dataNavigator1_eMoveNext);
            this.dataNavigator1.eUndo += new Passero.Framework.Controls.DataNavigator.eUndoEventHandler(this.dataNavigator1_eUndo);
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.dgvListView);
            this.tabList.Location = new System.Drawing.Point(0, 39);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new Wisej.Web.Padding(3);
            this.tabList.Size = new System.Drawing.Size(902, 581);
            this.tabList.Text = "List View";
            // 
            // dgvListView
            // 
            this.dgvListView.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvListView.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvListView.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvListView.KeepSameRowHeight = true;
            this.dgvListView.Location = new System.Drawing.Point(3, 3);
            this.dgvListView.Name = "dgvListView";
            this.dgvListView.ReadOnly = true;
            this.dgvListView.Size = new System.Drawing.Size(894, 573);
            this.dgvListView.TabIndex = 0;
            // 
            // frmTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 620);
            this.Controls.Add(this.tabDataNavigator);
            this.Name = "frmTitle";
            this.Text = "Titles";
            this.Load += new System.EventHandler(this.frmTitle_Load);
            this.tabTitle.ResumeLayout(false);
            this.tabPageTitles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TitleAuthors)).EndInit();
            this.pnl_Title.ResumeLayout(false);
            this.pnl_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_YtdSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTitles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Royalties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Advance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Price)).EndInit();
            this.tabDataNavigator.ResumeLayout(false);
            this.tabForm.ResumeLayout(false);
            this.tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.TabControl tabTitle;
        private Wisej.Web.TabPage tabPageTitles;
        private Wisej.Web.TabPage tabPageTitleAuthors;
        private Wisej.Web.TabControl tabDataNavigator;
        private Wisej.Web.TabPage tabForm;
        private Wisej.Web.Panel pnl_Title;
        private Wisej.Web.TextBox txt_notes;
        private Wisej.Web.ComboBox cmb_pub_id;
        private Wisej.Web.TextBox txt_title_id;
        private Wisej.Web.TextBox txt_title;
        private Wisej.Web.TextBox txt_type;
        private Wisej.Web.DateTimePicker dtp_pubbdate;
        private Wisej.Web.TabPage tabList;
        private Wisej.Web.DataGridView dgvListView;
        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.BindingSource bsTitles;
        private Wisej.Web.NumericUpDown txt_Price;
        private Wisej.Web.NumericUpDown txt_Advance;
        private Wisej.Web.NumericUpDown txt_YtdSales;
        private Wisej.Web.NumericUpDown txt_Royalties;
        private Wisej.Web.DataGridView dgv_TitleAuthors;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_title_id;
        private Wisej.Web.DataGridViewMaskedTextBoxColumn dgvc_au_id;
        private Wisej.Web.DataGridViewButtonColumn dgvc_qbe_authors;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_au_fullname;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_au_ord;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_royaltyper;
        private Wisej.Web.Button button1;
    }
}