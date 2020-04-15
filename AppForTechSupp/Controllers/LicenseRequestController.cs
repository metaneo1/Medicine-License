using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;

namespace MvcBaseApp.Controllers
{
    public class LicenseRequestController : BaseMainFormController<LicenseRequest>
    {
        // GET: LicenseRequest


        protected override LicenseRequest InitNewItem()
        {
            throw new NotImplementedException();
        }

        protected override IModel<LicenseRequest> CreateModel()
        {
            return new LicenseRequestAddEditModel();
        }

        protected override void FillModel(IndexGridModel<List<LicenseRequest>> model)
        {
            model.Entity = entities.LicenseRequest.ToList();
        }
    }

    public class LicenseRequestAddEditModel: IModel<LicenseRequest>
    {

    }
}