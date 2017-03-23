using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetTripProvidersSelectList()
        {
            var _dbTripProviders = new List<TripProvider>();
            var blError = TripManager.GetAllTripProviders(out _dbTripProviders);
            var _tripProviders = _dbTripProviders.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });

            return new SelectList(_tripProviders, "Value", "Text");
        }

        public static IEnumerable<SelectListItem> GetDestinationsSelectList()
        {
            var _dbDestinations = new List<Destination>();
            var blError = DestinationManager.GetAllDestinations(out _dbDestinations);
            var _destinations = _dbDestinations.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });

            return new SelectList(_destinations, "Value", "Text");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdminEditTripTemplateViewModel
    {
        public TripTemplate TripTemplate { get; set; }

        public IEnumerable<SelectListItem> DestinationList { get; set; }

        public IEnumerable<SelectListItem> TripProviderList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdminTripBookingAccommodationEditModel
    {
        public View_TripBookingAccommodation DbObject { get; set; }

        public int TripId { get; set; }

        public IEnumerable<SelectListItem> BookingStatusList
        {
            get
            {
                var dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                        new SelectListItem() { Text = "--- please select one ---", Value = "" },
                            new SelectListItem() { Text = AccommodationBookingStatus.requested.ToString(), Value = AccommodationBookingStatus.requested.ToString() },
                            new SelectListItem() { Text = AccommodationBookingStatus.booked.ToString(), Value = AccommodationBookingStatus.booked.ToString() },
                            new SelectListItem() { Text = AccommodationBookingStatus.modified.ToString(), Value = AccommodationBookingStatus.modified.ToString() },
                            new SelectListItem() { Text = AccommodationBookingStatus.cancelled.ToString(), Value = AccommodationBookingStatus.cancelled.ToString()}});

                return dropdownItems;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdminTripBookingTransportEditModel
    {
        public View_TripBookingTransport DbObject { get; set; }

        public int TripId { get; set; }

        public IEnumerable<SelectListItem> BookingStatusList
        {
            get
            {
                var dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                        new SelectListItem() { Text = "--- please select one ---", Value = "" },
                            new SelectListItem() { Text = AccommodationBookingStatus.requested.ToString(), Value = AccommodationBookingStatus.requested.ToString() },
                            new SelectListItem() { Text = AccommodationBookingStatus.booked.ToString(), Value = AccommodationBookingStatus.booked.ToString() },
                            new SelectListItem() { Text = AccommodationBookingStatus.modified.ToString(), Value = AccommodationBookingStatus.modified.ToString() },
                            new SelectListItem() { Text = AccommodationBookingStatus.cancelled.ToString(), Value = AccommodationBookingStatus.cancelled.ToString()}});

                return dropdownItems;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditDestinationViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Destination DbObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SubDestination> SubDestinations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DestinationActivity> Activities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DestinationInterest> Interests { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DestinationCost> CostObjects { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NewMemberViewModel
    {
        [Required(ErrorMessage = "Please enter First Name.")]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name.")]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Email.")]
        [Display(Name = "Email:")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(14, MinimumLength = 7, ErrorMessage = "This does not look like a valid number! ")]
        [Display(Name = "Telephone Number:")]
        [Required(ErrorMessage = "Please enter your Telephone or Mobile number.")]
        //[RegularExpression(@"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)", ErrorMessage = "Please enter a valid Telephone number")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Please enter Notes.")]
        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}