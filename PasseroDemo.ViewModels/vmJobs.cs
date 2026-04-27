using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Passero.Framework;
namespace PasseroDemo.ViewModels
{
    public class vmJobs : Passero.Framework.ViewModel<Models .Job >
    {
        public ViewModel<Models.Job> Jobs = new ViewModel<Models.Job >();

        public vmJobs()
        {
            
            //this.Repository.SQLQuery = "Select * FROM Jobs";
            //this.Repository.Parameters = new { };
            //this.UseModelData = Passero.Framework.UseModelData.External;
        }

    }
}
