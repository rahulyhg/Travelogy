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
        [Authorize]
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
                var _plannedTrips = _allTrips.Where(p => (p.DlTripView.Status.Trim() == TripStatus.planned.ToString() || p.DlTripView.Status.Trim() == TripStatus.consulting.ToString())).ToList();
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
        [Authorize]
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
                    // if there is already a trip that includes this template - then
                    if (_trip != null && _trip.DlTripView != null && !String.IsNullOrEmpty( _trip.DlTripView.Templates) 
                        && _trip.DlTripView.Templates.Contains(template.DlTemplate.Id.ToString()))
                    {
                        // do nothing
                    }

                    else // add it to the available list
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
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewTripTemplate(int TemplateId)
        {
            BlTripTemplate _template = null;
            var _blError = TripManager.GetTripTemplatesById(TemplateId, out _template);
            if(_blError.ErrorCode != 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }            

            var _model = new ViewTripTemplateViewModel() {  TripTemplate = _template };

            var _trip = TripManager.GetImmediateTripForUser(User.Identity.GetUserId());
            if(_trip != null)
            {
                _model.TripId = _trip.DlTripView.Id;
            }

            return View(_model);
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

            var startLocationOptions = _template.DlTemplate.StartLocation.Split('/').ToList();
            var _tripStartLocationOptions = new List<SelectListItem>();
            foreach(var startLocation in startLocationOptions)
            {
                _tripStartLocationOptions.Add(new SelectListItem() { Text = startLocation, Value = startLocation });
            }

            _model.CreateTripStartLocationOptions = _tripStartLocationOptions;

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

            var startLocationOptions = _template.DlTemplate.StartLocation.Split('/').ToList();
            var _tripStartLocationOptions = new List<SelectListItem>();
            foreach (var startLocation in startLocationOptions)
            {
                _tripStartLocationOptions.Add(new SelectListItem() { Text = startLocation, Value = startLocation });
            }

            _model.CreateTripStartLocationOptions = _tripStartLocationOptions;

            _model.CreateTripViewModel = new CreateTripViewModel() { DestinationId = _template.DlTemplate.DestinationId, TemplateId = _template.DlTemplate.Id, StartDate = DateTime.Now };

            return View("Trip", _model);
        }

        /// <summary>
        /// Creates a new trip for the user - based on template and the user's inputs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
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
                StartLocation = model.CreateTripViewModel.StartLocation,
                Status = TripStatus.planned.ToString(),
                HomeLocation = model.CreateTripViewModel.HomeLocation,
                PaxAdults = model.CreateTripViewModel.Adults,
                PaxMinors = model.CreateTripViewModel.Minors,
                TripType = model.CreateTripViewModel.TripType,
                TripCurrency = model.CreateTripViewModel.Currency,
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
            // get the trip details
            BlViewTrip trip = null;
            var _blError = TripManager.GetTripById(tripId, out trip);

            // error handling
            if(_blError.ErrorCode != 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }

            if(trip == null || trip.DlTripView == null)
            {
                throw new ApplicationException(String.Format("Invalid trip id requested: {0}", tripId));
            }

            if(trip.DlTripView.AspNetUserId != User.Identity.GetUserId())
            {
                throw new ApplicationException(String.Format("Unauthorized trip id requested: {0} by {1}", tripId, User.Identity.GetUserId()));
            }

            // all seems ok if you reach here
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

        [Authorize]
        public ActionResult DeleteTrip(int tripId)
        {
            BlViewTrip trip = null;
            var _blError = TripManager.GetTripById(tripId, out trip);
            // error handling
            if (_blError.ErrorCode != 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }

            if (trip == null || trip.DlTripView == null)
            {
                throw new ApplicationException(String.Format("Invalid trip id requested: {0}", tripId));
            }

            if (trip.DlTripView.AspNetUserId != User.Identity.GetUserId())
            {
                throw new ApplicationException(String.Format("Unauthorized trip id requested: {0} by {1}", tripId, User.Identity.GetUserId()));
            }

            // looks like all ok
            var _model = new EditTripViewModel() { ActiveTrip = trip, RelatedTemplates = null };
            return View(_model);
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
            // error handling
            if (_blError.ErrorCode != 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }

            if (trip == null || trip.DlTripView == null)
            {
                throw new ApplicationException(String.Format("Invalid trip id requested: {0}", tripId));
            }

            if (trip.DlTripView.AspNetUserId != User.Identity.GetUserId())
            {
                throw new ApplicationException(String.Format("Unauthorized trip id requested: {0} by {1}", tripId, User.Identity.GetUserId()));
            }

            // looks like all ok
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

            var startLocationOptions = trip.DlTripView.StartLocation.Split('/').ToList();
            var _tripStartLocationOptions = new List<SelectListItem>();
            foreach (var startLocation in startLocationOptions)
            {
                _tripStartLocationOptions.Add(new SelectListItem() { Text = startLocation, Value = startLocation });
            }
            
            var _model = new EditTripViewModel() { ActiveTrip = trip, RelatedTemplates = _relatedTemplates, TripStartLocationOptions = _tripStartLocationOptions };
            return View("EditTrip", _model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTripAsync(EditTripViewModel model)
        {
            var _blError = await TripManager.SaveUserTripChangesAsync(model.ActiveTrip.DlTripView, model.ActiveTrip.DlTripStepsView);
            return RedirectToAction("ViewTrip", new { @tripId = model.ActiveTrip.DlTripView.Id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="submit"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveDestinationFromTripAsync(RemoveDestinationFromTripViewModel model, string submit)
        {
            if (submit == "remove")
            {
                var _blError = await TripManager.DeleteUserTripstepAsync(model.DlTripStep.Id);
                if (_blError.ErrorCode != 0)
                {
                    throw new ApplicationException(_blError.ErrorMessage);
                }
            }

            return RedirectToAction("EditTrip", new { @tripId = model.DlTripStep.TripId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="submit"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTripAsync(EditTripViewModel model, string submit)
        {
            if(submit == "discard")
            {
                // remove session var
                Session["ImmediateTrip"] = null;

                // update the status to DELETED
                var _blError = await TripManager.DeleteUserTripAsync(model.ActiveTrip.DlTripView);
                if(_blError.ErrorCode != 0)
                {
                    throw new ApplicationException(_blError.ErrorMessage);
                }

                return RedirectToAction("Index", "Home");
            }
            
            return RedirectToAction("ViewTrip", new { @tripId = model.ActiveTrip.DlTripView.Id });
        }


        [Authorize]        
        public async Task<ActionResult> StartTripBooking(int tripId)
        {
            var _blError = await TripManager.StartTripBookingAsync(tripId);
            return RedirectToAction("ViewTrip", new { @tripId = tripId });
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
            if (_blError.ErrorCode != 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }

            var model = new TripViewModel();
            model.ActiveTrip = trip;

            BlTripTemplate template = null;
            _blError = TripManager.GetTripTemplatesById(templateId, out template);
            if (_blError.ErrorCode != 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }

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
            if (blError.ErrorCode != 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            return RedirectToAction("ViewTrip", new { tripId = model.ActiveTrip.DlTripView.Id });            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripStepId"></param>
        /// <param name="tripId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult RemoveDestinationFromTrip(int tripStepId, int tripId)
        {
            var _model = new RemoveDestinationFromTripViewModel();
            BlViewTrip tripObj = null;
            var blError = TripManager.GetTripstepDetailsById(tripStepId, out tripObj);
            if(blError.ErrorCode != 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            _model.DlTripStep = tripObj.DlTripStepsView[0];
            _model.DlTrip = tripObj.DlTripView;
            _model.DlBookingsView = tripObj.DlBookingsView;
            _model.DlTransportsBookingsView = tripObj.DlTransportsBookingsView;

            return View(_model);
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
                TripStepName = String.Format("{0} : {1}", tripStepObj.Destination, tripStepObj.ShortDescription),
                TripStepDescription = tripStepObj.LongDescription,
                TripStepStartDate = tripStepObj.StartDate,
                TripStepEndDate = tripStepObj.EndDate,
                TripStepId = tripStepId,
                TripId = tripId,
                Adults = tripObj.DlTripView.PaxAdults,
                Kids = tripObj.DlTripView.PaxMinors,
                TownOrCity = tripStepObj.Destination,
            };

            if(tripStepObj.StartDate.HasValue)
            {
                model.CheckinDate = tripStepObj.StartDate.Value;
            }

            if(tripStepObj.EndDate.HasValue)
            {
                model.CheckoutDate = tripStepObj.EndDate.Value;
            }

            return View("AccommodationBooking", model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
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
                TripDescription = tripObj.DlTripView.Description,
                Adults = tripObj.DlTripView.PaxAdults,
                Kids = tripObj.DlTripView.PaxMinors,
                TripStartDate = (tripStepObj != null && tripStepObj.StartDate.HasValue) ? tripStepObj.StartDate : tripObj.DlTripView.StartDate,
                FlightDate = (tripStepObj != null && tripStepObj.StartDate.HasValue) ? tripStepObj.StartDate.Value : tripObj.DlTripView.StartDate.Value,
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
            else if (model.FlightClass.ToLower().Contains("flight")) { _booking.TransportType = "Flight"; }
            else if (model.FlightClass.ToLower().Contains("bus") || model.FlightClass.ToLower().Contains("coach")) { _booking.TransportType = "Bus"; }
            else if (model.FlightClass.ToLower().Contains("taxi")) { _booking.TransportType = "Taxi"; }
            else { _booking.TransportType = "Unknown"; }

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