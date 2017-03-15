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
    public class RemoveDestinationFromTripViewModel
    {
        public View_Trip DlTrip { get; set; }

        public View_TripStep DlTripStep { get; set; }

        public List<View_TripBookingAccommodation> DlBookingsView { get; set; }
        
        public List<View_TripBookingTransport> DlTransportsBookingsView { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AccommodationBookingViewModel
    {
        public string BookingStatus { get; set; }

        public int TripStepId { get; set; }

        public int TripId { get; set; }

        public string TripName { get; set; }

        public string TripDescription { get; set; }

        public string TripStepName { get; set; }

        public string TripStepDescription { get; set; }

        public string PropertyName { get; set; }

        public string PropertyAddress { get; set; }

        public DateTime? TripStepStartDate {get; set; }

        public DateTime? TripStepEndDate { get; set; }

        public string AdminNotes { get; set; }

        [Required(ErrorMessage = "Please select a type of Accommodation")]
        [Display(Name = "Type of Accommodation")]
        public string AccommodationType { get; set; }

        [Display(Name = "Check in Date")]
        [Required]
        public DateTime CheckinDate { get; set; }

        [Display(Name = "Check out Date")]
        [Required]
        public DateTime CheckoutDate { get; set; }

        [Required]
        [Display(Name = "Town or City")]
        public string TownOrCity { get; set; }

        [Display(Name = "Adults")]
        [Required(ErrorMessage = "Please select at least one adult")]
        public int Adults { get; set; }

        [Display(Name = "Adults")]
        public int Kids { get; set; }

        [Display(Name = "Your message")]
        public string TravellerNotes { get; set; }

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


    public class LocalTransferViewModel
    {
        public int TripBookingTransportId { get; set; }

        public int TripStepId { get; set; }

        public int TripId { get; set; }

        public string TripName { get; set; }

        public string TripDescription { get; set; }

        public string TripStepName { get; set; }

        public string TripStepDescription { get; set; }        

        public DateTime? TripStepDate { get; set; }

        public DateTime? TripStepStartDate { get; set; }

        public DateTime? TripStepEndDate { get; set; }

        [Display(Name = "Preferred Date")]
        [Required]
        public DateTime TransferDate { get; set; }

        [Display(Name = "From:")]
        [Required]
        public string From { get; set; }

        [Display(Name = "To:")]
        [Required]
        public string To { get; set; }

        [Display(Name = "Your message")]
        public string TravellerNotes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ListOfTransferClasses
        {
            get
            {
                var dropdownItems = new List<System.Web.Mvc.SelectListItem>();
                dropdownItems.AddRange(new[]{
                            new System.Web.Mvc.SelectListItem() { Text = "--- please select one ---", Value = "" },
                            new System.Web.Mvc.SelectListItem() { Text = "Economy", Value = "Economy" },
                            new System.Web.Mvc.SelectListItem() { Text = "Business", Value = "Business" },
                            new System.Web.Mvc.SelectListItem() { Text = "First", Value = "First" }});

                return dropdownItems;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FlightBookingViewModel
    {
        public int TripBookingTransportId { get; set; }

        public int TripStepId { get; set; }

        public int TripId { get; set; }

        public string TripName { get; set; }

        public string TripDescription { get; set; }

        public string TripStepName { get; set; }

        public string TransportType { get; set; }

        public string TripStepDescription { get; set; }

        public DateTime? TripStartDate { get; set; }

        public DateTime? TripStepStartDate { get; set; }

        public DateTime? TripStepEndDate { get; set; }

        public string BookingStatus { get; set; }

        public string TransferDetails { get; set; }

        public string AdminNotes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ListOfFlightClasses
        {
            get
            {
                var dropdownItems = new List<System.Web.Mvc.SelectListItem>();
                dropdownItems.AddRange(new[]{
                            new System.Web.Mvc.SelectListItem() { Text = "--- please select one ---", Value = "" },
                            new System.Web.Mvc.SelectListItem() { Text = "Flight - Economy", Value = "Economy" },
                            new System.Web.Mvc.SelectListItem() { Text = "Flight - Business", Value = "Business" },
                            new System.Web.Mvc.SelectListItem() { Text = "Flight - First", Value = "First" },
                            new System.Web.Mvc.SelectListItem() { Text = "Train - Airconditioned", Value = "Train - AC" },
                            new System.Web.Mvc.SelectListItem() { Text = "Train - Non Airconditioned", Value = "Train - Non-AC" },
                            new System.Web.Mvc.SelectListItem() { Text = "Bus or Coach", Value = "Bus" },
                            new System.Web.Mvc.SelectListItem() { Text = "Taxi or Car", Value = "Taxi" }
                });

                return dropdownItems;
            }
        }

        [Display(Name = "Preferred Date:")]
        [Required]
        public DateTime FlightDate { get; set; }

        [Display(Name = "From:")]
        [Required]
        public string From { get; set; }

        [Display(Name = "To:")]
        [Required]
        public string To { get; set; }

        [Display(Name = "Travel Class:")]
        [Required]
        public string FlightClass { get; set; }

        [Display(Name = "Your message:")]
        public string TravellerNotes { get; set; }

        [Display(Name = "No of Adults:")]
        [Required]
        public int Adults { get; set; }

        [Display(Name = "No of Children:")]
        [Required]
        public int Kids { get; set; }
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
        [Display(Name = "Trip Start Date")]
        [Required(ErrorMessage = "Please specify a date (approximate will do!)")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "A nick name for your Trip:")]
        [Required(ErrorMessage = "Please give a name to your Trip!")]
        public string NickName { get; set; }

        [Display(Name = "Choose where you want to fly out from:")]
        [Required(ErrorMessage = "Please state where you plan to start from.")]
        public string HomeLocation { get; set; }

        [Display(Name = "Adults:")]
        [Required(ErrorMessage = "Please mention the number of adults.")]
        [Range(1, 15, ErrorMessage = "Please add one or more adults .. upto 15")]
        public int Adults { get; set; }

        [Display(Name = "Minors:")]        
        public int Minors { get; set; }

        [Display(Name = "Pick a starting point for the Trip:")]
        [Required(ErrorMessage = "Please pick a starting point for the Trip.")]
        public string StartLocation { get; set; }

        [Display(Name = "Choose type of trip:")]
        [Required(ErrorMessage = "Please pick a type for the Trip.")]
        public string TripType { get; set; }

        [Display(Name = "Currency:")]
        public string Currency { get; set; }        

    }

    /// <summary>
    /// 
    /// </summary>
    public class TripViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ImmediateTripId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }

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
        public IEnumerable<System.Web.Mvc.SelectListItem> CreateTripStartLocationOptions { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<System.Web.Mvc.SelectListItem> TripStartLocationOptions { get; set; }
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

    public class ViewTripTemplateViewModel
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