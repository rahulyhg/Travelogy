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
    public class CircuitController : DomingoControllerBase
    {
        // GET: Circuit - get all circuits
        public ActionResult Index()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetAllDestinations(out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "All Destinations" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for India
        public ActionResult India()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForCountry("India", out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "India" };
            return View(_model);
        }

        // all indian destinations here
        public ActionResult EasternHimalaya()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("EasternHimalaya", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var model = new DomingoModelBase() { PageName = "Trips in Eastern Himalayas", Destination = destination };
            return View(model);
        }

        public ActionResult WesternHimalaya()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("WesternHimalaya", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Trips in Western Himalayas", Destination = destination };
            return View(model);
        }

        public ActionResult PhotographyToursIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("PhotographyToursIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Photography Tours in India", Destination = destination };
            return View(model);
        }

        // GET: Circuit - get all circuits for asia
        public ActionResult Asia()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForContinent("asia", out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Asia" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for Africa
        public ActionResult Africa()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForContinent("africa", out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Africa" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for Europe
        public ActionResult Europe()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForContinent("europe", out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Europe" };
            return View(_model);
        }

        public ActionResult Kilimanjaro()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("Kilimanjaro", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Kilimanjaro", Destination = destination };
            return View(model);
        }

        public ActionResult TanzaniaSafari()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("TanzaniaSafari", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Tanzania Safari", Destination = destination };
            return View(model);
        }

        public ActionResult TransSiberian()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("TransSiberian", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Trans-Siberian Trips", Destination = destination };
            return View(model);
        }

        public ActionResult ScottishIsles()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("ScottishIsles", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Trips to the Scottish Isles", Destination = destination };
            return View(model);
        }

        

        public ActionResult LakeBaikal()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("LakeBaikal", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Lake Baikal Trips", Destination = destination };
            return View(model);
        }

        public ActionResult Mongolia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("Mongolia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Mongolian Adventures", Destination = destination };
            return View(model);
        }

        public ActionResult Tibet()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("Tibet", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Tibet Trips", Destination = destination };
            return View(model);
        }

        public ActionResult SupercarSuperbikeBritain()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("SupercarBritain", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var model = new DomingoModelBase() { PageName = "Supercar & Superbike tours of Britain", Destination = destination };
            return View(model);
        }
    }
}