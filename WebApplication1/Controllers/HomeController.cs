using DomingoBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DomingoDAL;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.IO;

namespace WebApplication1.Controllers
{
    public class HomeController : DomingoControllerBase
    {

        public ActionResult Error()
        {
            var _logger = NLog.LogManager.GetCurrentClassLogger();
            _logger.Info(String.Format("_getMeOutofHere: URL:{0}", Request.RawUrl));
            
            return View("Error");
        }

        public ActionResult Error404()
        {
            return View("Error404");
        }

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                DomingoUserManager.TraceSession(User.Identity.GetUserName(), "/Home");
            }

            return View();
        }       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}