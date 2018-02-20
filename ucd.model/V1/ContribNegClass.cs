using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class ContribNegClass: BaseSiraClass
    {
        [Display(Name = @"contribNeg\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }

        [Display(Name = @"contribNeg\assessmentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string assessmentDate { get; set; }


    }
}