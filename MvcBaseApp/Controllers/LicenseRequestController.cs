using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DataModel;
using DevExpress.Web.Mvc;
using DevExpress.XtraRichEdit;
using Microsoft.AspNet.Identity;
using MvcBaseApp.Models;
using Newtonsoft.Json;

namespace MvcBaseApp.Controllers
{
    public class LicenseRequestController : BaseMainFormController<LicenseRequest>
    {

        [HttpGet]
        public ActionResult pnl_LicensePreviewPanel()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("pnl_LicensePreviewPanel");
        }
        [HttpPost]
        public ActionResult pnl_LicensePreviewPanel(int? Id_Request)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = Id_Request;
            return PartialView("pnl_LicensePreviewPanel");
        }

        [HttpGet]
        public ActionResult pnl_LicenseFullView()
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            return PartialView("pnl_LicenseFullView");
        }
        [HttpPost]
        public ActionResult pnl_LicenseFullView(int? Id_Request)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = Id_Request;
            return PartialView("pnl_LicenseFullView");
        }

        public ActionResult LicensePreviewPartial(int? stateId)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = stateId;
            var userId = GetCurrentUser();


            var ss1 = entities.LicenseRequest
        .Select(x => new
        {
            Request = x,
            Status = x.RequestStateHistory.OrderByDescending(y => y.DateStatusChange).Take(1)
        }).ToList();

            var ss2 = ss1
                .Select(x => x.Status)
                .SelectMany(x => x).ToList();
            var ss = ss2
                .Where(x => x.Id_State == stateId)
                .Select(x => x.Id_Request)
                .ToList();


            var list = entities.LicenseRequest
                .Where(x => ss.Contains(x.Id))
                .Where(x => x.Id_User == userId).ToList();//.LicenseRequest.Where(x => x.Id == Id_Request).SelectMany(x => x.Message).ToList();

            return PartialView("_LicensePreview", list);
        }

        public ActionResult LicenseFullViewPartial(int? licenseId)
        {
            //StatusFilter = history => history.Id_State == Const.RequestStateId.Unassigned;
            DevExpressHelper.Theme = Startup.THEME;
            ViewBag.Message_Id_Request = licenseId;
            var userId = GetCurrentUser();

            var steps = entities.RequestStep.Where(x => x.Id_Parent == null)
                .Select(x => new ItemData()
                {
                    Id = x.Id,
                    Name = x.Name_kg,
                    Text = x.Description_kg,
                    ActiveImageName = x.ActiveImageUrl,
                    InactiveImageName = x.DiabledImageUrl,
                    NavigateUrl = x.Url,
                    HasChildren = x.RequestStep1.Any()
                })
                .ToList();

            foreach (var step in steps)
            {
                var innerSteps = entities.RequestStep.Where(x => x.Id_Parent == step.Id)
                    .Select(x => new ItemData()
                    {
                        Id = x.Id,
                        Name = x.Name_kg,
                        Text = x.Description_kg,
                        ActiveImageName = "",
                        InactiveImageName = "",
                        NavigateUrl = x.Url,
                        HasChildren = false,
                    }).ToList();
                step.Children = new ItemsData(innerSteps);
            }
            var navbarData = new ItemsData(steps);
            ViewBag.NAVBAR = navbarData;
            var license = entities.LicenseRequest.FirstOrDefault(x => x.Id == licenseId && x.Id_User == userId);

            return PartialView("_LicenseFullView", license);
        }

        [HttpPost]
        public int GenerateWord(int id)
        {
            var userId = GetCurrentUser();
            var request = entities.LicenseRequest.FirstOrDefault(x => x.Id == id && x.Id_User == userId);

            if (request == null)
                return 0;
            

            var richEdit = new RichEditDocumentServer();
            var richEdit2 = new RichEditDocumentServer();


            richEdit.Document.LoadDocument(FileSystemHelper.GetLocalPathForFile("LicenseRequest.docx"), DocumentFormat.OpenXml);

            richEdit.Options.MailMerge.DataSource = new [] {request};

            richEdit.Document.MailMerge(richEdit2.Document);

            var guid = Guid.NewGuid().ToString();
            var fileName = FileSystemHelper.MapFileName(guid);
            richEdit2.Document.SaveDocument(fileName, DocumentFormat.OpenXml);


            var fileId = request.RequestType.PrintTemplate_ru.Replace(" ", "_") + "_от_" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm") + ".docx";
            var document = new Document();
            document.Filename = fileId;
            document.PathToFile = guid;
            document.Id_DocumentFormat = entities.Document_Format.FirstOrDefault().Id;
            document.Name = request.RequestType.PrintTemplate_ru;
            entities.Document.Add(document);
            entities.SaveChanges();
            return document.Id;
        }

        public FileContentResult LicensePrintTemplate(int id)
        {
            var document = entities.Document.Where(x => x.Id == id).FirstOrDefault();

            var filedata = FileSystemHelper.GetFileByName(document.PathToFile);
            var contentType = MimeMapping.GetMimeMapping(document.Filename);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = document.Filename,
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());


            return File(filedata, contentType);

        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {

        }


        //Создание новой сущности
        protected override LicenseRequest InitNewItem()
        {
            return new LicenseRequest { DateCreate = DateTime.Now, Id_User = GetCurrentUser(), IsDraft = true };
        }


        protected override void FillModel(IndexGridModel<LicenseRequest> model)
        {
            //model.IndexPrefix = entities.LicenseRequest.FirstOrDefault(x => x.Id != 0)?.ClinicName;

        }

        //Создание модели
        protected override IModel<LicenseRequest> CreateModel()
        {
            var model = new LicenseRequestAddEditModel();
            //model.LicenseRequestList = entities.LicenseRequest.ToList();
            //model.Document_FormatList = entities.Document_Format.ToList();

            return model;
        }


        protected override object GetDataForExport()
        {
            return entities.Set<LicenseRequest>().ToList();
        }


        [ValidateInput(false)]
        public ActionResult TreeListPartial(string SearchText, int? Id_AdminUnit)
        {
            ViewData["SearchText"] = SearchText;
            ViewData["Id_AdminUnit"] = Id_AdminUnit;
            var model = entities.AdminUnits.ToList();

            //AdminUnits GetLocalGovernment(int groupId) => model.FirstOrDefault(g => g.Id == groupId);
            //int GetDepth(AdminUnits g) => (g == null) ? 0 : (g.Id_Parent.HasValue ? GetDepth(GetLocalGovernment(g.Id_Parent.Value)) + 1 : 1);

            return PartialView("_TreeListPartial", model.Select(x => new AdminUnitsController.AdminUnitTreeModel
            {
                Name = x.Name_ru,
                ParentId = x.ParentId,
                Id = x.Id
            }).ToList());
        }

        public ActionResult TreeListCustomPartial(string SearchText, bool? isNewSearch)
        {
            ViewData["IsNewSearch"] = isNewSearch;
            ViewData["SearchText"] = SearchText;
            var model = entities.AdminUnits.ToList();


            return PartialView("_TreeListPartial", model.Select(x => new AdminUnitsController.AdminUnitTreeModel
            {
                Name = x.Name_ru,
                ParentId = x.ParentId,
                Id = x.Id
            }).ToList());
        }

        [HttpGet]
        public override ActionResult AddEdit(int id, bool isDebug = false)
        {
            var neededId = id;
            if (neededId == 0)
            {
                var licenseRequest = InitNewItem();
                entities.LicenseRequest.Add(licenseRequest);
                entities.SaveChanges();
                neededId = licenseRequest.Id;
                return RedirectToAction("AddEdit", new { id = neededId, isDebug });
            }
            return base.AddEdit(id, isDebug);
        }

        [HttpPost]
        protected override void AddEditAfterParseActions(LicenseRequest request)
        {
            request.IsDraft = false;
        }

    }

    public class ItemsData : IHierarchicalEnumerable, IEnumerable
    {
        public ItemsData(IEnumerable data)
        {
            Data = data;
        }

        public IEnumerable Data { get; set; }

        public IEnumerator GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return (IHierarchyData)enumeratedItem;
        }

        public ItemData GetItem(string categoryName)
        {
            return (Data as List<ItemData>).Where(x => x.Name == categoryName).FirstOrDefault();
        }
    }
    public class ItemData : IHierarchyData
    {
        public ItemData()
        {
            HasChildren = false;
            Name = Text = ActiveImageName = InactiveImageName = String.Empty;
        }

        public ItemData(string name, string text, string activeImageName, string inactiveImageName, bool hasChildren)
        {
            Name = name;
            Text = text;
            HasChildren = hasChildren;
            ActiveImageName = activeImageName;
            InactiveImageName = inactiveImageName;
            if (hasChildren)
                Children = new ItemsData(new List<ItemData> { new ItemData() });
        }

        public string Name { get; set; }
        public string Text { get; set; }
        public string ActiveImageName { get; set; }
        public string InactiveImageName { get; set; }

        public ItemsData Children { get; set; }

        public bool HasChildren { get; set; }

        public string NavigateUrl { get; set; }

        // IHierarchyData
        bool IHierarchyData.HasChildren
        {
            get { return HasChildren; }
        }

        object IHierarchyData.Item
        {
            get { return this; }
        }

        string IHierarchyData.Path
        {
            get { return String.Empty; }
        }

        string IHierarchyData.Type
        {
            get { return GetType().ToString(); }
        }

        public int Id { get; set; }

        IHierarchicalEnumerable IHierarchyData.GetChildren()
        {
            return Children;
        }

        IHierarchyData IHierarchyData.GetParent()
        {
            return null;
        }
    }
    public static class ItemDataFactory
    {
        public static ItemData GetParentItem(string name, string text, string activeImageName, string inactiveImageName)
        {
            return new ItemData(name, text, activeImageName, inactiveImageName, true);
        }

        public static ItemData GetChildItem(string name, string text, string navigateUrl)
        {
            return new ItemData(name, text, String.Empty, String.Empty, false) { NavigateUrl = navigateUrl };
        }
    }
    public class LicenseRequestAddEditModel : IModel<LicenseRequest>
    {
        //public List<LicenseRequest> LicenseRequestList { get; set; }
        //public List<Document_Format> Document_FormatList { get; set; }
    }
}
