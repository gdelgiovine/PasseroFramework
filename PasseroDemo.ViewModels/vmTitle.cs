
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmTitle : Passero.Framework.ViewModel<Models.Title>
    {
        public vmTitle()
        {
        }

        public Models.Title GetTitle(string title_id)
        {

            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Title>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@title_id", title_id);  
            string sql = $"Select * FROM {TableName} Where title_id=@title_id";
            return this.GetItem(sql,parameters ).Value  ;
            
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

