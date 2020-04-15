using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Models;
using MvcBaseApp.Resources;
using DataModel;
namespace MvcBaseApp.Controllers
{
    public class AdminUnitsController : BaseDictionaryController<AdminUnits>
    {
        protected override string AddEditWrapperName { get { return "AddEdit"; } }

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
        public class AdminUnitTreeModel
        {
            public string Name { get; set; }
            public int? ParentId { get; set; }
            public int Id { get; set; }
        }

    }
}
