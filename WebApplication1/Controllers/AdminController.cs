using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
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
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditDestination(int id)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _model = context.Destinations.Find(id);
            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateDestination()
        {
            _CheckForAdminAccess();

            var _model = new Destination();
            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveDestinationAsync(Destination model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveDestination(model);
            }

            return RedirectToAction("Destinations");

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
            return View(context.TripTemplates.OrderBy(p => p.Id));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripAsync(BlTrip model)
        {
            _CheckForAdminAccess();

            var _blError = await AdminUtility.SaveTrip(model.DlTrip);

            return RedirectToAction("Trips");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripStepAsync(View_TripStep model)
        {
            _CheckForAdminAccess();

            var _blError = await AdminUtility.SaveTripStep(model);

            return RedirectToAction("TripManagement", new { @id = model.TripId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripProviderAsync(TripProvider model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveTripProvider(model);
            }

            return RedirectToAction("TripProviders");
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
            var model = new AdminEditTripTemplateViewModel()
            {
                TripTemplate = _tripTemmplate,
                DestinationList = AdminModel.GetDestinationsSelectList(),
                TripProviderList = AdminModel.GetTripProvidersSelectList()
            };
            return View(model);
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
            if(_tripTemmplate == null)
            {
                RedirectToAction("CreateTripTemplate");
            }

            var model = new AdminEditTripTemplateViewModel()
            {
                TripTemplate = _tripTemmplate,
                DestinationList = AdminModel.GetDestinationsSelectList(),
                TripProviderList = AdminModel.GetTripProvidersSelectList()
            };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripTemplateAsync(AdminEditTripTemplateViewModel model)
        {
            _CheckForAdminAccess();

            await AdminUtility.SaveTripTemplate(model.TripTemplate);

            return RedirectToAction("TripTemplates");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateStep"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripTemplateStepAsync(TripTemplateStep templateStep)
        {
            _CheckForAdminAccess();

            if (templateStep != null)
            {
                await AdminUtility.SaveTripTemplateStep(templateStep);
            }

            return RedirectToAction("EditTripTemplate", new { @id = templateStep.TripTemplateId });

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripStep(int id)
        {
            _CheckForAdminAccess();

            View_TripStep step = null;
            var _blError = TripManager.Admin_GetTripStepById(id, out step);

            if(_blError.ErrorCode > 0)
            {
                throw new ApplicationException(_blError.ErrorMessage);
            }

            return View(step);
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
            var model = context.View_TripBookingTransport;
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
            var _model = new AdminTripBookingTransportEditModel()
            {
                DbObject = context.View_TripBookingTransport.Where(p => p.Id == id).FirstOrDefault()
            };

            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripBookingTransportAsync(AdminTripBookingTransportEditModel model)
        {
            _CheckForAdminAccess();

            var _blError = await AdminUtility.SaveTripBookingTransport(model.DbObject);

            return RedirectToAction("TripManagement", new { @id = model.DbObject.TripId });
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
            var model = context.View_TripBookingAccommodation;
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
            var _model = new AdminTripBookingAccommodationEditModel()
            {
                DbObject = context.View_TripBookingAccommodation.Where(p => p.Id == id).FirstOrDefault()
            };

            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveTripBookingAccommodationAsync(AdminTripBookingAccommodationEditModel model)
        {
            _CheckForAdminAccess();

            var _blError = await AdminUtility.SaveTripBookingAccommodation(model.DbObject);

            return RedirectToAction("TripBookingAccommodations");
        }

    }
}