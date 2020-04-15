using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class DocTemplForReqElemType: IDictionaryEntity
    {
        public IParsable Parse(FormCollection formData)
        {
 Id = DataTypeParser.Int(formData["Id"]);
Name_ru = DataTypeParser.String(formData["Name_ru"]);
Name_kg = DataTypeParser.String(formData["Name_kg"]);
Description_ru = DataTypeParser.String(formData["Description_ru"]);
Description_kg = DataTypeParser.String(formData["Description_kg"]);
CODE = DataTypeParser.String(formData["CODE"]);
 Id_Document = DataTypeParser.Int(formData["Id_Document"]);
 Id_RequestElementType = DataTypeParser.Int(formData["Id_RequestElementType"]);
			
            return this;
        }
    }
}
