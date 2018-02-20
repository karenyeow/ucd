using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Comlib.Common.Framework.Attributes
{
    public class ValidYesNoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex r = new Regex("^y$|^Y$|^n$|^N$");
            string stringValue = (string)value;
            if(r.IsMatch(stringValue)) return ValidationResult.Success;

            return new ValidationResult(stringValue + " is not either 'Y', 'y', 'N' or 'n'.");
        }
    }
}
