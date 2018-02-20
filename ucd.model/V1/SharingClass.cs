using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class SharingClass:BaseSiraClass 
    {
        [Display(Name = @"sharing\percentageShare")]
        [ValidPrecision(3, 4)]
        public string percentageShare { get; set; }

        [Display(Name = @"sharing\insurerCode")]
        [ValidLengthLimit(50)]
        public string insurerCode { get; set; }

        [Display(Name = @"sharing\vehicleNum")]
        [ValidLengthLimit(50)]
        [EnsureDouble]
        public string vehicleNum { get; set; }

    }
}