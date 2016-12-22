using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}