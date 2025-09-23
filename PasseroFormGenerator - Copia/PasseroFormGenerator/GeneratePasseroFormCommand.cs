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

        private GeneratePasseroFormCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            new GeneratePasseroFormCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var dte = (DTE2)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
                var item = ((Array)dte.ToolWindows.SolutionExplorer.SelectedItems).Cast<UIHierarchyItem>().FirstOrDefault();
                
                if (item?.Object is ProjectItem projectItem)
                {
                    string filePath = projectItem.FileNames[1];
                    string className = Path.GetFileNameWithoutExtension(filePath);
                    string templateDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "templates");

                    // Verifica che la cartella templates esista
                    if (!Directory.Exists(templateDir))
                    {
                        VsShellUtilities.ShowMessageBox(
                            this.package,
                            $"Template directory not found: {templateDir}",
                            "Wisej Form Generator",
                            OLEMSGICON.OLEMSGICON_WARNING,
                            OLEMSGBUTTON.OLEMSGBUTTON_OK,
                            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                        return;
                    }

                    var model = new { Entity = className, Title = className + " Management", PrimaryKey = "id" };

                    // Genera i file solo se i template esistono
                    GenerateFileFromTemplate(templateDir, "frmEntity.cs.scriban", Path.GetDirectoryName(filePath), $"frm{className}.cs", model);
                    GenerateFileFromTemplate(templateDir, "frmEntity.Designer.cs.scriban", Path.GetDirectoryName(filePath), $"frm{className}.Designer.cs", model);
                    GenerateFileFromTemplate(templateDir, "vmEntity.cs.scriban", Path.GetDirectoryName(filePath), $"vm{className}.cs", model);
                    GenerateFileFromTemplate(templateDir, "EntityRepository.cs.scriban", Path.GetDirectoryName(filePath), $"{className}Repository.cs", model);

                    VsShellUtilities.ShowMessageBox(
                        this.package,
                        $"Wisej form files generated for {className}",
                        "Wisej Form Generator",
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                }
            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    $"Error generating Wisej form: {ex.Message}",
                    "Wisej Form Generator",
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        private void GenerateFileFromTemplate(string templateDir, string templateFile, string outputDir, string outputFile, object model)
        {
            string templatePath = Path.Combine(templateDir, templateFile);
            
            if (File.Exists(templatePath))
            {
                string templateCode = File.ReadAllText(templatePath);
                var template = Template.Parse(templateCode);
                string output = template.Render(model);
                string outPath = Path.Combine(outputDir, outputFile);
                File.WriteAllText(outPath, output);
            }
        }
    }
}
