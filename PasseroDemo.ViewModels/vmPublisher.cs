using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmPublisher : Passero.Framework.ViewModel<Models.Publisher >
    {


        public vmPublisher ()
        {

        }

        public Models.Publisher  Get(string au_id)
        {
            return this.Repository.DbConnection.Query<Models.Publisher >($"Select * FROM {this.Repository.GetTableName()} Where au_id=@ID", new { au_id = au_id }).Single();
        }


        public IEnumerable<Models.Publisher > GetPublishers ()
        {
            return this.Repository.DbConnection.Query<Models.Publisher>($"Select * FROM {this.Repository.GetTableName()}");

        }
    }
}
