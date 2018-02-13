using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Comlib.Common.Helpers.Constants;

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
