using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class RiskScreeningClass:BaseSiraClass 
    {
        [Display(Name = @"riskScreening\outcomeDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string outcomeDate { get; set; }

        [Display(Name = @"riskScreening\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }

    }
}