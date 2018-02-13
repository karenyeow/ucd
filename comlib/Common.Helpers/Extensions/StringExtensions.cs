
using ICP.Utilities.Format;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Comlib.Common.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this IEnumerable<string> list, string str)
        {
            return list.Any(x => x.Equals(str, StringComparison.OrdinalIgnoreCase));
        }

        public static string Joined(this IEnumerable<string> list, string seperator)
        {
            return string.Join(seperator, list);
        }

        public static bool IsExists(this string[] strArray, string strValue, bool ignoreCase)
        {
            return strArray.Where(x => string.Compare(x, strValue, ignoreCase) == 0).Count() != 0;
        }

        public static bool IsValidEmailFormat(this string str)
        {
            string strRegex = @"^([0-9a-zA-Z]([-.'\w]*[0-9a-zA-Z])*[-.\w]*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            return str.Trim().IsValidExpression(strRegex);
        }

        public static bool IsFirstLastNameInValidFormat(this string str)
        {
            string strRegex = @"^[A-Za-z\p{Lu}\p{Ll} ]+['-.\w]*[A-Za-z ]+$";


            if (!str.IsValidExpression(strRegex) && str.Trim() != ".") return false;
            else return true;
        }

        public static bool IsAlphaNumeric(this string str)
        {
            string strRegex = @"^[A-Za-z0-9 ]*$";

            if (!str.IsValidExpression(strRegex) && str.Trim() != ".") return false;
            else return true;
        }

        public static bool IsNumeric(this string str)
        {
            string strRegex = @"^[0-9]+$";

            if (!str.IsValidExpression(strRegex)) return false;
            else return true;
        }

        public static bool IsValidIntegerFormat(this string str)
        {

            return int.TryParse(str, out int res);
        }


        public static bool IsValidExpression(this string str, string regularExpression)
        {
            Regex re = new Regex(regularExpression);
            if (re.IsMatch(str))
                return true;
            else return false;
        }


        public static string Break(this string str, int charCount)
        {
            List<string> result = new List<string>();
            string inputString = str;
            while (inputString.Length > 0)
            {
                if (inputString.Length > charCount)
                {
                    string strPart = inputString.Substring(0, charCount);
                    result.Add(strPart);
                    inputString = inputString.Replace(strPart, string.Empty);
                }
                else
                {
                    result.Add(inputString);
                    inputString = string.Empty;
                }
            }
            return result.Count == 0 ? string.Empty : (result.Count == 1 ? result[0] : string.Join("<br/>", result.ToArray()));

        }

        /// <summary>
        /// Replaces newLine chars with HTML breaks <br/>, for displaying in HTML
        /// </summary>
        /// <param name="str"></param>
        /// <param name="removeEmptyLines"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public static string Break(this string str, bool removeEmptyLines = false, string newLine = "\n\r")
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            StringSplitOptions options = removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            List<string> result = str.Split(newLine.ToCharArray(), options).ToList();
            return string.Join("<br/>", result.ToArray());
        }

        /// <summary>
        /// Adds the supplied breakChar in the string after every 50th char
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charSpacing"></param>
        /// <param name="breakChar"></param>
        /// <returns></returns>
        public static string BreakOneLiner(this string str, int charSpacing = 50, string breakChar = "\r\n")
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            IEnumerable<string> strings = str.Split(charSpacing);
            return string.Join(breakChar, strings);
        }

        public static string FormatEmailWithBreak(this string email, string BreakChar = "<br />")
        {
            if (email.IsNullOrEmptyAfterTrim()) return email;
            if (!email.Contains("@")) return email;
            return email.Insert(email.IndexOf("@") + 1, BreakChar);
        }
        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            if (str != null)
            {
                if (str.Length <= chunkSize)
                    return new List<string>() { str };

                int count = str.Length / chunkSize;
                if (str.Length % chunkSize > 0)
                    count++;

                return Enumerable.Range(0, count)
                    .Select(i => str.Substring(i * chunkSize, (i * chunkSize + chunkSize) > str.Length ? str.Length - i * chunkSize : chunkSize));
            }
            return null;
        }

        public static string[] SplitStringAndTrim(this string original, char separator = ',')
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(separator)
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

        public static string NAIfEmpty(this string str)
        {
            return string.IsNullOrEmpty(str) ? "NA" : str;
        }

        public static string Coalesce(this string str)
        {
            return str ?? string.Empty;
        }

        public static string CoalesceFormat(this string str, string format)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            return string.Format(format, str);
        }

        public static string NullIfEquals(this string str, string compareTo)
        {
            return str.Equals(compareTo) ? null : str;
        }

        /// <summary>
        /// Strips the whitespace from the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string Strip(this string str)
        {
            if (str == null)
                return null;

            Regex r = new Regex(@"\s+");
            string strippedStr = r.Replace(str, @"");
            return strippedStr;
        }
        public static string StripAllNonNumeric(this string str, string exceptions = null)
        {
            var regStr = string.Format(@"[^{0}]", "0-9" + exceptions);
            var r = new Regex(regStr);
            var strippedStr = r.Replace(str, @"");
            return strippedStr;
        }

        public static string StripAllNonAlphaNumeric(this string str)
        {
            Regex r = new Regex(@"[^A-Za-z0-9]");
            string strippedStr = r.Replace(str, @"");
            return strippedStr;
        }

        public static string StripAllNonAlpha(this string str)
        {
            Regex r = new Regex(@"[^A-Za-z]");
            string strippedStr = r.Replace(str, @"");
            return strippedStr;
        }

        public static string StripAllNonNumeric(this string str)
        {
            Regex r = new Regex(@"[^0-9]");
            string strippedStr = r.Replace(str, @"");
            return strippedStr;
        }

        public static string CleanStringForSQL(this string str)
        {
            return str.IsNullOrEmptyAfterTrim() ? str : str.Replace("'", "''");
        }

        public static string CleanStringForSiebel(this string str)
        {
            return str.IsNullOrEmptyAfterTrim() ? str : str.Replace(' ', '_').Replace('>', '_').Replace('*', '_').Replace('%', '_').Replace('$', '_').Replace('<', '_').Replace("\"", "").Replace("'", "").Replace("&", "and").Replace("?", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("^", "_");
        }

        public static string CleanStringForSiebel2(this string str)
        {
            return str.IsNullOrEmptyAfterTrim() ? str : CleanStringForSiebel(str).Replace('/', '-');
        }

        /// <summary>
        /// Returns the left most characters in the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">The amount of chars to be returned from the left</param>
        /// <returns></returns>
        public static string Left(this string str, int length)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > length)
                    result = str.Substring(0, length);
                else
                    result = str;
            }

            return result;
        }

        /// <summary>
        /// Returns the right most characters in the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">The amount of chars to be returned from the right</param>
        /// <returns></returns>
        public static string Right(this string str, int length)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > length)
                    result = str.Substring(str.Length - length);
                else
                    result = str;
            }

            return result;
        }

        public static bool IsNullOrEmptyAfterTrim(this string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Length == 0;
        }

        public static bool ContainsIgnoreCase(this string str, string value)
        {
            if (!string.IsNullOrEmpty(str))
                return str.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
            else
                return false;
        }

        public static bool ContainsWholeWord(this string str, string value, RegexOptions matchOptions = RegexOptions.IgnoreCase)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Regex.IsMatch(str, string.Format("\\b{0}\\b", value), matchOptions);
            }
            else
                return false;
        }

        public static bool? ToBool(this Char? value)
        {
            if (!value.HasValue) return false;
            char charVal = value.Value;
            if (charVal.ToString().ToLower().Trim() == "n") return false;
            if (charVal.ToString().ToLower().Trim() == "y") return true;
            return null;
        }
        public static bool ToBool(this Char value)
        {

            if (value.ToString().ToLower().Trim() == "n") return false;
            if (value.ToString().ToLower().Trim() == "y") return true;
            return false;
        }

        public static string ToString(this Char? value)
        {
            if (!value.HasValue) return null;
            char charVal = value.Value;
            if (charVal.ToString().ToLower().Trim() == "n") return "n";
            if (charVal.ToString().ToLower().Trim() == "y") return "y";
            return null;
        }

        public static bool? ToBool(this string value)
        {
            if (!value.IsNullOrEmptyAfterTrim())
            {
                if (value.ToString().ToLower().Trim() == "n" || value.ToString().ToLower().Trim() == "false") return false;
                if (value.ToString().ToLower().Trim() == "y" || value.ToString().ToLower().Trim() == "true") return true;
            }
            return null;
        }

        public static DateTime? ToDateTime(this string value)
        {
            if (value.IsNullOrEmptyAfterTrim()) return null; 

            if (DateTime.TryParse(value, out DateTime dt))
                return dt;
            else
                return null;
        }

        public static DateTimeOffset? ParseIso8601(this string value, string format)
        {
            if (value.IsNullOrEmptyAfterTrim()) return null;
  
            if (DateTimeOffset.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset parsedDateTimeOffSetVal))
            {
                return parsedDateTimeOffSetVal;
            }
            else
            {
                return null;
            }
        }
        public static int ToInt(this string value)
        {

            if (int.TryParse(value, out int  intVal))
                return intVal;
            else
                throw new ArgumentException("Unable to convert value " + value + " to integer");

        }
        public static string ToYesNoString(this string value)
        {
            if (!value.IsNullOrEmptyAfterTrim())
            {
                if (value.Equals("Y", StringComparison.CurrentCultureIgnoreCase)) return "Yes";
                if (value.Equals("N", StringComparison.CurrentCultureIgnoreCase)) return "No";
                if (value.Equals("U", StringComparison.CurrentCultureIgnoreCase)) return "Unknown";
            }
            return null;
        }
        public static string ToYNString(this string value)
        {
            if (!value.IsNullOrEmptyAfterTrim())
            {
                if (value.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)) return "Y";
                if (value.Equals("True", StringComparison.CurrentCultureIgnoreCase)) return "Y";
                if (value.Equals("No", StringComparison.CurrentCultureIgnoreCase)) return "N";
                if (value.Equals("False", StringComparison.CurrentCultureIgnoreCase)) return "N";
                if (value.Equals("Unknown", StringComparison.CurrentCultureIgnoreCase)) return "U";
            }
            return null;
        }
        /// <summary>
        /// Gets the Hour component from Time string, e.g. "12:00 AM", or "12AM" or "12 AM"
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public static string GetHourStringFromSiebelTimeString(this string time)
        {
            string timeNoAMPM = string.Empty, result = string.Empty;
            int AMIndex = time.IndexOf("AM");
            int PMIndex = time.IndexOf("PM");

            if (AMIndex == -1 && PMIndex == -1)
            {
                timeNoAMPM = time.Trim();
            }
            else
            {
                if (AMIndex == -1) { timeNoAMPM = time.Substring(0, PMIndex); }
                else { timeNoAMPM = time.Substring(0, AMIndex); }
            }
            int columnIndex = timeNoAMPM.IndexOf(":");
            if (columnIndex == -1) { result = timeNoAMPM.Trim(); }
            else { result = timeNoAMPM.Substring(0, columnIndex); }
            return time;
        }

        /// <summary>
        /// Gets the minute component from Time string, e.g. "12:00 AM", or "12AM" or "12 AM"
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public static string GetMinuteStringFromSiebelTimeString(this string time)
        {
            string timeNoAMPM = string.Empty, result = string.Empty;
            int AMIndex = time.IndexOf("AM");
            int PMIndex = time.IndexOf("PM");

            if (AMIndex == -1 && PMIndex == -1)
            {
                timeNoAMPM = time.Trim();
            }
            else
            {
                if (AMIndex == -1) { timeNoAMPM = time.Substring(0, PMIndex); }
                else { timeNoAMPM = time.Substring(0, AMIndex); }
            }
            int columnIndex = timeNoAMPM.IndexOf(":");
            if (columnIndex == -1) { result = string.Empty; }
            else { result = timeNoAMPM.Substring(columnIndex + 1, timeNoAMPM.Length - 3); }
            return time;
        }

        public static List<string> GetDifference(this List<string> theList, List<string> theOtherList)
        {
            var inter = (from i in theList
                         select i).Except(
                  from o in theOtherList
                  select o).Distinct().ToList();
            return inter;
        }

        public static string ToHHMMSSFormat(this string time)
        {
            return time + ":00";
        }

        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var titlecase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());

            ////StringBuilder sb = new StringBuilder();
            string[] temp = titlecase.Split(new char[] { ',', '-', '\'', '(' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in temp.Where(x => !x.IsNullOrEmptyAfterTrim()))
            {
                if (!char.IsUpper(s[0]))
                {
                    titlecase = titlecase.ReplaceAt(titlecase.IndexOf(s), char.ToUpper(s[0])); //just replace first letter of each word
                }
                //sb.Append(s.Left(1).ToUpper() + s.Substring(1).ToLower());
            }
            return titlecase;
        }

        public static string ReplaceFirst(this string str, string oldValue, string newValue)
        {
            int pos = str.IndexOf(oldValue);
            if (pos < 0)
            {
                return str;
            }
            return str.Substring(0, pos) + newValue + str.Substring(pos + oldValue.Length);
        }

        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                return null;
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        private static RSACryptoServiceProvider Cryptographer()
        {
            CspParameters param = new CspParameters
            {
                KeyContainerName = "GlSOnlineRSA",
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            return new RSACryptoServiceProvider(param);
        }

        public static string EncryptString(this string str, string salt)
        {
            byte[] bytes = new UnicodeEncoding().GetBytes(salt + str);
            byte[] encBytes = Cryptographer().Encrypt(bytes, false);
            return Convert.ToBase64String(encBytes);
        }

        public static string DecryptString(this string str, string salt)
        {
            byte[] encBytes = Convert.FromBase64String(str);
            byte[] bytes = Cryptographer().Decrypt(encBytes, false);
            string result = new UnicodeEncoding().GetString(bytes);
            if (result.StartsWith(salt))
                return result.Substring(salt.Length);
            else
                throw new CryptographicException("Incorrect salt supplied");
        }

        public static string ToDelimitedString(this IEnumerable<string> lst, string delimiter, bool applyQuotes = false)
        {
            if (lst == null || lst.Count() == 0)
                return null;

            StringBuilder sb = new StringBuilder();

            foreach (string s in lst)
            {
                sb.Append(string.Format("{0}{1}{2}{3}", applyQuotes ? "\"" : string.Empty, s, applyQuotes ? "\"" : string.Empty, delimiter));
            }

            return sb.ToString().TrimEnd(delimiter.ToCharArray());
        }

        /// <summary>
        /// Determines whether the string is in the format of a address title reference
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTitleReference(this string str)
        {
            Regex r = new Regex(@"^([A-Za-z0-9]*/[A-Za-z0-9]*/(dp|sp|DP|SP)[A-Za-z0-9]+)$");
            if (!str.IsNullOrEmptyAfterTrim() && r.IsMatch(str.Trim()))
                return true;
            else
                return false;
        }

        public static bool ListEquals<T>(this IList<T> list1, IList<T> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list2.Contains(list1[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a truncated form of the string with "..." indicating it has been masked
        /// </summary>
        /// <param name="str"></param>
        /// <param name="totalLength"></param>
        /// <param name="excludeCharsAfter">If specified, the chars after the supplied param will not be masked, but will be included in the total length count</param>
        /// <returns></returns>
        public static string MaskLongString(this string str, int totalLength, string excludeCharsAfter = null, string mask = "...")
        {
            if (str.Length <= totalLength)
                return str;

            string temp = str;
            string remaining = string.Empty;
            if (!string.IsNullOrEmpty(excludeCharsAfter) && str.LastIndexOf(excludeCharsAfter) >= 0)
            {
                temp = str.Substring(0, str.LastIndexOf(excludeCharsAfter));
                remaining = str.Substring(str.LastIndexOf(excludeCharsAfter), str.Length - str.LastIndexOf(excludeCharsAfter));
            }
            temp = temp.Substring(0, totalLength - mask.Length - remaining.Length);

            return temp + mask + remaining;
        }

        public static bool IsValidBritishDate(this string strDate)
        {
            var date = DateTime.MinValue;
            return !DateTime.TryParseExact(strDate, new string[] { "dd/MM/yyyy", "d/MM/yyyy", "d/M/yyyy" }, new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out date);
        }

        public static bool DateStringIsNullOrIsWatermark(this string strDate)
        {
            return strDate.IsNullOrEmptyAfterTrim() || (string.Compare(strDate, "dd/mm/yyyy", true) == 0);
        }

        public static string TrimIfNotNull(this string stringToBeTrim)
        {
            return stringToBeTrim == null ? stringToBeTrim : stringToBeTrim.Trim();
        }

        public static string RemoveNonUTFForXML(this string in_string, out bool bNonUTFFound)
        {
            bNonUTFFound = false;
            if (string.IsNullOrWhiteSpace(in_string)) return in_string;

            StringBuilder sbOutput = new StringBuilder();
            char ch;

            for (int i = 0; i < in_string.Length; i++)
            {
                ch = in_string[i];
                if ((ch >= 0x0020 && ch <= 0xD7FF) ||
                        (ch >= 0xE000 && ch <= 0xFFFD) ||
                        ch == 0x0009 ||
                        ch == 0x000A ||
                        ch == 0x000D)
                {
                    sbOutput.Append(ch);
                }
                else if (!bNonUTFFound)
                    bNonUTFFound = true;
            }
            return sbOutput.ToString();
        }

        /// <summary>
        /// Decodes a base64EncodedString to a string.
        /// </summary>
        /// <param name="value">Base64 encoded string string containing the bytes to decode.</param>
        /// <returns><see cref="string"/> value representing the decoded <paramref name="value"/>.</returns>
        public static string DecodeBase64(this string value)
        {
            if (value != null)
            {
                byte[] bytes = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(bytes);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Encodes a string to a base64EncodedString.
        /// </summary>
        /// <param name="value">String to encode to a base base64 Encoded String.</param>
        /// <returns><see cref="string"/> value representing the base64 Encoded <paramref name="value"/>.</returns>
        public static string EncodeBase64(this string value)
        {
            if (value != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                return null;
            }
        }

        public static bool IsValidIntegerWithRegexCheck(this string str)
        {
            var isIntegerRegex = new Regex(@"^\d+$", RegexOptions.Compiled);
            return isIntegerRegex.IsMatch(str);
        }


        public static T ToWSEnum<T>(this string s)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), s, true);
            }
            catch
            {

                //remove it when reference data is ready
                var values = (T[])Enum.GetValues(typeof(T));
                return values[0];

                //log 
                //throw;
            }
        }





        public static DateTime? ToWSNullDate(this string s)
        {
            try
            {
                return DateTime.Parse(s);
            }
            catch
            {
                //remove it when reference data is ready

                return null;

                //log 
                //throw;
            }
        }


        public static DateTime ToWSDate(this string s)
        {
            try
            {
                return DateTime.Parse(s);
            }
            catch
            {
                //remove it when reference data is ready

                return DateTime.Now;

                //log 
                //throw;
            }
        }


        public static int ToWSInt(this Enum s)
        {
            try
            {
                return int.Parse(s.ToString().Replace("_", ""));
            }
            catch
            {
                //remove it when reference data is ready

                return 0;

                //log 
                //throw;
            }
        }


        public static bool ToWSBoolean(this string s)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch
            {
                //remove it when reference data is ready

                return false;

                //log 
                //throw;
            }
        }


        public static bool ToWSBoolean(this int? s)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch
            {
                //remove it when reference data is ready

                return false;

                //log 
                //throw;
            }
        }



        //this formats the address for google URL submission, to replace space or enter to '+'.
        public static string ToGoogleDistanceURLFormat(this string location)
        {
            return location.Replace(" ", "+").Replace("\r\n", "+").Replace("&", "and"); ;
        }

        public static string FormatWebsiteURL(this string URL)
        {
            var url = URL.Trim();
            return url.ContainsIgnoreCase("http://") ? url : string.Format("http://{0}", url);
        }

        public static Decimal ToDecimal(this string decimalStr)
        {
            return NumericExtension.ParseDecimal(decimalStr);
        }

        public static int? ToNullableInt(this string intStr)
        {
  
            if (int.TryParse(intStr, out int intValue))
            {
                return intValue;
            }
            else
            {
                return null;
            }
        }


        public static string ToFormatWithFirstLetterUpperCaseAndOthersLowerCase(this string input)
        {
            var result = string.Empty;

            if (!input.IsNullOrEmptyAfterTrim())
            {
                var firstChar = input.Substring(0, 1);
                var theRestChars = input.Substring(1, input.Length - 1);
                result = string.Format("{0}{1}", firstChar.ToUpper(), theRestChars.ToLower());
            }
            return result;
        }

        public static string ToFileSize(this int l)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }

        public static string FindMatchInList(string searchText, List<string> source)
        {
            if (!searchText.IsNullOrEmptyAfterTrim())
            {
                var searchBucket = searchText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string result = string.Empty;
                foreach (var searchTerm in searchBucket)
                {
                    foreach (var value in source)
                    {
                        if (value.ContainsIgnoreCase(searchTerm))
                        {
                            return value;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static string Replace(this string source, string newString, StringComparison comp)
        {
            int index = source.IndexOf(newString, comp);

            // Determine if we found a match
            bool MatchFound = index >= 0;

            if (MatchFound)
            {
                // Remove the old text
                source = source.Remove(index, newString.Length);

                // Add the replacemenet text
                source = source.Insert(index, "<b>" + newString + "</b>");
            }

            return source;
        }

        public static string SubstituteIf(this string current, string substitution, Func<string, bool> condition)
        {
            if (condition(current))
                return substitution;
            else
            {
                return current;
            }
        }

        public static string MakeMatchingWordBold(string searchTerms, string value)
        {
            var result = value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var searchBucket = searchTerms.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var formattedWords = new List<string>();
            foreach (var word in result)
            {
                var formattedWord = word;
                bool isMatched = false;
                foreach (var term in searchBucket)
                {
                    if (word.ContainsIgnoreCase(term))
                    {
                        var substring = word.Substring(word.IndexOf(term, StringComparison.InvariantCultureIgnoreCase),
                            term.Length);
                        formattedWords.Add(word.Replace(substring, "<span class=\"wordbold\">" + substring + "</span>"));
                        isMatched = true;
                        break;
                    }

                }
                if (!isMatched) formattedWords.Add(formattedWord);
            }
            return string.Join(" ", formattedWords);
            //foreach (var term in searchBucket)
            //{
            //    if (result.ContainsIgnoreCase(term))
            //    {
            //        int index = result.IndexOf(term, StringComparison.InvariantCultureIgnoreCase);

            //        // Determine if we found a match
            //        bool MatchFound = index >= 0;

            //        if (MatchFound)
            //        {
            //            // Remove the old text
            //            substring = result.Substring(index, term.Length);
            //            result = result.Remove(index, term.Length);

            //            // Add the replacemenet text
            //            result = result.Insert(index, "<span class=\"wordbold\">" + substring + "</span>");
            //        }
            //    }
            //}

            //return result;
        }

        public static DateTime? SiebelDateTimeStringToDateTime(this string siebelDateTimeString)
        {
            if (siebelDateTimeString.IsNullOrEmptyAfterTrim()) return null;
            DateTimeFormatInfo siebelDateTimeFormat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy HH:mm:ss",
                DateSeparator = "/"
            };
            try
            {
                return Convert.ToDateTime(siebelDateTimeString, siebelDateTimeFormat);
            }
            catch
            {
                return siebelDateTimeString.ToDateTime();
            }
        }



        public static string Coalesce(this string str, string coalesce = null)
        {
            return string.IsNullOrWhiteSpace(str) ? (coalesce ?? string.Empty) : str;
        }
        public static void WriteToFile(this string Str, string Filename)
        {
            var text = string.Empty;
            if (File.Exists(Filename))
            {
                text = File.ReadAllText(Filename);
            }
            if (text.IsNullOrEmptyAfterTrim()) text += $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}:    {Str}";
            else text += $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}:    {Str}";
            File.WriteAllText(Filename, text);
        }

        public static string ToSafeFileName(this string s)
        {
            return s != null ? s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "") : string.Empty;
        }
    }
}
