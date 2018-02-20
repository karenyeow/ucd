using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class InjurySeverityClass: BaseSiraClass
    {
        [Display(Name = @"injurySeverity\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }

        [Display(Name = @"injurySeverity\assessmentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string assessmentDate { get; set; }
    }
}