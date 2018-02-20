using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Comlib.Common.Framework.Attributes
{
    public class ValidLengthLimitAttribute : ValidationAttribute
    {
        int length;
        public ValidLengthLimitAttribute(int length)
        {
            this.length = length;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            string stringValue =value.ToString();
            if (stringValue.Length <= length)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(validationContext.DisplayName  + " must only contain up to " + length + " characters in length", new List<string>() { validationContext.MemberName });
        }
    }
}
