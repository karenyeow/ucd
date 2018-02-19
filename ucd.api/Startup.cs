using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comlib.Common.Helpers.Connections;
using Comlib.Common.Helpers.Email;
using Comlib.Common.Helpers.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using UCD.Repository;

namespace ucd.api
{
    public class Startup
    {
        private const string ExceptionsOnStartup = "Startup";
        private const string ExceptionsOnConfigureServices = "ConfigureServices";
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly Dictionary<string, List<Exception>> _exceptions;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _exceptions = new Dictionary<string, List<Exception>>
            {
                {ExceptionsOnStartup, new List<Exception>()},
                {ExceptionsOnConfigureServices, new List<Exception>()},
            };
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(hostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
                _configuration = builder.Build();
            }
            catch (Exception exception)
            {
                _exceptions[ExceptionsOnStartup].Add(exception);
            }
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()); ;

            var config = _configuration.GetSection("AppInfo");
            _hostingEnvironment.ApplicationName = config["ApplicationName"];

            services.AddDataProtection()
                .SetApplicationName(_hostingEnvironment.ApplicationName + _hostingEnvironment.EnvironmentName)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(365));

            services.AddSingleton(_configuration).AddSingleton<IExecutionContext, ExecutionContext>();

            services.AddSingleton<IUCDRepository, UCDRepositoryProvider>();
            services.AddSingleton<IUCDConnectionProvider, UCDConnectionProvider>();
            services.AddSingleton<IAPILoggingConnectionProvider, APILoggingConnectionProvider>();

            services.AddTransient<IOneGovEmailSender, OneGovEmailSender>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IExecutionContext executionContext)
        {
            app.ExtractCommonHeaders();

            if (_hostingEnvironment.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSetCommonHeaders();

            app.UseMiddleware<RequestLoggingMiddleware>();

            if (_exceptions.Any(p => p.Value.Any()))
            {
                app.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/plain";

                        foreach (var ex in _exceptions)
                        {
                            foreach (var val in ex.Value)
                            {
                                await context.Response.WriteAsync($"Error on {ex.Key}: {val.Message}").ConfigureAwait(false);
                            }
                        }
                    });
                return;
            }

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(async context =>
                    {
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            var errorObject = new
                            {
                                Error = error.Error.HResult.ToString(),
                                ErrorDescription = error.Error.Message
                            };

                            context.Response.ContentType = executionContext.GetResponseContentType(context);
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorObject)).ConfigureAwait(false);

                            await new OneGovEmailSender(_configuration).SendErrorEmailAsync(error).ConfigureAwait(false);
                        }
                    });
                });

            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile(_configuration.GetSection("Logging"));
            app.UseMvc();
        }

    }
}
