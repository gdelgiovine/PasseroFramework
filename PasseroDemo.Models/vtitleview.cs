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