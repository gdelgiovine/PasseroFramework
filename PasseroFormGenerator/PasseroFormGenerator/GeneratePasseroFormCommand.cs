using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop; 
using EnvDTE;
using EnvDTE80;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;
using Scriban;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PasseroFormGenerator
{
    internal sealed class GeneratePasseroFormCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("7f78f87d-9a6b-46b1-9b71-1e3e4e9e2b7d");

        private readonly AsyncPackage package;
        public static GeneratePasseroFormCommand Instance { get; private set; }

        private GeneratePasseroFormCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            if (commandService == null)
                throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));

            // Deve essere sul thread UI per aggiungere i comandi
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService == null)
                throw new InvalidOperationException("OleMenuCommandService non disponibile.");

            Instance = new GeneratePasseroFormCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE2;
                if (dte == null)
                {
                    ShowInfo("Impossibile ottenere l'istanza DTE.");
                    return;
                }

                string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string templateDir = Path.Combine(assemblyDir ?? ".", "templates");

                if (!Directory.Exists(templateDir))
                {
                    ShowWarning($"Cartella template non trovata:\r\n{templateDir}");
                    return;
                }

                // Apri il form di selezione entità
                using (var selectorForm = new PasseroEntitySelectorForm())
                {
                    selectorForm.LoadEntities(dte);

                    if (selectorForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

                    if (selectorForm.SelectedEntities.Count == 0)
                    {
                        ShowInfo("Nessuna entità selezionata.");
                        return;
                    }

                    int generatedCount = 0;
                    foreach (var entity in selectorForm.SelectedEntities)
                    {
                        try
                        {
                            var model = new
                            {
                                Entity = entity.ClassName,
                                Title = entity.ClassName + " Management", 
                                PrimaryKey = entity.PrimaryKey,
                                Namespace = entity.Namespace,  // ✅ Questo è fondamentale!
                                Properties = entity.Properties,
                                GenerateRepository = selectorForm.GenerateRepository,
                                GenerateViewModel = selectorForm.GenerateViewModel,
                                GenerateDesigner = selectorForm.GenerateDesigner,
                                IncludeValidation = selectorForm.IncludeValidation
                            };

                            string targetDir = Path.GetDirectoryName(entity.FilePath);
                            if (string.IsNullOrEmpty(targetDir))
                                continue;

                            // Genera sempre il form principale
                            GenerateFileFromTemplate(templateDir, "frmEntity.cs.scriban", targetDir, $"frm{entity.ClassName}.cs", model);
                            
                            // Genera i file opzionali in base alle selezioni
                            if (selectorForm.GenerateDesigner)
                                GenerateFileFromTemplate(templateDir, "frmEntity.Designer.cs.scriban", targetDir, $"frm{entity.ClassName}.Designer.cs", model);
                            
                            if (selectorForm.GenerateViewModel)
                                GenerateFileFromTemplate(templateDir, "vmEntity.cs.scriban", targetDir, $"vm{entity.ClassName}.cs", model);
                            
                            if (selectorForm.GenerateRepository)
                                GenerateFileFromTemplate(templateDir, "EntityRepository.cs.scriban", targetDir, $"{entity.ClassName}Repository.cs", model);

                            generatedCount++;
                        }
                        catch (Exception ex)
                        {
                            ShowWarning($"Errore generando i file per l'entità {entity.ClassName}: {ex.Message}");
                        }
                    }

                    ShowInfo($"Generazione completata per {generatedCount} entità.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Errore durante la generazione:\r\n" + ex.Message);
            }
        }

        private bool TryGetSelectedProjectItem(DTE2 dte, out ProjectItem projectItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            projectItem = null;

            var selected = dte?.ToolWindows?.SolutionExplorer?.SelectedItems;
            if (selected == null)
                return false;

            try
            {
                var uiItem = ((Array)selected).Cast<UIHierarchyItem>().FirstOrDefault();
                if (uiItem?.Object is ProjectItem pi)
                {
                    projectItem = pi;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        private void GenerateFileFromTemplate(string templateDir, string templateFile, string outputDir, string outputFile, object model)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string templatePath = Path.Combine(templateDir, templateFile);
            if (!File.Exists(templatePath))
                return;

            try
            {
                string templateCode = File.ReadAllText(templatePath);
                var template = Template.Parse(templateCode);
                if (template.HasErrors)
                    return;

                string output = template.Render(model);
                string outPath = Path.Combine(outputDir, outputFile);
                File.WriteAllText(outPath, output, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ShowWarning($"Errore generando {outputFile}: {ex.Message}");
            }
        }

        private void ShowInfo(string message) =>
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Passero Form Generator",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

        private void ShowWarning(string message) =>
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Passero Form Generator",
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

        private void ShowError(string message) =>
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Passero Form Generator",
                OLEMSGICON.OLEMSGICON_CRITICAL,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }
}
