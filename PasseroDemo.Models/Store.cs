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
}