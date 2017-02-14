using DomingoBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DomingoDAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : DomingoControllerBase
    {

        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult Index()
        {
            //if (ApplicationUserManager.IsTravelogyAdmin(User.Identity.Name))
            //{
            //    return RedirectToAction("Index", "Admin");
            //}

            if (Request.IsAuthenticated)
            {
                DomingoUserManager.TraceSession(User.Identity.GetUserName(), "/Home");
            }

            return View();
        }       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}