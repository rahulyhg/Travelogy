using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public partial class IndiaController : DomingoControllerBase
    {
        // GET: India
        public ActionResult Index()
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
    }
}