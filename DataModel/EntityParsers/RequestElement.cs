using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class RequestElement: IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
Id = DataTypeParser.Int(formData["Id"]);
Id_Parent = DataTypeParser.Int(formData["Id_Parent"]);
IsActive = DataTypeParser.String(formData["IsActive"]) == "C";
OrderNumber = DataTypeParser.Int(formData["OrderNumber"]);
 Id_RequestElemType = DataTypeParser.Int(formData["Id_RequestElemType"]);
 Id_TemplateDocument = DataTypeParser.Int(formData["Id_TemplateDocument"]);
 Id_RequestType = DataTypeParser.Int(formData["Id_RequestType"]);
 Id_StructureType = DataTypeParser.Int(formData["Id_StructureType"]);
IsRequired = DataTypeParser.String(formData["IsRequired"]) == "C";
			
            return this;
        }
    }
}
