
using Comlib.Common.Helpers.Constants;
using Comlib.Common.Helpers.Extensions;
using Comlib.Common.Helpers.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Globalization;

namespace Comlib.Common.Framework.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSetCommonHeaders(this IApplicationBuilder app)
        {
            app.UseMiddleware<CommonHeaders>();
            return app;
        }
        public static void ExtractCommonHeaders(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                DateTime.TryParseExact(context.Request.GetHeaderValues(APIHeaderConstants.RequestTimeHeaderKey),
                    "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var extractedDateTime);

                context.Items[APIHeaderConstants.RequestTimeHeaderKey] = context.Request.GetHeaderValues(APIHeaderConstants.RequestTimeHeaderKey) == string.Empty ?
                    DateTime.Now.ToString(CultureInfo.CurrentCulture) : extractedDateTime.ToString(CultureInfo.CurrentCulture);

                context.Items[APIHeaderConstants.TransactionIdHeaderKey] = context.Request.GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey);
                context.Items[APIHeaderConstants.ApiKeyHeaderKey] = context.Request.GetHeaderValues(APIHeaderConstants.ApiKeyHeaderKey);

                await next.Invoke();
            });
        }
    }
}

