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
        //[Guid("9FF35F56-5324-4B94-8038-EA9C10C3EB76")]
        public ActionResult HB9FF35F56_5324_4B94_8038_EA9C10C3EB76()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetAllDestinations(out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "All Destinations" };
            return View("Index", _model);
        }

        // GET: Circuit - get Destination        
        public ActionResult Destination(string destination)
        {
            List<SubDestination> _subDestinationList = null;
            var _blerror = DestinationManager.SearchSubDestination(destination, out _subDestinationList);

            if(_blerror.ErrorCode != 0 || _subDestinationList == null || _subDestinationList.Count == 0) 
            {
                return View("Error404");
            }

            var _model = new CircuitDestinationViewModel() { Name = destination, SubDestinations = _subDestinationList };
            return View(_model);
        }

        // GET: Circuit - get Interest        
        public ActionResult Interest(string interest)
        {
            var _model = new CircuitDestinationViewModel() { Name = interest };
            return View(_model);
        }

        // GET: Circuit - get Activity        
        public ActionResult Activity(string activity)
        {
            var _model = new CircuitDestinationViewModel() { Name = activity };
            return View(_model);
        }

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


        // GET: Circuit - get all circuits for Mongolia
        public ActionResult Mongolia()
        {
            // get the destination object
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("Mongolia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips to Mongolia", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        // GET: Circuit - get all circuits for Russia
        public ActionResult Russia()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForCountry("Russia", out _destinations);
            if (blError.ErrorCode != 0 || _destinations == null || _destinations.Count == 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            var _model = new CircuitModelBase() { AllDestinations = _destinations, CircuitName = "Asia" };
            return View(_model);
        }

        // GET: Circuit - get all circuits for Tanzania
        public ActionResult Tanzania()
        {
            List<Destination> _destinations = null;
            var blError = DestinationManager.GetDestinationsForCountry("Tanzania", out _destinations);
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
            // get the destination object
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("ScottishIsles", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags = null;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Trips to the Scottish Isles", Destination = destination, DestinationTags = tags};

            return View(model);
        }

        
        public ActionResult LakeBaikal()
        {
            // get the destination object
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("LakeBaikal", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips to Lake Baikal", Destination = destination, DestinationTags = tags };
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