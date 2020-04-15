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
    public class DocumentInRequestController : BaseDocumentController<DocumentInRequest, vw_DocumentInRequest>
    {

        [HttpGet]
        public ActionResult pnl_LicenseDocumentsPartial()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("pnl_Documents");
        }
        [HttpPost]
        public ActionResult pnl_LicenseDocumentsPartial(int? Id_Request)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.DocumentInRequest_Id_Request = Id_Request;
            return PartialView("pnl_Documents");
        }


        [HttpGet]
        public ActionResult GetLicenseDocumentsPartial()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;

            ViewBag.DocumentInRequest_Id_Request = 0;
            var r = entities.LicenseRequest.FirstOrDefault(x => x.Id == 0);
            var idtype = r?.Id_RequestType;
            var list = DatabaseProvider.DB.GetDocumentAndRequestElements(idtype, 0).ToList();

            var model = new IndexGridModel<List<GetDocumentAndRequestElements_Result>> {EntityName = GetControllerName()};
            model.Entity = list;
            return PartialView("LicenseDocuments", model);
        }

        public ActionResult LicenseDocumentsPartial(int? Id_Request)
        {
            _Id_Request = Id_Request ?? 0;
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.DocumentInRequest_Id_Request = _Id_Request;
            var r = entities.LicenseRequest.FirstOrDefault(x => x.Id == _Id_Request);
            var idtype = r?.Id_RequestType;
            var list = DatabaseProvider.DB.GetDocumentAndRequestElements(idtype, _Id_Request).ToList();

            var model = new IndexGridModel<List<GetDocumentAndRequestElements_Result>> { EntityName = GetControllerName() };
            model.Entity = list;
            return PartialView("_TreeList", list);
        }

        public ActionResult DataBindingPartial(int? Id_Request = null)
        {
            var r = entities.LicenseRequest.FirstOrDefault(x => x.Id == Id_Request);
            var idtype = r?.Id_RequestType;
            var list = DatabaseProvider.DB.GetDocumentAndRequestElements(idtype, Id_Request).ToList();
            ViewBag.DocumentInRequest_Id_Request = Id_Request;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("_TreeList", list);
        }

        //тут айдишники всех сущностей, по которым можно получить список. 
        //в данном случае мы берём поездки только по Worker-у, можно ещё брать по цели и стране\
        //создаются при каждом запросе в момент разбора пришедшего URL
        private int _Id_Request;
        private int _Id_RequestElement;

        //Разбор пришедшего URL для построения гриды
        protected override bool ParseInnerParametersForIndex()
        {
            return Parse_Id_Request();
        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {
            ViewBag.DocumentInRequest_Id_Request = _Id_Request;
            var r = entities.LicenseRequest.FirstOrDefault(x => x.Id == _Id_Request);

            ViewBag.DocumentInRequest_Id_RequestType = r?.Id_RequestType;
            ViewBag.DocumentInRequest_LicenseRequest = r;

            //var r = entities.LicenseRequest.FirstOrDefault(x => x.Id == _Id_Request);
            var idtype = r?.Id_RequestType;
            var list = DatabaseProvider.DB.GetDocumentAndRequestElements(idtype, _Id_Request).ToList();
            ViewBag.TreeListData = list;

        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            var r = entities.LicenseRequest.FirstOrDefault(x => x.Id == _Id_Request);

            ViewBag.DocumentInRequest_Id_RequestType = r?.Id_RequestType;
            ViewBag.DocumentInRequest_Id_Request = _Id_Request;
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
            ViewBag.DocumentInRequest_Id_Request = _Id_Request;
        }

        //Создание новой сущности
        protected override DocumentInRequest InitNewItem()
        {
            return new DocumentInRequest() { Id_Request = _Id_Request , Id_RequestElement = _Id_RequestElement };
        }

        //Разбор входящих параметров
        private bool Parse_Id_Request()
        {
            if (!int.TryParse(Request.QueryString["Id_Request"], out _Id_Request))
            {
                return false;
            }

            var Id_RequestElement = Request.QueryString["Id_RequestElement"];
            if (Id_RequestElement!=null)
                if (Id_RequestElement.Contains("_"))
                {
                    if (!int.TryParse(Request.QueryString["Id_RequestElement"].Split('_')[0], out _Id_RequestElement))
                    {
                        //return false;
                    }
                }
                else if (!int.TryParse(Request.QueryString["Id_RequestElement"], out _Id_RequestElement))
                {
                    //return false;
                }
            return true;
        }


        protected override void FillModel(IndexGridModel<List<DocumentInRequest>> model)
        {
            model.IndexPrefix = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.ClinicName).FirstOrDefault();
            //var licenseRequest = entities.LicenseRequest.FirstOrDefault(x => x.Id == _Id_Request);
            model.AdditionalUrlParamenter = "&Id_Request=" + _Id_Request;
            //model.AdditionalUrlParamenter = "&Id_RequestElement=" + _Id_RequestElement;

            //var list1 = entities.DocumentInRequest.Where(x => x.Id_Request == _Id_Request).ToList();
            //var list2 = entities.RequestElement.Where(x => x.Id_RequestType == licenseRequest.Id_RequestType).ToList();
            //var list3 = list2.Select(x => new DocumentInRequest
            //{
            //    Id_Request = _Id_Request,
            //    Id_RequestElement = x.Id,
            //    Description = x.RequestElementType.Name_ru // RequestElement2.RequestElementType.Name_ru
            //}).ToList();
            //model.List = list1.Where(x => list3.Any(c => c.Id_RequestElement != x.Id_RequestElement)).Union(list3).ToList();
            //model.List = 
        }
        
        //Создание модели
        protected override IModel<DocumentInRequest> CreateModel()
        {
            var model = new DocumentInRequestAddEditModel();
            model.DocumentList = entities.Document.ToList();
            model.DocumentStateList = entities.DocumentState.ToList();
            model.RequestElementList = entities.RequestElement.ToList();
            model.Document_FormatList = entities.Document_Format.ToList();
            return model;
        }

        protected override Expression<Func<vw_DocumentInRequest, bool>> GetFilterForParentEntity()
        {
            return x => x.Id == _Id_Request;

        }
    }


    public class DocumentInRequestAddEditModel : IModel<DocumentInRequest>
    {
        public List<Document> DocumentList { get; set; }
        public List<RequestElement> RequestElementList { get; set; }
        public List<DocumentState> DocumentStateList { get; set; }
        public List<Document_Format> Document_FormatList { get; set; }
    }
}
