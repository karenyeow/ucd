using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Helpers.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToStringWith2DecimalPointOrEmpty(this decimal value)
        {
            var v = string.Format("{0:0.00}", value);
            if (v == "0.00")
            {
                return string.Empty;
            }
            return v;
        }

        public static string ToStringWith2DecimalPoint(this decimal value)
        {
            return string.Format("{0:0.00}", value);
        }


        public static string ToStringCurrency(this decimal value)
        {
            return string.Format("{0:c}", value);
        }

        public static string ToFormatAmount(this decimal value)
        {
            return string.Format("{0:C}", value);
        }


    }
}
