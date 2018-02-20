using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class MinorInjuryClass: BaseSiraClass
    {

        [Display(Name = @"minorInjury\assessmentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string assessmentDate { get; set; }
        [Display(Name = @"minorInjury\assessmentType")]
        [ValidLengthLimit(50)]
        public string assessmentType { get; set; }

        [Display(Name = @"minorInjury\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }
        [Display(Name = @"minorInjury\reason")]
        [ValidLengthLimit(50)]
        public string reason { get; set; }
    }
}