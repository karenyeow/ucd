using Comlib.Common.Helpers.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;

namespace Comlib.Common.Framework.Attributes
{
    public class IsValidDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] dateFormats = { "dd/MM/yyyy" };
            var cultureInfo = new CultureInfo("en-AU");

            var dateValue = (string)value;
            if (dateValue.IsNullOrEmptyAfterTrim()) return ValidationResult.Success;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-AU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-AU");


            if (!DateTime.TryParseExact(dateValue, dateFormats, cultureInfo, DateTimeStyles.None, out DateTime theParsedDate))
            {
                return new ValidationResult(validationContext.MemberName + " has invalid date format.");
            }
            return ValidationResult.Success;

        }
    }
}
