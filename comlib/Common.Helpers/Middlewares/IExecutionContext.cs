using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainDev.Common.Helpers.Middlewares
{
    public interface IExecutionContext
    {
        string GetResponseContentType(HttpContext context);
    }
}
