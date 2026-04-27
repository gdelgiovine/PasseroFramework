using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PasseroCodeGeneratorStandalone;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PasseroCodeGeneratorStandalone
{
    public class ModelBaseAnalyzer
    {
        public static List<ModelBaseEntity> AnalyzeAssembly(string assemblyPath)
        {
            var entities = new List<ModelBaseEntity>();

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var modelBaseTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && InheritsFromModelBase(t));

                foreach (var type in modelBaseTypes)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"Analizzando tipo: {type.FullName}");
                        
                        var properties = ExtractPropertiesFromType(type);
                        
                        var entity = new ModelBaseEntity
                        {
                            ClassName = type.Name,
                            FilePath = assemblyPath,
                            Namespace = type.Namespace ?? "DefaultNamespace",
                            Properties = properties,
                            PrimaryKey = properties.Where(p => p.IsKey || p.IsExplicitKey).Select(p => p.Name).ToList()
                        };

                        // Se non ci sono chiavi con attributi, fallback alla logica precedente
                        if (entity.PrimaryKey.Count == 0)
                        {
                            var fallbackKey = properties.FirstOrDefault(p => 
                                p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                                p.Name.Equals(entity.ClassName + "Id", StringComparison.OrdinalIgnoreCase));
                            
                            if (fallbackKey != null)
                                entity.PrimaryKey.Add(fallbackKey.Name);
                        }

                        System.Diagnostics.Debug.WriteLine($"Trovate {entity.PrimaryKey.Count} chiavi primarie per {type.Name}");
                        entities.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Errore nell'analisi del tipo {type.Name}: {ex.Message}");
                        // Continua con gli altri tipi
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Errore nell'analisi dell'assembly: {ex.Message}", ex);
            }

            return entities;
        }

        public static List<ModelBaseEntity> AnalyzeCSharpFiles(string[] filePaths)
        {
            var entities = new List<ModelBaseEntity>();

            foreach (var filePath in filePaths)
            {
                try
                {
                    entities.AddRange(AnalyzeCSharpFile(filePath));
                }
                catch (Exception ex)
                {
                    // Log dell'errore ma continua con gli altri file
                    System.Diagnostics.Debug.WriteLine($"Errore nell'analisi del file {filePath}: {ex.Message}");
                }
            }

            return entities;
        }

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

                foreach (var classDecl in classDeclarations)
                {
                    var properties = ExtractProperties(classDecl);
                    
                    var entity = new ModelBaseEntity
                    {
                        ClassName = classDecl.Identifier.Text,
                        FilePath = filePath,
                        Namespace = GetNamespace(classDecl),
                        Properties = properties,
                        PrimaryKey = properties.Where(p => p.IsKey || p.IsExplicitKey).Select(p => p.Name).ToList()
                    };

                    // Se non ci sono chiavi con attributi, fallback alla logica precedente
                    if (entity.PrimaryKey.Count == 0)
                    {
                        var fallbackKey = properties.FirstOrDefault(p => 
                            p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                            p.Name.Equals(entity.ClassName + "Id", StringComparison.OrdinalIgnoreCase));
                        
                        if (fallbackKey != null)
                            entity.PrimaryKey.Add(fallbackKey.Name);
                    }

                    entities.Add(entity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nell'analisi del file {filePath}: {ex.Message}");
            }

            return entities;
        }

        private static bool InheritsFromModelBase(Type type)
        {
            var baseType = type.BaseType;
            while (baseType != null)
            {
                if (baseType.Name.Contains("ModelBase") || 
                    baseType.FullName?.Contains("PasseroFramework.ModelBase") == true ||
                    baseType.FullName?.Contains("Passero.Framework.ModelBase") == true)
                {
                    return true;
                }
                baseType = baseType.BaseType;
            }
            return false;
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

        private static List<PropertyInfo> ExtractPropertiesFromType(Type type)
        {
            var properties = new List<PropertyInfo>();

            var publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in publicProperties)
            {
                var propertyInfo = new PropertyInfo
                {
                    Name = prop.Name,
                    Type = GetSimpleTypeName(prop.PropertyType),
                    IsNullable = IsNullableType(prop.PropertyType),
                    DisplayName = prop.Name,
                    ColumnName = prop.Name
                };

                try
                {
                    var allAttributes = prop.GetCustomAttributes(false);
                    
                    foreach (var attr in allAttributes)
                    {
                        var attrTypeName = attr.GetType().Name;
                        var attrFullName = attr.GetType().FullName;
                        
                        // DataAnnotations Key
                        if (attrTypeName == "KeyAttribute" && attrFullName?.Contains("System.ComponentModel.DataAnnotations") == true)
                        {
                            propertyInfo.IsKey = true;
                        }
                        
                        // Dapper.Contrib Key
                        if (attrTypeName == "KeyAttribute" && attrFullName?.Contains("Dapper.Contrib") == true)
                        {
                            propertyInfo.IsKey = true;
                        }
                        
                        // Dapper.Contrib ExplicitKey
                        if (attrTypeName == "ExplicitKeyAttribute")
                        {
                            propertyInfo.IsExplicitKey = true;
                        }
                        
                        // Dapper.Contrib Computed
                        if (attrTypeName == "ComputedAttribute")
                        {
                            propertyInfo.IsComputed = true;
                            propertyInfo.IsWritable = false;
                        }
                        
                        // Dapper.Contrib Write
                        if (attrTypeName == "WriteAttribute")
                        {
                            try
                            {
                                var writeProperty = attr.GetType().GetProperty("Write");
                                if (writeProperty != null)
                                {
                                    var writeValue = writeProperty.GetValue(attr);
                                    propertyInfo.IsWritable = (bool)(writeValue ?? true);
                                }
                            }
                            catch
                            {
                                propertyInfo.IsWritable = true;
                            }
                        }
                        
                        // DataAnnotations Required
                        if (attrTypeName == "RequiredAttribute")
                        {
                            propertyInfo.IsRequired = true;
                        }
                        
                        // DataAnnotations Display
                        if (attrTypeName == "DisplayAttribute")
                        {
                            try
                            {
                                var nameProperty = attr.GetType().GetProperty("Name");
                                var descProperty = attr.GetType().GetProperty("Description");
                                var orderProperty = attr.GetType().GetProperty("Order");
                                
                                if (nameProperty != null)
                                {
                                    var nameValue = nameProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(nameValue))
                                        propertyInfo.DisplayName = nameValue;
                                }
                                
                                if (descProperty != null)
                                {
                                    var descValue = descProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(descValue))
                                        propertyInfo.Description = descValue;
                                }
                                
                                if (orderProperty != null)
                                {
                                    var orderValue = orderProperty.GetValue(attr);
                                    if (orderValue != null)
                                        propertyInfo.DisplayOrder = (int)orderValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations DisplayName
                        if (attrTypeName == "DisplayNameAttribute")
                        {
                            try
                            {
                                var displayNameProperty = attr.GetType().GetProperty("DisplayName");
                                if (displayNameProperty != null)
                                {
                                    var displayNameValue = displayNameProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(displayNameValue))
                                        propertyInfo.DisplayName = displayNameValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations Column
                        if (attrTypeName == "ColumnAttribute" && attrFullName?.Contains("System.ComponentModel.DataAnnotations") == true)
                        {
                            try
                            {
                                var nameProperty = attr.GetType().GetProperty("Name");
                                if (nameProperty != null)
                                {
                                    var nameValue = nameProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(nameValue))
                                        propertyInfo.ColumnName = nameValue;
                                }
                            }
                            catch { }
                        }
                        
                        // Dapper.ColumnMapper ColumnMapping
                        if (attrTypeName == "ColumnMappingAttribute")
                        {
                            try
                            {
                                var columnNameProperty = attr.GetType().GetProperty("ColumnName");
                                if (columnNameProperty != null)
                                {
                                    var columnNameValue = columnNameProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(columnNameValue))
                                        propertyInfo.ColumnName = columnNameValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations StringLength
                        if (attrTypeName == "StringLengthAttribute")
                        {
                            try
                            {
                                var maxProperty = attr.GetType().GetProperty("MaximumLength");
                                var minProperty = attr.GetType().GetProperty("MinimumLength");
                                
                                if (maxProperty != null)
                                {
                                    var maxValue = maxProperty.GetValue(attr);
                                    if (maxValue != null)
                                        propertyInfo.StringLengthMax = (int)maxValue;
                                }
                                
                                if (minProperty != null)
                                {
                                    var minValue = minProperty.GetValue(attr);
                                    if (minValue != null)
                                        propertyInfo.StringLengthMin = (int)minValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations Range
                        if (attrTypeName == "RangeAttribute")
                        {
                            try
                            {
                                var minProperty = attr.GetType().GetProperty("Minimum");
                                var maxProperty = attr.GetType().GetProperty("Maximum");
                                
                                if (minProperty != null)
                                {
                                    var minValue = minProperty.GetValue(attr);
                                    if (minValue != null)
                                        propertyInfo.RangeMin = minValue.ToString();
                                }
                                
                                if (maxProperty != null)
                                {
                                    var maxValue = maxProperty.GetValue(attr);
                                    if (maxValue != null)
                                        propertyInfo.RangeMax = maxValue.ToString();
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations RegularExpression
                        if (attrTypeName == "RegularExpressionAttribute")
                        {
                            try
                            {
                                var patternProperty = attr.GetType().GetProperty("Pattern");
                                if (patternProperty != null)
                                {
                                    var patternValue = patternProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(patternValue))
                                        propertyInfo.RegularExpression = patternValue;
                                }
                            }
                            catch { }
                        }
                        
                        // Altri attributi di validazione
                        if (attrTypeName == "EmailAddressAttribute")
                            propertyInfo.IsEmailAddress = true;
                        
                        if (attrTypeName == "PhoneAttribute")
                            propertyInfo.IsPhoneNumber = true;
                        
                        if (attrTypeName == "UrlAttribute")
                            propertyInfo.IsUrl = true;
                        
                        if (attrTypeName == "CreditCardAttribute")
                            propertyInfo.IsCreditCard = true;
                        
                        // DataAnnotations Compare
                        if (attrTypeName == "CompareAttribute")
                        {
                            try
                            {
                                var otherProperty = attr.GetType().GetProperty("OtherProperty");
                                if (otherProperty != null)
                                {
                                    var otherValue = otherProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(otherValue))
                                        propertyInfo.CompareProperty = otherValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations NotMapped
                        if (attrTypeName == "NotMappedAttribute")
                            propertyInfo.IsNotMapped = true;
                        
                        // DataAnnotations ForeignKey
                        if (attrTypeName == "ForeignKeyAttribute")
                        {
                            propertyInfo.IsForeignKey = true;
                            try
                            {
                                var nameProperty = attr.GetType().GetProperty("Name");
                                if (nameProperty != null)
                                {
                                    var nameValue = nameProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(nameValue))
                                        propertyInfo.ForeignKeyProperty = nameValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations Timestamp
                        if (attrTypeName == "TimestampAttribute")
                            propertyInfo.IsTimestamp = true;
                        
                        // DataAnnotations ConcurrencyCheck
                        if (attrTypeName == "ConcurrencyCheckAttribute")
                            propertyInfo.IsConcurrencyCheck = true;
                        
                        // DataAnnotations Editable
                        if (attrTypeName == "EditableAttribute")
                        {
                            try
                            {
                                var allowEditProperty = attr.GetType().GetProperty("AllowEdit");
                                if (allowEditProperty != null)
                                {
                                    var allowEditValue = allowEditProperty.GetValue(attr);
                                    if (allowEditValue != null)
                                        propertyInfo.IsEditable = (bool)allowEditValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations ScaffoldColumn
                        if (attrTypeName == "ScaffoldColumnAttribute")
                        {
                            try
                            {
                                var scaffoldProperty = attr.GetType().GetProperty("Scaffold");
                                if (scaffoldProperty != null)
                                {
                                    var scaffoldValue = scaffoldProperty.GetValue(attr);
                                    if (scaffoldValue != null)
                                        propertyInfo.IsScaffoldColumn = (bool)scaffoldValue;
                                }
                            }
                            catch { }
                        }
                        
                        // DataAnnotations DisplayFormat
                        if (attrTypeName == "DisplayFormatAttribute")
                        {
                            try
                            {
                                var formatProperty = attr.GetType().GetProperty("DataFormatString");
                                var editModeProperty = attr.GetType().GetProperty("ApplyFormatInEditMode");
                                
                                if (formatProperty != null)
                                {
                                    var formatValue = formatProperty.GetValue(attr) as string;
                                    if (!string.IsNullOrEmpty(formatValue))
                                        propertyInfo.DisplayFormat = formatValue;
                                }
                                
                                if (editModeProperty != null)
                                {
                                    var editModeValue = editModeProperty.GetValue(attr);
                                    if (editModeValue != null)
                                        propertyInfo.ApplyFormatInEditMode = (bool)editModeValue;
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore nel rilevamento attributi per proprietà {prop.Name}: {ex.Message}");
                }

                properties.Add(propertyInfo);
            }

            return properties;
        }

        private static List<PropertyInfo> ExtractProperties(ClassDeclarationSyntax classDecl)
        {
            var properties = new List<PropertyInfo>();

            var propertyDeclarations = classDecl.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(p => p.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)) == true);

            foreach (var prop in propertyDeclarations)
            {
                // Verifica attributi nei file C#
                bool hasKeyAttribute = false;
                bool hasExplicitKeyAttribute = false;

                if (prop.AttributeLists != null)
                {
                    foreach (var attrList in prop.AttributeLists)
                    {
                        foreach (var attr in attrList.Attributes)
                        {
                            var attrName = attr.Name.ToString();
                            if (attrName.Contains("Key") && !attrName.Contains("ExplicitKey"))
                                hasKeyAttribute = true;
                            if (attrName.Contains("ExplicitKey"))
                                hasExplicitKeyAttribute = true;
                        }
                    }
                }

                var propertyInfo = new PropertyInfo
                {
                    Name = prop.Identifier.Text,
                    Type = prop.Type.ToString(),
                    IsNullable = prop.Type.ToString().Contains("?"),
                    DisplayName = prop.Identifier.Text,
                    IsKey = hasKeyAttribute,
                    IsExplicitKey = hasExplicitKeyAttribute
                };

                properties.Add(propertyInfo);
            }

            return properties;
        }

        private static string GetNamespace(ClassDeclarationSyntax classDecl)
        {
            var namespaceDecl = classDecl.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            if (namespaceDecl != null)
                return namespaceDecl.Name?.ToString() ?? "DefaultNamespace";

            var fileScopedNamespace = classDecl.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            return fileScopedNamespace?.Name?.ToString() ?? "DefaultNamespace";
        }

        private static string GetSimpleTypeName(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetSimpleTypeName(type.GetGenericArguments()[0]) + "?";
            }

            switch (type.Name)
            {
                case "String": return "string";
                case "Int32": return "int";
                case "Int64": return "long";
                case "Decimal": return "decimal";
                case "Double": return "double";
                case "Single": return "float";
                case "Boolean": return "bool";
                case "DateTime": return "DateTime";
                default: return type.Name;
            }
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}