using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class CaseEstimateClass: BaseSiraClass
    {
        [Display(Name = @"caseEstimate\estimateDate")]
        [ValidPrecision(10, 2)]
        public string amount { get; set; }

        [Display(Name = @"caseEstimate\estimateDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string estimateDate { get; set; }

        [Display(Name = @"caseEstimate\estimateType")]
        [ValidLengthLimit(50)]
        public string estimateType { get; set; }
    }
}