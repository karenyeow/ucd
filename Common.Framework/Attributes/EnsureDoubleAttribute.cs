using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Comlib.Common.Framework.Attributes
{
    public class EnsureDoubleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            //  int intValue = 0;

            if (Double.TryParse(value.ToString(), out double result)) return ValidationResult.Success;
            return new ValidationResult(validationContext.DisplayName + " must be numeric", new List<string>() { validationContext.MemberName });

       

        }
    }

}
