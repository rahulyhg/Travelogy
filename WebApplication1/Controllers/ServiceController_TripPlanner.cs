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
                blError = DestinationManager.GetTopDestinations("", 3, out _destinations);
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
            var _blError = TripManager.SearchTripTemplatesByAlias(templateAlias, out _availableTemplates);
            var _model = new TripViewModel() { AliasName = templateAlias, AllTemplates = _availableTemplates };

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
            _model.CreateTripViewModel = new CreateTripViewModel() { TemplateId = _template.DlTemplate.Id, StartDate = DateTime.Now };

            return View("Trip", _model);
        }

        /// <summary>
        /// Creates a new trip for the user - based on template and the user's inputs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrip(TripViewModel model)
        {
            // create a new Trip object from the form and save it
            Trip trip = new Trip()
            {
                AspNetUserId = User.Identity.GetUserId(),
                TripTemplateId = model.CreateTripViewModel.TemplateId,
                StartDate = model.CreateTripViewModel.StartDate,
                Status = TripStatus.planned.ToString()
            };

            var _blError = TripManager.CreateTrip(trip);
            BlViewTrip viewTrip = null;
            _blError = TripManager.GetTripById(trip.Id, out viewTrip);
            var _model = new TripViewModel() { ActiveTrip = viewTrip };
            return View("Trip", _model);
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
            var _model = new TripViewModel() { ActiveTrip = trip };
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
            List<BlTripTemplate> _relatedTemplates = null;
            _blError = TripManager.SearchTripTemplatesByAlias(trip.DlTripView.SearchAlias, out _relatedTemplates, trip.DlTripView.TripTemplateId);
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
        public ActionResult SaveTrip(EditTripViewModel model)
        {
            BlViewTrip viewTrip = null;
            var _blError = TripManager.SaveUserTripChanges(model.ActiveTrip.DlTripView, model.ActiveTrip.DlTripStepsView);
            _blError = TripManager.GetTripById(model.ActiveTrip.DlTripView.Id, out viewTrip);
            var _tripModel = new TripViewModel() { ActiveTrip = viewTrip };
            return View("Trip", _tripModel);
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
    }
}