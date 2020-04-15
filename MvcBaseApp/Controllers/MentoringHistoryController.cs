using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Resources;

namespace MvcBaseApp.Controllers
{
    public class MentoringHistoryController : BaseManyToManyController<MentoringHistory>
    {
  
      //тут айдишники всех сущностей, по которым можно получить список. 
        //в данном случае мы берём поездки только по Worker-у, можно ещё брать по цели и стране\
        //создаются при каждом запросе в момент разбора пришедшего URL
        private int _Id_Mentoring;

        //Разбор пришедшего URL для построения гриды
        protected override bool ParseInnerParametersForIndex()
        {
            return Parse_Id_Mentoring();
        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {
            ViewBag.MentoringHistory_Id_Mentoring = _Id_Mentoring;
            ViewBag.MentoringHistory_Mentoring = entities.Mentoring.Where(x => x.Id == _Id_Mentoring).Select(x => x.Name).FirstOrDefault();
        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Mentoring();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            ViewBag.MentoringHistory_Id_Mentoring = _Id_Mentoring;
        }

        //Разбор пришедшего URL для создания новой поездки
        protected override bool ParseInnerParametersForAddEdit()
        {
            return Parse_Id_Mentoring();
        }
        //Эти параметры через ViewBag используются на вьюшке
        //В данном случае их нет, но теоретически может понадобиться какя-то новая фигня
        protected override void PrepareViewBagForAddEdit()
        {
        }

        //Создание новой сущности
        protected override MentoringHistory InitNewItem()
        {
            return new MentoringHistory() { Id_Mentoring = _Id_Mentoring };
        }

		//Разбор входящих параметров
        private bool Parse_Id_Mentoring()
        {
            if (!int.TryParse(Request.QueryString["Id_Mentoring"], out _Id_Mentoring))
            {
                return false;
            }
            return true;
        }
		
		protected override void FillModel(IndexGridModel<MentoringHistory> model)
        {
            model.IndexPrefix = entities.Mentoring.Where(x => x.Id == _Id_Mentoring).Select(x => x.Name).FirstOrDefault() + ": ";
            model.AdditionalUrlParamenter = "&Id_Mentoring=" + _Id_Mentoring;
        }
        //Создание модели
        protected override IModel<MentoringHistory> CreateModel()
        {
            var model = new MentoringHistoryAddEditModel();
model.EvaluationList = entities.Evaluation.ToList();
model.RecomendationList = entities.Recomendation.ToList();
model.SubjectList = entities.Subject.ToList();
            return model;
        }


    }

    public class MentoringHistoryAddEditModel : IModel<MentoringHistory>
    {
public List<Evaluation> EvaluationList { get; set; }
public List<Recomendation> RecomendationList { get; set; }
public List<Subject> SubjectList { get; set; }
    }
}
