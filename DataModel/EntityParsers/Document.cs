using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class Document : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Description = DataTypeParser.String(formData["Description"]);
            Name = DataTypeParser.String(formData["Name"]);
            Filename = DataTypeParser.String(formData["Filename"]);
            PathToFile = DataTypeParser.String(formData["PathToFile"]);
            Id_DocumentFormat = DataTypeParser.Int(formData["Id_DocumentFormat"]);

            return this;
        }
    }
}
