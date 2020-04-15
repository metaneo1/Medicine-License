using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    public abstract class BaseDocumentController<T, U> : BaseController<T>
        where T : class, IEntityWithId, IEntityAlwaysWithDocument, IParsable, new()
        where U : class, IEntityView
    {
        //protected static readonly string PATH = "~/Content/Files/";

        //protected hrmsEntities entities = new hrmsEntities();
        protected abstract bool ParseInnerParametersForIndex();
        protected abstract void PrepareViewBagForIndex();
        protected abstract bool ParseInnerParametersForIndexPartial();
        protected abstract void PrepareViewBagForIndexPartial();
        protected abstract bool ParseInnerParametersForAddEdit();
        protected abstract void PrepareViewBagForAddEdit();
        protected abstract T InitNewItem();
        protected abstract IModel<T> CreateModel();

        protected virtual void AddEditFinished(T t) { }
        protected virtual string AddEditSuccessResult() { return null; }
        protected virtual void DeleteSuccessFinished(T t) { }
        protected virtual string DeleteSuccessResult() { return null; }

        //protected override void FillModel(IndexGridModel<T> model)
        //{

        //}

        protected abstract System.Linq.Expressions.Expression<Func<U, bool>> GetFilterForParentEntity();

        protected override object GetDataForExport()
        {
            ParseInnerParametersForIndexPartial();
            return entities.Set<U>().Where(GetFilterForParentEntity()).ToList();
        }

        public new ActionResult Index(bool isDebug = false)
        {
            if (!ParseInnerParametersForIndex())
            {
                return null;
            }
            //ViewBag.AJAX = isDebug;
            PrepareViewBagForIndex();
            return base.Index(isDebug);
            //DevExpressHelper.Theme = Startup.THEME;
            //return View();
        }

        public new ActionResult IndexPartial()
        {
            if (!ParseInnerParametersForIndexPartial())
            {
                return null;
            }
            PrepareViewBagForIndexPartial();
            return base.IndexPartial();
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
            DeleteSuccessFinished(t);
            return DeleteSuccessResult();
        }

        public ActionResult IsDocumentEmpty(int id)
        {
            var document = entities.Set<T>().Where(x => x.Id == id).Select(x => x.Document).FirstOrDefault();
            if (document == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult ShowDocument(int id)
        {
            var document = entities.Set<T>().Where(x => x.Id == id).Select(x => x.Document).FirstOrDefault();

            string contentType = "";
            ContentDisposition cd = null;
            if (document == null)
            {
                return null;
            }
            //if (document.body != null && document.PathToFile == null)
            //{
            //    var data = document.body;
            //    if (data == null)
            //    {
            //        return null;
            //    }

            //    contentType = MimeMapping.GetMimeMapping(document.Filename);

            //    cd = new System.Net.Mime.ContentDisposition
            //    {
            //        FileName = document.Filename,
            //        Inline = false,
            //    };

            //    //Response.AppendHeader("Content-Disposition", cd.ToString());
            //    return File(data, contentType);
            //}
            //var filePath = Path.Combine(Server.MapPath(PATH), document.PathToFile);
            var filedata = FileSystemHelper.GetFileByName(document.PathToFile);
            contentType = MimeMapping.GetMimeMapping(document.Filename);

            cd = new System.Net.Mime.ContentDisposition
            {
                FileName = document.Filename,
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());


            return File(filedata, contentType);
            //var data = document.body;
            //if (data == null)
            //{
            //    return null;
            //}
            //if (document.Id_DocumentFormat == 1)
            //{
            //    return File(data, "application/pdf");
            //}
            //else
            //{
            //    return File(data, "image/jpg");
            //}
        }

        protected virtual void AddEditModelReady(IModel<T> model, T t)
        {

        }
        public ActionResult AddEdit(int id, bool isDebug = false)
        {
            if (!ParseInnerParametersForAddEdit())
            {
                return null;
            }
            PrepareViewBagForAddEdit();
            ViewBag.AJAX = isDebug;
            T t = null;
            if (id == 0)
            {
                t = InitNewItem();
                t.Document = new Document();
            }
            else
            {
                t = entities.Set<T>().FirstOrDefault(x => x.Id == id);
                if (t.Document == null)
                {
                    t.Document = new Document();
                }
            }

            ViewBag.MODEL = CreateModel();
            AddEditModelReady(ViewBag.MODEL, t);

            return View(t);
        }

        [HttpPost]
        public string AddEdit(FormCollection formData)
        {
            DevExpressHelper.Theme = Startup.THEME;
            var t = (T)new T().Parse(formData);
            var document = new Document();
            DocumentHelper.ParseDocument(ref document, formData, Request);

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
                entities.Set<T>().Add(t);
                entities.SaveChanges();
                AddEditFinished(t);
                return AddEditSuccessResult();
            }


            int oldDocumentId;
            var alwaysDocument = t as IEntityAlwaysWithDocument;
            if (alwaysDocument == null)
            {
                oldDocumentId =
                    entities.Set<T>().Where(x => x.Id == t.Id).Select(x => x.Id_Document).FirstOrDefault();
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
                AddEditFinished(t);
                return AddEditSuccessResult();
            }

            if (oldDocumentId == 0)
            {
                entities.Entry(t).State = EntityState.Modified;
                if (document.Filename != null)
                {
                    t.Document = document;
                    entities.Document.Add(document);
                }

                entities.SaveChanges();
                AddEditFinished(t);
                return AddEditSuccessResult();

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
                AddEditFinished(t);
                return AddEditSuccessResult();
            }

            entities.Entry(t).State = EntityState.Modified;
            t.Id_Document = 0;

            entities.SaveChanges();
            AddEditFinished(t);
            return AddEditSuccessResult();


            return "";


            //var t = (T)new T().Parse(formData);
            //var document = new Document();
            //ParseDocument(ref document, formData);

            //if (t.Id == 0)
            //{
            //    t.Document = document;
            //    t.Id_Document = null;
            //    t.Document = null;
            //    if (document.body != null)
            //    {
            //        t.Document = document;
            //        t.Id_Document = document.Id;
            //        entities.Document.Add(t.Document);
            //    }
            //    entities.Set<T>().Add(t);
            //    entities.SaveChanges();
            //    AddEditFinished(t);
            //    return AddEditSuccessResult();
            //}

            //int oldDocumentId;
            //var alwaysDocument = t as IEntityAlwaysWithDocument;
            //if (alwaysDocument == null)
            //{
            //    oldDocumentId =
            //        entities.Set<T>().Where(x => x.Id == t.Id).Select(x => x.Id_Document).FirstOrDefault() ?? 0;
            //}
            //else
            //{
            //    oldDocumentId = AlwaysDocuemntExtender.GetAlwaysDocuemntId<Worker_document>(t, entities);
            //}
            //if ((oldDocumentId) == document.Id && document.Id != 0)
            //{
            //    entities.Entry(t).State = EntityState.Modified;
            //    t.Id_Document = oldDocumentId;
            //    var oldDocument = entities.Document.FirstOrDefault(x => x.Id == oldDocumentId);
            //    oldDocument.Name = document.Name;
            //    oldDocument.Description = document.Description;
            //    oldDocument.Id_DocumentFormat = document.Id_DocumentFormat;

            //    entities.SaveChanges();
            //    AddEditFinished(t);
            //    return AddEditSuccessResult();
            //}

            //if (oldDocumentId == 0)
            //{
            //    entities.Entry(t).State = EntityState.Modified;
            //    if (document.body != null)
            //    {
            //        t.Document = document;
            //        entities.Document.Add(document);
            //    }

            //    entities.SaveChanges();
            //    AddEditFinished(t);
            //    return AddEditSuccessResult();

            //}

            //if (document.body != null)
            //{
            //    entities.Entry(t).State = EntityState.Modified;
            //    t.Id_Document = oldDocumentId;
            //    var oldDocument = entities.Document.FirstOrDefault(x => x.Id == oldDocumentId);
            //    oldDocument.Name = document.Name;
            //    oldDocument.Description = document.Description;
            //    oldDocument.Id_DocumentFormat = document.Id_DocumentFormat;
            //    oldDocument.body = document.body;

            //    entities.SaveChanges();
            //    AddEditFinished(t);
            //    return AddEditSuccessResult();
            //}

            //entities.Entry(t).State = EntityState.Modified;
            //t.Id_Document = null;

            //entities.SaveChanges();
            //AddEditFinished(t);
            //return AddEditSuccessResult();

            ////entities.Entry(t).State = EntityState.Modified;

            ////if (t.Document == null)
            ////{
            ////    t.Id_Document = null;
            ////    entities.SaveChanges();
            ////    return null;
            ////}

            ////var isDirty = Convert.ToBoolean(formData["IsDirty"]);
            ////if (!isDirty)
            ////{
            ////    entities.SaveChanges();
            ////    return null;
            ////}

            ////var oldDocument = entities.Set<T>().Where(x => x.Id == t.Id).Select(x => x.Document).FirstOrDefault();
            ////oldDocument = oldDocument ?? new Document();

            ////oldDocument.filename = document.filename;
            ////oldDocument.Name = document.Name;
            ////oldDocument.body = document.body;
            ////oldDocument.Description = document.Description;
            ////oldDocument.Id_DocumentFormat = document.Id_DocumentFormat;

            ////if (oldDocument.Id == 0)
            ////{
            ////    entities.Document.Add(oldDocument);
            ////    t.Document = oldDocument;
            ////}

            ////entities.SaveChanges();
            //return null;
        }




        //protected void ParseDocument(ref Document document, FormCollection formData)
        //{
        //    //Отследить изменение документа
        //    document.Id = DataTypeParser.Int(formData["document_Id"]);
        //    document.Name = DataTypeParser.String(formData["Document.Name"]);
        //    document.Id_DocumentFormat = DataTypeParser.Int(formData["Document.Id_DocumentFormat_VI"]);

        //    document.Description = DataTypeParser.String(formData["Document.Description"]);

        //    document.Filename = null;

        //    if (Request.Files.Count > 0)
        //    {
        //        var fileContent = Request.Files[0];
        //        if (fileContent != null && fileContent.ContentLength > 0)
        //        {
        //            document.Filename = fileContent.FileName;
        //            var filename = Guid.NewGuid().ToString();
        //            var filePath = FileSystemHelper.MapFileName(filename);//Path.Combine(Server.MapPath(PATH), System.IO.Path.GetFileName(filename));


        //            using (FileStream fs = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
        //            {
        //                byte[] bytes = new byte[3000];
        //                int bytesRead;
        //                while ((bytesRead = fileContent.InputStream.Read(bytes, 0, bytes.Length)) > 0)
        //                {
        //                    fs.Write(bytes, 0, bytesRead);
        //                }



        //                //using (FileStream file = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
        //                //    fs.CopyTo(file);

        //                //fs.ToArray();
        //            }
        //            document.PathToFile = filename;
        //        }
        //    }

        //    ////Отследить изменение документа
        //    //document.Id = DataTypeParser.Int(formData["document_Id"]);
        //    //document.Name = DataTypeParser.String(formData["Document.Name"]);
        //    //document.Id_DocumentFormat = DataTypeParser.Int(formData["Document.Id_DocumentFormat_VI"]);
        //    //document.Description = DataTypeParser.String(formData["Document.Description"]);

        //    //if (Request.Files.Count > 0)
        //    //{
        //    //    var fileContent = Request.Files[0];
        //    //    if (fileContent != null && fileContent.ContentLength > 0)
        //    //    {
        //    //        document.filename = fileContent.FileName;
        //    //        using (MemoryStream fs = new MemoryStream())
        //    //        {
        //    //            byte[] bytes = new byte[3000];
        //    //            int bytesRead;
        //    //            while ((bytesRead = fileContent.InputStream.Read(bytes, 0, bytes.Length)) > 0)
        //    //            {
        //    //                fs.Write(bytes, 0, bytesRead);
        //    //            }
        //    //            document.body = fs.ToArray();
        //    //        }
        //    //    }
        //    //    //var fileContent = Request.Files[0];
        //    //    //if (fileContent != null && fileContent.ContentLength > 0)
        //    //    //{
        //    //    //    document.filename = fileContent.FileName;
        //    //    //    var stream = fileContent.InputStream;
        //    //    //    document.body = new byte[fileContent.ContentLength];
        //    //    //    stream.Read(document.body, 0, fileContent.ContentLength);
        //    //    //}
        //    //}
        //}
    }

    public static class AlwaysDocuemntExtender
    {
        public static int GetAlwaysDocuemntId<T>(object t, MedlicenseSessionEntities entities) where T : class, IEntityWithId, IEntityAlwaysWithDocument, new()
        {
            var s = (T)t;
            return entities.Set<T>().Where(x => x.Id == s.Id).Select(x => x.Id_Document).FirstOrDefault();
        }
    }

    //public interface IEntityWithDocument
    //{
    //    int? Id_Document { get; set; }
    //    Document Document { get; set; }

    //}

    //public interface IEntityAlwaysWithDocument
    //{
    //    int Id_documents { get; set; }
    //}

    //public interface IEntityView
    //{

    //}
}