using Comlib.Common.Framework.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class PersonClass : BaseSiraClass
    {
        [Display(Name = @"person\phoneAreaCode")]
        [ValidLengthLimit(3)]
        public string phoneAreaCode { get; set; }

        [Display(Name = @"person\claimantInd")]
        [ValidLengthLimit(50)]
        public string claimantInd { get; set; }

        [Display(Name = @"person\dateOfBirth")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string dateOfBirth { get; set; }

        [Display(Name = @"person\firstName")]
        [ValidLengthLimit(150)]
        public string firstName { get; set; }

        [Display(Name = @"person\gender")]
        [ValidLengthLimit(50)]
        public string gender { get; set; }

        [Display(Name = @"person\idNum")]
        [ValidLengthLimit(50)]
        public string idNum { get; set; }

        [Display(Name = @"person\injuredPartyInd")]
        [ValidLengthLimit(50)]
        public string injuredPartyInd { get; set; }

        [Display(Name = @"person\surname")]
        [ValidLengthLimit(150)]
        public string surname { get; set; }

        [Display(Name = @"person\licenseState")]
        [ValidLengthLimit(50)]
        public string licenseState { get; set; }

        [Display(Name = @"person\midInitial")]
        [ValidLengthLimit(150)]
        public string midInitial { get; set; }

        [Display(Name = @"person\vehicleNum")]
        [ValidLengthLimit(50)]
        public string vehicleNum { get; set; }


        [Display(Name = @"person\phoneNumber")]
        [ValidLengthLimit(50)]
        public string phoneNumber { get; set; }

        [Display(Name = @"person\postcode")]
        [ValidLengthLimit(50)]
        public string postcode { get; set; }



        [Display(Name = @"person\streetName")]
        [ValidLengthLimit(100)]
        public string streetName { get; set; }

        [Display(Name = @"person\streetNum")]
        [ValidLengthLimit(100)]
        public string streetNum { get; set; }

        [Display(Name = @"person\suburb")]
        [ValidLengthLimit(100)]
        public string suburb { get; set; }

        [Display(Name = @"person\idType")]
        [ValidLengthLimit(50)]
        public string idType { get; set; }

        [Display(Name = @"person\unitNum")]
        [ValidLengthLimit(100)]
        public string unitNum { get; set; }

        [Display(Name = @"person\personRoleCode")]
        public List<string> personRoleCode { get; set; }
















    }
}