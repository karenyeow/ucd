using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class InternalReviewClass:BaseSiraClass 
    {
        [Display(Name = @"internalReview\statusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string statusDate { get; set; }

        [Display(Name = @"internalReview\refNum")]
        [ValidLengthLimit(50)]
        public string refNum { get; set; }

        [Display(Name = @"internalReview\statusCode")]
        [ValidLengthLimit(50)]
        public string statusCode { get; set; }

        [Display(Name = @"internalReview\assessmentType")]
        [ValidLengthLimit(50)]
        public string assessmentType { get; set; }

        [Display(Name = @"internalReview\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }
        [Display(Name = @"internalReview\reason")]
        [ValidLengthLimit(50)]
        public string reason { get; set; }
    }
}