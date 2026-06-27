using System;
using Dapper.Contrib.Extensions;
using Dapper.ColumnMapper;
using System.ComponentModel.DataAnnotations;
using Dapper;
using Passero.Framework.BusinessSystem;
using System.ComponentModel;

namespace PasseroDemo.Models
{
    [Table("TestTable")]
    public class TestTable
    {
        [Dapper.Contrib .Extensions.Key]
        [System.ComponentModel.DataAnnotations.Key]
        public int pk_id { get; set; }
        public string c_string { get; set; }
        public int? c_int { get; set; }
        public DateTime? c_datetime { get; set; }
        public DateTime? c_date { get; set; }
        public TimeSpan? c_time { get; set; }
        public decimal? c_numeric { get; set; }
        public double? c_float { get; set; }
        public bool? c_boolean { get; set; }
    }

}
