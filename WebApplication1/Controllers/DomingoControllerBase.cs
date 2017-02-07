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
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

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