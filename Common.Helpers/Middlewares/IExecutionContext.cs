using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Helpers.Middlewares
{
    public interface IExecutionContext
    {
        string GetResponseContentType(HttpContext context);
    }
}
