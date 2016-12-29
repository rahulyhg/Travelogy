using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

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
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateThread(MessageViewModel _model)
        {
            return RedirectToAction("MessageCenter");
        }
    }
}