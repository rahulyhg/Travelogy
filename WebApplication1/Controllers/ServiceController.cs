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
    public class ServiceController : DomingoControllerBase
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: Service
        public ActionResult TravelLogistics()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MessageCenter()
        {            
            var _allMessages = new List<MessageCollection>(); // all messages
            var _tripMessages = new List<MessageCollection>(); // trip related messages
            var _recentMessages = new List<MessageCollection>(); // trip related messages
            var _oldtMessages = new List<MessageCollection>(); // trip related messages

            // get all messages for the user from the db
            var _blError = ThreadManager.GetAllMessages(User.Identity.GetUserId(), out _allMessages);

            // find the trip related messages and add to collection
            foreach(var _thread in _allMessages)
            {
                if(_thread.Thread.tripthreadid.HasValue && _thread.Thread.tripthreadid.Value > 0)
                {
                    _tripMessages.Add(_thread);
                }
            }

            // get upto three latest message threads into recent list
            // rest in the old one
            if(_allMessages.Count > 3)
            {
                _recentMessages.AddRange(_allMessages.GetRange(0,3));
                _oldtMessages.AddRange(_allMessages.GetRange(3, _allMessages.Count-3));
            }
            else
            {
                _recentMessages = _allMessages;
            }



            // pass all objects to the view
            var model = new MessageListModel() { TripMessages = _tripMessages, RecentMessages = _recentMessages, OldMessages = _oldtMessages};

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateThread(MessageViewModel _model)
        {
            var _message = new ThreadMessage()
                {
                    TravellerId = 999,
                    Body = _model.Body,
                    CreatedDate = DateTime.Now,                    
                    AspnetUserId = User.Identity.GetUserId()
                };

            var _blError = new DomingoBlError();
            if(_model.TripId == 0)
            {
                _blError = ThreadManager.CreateThread(_message, _model.Subject);
            }
            else
            {
                _blError = ThreadManager.CreateThreadforTrip(_message, _model.Subject, _model.TripId);
            }
            
            if (_blError.ErrorCode == 0)
            {
                if (_model.TripId == 0)
                {
                    return RedirectToAction("MessageCenter");
                }

                else
                {
                    View_Trip trip = null;
                    _blError = TripManager.GetTripById(_model.TripId, out trip);
                    var _tripViewModel = new TripViewModel() { ActiveTrip = trip };
                    return View("Trip", _tripViewModel);
                }
            }

            return RedirectToAction("MessageCenter");
        }

        /// <summary>
        /// Reply to a message - user (admin console is in a different method)
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="replyMessage"></param>
        /// <param name="tripId"></param>
        /// <param name="aDmin"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ReplyToThread(int threadId, string replyMessage, int tripId, int aDmin = 0)
        {
            if(!string.IsNullOrEmpty(replyMessage))
            {
                var threadMessage = new ThreadMessage()
                {
                    AspnetUserId = User.Identity.GetUserId(),
                    ThreadId = threadId,
                    Body = replyMessage,
                    CreatedDate = DateTime.Now,
                    TravellerId = 990,
                    IsAdmin = aDmin > 0 ? true : false
                };

                var _blError = ThreadManager.AddToThread(threadMessage);
            }

            if (aDmin > 0)
            {
                return RedirectToAction("MessageCenter", "Admin");
            }


            if (tripId != 0)
            {
                return RedirectToAction("TripPlanning");
            }

            return RedirectToAction("MessageCenter");
        }

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
                List<View_Trip> _allTrips = null;
                var blError = TripManager.GetAllTripsForUser(User.Identity.GetUserId(), out _allTrips);
                model.AllTrips = _allTrips;

                // find any active trip - to move to a business layer method
                var _activeTrip = _allTrips.FirstOrDefault(p => (p.Status.Trim() == TripStatus.booked.ToString()));

                // if there IS an active trip - redirect to view
                if (_activeTrip != null)
                {
                    var _tripViewModel = new TripViewModel() { ActiveTrip = _activeTrip };
                    return View("Trip", _tripViewModel);
                }

                // else show all the planned trips
                var _plannedTrips = _allTrips.Where(p => (p.Status.Trim() == TripStatus.planned.ToString())).ToList();
                model.PlannedTrips = _plannedTrips;
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
            View_Trip viewTrip = null;
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
            View_Trip trip = null;
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
            View_Trip trip = null;
            var _blError = TripManager.GetTripById(tripId, out trip);
            var _model = new EditTripViewModel() { ActiveTrip = trip };
            return View("EditTrip", _model);
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