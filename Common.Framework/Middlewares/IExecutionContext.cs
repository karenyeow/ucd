using Microsoft.AspNetCore.Http;

namespace Comlib.Common.Framework.Middlewares
{
    public interface IExecutionContext
    {
        string GetResponseContentType(HttpContext context);
    }
}
