using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class StatutoryBenefitsClass: BaseSiraClass
    {
        [Display(Name = @"statutoryBenefits\liabilityStatusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string liabilityStatusDate        { get; set; }

        [Display(Name = @"statutoryBenefits\declReason")]
        [ValidLengthLimit(50)]
        public string declReason { get; set; }
        [Display(Name = @"statutoryBenefits\liabilityStatusCode")]
        [ValidLengthLimit(50)]
        public string liabilityStatusCode { get; set; }

        [Display(Name = @"statutoryBenefits\lodgedDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string lodgedDate { get; set; }
        [Display(Name = @"statutoryBenefits\statusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string statusDate { get; set; }
        [Display(Name = @"statutoryBenefits\statusReason")]
        [ValidLengthLimit(50)]
        public string statusReason { get; set; }
        [Display(Name = @"statutoryBenefits\statusCode")]
        [ValidLengthLimit(50)]
        public string statusCode { get; set; }
    }
}