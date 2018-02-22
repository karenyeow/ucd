using Comlib.Common.Helpers.Constants;
using Comlib.Common.Helpers.Extensions;
using Comlib.Common.Model.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using UCD.API.Helpers;
using UCD.Repository;

namespace UCD.API.Attributes
{
    public class UnqiueTransactionIDFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var transactionID = context.HttpContext.Request.GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey);
            var pirCode = context.HttpContext.Request.GetHeaderValues("insurerCode");
            if (!UCDGlobal.UCDRepository.IsTransactionNumberUnique(pirCode, transactionID))
                throw new AppException("TransactionIDValidationError", "TransactionID is not unique");
               
        }
     
    }
}