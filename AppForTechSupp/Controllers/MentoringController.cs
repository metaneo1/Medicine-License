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
    public class MentoringController : BaseManyToManyController<Mentoring>
    {
  
      //тут айдишники всех сущностей, по которым можно получить список. 
        //в данном случае мы берём поездки только по Worker-у, можно ещё брать по цели и стране\
        //создаются при каждом запросе в момент разбора пришедшего URL
        private int _Id_Person;

        //Разбор пришедшего URL для построения гриды
        protected override bool ParseInnerParametersForIndex()
        {
            return Parse_Id_Person();
        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {
            ViewBag.Mentoring_Id_Person = _Id_Person;
            ViewBag.Mentoring_Workers = entities.Workers.Where(x => x.Id == _Id_Person).Select(x => x.Name).FirstOrDefault();
        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Person();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            ViewBag.Mentoring_Id_Person = _Id_Person;
        }

        //Разбор пришедшего URL для создания новой поездки
        protected override bool ParseInnerParametersForAddEdit()
        {
            return Parse_Id_Person();
        }
        //Эти параметры через ViewBag используются на вьюшке
        //В данном случае их нет, но теоретически может понадобиться какя-то новая фигня
        protected override void PrepareViewBagForAddEdit()
        {
        }

        //Создание новой сущности
        protected override Mentoring InitNewItem()
        {
            return new Mentoring() { Id_Person = _Id_Person };
        }

		//Разбор входящих параметров
        private bool Parse_Id_Person()
        {
            if (!int.TryParse(Request.QueryString["Id_Person"], out _Id_Person))
            {
                return false;
            }
            return true;
        }
		
		protected override void FillModel(IndexGridModel<Mentoring> model)
        {
            model.IndexPrefix = entities.Workers.Where(x => x.Id == _Id_Person).Select(x => x.Name).FirstOrDefault() + ": ";
            model.AdditionalUrlParamenter = "&Id_Person=" + _Id_Person;
        }
        //Создание модели
        protected override IModel<Mentoring> CreateModel()
        {
            var model = new MentoringAddEditModel();
model.WorkersList = entities.Workers.ToList();
            return model;
        }


    }

    public class MentoringAddEditModel : IModel<Mentoring>
    {
public List<Workers> WorkersList { get; set; }
    }
}
