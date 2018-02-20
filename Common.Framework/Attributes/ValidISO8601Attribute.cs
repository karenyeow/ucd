using Comlib.Common.Helpers.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Comlib.Common.Framework.Attributes
{
    public  class ValidISO8601Attribute : ValidationAttribute
    {
        private string _isoDateFormat;
        public ValidISO8601Attribute(string format)
        {
            _isoDateFormat = format;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            
  

            if (value.ToString().ParseIso8601(_isoDateFormat).HasValue) return ValidationResult.Success;
            {
                return new ValidationResult(validationContext.DisplayName + " must be a valid date", new List<string>() { validationContext.MemberName });
            }


        }
        }
    }
