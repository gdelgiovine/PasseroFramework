namespace PasseroFormGenerator
{
    partial class PasseroEntitySelectorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.CheckedListBox lstEntities;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkGenerateRepository;
        private System.Windows.Forms.CheckBox chkGenerateViewModel;
        private System.Windows.Forms.CheckBox chkGenerateDesigner;
        private System.Windows.Forms.CheckBox chkIncludeValidation;
        private System.Windows.Forms.Label lblEntities;
        private System.Windows.Forms.Label lblOptions;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtEntityDetails;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.SplitContainer splitContainer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstEntities = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lblEntities = new System.Windows.Forms.Label();
            this.txtEntityDetails = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.chkGenerateRepository = new System.Windows.Forms.CheckBox();
            this.chkGenerateViewModel = new System.Windows.Forms.CheckBox();
            this.chkGenerateDesigner = new System.Windows.Forms.CheckBox();
            this.chkIncludeValidation = new System.Windows.Forms.CheckBox();
            this.lblOptions = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectNone);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectAll);
            this.splitContainer1.Panel1.Controls.Add(this.lstEntities);
            this.splitContainer1.Panel1.Controls.Add(this.lblEntities);
            this.splitContainer1.Panel2.Controls.Add(this.txtEntityDetails);
            this.splitContainer1.Panel2.Controls.Add(this.lblDetails);
            this.splitContainer1.Size = new System.Drawing.Size(760, 350);
            this.splitContainer1.SplitterDistance = 380;
            this.splitContainer1.TabIndex = 0;
            
            // 
            // lblEntities
            // 
            this.lblEntities.AutoSize = true;
            this.lblEntities.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntities.Location = new System.Drawing.Point(3, 5);
            this.lblEntities.Name = "lblEntities";
            this.lblEntities.Size = new System.Drawing.Size(181, 15);
            this.lblEntities.TabIndex = 0;
            this.lblEntities.Text = "Classi che derivano da ModelBase:";
            
            // 
            // lstEntities
            // 
            this.lstEntities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEntities.CheckOnClick = true;
            this.lstEntities.FormattingEnabled = true;
            this.lstEntities.Location = new System.Drawing.Point(3, 25);
            this.lstEntities.Name = "lstEntities";
            this.lstEntities.Size = new System.Drawing.Size(374, 284);
            this.lstEntities.TabIndex = 1;
            this.lstEntities.SelectedIndexChanged += new System.EventHandler(this.lstEntities_SelectedIndexChanged);
            
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(3, 315);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Seleziona Tutto";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectNone.Location = new System.Drawing.Point(109, 315);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(100, 23);
            this.btnSelectNone.TabIndex = 3;
            this.btnSelectNone.Text = "Deseleziona Tutto";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(3, 5);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(102, 15);
            this.lblDetails.TabIndex = 0;
            this.lblDetails.Text = "Dettagli Entità:";
            
            // 
            // txtEntityDetails
            // 
            this.txtEntityDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEntityDetails.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntityDetails.Location = new System.Drawing.Point(3, 25);
            this.txtEntityDetails.Multiline = true;
            this.txtEntityDetails.Name = "txtEntityDetails";
            this.txtEntityDetails.ReadOnly = true;
            this.txtEntityDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEntityDetails.Size = new System.Drawing.Size(370, 313);
            this.txtEntityDetails.TabIndex = 1;
            
            // 
            // lblOptions
            // 
            this.lblOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOptions.AutoSize = true;
            this.lblOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptions.Location = new System.Drawing.Point(12, 375);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(140, 15);
            this.lblOptions.TabIndex = 1;
            this.lblOptions.Text = "Opzioni di Generazione:";
            
            // 
            // chkGenerateRepository
            // 
            this.chkGenerateRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGenerateRepository.AutoSize = true;
            this.chkGenerateRepository.Checked = true;
            this.chkGenerateRepository.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateRepository.Location = new System.Drawing.Point(15, 395);
            this.chkGenerateRepository.Name = "chkGenerateRepository";
            this.chkGenerateRepository.Size = new System.Drawing.Size(121, 17);
            this.chkGenerateRepository.TabIndex = 2;
            this.chkGenerateRepository.Text = "Genera Repository";
            this.chkGenerateRepository.UseVisualStyleBackColor = true;
            
            // 
            // chkGenerateViewModel
            // 
            this.chkGenerateViewModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGenerateViewModel.AutoSize = true;
            this.chkGenerateViewModel.Checked = true;
            this.chkGenerateViewModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateViewModel.Location = new System.Drawing.Point(150, 395);
            this.chkGenerateViewModel.Name = "chkGenerateViewModel";
            this.chkGenerateViewModel.Size = new System.Drawing.Size(124, 17);
            this.chkGenerateViewModel.TabIndex = 3;
            this.chkGenerateViewModel.Text = "Genera ViewModel";
            this.chkGenerateViewModel.UseVisualStyleBackColor = true;
            
            // 
            // chkGenerateDesigner
            // 
            this.chkGenerateDesigner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGenerateDesigner.AutoSize = true;
            this.chkGenerateDesigner.Checked = true;
            this.chkGenerateDesigner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateDesigner.Location = new System.Drawing.Point(290, 395);
            this.chkGenerateDesigner.Name = "chkGenerateDesigner";
            this.chkGenerateDesigner.Size = new System.Drawing.Size(139, 17);
            this.chkGenerateDesigner.TabIndex = 4;
            this.chkGenerateDesigner.Text = "Genera Form Designer";
            this.chkGenerateDesigner.UseVisualStyleBackColor = true;
            
            // 
            // chkIncludeValidation
            // 
            this.chkIncludeValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIncludeValidation.AutoSize = true;
            this.chkIncludeValidation.Location = new System.Drawing.Point(445, 395);
            this.chkIncludeValidation.Name = "chkIncludeValidation";
            this.chkIncludeValidation.Size = new System.Drawing.Size(119, 17);
            this.chkIncludeValidation.TabIndex = 5;
            this.chkIncludeValidation.Text = "Include Validation";
            this.chkIncludeValidation.UseVisualStyleBackColor = true;
            
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(615, 425);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 30);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Genera";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(696, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(12, 430);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(500, 20);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Pronto";
            
            // 
            // PasseroEntitySelectorForm
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 467);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.chkIncludeValidation);
            this.Controls.Add(this.chkGenerateDesigner);
            this.Controls.Add(this.chkGenerateViewModel);
            this.Controls.Add(this.chkGenerateRepository);
            this.Controls.Add(this.lblOptions);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "PasseroEntitySelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Passero Form Generator - Selezione Entità";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}