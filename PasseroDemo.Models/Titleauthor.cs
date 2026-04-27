using Dapper.Contrib.Extensions;
using Passero.Framework.BusinessSystem;

namespace PasseroDemo.Models
{
    [BusinessAttributes.SystemName("ERP")]
    [Dapper.Contrib.Extensions.Table("titleauthor")]
    [System.ComponentModel.DataAnnotations.Schema.Table("titleauthor")]
    public class Titleauthor : Passero.Framework.ModelBase
    {
        /// <summary>
        /// Author ID — parte 1 della chiave primaria composta.
        /// </summary>
        [ExplicitKey]
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column(Order = 0)]
        public string au_id { get; set; }

        /// <summary>
        /// Title ID — parte 2 della chiave primaria composta.
        /// </summary>
        [ExplicitKey]
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column(Order = 1)]
        public string title_id { get; set; }

        public byte? au_ord { get; set; }

        public int? royaltyper { get; set; }
    }
}