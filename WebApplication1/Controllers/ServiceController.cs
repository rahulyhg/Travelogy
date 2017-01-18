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
    public class ServiceController : Controller
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
            var model = new TripModel { AllTrips = _allTrips };
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
        public ActionResult CreateTrip(string templateAlias)
        {
            var _model = new TripViewModel() { AliasName = templateAlias };
            return RedirectToAction("trip", _model);
        }

        [Authorize]
        public ActionResult Trip(TripViewModel model)
        {            
            return View(model);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            // Redirect on error:
            filterContext.Result = RedirectToAction("Error", "Home");            
        }

    }
}