using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Comlib.Common.Framework.Attributes
{
    public class ValidDateFormat : ValidationAttribute
    {
        private string _dateFormat = string.Empty;
        private ValidateModeEnum _validateMode;
        public enum ValidateModeEnum { Date, Time}



        public ValidDateFormat (ValidateModeEnum validateMode, string dateFormat)
        {
            _validateMode = validateMode;
            this._dateFormat = dateFormat;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var valueStr = value.ToString();
            if (this._validateMode == ValidateModeEnum.Date) {

                if (DateTime.TryParseExact(valueStr, this._dateFormat, null, DateTimeStyles.None, out DateTime parsedDate)) return ValidationResult.Success;

                return new ValidationResult(validationContext.DisplayName + " must be a valid date"  , new List<string>() { validationContext.MemberName });

            }
            else
            {
                var timeRegularExpression = new Regex("^([0-1][0-9]|[2][0-3])([0-5][0-9])$");
                if (timeRegularExpression.IsMatch(valueStr)) return ValidationResult.Success;
                return new ValidationResult(validationContext.DisplayName + " must be a valid time", new List<string>() { validationContext.MemberName });

            }

        }

     
    }
}
