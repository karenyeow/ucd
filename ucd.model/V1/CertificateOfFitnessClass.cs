using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class CertificateOfFitnessClass:BaseSiraClass 
    {
        [Display(Name = @"certificateOfFitness\statusCode")]
        [ValidLengthLimit(50)]
        public string statusCode { get; set; }

        [Display(Name = @"certificateOfFitness\hoursFitForWork")]
        [ValidPrecision(3, 2)]
        public string hoursFitForWork { get; set; }

        [Display(Name = @"certificateOfFitness\endDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string endDate { get; set; }

        [Display(Name = @"certificateOfFitness\startDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string startDate { get; set; }

        [Display(Name = @"certificateOfFitness\issuedDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string issuedDate { get; set; }

        [Display(Name = @"certificateOfFitness\providerName")]
        [ValidLengthLimit(150)]
        public string providerName { get; set; }

        [Display(Name = @"certificateOfFitness\providerID")]
        [ValidLengthLimit(50)]
        public string providerID { get; set; }

    }
}