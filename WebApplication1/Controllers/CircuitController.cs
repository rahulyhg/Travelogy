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

        public ActionResult Circuits()
        {
            return View();
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

        public ActionResult HistoricBritain()
        {
            return View();
        }
    }
}