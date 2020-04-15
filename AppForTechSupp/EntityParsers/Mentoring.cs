using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBaseApp.Controllers;

namespace MvcBaseApp
{
    public partial class Mentoring : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
Id = DataTypeParser.IntNull(formData["Id"]);
DateStart = DataTypeParser.DateTimeNull(formData["DateStart"]);
DateFinish = DataTypeParser.DateTimeNull(formData["DateFinish"]);
 Id_Mentor = DataTypeParser.IntNull(formData["Id_Mentor_AddMentoring_VI"]);
Id_Person = DataTypeParser.IntNull(formData["Id_Person"]);
            return this;
        }
    }
}

