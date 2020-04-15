using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Interfaces
{
    public interface IDictionaryEntity : IEntityWithId, IParsable
    {
        string Name_ru { get; set; }
        string Name_kg { get; set; }
        string CODE { get; set; }
        string Description_kg { get; set; }
        string Description_ru { get; set; }
    }
}
