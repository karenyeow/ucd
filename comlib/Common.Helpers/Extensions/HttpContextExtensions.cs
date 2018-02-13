using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Helpers.Extensions
{
   public static class HttpContextExtensions
    {
        public static IServiceProvider ServiceProvider;
        public static HttpContext GetCurrentContext(this HttpContext httpContext)
        {

            object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));

            return ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;

        }
    }
}
