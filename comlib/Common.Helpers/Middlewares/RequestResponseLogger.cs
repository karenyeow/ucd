
using Dapper;
using Comlib.Common.Helpers.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.Common.Helpers.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAPILoggingConnectionProvider  _apiLoggingConnectionProvider;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RequestLoggingMiddleware(RequestDelegate next, IAPILoggingConnectionProvider apiLoggingConnectionProvider, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _apiLoggingConnectionProvider = apiLoggingConnectionProvider;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var responseLoggerStream = new ResponseLoggerStream(context.Response.Body, ownsParent: false);
            context.Response.Body = responseLoggerStream;

            var requestLoggerStream = new RequestLoggerStream(context.Request.Body, ownsParent: false);
            context.Request.Body = requestLoggerStream;

            try
            {
                var watch = Stopwatch.StartNew();
                await _next.Invoke(context);
                watch.Stop();
                var requestUri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";

                if (context.Request.QueryString.HasValue)
                {
                    requestUri += $"?{context.Request.QueryString}";
                }

                var headerBuilder = new StringBuilder();
                headerBuilder.AppendLine("Headers:");
                headerBuilder.AppendLine("{");

                foreach (var header in context.Request.Headers)
                {
                    headerBuilder.AppendLine($"{header.Key}:{header.Value}");
                }

                headerBuilder.AppendLine("}");

                var requestTracerStream = requestLoggerStream.TracerStream;
                var requestData = requestTracerStream.ToArray();
                var requestContent = Encoding.UTF8.GetString(requestData);

                var responseTracerStream = responseLoggerStream.TracerStream;

                var responseData = responseTracerStream.ToArray();
                var responseContent = Encoding.UTF8.GetString(responseData);

                string apiKey = context.Request.Headers["apikey"];
                string userAgent = context.Request.Headers["User-Agent"];
                string transactionId = context.Request.Headers["transactionID"];

                var entry = new ApiLogEntry
                {
                    HostName = Environment.MachineName,
                    ApplicationName = _hostingEnvironment.ApplicationName,
                    RequestURI = requestUri,
                    RequestContentType = context.Request.ContentType ?? string.Empty,
                    RequestMethod = context.Request.Method,
                    RequestIPAddress = context.Connection.RemoteIpAddress.ToString(),
                    RequestContent = headerBuilder + "\r\n" + requestContent,
                    RequestCreated = DateTime.Now,
                    RequestAPIKey = string.IsNullOrEmpty(apiKey) ? "N/A" : apiKey,
                    RequestUserAgent = string.IsNullOrEmpty(userAgent) ? "N/A" : userAgent,
                    RequestTransactionID = string.IsNullOrEmpty(transactionId) ? "N/A" : transactionId,
                    RequestUserID = context.Request.Headers["sfUserID"].ToString() ?? context.Request.Headers["userID"].ToString(),
                    RequestUserName = context.Request.Headers["sfUserName"],
                    RequestUserLocation = null,
                    RequestTimeStamp = context.Request.Headers["requestedTimeStamp"],
                    ResponseCode = context.Response.StatusCode.ToString(),
                    ResponseCodeDescription = context.Response.StatusCode.ToString(),
                    ResponseContent = responseContent,
                    ResponseContentType = string.IsNullOrEmpty(context.Response.ContentType) ? "NA" : context.Response.ContentType,
                    ResponseCreated = DateTime.Now,
                };

                using (var connection = _apiLoggingConnectionProvider.CreateSqlConnection())
                {
                    try
                    {
                        connection.Execute("dbo.OneGovAPIRequestResponseInsert", entry, commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        var e = ex;
                    }
                }
            }
            finally
            {
                context.Response.Body = responseLoggerStream.Inner;
                responseLoggerStream.Dispose();

                context.Request.Body = requestLoggerStream.Inner;
                requestLoggerStream.Dispose();
            }
        }
        public class ApiLogEntry
        {
            public string HostName { get; set; }
            public string ApplicationName { get; set; }         // The application that made the request.
            public string RequestURI { get; set; }              // The request URI.
            public string RequestContentType { get; set; }      // The request content type.
            public string RequestMethod { get; set; }           // The request method (GET, POST, etc).
            public string RequestIPAddress { get; set; }        // The IP address that made the request.
            public string RequestContent { get; set; }          // The request content body.
            public DateTime RequestCreated { get; set; }        // The request timestamp.
            public string RequestAPIKey { get; set; }
            public string RequestUserAgent { get; set; }
            public string RequestTransactionID { get; set; }
            public string RequestUserID { get; set; }
            public string RequestUserName { get; set; }
            public string RequestUserLocation { get; set; }
            public string RequestTimeStamp { get; set; }
            public string ResponseCode { get; set; }            // The response content type.
            public string ResponseCodeDescription { get; set; } // The response content body.
            public string ResponseContent { get; set; }
            public string ResponseContentType { get; set; }
            public DateTime ResponseCreated { get; set; }       // The response timestamp.
        }
    }
}
