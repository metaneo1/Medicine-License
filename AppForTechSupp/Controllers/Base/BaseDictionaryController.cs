using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Interfaces;
using DevExpress.Data;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting.Shape;
using DevExpress.XtraReports.UI;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    public abstract class BaseDictionaryController<T> : BaseController<T> where T : class, IDictionaryEntity, new()
    {
        protected override object GetDataForExport()
        {
            return entities.Set<T>().ToList();
        }

        protected new void generator_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
            e.ColumnsInfo[1].ColumnWidth *= 2;
            //e.ColumnsInfo[4].ColumnWidth += 30;
            e.ColumnsInfo[2].ColumnWidth -= 30;
            //e.ColumnsInfo[e.ColumnsInfo.Count - 1].IsVisible = false;
        }

        protected virtual string AddEditWrapperName { get { return "BaseDictionary/AddEdit"; } }
        public ActionResult GetLookUp(string JsElementName, string CallerConrtoller, int? CurrentValue, LookUpReloadButton buttons = LookUpReloadButton.All)
        {
            var list = entities.Set<T>().ToList().Select(x => (IEntityWithId) x).ToList();
            var model = new LookUpReloadModel<IEntityWithId>()
            {
                CurrentValue = CurrentValue,
                ControllerName = GetControllerName(),
                CallerConrtoller = CallerConrtoller,
                Buttons = buttons,
                //This method name
                //Needed for path generation  
                ActionName = "GetLookUp",
                //Id + [Dictionary Entity (Country)] + [View (Add)] + [Complex Entity (Worker)]
                //Needed for unique variable names
                JsElementName = JsElementName,
                //Cast to object is needed for generic lookup control _LookUpReload
                List = list,
            };
            return View("_LookUpReload", model);
        }
        protected override void FillModel(IndexGridModel<List<T>> model)
        {

        }
        public new ActionResult Index(bool isDebug = false)
        {
            return base.Index(isDebug, "BaseDictionary/Index");
        }

        public new ActionResult IndexPartial()
        {
            return base.IndexPartial();
        }

        public virtual ActionResult AddEdit(int id, bool isDebug = false)
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
            return View(AddEditWrapperName, t);
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
        public virtual string AddEdit(FormCollection formData)
        {
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

    //public interface IParsable
    //{
    //    IParsable Parse(FormCollection formData);
    //}

    //public interface IDictionaryEntity : IEntityWithId, IParsable
    //{
    //    string Name { get; set; }
    //    string Code { get; set; }
    //    string Description { get; set; }
    //}

    //public interface IEntityWithId
    //{
    //    int Id { get; set; }
    //}
}