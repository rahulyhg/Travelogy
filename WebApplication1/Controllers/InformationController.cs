using DomingoBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InformationController : Controller
    {
        // GET: Information
        public ActionResult Index()
        {
            return View();
        }

        // GET: AboutUs
        public ActionResult AboutUs()
        {
            return View();
        }

        // GET: FAQ
        public ActionResult FAQ()
        {
            return View();
        }

        // GET: Privacy
        public ActionResult Privacy()
        {
            return View();
        }

        // GET: Philosophy
        public ActionResult Philosophy()
        {
            return View();
        }

        // GET: Contact Us
        public ActionResult ContactUs()
        {
            return View();
        }

        // GET: Contact Us form submisison thanks
        public ActionResult ContactUsThanks()
        {
            return View();
        }

        /// <summary>
        /// Contact us form submit - 
        ///     a. Send a thank you mail to customer
        ///     b. Create a lead in CRM
        /// </summary>
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="EMAIL"></param>
        /// <param name="TRIP_REQUEST"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUsFormSubmitAsync(string FIRST_NAME, string LAST_NAME, string EMAIL, string TRIP_REQUEST)
        {
            string test = FIRST_NAME;
            test = LAST_NAME;
            test = EMAIL;
            test = TRIP_REQUEST;

            var blError = await UserManager.CreateCrmLead(FIRST_NAME, LAST_NAME, EMAIL, TRIP_REQUEST);

            return RedirectToAction("ContactUsThanks");
        }

        // GET: TermsAndConditions
        public ActionResult TermsAndConditions()
        {
            return View();
        }        
    }
}