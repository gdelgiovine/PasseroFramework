using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework.BusinessSystem  
{
    /// <summary>
    /// Contiene gli attributi e le utilità per gestire la metainformazione business dell'applicazione
    /// </summary>
    public static class BusinessAttributes  
    {
        /// <summary>
        /// Attributo per definire il nome del sistema di business a cui appartiene una classe
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
        public class SystemNameAttribute : Attribute
        {
            /// <summary>
            /// Nome del sistema di business
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Inizializza una nuova istanza dell'attributo SystemName
            /// </summary>
            /// <param name="name">Nome del sistema di business</param>
            public SystemNameAttribute(string name)
            {
                Name = name ?? string.Empty;
            }
        }

        /// <summary>
        /// Attributo per definire il nome dell'oggetto di business (classe o proprietà)
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
        public class ObjectNameAttribute : Attribute
        {
            /// <summary>
            /// Nome dell'oggetto di business
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Descrizione dell'oggetto di business
            /// </summary>
            public string Description { get; }

            /// <summary>
            /// Inizializza una nuova istanza dell'attributo ObjectName
            /// </summary>
            /// <param name="name">Nome dell'oggetto di business</param>
            /// <param name="description">Descrizione dell'oggetto di business</param>
            public ObjectNameAttribute(string name, string description = "")
            {
                Name = name ?? string.Empty;
                Description = description ?? string.Empty;
            }
        }

        /// <summary>
        /// Rappresenta un nodo nella struttura gerarchica degli oggetti di business
        /// </summary>
        public class BusinessNode
        {
            /// <summary>
            /// Nome del nodo
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Descrizione del nodo
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Tipo associato al nodo (per classi)
            /// </summary>
            public Type Type { get; set; }

            /// <summary>
            /// Informazioni sulla proprietà associata (per proprietà)
            /// </summary>
            public PropertyInfo Property { get; set; }

            /// <summary>
            /// Nodi figlio
            /// </summary>
            public List<BusinessNode> Children { get; } = new List<BusinessNode>();

            /// <summary>
            /// Indica se il nodo rappresenta un oggetto o una proprietà
            /// </summary>
            public bool IsProperty => Property != null;

            /// <summary>
            /// Percorso completo del nodo nella gerarchia
            /// </summary>
            public string FullPath { get; set; }
        }

        /// <summary>
        /// Costruisce la struttura ad albero degli oggetti di business a partire da un insieme di tipi
        /// </summary>
        /// <param name="types">Tipi da analizzare</param>
        /// <returns>Radice della struttura ad albero</returns>
        public static BusinessNode BuildBusinessHierarchy(IEnumerable<Type> types)
        {
            var root = new BusinessNode { Name = "Root", FullPath = "Root" };
            var systemNodes = new Dictionary<string, BusinessNode>();

            foreach (var type in types)
            {
                // Recupera l'attributo SystemName se presente
                var systemAttr = type.GetCustomAttribute<SystemNameAttribute>();
                if (systemAttr == null)
                    continue;

                // Crea o recupera il nodo di sistema
                string systemName = systemAttr.Name;
                if (!systemNodes.TryGetValue(systemName, out BusinessNode systemNode))
                {
                    systemNode = new BusinessNode
                    {
                        Name = systemName,
                        FullPath = $"Root.{systemName}"
                    };
                    root.Children.Add(systemNode);
                    systemNodes[systemName] = systemNode;
                }

                // Recupera l'attributo ObjectName della classe se presente
                var objectAttr = type.GetCustomAttribute<ObjectNameAttribute>();
                if (objectAttr == null)
                    continue;

                // Crea il nodo per la classe
                var classNode = new BusinessNode
                {
                    Name = objectAttr.Name,
                    Description = objectAttr.Description,
                    Type = type,
                    FullPath = $"{systemNode.FullPath}.{objectAttr.Name}"
                };
                systemNode.Children.Add(classNode);

                // Analizza le proprietà della classe
                foreach (var prop in type.GetProperties())
                {
                    var propAttr = prop.GetCustomAttribute<ObjectNameAttribute>();
                    if (propAttr != null)
                    {
                        // Crea il nodo per la proprietà
                        var propNode = new BusinessNode
                        {
                            Name = propAttr.Name,
                            Description = propAttr.Description,
                            Property = prop,
                            FullPath = $"{classNode.FullPath}.{propAttr.Name}"
                        };
                        classNode.Children.Add(propNode);
                    }
                }
            }

            return root;
        }

        /// <summary>
        /// Recupera tutti i tipi decorati con gli attributi Business in un assembly
        /// </summary>
        /// <param name="assembly">Assembly da analizzare</param>
        /// <returns>Enumerazione dei tipi con attributi Business</returns>
        public static IEnumerable<Type> GetBusinessTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<SystemNameAttribute>() != null ||
                           t.GetCustomAttribute<ObjectNameAttribute>() != null);
        }

        /// <summary>
        /// Helper per esplorare le proprietà di un tipo a design time
        /// </summary>
        /// <param name="typeName">Nome completo del tipo</param>
        /// <returns>Lista di nodi rappresentanti le proprietà</returns>
        public static List<PropertyDescriptor> GetTypeProperties(string typeName)
        {
            try
            {
                Type type = Type.GetType(typeName);
                if (type == null)
                    return new List<PropertyDescriptor>();

                return GetTypeProperties(type);
            }
            catch
            {
                return new List<PropertyDescriptor>();
            }
        }

        /// <summary>
        /// Helper per esplorare le proprietà di un tipo a design time
        /// </summary>
        /// <param name="type">Tipo da esplorare</param>
        /// <returns>Lista di nodi rappresentanti le proprietà</returns>
        public static List<PropertyDescriptor> GetTypeProperties(Type type)
        {
            var result = new List<PropertyDescriptor>();

            if (type == null)
                return result;

            foreach (var prop in type.GetProperties())
            {
                var objectAttr = prop.GetCustomAttribute<ObjectNameAttribute>();

                result.Add(new PropertyDescriptor
                {
                    Name = prop.Name,
                    DisplayName = objectAttr?.Name ?? prop.Name,
                    Description = objectAttr?.Description ?? string.Empty,
                    PropertyType = prop.PropertyType,
                    DeclaringType = type
                });
            }

            return result;
        }

        /// <summary>
        /// Descrittore di una proprietà per l'esplorazione a design time
        /// </summary>
        public class PropertyDescriptor
        {
            /// <summary>
            /// Nome della proprietà
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Nome visualizzato della proprietà
            /// </summary>
            public string DisplayName { get; set; }

            /// <summary>
            /// Descrizione della proprietà
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Tipo della proprietà
            /// </summary>
            public Type PropertyType { get; set; }

            /// <summary>
            /// Tipo che dichiara la proprietà
            /// </summary>
            public Type DeclaringType { get; set; }

            /// <summary>
            /// Restituisce il nome visualizzato della proprietà
            /// </summary>
            public override string ToString()
            {
                return DisplayName;
            }
        }
    }
}