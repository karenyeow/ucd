﻿using Comlib.Common.Helpers.Constants;
using Microsoft.AspNetCore.Http;

namespace Comlib.Common.Helpers.Middlewares
{
    public class ExecutionContext : IExecutionContext
    {


        public string GetResponseContentType(HttpContext context)
        {
            return APIHeaderConstants.MediaTypeHeaderValueApplicationJson;
        }
    }
}
