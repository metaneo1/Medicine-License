using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBaseApp.Controllers;

namespace MvcBaseApp
{
    public partial class Evaluation: IDictionaryEntity
    {
        public IParsable Parse(FormCollection formData)
        {
 Id = Convert.ToInt32(formData["Id"]);
Name = Convert.ToString(formData["Name"]);
Description = Convert.ToString(formData["Description"]);
			
            return this;
        }
    }
}
