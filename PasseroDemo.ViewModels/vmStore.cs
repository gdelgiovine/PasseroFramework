using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmStore : Passero.Framework.ViewModel<Models.Store>
    {
        public vmStore()
        {

        }

        public Models.Store GetStore(string stor_id)
        {
            return this.GetItem($"SELECT * FROM {this.Repository.GetTableName()} WHERE stor_id=@stor_id", new { stor_id }).Value;
        }

        public IEnumerable<Models.Store> GetStores()
        {
            return this.GetItems($"SELECT * FROM {this.Repository.GetTableName()}").Value;
        }
    }
}
