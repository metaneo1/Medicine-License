using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class Sex : IEntityWithId, IParsable, IDictionaryEntity
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Name_ru = DataTypeParser.String(formData["Name_ru"]);
            Name_kg = DataTypeParser.String(formData["Name_kg"]);
            Description = DataTypeParser.String(formData["Description"]);
            Code = DataTypeParser.String(formData["Code"]);

            return this;
        }

        public string CODE { get; set; }
        public string Description_kg { get; set; }
        public string Description_ru { get; set; }
    }
}
