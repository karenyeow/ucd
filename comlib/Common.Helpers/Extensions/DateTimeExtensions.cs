using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Comlib.Common.Helpers.Extensions
{
    public static  class DateTimeExtensions
    {
        public static string ToUniversalTimeString(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        private static Dictionary<int, string> _monthDict = new Dictionary<int, string>()
        {
              {1, "Jan"}, {2, "Feb"}, {3, "Mar"}, {4,"Apr"}, {5, "May"}, {6, "Jun"}, {7, "Jul"}, {8, "Aug"}, {9, "Sep"}, {10, "Oct"}, {11, "Nov"}, {12, "Dec"}
        };

        private static Dictionary<string, int> _monthDict2 = new Dictionary<string, int>()
        {
              {"Jan", 1}, {"Feb", 2}, {"Mar", 3}, {"Apr", 4}, {"May", 5}, {"Jun", 6}, {"Jul", 7}, {"Aug", 8}, {"Sep", 9}, {"Oct", 10}, {"Nov",11}, { "Dec", 12}

        };

        public static string GetShortMonthName(int month)
        {
            return _monthDict.ContainsKey(month) ? _monthDict[month] : string.Empty;
        }

        public static int? ToNumericMonth(string shortMonthName)
        {
            if (_monthDict2.ContainsKey(shortMonthName)) return _monthDict2[shortMonthName];
            else return null;
        }
        public static DateTime? ConvertToDateTime(string year, string month, string day)
        {
            DateTime dateOut;
            if (DateTime.TryParse(year + " " + month + " " + day, out dateOut))
            {
                return dateOut;
            }
            else
            {
                return null;
            }
        }
        public static DateTime? ConvertToDateTime(string year, string month, string day, string hour, string min)
        {
            DateTime dateOut;
            if (string.IsNullOrEmpty(hour) && string.IsNullOrEmpty(min))
                return DateTimeExtensions.ConvertToDateTime(year, month, day);
            if (DateTime.TryParse(year + " " + month + " " + day + " " + hour + ":" + min, out dateOut))
            {
                return dateOut;
            }
            else
            {
                return null;
            }
        }

        public static string ToDDMMYYYY(this DateTime dt)
        {
            return dt.Day.To2DigitString() + dt.Month.To2DigitString() + dt.Year.ToString();
        }
        public static bool IsBetween(this DateTime dt, int startHour, int startMinute, int endHour, int endMinute)
        {
            DateTime startDateTime = DateTime.Parse(dt.Date.ToString("dd MMM yyyy ") + startHour.To2DigitString() + ":" + startMinute.To2DigitString());
            DateTime endDateTime = DateTime.Parse(dt.Date.ToString("dd MMM yyyy ") + endHour.To2DigitString() + ":" + endMinute.To2DigitString());
            return DateTime.Compare(dt, startDateTime) >= 0 && DateTime.Compare(dt, endDateTime) <= 0;
        }

        public static bool IsGreaterOrEqualTo(this DateTime dt, int hour, int minute)
        {
            DateTime otherDateTime = DateTime.Parse(dt.Date.ToString("dd MMM yyyy ") + hour.To2DigitString() + ":" + minute.To2DigitString());
            return DateTime.Compare(dt, otherDateTime) >= 0;

        }

        public static bool IsLessorOrEqualTo(this DateTime dt, int hour, int minute)
        {
            DateTime otherDateTime = DateTime.Parse(dt.Date.ToString("dd MMM yyyy ") + hour.To2DigitString() + ":" + minute.To2DigitString());
            return DateTime.Compare(dt, otherDateTime) <= 0;
        }

        public static bool IsNullOrFutureDate(this DateTime? dt, bool excludeToday = true)
        {
            if (!dt.HasValue)
            {
                return true;
            }
            if (excludeToday)
                return dt.Value.Date > DateTime.Today;

            if (dt.Value.Hour > 0 || dt.Value.Minute > 0 || dt.Value.Second > 0)
                return dt.Value >= DateTime.Now;
            else
                return dt.Value.Date >= DateTime.Today;
        }

        public static bool IsNullOrPastOrCurrentDate(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return true;
            }

            return dt.Value.Date >= DateTime.Today;
        }

        public static string ToSiebelDateFormatString(this DateTime dt)
        {
            return dt.ToString("MM/dd/yyyy");
        }

        public static string ToSiebelDateFormatString(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToSiebelDateFormatString();
            else
                return null;
        }

        public static string ToSiebelDateTimeFormatString(this DateTime dt)
        {
            return dt.ToString("MM/dd/yyyy HH:mm");
        }
        public static string ToIsoFormatString(this DateTime? dt)
        {
            return dt?.ToString("yyyy-MM-dd HH':'mm':'ss") ?? string.Empty;
        }

        public static string ToSiebelDateTimeFormatStringFromString(this string dt)
        {
            return (Convert.ToDateTime(dt)).ToString("MM/dd/yyyy HH:mm");
        }

        public static string ToSiebelDateTimeIncludeSecondFormatString(this DateTime dt)
        {
            return dt.ToString("MM/dd/yyyy HH:mm:ss");
        }

        public static string ToSiebelDateTimeIncludeSecondFormatString(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToSiebelDateTimeIncludeSecondFormatString();
            else
                return null;
        }

        public static DateTime ParseSiebelDateTimeFormatString(this string str)
        {
            DateTime dt;
            if (DateTime.TryParseExact(str, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;
            else if (DateTime.TryParseExact(str, "MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;
            else
                return DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }



        public static string ToLongDateTimeNoSecond(this DateTime dt)
        {
            return string.Format("{0} {1}", dt.ToString("dd MMM yyyy"), dt.ToShortTimeString());
        }

        public static string ToLongDateNoDate(this DateTime dt)
        {
            return dt.ToString("dd MMM yyyy");
        }

        public static string ToShortDateStringIfDateTimeIsNotMinDate(this DateTime dt)
        {
            if (DateTime.MinValue == dt || DateTime.Compare(dt, new DateTime(1900, 01, 01)) == 0)
            {
                return string.Empty;
            }

            return dt.ToShortDateString();
        }


        public static string ToWSDate(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToWSDate() : DateTime.Now.ToWSDate();
        }
        public static string ToWSDate(this DateTime dt)
        {
            return dt.ToString();
        }

        public static string ToDateString(this DateTime? dt, string format = null)
        {
            if (dt.HasValue)
            {
                if (string.IsNullOrWhiteSpace(format))
                    return dt.Value.ToShortDateString();
                else
                {
                    return dt.Value.ToString(format);
                }
            }
            else
                return null;
        }
        public static string ToMediumDateTimeString(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToMediumDateTimeString();
            else
                return null;
        }

        public static string ToMediumDateString(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToMediumDateString();
            else
                return null;
        }
        public static string ToMediumDateString(this DateTime dt)
        {

            return dt.ToString("dd MMM yyyy");

        }
        public static string ToMediumDateTimeString(this DateTime dt)
        {

            return dt.ToString("dd MMM yyyy HH:mm:ss");

        }
        public static DateTime SetYear(this DateTime dt, int year)
        {
            if (dt.Month == 2 && dt.Day == 29)
                return new DateTime(year, 3, 1, dt.Hour, dt.Minute, dt.Second);
            else
                return new DateTime(year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}
