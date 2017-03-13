using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    // Bing maps test key: 
    // Au_oubgAnV-h-Vd2AMHY5E5PySpNaaSBgGRKffDdL8qXFrzu7s1uyPZvXl6G2Knx

    /// <summary>
    /// 
    /// </summary>
    public class DomingoTripPlannerv2Controller : DomingoControllerBase
    {
        // GET: DomingoTripPlannerv2
        [Authorize]
        public ActionResult Index()
        {
            _CheckForAdminAccess();
            return View();
        }
    }
}