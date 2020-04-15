using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataTypeConverters
{
    public static class DateTimeUtils
    {
        public static string ToMainDateFormat(this DateTime dt)
        {
            return dt.ToString(DataTypeFormatter.DateFormat);
        }
        public static string ToMainDateTimeFormat(this DateTime dt)
        {
            return dt.ToString(DataTypeFormatter.DateTimeFormat);
        }
    }
}
