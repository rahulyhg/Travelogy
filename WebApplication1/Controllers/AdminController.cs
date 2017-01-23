using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    public class AdminController : DomingoControllerBase
    {
        [Authorize]
        public ActionResult Index()
        {
            if(!ApplicationUserManager.IsTravelogyAdmin(User.Identity.Name))
            {
                throw new ApplicationException("Unauthorized access of admin feature!");
            }

            return View();
        }

        [Authorize]
        public ActionResult MessageCenter()
        {
            if (!ApplicationUserManager.IsTravelogyAdmin(User.Identity.Name))
            {
                throw new ApplicationException("Unauthorized access of admin feature!");
            }

            var _messageList = new List<MessageCollection>();
            var _blError = ThreadManager.GetMessagesForAdmin(out _messageList);
            var model = new MessageListModel() { AllMessages = _messageList };

            return View(model);
        }

        [Authorize]
        public ActionResult TripManagement()
        {
            if (!ApplicationUserManager.IsTravelogyAdmin(User.Identity.Name))
            {
                throw new ApplicationException("Unauthorized access of admin feature!");
            }

            return View();
        }
    }
}