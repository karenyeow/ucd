using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Helpers.Connections;
using Common.Helpers.Email;
using MainDev.Common.Helpers.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

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

            //services.AddSingleton<IHbcfRepository, HbcfRepository>();
            //services.AddSingleton<IHbcfConnectionProvider, HbcfConnectionProvider>();

            services.AddSingleton<IApplicationConnectionProvider, ApplicationConnectionProvider>();

            services.AddTransient<IOneGovEmailSender, OneGovEmailSender>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
