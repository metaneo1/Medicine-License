using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class TypeAdminUnits : IEntityWithId, IParsable, IDictionaryEntity
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Name_kg = DataTypeParser.String(formData["Name_kg"]);
            Name_ru = DataTypeParser.String(formData["Name_ru"]);
            Description_kg = DataTypeParser.String(formData["Description_kg"]);
            Description_ru = DataTypeParser.String(formData["Description_ru"]);
            CODE = DataTypeParser.String(formData["CODE"]);

            return this;
        }

       
    }
}
