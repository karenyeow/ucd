
using Comlib.Common.Helpers.Constants;
using Comlib.Common.Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Comlib.Common.Framework.Middlewares
{
    public class CommonHeaders
    {
        private readonly RequestDelegate _next;
        private readonly IExecutionContext _executionContext;

        public CommonHeaders(RequestDelegate next, IExecutionContext executionContext)
        {
            _executionContext = executionContext;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add(APIHeaderConstants.RequestTimeHeaderKey, context.Request.GetHeaderValues(APIHeaderConstants.RequestTimeHeaderKey));
                httpContext.Response.Headers.Add(APIHeaderConstants.TransactionIdHeaderKey, context.Request.GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey));
                httpContext.Response.Headers.Add(APIHeaderConstants.DeviceIdHeaderKey, context.Request.GetHeaderValues(APIHeaderConstants.DeviceIdHeaderKey));
                httpContext.Response.Headers.Add(APIHeaderConstants.ResponseTimeHeaderKey, DateTime.Now.ToUniversalTimeString());

                return Task.FromResult(0);
            }, context);

            await _next.Invoke(context);
        }
    }


}
