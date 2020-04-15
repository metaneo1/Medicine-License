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
    public class MessageController : BaseManyToManyController<Message, LicenseRequest>
    {
        protected override void AddEditFinished(Message message, bool isNew)
        {
            if (message.Id_Document != null)
            {
                if (isNew)
                {
                    var documentMessage = new MessageForSentDocument();
                    documentMessage.Id_SentDocument = message.Id_Document.Value;
                    documentMessage.Id_Message = message.Id;
                    entities.MessageForSentDocument.Add(documentMessage);
                    entities.SaveChanges();
                }
            }
        }
        public ActionResult IndexForDocument(bool isDebug = false)
        {
            var _Id_Document = 0;
            if (!int.TryParse(Request.QueryString["Id_Document"], out _Id_Document))
            {
                return null;
            }
            if (!int.TryParse(Request.QueryString["Id_Request"], out _Id_Request))
            {
                return null;
            }
            ViewBag.Message_Id_Document = _Id_Document;
            ViewBag.Message_Id_Request = _Id_Request;
            ViewBag.AJAX = isDebug;
            ViewBag.Message_Id_Request = _Id_Request;
            ViewBag.Message_LicenseRequest = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.ClinicName).FirstOrDefault();
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View(model);
        }

        public ActionResult IndexForDocumentPartial()
        {
            var _Id_Document = 0;
            if (!int.TryParse(Request.QueryString["Id_Document"], out _Id_Document))
            {
                return null;
            }
            if (!int.TryParse(Request.QueryString["Id_Request"], out _Id_Request))
            {
                return null;
            }
            ViewBag.Message_Id_Document = _Id_Document;
            ViewBag.Message_Id_Request = _Id_Request;
            ViewBag.Message_LicenseRequest = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.ClinicName).FirstOrDefault();
            var model = CreateIndexModel();
            DevExpressHelper.Theme = Startup.THEME;
            return View("_IndexForDocument", model);
        }
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
            ViewBag.Message_LicenseRequest = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.ClinicName).FirstOrDefault();
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


        public ActionResult AllMessages()
        {
            var userId = GetCurrentUser();
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Filter = (Expression<Func<Message, bool>>)(x => x.LicenseRequest.Id_User == userId);
            var model = CreateIndexModel();
            return View(model);
        }

        public ActionResult AllMessagesPartial()
        {
            var userId = GetCurrentUser();
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Filter = (Expression<Func<Message, bool>>)(x => x.LicenseRequest.Id_User == userId);
            var model = CreateIndexModel();
            return View("_AllMessages", model);
        }

        private System.Linq.Expressions.Expression<Func<LicenseRequest, bool>> GetPred()
        {
            return x => x.Id == _Id_Request;
        }
        protected override void FillModel(IndexGridModel<Message> model)
        {
            model.IndexPrefix = entities.LicenseRequest.Where(GetPred()).Select(x => x.ClinicName).FirstOrDefault();
            model.AdditionalUrlParamenter = "&Id_Request=" + _Id_Request;
        }
        protected override Expression<Func<Message, bool>> GetFilterForParentEntity()
        {
            return x => x.Id_Request == _Id_Request;
        }
        //Создание модели
        protected override IModel<Message> CreateModel()
        {
            var _Id_Document = 0;
            int.TryParse(Request.QueryString["Id_Document"], out _Id_Document);
            ViewBag.DocumentId = _Id_Document;
            var model = new MessageAddEditModel();
            model.QuestionTypeList = entities.QuestionType.ToList();
            model.MessageList = entities.Message.ToList();
            return model;
        }


    }

    public class MessageAddEditModel : IModel<Message>
    {
        public List<QuestionType> QuestionTypeList { get; set; }
        public List<Message> MessageList { get; set; }
    }
}
