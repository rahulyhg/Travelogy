using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CircuitModelBase : DomingoModelBase
    {
        public string CircuitName { get; set; }

        public List<Destination> AllDestinations { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CircuitDestinationViewModel
    {
        public string Name { get; set; }

        public List<SubDestination> SubDestinations { get; set; }

        public List<DestinationActivity> Activities { get; set; }
    }    
}