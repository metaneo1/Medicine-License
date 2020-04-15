using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBaseApp.Models;

namespace MvcBaseApp.Controllers
{
    //public class AspNetRolesController : EntityControllerController<AspNetRoles>
    //{
    //    protected override void FillModel(IndexGridModel<AspNetRoles> model)
    //    {
    //    }

    //    protected override object GetDataForExport()
    //    {
    //        return entities.AspNetRoles.ToList();
    //    }

    //    public ActionResult Index(bool isDebug = false)
    //    {
    //        return base.Index(isDebug);
    //    }

    //    public new ActionResult IndexPartial()
    //    {
    //        return base.IndexPartial();
    //    }

    //    public ActionResult AddEdit(string id, bool isDebug = false)
    //    {
    //        ViewBag.AJAX = isDebug;
    //        AspNetRoles t = null;
    //        if (id == "0")
    //        {
    //            t = new AspNetRoles();
    //            t.Id = "0";
    //        }
    //        else
    //        {
    //            t = entities.Set<AspNetRoles>().FirstOrDefault(x => x.Id == id);
    //        }
    //        return View(t);
    //    }

    //    [HttpPost]
    //    public string AddEdit(FormCollection formData)
    //    {
    //        var t = (AspNetRoles)new AspNetRoles().Parse(formData);
    //        if (t.Id == "0")
    //        {
    //            t.Id = Guid.NewGuid().ToString();
    //            entities.Set<AspNetRoles>().Add(t);
    //        }
    //        else
    //        {
    //            entities.Set<AspNetRoles>().Attach(t);
    //            entities.Entry(t).State = EntityState.Modified;
    //        }
    //        entities.SaveChanges();
    //        return t.Id.ToString();
    //    }

    //    public ActionResult GetLookUp(string JsElementName, string CallerConrtoller, string CurrentValue, LookUpReloadButton buttons = LookUpReloadButton.All)
    //    {
    //        var list = entities.AspNetRoles.ToList();
    //        var model = new LookUpReloadModelString<AspNetRoles>()
    //        {
    //            CurrentValue = CurrentValue,
    //            ControllerName = GetControllerName(),
    //            CallerConrtoller = CallerConrtoller,
    //            Buttons = buttons,
    //            //This method name
    //            //Needed for path generation  
    //            ActionName = "GetLookUp",
    //            //Id + [Dictionary Entity (Country)] + [View (Add)] + [Complex Entity (Worker)]
    //            //Needed for unique variable names
    //            JsElementName = JsElementName,
    //            //Cast to object is needed for generic lookup control _LookUpReload
    //            List = list,
    //        };
    //        return View("_LookUpReloadString", model);
    //    }
    //}
}