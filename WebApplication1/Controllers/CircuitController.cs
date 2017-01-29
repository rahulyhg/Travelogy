using DomingoBL;
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
        // GET: Circuit - get all circuits
        public ActionResult Index()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetAllDestinations(out _destinations);
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "All Destinations" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for asia
        public ActionResult Asia()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForContinent("asia", out _destinations);
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Asia" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for Africa
        public ActionResult Africa()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForContinent("africa", out _destinations);
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Africa" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for Europe
        public ActionResult Europe()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForContinent("europe", out _destinations);
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Europe" };
            return View(_model);
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