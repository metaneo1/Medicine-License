using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class ApplicationUser : IDictionaryEntity
    {

        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            FullName = DataTypeParser.String(formData["FullName"]);
            phone1 = DataTypeParser.String(formData["phone1"]);
            phone2 = DataTypeParser.String(formData["phone2"]);
            email = DataTypeParser.String(formData["email"]);
            DOB = DataTypeParser.DateNull(formData["DOB"]);
            Id_Sex = DataTypeParser.Int(formData["Id_Sex_AddMessage_VI"]);
            IsActive = DataTypeParser.String(formData["IsActive"]) == "C";
            //AspUserId = DataTypeParser.String(formData["AspUserId"]);

            return this;
        }

        public string Name_ru { get; set; }
        public string Name_kg { get; set; }
        public string CODE { get; set; }
        public string Description_kg { get; set; }
        public string Description_ru { get; set; }
    }
}
