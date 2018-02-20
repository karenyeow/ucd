using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class WPIClass:BaseSiraClass
    {
        [Display(Name = @"wpi\assessmentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string assessmentDate { get; set; }

        [Display(Name = @"wpi\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }

        [Display(Name = @"wpi\reason")]
        [ValidLengthLimit(50)]
        public string reason { get; set; }

        [Display(Name = @"wpi\assessmentType")]
        [ValidLengthLimit(50)]
        public string assessmentType { get; set; }
    }
}