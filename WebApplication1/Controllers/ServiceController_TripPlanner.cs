using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using DomingoBL.BlObjects;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ServiceController : DomingoControllerBase
    {
        // GET: TripPlanning
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TripPlanning()
        {
            var model = new TripPlanningViewModel();

            if (Request.IsAuthenticated)
            {
                // find all trips for the user
                List<BlViewTrip> _allTrips = null;
                var blError = TripManager.GetAllTripsForUser(User.Identity.GetUserId(), out _allTrips);
                model.AllTrips = _allTrips;

                // find any active trip - to move to a business layer method
                var _activeTrip = _allTrips.FirstOrDefault(p => (p.DlTripView.Status.Trim() == TripStatus.booked.ToString()));

                // if there IS an active trip - redirect to view
                if (_activeTrip != null)
                {
                    var _tripViewModel = new TripViewModel() { ActiveTrip = _activeTrip };
                    return View("Trip", _tripViewModel);
                }

                // else show all the planned trips
                var _plannedTrips = _allTrips.Where(p => (p.DlTripView.Status.Trim() == TripStatus.planned.ToString())).ToList();
                model.PlannedTrips = _plannedTrips;

                List<Destination> _destinations = null;
                blError = DestinationManager.GetTopDestinations("", 8, out _destinations);
                model.SuggestedDestinations = _destinations;
            }

            return View(model);
        }

        /// <summary>
        /// List all templates based on a search alias
        /// </summary>
        /// <param name="templateAlias"></param>
        /// <returns></returns>
        //[Authorize]
        public ActionResult ListAllTripTemplates(string templateAlias)
        {
            var _availableTemplates = new List<BlTripTemplate>();
            var _allTemplates = new List<BlTripTemplate>();
            var _blError = TripManager.SearchTripTemplatesByAlias(templateAlias, out _availableTemplates);
            var _trip = TripManager.GetImmediateTripForUser(User.Identity.GetUserId());

            if (_availableTemplates != null)
            {
                foreach (var template in _availableTemplates)
                {
                    if (!_trip.DlTripView.Templates.Contains(template.DlTemplate.Id.ToString()))
                    {
                        _allTemplates.Add(template);
                    }
                }
            }

            var _model = new TripViewModel()
            {
                AliasName = templateAlias,
                AllTemplates = _allTemplates
            };

            if(_trip != null)
            {
                _model.ImmediateTripId = _trip.DlTripView.Id;
                _model.ActiveTrip = _trip;
            }

            if (_availableTemplates.Count > 0)
            {
                _model.Country = _availableTemplates[0].Country;
            }

            return View("TripTemplates", _model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateTripForCountry(string country)
        {
            BlTripTemplate _template = null;
            var _blerror = TripManager.GetDefaultTemplateForCountry(country, out _template);
            if(_blerror.ErrorCode > 0 || _template == null || _template.DlTemplate == null)
            {
                throw new ApplicationException("No GetDefaultTemplateForCountry for: " + country);
            }

            // assign the template to the view model
            var _model = new TripViewModel();
            _model.CreateTripTemplate = _template;
            _model.CreateTripViewModel 
                = new CreateTripViewModel()
                    {
                        DestinationId = _template.DlTemplate.DestinationId,
                        TemplateId = _template.DlTemplate.Id,
                        StartDate = DateTime.Now
                    };

            return View("Trip", _model);
        }

        /// <summary>
        /// method to create a form for trip based on a template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateTripFromTemplate(int templateId)
        {
            // get the trip template
            BlTripTemplate _template = null;
            var _blerror = TripManager.GetTripTemplatesById(templateId, out _template);

            // assign the template to the view model
            var _model = new TripViewModel();
            _model.CreateTripTemplate = _template;
            _model.CreateTripViewModel = new CreateTripViewModel() { DestinationId = _template.DlTemplate.DestinationId, TemplateId = _template.DlTemplate.Id, StartDate = DateTime.Now };

            return View("Trip", _model);
        }

        /// <summary>
        /// Creates a new trip for the user - based on template and the user's inputs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrip(TripViewModel model)
        {
            // create a new Trip object from the form and save it
            Trip trip = new Trip()
            {
                AspNetUserId = User.Identity.GetUserId(),
                DestinationId = model.CreateTripViewModel.DestinationId,
                NickName = model.CreateTripViewModel.NickName,
                StartDate = model.CreateTripViewModel.StartDate,
                Status = TripStatus.planned.ToString()
            };

            var _blError = await TripManager.CreateTrip(trip, model.CreateTripViewModel.TemplateId);
            if(_blError.ErrorCode > 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }   
                    
            return RedirectToAction("ViewTrip", new { tripId = trip.Id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewTrip(int tripId)
        {
            BlViewTrip trip = null;
            var _blError = TripManager.GetTripById(tripId, out trip);
            List<BlTripTemplate> _allTemplates = null;
            List<BlTripTemplate> _relatedTemplates = new List<BlTripTemplate>();
            _blError = TripManager.SearchRelatedTripTemplates(trip.DlTripView.Id, out _allTemplates);

            if (_allTemplates != null)
            {
                foreach (var template in _allTemplates)
                {
                    if (!trip.DlTripView.Templates.Contains(template.DlTemplate.Id.ToString()))
                    {
                        _relatedTemplates.Add(template);
                    }
                }
            }
            var _model = new TripViewModel() { ActiveTrip = trip, RelatedTemplates = _relatedTemplates };
            return View("Trip", _model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTrip(int tripId)
        {
            BlViewTrip trip = null;
            var _blError = TripManager.GetTripById(tripId, out trip);
            List<BlTripTemplate> _allTemplates = null;
            List<BlTripTemplate> _relatedTemplates = new List<BlTripTemplate>();
            _blError = TripManager.SearchTripTemplatesByAlias(trip.DlTripView.TemplateSearchAlias, out _allTemplates);

            if(_allTemplates != null)
            {
                foreach(var template in _allTemplates)
                {
                    if(!trip.DlTripView.Templates.Contains(template.DlTemplate.Id.ToString()))
                    {
                        _relatedTemplates.Add(template);
                    }
                }
            }
            var _model = new EditTripViewModel() { ActiveTrip = trip, RelatedTemplates = _relatedTemplates };
            return View("EditTrip", _model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTripAsync(EditTripViewModel model)
        {
            var _blError = await TripManager.SaveUserTripChangesAsync(model.ActiveTrip.DlTripView, model.ActiveTrip.DlTripStepsView);
            return RedirectToAction("ViewTrip", new { @tripId = model.ActiveTrip.DlTripView.Id });
        }


        /// <summary>
        /// Show the view for Trip
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Trip(TripViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddCircuitToTrip(int tripId, int templateId = 0)
        {
            BlViewTrip trip = null;
            var _blError = TripManager.GetTripById(tripId, out trip);

            var model = new TripViewModel();
            model.ActiveTrip = trip;

            BlTripTemplate template = null;
            TripManager.GetTripTemplatesById(templateId, out template);
            model.CreateTripTemplate = template;

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCircuitToTripSubmit(TripViewModel model)
        {
            var blError = await TripManager.AddTemplateToTripAsync(model.ActiveTrip.DlTripView, model.CreateTripTemplate.DlTemplate.Id);
            return RedirectToAction("ViewTrip", new { tripId = model.ActiveTrip.DlTripView.Id });            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripStepId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddTripBookingAccommodation(int tripStepId, int tripId)
        {
            BlViewTrip tripObj = null;
            var blError = TripManager.GetTripById(tripId, out tripObj);
            if(blError.ErrorCode > 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var tripStepObj = tripObj.DlTripStepsView.FirstOrDefault(p => p.Id == tripStepId);
            if(tripStepObj == null)
            {
                throw new ApplicationException(string.Format("Invalid parameter - [TripStepId:{0}]", tripStepId));
            }

            var model = new AccommodationBookingViewModel()
            {                
                TripName = tripObj.DlTripView.Name,
                TripDescription = tripObj.DlTripView.Description,
                TripStepName = tripStepObj.ShortDescription,
                TripStepDescription = tripStepObj.LongDescription,
                TripStepStartDate = tripStepObj.StartDate,
                TripStepEndDate = tripStepObj.EndDate,
                TripStepId = tripStepId,
                TripId = tripId
            };

            return View("AccommodationBooking", model);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewAccommodationBooking(int id)
        {
            TripBookingAccommodation accommodation = null;
            var blError = TripManager.GetTripBookingAccommodation(id, out accommodation);
            if(blError.ErrorCode > 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var model = new AccommodationBookingViewModel()
            {
                AccommodationType = accommodation.AccommodationType,
                CheckinDate = accommodation.CheckinDate.HasValue ? accommodation.CheckinDate.Value : DateTime.MinValue,
                CheckoutDate = accommodation.CheckoutDate.HasValue ? accommodation.CheckoutDate.Value : DateTime.MinValue,
                TravellerNotes = accommodation.TravellerNotes,
                SpecialRequests = accommodation.SpecialRequests,
                BookingStatus = accommodation.Status,
                AdminNotes = accommodation.AdminNotes,
                TripId = accommodation.TripId,
                TownOrCity = accommodation.TownOrCity,
                Adults = accommodation.Adults.HasValue ? accommodation.Adults.Value : 0,
                Kids = accommodation.Kids.HasValue ? accommodation.Kids.Value : 0,
                PropertyName = accommodation.PropertyName,
                PropertyAddress = accommodation.PropertyAddress,
            };
            
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTripBookingAccommodationAsync(AccommodationBookingViewModel model)
        {
            var accommodation = new TripBookingAccommodation()
            {
                Kids = model.Kids,
                Adults = model.Adults,
                TownOrCity = model.TownOrCity,
                AccommodationType = model.AccommodationType,
                CheckinDate = model.CheckinDate,
                CheckoutDate = model.CheckoutDate,
                TravellerNotes = model.TravellerNotes,
                SpecialRequests = model.SpecialRequests,
                Status = AccommodationBookingStatus.requested.ToString(),
                TripId = model.TripId,
                TripStepId = model.TripStepId
            };
            
            var blError = await TripManager.SaveTripBookingAccommodationAsync(accommodation);
            if(blError.ErrorCode == 0)
            {
                return RedirectToAction("ViewTrip", new { @tripId = model.TripId });
            }
            else
            {
                ModelState.AddModelError("blError.ErrorMessage", blError.ErrorMessage);
                return View("AccommodationBooking", model);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="tripStepId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddFlightBooking(int tripId, int tripStepId = 0)
        {
            BlViewTrip tripObj = null;
            var blError = TripManager.GetTripById(tripId, out tripObj);
            if (blError.ErrorCode > 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            View_TripStep tripStepObj = null;
            if(tripStepId > 0)
            {
                tripStepObj = tripObj.DlTripStepsView.FirstOrDefault(p => p.Id == tripStepId);
                if (tripStepObj == null)
                {
                    throw new ApplicationException(string.Format("Invalid parameter - [TripStepId:{0}]", tripStepId));
                }
            }            

            var model = new FlightBookingViewModel()
            {
                TripBookingTransportId = 0,
                TripId = tripId,
                TripStepId = tripStepId,
                TripName = tripObj.DlTripView.Name,
                TripDescription = tripObj.DlTripView.Description
            };

            return View("FlightBooking", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveFlightBookingAccommodationAsync(FlightBookingViewModel model)
        {
            var _booking = new TripBookingTransport();

            if (model.FlightClass.ToLower().Contains("train")) { _booking.TransportType = "Train"; }
            else if (model.FlightClass.ToLower().Contains("bus")) { _booking.TransportType = "Bus"; }
            else if (model.FlightClass.ToLower().Contains("taxi")) { _booking.TransportType = "Taxi"; }
            else { _booking.TransportType = "Flight"; }

            _booking.TripId = model.TripId;
            _booking.TripStepId = model.TripStepId;

            _booking.Id = model.TripBookingTransportId;
            _booking.Adults = model.Adults;
            _booking.BookingDate = model.FlightDate;
            _booking.BookingStatus = TransferBookingStatus.requested.ToString();
            _booking.Kids = model.Kids;
            _booking.TransportFrom = model.From;
            _booking.TransportTo = model.To;
            _booking.TravelClass = model.FlightClass;
            _booking.TravellerNotes = model.TravellerNotes;

            var blError = await TripManager.SaveTripBookingTransportAsync(_booking);

            return RedirectToAction("ViewTrip", new { @tripId = model.TripId });
        }

        [Authorize]
        public ActionResult ViewTransferBooking(int id)
        {
            View_TripBookingTransport transfer = null;
            var blError = TripManager.GetTripBookingTransport(id, out transfer);
            if(blError.ErrorCode > 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var model = new FlightBookingViewModel()
            {
                TripId = transfer.TripId,
                Adults = transfer.Adults.HasValue ? transfer.Adults.Value : 0,
                BookingStatus = transfer.BookingStatus,
                FlightClass = transfer.TravelClass,
                FlightDate = transfer.BookingDate.HasValue ? transfer.BookingDate.Value : DateTime.MinValue,
                From = transfer.TransportFrom, To = transfer.TransportTo,
                Kids = transfer.Kids.HasValue ? transfer.Kids.Value : 0,
                TravellerNotes = transfer.TravellerNotes,
                TransportType = transfer.TransportType,
                AdminNotes = transfer.AdminNotes,
                TransferDetails = transfer.TransferDetails                                                
            };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="tripStepId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddLocalTransfer(int tripId, int tripStepId = 0)
        {
            var model = new LocalTransferViewModel()
            {
                TripId = tripId,
                TripStepId = tripStepId
            };

            return View("LocalTransfer", model);
        }
    }
}