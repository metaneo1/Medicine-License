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
    public class QuestionTypeController : BaseDictionaryController<QuestionType>
    {
         protected override string AddEditWrapperName { get { return "AddEdit"; } }

        [ValidateInput(false)]
        public ActionResult TreeListPartial(string SearchText, int? Id_QuestionType)
        {
            ViewData["SearchText"] = SearchText;
            ViewData["Id_QuestionType"] = Id_QuestionType;
            var model = entities.QuestionType.ToList();

            //QuestionType GetLocalGovernment(int groupId) => model.FirstOrDefault(g => g.Id == groupId);
            //int GetDepth(QuestionType g) => (g == null) ? 0 : (g.Id_Parent.HasValue ? GetDepth(GetLocalGovernment(g.Id_Parent.Value)) + 1 : 1);

            return PartialView("_TreeListPartial", model.Select(x => new QuestionTypeController.QuestionTypeTreeModel
            {
                Name = x.Name_ru,
                ParentId = x.Id_Parent,
                Id = x.Id
            }).ToList());
        }

        public ActionResult TreeListCustomPartial(string SearchText, bool? isNewSearch)
        {
            ViewData["IsNewSearch"] = isNewSearch;
            ViewData["SearchText"] = SearchText;
            var model = entities.QuestionType.ToList();


            return PartialView("_TreeListPartial", model.Select(x => new QuestionTypeController.QuestionTypeTreeModel
            {
                Name = x.Name_ru,
                ParentId = x.Id_Parent,
                Id = x.Id
            }).ToList());
        }
        public class QuestionTypeTreeModel
        {
            public string Name { get; set; }
            public int? ParentId { get; set; }
            public int Id { get; set; }
        }
    }
}
