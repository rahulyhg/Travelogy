using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BusinessController : DomingoControllerBase
    {
        // GET: Business
        public ActionResult Index()
        {
            var model = new BusinessContactViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BusinessContactUsFormSubmitAsync(BusinessContactViewModel model)
        {
            model.FormSubmittedSuccess = true;
            return View("Index", model);
        }
    }
}