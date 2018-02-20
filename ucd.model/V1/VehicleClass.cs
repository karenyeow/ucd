using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class VehicleClass: BaseSiraClass
    {


        [Display(Name = @"vehicle\ctpPolicyNum")]
        [ValidLengthLimit(50)]
        public string ctpPolicyNum { get; set; }

        [Display(Name = @"vehicle\garagePostcode")]
        [ValidLengthLimit(50)]
        public string garagePostcode { get; set; }


        [Display(Name = @"vehicle\insuranceCategory")]
        [ValidLength(3)]
        public string insuranceCategory { get; set; }

        [Display(Name = @"vehicle\registration")]
        [ValidLengthLimit(50)]
        public string registration { get; set; }

        [Display(Name = @"vehicle\rmsCertNum")]
        [ValidLengthLimit(50)]
        public string rmsCertNum { get; set; }

        [Display(Name = @"vehicle\state")]
        [ValidLengthLimit(50)]
        public string state{ get; set; }

        [Display(Name = @"vehicle\ownType")]
        [ValidLengthLimit(50)]
        public string ownType { get; set; }

        [Display(Name = @"vehicle\vehicleCode")]
        [ValidLengthLimit(50)]
        public string vehicleCode { get; set; }

        [Display(Name = @"vehicle\insuredInd")]
        [ValidLengthLimit(50)]
        public string insuredInd { get; set; }

        [Display(Name = @"vehicle\insurerCode")]
        [ValidLengthLimit(50)]
        public string insurerCode { get; set; }

        [Display(Name = @"vehicle\vehicleNum")]
        [ValidLengthLimit(50)]
        public string vehicleNum { get; set; }
        [Display(Name = @"vehicle\vin")]
        [ValidLengthLimit(50)]
        public string vin { get; set; }







    }
}