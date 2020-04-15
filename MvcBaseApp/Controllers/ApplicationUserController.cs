using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DataModel.Const;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;
using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    public class ApplicationUserController : BaseDictionaryController<ApplicationUser>, IEntityWithDocument
    {
        protected override string AddEditWrapperName { get { return "AddEdit"; } }


        public override ActionResult AddEdit(int userId = 0, bool isDebug = false)
        {
            ViewBag.AJAX = isDebug;
            ApplicationUser t;
            if (!User.Identity.IsAuthenticated)
            {
                t = new ApplicationUser();
            }
            else
            {
                var id = GetCurrentUser();
                t = entities.ApplicationUser.FirstOrDefault(x => x.Id == id);
            }

            return View(AddEditWrapperName, t);
        }


        [HttpPost]
        public override string AddEdit(FormCollection formData)
        {
            var t = (ApplicationUser)new ApplicationUser().Parse(formData);
            //if (t.Id == 0)
            //{
            //    entities.ApplicationUser.Add(t);

            //    entities.SaveChanges();
            //    return t.Id.ToString();

            //    //return base.AddEdit(formData);
            //}

            var document = new Document();
            DocumentHelper.ParseDocument(ref document, formData, Request);
            document.Id_DocumentFormat = Const.DocumentFormatId.Image;
            //entities.ApplicationUser.Attach(t);
            if (t.Id == 0)
            {
                t.Document = document;
                t.Id_Document = 0;
                t.Document = null;
                if (document.Filename != null)
                {
                    t.Document = document;
                    t.Id_Document = document.Id;
                    entities.Document.Add(t.Document);
                }
                entities.ApplicationUser.Add(t);
                //entities.Entry(t).State = EntityState.Modified;
                entities.SaveChanges();
                return t.Id.ToString();
            }


            int? oldDocumentId;
            var alwaysDocument = t as IEntityWithDocument;
            if (alwaysDocument == null)
            {
                oldDocumentId =
                    entities.ApplicationUser.Where(x => x.Id == t.Id).Select(x => x.Id_Document).FirstOrDefault() ?? 0;
            }
            else
            {
                oldDocumentId = alwaysDocument.Id_Document;
                //oldDocumentId = AlwaysDocuemntExtender.GetAlwaysDocuemntId<Worker_document>(t, entities);
            }
            if (oldDocumentId == document.Id && document.Id != 0)
            {
                entities.Entry(t).State = EntityState.Modified;
                t.Id_Document = oldDocumentId;
                var oldDocument = entities.Document.FirstOrDefault(x => x.Id == oldDocumentId);
                oldDocument.Name = document.Name;
                oldDocument.Description = document.Description;
                oldDocument.Id_DocumentFormat = document.Id_DocumentFormat;

                entities.SaveChanges();
                return "Ok";
            }

            if (oldDocumentId == 0)
            {
                entities.Entry(t).State = EntityState.Modified;
                if (document.Filename != null)
                {
                    t.Document = document;
                    entities.Document.Add(document);
                }
                else
                {
                    t.Id_Document = null;
                }
                entities.SaveChanges();
                return "Ok";

            }

            if (document.Filename != null)
            {
                entities.Entry(t).State = EntityState.Modified;
                t.Id_Document = oldDocumentId;
                var oldDocument = entities.Document.FirstOrDefault(x => x.Id == oldDocumentId);
                oldDocument.Name = document.Name;
                oldDocument.Description = document.Description;
                oldDocument.Id_DocumentFormat = document.Id_DocumentFormat;
                oldDocument.Filename = document.Filename;

                entities.SaveChanges();
                return "Ok";
            }

            entities.Entry(t).State = EntityState.Modified;
            t.Id_Document = 0;

            entities.SaveChanges();
            return "Ok";


            return "";


            return t.Id.ToString();
        }


        public ActionResult MyImage(int? id = 0)
        {
            var doc = entities.Document.FirstOrDefault(x => x.Id == id);
            var filename = doc == null ? "icon.png" : doc.PathToFile;
            var path = FileSystemHelper.GetLocalPathForFile(filename);
            return File(path, "image/jpg");
        }

        //[HttpPost]
        //public override string AddEdit(FormCollection formData)
        //{
        //    var t = (ApplicationUser)new ApplicationUser().Parse(formData);
        //    if (t.Id == 0)
        //        return base.AddEdit(formData);
        //    //t.AspUserId = User.Identity.GetUserId();
        //    entities.ApplicationUser.Attach(t);
        //    entities.Entry(t).State = EntityState.Modified;
        //    entities.SaveChanges();
        //    return t.Id.ToString();
        //}
        public int? Id_Document { get; set; }
        public Document Document { get; set; }
    }
}
