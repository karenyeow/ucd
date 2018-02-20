using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    /// <summary>
    /// 
    /// </summary>
    public class ClaimRequestClass: BaseSiraClass
    {
 

        [Required]
        [ValidISO8601("yyyyMMdd'T'HHmmss'Z'")]
        public string providerProcessedDateTime { get; set; }

        [Required]
        public ClaimClass claim { get; set; } = new ClaimClass();
    }
}