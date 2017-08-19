using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OddMonitor.Common.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace OddMonitor
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public override void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "OddMonitor API"); });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            if (serverAddressesFeature != null)
            {
                _logger.LogInformation("Application listening on: {Url}", string.Join(", ", serverAddressesFeature.Addresses));
            }
        }

        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "OddMonitor API", Version = "v1" }); });
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.InjectDependencies(GetType());

            return new AutofacServiceProvider(builder.Build());
        }
    }
}