using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comlib.Common.Helpers.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Comlib.Common.Helpers.Constants;

namespace iCare.Api.Controllers
{
    public class AbstractApiController : Controller
    {
        private readonly IOneGovEmailSender _oneGovEmailSender;

        public AbstractApiController(IOneGovEmailSender oneGovEmailSender)
        {
            _oneGovEmailSender = oneGovEmailSender;
        }

        protected async Task<IActionResult> HandleException(Exception ex)
        {
            await _oneGovEmailSender.SendErrorEmailAsync(ex);
            return new BadRequestObjectResult(new
            {
                Error = ex.HResult.ToString(),
                ErrorDescription = ex.Message
            });
        }

        protected string GetHeaderValues(string key)
        {
    
            var value = string.Empty;

            if (Request.Headers.TryGetValue(key, out StringValues  headerValues))
            {
                value = headerValues.FirstOrDefault();
            }
            return value;
        }
        protected void SetHeaderValues()
        {
            _SetCommonHeaderValues();
            Request.Headers.Add(APIHeaderConstants.ResponseTimeHeaderKey, DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + "Z");

        }
        protected void SetHeaderValuesUTC()
        {
            _SetCommonHeaderValues();
            Request.Headers.Add(APIHeaderConstants.ResponseTimeHeaderKey, DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + "Z");
           
        }

        private void _SetCommonHeaderValues()
        {
            var requestedTimeStampRaw = GetHeaderValues(APIHeaderConstants.RequestTimeHeaderKey);
            var transactionIdKey = GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey);
            var lastModifiedTimeStampRaw = GetHeaderValues(APIHeaderConstants.RequestIfModifiedSinceHeaderKey);

            Request.Headers.Clear();
            Request.Headers.Add(APIHeaderConstants.RequestTimeHeaderKey, requestedTimeStampRaw);
            Request.Headers.Add(APIHeaderConstants.RequestIfModifiedSinceHeaderKey, lastModifiedTimeStampRaw);
            Request.Headers.Add(APIHeaderConstants.TransactionIdHeaderKey, transactionIdKey);


        }


    }
}
