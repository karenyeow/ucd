using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    public class PaymentClass:BaseSiraClass
    {

        [Display(Name = @"payment\providerServiceEndDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string providerServiceEndDate { get; set; }

        [Display(Name = @"payment\gstType")]
        [ValidLengthLimit(50)]
        public string gstType { get; set; }

        [Display(Name = @"payment\invoiceDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string invoiceDate { get; set; }

        [Display(Name = @"payment\invoiceNum")]
        [ValidLengthLimit(50)]
        public string invoiceNum { get; set; }

        [Display(Name = @"payment\netAmount")]
        [ValidPrecision(10, 2)]
        public string netAmount { get; set; }

        [Display(Name = @"payment\originalTransactionID")]
        [ValidLengthLimit(50)]
        public string originalTransactionID { get; set; }

        [Display(Name = @"payment\payeeName")]
        [ValidLengthLimit(150)]
        public string payeeName { get; set; }

        [Display(Name = @"payment\payeeSuburb")]
        [ValidLengthLimit(100)]
        public string payeeSuburb { get; set; }

        [Display(Name = @"payment\payeeID")]
        [ValidLengthLimit(20)]
        public string payeeID { get; set; }

        [Display(Name = @"payment\grossAmount")]
        [ValidPrecision(10, 2)]
        public string grossAmount { get; set; }

        [Display(Name = @"payment\paymentToClaimantInd")]
        [ValidLengthLimit(50)]
        public string paymentToClaimantInd { get; set; }

        [Display(Name = @"payment\paymentDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string paymentDate { get; set; }

        [Display(Name = @"payment\paymentType")]
        [ValidLengthLimit(50)]
        public string paymentType { get; set; }

        [Display(Name = @"payment\providerEmail")]
        [ValidLengthLimit(150)]
        public string providerEmail  { get; set; }


        [Display(Name = @"payment\providerID")] 
        [ValidLengthLimit(50)]
        public string providerID { get; set; }



        [Display(Name = @"payment\providerFirstName")]
        [ValidLengthLimit(150)]
        public string providerFirstName { get; set; }



        [Display(Name = @"payment\providerSurname")]
        [ValidLengthLimit(150)]
        public string providerSurname { get; set; }

        [Display(Name = @"payment\providerPhoneNum")]
        [ValidLengthLimit(50)]
        public string providerPhoneNum { get; set; }

        [Display(Name = @"payment\providerServiceNum")]
        [ValidLengthLimit(50)]
        public string providerServiceNum { get; set; }

        [Display(Name = @"payment\providerStartDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string providerStartDate { get; set; }

        [Display(Name = @"payment\transactionID")]
        [ValidLengthLimit(50)]
        public string transactionID { get; set; }







    }
}