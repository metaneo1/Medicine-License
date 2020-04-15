using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    public class ApplicationUserController : BaseDictionaryController<ApplicationUser>
    {
        protected override string AddEditWrapperName { get { return "AddEdit"; } }


        public override ActionResult AddEdit(int userId = 0, bool isDebug = false)
        {
            ViewBag.AJAX = isDebug;
            ApplicationUser t;
            if (!User.Identity.IsAuthenticated)
            {
                t = new ApplicationUser();
            }
            else
            {
                var id = GetCurrentUser();
                t = entities.ApplicationUser.FirstOrDefault(x => x.Id == id);
            }

            return View(AddEditWrapperName, t);
        }


        [HttpPost]
        public override string AddEdit(FormCollection formData)
        {
            var t = (ApplicationUser)new ApplicationUser().Parse(formData);
            if (t.Id == 0)
                return base.AddEdit(formData);
            //t.AspUserId = User.Identity.GetUserId();
            entities.ApplicationUser.Attach(t);
            entities.Entry(t).State = EntityState.Modified;
            entities.SaveChanges();
            return t.Id.ToString();
        }
    }
}
