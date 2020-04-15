using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting.Shape;
using DevExpress.XtraReports.UI;
using Microsoft.AspNet.Identity;
using MvcBaseApp.Models;

namespace MvcBaseApp.Controllers
{
    [CustomAuthorizeFilter]
    public abstract class EntityControllerController<T> : Controller where T : class
    {
        private HrmSessionEntities _entities = null;

        protected HrmSessionEntities entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = new HrmSessionEntities((string)Session[InPageAutorizeController.DB_SESSION_KEY]);
                }
                return _entities;
            }
        }

        private readonly string CONTROLLER_NAME = typeof(T).Name;

        protected virtual object GetDataForExport()
        {
            return null;
        }

        protected string GetControllerName()
        {
            return CONTROLLER_NAME;
        }

        protected abstract void FillModel(IndexGridModel<T> model);

        protected IndexGridModel<T> CreateIndexModel()
        {
            var model = new IndexGridModel<T>() { EntityName = GetControllerName() };
            FillModel(model);
            return model;
        }

        protected ActionResult Index(bool isDebug = false, string viewName = "Index")
        {
            ViewBag.AJAX = isDebug;
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View(viewName, model);
        }

        protected ActionResult IndexPartial()
        {
            DevExpressHelper.Theme = Startup.THEME;
            var model = CreateIndexModel();
            return View("_Index", model);
        }



        protected virtual void generator_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {
            if (e.FieldName == "Discontinued")
            {
                XRShape control = new XRShape();
                control.SizeF = e.Owner.SizeF;
                control.LocationF = new System.Drawing.PointF(0, 0);
                e.Owner.Controls.Add(control);
                control.Shape = new ShapeStar()
                {
                    StarPointCount = 5,
                    Concavity = 30
                };
                control.BeforePrint += new System.Drawing.Printing.PrintEventHandler(control_BeforePrint);
                e.IsModified = true;
            }
        }

        protected virtual void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(((XRShape)sender).Report.GetCurrentColumnValue("Name")) == true)
                ((XRShape)sender).FillColor = Color.Yellow;
            else
                ((XRShape)sender).FillColor = Color.White;
        }

        protected virtual void generator_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
        }

        public ActionResult Export()
        {
            var md = CreateIndexModel();

            var model = GetDataForExport();

            MVCxGridViewState gridViewState = (MVCxGridViewState)Session["gv" + md.EntityName + "State"];

            if (gridViewState != null)
            {
                MVCReportGeneratonHelper generator = new MVCReportGeneratonHelper();
                generator.CustomizeColumnsCollection += new CustomizeColumnsCollectionEventHandler(generator_CustomizeColumnsCollection);
                generator.CustomizeColumn += new CustomizeColumnEventHandler(generator_CustomizeColumn);
                XtraReport report = generator.GenerateMVCReport(gridViewState, model);
                generator.WritePdfToResponse(Response, "test.pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
                return null;
            }
            else
                return View("Index");
        }
    }

    public class HrmSessionEntities : MedlicenseEntities
    {
        private string SessionId { get; set; }
        public HrmSessionEntities(string sessionId)
        {
            SessionId = sessionId;
        }

        public override int SaveChanges()
        {
            if (Database.Connection.State != ConnectionState.Open)
                Database.Connection.Open();

            var command = Database.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "declare @p as VARBINARY(128) set @p = CAST(N'" + SessionId +
                                  "' as VARBINARY(128)) SET CONTEXT_INFO @p";
            command.ExecuteNonQuery();

            return base.SaveChanges();
        }
    }

    public class CustomAuthorizeFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
            //var isAuthorized = base.AuthorizeCore(httpContext);

            //var rolesKey = httpContext.Session[InPageAutorizeController.UI_ROLES_KEY];
            //var userNameKey = httpContext.Session[InPageAutorizeController.DB_SESSION_KEY];

            //if (!isAuthorized)
            //{
            //    return false; //User not Authorized
            //}

            //else
            //{
            //    var entities = new MedlicenseEntities();
            //    if (rolesKey == null)
            //    {
            //        var id = httpContext.User.Identity.GetUserId();
            //        var dictionary = entities
            //            .AspNetUsers.Where(x => x.Id == id)
            //            .SelectMany(x => x.AspNetUserRoles)
            //            .Select(x => x.AspNetRoles)
            //            .SelectMany(x => x.RoleUIControl)
            //            .Select(x => new
            //            {
            //                UIColntol = x.UIColntol.Name,
            //                x.Id_AccessType
            //            })
            //            .ToList()
            //            .ToLookup(x => x.UIColntol, x => x.Id_AccessType);

            //        httpContext.Session[InPageAutorizeController.UI_ROLES_KEY] = dictionary;
            //    }
            //    if (userNameKey == null)
            //    {
            //        httpContext.Session[InPageAutorizeController.DB_SESSION_KEY] = Guid.NewGuid().ToString();
            //        var userSession = new UsersSessions()
            //        {
            //            Id_AspnetUser = httpContext.User.Identity.GetUserId(),
            //            Session_ID = (string)httpContext.Session[InPageAutorizeController.DB_SESSION_KEY],
            //            StartDate = DateTime.Now,
            //        };

            //        entities.UsersSessions.Add(userSession);
            //        entities.SaveChanges();
            //    }
            //    return true;
            //}
        }
    }
}