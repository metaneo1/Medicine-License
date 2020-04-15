using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;
using DataModel.Interfaces;
namespace MvcBaseApp.Controllers
{
    public abstract class BaseController<T> : EntityControllerController<T> where T : class, IEntityWithId, new()
    {
        public string GetDeleteText(int id)
        {
            var t = entities.Set<T>().FirstOrDefault(x => x.Id == id);
            if (t == null)
            {
                return "";
            }
            return Labels.ShureForRemoveEntry;
        }
	}
}