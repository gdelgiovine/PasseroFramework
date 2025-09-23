using Scriban;
using Scriban.Runtime;
using System;
using System.IO;
using System.Text;

namespace PasseroFormGeneratorStandalone
{
    public class TemplateManager
    {
        private readonly string templateDirectory;

        public TemplateManager(string templateDirectory)
        {
            this.templateDirectory = templateDirectory;
        }

        public void GenerateFileFromTemplate(string templateFile, string outputDir, string outputFile, object model)
        {
            string templatePath = Path.Combine(templateDirectory, templateFile);
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template non trovato: {templatePath}");
            }

            try
            {
                string templateCode = File.ReadAllText(templatePath);
                var template = Template.Parse(templateCode);
                
                if (template.HasErrors)
                {
                    var errors = string.Join(Environment.NewLine, template.Messages);
                    throw new InvalidOperationException($"Errori nel template {templateFile}:\n{errors}");
                }

                TemplateContext templateContext = new TemplateContext();
                // Mantiene i nomi delle proprietà senza conversione snake_case
                templateContext.MemberRenamer = member => member.Name;
                // Converti l'oggetto in ScriptObject
                var scriptObject = new ScriptObject();
                scriptObject.Import(model, renamer: templateContext.MemberRenamer);

                templateContext.PushGlobal(scriptObject);

                string output = template.Render(templateContext);

                //string output = template.Render(model);
                
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                string outPath = Path.Combine(outputDir, outputFile);
                File.WriteAllText(outPath, output, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Errore generando {outputFile}: {ex.Message}", ex);
            }
        }

        public bool TemplateExists(string templateFile)
        {
            string templatePath = Path.Combine(templateDirectory, templateFile);
            return File.Exists(templatePath);
        }
    }
}