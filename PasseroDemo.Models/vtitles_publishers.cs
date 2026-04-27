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
    public class vtitles_publishers : Passero.Framework.ModelBase
    {
        public string title_id { get; set; }
        public string title { get; set; }
        public string pub_id { get; set; }
        public string pub_name { get; set; }
    }
}