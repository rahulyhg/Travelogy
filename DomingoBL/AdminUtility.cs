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
    public static class AdminUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripTemplate(TripTemplate template)
        {
            try
            {
                if (template != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        if (template.Id > 0)
                        {
                            var _tripTemmplate = context.TripTemplates.Find(template.Id);
                            _tripTemmplate.SearchAlias = template.SearchAlias;
                            _tripTemmplate.DestinationId = template.DestinationId;
                            _tripTemmplate.StartLocation = template.StartLocation;
                            _tripTemmplate.Weightage = template.Weightage;
                            _tripTemmplate.DurationDaysMin = template.DurationDaysMin;
                            _tripTemmplate.DurationDaysMax = template.DurationDaysMax;
                            _tripTemmplate.BestTimeToVisit = template.BestTimeToVisit;
                            _tripTemmplate.Name = template.Name;
                            _tripTemmplate.Description = template.Description;
                            _tripTemmplate.ThumbnailPath = template.ThumbnailPath;
                            _tripTemmplate.TripProviderId = template.TripProviderId;

                            await context.SaveChangesAsync();
                        }

                        else
                        {
                            context.TripTemplates.Add(template);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateStep"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripTemplateStep(TripTemplateStep templateStep)
        {
            try
            {
                if (templateStep != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        var _tripTemmplateStep = context.TripTemplateSteps.Where(p => p.TripTemplateId == templateStep.TripTemplateId
                                && p.TripTemplateStepIdentifier == templateStep.TripTemplateStepIdentifier).FirstOrDefault();

                        if (_tripTemmplateStep != null)
                        {

                            _tripTemmplateStep.ShortDescription = templateStep.ShortDescription;
                            _tripTemmplateStep.LongDescription = templateStep.LongDescription;
                            _tripTemmplateStep.NightStay = templateStep.NightStay;
                            _tripTemmplateStep.ThumbnailPath = templateStep.ThumbnailPath;
                            _tripTemmplateStep.Destination = templateStep.Destination;
                            _tripTemmplateStep.TypicalDurationDays = templateStep.TypicalDurationDays;

                            await context.SaveChangesAsync();
                        }

                        if (string.IsNullOrEmpty(templateStep.TripTemplateStepIdentifier))
                        {
                            _tripTemmplateStep = new TripTemplateStep()
                            {
                                TripTemplateId = templateStep.TripTemplateId,
                                ShortDescription = templateStep.ShortDescription,
                                LongDescription = templateStep.LongDescription,
                                NightStay = templateStep.NightStay,
                                ThumbnailPath = templateStep.ThumbnailPath,
                                Destination = templateStep.Destination,
                                TypicalDurationDays = templateStep.TypicalDurationDays,
                            };

                            context.TripTemplateSteps.Add(_tripTemmplateStep);
                            await context.SaveChangesAsync();

                            _tripTemmplateStep.TripTemplateStepIdentifier = string.Format("{0}-{1}", _tripTemmplateStep.TripTemplateId, _tripTemmplateStep.Id);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveDestination(Destination model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        if (model.Id == 0)
                        {
                            context.Destinations.Add(model);
                            await context.SaveChangesAsync();
                        }

                        else
                        {
                            var _dbDestinationObj = context.Destinations.Find(model.Id);
                            if (_dbDestinationObj != null)
                            {
                                _dbDestinationObj.Alias = model.Alias;
                                _dbDestinationObj.BestTimeToVisit = model.BestTimeToVisit;
                                _dbDestinationObj.Country = model.Country;
                                _dbDestinationObj.ShortDescription = model.ShortDescription;
                                _dbDestinationObj.CircuitUrl = model.CircuitUrl;
                                _dbDestinationObj.Description = model.Description;
                                _dbDestinationObj.Name = model.Name;
                                _dbDestinationObj.Tagline = model.Tagline;
                                _dbDestinationObj.TemplateSearchAlias = model.TemplateSearchAlias;
                                _dbDestinationObj.ThumbnailPath = model.ThumbnailPath;
                                _dbDestinationObj.TourContinent = model.TourContinent;
                                _dbDestinationObj.TravelStyles = model.TravelStyles;
                                _dbDestinationObj.Weightage = model.Weightage;

                                await context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripProvider(TripProvider model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        if (model.Id == 0)
                        {
                            context.TripProviders.Add(model);
                            context.SaveChanges();
                        }

                        else
                        {
                            var _tripProvider = context.TripProviders.Find(model.Id);
                            _tripProvider.Address = model.Address;
                            _tripProvider.Description = model.Description;
                            _tripProvider.EmailAddressCustSupport = model.EmailAddressCustSupport;
                            _tripProvider.EmailAddressMarketingSales = model.EmailAddressMarketingSales;
                            _tripProvider.EmailAddressPrimary = model.EmailAddressPrimary;
                            _tripProvider.Name = model.Name;
                            _tripProvider.Telephone01 = model.Telephone01;
                            _tripProvider.Telephone02 = model.Telephone02;
                            _tripProvider.Telephone03 = model.Telephone03;
                            _tripProvider.Type = model.Type;
                            _tripProvider.Website = model.Website;

                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripBookingTransport(View_TripBookingTransport model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        // add a new record if this is new
                        if (model.Id == 0)
                        {
                            var _newBooking = new TripBookingTransport();

                            _newBooking.AdminNotes = model.AdminNotes;
                            _newBooking.Adults = model.Adults;
                            _newBooking.BookingDate = model.BookingDate;
                            _newBooking.BookingStatus = model.BookingStatus;
                            _newBooking.EstimatedCost = model.EstimatedCost;
                            _newBooking.Kids = model.Kids;
                            _newBooking.TransportFrom = model.TransportFrom;
                            _newBooking.TransportTo = model.TransportTo;
                            _newBooking.TransportType = model.TransportType;
                            _newBooking.TravelClass = model.TravelClass;
                            _newBooking.TransferDetails = model.TransferDetails;

                            context.TripBookingTransports.Add(_newBooking);
                            await context.SaveChangesAsync();
                        }

                        // update existing record
                        else
                        {
                            var _booking = context.TripBookingTransports.Find(model.Id);
                            if (_booking != null)
                            {
                                _booking.AdminNotes = model.AdminNotes;
                                _booking.Adults = model.Adults;
                                if (model.BookingDate.HasValue) { _booking.BookingDate = model.BookingDate; }
                                _booking.BookingStatus = model.BookingStatus;
                                _booking.EstimatedCost = model.EstimatedCost;
                                _booking.Kids = model.Kids;
                                _booking.TransportFrom = model.TransportFrom;
                                _booking.TransportTo = model.TransportTo;
                                _booking.TransportType = model.TransportType;
                                _booking.TravelClass = model.TravelClass;
                                _booking.TransferDetails = model.TransferDetails;

                                await context.SaveChangesAsync();
                            }
                        }                            
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripBookingAccommodation(View_TripBookingAccommodation model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {                        
                        if (model.Id == 0)
                        {
                            var _newBooking = new TripBookingAccommodation();
                            _newBooking.AccommodationType = model.AccommodationType;
                            _newBooking.CheckinDate = model.CheckinDate;
                            _newBooking.CheckoutDate = model.CheckoutDate;
                            _newBooking.EstimatedCost = model.EstimatedCost;
                            _newBooking.AdminNotes = model.AdminNotes;
                            _newBooking.PropertyAddress = model.PropertyAddress;
                            _newBooking.PropertyName = model.PropertyName;
                            _newBooking.Status = model.Status;
                            _newBooking.Adults = model.Adults;
                            _newBooking.Kids = model.Kids;
                            _newBooking.TownOrCity = model.TownOrCity;

                            context.TripBookingAccommodations.Add(_newBooking);
                            await context.SaveChangesAsync();
                        }

                        else
                        {
                            var _booking = context.TripBookingAccommodations.Find(model.Id);
                            if (_booking != null)
                            {
                                _booking.AccommodationType = model.AccommodationType;
                                if(model.CheckinDate.HasValue) // only if there is a new value
                                { _booking.CheckinDate = model.CheckinDate; }
                                if (model.CheckoutDate.HasValue) // only if there is a new value
                                { _booking.CheckoutDate = model.CheckoutDate; }                                  
                                _booking.EstimatedCost = model.EstimatedCost;
                                _booking.AdminNotes = model.AdminNotes;
                                _booking.PropertyAddress = model.PropertyAddress;
                                _booking.PropertyName = model.PropertyName;
                                _booking.Status = model.Status;
                                _booking.Adults = model.Adults;
                                _booking.Kids = model.Kids;
                                _booking.TownOrCity = model.TownOrCity;

                                await context.SaveChangesAsync();
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTrip(Trip trip)
        {
            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    var _dbTrip = context.Trips.Find(trip.Id);
                    if (_dbTrip != null)
                    {
                        if (trip.EndDate.HasValue) { _dbTrip.EndDate = trip.EndDate; }
                        if (trip.StartDate.HasValue) { _dbTrip.StartDate = trip.StartDate; }
                        _dbTrip.PaxAdults = trip.PaxAdults;
                        _dbTrip.PaxMinors = trip.PaxMinors;
                        if (!string.IsNullOrEmpty(trip.Status)) { _dbTrip.Status = trip.Status; }
                        if (!string.IsNullOrEmpty(trip.StartLocation)) { _dbTrip.StartLocation = trip.StartLocation; }
                        if (!string.IsNullOrEmpty(trip.EndLocation)) { _dbTrip.EndLocation = trip.EndLocation; }
                        if (!string.IsNullOrEmpty(trip.Description)) { _dbTrip.Description = trip.Description; }

                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripStep"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripStep(View_TripStep tripStep)
        {
            try
            {
                TripStep _dbTripStep = null;

                using (var context = new TravelogyDevEntities1())
                {
                    if (tripStep.Id == 0)
                    {
                        _dbTripStep = new TripStep() { TripId = tripStep.TripId };

                        _dbTripStep.Destination = tripStep.Destination;
                        if (tripStep.StartDate.HasValue) { _dbTripStep.StartDate = tripStep.StartDate; }
                        if (tripStep.EndDate.HasValue) { _dbTripStep.EndDate = tripStep.EndDate; }
                        _dbTripStep.LongDescription = tripStep.LongDescription;
                        _dbTripStep.NightStay = tripStep.NightStay;
                        _dbTripStep.ShortDescription = tripStep.ShortDescription;
                        _dbTripStep.TravelogerNote = tripStep.TravelogerNote;

                        // insert a new one
                        context.TripSteps.Add(_dbTripStep);

                        await context.SaveChangesAsync();
                    }

                    else
                    {
                        _dbTripStep = context.TripSteps.Find(tripStep.Id);
                        if (_dbTripStep != null)
                        {
                            _dbTripStep.Destination = tripStep.Destination;
                            if (tripStep.StartDate.HasValue) { _dbTripStep.StartDate = tripStep.StartDate; }
                            if (tripStep.EndDate.HasValue) { _dbTripStep.EndDate = tripStep.EndDate; }
                            _dbTripStep.LongDescription = tripStep.LongDescription;
                            _dbTripStep.NightStay = tripStep.NightStay;
                            _dbTripStep.ShortDescription = tripStep.ShortDescription;
                            _dbTripStep.TravelogerNote = tripStep.TravelogerNote;

                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveSubDestination(SubDestination model)
        {
            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    // create new
                    if (model.Id == 0)
                    {
                        context.SubDestinations.Add(model);
                        await context.SaveChangesAsync();
                    }

                    // update existing
                    else
                    {
                        var _dbSubDestination = context.SubDestinations.Find(model.Id);
                        if(_dbSubDestination != null)
                        {
                            _dbSubDestination.Name = model.Name;
                            _dbSubDestination.ThumbnailPath = model.ThumbnailPath;
                            _dbSubDestination.Type = model.Type;
                            _dbSubDestination.Description = model.Description;

                            await context.SaveChangesAsync();
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveDestinationCost(DestinationCost model)
        {
            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    // create new
                    if (model.Id == 0)
                    {
                        context.DestinationCosts.Add(model);
                        await context.SaveChangesAsync();
                    }

                    // else update existing
                    else
                    {
                        var _dbDestinationCost = context.DestinationCosts.Find(model.Id);
                        if(_dbDestinationCost != null)
                        {
                            _dbDestinationCost.Amount = model.Amount;
                            _dbDestinationCost.Class = model.Class;
                            _dbDestinationCost.Currency = model.Currency;
                            _dbDestinationCost.Description = model.Description;
                            _dbDestinationCost.Destination = model.Destination;
                            _dbDestinationCost.DestinationId = model.DestinationId;
                            _dbDestinationCost.Season = model.Season;
                            _dbDestinationCost.ShortDescription = model.ShortDescription;
                            _dbDestinationCost.Type = model.Type;

                            await context.SaveChangesAsync();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveDestinationActivity(DestinationActivity model)
        {
            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    // create new
                    if (model.Id == 0)
                    {
                        context.DestinationActivities.Add(model);
                        await context.SaveChangesAsync();
                    }

                    // update existing
                    else
                    {
                        var _dbActivityObj = context.DestinationActivities.Find(model.Id);
                        if (_dbActivityObj != null)
                        {
                            _dbActivityObj.Name = model.Name;
                            _dbActivityObj.ThumbnailPath = model.ThumbnailPath;
                            _dbActivityObj.Type = model.Type;
                            _dbActivityObj.Description = model.Description;

                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveDestinationInterest(DestinationInterest model)
        {
            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    // create new
                    if (model.Id == 0)
                    {
                        context.DestinationInterests.Add(model);
                        await context.SaveChangesAsync();
                    }

                    // update existing
                    else
                    {
                        var _dbInterestObj = context.DestinationInterests.Find(model.Id);
                        if (_dbInterestObj != null)
                        {
                            _dbInterestObj.Name = model.Name;
                            _dbInterestObj.ThumbnailPath = model.ThumbnailPath;
                            _dbInterestObj.Type = model.Type;
                            _dbInterestObj.Description = model.Description;

                            await context.SaveChangesAsync();
                        }
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
