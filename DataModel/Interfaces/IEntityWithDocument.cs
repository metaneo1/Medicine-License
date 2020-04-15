using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataModel.Interfaces
{
    public interface IEntityWithDocument
    {
        int? Id_Document { get; set; }
        Document Document { get; set; }

    }
}
