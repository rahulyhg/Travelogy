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
    public partial class IndiaController : DomingoControllerBase
    {       

        // all indian destinations here
        public ActionResult EasternHimalaya()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("EasternHimalaya", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Trips in Eastern Himalayas", Destination = destination, DestinationTags = tags };
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
            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips in Western Himalayas", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult GangeticPlains()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("GangeticPlains", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips in the Gangetic Plains", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult GoldenTriangle()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("GoldenTriangle", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips in the Golden Triangle", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult IndianWestCoast()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("IndianWestCoast", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips in the Indian West Coast", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult DeccanPlataue()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("DeccanPlataue", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips in the Deccan Plataue", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult SouthernIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("SouthernIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);

            var model = new DomingoModelBase() { PageName = "Trips in the Southern India", Destination = destination, DestinationTags = tags };
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

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Photography Tours in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult MotorcycleToursIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("MotorcycleToursIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Motorcycle Tours in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult WildlifeToursIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("WildlifeToursIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Wildlife Tours in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult AdventureToursIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("AdventureToursIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Adventure Tours in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult UrbanAdventuresIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("UrbanAdventuresIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Urban Adventures in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult RuralTourismIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("RuralTourismIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Rural tours in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult SpiritualTourismIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("SpiritualTourismIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Spiritual Tourism in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }

        public ActionResult FestivalsOfIndia()
        {
            var destination = new Destination();
            var blError = DestinationManager.GetDestinationForAlias("FestivalsOfIndia", out destination);
            if (blError.ErrorCode != 0 || destination == null)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }

            // get all the tags for this destination
            List<View_TagDestination> tags;
            _GetTagsForDestination(destination, out blError, out tags);
            var model = new DomingoModelBase() { PageName = "Festivals in India", Destination = destination, DestinationTags = tags };
            return View(model);
        }
    }
}