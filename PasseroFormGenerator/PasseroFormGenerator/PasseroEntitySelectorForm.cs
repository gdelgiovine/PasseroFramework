using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EnvDTE80;

namespace PasseroFormGenerator
{
    public partial class PasseroEntitySelectorForm : Form
    {
        public List<ModelBaseEntity> SelectedEntities { get; private set; } = new List<ModelBaseEntity>();
        public bool GenerateRepository { get; set; } = true;
        public bool GenerateViewModel { get; set; } = true;
        public bool GenerateDesigner { get; set; } = true;
        public bool IncludeValidation { get; set; } = false;

        private List<ModelBaseEntity> allEntities = new List<ModelBaseEntity>();

        public PasseroEntitySelectorForm()
        {
            InitializeComponent();
        }

        public void LoadEntities(DTE2 dte)
        {
            try
            {
                lblStatus.Text = "Analizzando progetto...";
                Application.DoEvents();

                allEntities = ModelBaseAnalyzer.FindModelBaseClasses(dte);
                
                lstEntities.Items.Clear();
                foreach (var entity in allEntities)
                {
                    lstEntities.Items.Add(entity, false);
                }

                lblStatus.Text = $"Trovate {allEntities.Count} classi che derivano da ModelBase";
                
                if (allEntities.Count == 0)
                {
                    lblStatus.Text = "Nessuna classe trovata che deriva da PasseroFramework.ModelBase";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Errore durante l'analisi: " + ex.Message;
                MessageBox.Show($"Errore durante l'analisi del progetto: {ex.Message}", 
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SelectedEntities.Clear();
            
            foreach (int index in lstEntities.CheckedIndices)
            {
                SelectedEntities.Add(allEntities[index]);
            }

            if (SelectedEntities.Count == 0)
            {
                MessageBox.Show("Seleziona almeno una entità per continuare.", 
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerateRepository = chkGenerateRepository.Checked;
            GenerateViewModel = chkGenerateViewModel.Checked;
            GenerateDesigner = chkGenerateDesigner.Checked;
            IncludeValidation = chkIncludeValidation.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lstEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEntities.SelectedItem is ModelBaseEntity entity)
            {
                // Mostra i dettagli dell'entità selezionata
                UpdateEntityDetails(entity);
            }
        }

        private void UpdateEntityDetails(ModelBaseEntity entity)
        {
            txtEntityDetails.Clear();
            txtEntityDetails.AppendText($"Classe: {entity.ClassName}\r\n");
            txtEntityDetails.AppendText($"Namespace: {entity.Namespace}\r\n");
            txtEntityDetails.AppendText($"File: {entity.FilePath}\r\n");
            txtEntityDetails.AppendText($"Chiave Primaria: {entity.PrimaryKey}\r\n\r\n");
            txtEntityDetails.AppendText("Proprietà:\r\n");
            
            foreach (var prop in entity.Properties)
            {
                txtEntityDetails.AppendText($"  - {prop.Name} ({prop.Type})\r\n");
            }
        }
    }
}