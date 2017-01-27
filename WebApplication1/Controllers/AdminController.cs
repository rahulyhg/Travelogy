using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using DomingoBL.BlObjects;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminController : DomingoControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private void _CheckForAdminAccess()
        {
            if (!ApplicationUserManager.IsTravelogyAdmin(User.Identity.Name))
            {
                throw new ApplicationException("Unauthorized access of admin feature!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            _CheckForAdminAccess();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Destinations()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            return View(context.Destinations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TripTemplates()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            return View(context.TripTemplates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Trips()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            return View(context.Trips);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateTripTemplate()
        {
            _CheckForAdminAccess();

            var _tripTemmplate = new TripTemplate();
            return View(_tripTemmplate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripTemplate(int id)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _tripTemmplate = context.TripTemplates.Find(id);
            return View(_tripTemmplate);
        }
                

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SaveTripTemplate(TripTemplate template)
        {
            _CheckForAdminAccess();

            if(template != null)
            {
                using (var context = new TravelogyDevEntities1())
                {
                    var _tripTemmplate = context.TripTemplates.Find(template.Id);
                    _tripTemmplate.Description = template.Description;
                    context.SaveChanges();
                }
            }                      

            return RedirectToAction("TripTemplates");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateStep"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SaveTripTemplateStep(TripTemplateStep templateStep)
        {
            _CheckForAdminAccess();

            if (templateStep != null)
            {
                using (var context = new TravelogyDevEntities1())
                {
                    var _tripTemmplateStep = context.TripTemplateSteps.Where(p => p.TripTemplateId == templateStep.TripTemplateId 
                            && p.TripTemplateStepIdentifier == templateStep.TripTemplateStepIdentifier).FirstOrDefault();

                    if(_tripTemmplateStep != null)
                    {
                        _tripTemmplateStep.ShortDescription = templateStep.ShortDescription;
                        _tripTemmplateStep.LongDescription = templateStep.LongDescription;
                        _tripTemmplateStep.NightStay = templateStep.NightStay;
                        context.SaveChanges();
                    }

                    if(string.IsNullOrEmpty(templateStep.TripTemplateStepIdentifier))
                    {
                        _tripTemmplateStep = new TripTemplateStep()
                        {
                            TripTemplateId = templateStep.TripTemplateId,
                            ShortDescription = templateStep.ShortDescription,
                            LongDescription = templateStep.LongDescription,
                            NightStay = templateStep.NightStay
                        };

                        var _blError = TripStepManager.CreateTriptemplateStep(_tripTemmplateStep);
                    }
                }
            }

            var context2 = new TravelogyDevEntities1();
            var _tripTemplate = context2.TripTemplates.Find(templateStep.TripTemplateId);
            return View("EditTripTemplate", _tripTemplate);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MessageCenter()
        {
            _CheckForAdminAccess();

            var _messageList = new List<MessageCollection>();
            var _blError = ThreadManager.GetMessagesForAdmin(out _messageList);
            var model = new MessageListModel() { AllMessages = _messageList };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TripManagement(int id)
        {
            _CheckForAdminAccess();

            BlTrip trip = null;
            var _blError = TripManager.Admin_GetTripById(id, out trip);
            
            return View(trip);
        }
    }
}