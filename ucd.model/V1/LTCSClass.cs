using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class LTCSClass:BaseSiraClass 
    {
        [Display(Name = @"ltcs\applicationStatusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string applicationStatusDate { get; set; }

        [Display(Name = @"ltcs\transferDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string transferDate { get; set; }

        [Display(Name = @"ltcs\applicationRef")]
        [ValidLengthLimit(50)]
        public string applicationRef { get; set; }

        [Display(Name = @"ltcs\applicationStatusCode")]
        [ValidLengthLimit(50)]
        public string applicationStatusCode { get; set; }

    }
}