using System;
using Dapper.Contrib.Extensions;
using Dapper.ColumnMapper;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using Dapper;

namespace PasseroDemo.Models
{

    [Table("Authors")]
    public class AuthorSmall : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        [ColumnMapping("au_id")]
        public string? au_id { get; set; }
        [ColumnMapping("au_lname")]
        public string? au_lname { get; set; }
        public string? au_fname { get; set; }
      

        }
   
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

        public Author()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Author), new Dapper.ColumnMapper.ColumnTypeMapper(typeof(Author)));
            //oppure forma breve --- NOTA: se si passa un ColumTypeMapper nullo la mappatura viere riportata al default.
            //C'è anche una forma abbreviata
            //ColumnTypeMapper.RegisterForTypes(typeof(Author ));


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


    [Table("titles")]
    public class Title : Passero.Framework.ModelBase
    {
        [Dapper .Contrib .Extensions .ExplicitKey ]
        [System.ComponentModel .DataAnnotations .Key]
        public string title_id { get; set; }
        public string? title { get; set; }

     
        public string? type { get; set; }
        public string? pub_id { get; set; }
        public decimal? price { get; set; } = 0;
        public decimal? advance { get; set; } = 0;
        public int? royalty { get; set; } = 0;
        public int? ytd_sales { get; set; } = 0;
        public string? notes { get; set; }

        public DateTime? pubdate { get; set; }

        public Title()
        {
            //Dapper.SqlMapper.SetTypeMap(typeof(Title), new Dapper.ColumnMapper.ColumnTypeMapper(typeof(Title)));
        }
    }




    [Table("discounts")]
    public class Discount : Passero.Framework.ModelBase
    {

       
        [ExplicitKey]
        public string discount_id { get; set; }
        public string discounttype { get; set; }
        public string stor_id { get; set; } = "";
        public int lowqty { get; set; } = 0;
        public int highqty { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
       
    }

    [Table("employee")]
    public class Employee : Passero.Framework.ModelBase
    {

        [ExplicitKey]
        public string emp_id { get; set; }
        public string fname { get; set; }
        public string minit { get; set; }
        public string lname { get; set; }
        public short job_id { get; set; }
        public byte? job_lvl { get; set; }
        public string pub_id { get; set; }
        public DateTime hire_date { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    [Table("jobs")]
    public class Job : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public short job_id { get; set; }
        public string job_desc { get; set; }
        public byte min_lvl { get; set; }
        public byte max_lvl { get; set; }
    }

    [Table("pub_info")]
    public class Pub_info : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string pub_id { get; set; }
        public byte[] logo { get; set; }
        public string pr_info { get; set; }
    }

    [Table("publishers")]
    public class Publisher : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string pub_id { get; set; }
        public string pub_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    [Table("roysched")]
    public class Roysched : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string title_id { get; set; }
        [ExplicitKey]
        
        public int lorange { get; set; }
       
        public int hirange { get; set; }
        public int? royalty { get; set; }
    }

    [Table("sales")]
    public class Sale : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string stor_id { get; set; }
        
        public string ord_num { get; set; }
        
        public DateTime ord_date { get; set; }
        
        public short qty { get; set; }
        
        public string payterms { get; set; }
        
        public string title_id { get; set; }
    }

    [Table("salesdetails")]
    public class Salesdetail : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public int ord_num { get; set; }
        [ExplicitKey ]
        public string title_id { get; set; }
        public short qty { get; set; }
        public decimal price { get; set; }
    }

    [Table("salesmaster")]
    public class Salemaster : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public int ord_num { get; set; }
        public string stor_id { get; set; }
        public DateTime ord_date { get; set; }
        public string payterms { get; set; }
        public string stor_ord_num { get; set; }
        public DateTime? stor_ord_date { get; set; }
    }

    [Table("stores")]
    public class Store : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string stor_id { get; set; }
        public string? stor_name { get; set; }
        public string? stor_address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zip { get; set; }
        public string? mcode { get; set; }
    }

    [Table("titleauthor")]
    public class Titleauthor : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string au_id { get; set; }
        [ExplicitKey]
        public string title_id { get; set; }
        public byte? au_ord { get; set; }
        public int? royaltyper { get; set; }
    }

    
 
    public class vAuthorsFullname : Passero.Framework.ModelBase
    {
        public string au_id { get; set; }
        public string au_lname { get; set; }
        public string au_fname { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public bool contract { get; set; }
        public string email { get; set; }
        public string au_fullname { get; set; }
    }

    public class vpub_info_publisher : Passero.Framework.ModelBase
    {
        public string pub_id { get; set; }
        public string pr_info { get; set; }
        public byte[] logo { get; set; }
        public string pub_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class vtitles_publishers : Passero.Framework.ModelBase
    {
        public string title_id { get; set; }
        public string title { get; set; }
        public string pub_id { get; set; }
        public string pub_name { get; set; }
    }

    public class vtitleview : Passero.Framework.ModelBase
    {
        public string title { get; set; }
        public byte? au_ord { get; set; }
        public string au_lname { get; set; }
        public decimal? price { get; set; }
        public int? ytd_sales { get; set; }
        public string pub_id { get; set; }
    }


}