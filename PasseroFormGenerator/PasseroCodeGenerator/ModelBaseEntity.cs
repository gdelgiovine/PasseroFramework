using System.Collections.Generic;

namespace PasseroCodeGeneratorStandalone
{
    public class ModelBaseEntity
    {
        public string ClassName { get; set; }
        public string FilePath { get; set; }
        public string Namespace { get; set; }
        public List<PropertyInfo> Properties { get; set; } = new List<PropertyInfo>();
        public List<string> PrimaryKey { get; set; } = new List<string>(); // Cambiato da string a List<string>

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
        public bool IsKey { get; set; } = false;        // Chiave primaria
        public bool IsExplicitKey { get; set; } = false; // Chiave primaria esplicita (non auto-incrementale)

        // Nuove proprietà per gli attributi aggiuntivi
        public bool IsComputed { get; set; } = false;    // Proprietà calcolata (Dapper.Contrib)
        public bool IsWritable { get; set; } = true;     // Proprietà scrivibile (Dapper.Contrib)
        public string ColumnName { get; set; }           // Nome della colonna nel database
        public bool IsNotMapped { get; set; } = false;   // Proprietà non mappata sul database

        // Attributi di validazione DataAnnotations
        public int? StringLengthMax { get; set; }        // Lunghezza massima stringa
        public int? StringLengthMin { get; set; }        // Lunghezza minima stringa
        public string RangeMin { get; set; }             // Valore minimo range
        public string RangeMax { get; set; }             // Valore massimo range
        public string RegularExpression { get; set; }    // Pattern espressione regolare
        public bool IsEmailAddress { get; set; } = false; // Validazione email
        public bool IsPhoneNumber { get; set; } = false;  // Validazione telefono
        public bool IsUrl { get; set; } = false;          // Validazione URL
        public bool IsCreditCard { get; set; } = false;   // Validazione carta di credito
        public string CompareProperty { get; set; }       // Proprietà per confronto

        // Attributi di visualizzazione
        public string Description { get; set; }          // Descrizione del campo
        public int? DisplayOrder { get; set; }           // Ordine di visualizzazione
        public string DisplayFormat { get; set; }        // Formato di visualizzazione
        public bool ApplyFormatInEditMode { get; set; } = false; // Applica formato in modifica

        // Attributi di database
        public bool IsForeignKey { get; set; } = false;  // Chiave esterna
        public string ForeignKeyProperty { get; set; }   // Proprietà chiave esterna
        public bool IsTimestamp { get; set; } = false;   // Timestamp per concorrenza
        public bool IsConcurrencyCheck { get; set; } = false; // Controllo concorrenza
        public bool IsEditable { get; set; } = true;     // Campo modificabile
        public bool IsScaffoldColumn { get; set; } = true; // Incluso nello scaffolding

        // Costruttore che inizializza il ColumnName con il Name
        public PropertyInfo()
        {
            ColumnName = Name;
        }
    }
}