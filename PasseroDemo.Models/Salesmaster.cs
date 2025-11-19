using System;
using Dapper.Contrib.Extensions;
using Dapper.ColumnMapper;
using Dapper;
using Passero.Framework.BusinessSystem;
using System.ComponentModel;

namespace PasseroDemo.Models
{
    [BusinessAttributes.SystemName("ERP")]
    [Table("salesmaster")]
    public class Salesmaster : Passero.Framework.ModelBase
    {
        [Dapper.Contrib.Extensions.Key]
        public int ord_num { get; set; }
        public string stor_id { get; set; }
        public DateTime ord_date { get; set; }
        public string payterms { get; set; }
        public string stor_ord_num { get; set; }
        public DateTime? stor_ord_date { get; set; }
    }
}