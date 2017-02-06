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
    public class DomingoModelBase
    {
        public string PageName { get; set; }

        public Destination Destination { get; set; }

        public List<View_TagDestination> DestinationTags { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DomingoDateViewModel
    {
        public DateTime DateValue { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ListOfMonths { get; set; }
    }
}