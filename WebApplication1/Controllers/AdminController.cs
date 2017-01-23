using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models;

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

            return View();
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