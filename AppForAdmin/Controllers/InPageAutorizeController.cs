using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DevExpress.Data.WcfLinq.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;
using ApplicationUser = MvcBaseApp.Models.ApplicationUser;

namespace MvcBaseApp.Controllers
{
    public class InPageAutorizeController : Controller
    {
        public const string DB_SESSION_KEY = "DB_SESSION";
        public const string UI_ROLES_KEY = "UI_ROLES_KEY";
        private MedlicenseEntities entities = new MedlicenseEntities();

        public InPageAutorizeController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public InPageAutorizeController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public ActionResult Login(bool isDebug = false)
        {
            ViewBag.AJAX = isDebug;
            return View("_Authorization");
        }

        [HttpPost]
        public string Login(FormCollection formData)
        {
            var model = new LoginViewModel();

            model.UserName = Convert.ToString(formData["UserName"]);
            model.Password = Convert.ToString(formData["Password"]);
            model.RememberMe = Convert.ToString(formData["RememberMe"]) == "C";
            try
            {
                var user = UserManager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    Session[DB_SESSION_KEY] = Guid.NewGuid().ToString();
                    //var userSession = new UsersSessions()
                    //{
                    //    Id_AspnetUser = user.Id,
                    //    Session_ID = (string)Session[DB_SESSION_KEY],
                    //    StartDate = DateTime.Now,
                    //};
                    //entities.UsersSessions.Add(userSession);
                    //entities.SaveChanges();
                    SignInAsync(user, model.RememberMe);


                    //var dictionary = new Dictionary<string, List<WebControlAccessType>>();

                    //var dictionary = entities
                    //    .AspNetUsers.Where(x => x.Id == user.Id)
                    //    .SelectMany(x => x.AspNetUserRoles)
                    //    .Select(x => x.AspNetRoles)
                    //    .SelectMany(x => x.RoleUIControl)
                    //    .Select(x => new
                    //    {
                    //        UIColntol = x.UIColntol.Name,
                    //        x.Id_AccessType
                    //    })
                    //    .ToList()
                    //    .ToLookup(x => x.UIColntol, x => x.Id_AccessType);

                    //Session[UI_ROLES_KEY] = dictionary;
                    return "OK:" + Session[DB_SESSION_KEY];
                }
                else
                {
                    return Labels.ErrorWrondPassword;
                }
            }
            catch (Exception e)
            {
                return Labels.ErrorNoDb;
            }
        }

        public string RegisterViaDesktop(FormCollection formData)
        {
            return "";
            //var login = DataTypeParser.String(formData["UserName"]);
            //var password = DataTypeParser.String(formData["Password"]);
            //var workerId = DataTypeParser.Int(formData["Id_Worker"]);
            //if (workerId == 0)
            //{
            //    workerId = DataTypeParser.Int(formData["Id_Worker_AddAspNetUsers_VI"]);
            //}
            //try
            //{
            //    var user = new ApplicationUser() { UserName = login };
            //    var result = UserManager.Create(user, password);
            //    if (result.Succeeded)
            //    {
            //        var aspnetUser = entities.AspNetUsers.FirstOrDefault(x => x.Id == user.Id);
            //        if (aspnetUser == null)
            //        {
            //            return Labels.ErrorNoDb;
            //        }
            //        aspnetUser.Id_Worker = workerId;
            //        entities.SaveChanges();
            //        return "OK:" + user.Id;
            //    }

            //    var errors = result.Errors.ToList();

            //    for (var i = 0; i < errors.Count; i++)
            //    {
            //        if (errors[i].StartsWith("User name") && errors[i].EndsWith("is invalid, can only contain letters or digits."))
            //        {
            //            errors[i] = "Имя пользователя " + login +
            //                        " некорректно, может содержать только латинские буквы и цифвы.";
            //        }

            //    }

            //    return "Error: " + string.Join(", ", errors);
            //    //return Labels.ErrorNoDb;
            //}
            //catch (Exception e)
            //{
            //    return "Error: " + Labels.ErrorNoDb;
            //}
        }

        [HttpPost]
        public string UpdateViaDesktop(FormCollection formData)
        {
            return "";
            //var userId = DataTypeParser.String(formData["Id"]);
            //var userName = DataTypeParser.String(formData["UserName"]);
            //var newPassword = DataTypeParser.String(formData["Password"]);

            //try
            //{
            //    var old = entities.AspNetUsers.FirstOrDefault(x => x.UserName == userName);
            //    old.PasswordHash = null;
            //    entities.SaveChanges();
            //    IdentityResult result = UserManager.AddPassword(userId, newPassword);

            //    var user = UserManager.Find(userName, newPassword);
            //    if (user != null)
            //    {
            //        return "OK:" + user.Id;
            //    }
            //    else
            //    {
            //        return Labels.ErrorWrondPassword;
            //    }
            //}
            //catch (Exception e)
            //{
            //    return Labels.ErrorNoDb;
            //}
        }


        private void SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }

    public enum WebControlAccessType
    {
        None = 0,
        View = 1,
        Enabled = 2,
    }
}