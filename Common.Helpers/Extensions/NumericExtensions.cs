using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers.Extensions
{

        public static class NumericExtension
        {

            public static bool ToBool(this int? value)
            {
                if (value.HasValue)
                {
                    return value.Value.ToBool();

                }
                return false;
            }
            public static bool ToBool(this int value)
            {
                if (value == 1) return true;
                else return false;
            }
            public static object ToDBValue(this int? val)
            {
                if (val.HasValue) return val.Value;
                else return DBNull.Value;

            }
            public static string To2DigitString(this int val)
            {
                string intString = val.ToString();

                return intString.Length == 1 ? intString.PadLeft(2, Char.Parse("0")) : intString;
            }

            public static bool IsValid(string valStr)
            {
                return IsValidInt(valStr) || IsValidDecimal(valStr);
            }

            public static bool IsValidInt(string valStr, bool dollarSignNotAllowed = false, bool commaNotAllowed = false)
            {
                int intVal;
                return int.TryParse(StripChars(valStr, dollarSignNotAllowed, commaNotAllowed), out intVal);
            }

            public static int ParseInt(string valStr)
            {
                return int.Parse(StripChars(valStr));
            }

            public static bool IsValidDecimal(string valStr, bool dollarSignNotAllowed = false, bool commaNotAllowed = false)
            {
                decimal decimalVal;
                return decimal.TryParse(StripChars(valStr, dollarSignNotAllowed, commaNotAllowed), out decimalVal);
            }

            public static decimal ParseDecimal(string valStr)
            {
                return decimal.Parse(StripChars(valStr));
            }

            public static string StripChars(string valStr, bool dollarSignNotAllowed = false, bool commaNotAllowed = false)
            {
                var result = valStr.Trim();
                if (!dollarSignNotAllowed)
                {
                    result = result.Replace("$", "");
                }

                if (!commaNotAllowed)
                {
                    result = result.Replace(",", "");
                }
                return result;
            }
        }
}
