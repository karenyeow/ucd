using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Comlib.Common.Framework.Attributes
{
    public class ValidYearManufactureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int intValue = (int)value;
            if (intValue >= 1800 && intValue <= DateTime.Now.Year + 1)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(intValue + "Value must be from 1800 to " + DateTime.Now.Year + ".");
        }
    }
}
