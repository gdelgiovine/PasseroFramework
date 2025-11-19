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
            Wisej.Web.ComponentTool componentTool15 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool16 = new Wisej.Web.ComponentTool();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            this.tabctlTitle = new Wisej.Web.TabControl();
            this.tabPageTitles = new Wisej.Web.TabPage();
            this.pnl_Title = new Wisej.Web.FlowLayoutPanel();
            this.pnl_TitleInfo = new Wisej.Web.FlowLayoutPanel();
            this.txt_title_id = new Wisej.Web.TextBox();
            this.bsTitles = new Wisej.Web.BindingSource(this.components);
            this.txt_title = new Wisej.Web.TextBox();
            this.txt_type = new Wisej.Web.TextBox();
            this.cmb_pub_id = new Wisej.Web.ComboBox();
            this.dtp_pubbdate = new Wisej.Web.DateTimePicker();
            this.txt_Royalties = new Wisej.Web.NumericUpDown();
            this.txt_Advance = new Wisej.Web.NumericUpDown();
            this.txt_Price = new Wisej.Web.NumericUpDown();
            this.txt_YtdSales = new Wisej.Web.NumericUpDown();
            this.txt_notes = new Wisej.Web.TextBox();
            this.dgv_TitleAuthors = new Wisej.Web.DataGridView();
            this.dgvc_title_id = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_au_id = new Wisej.Web.DataGridViewMaskedTextBoxColumn();
            this.dgvc_qbe_authors = new Wisej.Web.DataGridViewButtonColumn();
            this.dgvc_au_fullname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_au_ord = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.dgvc_royaltyper = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.tabPageTitleAuthors = new Wisej.Web.TabPage();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.tabctlTitle.SuspendLayout();
            this.tabPageTitles.SuspendLayout();
            this.pnl_Title.SuspendLayout();
            this.pnl_TitleInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsTitles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Royalties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Advance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Price)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_YtdSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TitleAuthors)).BeginInit();
            this.SuspendLayout();
            // 
            // tabctlTitle
            // 
            this.tabctlTitle.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.tabctlTitle.Controls.Add(this.tabPageTitles);
            this.tabctlTitle.Controls.Add(this.tabPageTitleAuthors);
            this.tabctlTitle.Location = new System.Drawing.Point(3, 4);
            this.tabctlTitle.Name = "tabctlTitle";
            this.tabctlTitle.PageInsets = new Wisej.Web.Padding(1, 40, 1, 1);
            this.tabctlTitle.Size = new System.Drawing.Size(958, 498);
            this.tabctlTitle.SizeMode = Wisej.Web.TabSizeMode.Normal;
            this.tabctlTitle.TabIndex = 0;
            this.tabctlTitle.SelectedIndexChanged += new System.EventHandler(this.tabTitle_SelectedIndexChanged);
            // 
            // tabPageTitles
            // 
            this.tabPageTitles.AutoScroll = true;
            this.tabPageTitles.Controls.Add(this.pnl_Title);
            this.tabPageTitles.Location = new System.Drawing.Point(1, 40);
            this.tabPageTitles.Name = "tabPageTitles";
            this.tabPageTitles.Size = new System.Drawing.Size(956, 457);
            this.tabPageTitles.Text = "Title";
            this.tabPageTitles.PanelCollapsed += new System.EventHandler(this.tabPageTitles_PanelCollapsed);
            // 
            // pnl_Title
            // 
            this.pnl_Title.AutoScroll = true;
            this.pnl_Title.AutoSizeMode = Wisej.Web.AutoSizeMode.GrowAndShrink;
            this.pnl_Title.Controls.Add(this.pnl_TitleInfo);
            this.pnl_Title.Controls.Add(this.dgv_TitleAuthors);
            this.pnl_Title.Dock = Wisej.Web.DockStyle.Fill;
            this.pnl_Title.Location = new System.Drawing.Point(0, 0);
            this.pnl_Title.Name = "pnl_Title";
            this.pnl_Title.Padding = new Wisej.Web.Padding(3);
            this.pnl_Title.ScrollBars = Wisej.Web.ScrollBars.Hidden;
            this.pnl_Title.ShowCloseButton = false;
            this.pnl_Title.ShowHeader = true;
            this.pnl_Title.Size = new System.Drawing.Size(956, 457);
            this.pnl_Title.TabIndex = 10;
            this.pnl_Title.Text = "Title";
            componentTool15.ImageSource = "menu-overflow";
            componentTool15.Position = Wisej.Web.LeftRightAlignment.Left;
            this.pnl_Title.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool15});
            this.pnl_Title.ToolClick += new Wisej.Web.ToolClickEventHandler(this.pnl_Title_ToolClick);
            // 
            // pnl_TitleInfo
            // 
            this.pnl_TitleInfo.AutoSize = true;
            this.pnl_TitleInfo.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnl_TitleInfo.Controls.Add(this.txt_title_id);
            this.pnl_TitleInfo.Controls.Add(this.txt_title);
            this.pnl_TitleInfo.Controls.Add(this.txt_type);
            this.pnl_TitleInfo.Controls.Add(this.cmb_pub_id);
            this.pnl_TitleInfo.Controls.Add(this.dtp_pubbdate);
            this.pnl_TitleInfo.Controls.Add(this.txt_Royalties);
            this.pnl_TitleInfo.Controls.Add(this.txt_Advance);
            this.pnl_TitleInfo.Controls.Add(this.txt_Price);
            this.pnl_TitleInfo.Controls.Add(this.txt_YtdSales);
            this.pnl_TitleInfo.Controls.Add(this.txt_notes);
            this.pnl_Title.SetFillWeight(this.pnl_TitleInfo, 100);
            this.pnl_Title.SetFlowBreak(this.pnl_TitleInfo, true);
            this.pnl_TitleInfo.Location = new System.Drawing.Point(6, 6);
            this.pnl_TitleInfo.Name = "pnl_TitleInfo";
            this.pnl_TitleInfo.Padding = new Wisej.Web.Padding(3);
            this.pnl_TitleInfo.ShowCloseButton = false;
            this.pnl_TitleInfo.Size = new System.Drawing.Size(944, 152);
            this.pnl_TitleInfo.TabIndex = 11;
            // 
            // txt_title_id
            // 
            this.txt_title_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "title_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_title_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title_id.LabelText = "Title ID";
            this.txt_title_id.Location = new System.Drawing.Point(6, 3);
            this.txt_title_id.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.txt_title_id.MaximumSize = new System.Drawing.Size(100, 0);
            this.txt_title_id.MinimumSize = new System.Drawing.Size(100, 0);
            this.txt_title_id.Name = "txt_title_id";
            this.txt_title_id.Size = new System.Drawing.Size(100, 48);
            this.txt_title_id.TabIndex = 0;
            this.txt_title_id.TextChanged += new System.EventHandler(this.txt_title_id_TextChanged);
            // 
            // bsTitles
            // 
            this.bsTitles.DataSource = typeof(PasseroDemo.Models.Title);
            this.bsTitles.CurrentChanged += new System.EventHandler(this.bsTitles_CurrentChanged);
            this.bsTitles.PositionChanged += new System.EventHandler(this.bsTitles_PositionChanged);
            // 
            // txt_title
            // 
            this.txt_title.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "title", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_title.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title.LabelText = "Title";
            this.txt_title.Location = new System.Drawing.Point(112, 3);
            this.txt_title.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.txt_title.MinimumSize = new System.Drawing.Size(100, 0);
            this.txt_title.Name = "txt_title";
            this.txt_title.Size = new System.Drawing.Size(433, 48);
            this.txt_title.TabIndex = 1;
            // 
            // txt_type
            // 
            this.txt_type.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "type", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.pnl_TitleInfo.SetFlowBreak(this.txt_type, true);
            this.txt_type.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_type.LabelText = "Type";
            this.txt_type.Location = new System.Drawing.Point(551, 3);
            this.txt_type.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.txt_type.MinimumSize = new System.Drawing.Size(100, 0);
            this.txt_type.Name = "txt_type";
            this.txt_type.Size = new System.Drawing.Size(312, 48);
            this.txt_type.TabIndex = 2;
            // 
            // cmb_pub_id
            // 
            this.cmb_pub_id.DataBindings.Add(new Wisej.Web.Binding("SelectedValue", this.bsTitles, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.cmb_pub_id.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmb_pub_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_pub_id.LabelText = "Publisher";
            this.cmb_pub_id.Location = new System.Drawing.Point(6, 51);
            this.cmb_pub_id.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.cmb_pub_id.Name = "cmb_pub_id";
            this.cmb_pub_id.Size = new System.Drawing.Size(264, 48);
            this.cmb_pub_id.TabIndex = 3;
            componentTool16.ImageSource = "icon-refresh";
            componentTool16.Name = "refresh";
            componentTool16.ToolTipText = "Reload Publishers";
            this.cmb_pub_id.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool16});
            this.cmb_pub_id.ToolClick += new Wisej.Web.ToolClickEventHandler(this.cmb_pub_id_ToolClick);
            // 
            // dtp_pubbdate
            // 
            this.dtp_pubbdate.DataBindings.Add(new Wisej.Web.Binding("NullableValue", this.bsTitles, "pubdate", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dtp_pubbdate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtp_pubbdate.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtp_pubbdate.LabelText = "Pub. Date";
            this.dtp_pubbdate.Location = new System.Drawing.Point(276, 51);
            this.dtp_pubbdate.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.dtp_pubbdate.Name = "dtp_pubbdate";
            this.dtp_pubbdate.Size = new System.Drawing.Size(113, 48);
            this.dtp_pubbdate.TabIndex = 4;
            this.dtp_pubbdate.Value = new System.DateTime(((long)(0)));
            // 
            // txt_Royalties
            // 
            this.txt_Royalties.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "royalty", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Royalties.HideUpDownButtons = true;
            this.txt_Royalties.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Royalties.LabelText = "Royalties %";
            this.txt_Royalties.Location = new System.Drawing.Point(395, 51);
            this.txt_Royalties.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
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
            this.txt_Advance.Location = new System.Drawing.Point(481, 51);
            this.txt_Advance.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
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
            this.txt_Price.Location = new System.Drawing.Point(567, 51);
            this.txt_Price.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
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
            // txt_YtdSales
            // 
            this.txt_YtdSales.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsTitles, "ytd_sales", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.pnl_TitleInfo.SetFlowBreak(this.txt_YtdSales, true);
            this.txt_YtdSales.HideUpDownButtons = true;
            this.txt_YtdSales.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_YtdSales.LabelText = "Ytd Sales";
            this.txt_YtdSales.Location = new System.Drawing.Point(653, 51);
            this.txt_YtdSales.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.txt_YtdSales.Maximum = new decimal(99999999);
            this.txt_YtdSales.Name = "txt_YtdSales";
            this.txt_YtdSales.Size = new System.Drawing.Size(80, 48);
            this.txt_YtdSales.TabIndex = 8;
            this.txt_YtdSales.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txt_notes
            // 
            this.txt_notes.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitles, "notes", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.pnl_TitleInfo.SetFillWeight(this.txt_notes, 100);
            this.txt_notes.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_notes.LabelText = "Notes";
            this.txt_notes.Location = new System.Drawing.Point(6, 99);
            this.txt_notes.Margin = new Wisej.Web.Padding(3, 0, 3, 0);
            this.txt_notes.Name = "txt_notes";
            this.txt_notes.Size = new System.Drawing.Size(930, 48);
            this.txt_notes.TabIndex = 9;
            // 
            // dgv_TitleAuthors
            // 
            this.dgv_TitleAuthors.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvc_title_id,
            this.dgvc_au_id,
            this.dgvc_qbe_authors,
            this.dgvc_au_fullname,
            this.dgvc_au_ord,
            this.dgvc_royaltyper});
            this.dgv_TitleAuthors.DefaultRowHeight = 20;
            this.pnl_Title.SetFillWeight(this.dgv_TitleAuthors, 100);
            this.pnl_Title.SetFlowBreak(this.dgv_TitleAuthors, true);
            this.dgv_TitleAuthors.Location = new System.Drawing.Point(6, 164);
            this.dgv_TitleAuthors.Name = "dgv_TitleAuthors";
            this.dgv_TitleAuthors.ReadOnly = true;
            this.dgv_TitleAuthors.SelectionMode = Wisej.Web.DataGridViewSelectionMode.NoSelection;
            this.dgv_TitleAuthors.Size = new System.Drawing.Size(944, 133);
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
            dataGridViewCellStyle15.NullValue = "...";
            this.dgvc_qbe_authors.DefaultCellStyle = dataGridViewCellStyle15;
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
            dataGridViewCellStyle16.Format = "N0";
            dataGridViewCellStyle16.NullValue = null;
            this.dgvc_au_ord.DefaultCellStyle = dataGridViewCellStyle16;
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
            // tabPageTitleAuthors
            // 
            this.tabPageTitleAuthors.Location = new System.Drawing.Point(1, 40);
            this.tabPageTitleAuthors.Name = "tabPageTitleAuthors";
            this.tabPageTitleAuthors.Size = new System.Drawing.Size(956, 457);
            this.tabPageTitleAuthors.Text = "Title Authors";
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "Titles";
            this.dataNavigator1.DelegateCurrencyManager = false;
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 507);
            this.dataNavigator1.ManageNavigation = false;
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(966, 65);
            this.dataNavigator1.TabIndex = 23;
            this.dataNavigator1.Maximized += new Passero.Framework.Controls.DataNavigator.MaximizedEventHandler(this.dataNavigator1_Maximized);
            this.dataNavigator1.Minimized += new Passero.Framework.Controls.DataNavigator.MinimizedEventHandler(this.dataNavigator1_Minimized);
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
            this.dataNavigator1.eDelete += new Passero.Framework.Controls.DataNavigator.eDeleteEventHandler(this.dataNavigator1_eDelete);
            this.dataNavigator1.eDeleteCompleted += new Passero.Framework.Controls.DataNavigator.eDeleteCompletedEventHandler(this.dataNavigator1_eDeleteCompleted);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eSave += new Passero.Framework.Controls.DataNavigator.eSaveEventHandler(this.dataNavigator1_eSave);
            this.dataNavigator1.eSaveCompleted += new Passero.Framework.Controls.DataNavigator.eSaveCompletedEventHandler(this.dataNavigator1_eSaveCompleted);
            this.dataNavigator1.eUndo += new Passero.Framework.Controls.DataNavigator.eUndoEventHandler(this.dataNavigator1_eUndo);
            this.dataNavigator1.eDeleteRequest += new Passero.Framework.Controls.DataNavigator.eDeleteRequestEventHandler(this.dataNavigator1_eDeleteRequest);
            this.dataNavigator1.eSaveRequest += new Passero.Framework.Controls.DataNavigator.eSaveRequestEventHandler(this.dataNavigator1_eSaveRequest);
            // 
            // frmTitle
            // 
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromName("@window");
            this.ClientSize = new System.Drawing.Size(966, 572);
            this.Controls.Add(this.dataNavigator1);
            this.Controls.Add(this.tabctlTitle);
            this.Name = "frmTitle";
            this.Text = "Titles";
            this.Load += new System.EventHandler(this.frmTitle_Load);
            this.Resize += new System.EventHandler(this.frmTitle_Resize);
            this.tabctlTitle.ResumeLayout(false);
            this.tabPageTitles.ResumeLayout(false);
            this.pnl_Title.ResumeLayout(false);
            this.pnl_Title.PerformLayout();
            this.pnl_TitleInfo.ResumeLayout(false);
            this.pnl_TitleInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsTitles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Royalties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Advance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Price)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_YtdSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TitleAuthors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.TabControl tabctlTitle;
        private Wisej.Web.TabPage tabPageTitles;
        private Wisej.Web.TabPage tabPageTitleAuthors;
        private Wisej.Web.BindingSource bsTitles;
        private Wisej.Web.DataGridView dgv_TitleAuthors;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_title_id;
        private Wisej.Web.DataGridViewMaskedTextBoxColumn dgvc_au_id;
        private Wisej.Web.DataGridViewButtonColumn dgvc_qbe_authors;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_au_fullname;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_au_ord;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_royaltyper;
        private Wisej.Web.FlowLayoutPanel pnl_Title;
        private Wisej.Web.TextBox txt_title_id;
        private Wisej.Web.DateTimePicker dtp_pubbdate;
        private Wisej.Web.TextBox txt_type;
        private Wisej.Web.TextBox txt_title;
        private Wisej.Web.ComboBox cmb_pub_id;
        private Wisej.Web.NumericUpDown txt_Price;
        private Wisej.Web.NumericUpDown txt_Advance;
        private Wisej.Web.NumericUpDown txt_Royalties;
        private Wisej.Web.NumericUpDown txt_YtdSales;
        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.TextBox txt_notes;
        private Wisej.Web.FlowLayoutPanel pnl_TitleInfo;
    }
}