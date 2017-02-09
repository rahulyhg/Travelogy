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
                            context.SaveChanges();
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
                        var _booking = context.TripBookingTransports.Find(model.Id);
                        if (_booking != null)
                        {
                            _booking.AdminNotes = model.AdminNotes;
                            _booking.Adults = model.Adults;
                            _booking.BookingDate = model.BookingDate;
                            _booking.BookingStatus = model.BookingStatus;
                            _booking.EstimatedCost = model.EstimatedCost;
                            _booking.Kids = model.Kids;
                            _booking.TransportFrom = model.TransportFrom;
                            _booking.TransportTo = model.TransportTo;
                            _booking.TransportType = model.TransportType;
                            _booking.TravelClass = model.TravelClass;

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
        public static async Task<DomingoBlError> SaveTripBookingAccommodation(View_TripBookingAccommodation model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        var _booking = context.TripBookingAccommodations.Find(model.Id);
                        if (_booking != null)
                        {
                            _booking.AccommodationType = model.AccommodationType;
                            _booking.CheckinDate = model.CheckinDate;
                            _booking.CheckoutDate = model.CheckoutDate;
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
                    if(_dbTrip != null)
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
                using (var context = new TravelogyDevEntities1())
                {
                    var _dbTripStep = context.TripSteps.Find(tripStep.Id);
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
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

    }
}
