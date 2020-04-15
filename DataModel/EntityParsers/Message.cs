using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;
using DataModel.Interfaces;

namespace DataModel
{
    public partial class Message : IEntityWithId, IParsable
    {
        public IParsable Parse(FormCollection formData)
        {
            Id = DataTypeParser.Int(formData["Id"]);
            Id_Parent = DataTypeParser.IntNull(formData["Id_Parent_AddMessage_VI"]);
            Id_Request = DataTypeParser.Int(formData["Id_Request"]);
            MessageText = DataTypeParser.String(formData["MessageText"]);
            MessageDate = DataTypeParser.DateTimeNull(formData["MessageDate"]);
            Id_QuestionType = DataTypeParser.IntNull(formData["Id_QuestionType_AddMessage_VI"]);
            Id_AnswerWriter = DataTypeParser.Int(formData["Id_AnswerWriter"]);
            Id_Document = DataTypeParser.IntNull(formData["Id_Document"]);
            return this;
        }

        public DocumentInRequest Document
        {
            get { return MessageForSentDocument.Select(x=>x.DocumentInRequest).FirstOrDefault(); }
        }

        private int? DocumentId = 0;
        public int? Id_Document
        {
            get
            {
                if (DocumentId == 0)
                {
                    var result = MessageForSentDocument.Select(x => x.Id_SentDocument).FirstOrDefault();
                    DocumentId = result == 0 ? (int?)null : result;
                }

                return DocumentId;
            }
            set { DocumentId = value; }
        }
    }
}

