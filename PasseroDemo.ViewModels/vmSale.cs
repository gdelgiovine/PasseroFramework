using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmSale : Passero.Framework.ViewModel<Models.Sale>
    {
        public vmSale()
        {

        }

        public Models.Sale GetSale(string stor_id, string ord_num)
        {
            return this.Repository.DbConnection.Query<Models.Sale>($"Select * FROM {this.Repository.GetTableName()} Where stor_id=@stor_id AND ord_num=@ord_num", new { stor_id, ord_num }).SingleOrDefault();
        }

        public IEnumerable<Models.Sale> GetSales(string stor_id = null, DateTime? fromDate = null, DateTime? toDate = null)
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

            return this.Repository.DbConnection.Query<Models.Sale>(query, parameters);
        }
    }
}
