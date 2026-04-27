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
    [Table("pub_info")]
    public class Pub_Info : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string pub_id { get; set; }
        public byte[] logo { get; set; }
        public string pr_info { get; set; }
    }
}