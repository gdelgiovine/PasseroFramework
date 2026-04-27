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
    [BusinessAttributes.ObjectName("JOBS","TABELLA JOBS")]
    [Table("jobs")]
    public class Job : Passero.Framework.ModelBase
    {
        [ExplicitKey]
        [Description("A")]
        [BusinessAttributes.ObjectName ("JOB_ID","DESCRIZIONE")]
        public short job_id { get; set; }
        public string job_desc { get; set; }
        public byte min_lvl { get; set; }
        public byte max_lvl { get; set; }
        [Computed]
        public string job_fullinfo=> $"{job_id} ({job_desc})"; 
    }
}