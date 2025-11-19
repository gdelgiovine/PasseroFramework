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
}