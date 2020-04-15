using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class AdminUnits : IDictionaryEntity
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            ParentId = DataTypeParser.IntNull(formData["ParentId"]);
            Name_ru = DataTypeParser.String(formData["Name_ru"]);
            Name_kg = DataTypeParser.String(formData["Name_kg"]);
            Description_kg = DataTypeParser.String(formData["Description_kg"]);
            Description_ru = DataTypeParser.String(formData["Description_ru"]);
            CODE = DataTypeParser.String(formData["CODE"]);
            IdTypeadm = DataTypeParser.Int(formData["IdTypeadm_AdminUnits_VI"]);
            Latitude = DataTypeParser.DecNull(formData["Latitude"]);
            Longitude = DataTypeParser.DecNull(formData["Longitude"]);
            Comment = DataTypeParser.String(formData["Comment"]);
            IsRayonCenter = DataTypeParser.BoolNull(formData["IsRayonCenter"]);
            
            return this;
        }

    }
}
