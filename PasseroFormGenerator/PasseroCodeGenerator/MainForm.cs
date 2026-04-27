using PasseroFormGeneratorStandalone;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PasseroCodeGeneratorStandalone
{
    public partial class MainForm : Form
    {
        private List<ModelBaseEntity> currentEntities = new List<ModelBaseEntity>();
        private string currentTemplateDirectory;

        public MainForm()
        {
            InitializeComponent();
            InitializeTemplateDirectory();
        }

        private void InitializeTemplateDirectory()
        {
            // Cerca la cartella templates nella directory dell'applicazione
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            currentTemplateDirectory = Path.Combine(appDir, "templates");

            if (!Directory.Exists(currentTemplateDirectory))
            {
                // Prova nella directory di sviluppo
                string devTemplateDir = Path.Combine(appDir, "..", "..", "templates");
                if (Directory.Exists(devTemplateDir))
                {
                    currentTemplateDirectory = Path.GetFullPath(devTemplateDir);
                }
            }

            lblTemplateDirectory.Text = $"Template Directory: {currentTemplateDirectory}";
            
            if (!Directory.Exists(currentTemplateDirectory))
            {
                lblTemplateDirectory.ForeColor = System.Drawing.Color.Red;
                lblTemplateDirectory.Text += " (NON TROVATA)";
            }
        }

        private void btnSelectAssembly_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Assembly Files (*.dll;*.exe)|*.dll;*.exe|All Files (*.*)|*.*";
                openFileDialog.Title = "Seleziona Assembly";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "Analizzando assembly...";
                        Application.DoEvents();

                        currentEntities = ModelBaseAnalyzer.AnalyzeAssembly(openFileDialog.FileName);
                        UpdateEntityList();
                        
                        lblStatus.Text = $"Assembly analizzato: {currentEntities.Count} classi ModelBase trovate";
                        txtSourcePath.Text = openFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Errore nell'analisi dell'assembly:\n{ex.Message}", 
                            "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Errore nell'analisi dell'assembly";
                    }
                }
            }
        }

        private void btnSelectCsFiles_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "C# Files (*.cs)|*.cs|All Files (*.*)|*.*";
                openFileDialog.Title = "Seleziona File C#";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "Analizzando file C#...";
                        Application.DoEvents();

                        currentEntities = ModelBaseAnalyzer.AnalyzeCSharpFiles(openFileDialog.FileNames);
                        UpdateEntityList();
                        
                        lblStatus.Text = $"File analizzati: {currentEntities.Count} classi ModelBase trovate";
                        txtSourcePath.Text = string.Join("; ", openFileDialog.FileNames);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Errore nell'analisi dei file C#:\n{ex.Message}", 
                            "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Errore nell'analisi dei file C#";
                    }
                }
            }
        }

        private void UpdateEntityList()
        {
            lstEntities.Items.Clear();
            foreach (var entity in currentEntities)
            {
                lstEntities.Items.Add(entity, false);
            }

            if (currentEntities.Count == 0)
            {
                lblStatus.Text = "Nessuna classe ModelBase trovata";
            }
        }

        private void lstEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEntities.SelectedItem is ModelBaseEntity entity)
            {
                UpdateEntityDetails(entity);
            }
        }

        private void UpdateEntityDetails(ModelBaseEntity entity)
        {
            try
            {
                txtEntityDetails.Clear();
                txtEntityDetails.AppendText($"Classe: {entity.ClassName}\r\n");
                txtEntityDetails.AppendText($"Namespace: {entity.Namespace}\r\n");
                txtEntityDetails.AppendText($"File/Assembly: {entity.FilePath}\r\n");
                
                // Gestione sicura delle chiavi primarie
                if (entity.PrimaryKey != null && entity.PrimaryKey.Count > 0)
                {
                    txtEntityDetails.AppendText($"Chiavi Primarie: {string.Join(", ", entity.PrimaryKey)}\r\n\r\n");
                }
                else
                {
                    txtEntityDetails.AppendText("Chiavi Primarie: Nessuna trovata\r\n\r\n");
                }
                
                txtEntityDetails.AppendText("Proprietà:\r\n");
                
                if (entity.Properties != null)
                {
                    foreach (var prop in entity.Properties)
                    {
                        try
                        {
                            string keyInfo = "";
                            if (prop.IsKey) keyInfo += " [Key]";
                            if (prop.IsExplicitKey) keyInfo += " [ExplicitKey]";
            
                            txtEntityDetails.AppendText($"  - {prop.Name} ({prop.Type}){keyInfo}\r\n");
                        }
                        catch (Exception ex)
                        {
                            txtEntityDetails.AppendText($"  - {prop.Name} (Errore: {ex.Message})\r\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtEntityDetails.Text = $"Errore nella visualizzazione dei dettagli: {ex.Message}";
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstEntities.Items.Count; i++)
            {
                lstEntities.SetItemChecked(i, true);
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstEntities.Items.Count; i++)
            {
                lstEntities.SetItemChecked(i, false);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var selectedEntities = new List<ModelBaseEntity>();
            
            foreach (int index in lstEntities.CheckedIndices)
            {
                selectedEntities.Add(currentEntities[index]);
            }

            if (selectedEntities.Count == 0)
            {
                MessageBox.Show("Seleziona almeno una entità per continuare.", 
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOutputDirectory.Text))
            {
                MessageBox.Show("Seleziona una directory di output.", 
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(currentTemplateDirectory))
            {
                MessageBox.Show($"Directory template non trovata: {currentTemplateDirectory}", 
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GenerateFiles(selectedEntities);
        }

        private void GenerateFiles(List<ModelBaseEntity> entities)
        {
            try
            {
                var templateManager = new TemplateManager(currentTemplateDirectory);
                int generatedCount = 0;
                int errorCount = 0;

                foreach (var entity in entities)
                {
                    try
                    {
                        // Estrai namespace base (senza .Models se presente)
                        string BaseNamespace = entity.Namespace;
                        if (BaseNamespace.EndsWith(".Models"))
                        {
                            BaseNamespace = BaseNamespace.Substring(0, BaseNamespace.Length - ".Models".Length);
                        }

                        var model = new
                        {
                            EntityName = entity.ClassName,
                            Title = entity.ClassName + " Management",
                            PrimaryKey = entity.PrimaryKey,
                            Namespace = entity.Namespace,
                            BaseNamespace = BaseNamespace,
                            FullEntityName = entity.Namespace + "." + entity.ClassName,
                            Properties = entity.Properties,
                            GenerateRepository = chkGenerateRepository.Checked,
                            GenerateViewModel = chkGenerateViewModel.Checked,
                            GenerateDesigner = chkGenerateDesigner.Checked,
                            IncludeValidation = chkIncludeValidation.Checked
                        };

                        string targetDir = txtOutputDirectory.Text;

                        // Genera il form principale solo se una checkbox specifica è selezionata
                        // (assumendo che ci sia una checkbox per il form principale, altrimenti usare una delle esistenti)
                        if (chkGenerateDesigner.Checked && templateManager.TemplateExists("frmEntity.cs.scriban"))
                            templateManager.GenerateFileFromTemplate("frmEntity.cs.scriban", targetDir, $"frm{entity.ClassName}.cs", model);
                        
                        // Genera i file opzionali in base alle selezioni
                        if (chkGenerateDesigner.Checked && templateManager.TemplateExists("frmEntity.Designer.cs.scriban"))
                            templateManager.GenerateFileFromTemplate("frmEntity.Designer.cs.scriban", targetDir, $"frm{entity.ClassName}.Designer.cs", model);
                        
                        if (chkGenerateViewModel.Checked && templateManager.TemplateExists("vmEntity.cs.scriban"))
                            templateManager.GenerateFileFromTemplate("vmEntity.cs.scriban", targetDir, $"vm{entity.ClassName}.cs", model);
                        
                        if (chkGenerateRepository.Checked && templateManager.TemplateExists("rpEntity.cs.scriban"))
                            templateManager.GenerateFileFromTemplate("rpEntity.cs.scriban", targetDir, $"rp{entity.ClassName}.cs", model);

                        generatedCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"Errore generando i file per l'entità {entity.ClassName}: {ex.Message}");
                    }
                }

                lblStatus.Text = $"Generazione completata: {generatedCount} entità generate, {errorCount} errori";
                
                string message = $"Generazione completata!\n\nEntità generate: {generatedCount}";
                if (errorCount > 0)
                    message += $"\nErrori: {errorCount}";

                MessageBox.Show(message, "Generazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la generazione:\n{ex.Message}", 
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Errore durante la generazione";
            }
        }

        private void btnSelectOutputDirectory_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Seleziona directory di output";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputDirectory.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnSelectTemplateDirectory_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Seleziona directory template";
                folderDialog.SelectedPath = currentTemplateDirectory;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    currentTemplateDirectory = folderDialog.SelectedPath;
                    lblTemplateDirectory.Text = $"Template Directory: {currentTemplateDirectory}";
                    
                    if (!Directory.Exists(currentTemplateDirectory))
                    {
                        lblTemplateDirectory.ForeColor = System.Drawing.Color.Red;
                        lblTemplateDirectory.Text += " (NON TROVATA)";
                    }
                    else
                    {
                        lblTemplateDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
                    }
                }
            }
        }
    }
}