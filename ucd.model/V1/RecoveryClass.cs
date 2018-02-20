using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class RecoveryClass : BaseSiraClass
    {
        [Display(Name = @"recovery\requestDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string requestDate { get; set; }

        [Display(Name = @"recovery\insurerName")]
        [ValidLengthLimit(150)]
        public string insurerName { get; set; }


        [Display(Name = @"recovery\insurerState")]
        [ValidLengthLimit(50)]
        public string insurerState { get; set; }



    }
}