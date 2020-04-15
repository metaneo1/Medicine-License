using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class RequestStateHistory : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Id_User = DataTypeParser.IntNull(formData["Id_User"]);
            Id_Request = DataTypeParser.Int(formData["Id_Request"]);
            Id_State = DataTypeParser.Int(formData["Id_State_AddRequestStateHistory_VI"]);
            DateStatusChange = DataTypeParser.DateTimeNull(formData["DateStatusChange"]);
            return this;
        }
    }
}

