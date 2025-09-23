namespace PasseroCodeGeneratorStandalone
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // Controlli per selezione sorgente
        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Button btnSelectAssembly;
        private System.Windows.Forms.Button btnSelectCsFiles;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblSourcePath;
        
        // Controlli per entità
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox lstEntities;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Label lblEntities;
        private System.Windows.Forms.TextBox txtEntityDetails;
        private System.Windows.Forms.Label lblDetails;
        
        // Controlli per opzioni
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox chkGenerateRepository;
        private System.Windows.Forms.CheckBox chkGenerateViewModel;
        private System.Windows.Forms.CheckBox chkGenerateDesigner;
        private System.Windows.Forms.CheckBox chkIncludeValidation;
        
        // Controlli per output
        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.Button btnSelectOutputDirectory;
        private System.Windows.Forms.Label lblOutputDirectory;
        
        // Controlli per template
        private System.Windows.Forms.GroupBox grpTemplate;
        private System.Windows.Forms.Label lblTemplateDirectory;
        private System.Windows.Forms.Button btnSelectTemplateDirectory;
        
        // Controlli principali
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

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
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.btnSelectAssembly = new System.Windows.Forms.Button();
            this.btnSelectCsFiles = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.lblSourcePath = new System.Windows.Forms.Label();
            
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstEntities = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lblEntities = new System.Windows.Forms.Label();
            this.txtEntityDetails = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkGenerateRepository = new System.Windows.Forms.CheckBox();
            this.chkGenerateViewModel = new System.Windows.Forms.CheckBox();
            this.chkGenerateDesigner = new System.Windows.Forms.CheckBox();
            this.chkIncludeValidation = new System.Windows.Forms.CheckBox();
            
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.btnSelectOutputDirectory = new System.Windows.Forms.Button();
            this.lblOutputDirectory = new System.Windows.Forms.Label();
            
            this.grpTemplate = new System.Windows.Forms.GroupBox();
            this.lblTemplateDirectory = new System.Windows.Forms.Label();
            this.btnSelectTemplateDirectory = new System.Windows.Forms.Button();
            
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpOutput.SuspendLayout();
            this.grpTemplate.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.txtSourcePath);
            this.grpSource.Controls.Add(this.lblSourcePath);
            this.grpSource.Controls.Add(this.btnSelectCsFiles);
            this.grpSource.Controls.Add(this.btnSelectAssembly);
            this.grpSource.Location = new System.Drawing.Point(12, 12);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(760, 80);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Selezione Sorgente";
            
            // 
            // btnSelectAssembly
            // 
            this.btnSelectAssembly.Location = new System.Drawing.Point(15, 25);
            this.btnSelectAssembly.Name = "btnSelectAssembly";
            this.btnSelectAssembly.Size = new System.Drawing.Size(120, 23);
            this.btnSelectAssembly.TabIndex = 0;
            this.btnSelectAssembly.Text = "Seleziona Assembly";
            this.btnSelectAssembly.UseVisualStyleBackColor = true;
            this.btnSelectAssembly.Click += new System.EventHandler(this.btnSelectAssembly_Click);
            
            // 
            // btnSelectCsFiles
            // 
            this.btnSelectCsFiles.Location = new System.Drawing.Point(150, 25);
            this.btnSelectCsFiles.Name = "btnSelectCsFiles";
            this.btnSelectCsFiles.Size = new System.Drawing.Size(120, 23);
            this.btnSelectCsFiles.TabIndex = 1;
            this.btnSelectCsFiles.Text = "Seleziona File C#";
            this.btnSelectCsFiles.UseVisualStyleBackColor = true;
            this.btnSelectCsFiles.Click += new System.EventHandler(this.btnSelectCsFiles_Click);
            
            // 
            // lblSourcePath
            // 
            this.lblSourcePath.AutoSize = true;
            this.lblSourcePath.Location = new System.Drawing.Point(15, 55);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new System.Drawing.Size(71, 13);
            this.lblSourcePath.TabIndex = 2;
            this.lblSourcePath.Text = "Percorso:";
            
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(90, 52);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(650, 20);
            this.txtSourcePath.TabIndex = 3;
            
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 110);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectNone);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectAll);
            this.splitContainer1.Panel1.Controls.Add(this.lstEntities);
            this.splitContainer1.Panel1.Controls.Add(this.lblEntities);
            this.splitContainer1.Panel2.Controls.Add(this.txtEntityDetails);
            this.splitContainer1.Panel2.Controls.Add(this.lblDetails);
            this.splitContainer1.Size = new System.Drawing.Size(760, 280);
            this.splitContainer1.SplitterDistance = 380;
            this.splitContainer1.TabIndex = 1;
            
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
            this.lstEntities.CheckOnClick = true;
            this.lstEntities.FormattingEnabled = true;
            this.lstEntities.Location = new System.Drawing.Point(3, 25);
            this.lstEntities.Name = "lstEntities";
            this.lstEntities.Size = new System.Drawing.Size(374, 214);
            this.lstEntities.TabIndex = 1;
            this.lstEntities.SelectedIndexChanged += new System.EventHandler(this.lstEntities_SelectedIndexChanged);
            
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(3, 245);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Seleziona Tutto";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(109, 245);
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
            this.txtEntityDetails.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntityDetails.Location = new System.Drawing.Point(3, 25);
            this.txtEntityDetails.Multiline = true;
            this.txtEntityDetails.Name = "txtEntityDetails";
            this.txtEntityDetails.ReadOnly = true;
            this.txtEntityDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEntityDetails.Size = new System.Drawing.Size(370, 243);
            this.txtEntityDetails.TabIndex = 1;
            
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkIncludeValidation);
            this.grpOptions.Controls.Add(this.chkGenerateDesigner);
            this.grpOptions.Controls.Add(this.chkGenerateViewModel);
            this.grpOptions.Controls.Add(this.chkGenerateRepository);
            this.grpOptions.Location = new System.Drawing.Point(12, 410);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(370, 70);
            this.grpOptions.TabIndex = 2;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Opzioni di Generazione";
            
            // 
            // chkGenerateRepository
            // 
            this.chkGenerateRepository.AutoSize = true;
            this.chkGenerateRepository.Checked = true;
            this.chkGenerateRepository.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateRepository.Location = new System.Drawing.Point(15, 25);
            this.chkGenerateRepository.Name = "chkGenerateRepository";
            this.chkGenerateRepository.Size = new System.Drawing.Size(121, 17);
            this.chkGenerateRepository.TabIndex = 0;
            this.chkGenerateRepository.Text = "Genera Repository";
            this.chkGenerateRepository.UseVisualStyleBackColor = true;
            
            // 
            // chkGenerateViewModel
            // 
            this.chkGenerateViewModel.AutoSize = true;
            this.chkGenerateViewModel.Checked = true;
            this.chkGenerateViewModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateViewModel.Location = new System.Drawing.Point(150, 25);
            this.chkGenerateViewModel.Name = "chkGenerateViewModel";
            this.chkGenerateViewModel.Size = new System.Drawing.Size(124, 17);
            this.chkGenerateViewModel.TabIndex = 1;
            this.chkGenerateViewModel.Text = "Genera ViewModel";
            this.chkGenerateViewModel.UseVisualStyleBackColor = true;
            
            // 
            // chkGenerateDesigner
            // 
            this.chkGenerateDesigner.AutoSize = true;
            this.chkGenerateDesigner.Checked = true;
            this.chkGenerateDesigner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateDesigner.Location = new System.Drawing.Point(15, 48);
            this.chkGenerateDesigner.Name = "chkGenerateDesigner";
            this.chkGenerateDesigner.Size = new System.Drawing.Size(139, 17);
            this.chkGenerateDesigner.TabIndex = 2;
            this.chkGenerateDesigner.Text = "Genera Form Designer";
            this.chkGenerateDesigner.UseVisualStyleBackColor = true;
            
            // 
            // chkIncludeValidation
            // 
            this.chkIncludeValidation.AutoSize = true;
            this.chkIncludeValidation.Location = new System.Drawing.Point(180, 48);
            this.chkIncludeValidation.Name = "chkIncludeValidation";
            this.chkIncludeValidation.Size = new System.Drawing.Size(119, 17);
            this.chkIncludeValidation.TabIndex = 3;
            this.chkIncludeValidation.Text = "Include Validation";
            this.chkIncludeValidation.UseVisualStyleBackColor = true;
            
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.btnSelectOutputDirectory);
            this.grpOutput.Controls.Add(this.txtOutputDirectory);
            this.grpOutput.Controls.Add(this.lblOutputDirectory);
            this.grpOutput.Location = new System.Drawing.Point(402, 410);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(370, 70);
            this.grpOutput.TabIndex = 3;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Directory di Output";
            
            // 
            // lblOutputDirectory
            // 
            this.lblOutputDirectory.AutoSize = true;
            this.lblOutputDirectory.Location = new System.Drawing.Point(15, 25);
            this.lblOutputDirectory.Name = "lblOutputDirectory";
            this.lblOutputDirectory.Size = new System.Drawing.Size(55, 13);
            this.lblOutputDirectory.TabIndex = 0;
            this.lblOutputDirectory.Text = "Directory:";
            
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.Location = new System.Drawing.Point(15, 41);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.Size = new System.Drawing.Size(270, 20);
            this.txtOutputDirectory.TabIndex = 1;
            
            // 
            // btnSelectOutputDirectory
            // 
            this.btnSelectOutputDirectory.Location = new System.Drawing.Point(291, 39);
            this.btnSelectOutputDirectory.Name = "btnSelectOutputDirectory";
            this.btnSelectOutputDirectory.Size = new System.Drawing.Size(70, 23);
            this.btnSelectOutputDirectory.TabIndex = 2;
            this.btnSelectOutputDirectory.Text = "Sfoglia...";
            this.btnSelectOutputDirectory.UseVisualStyleBackColor = true;
            this.btnSelectOutputDirectory.Click += new System.EventHandler(this.btnSelectOutputDirectory_Click);
            
            // 
            // grpTemplate
            // 
            this.grpTemplate.Controls.Add(this.btnSelectTemplateDirectory);
            this.grpTemplate.Controls.Add(this.lblTemplateDirectory);
            this.grpTemplate.Location = new System.Drawing.Point(12, 490);
            this.grpTemplate.Name = "grpTemplate";
            this.grpTemplate.Size = new System.Drawing.Size(600, 50);
            this.grpTemplate.TabIndex = 4;
            this.grpTemplate.TabStop = false;
            this.grpTemplate.Text = "Template";
            
            // 
            // lblTemplateDirectory
            // 
            this.lblTemplateDirectory.Location = new System.Drawing.Point(15, 20);
            this.lblTemplateDirectory.Name = "lblTemplateDirectory";
            this.lblTemplateDirectory.Size = new System.Drawing.Size(500, 20);
            this.lblTemplateDirectory.TabIndex = 0;
            this.lblTemplateDirectory.Text = "Template Directory: ";
            
            // 
            // btnSelectTemplateDirectory
            // 
            this.btnSelectTemplateDirectory.Location = new System.Drawing.Point(520, 17);
            this.btnSelectTemplateDirectory.Name = "btnSelectTemplateDirectory";
            this.btnSelectTemplateDirectory.Size = new System.Drawing.Size(70, 23);
            this.btnSelectTemplateDirectory.TabIndex = 1;
            this.btnSelectTemplateDirectory.Text = "Cambia...";
            this.btnSelectTemplateDirectory.UseVisualStyleBackColor = true;
            this.btnSelectTemplateDirectory.Click += new System.EventHandler(this.btnSelectTemplateDirectory_Click);
            
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(622, 500);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 30);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Genera";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(703, 500);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 30);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Esci";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 548);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Pronto";
            
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 570);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.grpTemplate);
            this.Controls.Add(this.grpOutput);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.grpSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Passero Form Generator - Standalone";
            
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.grpTemplate.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}