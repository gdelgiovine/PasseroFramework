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
}