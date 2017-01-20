using DomingoBL;
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

        // GET: TripPlanning
        public ActionResult TripPlanning()
        {
            List<Trip> _allTrips = null;
            var blError = TripManager.GetAllTripsForUser(User.Identity.GetUserId(), out _allTrips);
            var model = new TripPlanningViewModel { AllTrips = _allTrips };
            return View(model);
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
            var _message = new ThreadMessage() { TravellerId = 999, Body = _model.Body, CreatedDate = DateTime.Now, AspnetUserId = User.Identity.GetUserId() };
            var _blError = ThreadManager.CreateThread(_message, _model.Subject);
            if (_blError.ErrorCode == 0)
            {
                return RedirectToAction("MessageCenter");
            }

            return RedirectToAction("MessageCenter");
        }

        [Authorize]
        public ActionResult ReplyToThread(int threadId, string replyMessage)
        {
            if(!string.IsNullOrEmpty(replyMessage))
            {
                var threadMessage = new ThreadMessage()
                {
                    AspnetUserId = User.Identity.GetUserId(),
                    ThreadId = threadId,
                    Body = replyMessage,
                    CreatedDate = DateTime.Now,
                    TravellerId = 990
                };

                var _blError = ThreadManager.AddToThread(threadMessage);
            }            

            return RedirectToAction("MessageCenter");
        }

        [Authorize]
        public ActionResult ListAllTripTemplates(string templateAlias)
        {
            var _availableTemplates = new List<TripTemplate>();
            var _blError = TripManager.SearchTripTemplatesByAlias(templateAlias, out _availableTemplates);
            var _model = new TripViewModel() { AliasName = templateAlias, AllTemplates = _availableTemplates };

            return View("Trip", _model);
            //return RedirectToAction("trip", _model);
        }

        [Authorize]
        public ActionResult CreateTripFromTemplate(int templateId)
        {
            // get the trip template
            TripTemplate _template = null;
            var _blerror = TripManager.GetTripTemplatesById(templateId, out _template);

            // assign the template to the view model
            var _model = new TripViewModel();
            _model.CreateTripTemplate = _template;
            _model.CreateTripViewModel = new CreateTripViewModel() { TemplateId = _template.Id };

            return View("Trip", _model);
        }

        [Authorize]
        public ActionResult Trip(TripViewModel model)
        {            
            return View(model);
        }

        /// <summary>
        /// Creates a new trip for the user
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
                StartDate = model.CreateTripViewModel.StartDate
            };
            
            var _blError = TripManager.CreateTrip(trip);
            return View("Trip", model);
        }
    }
}