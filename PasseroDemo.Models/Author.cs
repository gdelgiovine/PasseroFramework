using System;
using Dapper.Contrib.Extensions;
using Dapper.ColumnMapper;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using Dapper;
using Passero.Framework.BusinessSystem;
using System.ComponentModel;

namespace PasseroDemo.Models
{
    [BusinessAttributes.SystemName("ERP")]
    [Table("Authors")]
    public class Author: Passero.Framework.ModelBase 
    {
        [ExplicitKey]
        [ColumnMapping("au_id")]
        public string? au_id { get; set; }
        [ColumnMapping("au_lname")]
        public string? au_lname { get; set; }
        public string? au_fname { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zip { get; set; }
        public bool contract { get; set; }
        public string? email { get; set; }
        [Computed]
        public string? au_fullname
        { get
            { 
                return $"{au_fname?.Trim()} {au_lname?.Trim()}"; 
            }
          set 
            {
            }
        }   

        static  Author()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Author), new Dapper.ColumnMapper.ColumnTypeMapper(typeof(Author)));
            //oppure forma breve --- NOTA: se si passa un ColumTypeMapper nullo la mappatura viere riportata al default.
            //C'è anche una forma abbreviata
            //ColumnTypeMapper.RegisterForTypes(typeof(Author  ));


            //Si possono usare tutti i fornitori di Mapping che uno vuole usanto una nuova istanza di TypeMapper nel secondo argomento
            //della funzione SetTypeMap. Nell'esempio si usa System.ComponentModel.DataAnnotation.Schema

            //Dapper.SqlMapper.SetTypeMap(typeof(Author), new CustomPropertyTypeMap(typeof(Author), (type, columnName) =>
            //                        type.GetProperties().FirstOrDefault(prop =>
            //                        prop.GetCustomAttributes(false)
            //                        //.OfType<System.Data.Linq.Mapping.ColumnAttribute>()
            //                        .OfType<System.ComponentModel .DataAnnotations.Schema.ColumnAttribute>()
            //                        .Any(attr => attr.Name == columnName))));

            //Si può anche usare un mapper custom senza ricorrere alle annotazioni. Cosi si riesce a creare un mapping dinamico a runtime
            //var AuthorMap = new CustomPropertyTypeMap( typeof(Author ),
            //                                (type, columnName) =>
            //                                {
            //                                    if (columnName == "AuthorId")
            //                                    {
            //                                        return type.GetProperty("au_id");
            //                                    }

            //                                    if (columnName == "AuthorName")
            //                                    {
            //                                        return type.GetProperty("au_fname");
            //                                    }

            //                                    throw new InvalidOperationException($"No matching mapping for {columnName}");
            //                                 });
            //Dapper.SqlMapper.SetTypeMap(typeof(Author ), AuthorMap);
        }
    }
}