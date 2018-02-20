using Comlib.Common.Framework.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class PaymentRequestClass:BaseSiraClass 
    {
        public string type { get; set; } = "payment";

        [Required]
        [ValidISO8601("yyyyMMdd'T'HHmmss'Z'")]
        public string providerProcessedDateTime { get; set; }
        [Required]
        [ValidLengthLimit(50)]
        public string claimID { get; set; }

        [Required]
        [ValidList]
        public List<PaymentClass>  payment { get; set; } 
    }
}
