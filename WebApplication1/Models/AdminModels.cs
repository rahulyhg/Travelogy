using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class AdminModel
    {
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
        public TripBookingAccommodation DbObject { get; set; }
    }
}