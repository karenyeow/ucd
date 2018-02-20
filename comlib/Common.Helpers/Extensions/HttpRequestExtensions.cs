using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetRawBodyBytesAsync(this HttpRequest request)
        {
            using (var ms = new MemoryStream(2048))
            {
                await request.Body.CopyToAsync(ms);
                return ms.ToArray();
            }
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
