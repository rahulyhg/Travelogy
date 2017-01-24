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
    /// <summary>
    /// 
    /// </summary>
    public class AdminController : DomingoControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private void _CheckForAdminAccess()
        {
            if (!ApplicationUserManager.IsTravelogyAdmin(User.Identity.Name))
            {
                throw new ApplicationException("Unauthorized access of admin feature!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            _CheckForAdminAccess();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MessageCenter()
        {
            _CheckForAdminAccess();

            var _messageList = new List<MessageCollection>();
            var _blError = ThreadManager.GetMessagesForAdmin(out _messageList);
            var model = new MessageListModel() { AllMessages = _messageList };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TripManagement()
        {
            _CheckForAdminAccess();

            return View();
        }
    }
}