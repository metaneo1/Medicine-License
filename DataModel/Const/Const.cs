using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Const
{
    public static class Const
    {
        public static class RequestStateId
        {
            public static int Unassigned { get; set; }
            public static int Finished { get; set; }
            public static int InProgress { get; set; }
            public static int Reopen { get; set; }
        }

        public static class DocumentStateId
        {
            public static int Backlog { get; set; }
            public static int Accepted { get; set; }
            public static int InProgress { get; set; }
            public static int Reopened { get; set; }
        }

        public static class DocumentFormatId
        {
            public static int Pdf { get; set; }
            public static int Image { get; set; }
        }

        public static class RequestElementStructureTypeId
        {
            public static int Group { get; set; }
            public static int SubGroup { get; set; }
            public static int Element { get; set; }
        }


        public static class FilePath
        {
            public static string PATH { get; set; }
        }
    }
}
