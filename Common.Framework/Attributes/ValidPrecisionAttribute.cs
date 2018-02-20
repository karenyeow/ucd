using Comlib.Common.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Comlib.Common.Framework.Attributes
{
    public class ValidPrecisionAttribute : ValidationAttribute
    {
        private int _numLength;
        private int _decLength;

        public ValidPrecisionAttribute (int numLength, int decLength)
        {
            _numLength = numLength;
            _decLength = decLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (!Double.TryParse(value.ToString(), out double result)) return new ValidationResult(validationContext.DisplayName + " must be numeric", new List<string>() { validationContext.MemberName });
            var valueArr =  value.ToString().SplitStringAndTrim(char.Parse ("."));



            if ((valueArr[0].Length > _numLength) || (valueArr.Count() == 2 && valueArr[1].Length > _decLength))
            {
                return new ValidationResult(validationContext.DisplayName +  " must not be more than  " + (_numLength + _decLength).ToString() + "  in length, precision " + _decLength.ToString() + " decimal place(s)", new List<string>() { validationContext.MemberName });
            }


            return ValidationResult.Success;



        }
    }

}
