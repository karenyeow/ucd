using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comlib.Common.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidListAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            return list?.Count > 0;
        }
    }
}