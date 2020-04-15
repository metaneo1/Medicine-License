using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcBaseApp.Resources;
using MvcBaseApp.Models;
using System.Linq.Expressions;
using DataModel;

namespace MvcBaseApp.Controllers
{
    public class LicenseRequestActivityTypeController : BaseManyToManyController<LicenseRequestActivityType, LicenseRequest>
    {
  
      //тут айдишники всех сущностей, по которым можно получить список. 
        //в данном случае мы берём поездки только по Worker-у, можно ещё брать по цели и стране\
        //создаются при каждом запросе в момент разбора пришедшего URL
        private int _Id_Request;

        //Разбор пришедшего URL для построения гриды
        protected override bool ParseInnerParametersForIndex()
        {
            return Parse_Id_Request();
        }

        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndex()
        {
            ViewBag.LicenseRequestActivityType_Id_Request = _Id_Request;
            ViewBag.LicenseRequestActivityType_LicenseRequest = entities.LicenseRequest.Where(x => x.Id == _Id_Request).Select(x => x.Organization.Name_kg).FirstOrDefault();
        }

        //Разбор пришедшего URL для обновления гриды
        protected override bool ParseInnerParametersForIndexPartial()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        protected override void PrepareViewBagForIndexPartial()
        {
            ViewBag.LicenseRequestActivityType_Id_Request = _Id_Request;
        }

        //Разбор пришедшего URL для создания новой поездки
        protected override bool ParseInnerParametersForAddEdit()
        {
            return Parse_Id_Request();
        }
        //Эти параметры через ViewBag используются на вьюшке
        //В данном случае их нет, но теоретически может понадобиться какя-то новая фигня
        protected override void PrepareViewBagForAddEdit()
        {
        }

        //Создание новой сущности
        protected override LicenseRequestActivityType InitNewItem()
        {
            return new LicenseRequestActivityType() { Id_Request = _Id_Request };
        }

		//Разбор входящих параметров
        private bool Parse_Id_Request()
        {
            if (!int.TryParse(Request.QueryString["Id_Request"], out _Id_Request))
            {
                return false;
            }
            return true;
        }
		

        private System.Linq.Expressions.Expression<Func<LicenseRequest, bool>> GetPred()
        {
            return x => x.Id == _Id_Request;
        }
		protected override void FillModel(IndexGridModel<List<LicenseRequestActivityType>> model)
        {
             model.IndexPrefix = entities.LicenseRequest.Where(GetPred()).Select(x => x.Organization.Name_kg).FirstOrDefault(); 
            model.AdditionalUrlParamenter = "&Id_Request=" + _Id_Request;
        }
        protected override Expression<Func<LicenseRequestActivityType, bool>> GetFilterForParentEntity()
        {
            return x => x.Id_Request == _Id_Request;
        }
        //Создание модели
        protected override IModel<LicenseRequestActivityType> CreateModel()
        {
            var model = new LicenseRequestActivityTypeAddEditModel();
model.ActivityTypeList = entities.ActivityType.ToList();
            return model;
        }


    }

    public class LicenseRequestActivityTypeAddEditModel : IModel<LicenseRequestActivityType>
    {
public List<ActivityType> ActivityTypeList { get; set; }
    }
}
