using System.Threading.Tasks;

namespace UCD.API.Controllers
{
    [UnqiueTransactionIDFilterAttribute]
    [ValidateAPIKeyInsurerCode]
    [RoutePrefix("api/siraapi")]
    public class UCDAPIV1Controller : UCDAPIBaseController
    {

        [HttpPost]
        [Route("CTPClaim/v1")]
        public override  async Task<IHttpActionResult> UploadCTPClaim()
        {
            return await base.UploadCTPClaim();
        }

        [HttpGet]
        [Route("CTPClaim/v1")]
        public override async Task<IHttpActionResult> GetCTPClaim([FromUri] string id)
        {
            return await base.GetCTPClaim(id);
        }

        [HttpPost]
        [Route("CTPPayment/v1")]
        public override async Task<IHttpActionResult> UploadCTPPayments()
        {
            return await base.UploadCTPPayments();
        }

        [HttpGet]
        [Route("CTPPayment/v1")]
        public override async Task<IHttpActionResult> GetCTPPayment([FromUri] string id)
        {
            return await base.GetCTPPayment(id);
        }


        [HttpPost]
        [Route("CTPClaimSearch/v1")]
        public override async Task<IHttpActionResult> SearchCTPClaims()
        {
            return await base.SearchCTPClaims();
        }
        [HttpPut]
        [Route("CTPException/v1")]
        public override async Task<IHttpActionResult> ClearException()
        {
            return await base.ClearException();
        }


    }
}
