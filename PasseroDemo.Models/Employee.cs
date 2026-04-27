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
    [Table("employee")]
    public class Employee : Passero.Framework.ModelBase
    {

        [ExplicitKey]
        public string emp_id { get; set; }
        public string fname { get; set; }
        public string minit { get; set; }
        public string lname { get; set; }
        public short job_id { get; set; }
        public byte? job_lvl { get; set; }
        public string pub_id { get; set; }
        public DateTime hire_date { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}