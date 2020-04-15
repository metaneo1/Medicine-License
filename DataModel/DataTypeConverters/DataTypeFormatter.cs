using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using MvcBaseApp.Resources;

namespace DataModel.DataTypeConverters
{
    public static class DataTypeFormatter
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string DateFormat = "dd/MM/yyyy";
        public static string DateTime(DateTime value)
        {
            return value.ToString(DateFormat);
        }
        public static string DateTimeNull(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString(DateFormat) : "";
        }

        //public static string Bool(bool value)
        //{
        //    return value ? Labels.Yes : Labels.No;
        //}

        //public static string BoolNull(bool? value)
        //{
        //    return (value ?? false) ? Labels.Yes : Labels.No;
        //}

        public static string Int(int value)
        {
            return value.ToString();
        }

        public static string IntNull(int? value)
        {
            return value.HasValue ? value.Value.ToString() : "";
        }

        public static string String(string value)
        {
            return value;
        }
    }
}