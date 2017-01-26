using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using DomingoBL.BlObjects;

namespace WebApplication1.Controllers
{
    public partial class ServiceController : DomingoControllerBase
    {
        // GET: Service
        public ActionResult Index()
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
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}