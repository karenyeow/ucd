using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class CommonLawClass : BaseSiraClass
    {
        [Display(Name = @"commonLaw\litigationLevel")]
        [ValidLengthLimit(50)]
        public string litigationLevel { get; set; }

        [Display(Name = @"commonLaw\liabilityStatusCode")]
        [ValidLengthLimit(50)]
        public string liabilityStatusCode { get; set; }

        [Display(Name = @"commonLaw\lifeCycleStageCode")]
        [ValidLengthLimit(50)]
        public string lifeCycleStageCode { get; set; }

        [Display(Name = @"commonLaw\settlementMethodCode")]
        [ValidLengthLimit(50)]
        public string settlementMethodCode { get; set; }

        [Display(Name = @"commonLaw\statusCode")]
        [ValidLengthLimit(50)]
        public string statusCode { get; set; }

        [Display(Name = @"commonLaw\contribNeg")]
        [ValidPrecision(3, 4)]
        public string contribNeg { get; set; }

        [Display(Name = @"commonLaw\litigationLevelChgDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string litigationLevelChgDate { get; set; }

        [Display(Name = @"commonLaw\statusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string statusDate { get; set; }

        [Display(Name = @"commonLaw\liabilityStatusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string liabilityStatusDate { get; set; }

        [Display(Name = @"commonLaw\notificationDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string notificationDate { get; set; }

        [Display(Name = @"commonLaw\contribNegDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string contribNegDate { get; set; }

        [Display(Name = @"commonLaw\earlyIndDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string earlyIndDate { get; set; }

        [Display(Name = @"commonLaw\earlyInd")]
        [ValidLengthLimit(50)]
        public string earlyInd        { get; set; }
        [Display(Name = @"commonLaw\declReason")]
        [ValidLengthLimit(50)]
        public string declReason { get; set; }

        [Display(Name = @"commonLaw\settlementAmount")]
        [ValidPrecision(10,2)]
        public string settlementAmount { get; set; }








    }
}