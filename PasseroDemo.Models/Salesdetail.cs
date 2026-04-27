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
    [Table("salesdetails")]
    public class Salesdetail : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column(Order = 0)]
        public int ord_num { get; set; }
        [ExplicitKey]
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column(Order = 1)]
        public string title_id { get; set; }
        public short qty { get; set; }
        public decimal price { get; set; }
    }
}