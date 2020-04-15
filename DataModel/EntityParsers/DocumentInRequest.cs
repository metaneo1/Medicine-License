using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class DocumentInRequest : IEntityWithId, IParsable, IEntityAlwaysWithDocument
    {

        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Id_Request = DataTypeParser.Int(formData["Id_Request"]);
            Id_Document = DataTypeParser.Int(formData["document_Id"]);
            Id_DocumentState = DataTypeParser.Int(formData["Id_DocumentState_AddDocumentInRequest_VI"]);
            Id_RequestElement = DataTypeParser.IntNull(formData["Id_RequestElement"]);
            Description = DataTypeParser.String(formData["Description"]);
            return this;
        }

    }
}
