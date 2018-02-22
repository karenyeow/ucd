using Comlib.Common.Helpers.Constants;
using Comlib.Common.Helpers.Extensions;
using Comlib.Common.Model.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using UCD.API.Helpers;
using UCD.Repository;

namespace UCD.API.Attributes
{
    public class ValidateAPIKeyInsurerCodeAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKey = context.HttpContext.Request.GetHeaderValues(APIHeaderConstants.ApiKeyHeaderKey);
            var pirCode = context.HttpContext.Request.GetHeaderValues("insurerCode");
            if (!UCDGlobal.UCDRepository.IsAPIKeyInsurerCodeValid(apiKey, pirCode)) throw new AppException("InvalidAPIKeyInsurerCode", "API Key Insurer Code combination is not valid");

        }
    
 
    }
}