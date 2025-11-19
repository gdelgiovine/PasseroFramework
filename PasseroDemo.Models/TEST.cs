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
    [Table("TEST")]
    public class TEST : Passero.Framework.ModelBase
    {
        [Dapper.Contrib.Extensions.ExplicitKey]
        [Dapper.Contrib.Extensions.Key]
        public int id { get; set; }
        public string name { get; set; }    
    }
}