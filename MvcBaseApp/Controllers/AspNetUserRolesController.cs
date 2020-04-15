using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    //public class AspNetUserRolesController : Controller
    //{
    //    private HrmSessionEntities _entities = null;

    //    protected HrmSessionEntities entities
    //    {
    //        get
    //        {
    //            if (_entities == null)
    //            {
    //                _entities = new HrmSessionEntities((string)Session[InPageAutorizeController.DB_SESSION_KEY]);
    //            }
    //            return _entities;
    //        }
    //    }

    //    public string _Id_User;
    //    protected bool ParseInnerParametersForIndex()
    //    {
    //        return Parse_Id_User();
    //    }

    //    private bool Parse_Id_User()
    //    {
    //        _Id_User = Request.QueryString["Id_User"];
    //        if (string.IsNullOrWhiteSpace(_Id_User))
    //            return false;
    //        return true;
    //    }

    //    protected void PrepareViewBagForIndex()
    //    {
    //        ViewBag.AspNetUserRoles_Id_User = _Id_User;
    //        ViewBag.AspNetUserRoles_User_Name = entities.AspNetUsers.Where(x => x.Id == _Id_User).Select(x => x.UserName).FirstOrDefault();
    //    }

    //    protected bool ParseInnerParametersForIndexPartial()
    //    {
    //        return Parse_Id_User();
    //    }

    //    protected void PrepareViewBagForIndexPartial()
    //    {
    //        ViewBag.AspNetUserRoles_Id_User = _Id_User;
    //    }

    //    protected bool ParseInnerParametersForAddEdit()
    //    {
    //        return Parse_Id_User();
    //    }
    //    protected void PrepareViewBagForAddEdit() { }

    //    protected AspNetUserRoles InitNewItem()
    //    {
    //        return new AspNetUserRoles() { UserId = _Id_User };
    //    }
    //    protected IModel<AspNetUserRoles> CreateModel()
    //    {
    //        var model = new AspNetUserRolesAddEditModel();
    //        model.RolesList = entities.AspNetRoles.ToList();
    //        return model;
    //    }

    //    private readonly string CONTROLLER_NAME = typeof(AspNetUserRoles).Name;

    //    protected void FillModel(IndexGridModel<AspNetUserRoles> model)
    //    {
    //        model.IndexPrefix = entities.AspNetUsers.Where(x => x.Id == _Id_User).Select(x => x.UserName).FirstOrDefault() + ": ";
    //        model.AdditionalUrlParamenter = "&Id_User=" + _Id_User;
    //    }
    //    protected string GetControllerName()
    //    {
    //        return CONTROLLER_NAME;
    //    }

    //    protected IndexGridModel<AspNetUserRoles> CreateIndexModel()
    //    {
    //        var model = new IndexGridModel<AspNetUserRoles>() { EntityName = GetControllerName() };
    //        FillModel(model);
    //        return model;
    //    }

    //    //Пока думаю, как сделать более универсальную штуку на шаблонах, чтоб можно было вообще клёво обращаться из справочника
    //    //Пока что склоняюсь к чему-то вроде return Parse_Id_Person() || Parse_Id_Country() || Parse_Id_AbroadTripReason();
    //    //Так можно будет сразу распарсить все нужные параметры и использовать один ManyToManyController и одни и те же вьюшки для захода с разных сторон
    //    //Например, елси передали в него Id_Country - он вернул все поездки в эту страну, Id_Person - все поездки этого человека и тд
    //    //Скорее всего это будет ещё один абстрактный метод
    //    public ActionResult Index(bool isDebug = false)
    //    {
    //        if (!ParseInnerParametersForIndex())
    //        {
    //            return null;
    //        }
    //        ViewBag.AJAX = isDebug;
    //        PrepareViewBagForIndex();
    //        DevExpressHelper.Theme = Startup.THEME;
    //        var model = CreateIndexModel();
    //        return View(model);
    //    }

    //    public ActionResult IndexPartial()
    //    {
    //        if (!ParseInnerParametersForIndexPartial())
    //        {
    //            return null;
    //        }
    //        PrepareViewBagForIndexPartial();
    //        var model = CreateIndexModel();
    //        DevExpressHelper.Theme = Startup.THEME;
    //        return View("_Index", model);
    //    }

    //    public string Delete(int id)
    //    {
    //        var t = entities.Set<AspNetUserRoles>().FirstOrDefault(x => x.Id == id);
    //        if (t == null)
    //        {
    //            return "null";
    //        }
    //        entities.Set<AspNetUserRoles>().Remove(t);
    //        entities.SaveChanges();
    //        return "deleted";
    //    }
    //    public string GetDeleteText(int id)
    //    {
    //        var t = entities.Set<AspNetUserRoles>().FirstOrDefault(x => x.Id == id);
    //        if (t == null)
    //        {
    //            return "";
    //        }
    //        return Labels.ShureForRemoveEntry;
    //    }

    //    public ActionResult AddEdit(int id, bool isDebug = false)
    //    {
    //        if (!ParseInnerParametersForAddEdit())
    //        {
    //            return null;
    //        }
    //        ViewBag.AJAX = isDebug;
    //        PrepareViewBagForAddEdit();

    //        AspNetUserRoles t = null;
    //        if (id == 0)
    //        {
    //            t = InitNewItem();
    //        }
    //        else
    //        {
    //            t = entities.Set<AspNetUserRoles>().FirstOrDefault(x => x.Id == id);
    //        }

    //        ViewBag.MODEL = CreateModel();

    //        return View(t);
    //    }

    //    [HttpPost]
    //    public ActionResult AddEdit(FormCollection formData)
    //    {
    //        var t = (AspNetUserRoles)new AspNetUserRoles().Parse(formData);

    //        if (t.Id == 0)
    //        {
    //            entities.Set<AspNetUserRoles>().Add(t);
    //        }
    //        else
    //        {
    //            entities.Entry(t).State = EntityState.Modified;
    //        }
    //        entities.SaveChanges();
    //        return null;
    //    }
    //}

    //public class AspNetUserRolesAddEditModel : IModel<AspNetUserRoles>
    //{
    //    public List<AspNetRoles> RolesList { get; set; }
    //}
}