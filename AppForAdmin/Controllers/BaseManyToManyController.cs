using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;
using DataModel.Interfaces;
namespace MvcBaseApp.Controllers
{
    public abstract class BaseManyToManyController<T, ParentType> : EntityControllerController<T> where T : class, IEntityWithId, IParsable, new()
                                                                                                  where ParentType : class, IEntityWithId, IParsable, new()
    {

        protected abstract bool ParseInnerParametersForIndex();
        protected abstract void PrepareViewBagForIndex();
        protected abstract bool ParseInnerParametersForIndexPartial();
        protected abstract void PrepareViewBagForIndexPartial();
        protected abstract bool ParseInnerParametersForAddEdit();
        protected abstract void PrepareViewBagForAddEdit();
        protected abstract T InitNewItem();
        protected abstract IModel<T> CreateModel();

        protected override object GetDataForExport()
        {
            ParseInnerParametersForIndexPartial();
            return entities.Set<T>().Where(GetFilterForParentEntity()).ToList();
        }
        protected abstract System.Linq.Expressions.Expression<Func<T, bool>> GetFilterForParentEntity();

        //Пока думаю, как сделать более универсальную штуку на шаблонах, чтоб можно было вообще клёво обращаться из справочника
        //Пока что склоняюсь к чему-то вроде return Parse_Id_Person() || Parse_Id_Country() || Parse_Id_AbroadTripReason();
        //Так можно будет сразу распарсить все нужные параметры и использовать один ManyToManyController и одни и те же вьюшки для захода с разных сторон
        //Например, елси передали в него Id_Country - он вернул все поездки в эту страну, Id_Person - все поездки этого человека и тд
        //Скорее всего это будет ещё один абстрактный метод
        public ActionResult Index(bool isDebug = false)
        {
            if (!ParseInnerParametersForIndex())
            {
                return null;
            }
            ViewBag.AJAX = isDebug;
            PrepareViewBagForIndex();
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View(model);
        }

        public new ActionResult IndexPartial()
        {
            if (!ParseInnerParametersForIndexPartial())
            {
                return null;
            }
            PrepareViewBagForIndexPartial();
            var model = CreateIndexModel();
            DevExpressHelper.Theme = Startup.THEME;
            return View("_Index", model);
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
        public string GetDeleteText(int id)
        {
            var t = entities.Set<T>().FirstOrDefault(x => x.Id == id);
            if (t == null)
            {
                return "";
            }
            return Labels.ShureForRemoveEntry;
        }

        public ActionResult AddEdit(int id, bool isDebug = false)
        {
            if (!ParseInnerParametersForAddEdit())
            {
                return null;
            }
            ViewBag.AJAX = isDebug;
            PrepareViewBagForAddEdit();
            
            T t = null;
            if (id == 0)
            {
                t = InitNewItem(); 
            }
            else
            {
                t = entities.Set<T>().FirstOrDefault(x => x.Id == id);
            }

            ViewBag.MODEL = CreateModel();

            return View(t);
        }

        [HttpPost]
        public ActionResult AddEdit(FormCollection formData)
        {
            var t = (T)new T().Parse(formData);

            if (t.Id == 0)
            {
                entities.Set<T>().Add(t);
            }
            else
            {
                entities.Entry(t).State = EntityState.Modified;
            }
            entities.SaveChanges();
            return null;
        }
	}

    public interface IModel<T> where T : class, new()
    {

    }
}