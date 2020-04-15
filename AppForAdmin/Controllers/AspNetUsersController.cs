using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBaseApp.Models;

namespace MvcBaseApp.Controllers
{
    //public class AspNetUsersController : EntityControllerController<AspNetUsers>
    //{
    //    public ActionResult Index(bool isDebug = false)
    //    {
    //        return base.Index(isDebug);
    //    }

    //    protected override object GetDataForExport()
    //    {
    //        return entities.AspNetUsers.ToList();
    //    }

    //    public new ActionResult IndexPartial()
    //    {
    //        return base.IndexPartial();
    //    }

    //    public ActionResult AddEdit(string id, bool isDebug = false)
    //    {
    //        ViewBag.AJAX = isDebug;
    //        AspNetUsers t = null;
    //        if (id == "0")
    //        {
    //            t = new AspNetUsers() { Id = "0" };
    //        }
    //        else
    //        {
    //            t = entities.Set<AspNetUsers>().FirstOrDefault(x => x.Id == id);
    //        }
    //        return View(t);
    //    }

    //    protected override void FillModel(IndexGridModel<AspNetUsers> model)
    //    {

    //    }
    //}
}