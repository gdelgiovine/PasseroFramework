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
}