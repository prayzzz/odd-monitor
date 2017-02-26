using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayTheOdds.Common.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace PlayTheOdds
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName.ToLower()}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom
                .Configuration(Configuration)
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUi(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlayTheOdds API"); });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "PlayTheOdds API", Version = "v1"}); });
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.InjectDependencies(GetType());

            return new AutofacServiceProvider(builder.Build());
        }
    }
}