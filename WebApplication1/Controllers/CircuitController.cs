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
    public class CircuitController : Controller
    {
        // GET: Circuit
        public ActionResult Index()
        {
            var _dataController = new DomingoDataController();
            var _allDestinations = _dataController.GetAllDestinations();
            CircuitModelBase circuitModel = new CircuitModelBase() { AllDestinations = _allDestinations };
            return View(circuitModel);
        }

        public ActionResult Kilimanjaro()
        {
            return View();
        }

        public ActionResult TanzaniaSafari()
        {
            return View();
        }

        public ActionResult TransSiberian()
        {
            return View();
        }

        public ActionResult ScottishIsles()
        {
            return View();
        }

        public ActionResult MotoEasternHimalaya()
        {
            return View();
        }

        public ActionResult BackpackEasternHimalaya()
        {
            return View();
        }

        public ActionResult PhotographyToursIndia()
        {
            return View();
        }

        public ActionResult LakeBaikal()
        {
            return View();
        }

        public ActionResult TransMongolian()
        {
            return View();
        }

        public ActionResult Mongolia()
        {
            return View();
        }

        public ActionResult Tibet()
        {
            return View();
        }

        public ActionResult SupercarSuperbikeBritain()
        {
            return View();
        }
    }
}