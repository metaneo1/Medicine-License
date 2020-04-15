using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class MessageForSentDocument : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
Id = DataTypeParser.Int(formData["Id"]);
Id_SentDocument = DataTypeParser.Int(formData["Id_SentDocument"]);
 Id_Message = DataTypeParser.Int(formData["Id_Message_AddMessageForSentDocument_VI"]);
            return this;
        }
    }
}

