using DomingoBL.BlObjects;
using DomingoDAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{

    /// <summary>
    /// 
    /// </summary>
    public enum TripStatus
    {
        planned,

        consulting,

        booked,

        archived
    }

    public enum AccommodationBookingStatus
    {
        requested,

        booked,

        modified,

        cancelled
    }

    public enum TransferBookingStatus
    {
        requested,

        booked,

        modified,

        cancelled
    }

    /// <summary>
    /// 
    /// </summary>
    public class TripManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspUserId"></param>
        /// <returns></returns>
        public static BlViewTrip GetImmediateTripForUser(string aspUserId)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var dlTrip = context.View_Trip.Where(p => p.AspNetUserId == aspUserId).Where(p => p.StartDate > DateTime.Now).OrderBy(p => p.StartDate).First();
                    if(dlTrip != null)
                    {
                        var trip = new BlViewTrip() { DlTripView = dlTrip };
                        return trip;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspUserId"></param>
        /// <param name="trips"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllTripsForUser(string aspUserId, out List<BlViewTrip> trips)
        {
            trips = new List<BlViewTrip>();

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // get all trips for the user
                    var dlTrips = context.View_Trip.Where(p => p.AspNetUserId == aspUserId).ToList();
                    foreach(var dlTrip in dlTrips)
                    {
                        // get all the steps and bookings and add to the BL object
                        var dlTripSteps = context.View_TripStep.Where(p => p.TripId == dlTrip.Id);
                        var dlTripBookings = context.View_TripBookingAccommodation.Where(p => p.TripId == dlTrip.Id);
                        var dlTransferBookings = context.View_TripBookingTransport.Where(p => p.TripId == dlTrip.Id);
                        
                        var blTrip = new BlViewTrip() { DlTripView = dlTrip }; 
                        blTrip.DlTripStepsView = (dlTripSteps != null) ? dlTripSteps.ToList() : null;
                        blTrip.DlBookingsView = (dlTripBookings != null) ? dlTripBookings.ToList() : null;
                        blTrip.DlTransportsBookingsView = (dlTransferBookings != null) ? dlTransferBookings.ToList() : null;

                        trips.Add(blTrip);
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspUserId"></param>
        /// <param name="trips"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllTripsForUserByName(string aspUserName, out List<BlViewTrip> trips)
        {
            trips = new List<BlViewTrip>();

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var aspNetUser = context.AspNetUsers.Where(p => p.UserName == aspUserName).FirstOrDefault();
                    var dlTrips = context.View_Trip.Where(p => p.AspNetUserId == aspNetUser.Id).ToList();
                    foreach (var dlTrip in dlTrips)
                    {
                        var dlTripSteps = context.View_TripStep.Where(p => p.TripId == dlTrip.Id);
                        var blTrip = new BlViewTrip() { DlTripView = dlTrip, DlTripStepsView = dlTripSteps.ToList() };
                        trips.Add(blTrip);
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateTrip(Trip trip, int tripTemplateId)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // if the trip object is for a new trip
                    // simply add it to the database table and save it
                    if (trip.Id == 0)
                    {
                        var template = context.TripTemplates.Find(tripTemplateId);
                        trip.Templates = tripTemplateId.ToString();
                        trip.TemplateSearchAlias = template.SearchAlias;
                        trip.DestinationId = template.DestinationId;
                        if (String.IsNullOrEmpty(trip.TripCurrency)) { trip.TripCurrency = "GBP"; }
                        if (String.IsNullOrEmpty(trip.StartLocation)) { trip.StartLocation = template.StartLocation; }

                        context.Trips.Add(trip);
                        await context.SaveChangesAsync();

                        // get the steps from the template
                        foreach (var tripStep in _CopyTripStepsFromTemplate(tripTemplateId, trip.Id, trip.StartDate, context))
                        {
                            context.TripSteps.Add(tripStep);
                        }

                        await context.SaveChangesAsync();
                    }

                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        private static List<TripStep> _CopyTripStepsFromTemplate(int tripTemplateId, int tripId, DateTime? startDate, TravelogyDevEntities1 context)
        {
            var tripStartDate = DateTime.MinValue;
            if(startDate.HasValue && startDate.Value > DateTime.MinValue)
            {
                tripStartDate = startDate.Value;
            }

            var tripSteps = new List<TripStep>();
            var templateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == tripTemplateId).OrderBy(p => p.SortOrder);
            foreach (var templateStep in templateSteps)
            {
                var tripStep = new TripStep()
                {
                    TripId = tripId,
                    TripTemplateStepId = templateStep.Id,
                    ShortDescription = templateStep.ShortDescription,
                    LongDescription = templateStep.LongDescription,
                    NightStay = templateStep.NightStay,
                    Destination = templateStep.Destination,
                };

                if (tripStartDate > DateTime.MinValue)
                {
                    tripStep.StartDate = tripStartDate;
                    if(templateStep.TypicalDurationDays.HasValue)
                    {
                        tripStep.EndDate = tripStartDate.AddDays(templateStep.TypicalDurationDays.Value);
                    }
                }

                tripSteps.Add(tripStep);

                // recalculate tripStartDate
                if(tripStartDate > DateTime.MinValue && templateStep.TypicalDurationDays.HasValue)
                {
                    tripStartDate = tripStartDate.AddDays(templateStep.TypicalDurationDays.Value);
                }
                
            }
            return tripSteps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="tripTemplateId"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> AddTemplateToTripAsync(View_Trip trip, int tripTemplateId)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // if the trip object is for a new trip
                    // simply add it to the database table and save it
                    if (trip.Id > 0)
                    {
                        // update the Templates field of the trip
                        var _dbTrip = context.Trips.Find(trip.Id);
                        if(_dbTrip.Templates.Contains(tripTemplateId.ToString()))
                        {
                            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Duplicate Template" };
                        }

                        _dbTrip.Templates = string.Format("{0};{1}", _dbTrip.Templates, tripTemplateId);

                        DateTime? dtDate = _dbTrip.EndDate.HasValue? _dbTrip.EndDate : _dbTrip.StartDate ;

                        // get the steps from the template and add them to this trip
                        foreach (var tripStep in _CopyTripStepsFromTemplate(tripTemplateId, _dbTrip.Id, dtDate, context))
                        {
                            context.TripSteps.Add(tripStep);
                            if (tripStep.EndDate.HasValue)
                            {
                                _dbTrip.EndDate = tripStep.EndDate;
                            }
                        }

                        await context.SaveChangesAsync();
                    }

                }
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> StartTripBookingAsync(int tripId)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var trip = context.Trips.Find(tripId);
                    if(trip == null)
                    {
                        return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Invalid Parameter : tripId" };
                    }

                    trip.Status = TripStatus.consulting.ToString();
                    await context.SaveChangesAsync();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// saves user's changes to the trip
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="tripSteps"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveUserTripChangesAsync(View_Trip trip, List<View_TripStep> tripSteps)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _dbTrip = context.Trips.Find(trip.Id);
                    if (_dbTrip != null)
                    {
                        if (trip.StartDate.HasValue)
                        {
                            _dbTrip.StartDate = trip.StartDate;
                        }

                        _dbTrip.StartLocation = trip.StartLocation;
                        _dbTrip.TripCurrency = trip.TripCurrency;
                        _dbTrip.TripType = trip.TripType;
                        _dbTrip.PaxAdults = trip.PaxAdults;
                        _dbTrip.PaxMinors = trip.PaxMinors;
                    }

                    if (tripSteps != null)
                    {
                        // save user notes and dates
                        _UpdateTripSteps(tripSteps, context, _dbTrip);
                    }

                    _dbTrip.EstimatedCost = _CalculateTripCost(tripSteps, context, _dbTrip);

                    await context.SaveChangesAsync();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripSteps"></param>
        /// <param name="context"></param>
        /// <param name="_dbTrip"></param>
        /// <returns></returns>
        private static decimal _CalculateTripCost(List<View_TripStep> tripSteps, TravelogyDevEntities1 context, Trip _dbTrip)
        {
            var costs = context.View_TripStepCost.Where(p => p.TripId == _dbTrip.Id);
            decimal _totalCost = 0;
            double _conversionFactor = 1;

            // for each step - 
            foreach(var tripStep in tripSteps)
            {
                // get the food, transport and acco costs 
                // factor in the number of pax - food - Minors 50% 
                var foodCost = costs.Where(p => p.TripStepId == tripStep.Id).Average(r => r.FOOD_COST);
                foodCost = foodCost * _dbTrip.PaxAdults + (_dbTrip.PaxMinors.HasValue ?  foodCost * _dbTrip.PaxMinors.Value / 2 : 0);

                // factor the currency
                var _foodCurrency = costs.Where(p => p.TripStepId == tripStep.Id).FirstOrDefault().FOOD_CURRENCY;
                var blError = CurrencyConvertGateway.GetCurrencyExchangeRate(_foodCurrency, _dbTrip.TripCurrency, out _conversionFactor);
                if(blError.ErrorCode == 0 && _conversionFactor != 0 && _conversionFactor != 1)
                {
                    foodCost = foodCost * (decimal)_conversionFactor;
                }

                // transport - Minors = Adults
                var transportCost = costs.Where(p => p.TripStepId == tripStep.Id).Average(r => r.TRANSPORT_COST);
                transportCost = transportCost * _dbTrip.PaxAdults + (_dbTrip.PaxMinors.HasValue ? transportCost * _dbTrip.PaxMinors.Value : 0);

                // factor the currency
                var _transportCurrency = costs.Where(p => p.TripStepId == tripStep.Id).FirstOrDefault().TRANSPORT_CURRENCY;
                if(string.Compare(_transportCurrency, _foodCurrency, true) != 0)
                {
                    blError = CurrencyConvertGateway.GetCurrencyExchangeRate(_transportCurrency, _dbTrip.TripCurrency, out _conversionFactor);
                }                

                transportCost = transportCost * (decimal)_conversionFactor;

                // acco - Minors free
                var accoCost = costs.Where(p => p.TripStepId == tripStep.Id).Average(r => r.ACCO_COST);
                accoCost = accoCost * _dbTrip.PaxAdults;

                var _accoCurrency = costs.Where(p => p.TripStepId == tripStep.Id).FirstOrDefault().ACCO_CURRENCY;
                if (string.Compare(_accoCurrency, _transportCurrency, true) != 0)
                {
                    blError = CurrencyConvertGateway.GetCurrencyExchangeRate(_accoCurrency, _dbTrip.TripCurrency, out _conversionFactor);
                }

                accoCost = accoCost * (decimal)_conversionFactor;

                // get actual days
                var stepDays = costs.Where(p => p.TripStepId == tripStep.Id).Average(r => r.ActualDays);

                // if not found, get the usual days for the template step
                if (stepDays == 0 || stepDays == null)
                {
                    stepDays = costs.Where(p => p.TripStepId == tripStep.Id).Average(r => r.TypicalDurationDays);
                    if (stepDays == null) { stepDays = 1; } // none found, default to 1
                }

                // add food + trans + food multiply by days
                _totalCost += ((foodCost + accoCost + transportCost) * (decimal)stepDays) ;
            }

            return _totalCost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripSteps"></param>
        /// <param name="context"></param>
        /// <param name="_dbTrip"></param>
        private static void _UpdateTripSteps(List<View_TripStep> tripSteps, TravelogyDevEntities1 context, Trip _dbTrip)
        {
            foreach (var tripStep in tripSteps)
            {
                var _dbTripStep = context.TripSteps.Find(tripStep.Id);
                if (_dbTripStep != null)
                {
                    // assign the notes
                    _dbTripStep.TravellerNote = tripStep.TravellerNote;

                    // if the start dates has been edited
                    if (tripStep.StartDate.HasValue && _dbTripStep.StartDate != tripStep.StartDate)
                    {
                        _dbTripStep.StartDate = tripStep.StartDate;
                    }

                    // if the end dates has been edited
                    if (tripStep.EndDate.HasValue && _dbTripStep.EndDate != tripStep.EndDate)
                    {
                        _dbTripStep.EndDate = tripStep.EndDate;
                        _dbTrip.EndDate = tripStep.EndDate;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="accommodation"></param>
        /// <returns></returns>
        public static DomingoBlError GetTripBookingAccommodation(int bookingId, out TripBookingAccommodation accommodation)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    accommodation = context.TripBookingAccommodations.Find(bookingId);
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                accommodation = null;
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static DomingoBlError GetTripBookingTransport(int bookingId, out View_TripBookingTransport transfer)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    transfer = context.View_TripBookingTransport.Where(p => p.Id == bookingId).FirstOrDefault();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                transfer = null;
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripBookingTransportAsync(TripBookingTransport booking)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    if (booking.Id == 0)
                    {
                        context.TripBookingTransports.Add(booking);
                        await context.SaveChangesAsync();
                    }

                    else
                    {
                        var _dbBooking = context.TripBookingTransports.Find(booking.Id);
                        if (_dbBooking != null)
                        {
                            _dbBooking.AdminNotes = booking.AdminNotes;
                            _dbBooking.Adults = booking.Adults;
                            if (booking.BookingDate.HasValue)
                            {
                                _dbBooking.BookingDate = booking.BookingDate;
                            }
                            _dbBooking.BookingStatus = booking.BookingStatus;
                            if(booking.EstimatedCost > 0)
                            {
                                _dbBooking.EstimatedCost = booking.EstimatedCost;
                            }

                            _dbBooking.Kids = booking.Kids;
                            _dbBooking.TransportFrom = booking.TransportFrom;
                            _dbBooking.TransportTo = booking.TransportTo;
                            _dbBooking.TransportType = booking.TransportType;
                            _dbBooking.TravelClass = booking.TravelClass;
                            _dbBooking.TravellerNotes = booking.TravellerNotes;

                            await context.SaveChangesAsync();
                        }
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accommodation"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripBookingAccommodationAsync(TripBookingAccommodation accommodation)
        {
            try
            {
                if(accommodation.CheckoutDate < accommodation.CheckinDate)
                {
                    return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Check out date cannot be earlier than the Check in date." };
                }

                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    if (accommodation.Id == 0)
                    {
                        context.TripBookingAccommodations.Add(accommodation);
                        await context.SaveChangesAsync();
                    }

                    else
                    {
                        var _dbAccommodation = context.TripBookingAccommodations.Find(accommodation.Id);
                        if(_dbAccommodation != null)
                        {
                            // save 
                            _dbAccommodation.AccommodationType = accommodation.AccommodationType;
                            _dbAccommodation.AdminNotes = accommodation.AdminNotes;
                            _dbAccommodation.Adults = accommodation.Adults;

                            if(accommodation.CheckinDate.HasValue)
                            {
                                _dbAccommodation.CheckinDate = accommodation.CheckinDate;
                            }

                            if(accommodation.CheckoutDate.HasValue)
                            {
                                _dbAccommodation.CheckoutDate = accommodation.CheckoutDate;
                            }

                            _dbAccommodation.EstimatedCost = accommodation.EstimatedCost;
                            _dbAccommodation.Kids = accommodation.Kids;
                            _dbAccommodation.PropertyAddress = accommodation.PropertyAddress;
                            _dbAccommodation.PropertyName = accommodation.PropertyName;
                            _dbAccommodation.SpecialRequests = accommodation.SpecialRequests;
                            _dbAccommodation.Status = accommodation.Status;
                            _dbAccommodation.TownOrCity = accommodation.TownOrCity;
                            _dbAccommodation.TravellerNotes = accommodation.TravellerNotes;

                            await context.SaveChangesAsync();
                        }
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="tripSteps"></param>
        /// <returns></returns>
        public static DomingoBlError SaveAdminTripChanges(View_Trip trip, List<View_TripStep> tripSteps)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _dbTrip = context.Trips.Find(trip.Id);
                    if (_dbTrip != null)
                    {
                        _dbTrip.StartLocation = trip.StartLocation;
                    }

                    // save user notes
                    foreach (var tripStep in tripSteps)
                    {
                        var _dbTripStep = context.TripSteps.Find(tripStep.Id);
                        if (_dbTripStep != null)
                        {
                            _dbTripStep.TravelogerNote = tripStep.TravelogerNote;
                        }
                    }

                    context.SaveChanges();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static DomingoBlError SearchTripTemplatesByAlias(string alias, out List<BlTripTemplate> templates)
        {
            try
            {
                templates = new List<BlTripTemplate>();

                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // search for trmplates with the search alias
                    var dbTemplateSearchRes = context.TripTemplates.Where(p => p.SearchAlias == alias);                    

                    // iterate through the results and populate the business layer object
                    foreach(var dbTemplate in dbTemplateSearchRes)
                    {
                        // add the template and then the steps
                        var _blTripTemplateObj = new BlTripTemplate() { DlTemplate = dbTemplate };
                        _blTripTemplateObj.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == dbTemplate.Id).ToList();

                        var _destination = context.Destinations.Where(dest => dest.Id == _blTripTemplateObj.DlTemplate.DestinationId).FirstOrDefault();
                        if(_destination != null)
                        {
                            _blTripTemplateObj.Country = _destination.Country;
                        }                       

                        templates.Add(_blTripTemplateObj);
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                templates = null;
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        public static DomingoBlError SearchRelatedTripTemplates(int tripId, out List<BlTripTemplate> templates)
        {
            templates = new List<BlTripTemplate> ();

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var innerJoinQuery =
                        from trip in context.Trips
                        join dest in context.Destinations on trip.DestinationId equals dest.Id
                        where trip.Id == tripId
                        select new { Country = dest.Country, TripId = trip.Id };

                    var country = innerJoinQuery.FirstOrDefault().Country;

                    if(string.IsNullOrEmpty(country))
                    {
                        return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Invalid parameter: tripId" };
                    }

                    var destinationTemplateQuery =
                        from template in context.TripTemplates
                        join dest in context.Destinations on template.DestinationId equals dest.Id
                        where dest.Country == country
                        select template;

                    foreach(var tripTemplate in destinationTemplateQuery)
                    {
                        var _blTripTemplateObj = new BlTripTemplate() { DlTemplate = tripTemplate };
                        _blTripTemplateObj.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == tripTemplate.Id).ToList();

                        templates.Add(_blTripTemplateObj);
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                templates = null;
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        public static DomingoBlError SearchTripTemplatesByCountry(string country, out List<BlTripTemplate> templates)
        {
            try
            {
                templates = new List<BlTripTemplate>();

                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var destinationTemplates = context.Destinations.Where(destination => destination.Country == country)
                        .Join(context.TripTemplates, destination => destination.Id, template => template.DestinationId, (destination, template) => template);

                    var templateDestinations = context.TripTemplates.Join(context.Destinations,
                                 template => template.DestinationId,
                                 destinaiton => destinaiton.Id,
                                 (template, destinaiton) => template);

                    // iterate through the results and populate the business layer object
                    foreach (var dbTemplate in templateDestinations)
                    {
                        // add the template and then the steps
                        var _blTripTemplateObj = new BlTripTemplate() { DlTemplate = dbTemplate };
                        _blTripTemplateObj.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == dbTemplate.Id).ToList();

                        templates.Add(_blTripTemplateObj);
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                templates = null;
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static DomingoBlError GetDefaultTemplateForCountry(string country, out BlTripTemplate template)
        {
            template = new BlTripTemplate();
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    string alias = "default:" + country;
                    // add the template db obj to the bl object
                    template.DlTemplate = context.TripTemplates.Where(p => p.SearchAlias == alias).FirstOrDefault();
                    int templateId = template.DlTemplate.Id;

                    // add the template steps
                    template.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == templateId).ToList();
                }
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static DomingoBlError GetTripTemplatesById(int templateId, out BlTripTemplate template)
        {
            template = new BlTripTemplate();
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // add the template db obj to the bl object
                    template.DlTemplate = context.TripTemplates.Where(p => p.Id == templateId).FirstOrDefault();                    

                    // add the template steps
                    template.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == templateId).ToList();                    
                }
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="viewTrip"></param>
        /// <returns></returns>
        public static DomingoBlError GetTripById(int tripId, out BlViewTrip viewTrip)
        {
            viewTrip = new BlViewTrip();
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var dlTripSteps = context.View_TripStep.Where(p => p.TripId == tripId);
                    viewTrip.DlTripView = context.View_Trip.Where(p => p.Id == tripId).FirstOrDefault();
                    viewTrip.DlTripStepsView = dlTripSteps.ToList();
                    viewTrip.DlBookingsView = context.View_TripBookingAccommodation.Where(p => p.TripId == tripId).ToList();
                    viewTrip.DlTransportsBookingsView = context.View_TripBookingTransport.Where(p => p.TripId == tripId).ToList();
                }
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripStepId"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static DomingoBlError Admin_GetTripStepById(int tripStepId, out View_TripStep step)
        {
            step = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    step = context.View_TripStep.Where(p => p.Id == tripStepId).FirstOrDefault();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="trip"></param>
        /// <returns></returns>
        public static DomingoBlError Admin_GetTripById(int tripId, out BlTrip trip)
        {
            trip = new BlTrip();
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var dlTripSteps = context.TripSteps.Where(p => p.TripId == tripId);
                    trip.DlTrip = context.Trips.Find(tripId);
                    trip.DlTripSteps = dlTripSteps.ToList();
                    trip.DlBookingsView = context.View_TripBookingAccommodation.Where(p => p.TripId == tripId).ToList();
                    trip.DlTransportsBookingsView = context.View_TripBookingTransport.Where(p => p.TripId == tripId).ToList();

                    // get the traveller info
                    var AspNetUserId = trip.DlTrip.AspNetUserId;
                    trip.DlTraveller = context.View_Traveller.Where(p => p.Id == AspNetUserId).FirstOrDefault();
                }


                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripProviders"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllTripProviders(out List<TripProvider> tripProviders)
        {
            tripProviders = null;

            try
            {
                // get all desitions, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _dbTripProviders = context.TripProviders.Select(p => p);
                    if (_dbTripProviders != null)
                    {
                        tripProviders = _dbTripProviders.ToList();
                    }
                }
            }

            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }
    }
}
