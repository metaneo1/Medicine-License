using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBaseApp.Models
{
    public class SimpleDictionaryAddEditModel<T>
    {
        public T Item { get; set; }
        public string Controller { get; set; }
    }
}