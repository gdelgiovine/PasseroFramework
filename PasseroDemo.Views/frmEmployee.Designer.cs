namespace PasseroDemo.Views
{
    partial class frmEmployee
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
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.txt_emp_id = new Wisej.Web.TextBox();
            this.txt_fname = new Wisej.Web.TextBox();
            this.txt_minit = new Wisej.Web.TextBox();
            this.txt_lname = new Wisej.Web.TextBox();
            this.cmb_job_id = new Wisej.Web.ComboBox();
            this.txt_job_lvl = new Wisej.Web.TextBox();
            this.cmb_pub_id = new Wisej.Web.ComboBox();
            this.dtp_hire_date = new Wisej.Web.DateTimePicker();
            this.txt_phone = new Wisej.Web.TextBox();
            this.txt_email = new Wisej.Web.TextBox();
            this.bsEmployee = new Wisej.Web.BindingSource(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsEmployee)).BeginInit();
            this.SuspendLayout();
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "Employees";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 421);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(753, 60);
            this.dataNavigator1.TabIndex = 0;
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
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
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.txt_emp_id);
            this.flowLayoutPanel1.Controls.Add(this.txt_fname);
            this.flowLayoutPanel1.Controls.Add(this.txt_minit);
            this.flowLayoutPanel1.Controls.Add(this.txt_lname);
            this.flowLayoutPanel1.Controls.Add(this.txt_phone);
            this.flowLayoutPanel1.Controls.Add(this.txt_email);
            this.flowLayoutPanel1.Controls.Add(this.dtp_hire_date);
            this.flowLayoutPanel1.Controls.Add(this.cmb_job_id);
            this.flowLayoutPanel1.Controls.Add(this.txt_job_lvl);
            this.flowLayoutPanel1.Controls.Add(this.cmb_pub_id);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(753, 421);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // txt_emp_id
            // 
            this.txt_emp_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "emp_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_emp_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_emp_id.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_emp_id.LabelText = "Emp. ID";
            this.txt_emp_id.Location = new System.Drawing.Point(5, 5);
            this.txt_emp_id.Margin = new Wisej.Web.Padding(2);
            this.txt_emp_id.Name = "txt_emp_id";
            this.txt_emp_id.Size = new System.Drawing.Size(90, 48);
            this.txt_emp_id.TabIndex = 10;
            // 
            // txt_fname
            // 
            this.txt_fname.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "fname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_fname.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_fname.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_fname.LabelText = "First Name";
            this.txt_fname.Location = new System.Drawing.Point(99, 5);
            this.txt_fname.Margin = new Wisej.Web.Padding(2);
            this.txt_fname.Name = "txt_fname";
            this.txt_fname.Size = new System.Drawing.Size(226, 48);
            this.txt_fname.TabIndex = 11;
            // 
            // txt_minit
            // 
            this.txt_minit.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "minit", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_minit.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_minit.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_minit.LabelText = "Middle Name";
            this.txt_minit.Location = new System.Drawing.Point(329, 5);
            this.txt_minit.Margin = new Wisej.Web.Padding(2);
            this.txt_minit.Name = "txt_minit";
            this.txt_minit.Size = new System.Drawing.Size(63, 48);
            this.txt_minit.TabIndex = 12;
            // 
            // txt_lname
            // 
            this.txt_lname.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "lname", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFlowBreak(this.txt_lname, true);
            this.txt_lname.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_lname.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_lname.LabelText = "Last Name";
            this.txt_lname.Location = new System.Drawing.Point(396, 5);
            this.txt_lname.Margin = new Wisej.Web.Padding(2);
            this.txt_lname.Name = "txt_lname";
            this.txt_lname.Size = new System.Drawing.Size(226, 48);
            this.txt_lname.TabIndex = 13;
            // 
            // cmb_job_id
            // 
            this.cmb_job_id.DataBindings.Add(new Wisej.Web.Binding("SelectedValue", this.bsEmployee, "job_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.cmb_job_id.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmb_job_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_job_id.LabelText = "Job ID";
            this.cmb_job_id.Location = new System.Drawing.Point(5, 109);
            this.cmb_job_id.Margin = new Wisej.Web.Padding(2);
            this.cmb_job_id.Name = "cmb_job_id";
            this.cmb_job_id.Size = new System.Drawing.Size(222, 48);
            this.cmb_job_id.TabIndex = 14;
            // 
            // txt_job_lvl
            // 
            this.txt_job_lvl.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "job_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFlowBreak(this.txt_job_lvl, true);
            this.txt_job_lvl.InputType.Max = "999";
            this.txt_job_lvl.InputType.Min = "1";
            this.txt_job_lvl.InputType.Mode = Wisej.Web.TextBoxMode.Numeric;
            this.txt_job_lvl.InputType.Step = 1D;
            this.txt_job_lvl.InputType.Type = Wisej.Web.TextBoxType.Number;
            this.txt_job_lvl.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_job_lvl.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_job_lvl.LabelText = "Job Level";
            this.txt_job_lvl.Location = new System.Drawing.Point(231, 109);
            this.txt_job_lvl.Margin = new Wisej.Web.Padding(2);
            this.txt_job_lvl.Name = "txt_job_lvl";
            this.txt_job_lvl.Size = new System.Drawing.Size(63, 48);
            this.txt_job_lvl.TabIndex = 15;
            // 
            // cmb_pub_id
            // 
            this.cmb_pub_id.DataBindings.Add(new Wisej.Web.Binding("SelectedValue", this.bsEmployee, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.cmb_pub_id.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmb_pub_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_pub_id.LabelText = "Assigned Publisher";
            this.cmb_pub_id.Location = new System.Drawing.Point(5, 161);
            this.cmb_pub_id.Margin = new Wisej.Web.Padding(2);
            this.cmb_pub_id.Name = "cmb_pub_id";
            this.cmb_pub_id.Size = new System.Drawing.Size(222, 48);
            this.cmb_pub_id.TabIndex = 16;
            // 
            // dtp_hire_date
            // 
            this.dtp_hire_date.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsEmployee, "hire_date", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFlowBreak(this.dtp_hire_date, true);
            this.dtp_hire_date.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtp_hire_date.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtp_hire_date.LabelText = "Hire date";
            this.dtp_hire_date.Location = new System.Drawing.Point(421, 57);
            this.dtp_hire_date.Margin = new Wisej.Web.Padding(2);
            this.dtp_hire_date.MaxDate = new System.DateTime(2090, 12, 31, 0, 0, 0, 0);
            this.dtp_hire_date.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtp_hire_date.Name = "dtp_hire_date";
            this.dtp_hire_date.Size = new System.Drawing.Size(118, 48);
            this.dtp_hire_date.TabIndex = 17;
            this.dtp_hire_date.Value = new System.DateTime(((long)(0)));
            // 
            // txt_phone
            // 
            this.txt_phone.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "phone", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_phone.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_phone.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_phone.LabelText = "Phone";
            this.txt_phone.Location = new System.Drawing.Point(5, 57);
            this.txt_phone.Margin = new Wisej.Web.Padding(2);
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Size = new System.Drawing.Size(154, 48);
            this.txt_phone.TabIndex = 18;
            // 
            // txt_email
            // 
            this.txt_email.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsEmployee, "email", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_email.InputType.Mode = Wisej.Web.TextBoxMode.Email;
            this.txt_email.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_email.Label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.txt_email.LabelText = "Email";
            this.txt_email.Location = new System.Drawing.Point(163, 57);
            this.txt_email.Margin = new Wisej.Web.Padding(2);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(254, 48);
            this.txt_email.TabIndex = 19;
            // 
            // bsEmployee
            // 
            this.bsEmployee.DataSource = typeof(PasseroDemo.Models.Employee);
            // 
            // frmEmployee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 481);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmEmployee";
            this.Text = "Employees";
            this.Load += new System.EventHandler(this.frmEmployee_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsEmployee)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
        private Wisej.Web.TextBox txt_emp_id;
        private Wisej.Web.TextBox txt_fname;
        private Wisej.Web.TextBox txt_minit;
        private Wisej.Web.TextBox txt_lname;
        private Wisej.Web.ComboBox cmb_job_id;
        private Wisej.Web.TextBox txt_job_lvl;
        private Wisej.Web.TextBox txt_email;
        private Wisej.Web.TextBox txt_phone;
        private Wisej.Web.DateTimePicker dtp_hire_date;
        private Wisej.Web.ComboBox cmb_pub_id;
        private Wisej.Web.BindingSource bsEmployee;
    }
}