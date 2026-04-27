using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Passero.Framework;

namespace PasseroDemo.Repositories
{

    public class rpTitle : Passero.Framework.Repository<Models.Title>
    {
        public rpTitle() { }

        public ExecutionResult<Models.Title> GetTitle(string title_id)
        {
            return this.GetItem($"SELECT * FROM {this.GetTableName()} WHERE title_id = @title_id", new { title_id });
        }

        public ExecutionResult<IList<Models.Title>> GetTitles()
        {
            return this.GetAllItems();
        }

        public ExecutionResult<IList<Models.Title>> GetTitlesByPublisher(string pub_id)
        {
            return this.GetItems($"SELECT * FROM {this.GetTableName()} WHERE pub_id = @pub_id", new { pub_id });
        }
    }

}