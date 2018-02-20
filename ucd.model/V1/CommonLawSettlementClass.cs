using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class CommonLawSettlementClass:BaseSiraClass 
    {
        [Display(Name = @"commonLawSettlement\amount")]
        [ValidPrecision(10, 2)]
        public string amount { get; set; }

        [Display(Name = @"commonLawSettlement\offerDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string offerDate { get; set; }

        [Display(Name = @"commonLawSettlement\outcomeDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string outcomeDate { get; set; }

        [Display(Name = @"commonLawSettlement\outcome")]
        [ValidLengthLimit(50)]
        public string outcome { get; set; }
        [Display(Name = @"commonLawSettlement\offerSourceCode")]
        [ValidLengthLimit(50)]
        public string offerSourceCode { get; set; }

    }
}