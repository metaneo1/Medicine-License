using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.DataTypeConverters
{
    public static class DataTypeParser
    {
        public static int? IntNull(string str)
        {
            return !string.IsNullOrWhiteSpace(str)
                ? int.Parse(str)
                : (int?)null;
        }

        public static int Int(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            return int.Parse(str);
        }

        public static decimal? DecNull(string str)
        {
            return !string.IsNullOrWhiteSpace(str)
                ? decimal.Parse(str)
                : (decimal?)null;
        }

        public static decimal Dec(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            return decimal.Parse(str);
        }

        public static double? DoubleNull(string str)
        {
            return !string.IsNullOrWhiteSpace(str)
                ? double.Parse(str)
                : (double?)null;
        }

        public static double Double(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            return double.Parse(str);
        }

        public static DateTime? DateNull(string str)
        {
            return !string.IsNullOrWhiteSpace(str)
                ? System.DateTime.ParseExact(str, DataTypeFormatter.DateFormat, null)
                : (DateTime?)null;
        }

        public static DateTime Date(string str)
        {
            return System.DateTime.ParseExact(str, DataTypeFormatter.DateFormat, null);
        }

        public static DateTime? DateTimeNull(string str)
        {
            return !string.IsNullOrWhiteSpace(str)
                ? System.DateTime.ParseExact(str, DataTypeFormatter.DateTimeFormat, null)
                : (DateTime?)null;
        }

        public static DateTime DateTime(string str)
        {
            return System.DateTime.ParseExact(str, DataTypeFormatter.DateTimeFormat, null);
        }

        public static bool? BoolNull(string str)
        {
            return !string.IsNullOrWhiteSpace(str)
                ? Convert.ToString(str) == "C"
                : (bool?)null; 
        }

        public static bool Bool(string str)
        {
            return Convert.ToString(str) == "C";
        }

        public static string String(string str)
        {
            return str;
        }

    }
}