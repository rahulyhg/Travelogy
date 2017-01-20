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
        public List<Trip> AllTrips { get; set; }

        public Trip ActiveTrip { get; set; }
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
        public List<Trip> AllTrips { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Trip ActiveTrip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TripTemplate> AllTemplates { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public TripTemplate CreateTripTemplate { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public CreateTripViewModel CreateTripViewModel { get; set; }
    }
}