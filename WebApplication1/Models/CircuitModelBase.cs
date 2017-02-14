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
    public class DestinationViewModel
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InterestViewModel
    {
        public string Name { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class ActivityViewModel
    {
        public string Name { get; set; }
    }
}