using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers.Extensions
{
    public static class BooleanExtensions
    {
        public static string ToYNString(this bool boolValue)
        {
            return boolValue ? "Y" : "N";

        }

        public static string ToYNString(this bool? boolValue)
        {
            if (!boolValue.HasValue) return "N";
            return boolValue.Value.ToYNString();
        }

        public static bool IsTrue(this bool? value)
        {
            return value.HasValue && value.Value;
        }
    }
}
