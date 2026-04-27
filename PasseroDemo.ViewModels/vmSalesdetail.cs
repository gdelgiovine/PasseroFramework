using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PasseroDemo.Models;

namespace PasseroDemo.ViewModels
{
    public class vmSalesdetail : Passero.Framework.ViewModel<Salesdetail>
    {
        public vmSalesdetail()
        {

        }

        public IEnumerable<Models.Salesdetail> GetSalesdetail(int ord_num)
        {

            return this.GetItems($"Select * FROM {this.Repository.GetTableName()} Where ord_num=@ord_num", new { ord_num  }).Value;
        }

        
    }
}