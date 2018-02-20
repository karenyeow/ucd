using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Comlib.Common.Framework.Attributes
{

    public class ValidLicenceTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly List<string> _validLicenceTypes = new List<string>()
        {
            "Recreational Fishing Fee",
            "Competency Card",
            "Boat Driver Licence",
            "Vessel Registration"
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var countryValue = (string)value;
            return _validLicenceTypes.Contains(
                countryValue.ToUpper().Trim(), StringComparer.InvariantCultureIgnoreCase) ? ValidationResult.Success :
                new ValidationResult("Only following licence types are supported : " + string.Join(",", _validLicenceTypes.ToArray()));
        }
    }
}
