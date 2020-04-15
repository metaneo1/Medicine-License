using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class LicenseRequestActivityType : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Id_Request = DataTypeParser.Int(formData["Id_Request"]);
            Id_Type = DataTypeParser.Int(formData["Id_Type_AddLicenseRequestActivityType_VI"]);
            return this;
        }
    }
}

