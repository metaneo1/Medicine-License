using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBaseApp.Controllers;

namespace MvcBaseApp
{
    public partial class MentoringHistory : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
Id = DataTypeParser.IntNull(formData["Id"]);
Date = DataTypeParser.DateTimeNull(formData["Date"]);
Description = DataTypeParser.String(formData["Description"]);
Notes = DataTypeParser.String(formData["Notes"]);
Id_Mentoring = DataTypeParser.IntNull(formData["Id_Mentoring"]);
 Id_Subject = DataTypeParser.IntNull(formData["Id_Subject_AddMentoringHistory_VI"]);
 Id_Evaluation = DataTypeParser.IntNull(formData["Id_Evaluation_AddMentoringHistory_VI"]);
 Id_Recomendation = DataTypeParser.IntNull(formData["Id_Recomendation_AddMentoringHistory_VI"]);
            return this;
        }
    }
}

