﻿using DomingoBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Configuration;

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

        // GET: Cookies
        public ActionResult Cookies()
        {
            return View();
        }

        // GET: HideCookieWarning
        public ActionResult HideCookieWarning(string returnUrl)
        {
            Session["HideCookieWarning"] = "true";
            return Redirect(returnUrl);         
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
        public async Task<ActionResult> ContactUsFormSubmitAsync(string FIRST_NAME, string LAST_NAME, string EMAIL, string TRIP_REQUEST, string PHONE)
        {
            var blError = await DomingoUserManager.CreateCrmLeadExternal(FIRST_NAME, LAST_NAME, EMAIL, PHONE, TRIP_REQUEST);

            return RedirectToAction("ContactUsThanks");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="EMAIL"></param>
        /// <param name="CIRCUIT"></param>
        /// <param name="PHONE"></param>
        /// <param name="TRIP_REQUEST"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitTripRequestAsync(string FIRST_NAME, string LAST_NAME, string EMAIL, string CIRCUIT, string PHONE, string TRIP_REQUEST)
        {
            TRIP_REQUEST = string.Format("Circuit requested for: {0} -- User comment: {1}", CIRCUIT, TRIP_REQUEST);

            var blError = await DomingoUserManager.CreateCrmLeadExternal(FIRST_NAME, LAST_NAME, EMAIL, PHONE, TRIP_REQUEST);

            return RedirectToAction("ContactUsThanks");
        }

        // GET: TermsAndConditions
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DownloadBrochureRequestAsync(_DownloadBrochureRequestModel model)
        {
            // create a CRM lead
            var brochurePath = ConfigurationManager.AppSettings["HostSite"] + model.BrochurePath;
            var blCrm = await DomingoUserManager.CreateCrmLeadDownloadBrochure(model.Name, model.Email, brochurePath);
            ViewBag.ConfirmationMessage = "Your brochure has been emailed to you.";
            return View("ContactUsThanks");            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CallMeBackMicroAsync(CallMeBackViewModel model)
        {
            // create a CRM lead
            var blCrm = await DomingoUserManager.CreateCrmLeadCallMeBack(model.WhomToCall, model.WhereToCall);
            ViewBag.ConfirmationMessage = "Our agents will call you back as soon as possible.";
            return View("ContactUsThanks");
        }
    }
}