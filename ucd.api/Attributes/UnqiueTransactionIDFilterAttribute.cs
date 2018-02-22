using Comlib.Common.Helpers.Constants;
using Comlib.Common.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using UCD.Repository;

namespace UCD.API.Attributes
{
    public class UnqiueTransactionIDFilterAttribute : ActionFilterAttribute
    {
        private IUCDRepository _ucdRepository;
        public UnqiueTransactionIDFilterAttribute(IUCDRepository ucdRepository)
        {
            _ucdRepository = ucdRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var transactionID = context.HttpContext.Request.GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey);
            var pirCode = context.HttpContext.Request.GetHeaderValues("insurerCode");
            if (!_ucdRepository.IsTransactionNumberUnique(pirCode, transactionID))
                throw new ApplicationException() {  Data = }
               context.HttpContext.Response.
                throw new HttpResponseException(actionContext.Request.CreateResponse<ErrorErrorDetails>(HttpStatusCode.BadRequest, new ErrorErrorDetails("TransactionIDValidationError", "TransactionID is not unique")));
        }
        public override void OnActionExecuting(HttpActionContext actionContext)

        {
            var transactionID = actionContext.Request.GetHeaderValues("transactionid");
            var pirCode = actionContext.Request.GetHeaderValues("insurerCode");
            if (!SiraDac.SIRAServiceProviderFactory.Provider.Service.IsTransactionNumberUnique(pirCode, transactionID))
                throw new HttpResponseException(actionContext.Request.CreateResponse<ErrorErrorDetails>(HttpStatusCode.BadRequest, new ErrorErrorDetails("TransactionIDValidationError", "TransactionID is not unique")));
        }
    }
}