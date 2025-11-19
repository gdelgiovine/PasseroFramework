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
    public class AuthorSmall : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        [Dapper.Contrib.Extensions.Key]
        [ColumnMapping("au_id")]
        
        public string? au_id { get; set; }
        [ColumnMapping("au_lname")]
        public string? au_lname { get; set; }
        public string? au_fname { get; set; }
      

    }
}