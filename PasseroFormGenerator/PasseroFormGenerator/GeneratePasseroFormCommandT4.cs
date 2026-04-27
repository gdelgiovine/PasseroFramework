using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;
using System.IO;
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace PasseroFormGenerator
{
    internal sealed class GeneratePasseroFormCommandT4
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("7f78f87d-9a6b-46b1-9b71-1e3e4e9e2b7d");

        private readonly AsyncPackage package;
        private readonly ITextTemplating templatingService;

        public static GeneratePasseroFormCommandT4 Instance { get; private set; }

        private GeneratePasseroFormCommandT4(
            AsyncPackage package,
            OleMenuCommandService commandService,
            ITextTemplating templatingService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            this.templatingService = templatingService;

            if (commandService == null)
                throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService
                ?? throw new InvalidOperationException("OleMenuCommandService non disponibile.");

            ITextTemplating templating =
                await package.GetServiceAsync(typeof(STextTemplating)) as ITextTemplating;

            Instance = new GeneratePasseroFormCommandT4(package, commandService, templating);
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

                if (!TryGetSelectedProjectItem(dte, out var projectItem))
                {
                    ShowInfo("Seleziona un file di partenza nella Solution Explorer (es. classe modello).");
                    return;
                }

                string filePath;
                try
                {
                    filePath = projectItem.FileNames[1];
                }
                catch
                {
                    ShowInfo("Percorso file non determinabile.");
                    return;
                }

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    ShowInfo("File selezionato non valido.");
                    return;
                }

                string className = Path.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrWhiteSpace(className))
                {
                    ShowInfo("Nome entità non valido.");
                    return;
                }

                string assemblyDir = Path.GetDirectoryName(GetType().Assembly.Location);
                string templateDir = Path.Combine(assemblyDir ?? ".", "templates");
                if (!Directory.Exists(templateDir))
                {
                    ShowWarning("Cartella template non trovata:\r\n" + templateDir);
                    return;
                }

                string targetDir = Path.GetDirectoryName(filePath);
                if (string.IsNullOrEmpty(targetDir))
                {
                    ShowInfo("Directory di destinazione non valida.");
                    return;
                }

                if (templatingService == null)
                {
                    ShowWarning("Servizio T4 non disponibile. Verifica i pacchetti TextTemplating.");
                    return;
                }

                var parms = new T4Parameters
                {
                    Entity = className,
                    PrimaryKey = "Id",
                    Title = className + " Management",
                    ModelsNamespace = "MyApp.Models",
                    RepositoryNamespace = "MyApp.Repository",
                    ViewModelsNamespace = "MyApp.ViewModels"
                };

                string[] templateFiles = new[]
                {
                    "EntityRepository.tt",
                    "vmEntity.tt",
                    "frmEntity.tt",
                    "frmEntity.Designer.tt"
                };

                foreach (var templateFile in templateFiles)
                {
                    string outName = GetOutputFileName(templateFile, className);
                    if (outName == null)
                        continue;

                    string outPath = Path.Combine(targetDir, outName);
                    GenerateFromT4(templateDir, templateFile, outPath, parms);
                }

                ShowInfo("Generazione completata per entità: " + className);
            }
            catch (Exception ex)
            {
                ShowError("Errore generazione T4:\r\n" + ex.Message);
            }
        }

        private string GetOutputFileName(string templateFile, string entity)
        {
            switch (templateFile)
            {
                case "EntityRepository.tt": return entity + "Repository.cs";
                case "vmEntity.tt": return "vm" + entity + ".cs";
                case "frmEntity.tt": return "frm" + entity + ".cs";
                case "frmEntity.Designer.tt": return "frm" + entity + ".Designer.cs";
                default: return null;
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

        private void GenerateFromT4(string templateDir, string templateFile, string outputPath, T4Parameters parms)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string templatePath = Path.Combine(templateDir, templateFile);
            if (!File.Exists(templatePath))
            {
                ShowWarning("Template non trovato: " + templateFile);
                return;
            }

            if (!string.Equals(Path.GetExtension(templatePath), ".tt", StringComparison.OrdinalIgnoreCase))
            {
                ShowWarning("Ignorato (estensione non .tt): " + templateFile);
                return;
            }

            string templateContent;
            try
            {
                templateContent = File.ReadAllText(templatePath);
            }
            catch (Exception ex)
            {
                ShowWarning("Lettura template fallita (" + templateFile + "): " + ex.Message);
                return;
            }

            string output;
            try
            {
                // Approccio semplificato senza session host - usa solo il servizio base
                if (templatingService is ITextTemplatingSessionHost sessionHost)
                {
                    // Crea una nuova sessione e popola i parametri
                    sessionHost.Session = sessionHost.CreateSession();
                    sessionHost.Session["Entity"] = parms.Entity;
                    sessionHost.Session["PrimaryKey"] = parms.PrimaryKey;
                    sessionHost.Session["Title"] = parms.Title;
                    sessionHost.Session["ModelsNamespace"] = parms.ModelsNamespace;
                    sessionHost.Session["RepositoryNamespace"] = parms.RepositoryNamespace;
                    sessionHost.Session["ViewModelsNamespace"] = parms.ViewModelsNamespace;
                    
                    // Usa il ProcessTemplate senza callback personalizzato
                    output = templatingService.ProcessTemplate(templatePath, templateContent);
                }
                else
                {
                    // Fallback: sostituisci manualmente i parametri nel template
                    output = ProcessTemplateManually(templateContent, parms);
                }
            }
            catch (Exception ex)
            {
                ShowWarning("Errore trasformazione " + templateFile + ": " + ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                ShowWarning("Template " + templateFile + " ha prodotto output vuoto.");
                return;
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
                File.WriteAllText(outputPath, output, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ShowWarning("Scrittura file fallita (" + Path.GetFileName(outputPath) + "): " + ex.Message);
            }
        }

        private string ProcessTemplateManually(string templateContent, T4Parameters parms)
        {
            // Fallback semplice per sostituire parametri senza engine T4
            var result = templateContent;
            
            // Sostituisci le variabili più comuni
            result = result.Replace("<#= Entity #>", parms.Entity);
            result = result.Replace("<#= PrimaryKey #>", parms.PrimaryKey);
            result = result.Replace("<#= ModelsNamespace #>", parms.ModelsNamespace);
            result = result.Replace("<#= RepositoryNamespace #>", parms.RepositoryNamespace);
            result = result.Replace("<#= ViewModelsNamespace #>", parms.ViewModelsNamespace);
            result = result.Replace("<#= Title #>", parms.Title);
            
            // Rimuovi le direttive T4
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<#@.*?#>", "", 
                System.Text.RegularExpressions.RegexOptions.Singleline);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<#.*?#>", "", 
                System.Text.RegularExpressions.RegexOptions.Singleline);
            
            return result;
        }

        private void ShowInfo(string message)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Passero Form Generator (T4)",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private void ShowWarning(string message)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Passero Form Generator (T4)",
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private void ShowError(string message)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Passero Form Generator (T4)",
                OLEMSGICON.OLEMSGICON_CRITICAL,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private class T4Parameters
        {
            public string Entity { get; set; }
            public string PrimaryKey { get; set; }
            public string Title { get; set; }
            public string ModelsNamespace { get; set; }
            public string RepositoryNamespace { get; set; }
            public string ViewModelsNamespace { get; set; }
        }
    }
}