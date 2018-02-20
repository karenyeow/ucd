using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class AccidentClass:BaseSiraClass  
    {
        [Display(Name= @"accident\accidentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string accidentDate { get; set; }

        [Display(Name = @"accident\accidentID")]
        [ValidLengthLimit(50)]
        public string accidentID { get; set; }

        [Display(Name = @"accident\postcode")]
        [ValidLengthLimit(50)]
        public string postcode { get; set; }

        [Display(Name = @"accident\streetName")]
        [ValidLengthLimit(100)]
        public string streetName { get; set; }

        [Display(Name = @"accident\suburb")]
        [ValidLengthLimit(100)]
        public string suburb { get; set; }

        [Display(Name = @"accident\accidentTime")]
        [ValidLength(4)]
        [EnsureInteger]
        public string  accidentTime { get; set; }

        [Display(Name = @"accident\blamelessInd")]
        [ValidLengthLimit(50)]
        public string blamelessInd{ get; set; }

        [Display(Name = @"accident\eventNum")]
        [ValidLengthLimit(50)]
        public string eventNum { get; set; }

        [Display(Name = @"accident\accidentDate")]
        [ValidLengthLimit(100)]
        public string crossRoad { get; set; }

        [Display(Name = @"accident\policeInd")]
        [ValidLength(2)]
        public string policeInd { get; set; }

        [Display(Name = "accident\rum")]
        [ValidLengthLimit(50)]
       public string rum { get; set; }

        [Display(Name = @"accident\state")]
        [ValidLengthLimit(50)]
        public string state        { get; set; }
    }
}