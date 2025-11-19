namespace PasseroDemo.Views
{
    partial class frmSales
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
            Wisej.Web.RequiredValidationRule requiredValidationRule1 = new Wisej.Web.RequiredValidationRule();
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool2 = new Wisej.Web.ComponentTool();
            Wisej.Web.RequiredValidationRule requiredValidationRule2 = new Wisej.Web.RequiredValidationRule();
            Wisej.Web.RequiredValidationRule requiredValidationRule3 = new Wisej.Web.RequiredValidationRule();
            Wisej.Web.RequiredValidationRule requiredValidationRule4 = new Wisej.Web.RequiredValidationRule();
            Wisej.Web.RequiredValidationRule requiredValidationRule5 = new Wisej.Web.RequiredValidationRule();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.tabSales = new Wisej.Web.TabControl();
            this.tabPageSalesMaster = new Wisej.Web.TabPage();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.pnl_SalesMaster = new Wisej.Web.FlowLayoutPanel();
            this.txt_ord_num = new Wisej.Web.TextBox();
            this.bsSalesmaster = new Wisej.Web.BindingSource(this.components);
            this.dtp_ord_date = new Wisej.Web.DateTimePicker();
            this.txt_stor_id = new Wisej.Web.MaskedTextBox();
            this.txt_stor_name = new Wisej.Web.TextBox();
            this.txt_stor_ord_num = new Wisej.Web.TextBox();
            this.dtp_stor_ord_date = new Wisej.Web.DateTimePicker();
            this.cmb_payterms = new Wisej.Web.ComboBox();
            this.dgv_SalesDetails = new Wisej.Web.DataGridView();
            this.dgvc_ord_num = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_title_id = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_qbe_titles = new Wisej.Web.DataGridViewButtonColumn();
            this.dgvc_title = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_qty = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.dgvc_price = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.tabPageSalesDetails = new Wisej.Web.TabPage();
            this.validation1 = new Wisej.Web.Validation(this.components);
            this.tabSales.SuspendLayout();
            this.tabPageSalesMaster.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnl_SalesMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSalesmaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SalesDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 408);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(1006, 64);
            this.dataNavigator1.TabIndex = 2;
            this.dataNavigator1.eAddNewCompleted += new Passero.Framework.Controls.DataNavigator.eAddNewCompletedEventHandler(this.dataNavigator1_eAddNewCompleted);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eSaveCompleted += new Passero.Framework.Controls.DataNavigator.eSaveCompletedEventHandler(this.dataNavigator1_eSaveCompleted);
            this.dataNavigator1.eUndoCompleted += new Passero.Framework.Controls.DataNavigator.eUndoCompletedEventHandler(this.dataNavigator1_eUndoCompleted);
            this.dataNavigator1.eBoundCompleted += new Passero.Framework.Controls.DataNavigator.eBoundCompletedEventHandler(this.dataNavigator1_eBoundCompleted);
            this.dataNavigator1.eAfterAddNewRequest += new Passero.Framework.Controls.DataNavigator.eAfterAddNewEventHandler(this.dataNavigator1_eAfterAddNewRequest);
            this.dataNavigator1.eSaveRequest += new Passero.Framework.Controls.DataNavigator.eSaveRequestEventHandler(this.dataNavigator1_eSaveRequest);
            // 
            // tabSales
            // 
            this.tabSales.BorderStyle = Wisej.Web.BorderStyle.None;
            this.tabSales.Controls.Add(this.tabPageSalesMaster);
            this.tabSales.Controls.Add(this.tabPageSalesDetails);
            this.tabSales.Dock = Wisej.Web.DockStyle.Fill;
            this.tabSales.Location = new System.Drawing.Point(0, 0);
            this.tabSales.Name = "tabSales";
            this.tabSales.PageInsets = new Wisej.Web.Padding(0, 40, 0, 0);
            this.tabSales.Size = new System.Drawing.Size(1006, 408);
            this.tabSales.TabIndex = 3;
            this.tabSales.SelectedIndexChanged += new System.EventHandler(this.tabSales_SelectedIndexChanged);
            // 
            // tabPageSalesMaster
            // 
            this.tabPageSalesMaster.AutoScroll = true;
            this.tabPageSalesMaster.Controls.Add(this.flowLayoutPanel1);
            this.tabPageSalesMaster.Location = new System.Drawing.Point(0, 40);
            this.tabPageSalesMaster.Name = "tabPageSalesMaster";
            this.tabPageSalesMaster.Size = new System.Drawing.Size(1006, 368);
            this.tabPageSalesMaster.Text = "Sales Master";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.pnl_SalesMaster);
            this.flowLayoutPanel1.Controls.Add(this.dgv_SalesDetails);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1006, 368);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // pnl_SalesMaster
            // 
            this.pnl_SalesMaster.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.pnl_SalesMaster.AutoSize = true;
            this.pnl_SalesMaster.AutoSizeMode = Wisej.Web.AutoSizeMode.GrowAndShrink;
            this.pnl_SalesMaster.Controls.Add(this.txt_ord_num);
            this.pnl_SalesMaster.Controls.Add(this.dtp_ord_date);
            this.pnl_SalesMaster.Controls.Add(this.txt_stor_id);
            this.pnl_SalesMaster.Controls.Add(this.txt_stor_name);
            this.pnl_SalesMaster.Controls.Add(this.txt_stor_ord_num);
            this.pnl_SalesMaster.Controls.Add(this.dtp_stor_ord_date);
            this.pnl_SalesMaster.Controls.Add(this.cmb_payterms);
            this.flowLayoutPanel1.SetFillWeight(this.pnl_SalesMaster, 100);
            this.flowLayoutPanel1.SetFlowBreak(this.pnl_SalesMaster, true);
            this.pnl_SalesMaster.Location = new System.Drawing.Point(3, 3);
            this.pnl_SalesMaster.Name = "pnl_SalesMaster";
            this.pnl_SalesMaster.Size = new System.Drawing.Size(1000, 108);
            this.pnl_SalesMaster.TabIndex = 12;
            // 
            // txt_ord_num
            // 
            this.txt_ord_num.CausesValidation = false;
            this.txt_ord_num.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsSalesmaster, "ord_num", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_ord_num.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_ord_num.LabelText = "Order Number";
            this.txt_ord_num.Location = new System.Drawing.Point(3, 3);
            this.txt_ord_num.MaxLength = 20;
            this.txt_ord_num.Name = "txt_ord_num";
            this.txt_ord_num.ReadOnly = true;
            this.txt_ord_num.Size = new System.Drawing.Size(80, 48);
            this.txt_ord_num.TabIndex = 0;
            // 
            // bsSalesmaster
            // 
            this.bsSalesmaster.DataSource = typeof(PasseroDemo.Models.Salesmaster);
            // 
            // dtp_ord_date
            // 
            this.dtp_ord_date.DataBindings.Add(new Wisej.Web.Binding("NullableValue", this.bsSalesmaster, "ord_date", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dtp_ord_date.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsSalesmaster, "ord_date", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dtp_ord_date.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtp_ord_date.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtp_ord_date.LabelText = "Order Date";
            this.dtp_ord_date.Location = new System.Drawing.Point(89, 3);
            this.dtp_ord_date.Name = "dtp_ord_date";
            this.dtp_ord_date.Size = new System.Drawing.Size(108, 48);
            this.dtp_ord_date.TabIndex = 1;
            requiredValidationRule1.InvalidMessage = "Occorre specificare una data!";
            this.validation1.SetValidationRules(this.dtp_ord_date, new Wisej.Web.ValidationRule[] {
            ((Wisej.Web.ValidationRule)(requiredValidationRule1))});
            this.dtp_ord_date.Value = new System.DateTime(((long)(0)));
            // 
            // txt_stor_id
            // 
            this.txt_stor_id.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left)));
            this.txt_stor_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsSalesmaster, "stor_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_stor_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_stor_id.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_stor_id.LabelText = "Store ";
            this.txt_stor_id.Location = new System.Drawing.Point(203, 3);
            this.txt_stor_id.Name = "txt_stor_id";
            this.txt_stor_id.ReadOnly = true;
            this.txt_stor_id.Size = new System.Drawing.Size(100, 48);
            this.txt_stor_id.TabIndex = 2;
            componentTool1.ImageSource = "icon-search";
            componentTool1.Name = "search";
            componentTool1.ToolTipText = "Search Store";
            componentTool2.ImageSource = "tab-close";
            componentTool2.Name = "clear";
            componentTool2.ToolTipText = "Clear Store";
            this.txt_stor_id.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1,
            componentTool2});
            requiredValidationRule2.InvalidMessage = "Occorre specificare il codice Store!";
            this.validation1.SetValidationRules(this.txt_stor_id, new Wisej.Web.ValidationRule[] {
            ((Wisej.Web.ValidationRule)(requiredValidationRule2))});
            this.txt_stor_id.ToolClick += new Wisej.Web.ToolClickEventHandler(this.txt_stor_id_ToolClick);
            this.txt_stor_id.TextChanged += new System.EventHandler(this.txt_stor_id_TextChanged);
            // 
            // txt_stor_name
            // 
            this.txt_stor_name.AutoSize = false;
            this.txt_stor_name.BackColor = System.Drawing.SystemColors.Window;
            this.txt_stor_name.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_stor_name.LabelText = "Store Description";
            this.txt_stor_name.Location = new System.Drawing.Point(309, 3);
            this.txt_stor_name.Name = "txt_stor_name";
            this.txt_stor_name.ReadOnly = true;
            this.txt_stor_name.Size = new System.Drawing.Size(235, 48);
            this.txt_stor_name.TabIndex = 3;
            this.txt_stor_name.TabStop = false;
            // 
            // txt_stor_ord_num
            // 
            this.txt_stor_ord_num.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsSalesmaster, "stor_ord_num", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_stor_ord_num.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_stor_ord_num.LabelText = "Store Order Num.";
            this.txt_stor_ord_num.Location = new System.Drawing.Point(550, 3);
            this.txt_stor_ord_num.MaxLength = 20;
            this.txt_stor_ord_num.Name = "txt_stor_ord_num";
            this.txt_stor_ord_num.Size = new System.Drawing.Size(141, 48);
            this.txt_stor_ord_num.TabIndex = 5;
            requiredValidationRule3.InvalidMessage = "Occorre specificare un numero Ordine!";
            this.validation1.SetValidationRules(this.txt_stor_ord_num, new Wisej.Web.ValidationRule[] {
            ((Wisej.Web.ValidationRule)(requiredValidationRule3))});
            // 
            // dtp_stor_ord_date
            // 
            this.dtp_stor_ord_date.DataBindings.Add(new Wisej.Web.Binding("NullableValue", this.bsSalesmaster, "stor_ord_date", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.dtp_stor_ord_date.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsSalesmaster, "stor_ord_date", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.pnl_SalesMaster.SetFlowBreak(this.dtp_stor_ord_date, true);
            this.dtp_stor_ord_date.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtp_stor_ord_date.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtp_stor_ord_date.LabelText = "Store Order Date";
            this.dtp_stor_ord_date.Location = new System.Drawing.Point(697, 3);
            this.dtp_stor_ord_date.Name = "dtp_stor_ord_date";
            this.dtp_stor_ord_date.Size = new System.Drawing.Size(108, 48);
            this.dtp_stor_ord_date.TabIndex = 6;
            requiredValidationRule4.InvalidMessage = "Occorre specificare una data!";
            this.validation1.SetValidationRules(this.dtp_stor_ord_date, new Wisej.Web.ValidationRule[] {
            ((Wisej.Web.ValidationRule)(requiredValidationRule4))});
            this.dtp_stor_ord_date.Value = new System.DateTime(((long)(0)));
            // 
            // cmb_payterms
            // 
            this.cmb_payterms.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsSalesmaster, "payterms", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.cmb_payterms.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.pnl_SalesMaster.SetFlowBreak(this.cmb_payterms, true);
            this.cmb_payterms.Items.AddRange(new object[] {
            "ON Invoice",
            "Net 30",
            "Net 60",
            "Net 90",
            "Net 120",
            "Gift"});
            this.cmb_payterms.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_payterms.LabelText = "Payment Terms";
            this.cmb_payterms.Location = new System.Drawing.Point(3, 57);
            this.cmb_payterms.Name = "cmb_payterms";
            this.cmb_payterms.Size = new System.Drawing.Size(168, 48);
            this.cmb_payterms.TabIndex = 4;
            requiredValidationRule5.InvalidMessage = "Occorre specificare una modalità di pagamento!";
            this.validation1.SetValidationRules(this.cmb_payterms, new Wisej.Web.ValidationRule[] {
            ((Wisej.Web.ValidationRule)(requiredValidationRule5))});
            // 
            // dgv_SalesDetails
            // 
            this.dgv_SalesDetails.AutoGenerateColumns = false;
            this.dgv_SalesDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvc_ord_num,
            this.dgvc_title_id,
            this.dgvc_qbe_titles,
            this.dgvc_title,
            this.dgvc_qty,
            this.dgvc_price});
            this.dgv_SalesDetails.DefaultRowHeight = 20;
            this.flowLayoutPanel1.SetFillWeight(this.dgv_SalesDetails, 100);
            this.dgv_SalesDetails.Location = new System.Drawing.Point(3, 117);
            this.dgv_SalesDetails.Name = "dgv_SalesDetails";
            this.dgv_SalesDetails.SelectionMode = Wisej.Web.DataGridViewSelectionMode.NoSelection;
            this.dgv_SalesDetails.Size = new System.Drawing.Size(1000, 245);
            this.dgv_SalesDetails.TabIndex = 2;
            this.dgv_SalesDetails.DataBindingComplete += new Wisej.Web.DataGridViewBindingCompleteEventHandler(this.dgv_SalesDetails_DataBindingComplete);
            this.dgv_SalesDetails.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.dgv_SalesDetails_CellClick);
            // 
            // dgvc_ord_num
            // 
            this.dgvc_ord_num.DataPropertyName = "ord_num";
            this.dgvc_ord_num.HeaderText = "dgvc_ord_num";
            this.dgvc_ord_num.Name = "dgvc_ord_num";
            // 
            // dgvc_title_id
            // 
            this.dgvc_title_id.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.dgvc_title_id.DataPropertyName = "title_id";
            this.dgvc_title_id.HeaderText = "Title ID";
            this.dgvc_title_id.MaxInputLength = 6;
            this.dgvc_title_id.Name = "dgvc_title_id";
            this.dgvc_title_id.Width = 120;
            // 
            // dgvc_qbe_titles
            // 
            this.dgvc_qbe_titles.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.NullValue = "...";
            this.dgvc_qbe_titles.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvc_qbe_titles.HeaderText = "?";
            this.dgvc_qbe_titles.Name = "dgvc_qbe_titles";
            this.dgvc_qbe_titles.Text = "...";
            this.dgvc_qbe_titles.Width = 20;
            // 
            // dgvc_title
            // 
            this.dgvc_title.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.dgvc_title.HeaderText = "Title";
            this.dgvc_title.Name = "dgvc_title";
            this.dgvc_title.ReadOnly = true;
            this.dgvc_title.Width = 200;
            // 
            // dgvc_qty
            // 
            this.dgvc_qty.DataPropertyName = "qty";
            dataGridViewCellStyle2.Format = "N";
            this.dgvc_qty.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvc_qty.HeaderText = "Qty";
            this.dgvc_qty.HideUpDownButtons = true;
            this.dgvc_qty.Increment = new decimal(0);
            this.dgvc_qty.Maximum = new decimal(9999);
            this.dgvc_qty.Name = "dgvc_qty";
            // 
            // dgvc_price
            // 
            this.dgvc_price.DataPropertyName = "price";
            this.dgvc_price.DecimalPlaces = 2;
            dataGridViewCellStyle3.Format = "N";
            this.dgvc_price.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvc_price.HeaderText = "Unit Price";
            this.dgvc_price.HideUpDownButtons = true;
            this.dgvc_price.Increment = new decimal(0);
            this.dgvc_price.Maximum = new decimal(99999);
            this.dgvc_price.Name = "dgvc_price";
            // 
            // tabPageSalesDetails
            // 
            this.tabPageSalesDetails.AutoScroll = true;
            this.tabPageSalesDetails.Location = new System.Drawing.Point(0, 40);
            this.tabPageSalesDetails.Name = "tabPageSalesDetails";
            this.tabPageSalesDetails.Size = new System.Drawing.Size(1006, 368);
            this.tabPageSalesDetails.Text = "Sales Details";
            // 
            // frmSales
            // 
            this.ClientSize = new System.Drawing.Size(1006, 472);
            this.Controls.Add(this.tabSales);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmSales";
            this.Text = "Sales";
            this.Load += new System.EventHandler(this.frmSales_Load);
            this.Resize += new System.EventHandler(this.frmSales_Resize);
            this.tabSales.ResumeLayout(false);
            this.tabPageSalesMaster.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.pnl_SalesMaster.ResumeLayout(false);
            this.pnl_SalesMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSalesmaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SalesDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.TabControl tabSales;
        private Wisej.Web.TabPage tabPageSalesMaster;
        private Wisej.Web.FlowLayoutPanel  pnl_SalesMaster;
        private Wisej.Web.ComboBox cmb_payterms;
        private Wisej.Web.DateTimePicker dtp_stor_ord_date;
        private Wisej.Web.TextBox txt_stor_ord_num;
        private Wisej.Web.MaskedTextBox txt_stor_id;
        private Wisej.Web.TextBox txt_stor_name;
        private Wisej.Web.DateTimePicker dtp_ord_date;
        private Wisej.Web.TextBox txt_ord_num;
        private Wisej.Web.DataGridView dgv_SalesDetails;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_ord_num;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_title_id;
        private Wisej.Web.DataGridViewButtonColumn dgvc_qbe_titles;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_title;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_qty;
        private Wisej.Web.DataGridViewNumericUpDownColumn dgvc_price;
        private Wisej.Web.TabPage tabPageSalesDetails;
        private Wisej.Web.BindingSource bsSalesmaster;
        private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
        private Wisej.Web.Validation validation1;
    }
}