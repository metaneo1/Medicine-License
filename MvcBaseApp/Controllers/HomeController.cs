using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace MvcBaseApp.Controllers
{
    public class HomeController : Controller
    {
        MedlicenseEntities entities = new MedlicenseEntities();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View(model: null);
            var model = new HomeIndexModel();
            model.RequestStates = entities.RequestState.ToList();
            return View(model);
        }
    }

    public class HomeIndexModel
    {
        public List<RequestState> RequestStates { get; set; }
    }
}