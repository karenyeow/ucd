using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Comlib.Common.Framework.Attributes
{
    public class EnsureIntegerAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            //  int intValue = 0;


            if (value.ToString().All(char.IsNumber)) return ValidationResult.Success;
            return new ValidationResult(validationContext.DisplayName + " must be numeric", new List<string>() { validationContext.MemberName });

       

        }
    }

}
