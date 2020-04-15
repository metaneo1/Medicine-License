using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Interfaces
{
    public interface IEntityAlwaysWithDocument
    {
        int Id_Document { get; set; }
        Document Document { get; set; }
    }
}
