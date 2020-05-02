using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Shared.Extensions
{
    public static class StringExtensions
    {
        public static DateTime? ToDateTime(this string value, string pattern) =>
            !string.IsNullOrEmpty(value)
                ? DateTime.ParseExact(value, pattern, CultureInfo.InvariantCulture)
                : (DateTime?)null;

        public static decimal? ToDecimal(this string value) =>
            !string.IsNullOrEmpty(value)
                ? decimal.Parse(value, CultureInfo.InvariantCulture)
                : (decimal?)null;

        public static double? ToDouble(this string value) =>
            string.IsNullOrEmpty(value) ? (double?)null : double.Parse(value);

        public static string Substring(this string value, string pattern) =>
            new Regex(pattern).Match(value).Value;
    }
}
