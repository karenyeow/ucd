using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Comlib.Common.Framework.Attributes
{
    public class ValidLengthAttribute : ValidationAttribute
    {
        int length;
        public ValidLengthAttribute(int length)
        {
            this.length = length;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            string stringValue = value.ToString();
            if (stringValue.Length == length)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(validationContext.DisplayName   + " must be exactly " + length + " characters long", new List<string>() { validationContext.MemberName  });
        }
    }
}
