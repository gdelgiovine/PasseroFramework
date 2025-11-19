namespace PasseroDemo.Views
{
    partial class frmRoySched
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
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            this.tabRoyalties = new Wisej.Web.TabControl();
            this.tabPageTitles = new Wisej.Web.TabPage();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.txt_title_id = new Wisej.Web.TextBox();
            this.bsTitle = new Wisej.Web.BindingSource(this.components);
            this.txt_title = new Wisej.Web.TextBox();
            this.txt_title_type = new Wisej.Web.TextBox();
            this.txt_pub_name = new Wisej.Web.TextBox();
            this.txt_title_pubdate = new Wisej.Web.TextBox();
            this.dgvRoyalties = new Wisej.Web.DataGridView();
            this.dgvc_title_id = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.dgvc_lorange = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.dgvc_hirange = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.dgvc_royalty = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.tabPageRoyalties = new Wisej.Web.TabPage();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.tabRoyalties.SuspendLayout();
            this.tabPageTitles.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoyalties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabRoyalties
            // 
            this.tabRoyalties.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.tabRoyalties.Controls.Add(this.tabPageTitles);
            this.tabRoyalties.Controls.Add(this.tabPageRoyalties);
            this.tabRoyalties.Location = new System.Drawing.Point(8, 8);
            this.tabRoyalties.Name = "tabRoyalties";
            this.tabRoyalties.PageInsets = new Wisej.Web.Padding(1, 40, 1, 1);
            this.tabRoyalties.Size = new System.Drawing.Size(731, 380);
            this.tabRoyalties.TabIndex = 22;
            this.tabRoyalties.SelectedIndexChanged += new System.EventHandler(this.tabRoyalties_SelectedIndexChanged);
            // 
            // tabPageTitles
            // 
            this.tabPageTitles.Controls.Add(this.flowLayoutPanel1);
            this.tabPageTitles.Location = new System.Drawing.Point(1, 40);
            this.tabPageTitles.Name = "tabPageTitles";
            this.tabPageTitles.Size = new System.Drawing.Size(729, 339);
            this.tabPageTitles.Text = "Titles";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.txt_title_id);
            this.flowLayoutPanel1.Controls.Add(this.txt_title);
            this.flowLayoutPanel1.Controls.Add(this.txt_title_type);
            this.flowLayoutPanel1.Controls.Add(this.txt_pub_name);
            this.flowLayoutPanel1.Controls.Add(this.txt_title_pubdate);
            this.flowLayoutPanel1.Controls.Add(this.dgvRoyalties);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(723, 333);
            this.flowLayoutPanel1.TabIndex = 19;
            // 
            // txt_title_id
            // 
            this.txt_title_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitle, "title_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_title_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title_id.Label.Padding = new Wisej.Web.Padding(0);
            this.txt_title_id.LabelText = "Title ID";
            this.txt_title_id.Location = new System.Drawing.Point(3, 3);
            this.txt_title_id.Name = "txt_title_id";
            this.txt_title_id.ReadOnly = true;
            this.txt_title_id.Size = new System.Drawing.Size(100, 43);
            this.txt_title_id.TabIndex = 14;
            componentTool1.ImageSource = "icon-search";
            componentTool1.Name = "Search";
            componentTool1.ToolTipText = "Search Title";
            componentTool1.Visible = false;
            this.txt_title_id.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.txt_title_id.ToolClick += new Wisej.Web.ToolClickEventHandler(this.txt_title_id_ToolClick);
            // 
            // bsTitle
            // 
            this.bsTitle.DataSource = typeof(PasseroDemo.Models.Title);
            // 
            // txt_title
            // 
            this.txt_title.BackColor = System.Drawing.Color.FromName("@tabHighlight");
            this.txt_title.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitle, "title", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_title, 80);
            this.flowLayoutPanel1.SetFlowBreak(this.txt_title, true);
            this.txt_title.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title.Label.Padding = new Wisej.Web.Padding(0);
            this.txt_title.LabelText = "Title";
            this.txt_title.Location = new System.Drawing.Point(109, 3);
            this.txt_title.Name = "txt_title";
            this.txt_title.ReadOnly = true;
            this.txt_title.Size = new System.Drawing.Size(611, 43);
            this.txt_title.TabIndex = 15;
            // 
            // txt_title_type
            // 
            this.txt_title_type.BackColor = System.Drawing.Color.FromName("@tabHighlight");
            this.txt_title_type.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitle, "type", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_title_type, 40);
            this.txt_title_type.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title_type.Label.Padding = new Wisej.Web.Padding(0);
            this.txt_title_type.LabelText = "Type";
            this.txt_title_type.Location = new System.Drawing.Point(3, 52);
            this.txt_title_type.Name = "txt_title_type";
            this.txt_title_type.ReadOnly = true;
            this.txt_title_type.Size = new System.Drawing.Size(270, 43);
            this.txt_title_type.TabIndex = 18;
            // 
            // txt_pub_name
            // 
            this.txt_pub_name.BackColor = System.Drawing.Color.FromName("@tabHighlight");
            this.txt_pub_name.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitle, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_pub_name, 50);
            this.txt_pub_name.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_pub_name.Label.Padding = new Wisej.Web.Padding(0);
            this.txt_pub_name.LabelText = "Publisher";
            this.txt_pub_name.Location = new System.Drawing.Point(279, 52);
            this.txt_pub_name.Name = "txt_pub_name";
            this.txt_pub_name.ReadOnly = true;
            this.txt_pub_name.Size = new System.Drawing.Size(338, 43);
            this.txt_pub_name.TabIndex = 16;
            // 
            // txt_title_pubdate
            // 
            this.txt_title_pubdate.BackColor = System.Drawing.Color.FromName("@tabHighlight");
            this.txt_title_pubdate.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsTitle, "pubdate", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFlowBreak(this.txt_title_pubdate, true);
            this.txt_title_pubdate.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_title_pubdate.Label.Padding = new Wisej.Web.Padding(0);
            this.txt_title_pubdate.LabelText = "Publishing date";
            this.txt_title_pubdate.Location = new System.Drawing.Point(623, 52);
            this.txt_title_pubdate.Name = "txt_title_pubdate";
            this.txt_title_pubdate.ReadOnly = true;
            this.txt_title_pubdate.Size = new System.Drawing.Size(97, 43);
            this.txt_title_pubdate.TabIndex = 17;
            // 
            // dgvRoyalties
            // 
            this.dgvRoyalties.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.dgvRoyalties.AutoGenerateColumns = false;
            this.dgvRoyalties.AutoSize = true;
            this.dgvRoyalties.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvc_title_id,
            this.dgvc_lorange,
            this.dgvc_hirange,
            this.dgvc_royalty});
            this.dgvRoyalties.Location = new System.Drawing.Point(3, 101);
            this.dgvRoyalties.Name = "dgvRoyalties";
            this.dgvRoyalties.ReadOnly = true;
            this.dgvRoyalties.SelectionMode = Wisej.Web.DataGridViewSelectionMode.NoSelection;
            this.dgvRoyalties.Size = new System.Drawing.Size(717, 229);
            this.dgvRoyalties.TabIndex = 13;
            // 
            // dgvc_title_id
            // 
            this.dgvc_title_id.DataPropertyName = "title_id";
            dataGridViewCellStyle1.Format = "N";
            this.dgvc_title_id.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvc_title_id.HeaderText = "Title ID";
            this.dgvc_title_id.Name = "dgvc_title_id";
            this.dgvc_title_id.ReadOnly = true;
            // 
            // dgvc_lorange
            // 
            this.dgvc_lorange.DataPropertyName = "lorange";
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.dgvc_lorange.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvc_lorange.HeaderText = "Low Range";
            this.dgvc_lorange.HideUpDownButtons = true;
            this.dgvc_lorange.Increment = new decimal(0);
            this.dgvc_lorange.Maximum = new decimal(99999999);
            this.dgvc_lorange.Name = "dgvc_lorange";
            // 
            // dgvc_hirange
            // 
            this.dgvc_hirange.DataPropertyName = "hirange";
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.dgvc_hirange.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvc_hirange.HeaderText = "High Range";
            this.dgvc_hirange.HideUpDownButtons = true;
            this.dgvc_hirange.Increment = new decimal(0);
            this.dgvc_hirange.Maximum = new decimal(99999999);
            this.dgvc_hirange.Name = "dgvc_hirange";
            // 
            // dgvc_royalty
            // 
            this.dgvc_royalty.DataPropertyName = "royalty";
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.dgvc_royalty.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvc_royalty.HeaderText = "Royalty";
            this.dgvc_royalty.HideUpDownButtons = true;
            this.dgvc_royalty.Increment = new decimal(0);
            this.dgvc_royalty.Name = "dgvc_royalty";
            // 
            // tabPageRoyalties
            // 
            this.tabPageRoyalties.Location = new System.Drawing.Point(1, 40);
            this.tabPageRoyalties.Name = "tabPageRoyalties";
            this.tabPageRoyalties.Size = new System.Drawing.Size(729, 339);
            this.tabPageRoyalties.Text = "Royalties";
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 394);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(749, 70);
            this.dataNavigator1.TabIndex = 21;
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eBoundCompleted += new Passero.Framework.Controls.DataNavigator.eBoundCompletedEventHandler(this.dataNavigator1_eBoundCompleted);
            this.dataNavigator1.eAddNewRequest += new Passero.Framework.Controls.DataNavigator.eAddNewRequestEventHandler(this.dataNavigator1_eAddNewRequest);
            this.dataNavigator1.eAfterAddNewRequest += new Passero.Framework.Controls.DataNavigator.eAfterAddNewEventHandler(this.dataNavigator1_eAfterAddNewRequest);
            // 
            // frmRoySched
            // 
            this.ClientSize = new System.Drawing.Size(749, 464);
            this.Controls.Add(this.tabRoyalties);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmRoySched";
            this.Text = "Royalties Sched.";
            this.Load += new System.EventHandler(this.frmRoySched_Load);
            this.tabRoyalties.ResumeLayout(false);
            this.tabPageTitles.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoyalties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.TabControl tabRoyalties;
        private Wisej.Web.TabPage tabPageTitles;
        private Wisej.Web.TextBox txt_title_id;
        //private Passero .Framework .Controls .DbLookUpTextBox  txt_title_id;
        private Wisej.Web.DataGridView dgvRoyalties;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_title_id;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_lorange;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_hirange;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_royalty;
        private Wisej.Web.TextBox txt_pub_name;
        private Wisej.Web.TextBox txt_title;
        private Wisej.Web.TabPage tabPageRoyalties;
        private Wisej.Web.BindingSource bsTitle;
        private Wisej.Web.TextBox txt_title_pubdate;
        private Wisej.Web.TextBox txt_title_type;
        private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
    }
}