using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public class vmPubInfo : Passero.Framework.ViewModel<Models.Pub_Info>
    {
        public vmPubInfo()
        {

        }

        public Models.Pub_Info GetPubInfo(string pub_id)
        {
            return this.Repository.DbConnection.Query<Models.Pub_Info>($"Select * FROM {this.Repository.GetTableName()} Where pub_id=@pub_id", new { pub_id }).SingleOrDefault();
        }

        public IEnumerable<Models.Pub_Info> GetPubInfos()
        {
            return this.Repository.DbConnection.Query<Models.Pub_Info>($"Select * FROM {this.Repository.GetTableName()}");
        }

        public bool UpdatePubInfo(Models.Pub_Info pubInfo = null)
        {
            return (bool)this.UpdateItem(pubInfo).Value;
        }
    }
}