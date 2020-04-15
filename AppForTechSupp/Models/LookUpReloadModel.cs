using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBaseApp.Models
{
    public class LookUpReloadModel<T>
    {
        public List<T> List { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string JsElementName { get; set; }
        public string CallerConrtoller { get; set; }
        public int? CurrentValue { get; set; }
        public LookUpReloadButton Buttons { get; set; }
        public bool Disabled { get; set; }

        public int? AdditionalParameter { get; set; }

        public string HOLDER { get { return "Holder"; } }
        public string INNER { get { return "Inner"; } }
    }

    public class LookUpReloadModelString<T>
    {
        public List<T> List { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string JsElementName { get; set; }
        public string CallerConrtoller { get; set; }
        public string CurrentValue { get; set; }
        public LookUpReloadButton Buttons { get; set; }
        public bool Disabled { get; set; }

        public int? AdditionalParameter { get; set; }

        public string HOLDER { get { return "Holder"; } }
        public string INNER { get { return "Inner"; } }
    }

    //public class LookUpReloadModel3
    //{
    //    public string ActionName { get; set; }
    //    public string ControllerName { get; set; }
    //    public string JsElementName { get; set; }
    //    public string CallerConrtoller { get; set; }
    //    public int? CurrentValue { get; set; }
    //    public LookUpReloadButton Buttons { get; set; }
    //    public bool Disabled { get; set; }

    //    public int? AdditionalParameter { get; set; }

    //    public string HOLDER { get { return "Holder"; } }
    //    public string INNER { get { return "Inner"; } }
    //}

    //public class LookUpReloadModel : LookUpReloadModel3
    //{
    //    public object List { get; set; }
    //}

    //public class LookUpReloadModel<T> : LookUpReloadModel3
    //{
    //    public object List { get; set; }
    //}

    [Flags]
    public enum LookUpReloadButton
    {
        //Должна всегда быть нулём
        None = 0,


        Edit = 1,
        Add = 2,
        Delete = 4,

        //Должна быть суммой всех значений
        All = 7,
    }
}