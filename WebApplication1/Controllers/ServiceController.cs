﻿using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;

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
            var _messageList = new List<MessageCollection>();
            var _blError = ThreadManager.GetAllMessages(User.Identity.GetUserId(), out _messageList);
            var model = new MessageListModel() { AllMessages = _messageList};

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
                    return RedirectToAction("TripPlanning");
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
                List<Trip> _allTrips = null;
                var blError = TripManager.GetAllTripsForUser(User.Identity.GetUserId(), out _allTrips);
                model.AllTrips = _allTrips;

                // find any active trip - to move to a business layer method
                var _activeTrip = _allTrips.FirstOrDefault((p => (p.Status.Trim() == TripStatus.booked.ToString())
                    || (p.Status.Trim() == TripStatus.planned.ToString())));

                // if there IS an active trip - redirect to view
                if (_activeTrip != null)
                {
                    var _tripViewModel = new TripViewModel() { ActiveTrip = _activeTrip };
                    return View("Trip", _tripViewModel);
                }
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
            var _availableTemplates = new List<TripTemplate>();
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
            TripTemplate _template = null;
            var _blerror = TripManager.GetTripTemplatesById(templateId, out _template);

            // assign the template to the view model
            var _model = new TripViewModel();
            _model.CreateTripTemplate = _template;
            _model.CreateTripViewModel = new CreateTripViewModel() { TemplateId = _template.Id, StartDate = DateTime.Now };

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
            List<Trip> _allTrips = null;
            _blError = TripManager.GetAllTripsForUser(User.Identity.GetUserId(), out _allTrips);
            model.AllTrips = _allTrips;
            model.ActiveTrip = _allTrips.FirstOrDefault((p => (p.Status.Trim() == TripStatus.booked.ToString())
                    || (p.Status.Trim() == TripStatus.planned.ToString())));
            return View("Trip", model);
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