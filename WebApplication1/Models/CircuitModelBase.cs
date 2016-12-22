using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CircuitModelBase : TravelogyModelBase
    {
        public string CircuitName { get; set; }

        public List<Destination> AllDestinations { get; set; }
    }
}