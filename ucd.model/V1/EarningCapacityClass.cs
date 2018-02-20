using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class EarningCapacityClass : BaseSiraClass
    {
        [Display(Name = @"earningCapacity\statusCode")]
        [ValidLengthLimit(50)]
        public string statusCode { get; set; }


        [Display(Name = @"earningCapacity\assessmentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string assessmentDate  { get; set; }

        [Display(Name = @"earningCapacity\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }

        [Display(Name = @"earningCapacity\decision")]
        [ValidLengthLimit(50)]
        public string decision { get; set; }



    }
}