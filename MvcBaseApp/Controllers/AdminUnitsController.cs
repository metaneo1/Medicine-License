using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    public class AdminUnitsController : BaseDictionaryController<AdminUnits>
    {
        public class AdminUnitTreeModel
        {
            public string Name { get; set; }
            public int? ParentId { get; set; }
            public int Id { get; set; }
        }
    }
}
