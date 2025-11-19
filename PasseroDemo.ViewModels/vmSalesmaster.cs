using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PasseroDemo.Models;

namespace PasseroDemo.ViewModels
{
    public class vmSalesmaster : Passero.Framework.ViewModel<Salesmaster>
    {
        public vmSalesmaster()
        {

        }

        public Salesmaster GetSalemaster(int ord_num)
        {
            return this.GetItem($"Select * FROM {this.Repository.GetTableName()} Where ord_num=@ord_num", new { ord_num }).Value ;
        }

        public IEnumerable<Salesmaster> GetSalemasters(string stor_id = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = $"Select * FROM {this.Repository.GetTableName()}";
            var parameters = new DynamicParameters();
            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(stor_id))
            {
                conditions.Add("stor_id = @stor_id");
                parameters.Add("stor_id", stor_id);
            }

            if (fromDate.HasValue)
            {
                conditions.Add("ord_date >= @fromDate");
                parameters.Add("fromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                conditions.Add("ord_date <= @toDate");
                parameters.Add("toDate", toDate.Value);
            }

            if (conditions.Any())
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            return this.GetItems(query, parameters).Value ;
        }

        public bool UpdateSalemaster(Salesmaster salemaster = null)
        {
            return (bool)this.UpdateItem(salemaster).Value;
        }
    }
}