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
    public partial class ServiceController : DomingoControllerBase
    {
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
            foreach (var _thread in _allMessages)
            {
                if (_thread.Thread.tripthreadid.HasValue && _thread.Thread.tripthreadid.Value > 0)
                {
                    _tripMessages.Add(_thread);
                }
            }

            // get upto three latest message threads into recent list
            // rest in the old one
            if (_allMessages.Count > 3)
            {
                _recentMessages.AddRange(_allMessages.GetRange(0, 3));
                _oldtMessages.AddRange(_allMessages.GetRange(3, _allMessages.Count - 3));
            }
            else
            {
                _recentMessages = _allMessages;
            }



            // pass all objects to the view
            var model = new MessageListModel() { TripMessages = _tripMessages, RecentMessages = _recentMessages, OldMessages = _oldtMessages };

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
            if (_model.TripId == 0)
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
                    BlViewTrip trip = null;
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
            if (!string.IsNullOrEmpty(replyMessage))
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
                return RedirectToAction("Message", "Admin", new { @id = threadId });
            }


            if (tripId != 0)
            {
                return RedirectToAction("TripPlanning");
            }

            return RedirectToAction("MessageCenter");
        }

    }
}