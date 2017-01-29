using DomingoBL.BlObjects;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// 
    /// </summary>
    //public class TripModel
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public List<Trip> AllTrips { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Trip ActiveTrip { get; set; }
    //}    
    public class TripPlanningViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BlViewTrip> AllTrips { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BlViewTrip> PlannedTrips { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public BlViewTrip ActiveTrip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Destination> SuggestedDestinations { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateTripViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "StartDate")]
        public DateTime StartDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TripViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BlViewTrip> AllTrips { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BlViewTrip ActiveTrip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BlTripTemplate> AllTemplates { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public BlTripTemplate CreateTripTemplate { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public CreateTripViewModel CreateTripViewModel { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditTripViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public BlViewTrip ActiveTrip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BlTripTemplate> RelatedTemplates { get; set; }
    }
}