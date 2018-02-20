using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.Common.Framework.Attributes
{
 public    class ValidRangeAttribute : ValidationAttribute
    {

        private double _min;
        private double _max;
        public ValidRangeAttribute (double min, double max)
        {
            this._min = min;
            this._max = max;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null) return ValidationResult.Success;

            if (!Double.TryParse(value.ToString(), out double dblValue)) return ValidationResult.Success;
            if (dblValue >= this._min && dblValue <= this._max) return ValidationResult.Success;

            return new ValidationResult(validationContext.DisplayName + " must be >= " + this._min.ToString()  + " and <= " + this._max.ToString(), new List<string>() { validationContext.MemberName });
        }
    }
}
