using System.Collections.Generic;

namespace PasseroFormGenerator
{
    public class ModelBaseEntity
    {
        public string ClassName { get; set; }
        public string FilePath { get; set; }
        public string Namespace { get; set; }
        public List<PropertyInfo> Properties { get; set; } = new List<PropertyInfo>();
        public string PrimaryKey { get; set; } = "Id";
        
        public override string ToString()
        {
            return ClassName;
        }
    }

    public class PropertyInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsNullable { get; set; }
        public bool IsRequired { get; set; }
        public string DisplayName { get; set; }
    }
}