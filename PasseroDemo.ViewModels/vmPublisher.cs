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

        public Models.Publisher GetPublisher(string pub_id)
        {
            var x= this.GetItem($"Select * FROM {this.GetTableName()} Where pub_id=@ID", new { ID = pub_id }).Value ;
            return x;   
        }


        public IEnumerable<Models.Publisher > GetPublishers ()
        {
            return this.GetItems($"Select * FROM {this.GetTableName()}").Value ;
        }
    }
}
