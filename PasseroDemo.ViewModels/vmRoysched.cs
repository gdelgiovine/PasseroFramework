using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmRoysched : Passero.Framework.ViewModel<Models.Roysched>
    {
        public vmRoysched()
        {

        }

        
        public IEnumerable<Models.Roysched> GetRoysched(string title_id = null)
        {
            var query = $"Select * FROM {this.Repository.GetTableName()}";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(title_id))
            {
                query += " WHERE title_id = @title_id";
                parameters.Add("title_id", title_id);
            }

            return this.GetItems(query, parameters).Value ;
        }
    }
}