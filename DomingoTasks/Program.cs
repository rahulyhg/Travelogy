using DomingoBL.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            var gApi = new GoogleApi();
            gApi.PopulateDistances();
        }
    }
}
