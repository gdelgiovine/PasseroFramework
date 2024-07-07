namespace PasseroDemo.Views
{
    partial class frmJobs
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
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.txt_job_id = new Wisej.Web.TextBox();
            this.bsJobs = new Wisej.Web.BindingSource(this.components);
            this.txt_job_desc = new Wisej.Web.TextBox();
            this.txt_min_lvl = new Wisej.Web.TextBox();
            this.txt_max_lvl = new Wisej.Web.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "Jobs";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 431);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(761, 64);
            this.dataNavigator1.TabIndex = 1;
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.txt_job_id);
            this.flowLayoutPanel1.Controls.Add(this.txt_job_desc);
            this.flowLayoutPanel1.Controls.Add(this.txt_min_lvl);
            this.flowLayoutPanel1.Controls.Add(this.txt_max_lvl);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(761, 431);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // txt_job_id
            // 
            this.txt_job_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsJobs, "job_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_job_id.InputType.Mode = Wisej.Web.TextBoxMode.Numeric;
            this.txt_job_id.InputType.Type = Wisej.Web.TextBoxType.Number;
            this.txt_job_id.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_job_id.LabelText = "Job ID";
            this.txt_job_id.Location = new System.Drawing.Point(3, 3);
            this.txt_job_id.Name = "txt_job_id";
            this.txt_job_id.Size = new System.Drawing.Size(100, 48);
            this.txt_job_id.TabIndex = 0;
            // 
            // bsJobs
            // 
            this.bsJobs.DataSource = typeof(PasseroDemo.Models.Job);
            // 
            // txt_job_desc
            // 
            this.txt_job_desc.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsJobs, "job_desc", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFlowBreak(this.txt_job_desc, true);
            this.txt_job_desc.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_job_desc.LabelText = "Job Description";
            this.txt_job_desc.Location = new System.Drawing.Point(109, 3);
            this.txt_job_desc.Name = "txt_job_desc";
            this.txt_job_desc.Size = new System.Drawing.Size(337, 48);
            this.txt_job_desc.TabIndex = 1;
            // 
            // txt_min_lvl
            // 
            this.txt_min_lvl.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsJobs, "min_lvl", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_min_lvl.InputType.Mode = Wisej.Web.TextBoxMode.Numeric;
            this.txt_min_lvl.InputType.Type = Wisej.Web.TextBoxType.Number;
            this.txt_min_lvl.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_min_lvl.LabelText = "Min. Level";
            this.txt_min_lvl.Location = new System.Drawing.Point(3, 57);
            this.txt_min_lvl.Name = "txt_min_lvl";
            this.txt_min_lvl.Size = new System.Drawing.Size(100, 48);
            this.txt_min_lvl.TabIndex = 2;
            // 
            // txt_max_lvl
            // 
            this.txt_max_lvl.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsJobs, "max_lvl", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_max_lvl.InputType.Mode = Wisej.Web.TextBoxMode.Numeric;
            this.txt_max_lvl.InputType.Type = Wisej.Web.TextBoxType.Number;
            this.txt_max_lvl.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_max_lvl.LabelText = "Max Level";
            this.txt_max_lvl.Location = new System.Drawing.Point(109, 57);
            this.txt_max_lvl.Name = "txt_max_lvl";
            this.txt_max_lvl.Size = new System.Drawing.Size(100, 48);
            this.txt_max_lvl.TabIndex = 3;
            // 
            // frmJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 495);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmJobs";
            this.Text = "Jobs";
            this.Load += new System.EventHandler(this.frmJobs_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsJobs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Passero.Framework.Controls.DataNavigator dataNavigator1;
        private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
        private Wisej.Web.TextBox txt_job_id;
        private Wisej.Web.TextBox txt_job_desc;
        private Wisej.Web.TextBox txt_max_lvl;
        private Wisej.Web.TextBox txt_min_lvl;
        private Wisej.Web.BindingSource bsJobs;
    }
}