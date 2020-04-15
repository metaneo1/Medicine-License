using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Resources;
using MvcBaseApp.Models;
using System.Linq.Expressions;
using System.Threading;
using DataModel;

namespace MvcBaseApp.Controllers
{
    public class MessageController : BaseManyToManyController<Message, LicenseRequest>
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
            ViewBag.Message_Id_Request = _Id_Request;
            ViewBag.Message_LicenseRequest = entities.LicenseRequest.Where(x => x.Id_RequestType == _Id_Request).Select(x => x.Organization.Name_kg).FirstOrDefault();
        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            ViewBag.Message_Id_Request = _Id_Request;
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
        protected override Message InitNewItem()
        {
            return new Message() { Id_Request = _Id_Request, MessageDate = DateTime.Now };
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

        public ActionResult RequestList()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            var model = entities.LicenseRequest.Where(x=>!x.IsDraft).ToList();
            return PartialView("_RequestList", model);
        }

        public ActionResult MessageList()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            var model = entities.Message.Where(x=>x.Id_QuestionType != null).OrderByDescending(x=>x.MessageDate).ToList();
            return PartialView("_MessageList", model);
        }

        [HttpGet]
        public ActionResult pnl_LicenseMessagesPartial()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("pnl_Messages");
        }
        [HttpPost]
        public ActionResult pnl_LicenseMessagesPartial(int? Id_Request)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = Id_Request;
            return PartialView("pnl_Messages");
        }

        [HttpGet]
        public ActionResult pnl_RequestStateHistoryPartial()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("pnl_RequestStateHistory");
        }
        [HttpPost]
        public ActionResult pnl_RequestStateHistoryPartial(int? Id_Request)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = Id_Request;
            return PartialView("pnl_RequestStateHistory");
        }

        public ActionResult LicenseTree()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            var model = entities.LicenseRequest.Where(x => x.Message.Any()).ToList();
            return PartialView("_LicenseTree", model);
        }

        [HttpGet]
        public ActionResult GetLicenseMessagesPartial()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = 0;
            var list = entities.Message.Where(x=>x.Id_Request == 0).ToList();//.LicenseRequest.Where(x => x.Id == Id_Request).SelectMany(x => x.Message).ToList();
            var model = CreateIndexModel();
            model.Entity = list;
            return PartialView("LicenseMessages", model);
        }

        public ActionResult LicenseMessagesPartial(int? Id_Request)
        {
            _Id_Request = Id_Request ?? 0;
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = Id_Request;
            var list = entities.Message.Where(x => x.Id_Request == Id_Request).ToList();//.LicenseRequest.Where(x => x.Id == Id_Request).SelectMany(x => x.Message).ToList();
            var model = CreateIndexModel();
            model.Entity = list;
            return PartialView("_LicenseMessages", model);
        }

        [HttpGet]
        public ActionResult GetLicenseStatusesPartial()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = 0;
            var list = entities.RequestStateHistory.Where(x=>x.Id_Request == 0).ToList();//.LicenseRequest.Where(x => x.Id == Id_Request).SelectMany(x => x.Message).ToList();
            var model = new IndexGridModel<List<RequestStateHistory>>() { EntityName = GetControllerName()};
            model.Entity = list;
            return PartialView("LicenseStatuses", model);
        }

        public ActionResult LicenseStatusesPartial(int? Id_Request)
        {
            _Id_Request = Id_Request ?? 0;
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = Id_Request;
            var list = entities.RequestStateHistory.Where(x => x.Id_Request == Id_Request).ToList();//.LicenseRequest.Where(x => x.Id == Id_Request).SelectMany(x => x.Message).ToList();
            var model = new IndexGridModel<List<RequestStateHistory>>() { EntityName = GetControllerName() };
            model.Entity = list;
            return PartialView("_LicenseStatuses", model);
        }

        public ActionResult LicenseRequestPartial(int FilterId)
        {
            var licenseRequest = entities.LicenseRequest.FirstOrDefault(x => x.Id == FilterId);
            //var s = RenderRazorViewToString("_LicensePreview", licenseRequest);
            //var model = new ReloadModel();
            //model.Action = "LicenseRequestPartial";
            //model.Controller = GetControllerName();
            //model.FilterId = FilterId;
            //model.Name = "pnl_Request";
            //model.Rendered = new MvcHtmlString(s);
            return PartialView("_ReloadPanel", licenseRequest);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
        public ActionResult MeActive()
        {
            var model = new MyMessageModel();
            model.Requests = entities.LicenseRequest.Where(x => x.Message.Any()).ToList();
            return View(model);
        }

        public ActionResult AllLicenses()
        {
            var model = new MyMessageModel();
            model.Requests = entities.LicenseRequest.Where(x => !x.IsDraft).ToList();
            return View(model);
        }

        private System.Linq.Expressions.Expression<Func<LicenseRequest, bool>> GetPred()
        {
            return x => x.Id == _Id_Request;
        }
        protected override void FillModel(IndexGridModel<List<Message>> model)
        {
            model.IndexPrefix = entities.LicenseRequest.Where(GetPred()).Select(x => x.Organization.Name_kg).FirstOrDefault();
            model.AdditionalUrlParamenter = "&Id_Request=" + _Id_Request;
        }
        protected override Expression<Func<Message, bool>> GetFilterForParentEntity()
        {
            return x => x.Id_Request == _Id_Request;
        }
        //Создание модели
        protected override IModel<Message> CreateModel()
        {
            var model = new MessageAddEditModel();
            model.QuestionTypeList = entities.QuestionType.ToList();
            model.MessageList = entities.Message.ToList();
            return model;
        }



        //public ActionResult CallbackPanelPartial()
        //{
        //    return PartialView("_CallbackPanelPartial");
        //}

        public ActionResult CallbackPanel1Partial()
        {
            return PartialView("_CallbackPanel1Partial");
        }
    }

    public class MyMessageModel
    {
        public List<LicenseRequest> Requests { get; set; }
    }
    public class MessageAddEditModel : IModel<Message>
    {
        public List<QuestionType> QuestionTypeList { get; set; }
        public List<Message> MessageList { get; set; }
    }

    public class ReloadModel
    {
        public MvcHtmlString Rendered { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public int FilterId { get; set; }
    }
}
