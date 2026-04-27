using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasseroFormGenerator
{
    public class ModelBaseAnalyzer
    {
        public static List<ModelBaseEntity> FindModelBaseClasses(DTE2 dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var entities = new List<ModelBaseEntity>();

            try
            {
                var solution = dte.Solution;
                if (solution == null) return entities;

                foreach (Project project in solution.Projects)
                {
                    entities.AddRange(AnalyzeProject(project));
                }
            }
            catch (Exception ex)
            {
                // Log dell'errore se necessario
                System.Diagnostics.Debug.WriteLine($"Errore nell'analisi: {ex.Message}");
            }

            return entities;
        }

        private static List<ModelBaseEntity> AnalyzeProject(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var entities = new List<ModelBaseEntity>();

            try
            {
                if (project.ProjectItems == null) return entities;

                foreach (ProjectItem item in project.ProjectItems)
                {
                    entities.AddRange(AnalyzeProjectItem(item));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nell'analisi del progetto {project.Name}: {ex.Message}");
            }

            return entities;
        }

        private static List<ModelBaseEntity> AnalyzeProjectItem(ProjectItem item)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var entities = new List<ModelBaseEntity>();

            try
            {
                // Analizza i sottoelementi ricorsivamente
                if (item.ProjectItems != null)
                {
                    foreach (ProjectItem subItem in item.ProjectItems)
                    {
                        entities.AddRange(AnalyzeProjectItem(subItem));
                    }
                }

                // Analizza solo i file .cs
                if (item.Kind == Constants.vsProjectItemKindPhysicalFile)
                {
                    string filePath = item.FileNames[1];
                    if (Path.GetExtension(filePath).ToLower() == ".cs")
                    {
                        // CORREZIONE: Ora otteniamo TUTTE le classi dal file
                        var entitiesInFile = AnalyzeCSharpFile(filePath);
                        entities.AddRange(entitiesInFile);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nell'analisi dell'item: {ex.Message}");
            }

            return entities;
        }

        // METODO CORRETTO: Ora restituisce una Lista invece di una singola entità
        private static List<ModelBaseEntity> AnalyzeCSharpFile(string filePath)
        {
            var entities = new List<ModelBaseEntity>();

            try
            {
                if (!File.Exists(filePath)) return entities;

                string content = File.ReadAllText(filePath);
                var tree = CSharpSyntaxTree.ParseText(content);
                var root = tree.GetCompilationUnitRoot();

                var classDeclarations = root.DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(c => InheritsFromModelBase(c));

                // CORREZIONE: Ora itera su TUTTE le classi trovate
                foreach (var classDecl in classDeclarations)
                {
                    var entity = new ModelBaseEntity
                    {
                        ClassName = classDecl.Identifier.Text,
                        FilePath = filePath,
                        Namespace = GetNamespace(classDecl),
                        Properties = ExtractProperties(classDecl)
                    };

                    // Trova la chiave primaria
                    var primaryKeyProperty = entity.Properties.FirstOrDefault(p => 
                        p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.Equals(entity.ClassName + "Id", StringComparison.OrdinalIgnoreCase));
                    
                    if (primaryKeyProperty != null)
                        entity.PrimaryKey = primaryKeyProperty.Name;

                    // Aggiungi l'entità alla lista invece di restituire immediatamente
                    entities.Add(entity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nell'analisi del file {filePath}: {ex.Message}");
            }

            return entities;
        }

        private static bool InheritsFromModelBase(ClassDeclarationSyntax classDecl)
        {
            if (classDecl.BaseList == null) return false;

            return classDecl.BaseList.Types.Any(baseType =>
            {
                var typeName = baseType.Type.ToString();
                return typeName.Contains("ModelBase") || 
                       typeName.Contains("Passero.Framework.ModelBase") ||
                       typeName.Contains("PasseroFramework.ModelBase");
            });
        }

        private static string GetNamespace(ClassDeclarationSyntax classDecl)
        {
            // Supporta sia namespace tradizionali che file-scoped (C# 10+)
            var namespaceDecl = classDecl.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            if (namespaceDecl != null)
                return namespaceDecl.Name?.ToString() ?? "DefaultNamespace";

            // Supporto per file-scoped namespaces (C# 10+) - se necessario in futuro
            var fileScopedNamespace = classDecl.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            return fileScopedNamespace?.Name?.ToString() ?? "DefaultNamespace";
        }

        private static List<PropertyInfo> ExtractProperties(ClassDeclarationSyntax classDecl)
        {
            var properties = new List<PropertyInfo>();

            var propertyDeclarations = classDecl.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(p => p.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)) == true);

            foreach (var prop in propertyDeclarations)
            {
                var propertyInfo = new PropertyInfo
                {
                    Name = prop.Identifier.Text,
                    Type = prop.Type.ToString(),
                    IsNullable = prop.Type.ToString().Contains("?"),
                    DisplayName = prop.Identifier.Text
                };

                properties.Add(propertyInfo);
            }

            return properties;
        }
    }
}