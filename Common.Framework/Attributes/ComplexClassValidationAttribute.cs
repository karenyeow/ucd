using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.Common.Framework.Attributes
{
    public class ComplexClassValidationAttribute : ValidationAttribute

    {

         
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
     

            var result = ValidationResult.Success;

            var nestedValidationProperties = value.GetType().GetProperties()

            .Where(p => IsDefined(p, typeof(ValidationAttribute)))

            .OrderBy(p => p.Name);

            foreach (var property in nestedValidationProperties)

            {

                var validators = GetCustomAttributes(property, typeof(ValidationAttribute)) as ValidationAttribute[];

                if (validators == null || validators.Length == 0) continue;

                foreach (var validator in validators)

                {
                    
                    var propertyValue = property.GetValue(value, null);
              
                    result = validator.GetValidationResult(propertyValue, new ValidationContext(propertyValue, null, null) { MemberName = property.Name, DisplayName = validationContext.DisplayName  +@"\"+ property.Name});
                   
                    //if (result == ValidationResult.Success) continue;

                    //isValid = false;

                    //break;

                }

                //if (!isValid)

                //{

                //    break;

                //}

            }

            return result;
        }



    }
}
