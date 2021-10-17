using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreApi.MeterData.BL
{
    public static class StringExtensions
    {
        public static List<string> CsvLineToList(this string values)
        {
            return string.IsNullOrEmpty(values) ? new List<string>() : values.Split(",").ToList();
        }
    }
}
