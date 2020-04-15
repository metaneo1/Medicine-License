using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Resources;
using MvcBaseApp.Models;
using System.Linq.Expressions;
using DataModel;

namespace MvcBaseApp.Controllers
{
    public class LicenseRequestActivityTypeController : BaseManyToManyController<LicenseRequestActivityType, ActivityType>
    {

        //тут айдишники всех сущностей, по которым можно получить список. 
        //в данном случае мы берём поездки только по Worker-у, можно ещё брать по цели и стране\
        //создаются при каждом запросе в момент разбора пришедшего URL
        private int _Id_Request;
        private bool _IsCallBack;

        //Разбор пришедшего URL для построения гриды
        protected override bool ParseInnerParametersForIndex()
        {
            return Parse_Id_Request();
        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {
            ViewBag.LicenseRequestActivityType_Id_Request = _Id_Request;
            ViewBag.LicenseRequestActivityType_LicenseRequest = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.ClinicName).FirstOrDefault();
        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            ViewBag.LicenseRequestActivityType_Id_Request = _Id_Request;
        }

        //Разбор пришедшего URL для создания новой поездки
        protected override bool ParseInnerParametersForAddEdit()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        //В данном случае их нет, но теоретически может понадобиться какя-то новая фигня
        protected override void PrepareViewBagForAddEdit()
        {
        }

        //Создание новой сущности
        protected override LicenseRequestActivityType InitNewItem()
        {
            return new LicenseRequestActivityType() { Id_Request = _Id_Request };
        }

        //Разбор входящих параметров
        private bool Parse_Id_Request()
        {
            var key = "Id_Request";
            if (Request.Path.Contains("/LicenseRequest/AddEdit"))
            {
                key = "id";
                _IsCallBack = true;
            }
            if (!int.TryParse(Request.QueryString[key], out _Id_Request))
            {
                return false;
            }
            if(!_IsCallBack)
                bool.TryParse(Request.QueryString["IsCallBack"], out _IsCallBack);
            return true;
        }

        [HttpGet]
        public ActionResult pnl_ActivityTypesPartial(int Id_Request, bool isDebug = false)
        {
            ViewBag.LicenseRequestActivityType_Id_Request = Id_Request;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("pnl_ActivityTypes");
        }
        [HttpPost]
        public ActionResult pnl_ActivityTypesPartial(int Id_Request)
        {
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.LicenseRequestActivityType_Id_Request = Id_Request;
            return PartialView("pnl_ActivityTypes");
        }

        private System.Linq.Expressions.Expression<Func<LicenseRequest, bool>> GetPred()
        {
            return x => x.Id == _Id_Request;
        }
        protected override void FillModel(IndexGridModel<LicenseRequestActivityType> model)
        {
            model.IsCallBack = _IsCallBack;
            model.IndexPrefix = entities.LicenseRequest.Where(GetPred()).Select(x => x.ClinicName).FirstOrDefault();
            model.AdditionalUrlParamenter = "&Id_Request=" + _Id_Request;
        }
        protected override Expression<Func<LicenseRequestActivityType, bool>> GetFilterForParentEntity()
        {
            return x => x.Id_Request == _Id_Request;
        }
        //Создание модели
        protected override IModel<LicenseRequestActivityType> CreateModel()
        {
            var model = new LicenseRequestActivityTypeAddEditModel();
            model.ActivityTypeList = entities.ActivityType.ToList();
            return model;
        }


    }

    public class LicenseRequestActivityTypeAddEditModel : IModel<LicenseRequestActivityType>
    {
        public List<ActivityType> ActivityTypeList { get; set; }
    }
}
