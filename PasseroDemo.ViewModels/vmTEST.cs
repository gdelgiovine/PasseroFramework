using Passero.Framework;
using Passero.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasseroDemo.ViewModels
{
    public  class vmTEST: Passero .Framework.ViewModel<Models.Job>  
    {
     
        private ViewModel<Models.Job> vmJob = new ViewModel<Models.Job>("vmJob");

        public vmTEST()
        {
          

        }


        public void GetJobs()
        {
            //var jobs = this.GetItems <Models.Job>("Jobs", "Select * FROM Jobs");
            
            

            string SQLQuery = "Select * FROM Jobs"; 
            //var jobs = this.GetItems<Models.Job>(jobsRepository, SQLQuery );

          


        }   

    }
}
