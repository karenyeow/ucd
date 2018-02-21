using System;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;
using SiraDac = ICPDataAccess.DLS.SIRA;
using ICP.Utilities.Extensions;
using ICPWeb.CommonV2.Api;

namespace ICPWeb.PublicServices.Filters.Sira
{
    public class ValidateAPIKeyInsurerCodeAttribute : ActionFilterAttribute
    { 
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var apiKey = actionContext.Request.GetHeaderValues("apikey");
            var pirCode = actionContext.Request.GetHeaderValues("insurerCode");
            if (!SiraDac.SIRAServiceProviderFactory.Provider.Service.IsAPIKeyInsurerCodeValid (apiKey, pirCode))
                throw new HttpResponseException(actionContext.Request.CreateResponse<ErrorErrorDetails>(HttpStatusCode.BadRequest, new ErrorErrorDetails("InvalidAPIKeyInsurerCode", "API Key Insurer Code combination is not valid")));
        }
    }
}