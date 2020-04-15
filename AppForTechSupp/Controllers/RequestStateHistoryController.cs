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
using DataModel.Const;

namespace MvcBaseApp.Controllers
{
    public class RequestStateHistoryController : BaseManyToManyController<RequestStateHistory, LicenseRequest>
    {

        //тут айдишники всех сущностей, по которым можно получить список. 
        //в данном случае мы берём поездки только по Worker-у, можно ещё брать по цели и стране\
        //создаются при каждом запросе в момент разбора пришедшего URL
        private int _Id_Request;

        //Разбор пришедшего URL для построения гриды
        protected override bool ParseInnerParametersForIndex()
        {
            return Parse_Id_Request();
        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {
            ViewBag.RequestStateHistory_Id_Request = _Id_Request;
            ViewBag.RequestStateHistory_LicenseRequest = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.Organization.Name_kg).FirstOrDefault();
        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            ViewBag.RequestStateHistory_Id_Request = _Id_Request;
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
        protected override RequestStateHistory InitNewItem()
        {
            var historyElement =  new RequestStateHistory() { Id_Request = _Id_Request };
            historyElement.LicenseRequest = entities.LicenseRequest.FirstOrDefault(x => x.Id == _Id_Request);
            historyElement.Id_User = GetCurrentUser();
            historyElement.DateStatusChange = DateTime.Now;
            return historyElement;
        }

        //Разбор входящих параметров
        private bool Parse_Id_Request()
        {
            if (!int.TryParse(Request.QueryString["Id_Request"], out _Id_Request))
            {
                return false;
            }
            return true;
        }

        public ActionResult Unassigned()
        {
            StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            ViewBag.AJAX = false;
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View(model);
        }

        public ActionResult UnassignedPartial()
        {
            StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View("_Unassigned", model);
        }

        public ActionResult My()
        {
            var userId = GetCurrentUser();
            StatusFilter = history => history.Id_User == userId && history.Id_State != Const.RequestStateId.Finished;
            ViewBag.AJAX = false;
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View(model);
        }

        public ActionResult MyPartial()
        {
            var userId = GetCurrentUser();
            StatusFilter = history => history.Id_User == userId && history.Id_State != Const.RequestStateId.Finished;
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View("_My", model);
        }

        private System.Linq.Expressions.Expression<Func<LicenseRequest, bool>> GetPred()
        {
            return x => x.Id == _Id_Request;
        }

        private System.Linq.Expressions.Expression<Func<RequestStateHistory, bool>> StatusFilter;
        protected override void FillModel(IndexGridModel<List<RequestStateHistory>> model)
        {
            var ss = entities.LicenseRequest
                .Select(x => new
                {
                    Request = x,
                    Status = x.RequestStateHistory.OrderByDescending(y => y.DateStatusChange).Take(1)
                })
                .Select(x => x.Status)
                .SelectMany(x => x);
            //var ss = entities.RequestStateHistory.OrderByDescending(x=>x.DateStatusChange).GroupBy(x => x.LicenseRequest).ToList();
            model.Entity = ss.Where(StatusFilter).ToList();
            model.IndexPrefix = entities.LicenseRequest.Where(GetPred()).Select(x => x.Organization.Name_kg).FirstOrDefault();
            model.AdditionalUrlParamenter = "&Id_Request=" + _Id_Request;
        }
        protected override Expression<Func<RequestStateHistory, bool>> GetFilterForParentEntity()
        {
            return x => x.Id_Request == _Id_Request;
        }
        //Создание модели
        protected override IModel<RequestStateHistory> CreateModel()
        {
            var model = new RequestStateHistoryAddEditModel();
            model.RequestStateList = entities.RequestState.ToList();
            return model;
        }


    }

    public class RequestStateHistoryAddEditModel : IModel<RequestStateHistory>
    {
        public List<RequestState> RequestStateList { get; set; }
    }
}
