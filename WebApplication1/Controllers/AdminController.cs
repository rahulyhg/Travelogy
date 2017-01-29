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
        public ActionResult TripProviders()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            return View(context.TripProviders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateTripProvider()
        {
            _CheckForAdminAccess();

            var _model = new TripProvider();
            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripProvider(int id)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _tripProvider = context.TripProviders.Find(id);
            return View(_tripProvider);
        }

        [Authorize]
        public ActionResult SaveTripProvider(TripProvider model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                using (var context = new TravelogyDevEntities1())
                {
                    if (model.Id == 0)
                    {
                        context.TripProviders.Add(model);
                        context.SaveChanges();
                    }

                    else
                    {
                        var _tripProvider = context.TripProviders.Find(model.Id);
                        _tripProvider.Address = model.Address;
                        _tripProvider.Description = model.Description;
                        _tripProvider.EmailAddressCustSupport = model.EmailAddressCustSupport;
                        _tripProvider.EmailAddressMarketingSales = model.EmailAddressMarketingSales;
                        _tripProvider.EmailAddressPrimary = model.EmailAddressPrimary;
                        _tripProvider.Name = model.Name;
                        _tripProvider.Telephone01 = model.Telephone01;
                        _tripProvider.Telephone02 = model.Telephone02;
                        _tripProvider.Telephone03 = model.Telephone03;
                        _tripProvider.Type = model.Type;
                        _tripProvider.Website = model.Website;

                        context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("TripTemplates");
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

            if (template != null)
            {
                using (var context = new TravelogyDevEntities1())
                {
                    if (template.Id > 0)
                    {
                        var _tripTemmplate = context.TripTemplates.Find(template.Id);
                        _tripTemmplate.Description = template.Description;
                        context.SaveChanges();
                    }

                    else
                    {
                        context.TripTemplates.Add(template);
                        context.SaveChanges();
                    }
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

                    if (_tripTemmplateStep != null)
                    {
                        _tripTemmplateStep.ShortDescription = templateStep.ShortDescription;
                        _tripTemmplateStep.LongDescription = templateStep.LongDescription;
                        _tripTemmplateStep.NightStay = templateStep.NightStay;
                        context.SaveChanges();
                    }

                    if (string.IsNullOrEmpty(templateStep.TripTemplateStepIdentifier))
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

            var _messageList = new List<Thread>();
            var _blError = ThreadManager.GetMessagesForAdmin(out _messageList);
            //var model = new MessageListModel() { AllMessages = _messageList };

            return View(_messageList);
        }

        [Authorize]
        public ActionResult Message(int id)
        {
            var _messageList = new MessageCollection();
            var _blError = ThreadManager.GetMessageThreadById(id, out _messageList);

            var _model = new MessageListModel();
            _model.AllMessages = new List<MessageCollection>();
            _model.AllMessages.Add(_messageList);

            return View(_model);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TripBookingAccommodations()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.TripBookingAccommodations;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripBookingAccommodation(int id)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _model = context.TripBookingAccommodations.Find(id);
            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SaveTripBookingAccommodation(TripBookingAccommodation model)
        {
            _CheckForAdminAccess();

            if(model != null)
            {
                using (var context = new TravelogyDevEntities1())
                {
                    var _booking = context.TripBookingAccommodations.Find(model.Id);
                    if(_booking != null)
                    {
                        _booking.AccommodationType = model.AccommodationType;
                        _booking.CheckinDate = model.CheckinDate;
                        _booking.CheckoutDate = model.CheckoutDate;
                        _booking.EstimatedCost = model.EstimatedCost;
                        _booking.Notes = model.Notes;
                        _booking.PropertyAddress = model.PropertyAddress;
                        _booking.PropertyName = model.PropertyName;
                        _booking.SpecialRequests = model.SpecialRequests;
                        _booking.Status = model.Status;

                        context.SaveChanges();
                    }
                }
            }

            return View("EditTripBookingAccommodation", model.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TripBookingTransports()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.TripBookingTransports;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripBookingTransport(int id)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _model = context.TripBookingTransports.Find(id);
            return View(_model);
        }
    }
}