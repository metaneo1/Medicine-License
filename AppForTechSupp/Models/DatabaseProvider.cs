using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel;

namespace MvcBaseApp.Models
{
    public static class DatabaseProvider
    {
        const string LargeDatabaseDataContextKey = "DXLargeDatabaseDataContext";

        public static MedlicenseEntities DB
        {
            get {
                if(HttpContext.Current.Items[LargeDatabaseDataContextKey] == null)
                    HttpContext.Current.Items[LargeDatabaseDataContextKey] = new MedlicenseEntities();
                return (MedlicenseEntities)HttpContext.Current.Items[LargeDatabaseDataContextKey];
            }
        }

    }
}