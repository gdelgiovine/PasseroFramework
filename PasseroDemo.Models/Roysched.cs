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
}