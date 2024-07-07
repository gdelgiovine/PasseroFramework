namespace PasseroDemo.Views
{
    partial class frmDiscount
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
            this.txt_Discount_discount_id = new Wisej.Web.TextBox();
            this.bsDiscount = new Wisej.Web.BindingSource(this.components);
            this.txt_Discount_discount_type = new Wisej.Web.TextBox();
            this.txt_Discount_discount = new Wisej.Web.NumericUpDown();
            this.txt_Discount_lowqty = new Wisej.Web.NumericUpDown();
            this.txt_Discount_highqty = new Wisej.Web.NumericUpDown();
            this.txt_Store_stor_name = new Wisej.Web.TextBox();
            this.txt_Store_stor_city = new Wisej.Web.TextBox();
            this.textBox1 = new Wisej.Web.TextBox();
            this.txt_Discount_store_id = new Passero.Framework.Controls.DbLookUpTextBox(this.components);
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.flpDiscount = new Wisej.Web.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.bsDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Discount_discount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Discount_lowqty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Discount_highqty)).BeginInit();
            this.flpDiscount.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Discount_discount_id
            // 
            this.txt_Discount_discount_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsDiscount, "discount_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Discount_discount_id.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Discount_discount_id.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Discount_discount_id.LabelText = "Discount Id";
            this.txt_Discount_discount_id.Location = new System.Drawing.Point(3, 3);
            this.txt_Discount_discount_id.Name = "txt_Discount_discount_id";
            this.txt_Discount_discount_id.Size = new System.Drawing.Size(85, 44);
            this.txt_Discount_discount_id.TabIndex = 0;
            // 
            // bsDiscount
            // 
            this.bsDiscount.DataSource = typeof(PasseroDemo.Models.Discount);
            this.bsDiscount.Sort = "discount_id";
            this.bsDiscount.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.bsDiscount_AddingNew);
            // 
            // txt_Discount_discount_type
            // 
            this.txt_Discount_discount_type.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsDiscount, "discounttype", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Discount_discount_type.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Discount_discount_type.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Discount_discount_type.LabelText = "Discount Type";
            this.txt_Discount_discount_type.Location = new System.Drawing.Point(94, 3);
            this.txt_Discount_discount_type.Name = "txt_Discount_discount_type";
            this.txt_Discount_discount_type.Size = new System.Drawing.Size(193, 44);
            this.txt_Discount_discount_type.TabIndex = 1;
            // 
            // txt_Discount_discount
            // 
            this.txt_Discount_discount.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsDiscount, "discount", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Discount_discount.DecimalPlaces = 2;
            this.flpDiscount.SetFlowBreak(this.txt_Discount_discount, true);
            this.txt_Discount_discount.HideUpDownButtons = true;
            this.txt_Discount_discount.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Discount_discount.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Discount_discount.LabelText = "Discount %";
            this.txt_Discount_discount.Location = new System.Drawing.Point(175, 53);
            this.txt_Discount_discount.Name = "txt_Discount_discount";
            this.txt_Discount_discount.Size = new System.Drawing.Size(80, 46);
            this.txt_Discount_discount.TabIndex = 5;
            this.txt_Discount_discount.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txt_Discount_lowqty
            // 
            this.txt_Discount_lowqty.HideUpDownButtons = true;
            this.txt_Discount_lowqty.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Discount_lowqty.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Discount_lowqty.LabelText = "Low Qty";
            this.txt_Discount_lowqty.Location = new System.Drawing.Point(3, 53);
            this.txt_Discount_lowqty.Maximum = new decimal(9999999);
            this.txt_Discount_lowqty.Name = "txt_Discount_lowqty";
            this.txt_Discount_lowqty.Size = new System.Drawing.Size(80, 46);
            this.txt_Discount_lowqty.TabIndex = 3;
            this.txt_Discount_lowqty.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txt_Discount_highqty
            // 
            this.txt_Discount_highqty.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsDiscount, "highqty", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Discount_highqty.HideUpDownButtons = true;
            this.txt_Discount_highqty.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Discount_highqty.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Discount_highqty.LabelText = "High Qty";
            this.txt_Discount_highqty.Location = new System.Drawing.Point(89, 53);
            this.txt_Discount_highqty.Maximum = new decimal(9999999);
            this.txt_Discount_highqty.Name = "txt_Discount_highqty";
            this.txt_Discount_highqty.Size = new System.Drawing.Size(80, 46);
            this.txt_Discount_highqty.TabIndex = 4;
            this.txt_Discount_highqty.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txt_Store_stor_name
            // 
            this.txt_Store_stor_name.AutoSize = false;
            this.txt_Store_stor_name.BackColor = System.Drawing.Color.FromName("@gray-100");
            this.txt_Store_stor_name.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Store_stor_name.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Store_stor_name.LabelText = "Store Name";
            this.txt_Store_stor_name.Location = new System.Drawing.Point(129, 105);
            this.txt_Store_stor_name.Name = "txt_Store_stor_name";
            this.txt_Store_stor_name.ReadOnly = true;
            this.txt_Store_stor_name.Size = new System.Drawing.Size(287, 44);
            this.txt_Store_stor_name.TabIndex = 7;
            // 
            // txt_Store_stor_city
            // 
            this.txt_Store_stor_city.AutoSize = false;
            this.txt_Store_stor_city.BackColor = System.Drawing.Color.FromName("@gray-100");
            this.txt_Store_stor_city.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Store_stor_city.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Store_stor_city.LabelText = "Store City";
            this.txt_Store_stor_city.Location = new System.Drawing.Point(422, 105);
            this.txt_Store_stor_city.Name = "txt_Store_stor_city";
            this.txt_Store_stor_city.ReadOnly = true;
            this.txt_Store_stor_city.Size = new System.Drawing.Size(215, 44);
            this.txt_Store_stor_city.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsDiscount, "stor_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flpDiscount.SetFlowBreak(this.textBox1, true);
            this.textBox1.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.textBox1.LabelText = "Discount Type";
            this.textBox1.Location = new System.Drawing.Point(293, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(122, 44);
            this.textBox1.TabIndex = 2;
            // 
            // txt_Discount_store_id
            // 
            this.txt_Discount_store_id.AutoSize = false;
            this.txt_Discount_store_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsDiscount, "stor_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_Discount_store_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_Discount_store_id.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_Discount_store_id.LabelText = "Store Id";
            this.txt_Discount_store_id.Location = new System.Drawing.Point(3, 105);
            this.txt_Discount_store_id.Name = "txt_Discount_store_id";
            this.txt_Discount_store_id.Size = new System.Drawing.Size(120, 44);
            this.txt_Discount_store_id.TabIndex = 6;
            componentTool1.ImageSource = "icon-search";
            componentTool1.Name = "search";
            componentTool1.ToolTipText = "Search";
            this.txt_Discount_store_id.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.txt_Discount_store_id.TextChanged += new System.EventHandler(this.txt_Discount_store_id_TextChanged);
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "Discounts";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 281);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(664, 70);
            this.dataNavigator1.TabIndex = 0;
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
            this.dataNavigator1.eAddNewCompleted += new Passero.Framework.Controls.DataNavigator.eAddNewCompletedEventHandler(this.dataNavigator1_eAddNewCompleted);
            this.dataNavigator1.ePrint += new Passero.Framework.Controls.DataNavigator.ePrintEventHandler(this.dataNavigator1_ePrint);
            this.dataNavigator1.eDelete += new Passero.Framework.Controls.DataNavigator.eDeleteEventHandler(this.dataNavigator1_eDelete);
            this.dataNavigator1.eRefresh += new Passero.Framework.Controls.DataNavigator.eRefreshEventHandler(this.dataNavigator1_eRefresh);
            this.dataNavigator1.eClose += new Passero.Framework.Controls.DataNavigator.eCloseEventHandler(this.dataNavigator1_eClose);
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            this.dataNavigator1.eSave += new Passero.Framework.Controls.DataNavigator.eSaveEventHandler(this.dataNavigator1_eSave);
            this.dataNavigator1.eMovePrevious += new Passero.Framework.Controls.DataNavigator.eMovePreviousEventHandler(this.dataNavigator1_eMovePrevious);
            this.dataNavigator1.eMoveFirst += new Passero.Framework.Controls.DataNavigator.eMoveFirstEventHandler(this.dataNavigator1_eMoveFirst);
            this.dataNavigator1.eMoveLast += new Passero.Framework.Controls.DataNavigator.eMoveLastEventHandler(this.dataNavigator1_eMoveLast);
            this.dataNavigator1.eMoveNext += new Passero.Framework.Controls.DataNavigator.eMoveNextEventHandler(this.dataNavigator1_eMoveNext);
            this.dataNavigator1.eUndo += new Passero.Framework.Controls.DataNavigator.eUndoEventHandler(this.dataNavigator1_eUndo);
            this.dataNavigator1.eBoundCompleted += new Passero.Framework.Controls.DataNavigator.eBoundCompletedEventHandler(this.dataNavigator1_eBoundCompleted);
            this.dataNavigator1.eDeleteRequest += new Passero.Framework.Controls.DataNavigator.eDeleteRequestEventHandler(this.dataNavigator1_eDeleteRequest);
            // 
            // flpDiscount
            // 
            this.flpDiscount.Controls.Add(this.txt_Discount_discount_id);
            this.flpDiscount.Controls.Add(this.txt_Discount_discount_type);
            this.flpDiscount.Controls.Add(this.textBox1);
            this.flpDiscount.Controls.Add(this.txt_Discount_lowqty);
            this.flpDiscount.Controls.Add(this.txt_Discount_highqty);
            this.flpDiscount.Controls.Add(this.txt_Discount_discount);
            this.flpDiscount.Controls.Add(this.txt_Discount_store_id);
            this.flpDiscount.Controls.Add(this.txt_Store_stor_name);
            this.flpDiscount.Controls.Add(this.txt_Store_stor_city);
            this.flpDiscount.Dock = Wisej.Web.DockStyle.Fill;
            this.flpDiscount.Location = new System.Drawing.Point(0, 0);
            this.flpDiscount.Name = "flpDiscount";
            this.flpDiscount.Size = new System.Drawing.Size(664, 281);
            this.flpDiscount.TabIndex = 9;
            // 
            // frmDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 351);
            this.Controls.Add(this.flpDiscount);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmDiscount";
            this.Text = "Discount";
            this.Load += new System.EventHandler(this.frmPasseroBaseView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Discount_discount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Discount_lowqty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Discount_highqty)).EndInit();
            this.flpDiscount.ResumeLayout(false);
            this.flpDiscount.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        internal Wisej.Web.TextBox txt_Discount_discount_id;
        internal Wisej.Web.TextBox txt_Discount_discount_type;
        private Wisej.Web.BindingSource bsDiscount;
        private Wisej.Web.NumericUpDown txt_Discount_discount;
        private Wisej.Web.NumericUpDown txt_Discount_lowqty;
        private Wisej.Web.NumericUpDown txt_Discount_highqty;
        private Passero.Framework.Controls.DbLookUpTextBox txt_Discount_store_id;
        private Wisej.Web.TextBox txt_Store_stor_name;
        private Wisej.Web.TextBox txt_Store_stor_city;
        internal Wisej.Web.TextBox textBox1;
        private Wisej.Web.FlowLayoutPanel flpDiscount;
    }
}