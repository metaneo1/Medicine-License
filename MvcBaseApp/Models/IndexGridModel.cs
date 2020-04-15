using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBaseApp.Models
{
    public class IndexGridModel<T>: IndexGridModel
    {
        public string EntityName { get; set; }
        public string AdditionalUrlParamenter { get; set; }
        public string IndexPrefix { get; set; }
        public T Entity { get; set; }
        public IEnumerable<T> List { get; set; }
        public bool IsCallBack { get; set; }
    }

    public interface IndexGridModel
    {
        string EntityName { get; set; }
        string AdditionalUrlParamenter { get; set; }
        string IndexPrefix { get; set; }
        bool IsCallBack { get; set; }
    }
}