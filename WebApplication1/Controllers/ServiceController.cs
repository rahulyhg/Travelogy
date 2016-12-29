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
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult MessageCenter()
        {
            var _messageList = new List<ThreadMessage>();
            var _blError = ThreadManager.GetAllMessages(User.Identity.GetUserId(), out _messageList);
            var model = new MessageListModel() { Header = "Your messages", Description = "Your travel queries",
                AllMessages = new List<MessageViewModel>() };

            foreach(var message in _messageList)
            {
                model.AllMessages.Add(new MessageViewModel() { Body = message.Body, CreateDate = message.CreatedDate } );
            } 
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}