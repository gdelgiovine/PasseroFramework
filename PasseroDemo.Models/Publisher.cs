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
    [Table("publishers")]
    public class Publisher : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        public string pub_id { get; set; }
        public string pub_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        [Computed]
        public string pub_fullinfo
        {
            get
            {
                return $"{pub_id} {pub_name }";
            }
        }

    }
}