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
            string userType = ApplicationUserManager.GetUserType(User.Identity.Name).ToLower().Trim();
            if(userType == "admin" || userType == "traveloger" || userType == "editor")
            {
                // things are just fine
            }
            else
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
        public ActionResult Users()
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            return View(context.View_User);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspUserId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult UserDetails(string userId)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.View_User.Where(p => p.Id == userId).FirstOrDefault();
            if(model != null)
            {
                return View(model);
            }

            throw new ApplicationException("User not found");
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
        public ActionResult CreateDestination()
        {
            _CheckForAdminAccess();

            var _model = new Destination();
            return View(_model);
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
            var _model = new EditDestinationViewModel
            {
                DbObject = context.Destinations.Find(id),
                SubDestinations = context.SubDestinations.Where(p => p.DestinationId == id),
                Interests = context.DestinationInterests.Where(p => p.DestinationId == id),
                Activities = context.DestinationActivities.Where(p => p.DestinationId == id),
                CostObjects = context.DestinationCosts.Where(p => p.DestinationId == id)
            };

            return View(_model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveDestinationAsync(EditDestinationViewModel model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveDestination(model.DbObject);
            }

            return RedirectToAction("EditDestination", new { @id = model.DbObject.Id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddSubDestination(int destinationId)
        {
            _CheckForAdminAccess();

            var model = new SubDestination() { Id = 0, DestinationId = destinationId };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subDestinationId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditSubDestination(int subDestinationId)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.SubDestinations.Find(subDestinationId);
            if(model == null)
            {
                throw new ApplicationException("Invalid parameter");
            }

            return View("AddSubDestination", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveSubDestinationAsync(SubDestination model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveSubDestination(model);
            }

            return RedirectToAction("EditDestination", new { @id = model.DestinationId });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddDestinationCost(int destinationId)
        {
            _CheckForAdminAccess();

            var model = new DestinationCost() { Id = 0, DestinationId = destinationId };
            return View("EditDestinationCost", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationCostId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditDestinationCost(int destinationCostId)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.DestinationCosts.Find(destinationCostId);
            if (model == null)
            {
                throw new ApplicationException("Invalid parameter");
            }

            return View("EditDestinationCost", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveDestinationCostAsync(DestinationCost model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveDestinationCost(model);
            }

            return RedirectToAction("EditDestination", new { @id = model.DestinationId });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddDestinationActivity(int destinationId)
        {
            _CheckForAdminAccess();

            var model = new DestinationActivity() { DestinationId = destinationId };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditDestinationActivity(int destinationActivityId)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.DestinationActivities.Find(destinationActivityId);
            if (model == null)
            {
                throw new ApplicationException("Invalid parameter");
            }

            return View("AddDestinationActivity", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveDestinationActivityAsync(DestinationActivity model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveDestinationActivity(model);
            }

            return RedirectToAction("EditDestination", new { @id = model.DestinationId });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddDestinationInterest(int destinationId)
        {
            _CheckForAdminAccess();

            var model = new DestinationInterest() { DestinationId = destinationId };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditDestinationInterest(int destinationInterestId)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var model = context.DestinationActivities.Find(destinationInterestId);
            if (model == null)
            {
                throw new ApplicationException("Invalid parameter");
            }

            return View("AddDestinationInterest", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> SaveDestinationInterestAsync(DestinationInterest model)
        {
            _CheckForAdminAccess();

            if (model != null)
            {
                await AdminUtility.SaveDestinationInterest(model);
            }

            return RedirectToAction("EditDestination", new { @id = model.DestinationId });

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

        [Authorize]
        public ActionResult AddTripStep(int tripId)
        {
            _CheckForAdminAccess();

            View_TripStep step = new View_TripStep() { TripId = tripId };
            return View("EditTripStep", step);
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
        /// <param name="tripId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripBookingTransport(int id, int tripId = 0)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _model = new AdminTripBookingTransportEditModel()
            {
                TripId = tripId,
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

            if(model.TripId > 0)
            {
                return RedirectToAction("TripManagement", new { @id = model.TripId });
            }

            else
            {
                return RedirectToAction("TripBookingTransports");
            }
            
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
        /// <param name="tripId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTripBookingAccommodation(int id, int tripId = 0)
        {
            _CheckForAdminAccess();

            var context = new TravelogyDevEntities1();
            var _model = new AdminTripBookingAccommodationEditModel()
            {
                TripId = tripId,
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

            if (model.TripId == 0)
            {
                return RedirectToAction("TripBookingAccommodations");
            }
            else
            {
                return RedirectToAction("TripManagement", new { @id = model.TripId });
            }
        }        
    }
}