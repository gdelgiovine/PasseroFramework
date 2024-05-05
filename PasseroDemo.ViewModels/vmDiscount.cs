using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmDiscount : Passero.Framework.ViewModel<Models.Discount>
    {
        public vmDiscount()
        {

        }

        public Models.Discount Get(string Id)
        {
            
            return this.Repository.DbConnection.Query<Models.Discount >($"Select * FROM {this.Repository.GetTableName()} Where discount_id=@Id", new { discount_id = Id }).Single();
        }


        public IEnumerable<Models.Discount> GetDiscounts ()
        {
            return this.Repository.DbConnection.Query<Models.Discount>($"Select * FROM {this.Repository.GetTableName()}");

        }
    }
}
