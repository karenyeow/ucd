
using Comlib.Common.Helpers.Middlewares;
using Microsoft.AspNetCore.Builder;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSetCommonHeaders(this IApplicationBuilder app)
    {
        app.UseMiddleware<CommonHeaders>();
        return app;
    }
}

