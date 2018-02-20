using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Comlib.Common.Framework.Attributes
{

    public class ValidLocalCountryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var localCountry = ConfigurationManager.AppSettings["AddressEntityLocalCountry"]?.ToUpperInvariant();
            var countryValue = (string)value;
            return localCountry.Equals(countryValue.ToUpper().Trim(), StringComparison.InvariantCulture) ? ValidationResult.Success :
                new ValidationResult("Address must be from " + localCountry);
        }
    }
}
