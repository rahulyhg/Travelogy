using DomingoBL;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DomingoControllerBase : Controller
    {
        protected void _CheckForAdminAccess()
        {
            string userType = ApplicationUserManager.GetUserType(User.Identity.Name).ToLower().Trim();
            if (userType == "admin" || userType == "traveloger" || userType == "editor")
            {
                // things are just fine
            }
            else
            {
                throw new ApplicationException("Unauthorized access of admin feature!");
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var _logger = NLog.LogManager.GetCurrentClassLogger();
            _logger.Error(filterContext.Exception);
            // Redirect on error:
            filterContext.Result = RedirectToAction("Error", "Home");
        }

        protected static void _GetTagsForDestination(Destination destination, out DomingoBlError blError, out List<View_TagDestination> tags)
        {
            tags = null;
            blError = TagManager.GetTagsForDestination(destinationId: destination.Id, tags: out tags);
            if (blError.ErrorCode != 0)
            {
                throw new ApplicationException(blError.ErrorMessage);
            }
        }
    }
}