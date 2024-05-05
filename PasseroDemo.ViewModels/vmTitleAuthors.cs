using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{

    public class TitleAuthor : Passero.Framework.ViewModel<Models.Titleauthor >
    {
        public TitleAuthor()
        {
        }

        public Models.Titleauthor Get(string title_id, string au_id)
        {

            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Titleauthor>();
            return this.GetItem($"Select * FROM {this.Repository.GetTableName()} Where title_id=@title_id and au_id=@au_id", new { title_id = title_id, au_id =au_id });

        }

        
        public List<Models.Titleauthor> GetTitleAuthors(string title_id)
        {
            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Titleauthor>();
            return this.Repository.DbConnection.Query<Models.Titleauthor>($"Select * FROM {TableName} WHERE title_id=@title_id", new { title_id = title_id }).ToList();
        }

        public List<Models.Titleauthor> GetAuthorTitles(string au_id)
        {
            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName<Models.Titleauthor>();
            return this.Repository.DbConnection.Query<Models.Titleauthor>($"Select * FROM {TableName} WHERE au_id=@au_id", new { au_id =au_id }).ToList();
        }
    }
}
