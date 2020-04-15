using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class Organization : IDictionaryEntity
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Name_ru = DataTypeParser.String(formData["Name_ru"]);
            Name_kg = DataTypeParser.String(formData["Name_kg"]);
            Description_kg = DataTypeParser.String(formData["Description_kg"]);
            Description_ru = DataTypeParser.String(formData["Description_ru"]);
            CODE = DataTypeParser.String(formData["CODE"]);
            TIN = DataTypeParser.String(formData["TIN"]);
            NSCCode = DataTypeParser.String(formData["NSCCode"]);
            RegistrationNumber = DataTypeParser.String(formData["RegistrationNumber"]);
            Id_Type = DataTypeParser.Int(formData["Id_Type_AddOrganization_VI"]);
            Address = DataTypeParser.String(formData["Address"]);
            Id_AdminUnit = DataTypeParser.Int(formData["Id_AdminUnit"]);
            return this;
        }

    }
}
