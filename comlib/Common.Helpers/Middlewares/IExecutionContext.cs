﻿using Microsoft.AspNetCore.Http;

namespace Comlib.Common.Helpers.Middlewares
{
    public interface IExecutionContext
    {
        string GetResponseContentType(HttpContext context);
    }
}
