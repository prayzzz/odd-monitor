using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayTheOdds.Common.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace PlayTheOdds
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "PlayTheOdds API", Version = "v1" }); });
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.InjectDependencies(GetType());

            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

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
    }
}
