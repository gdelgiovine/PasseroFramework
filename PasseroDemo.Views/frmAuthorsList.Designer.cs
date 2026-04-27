namespace PasseroDemo.Views
{
    partial class frmAuthorsList
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
            this.dataGridView1 = new Wisej.Web.DataGridView();
            this.dgvc_AU_ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_au_lname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvc_au_fname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = Wisej.Web.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dgvc_AU_ID,
            this.dgvc_au_lname,
            this.dgvc_au_fname});
            this.dataGridView1.Location = new System.Drawing.Point(3, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(732, 316);
            this.dataGridView1.TabIndex = 0;
            // 
            // dgvc_AU_ID
            // 
            this.dgvc_AU_ID.DataPropertyName = "au_id";
            this.dgvc_AU_ID.HeaderText = "Author ID";
            this.dgvc_AU_ID.Name = "dgvc_AU_ID";
            // 
            // dgvc_au_lname
            // 
            this.dgvc_au_lname.DataPropertyName = "au_lname";
            this.dgvc_au_lname.HeaderText = "Last Name";
            this.dgvc_au_lname.Name = "dgvc_au_lname";
            // 
            // dgvc_au_fname
            // 
            this.dgvc_au_fname.DataPropertyName = "au_fname";
            this.dgvc_au_fname.HeaderText = "First Name";
            this.dgvc_au_fname.Name = "dgvc_au_fname";
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.DelegateCurrencyManager = false;
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 342);
            this.dataNavigator1.ManageNavigation = false;
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(738, 70);
            this.dataNavigator1.TabIndex = 1;
            this.dataNavigator1.eAddNew += new Passero.Framework.Controls.DataNavigator.eAddNewEventHandler(this.dataNavigator1_eAddNew);
            this.dataNavigator1.eRefresh += new Passero.Framework.Controls.DataNavigator.eRefreshEventHandler(this.dataNavigator1_eRefresh);
            this.dataNavigator1.eSave += new Passero.Framework.Controls.DataNavigator.eSaveEventHandler(this.dataNavigator1_eSave);
            this.dataNavigator1.eMovePrevious += new Passero.Framework.Controls.DataNavigator.eMovePreviousEventHandler(this.dataNavigator1_eMovePrevious);
            this.dataNavigator1.eMoveFirst += new Passero.Framework.Controls.DataNavigator.eMoveFirstEventHandler(this.dataNavigator1_eMoveFirst);
            this.dataNavigator1.eMoveLast += new Passero.Framework.Controls.DataNavigator.eMoveLastEventHandler(this.dataNavigator1_eMoveLast);
            this.dataNavigator1.eMoveNext += new Passero.Framework.Controls.DataNavigator.eMoveNextEventHandler(this.dataNavigator1_eMoveNext);
            // 
            // frmAuthorsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 412);
            this.Controls.Add(this.dataNavigator1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmAuthorsList";
            this.Text = "Authors List";
            this.Load += new System.EventHandler(this.frmAuthorsList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.DataGridView dataGridView1;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_AU_ID;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_au_lname;
        private Wisej.Web.DataGridViewTextBoxColumn dgvc_au_fname;
        private Passero .Framework .Controls.DataNavigator dataNavigator1;
    }
}