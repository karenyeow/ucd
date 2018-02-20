using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class InjuryClass: BaseSiraClass
    {
        [Display(Name = @"injury\injuryCode")]
        [ValidLengthLimit(50)]
        public string injuryCode { get; set; }

        [Display(Name = @"injury\injuryNum")]
        [ValidLengthLimit(50)]
        public string injuryNum { get; set; }
    }
}