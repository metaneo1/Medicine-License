using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Interfaces;

namespace MvcBaseApp.Controllers
{
    public abstract class BaseMainFormController<T> : BaseController<T> where T : class, IEntityWithId, IParsable, new()
    {
        protected abstract T InitNewItem();
        protected abstract IModel<T> CreateModel();

        protected virtual void PrepareViewBagForIndex()
        {

        }

        public new ActionResult Index(bool isDebug = false)
        {
            PrepareViewBagForIndex();
            return base.Index(isDebug);
        }

        public new ActionResult IndexPartial()
        {
            return base.IndexPartial();
        }

        public ActionResult AddEdit(int id, bool isDebug = false)
        {
            ViewBag.AJAX = isDebug;
            T t = null;
            if (id == 0)
            {
                t = new T();
            }
            else
            {
                t = entities.Set<T>().FirstOrDefault(x => x.Id == id);
            }
            ViewBag.MODEL = CreateModel();
            return View(t);
        }

        public string Delete(int id)
        {
            var t = entities.Set<T>().FirstOrDefault(x => x.Id == id);
            if (t == null)
            {
                return "null";
            }
            entities.Set<T>().Remove(t);
            entities.SaveChanges();
            return "deleted";
        }

        [HttpPost]
        public string AddEdit(FormCollection formData)
        {
            //if (typeof(T) == typeof(Worker))
            //{
            //    return SaveWorker(formData);
            //}
            var t = (T)new T().Parse(formData);
            if (t.Id == 0)
            {
                entities.Set<T>().Add(t);
            }
            else
            {
                entities.Set<T>().Attach(t);
                entities.Entry(t).State = EntityState.Modified;
            }
            entities.SaveChanges();
            return t.Id.ToString();
        }

	}
}