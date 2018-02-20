using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class ReturnToWorkClass:BaseSiraClass 
    {
        [Display(Name = @"returnToWork\statusReviewDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string statusReviewDate { get; set; }

        [Display(Name = @"returnToWork\firstResumedWorkDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string firstResumedWorkDate { get; set; }

        [Display(Name = @"returnToWork\firstResumedWorkDate")]
        [ValidLengthLimit(50)]
        public string statusCode { get; set; }
    }
}