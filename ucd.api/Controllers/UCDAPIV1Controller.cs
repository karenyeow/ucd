
using Comlib.Common.Helpers.Email;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UCD.API.Attributes;
using UCD.Repository;

namespace UCD.API.Controllers
{
    [UnqiueTransactionIDFilter]
    [ValidateAPIKeyInsurerCode]
    [Route("api/siraapi")]
    public class UCDAPIV1Controller : UCDAPIBaseController
    {
        public UCDAPIV1Controller(IUCDRepository ucdRepository, IOneGovEmailSender onegovEmailSender): base(ucdRepository, onegovEmailSender)
        {

        }
       [HttpPost]
        [Route("CTPClaim/v1")]
        public override  async Task<IActionResult> UploadCTPClaim()
        {
            return await base.UploadCTPClaim();
        }

        [HttpGet]
        [Route("CTPClaim/v1")]
        public override async Task<IActionResult> GetCTPClaim(string id)
        {
            return await base.GetCTPClaim(id);
        }

        [HttpPost]
        [Route("CTPPayment/v1")]
        public override async Task<IActionResult> UploadCTPPayments()
        {
            return await base.UploadCTPPayments();
        }

        [HttpGet]
        [Route("CTPPayment/v1")]
        public override async Task<IActionResult> GetCTPPayment( string id)
        {
            return await base.GetCTPPayment(id);
        }


        [HttpPost]
        [Route("CTPClaimSearch/v1")]
        public override async Task<IActionResult> SearchCTPClaims()
        {
            return await base.SearchCTPClaims();
        }
        [HttpPut]
        [Route("CTPException/v1")]
        public override async Task<IActionResult> ClearException()
        {
            return await base.ClearException();
        }


    }
}
