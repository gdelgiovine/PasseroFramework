
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class Title : Passero.Framework.ViewModel<Models.Title>
    {
        public Title()
        {
        }

        public Models.Title Get(string title_id)
        {

            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Title>();
            return this.GetItem($"Select * FROM {this.Repository.GetTableName()} Where title_id=@ID", new { title_id = title_id }).Value ;
            
        }

        public List <Models.Title> GetTitles()
        {
            return this.GetAllItems().Value.ToList ();
        }
        public List<Models.Titleauthor > GetTitleAuthors(string title_id)
        {
            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Titleauthor>();
            return this.Repository .DbConnection .Query<Models.Titleauthor >($"Select * FROM {TableName} WHERE title_id=@title_id", new { title_id = title_id }).ToList ();
        }
    }
}

