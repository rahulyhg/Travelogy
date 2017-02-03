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
    public class AccommodationBookingViewModel
    {
        public int TripStepId { get; set; }

        public int TripId { get; set; }

        [Required(ErrorMessage = "Please select a type of Accommodation")]
        [Display(Name = "Type of Accommodation")]
        public string AccommodationType { get; set; }

        [Display(Name = "Check in Date")]
        [Required]
        public DateTime CheckinDate { get; set; }

        [Display(Name = "Check out Date")]
        [Required]
        public DateTime CheckoutDate { get; set; }

        [Display(Name = "Your message")]
        public string Notes { get; set; }

        [Display(Name = "Special Requests")]
        public string SpecialRequests { get; set; }        

        public IEnumerable<System.Web.Mvc.SelectListItem> ListOfAccommodationTypes
        {
            get
            {
                var dropdownItems = new List<System.Web.Mvc.SelectListItem>();
                dropdownItems.AddRange(new[]{
                            new System.Web.Mvc.SelectListItem() { Text = "--- please select one ---", Value = "" },
                            new System.Web.Mvc.SelectListItem() { Text = "Hostel Private room", Value = "Hostel Private room" },
                            new System.Web.Mvc.SelectListItem() { Text = "Hostel Bunk Bed", Value = "Hostel Bunk Bed" },
                            new System.Web.Mvc.SelectListItem() { Text = "Hotel - Budget", Value = "Hotel - Budget" },
                            new System.Web.Mvc.SelectListItem() { Text = "Hotel - Mid Range", Value = "Hotel - Mid Range" },
                            new System.Web.Mvc.SelectListItem() { Text = "Hotel - Luxury", Value = "Hotel - Luxury" },
                            new System.Web.Mvc.SelectListItem() { Text = "Other - please specify", Value = "Other" }});

                return dropdownItems;
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
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
        public int DestinationId { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public List<BlTripTemplate> RelatedTemplates { get; set; }
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

    /// <summary>
    /// 
    /// </summary>
    public class TripTemplateWidgetViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public BlTripTemplate TripTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TripId { get; set; }
    }
}