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
}