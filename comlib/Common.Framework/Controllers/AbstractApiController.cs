using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comlib.Common.Helpers.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Comlib.Common.Helpers.Constants;
using System.ComponentModel;
using System.Net;
using System.Globalization;
using Comlib.Common.Model.Error;

namespace Comlib.Common.Framework.Controllers
{
    public class AbstractApiController : Controller
    {
        private readonly IOneGovEmailSender _oneGovEmailSender;

        public AbstractApiController(IOneGovEmailSender oneGovEmailSender)
        {
            _oneGovEmailSender = oneGovEmailSender;
        }

        protected async Task<IActionResult> HandleExceptionAsync(Exception ex)
        {
            await _oneGovEmailSender.SendErrorEmailAsync(ex);
            return new BadRequestObjectResult(new
            {
                Error = ex.HResult.ToString(),
                ErrorDescription = ex.Message
            });
        }

        protected async Task<IActionResult> HandleInternalErrorExceptionAsync(Exception ex)
        {
            await _oneGovEmailSender.SendErrorEmailAsync(ex);
            var e = ex as Win32Exception ?? ex.InnerException as Win32Exception;

            if (e == null)
            {
                return StatusCode(500,  new  ErrorErrorDetails(e.ErrorCode.ToString(CultureInfo.InvariantCulture),ex.Message ));
   
            }
            else
            {
                return StatusCode(500, new ErrorErrorDetails(ex.HResult.ToString(CultureInfo.InvariantCulture), ex.Message));

            }

      
        }

        protected  IActionResult HandleInternalErrorException(Exception ex)
        {
             _oneGovEmailSender.SendErrorEmailSync(ex);
            var e = ex as Win32Exception ?? ex.InnerException as Win32Exception;

            if (e == null)
            {
                return StatusCode(500, new ErrorErrorDetails(e.ErrorCode.ToString(CultureInfo.InvariantCulture), ex.Message));

            }
            else
            {
                return StatusCode(500, new ErrorErrorDetails(ex.HResult.ToString(CultureInfo.InvariantCulture), ex.Message));

            }


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
            Response.Headers.Add(APIHeaderConstants.ResponseTimeHeaderKey, DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + "Z");

        }
        protected void SetHeaderValuesUTC()
        {
            _SetCommonHeaderValues();
            Response.Headers.Add(APIHeaderConstants.ResponseTimeHeaderKey, DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + "Z");
           
        }

        private void _SetCommonHeaderValues()
        {
            var requestedTimeStampRaw = GetHeaderValues(APIHeaderConstants.RequestTimeHeaderKey);
            var transactionIdKey = GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey);
            var lastModifiedTimeStampRaw = GetHeaderValues(APIHeaderConstants.RequestIfModifiedSinceHeaderKey);

            Response.Headers.Clear();
            Response.Headers.Add(APIHeaderConstants.RequestTimeHeaderKey, requestedTimeStampRaw);
            Response.Headers.Add(APIHeaderConstants.RequestIfModifiedSinceHeaderKey, lastModifiedTimeStampRaw);
            Response.Headers.Add(APIHeaderConstants.TransactionIdHeaderKey, transactionIdKey);


        }

     
    }
}
