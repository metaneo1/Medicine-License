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
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index()
        {
            return View();
            //if (User.Identity.IsAuthenticated && Session[InPageAutorizeController.UI_ROLES_KEY] == null)
            //{
            //    try
            //    {
            //        var entities = new MedlicenseEntities();
            //        var id = User.Identity.GetUserId();
            //        var dictionary = entities
            //            .AspNetUsers.Where(x => x.Id == id)
            //            .SelectMany(x => x.AspNetUserRoles)
            //            .Select(x => x.AspNetRoles)
            //            .SelectMany(x => x.RoleUIControl)
            //            .Select(x => new
            //            {
            //                UIColntol = x.UIColntol.Name,
            //                x.Id_AccessType
            //            })
            //            .ToList()
            //            .ToLookup(x => x.UIColntol, x => x.Id_AccessType);

            //        Session[InPageAutorizeController.UI_ROLES_KEY] = dictionary;

            //        Session[InPageAutorizeController.DB_SESSION_KEY] = Guid.NewGuid().ToString();
            //        var userSession = new UsersSessions()
            //        {
            //            Id_AspnetUser = User.Identity.GetUserId(),
            //            Session_ID = (string) Session[InPageAutorizeController.DB_SESSION_KEY],
            //            StartDate = DateTime.Now,
            //        };
            //        entities.UsersSessions.Add(userSession);
            //        entities.SaveChanges();
            //    }
            //    catch (Exception e)
            //    {
            //        AuthenticationManager.SignOut();
            //    }

            //}
            //return View();
        }
    }
}