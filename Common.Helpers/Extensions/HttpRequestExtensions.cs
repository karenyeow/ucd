using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Comlib.Common.Helpers.Extensions
{
   public static class HttpRequestExtensions
    {
        public static string GetHeaderValues(this HttpRequest request, string key)
        {
            var value = string.Empty;

            if (request.Headers.TryGetValue(key, out var headerValues))
            {
                value = headerValues.FirstOrDefault();
            }
            return value;
        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (request.Headers["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }


        //public static string Json(this HttpRequest request)
        //{
        //    if (request == null)
        //    {
        //        throw new ArgumentNullException("request");
        //    }

        //    if (request.ContentType != "application/json")
        //    {
        //        return null;
        //    }

        //    string jsonString;
        //    request.InputStream.Position = 0;
        //    using (var inputStream = new StreamReader(request.InputStream))
        //    {
        //        jsonString = inputStream.ReadToEnd();
        //    }
        //    return jsonString;
        //}
    }
}
