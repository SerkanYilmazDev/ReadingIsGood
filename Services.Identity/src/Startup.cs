using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Services.Identity.Authentication;
using Services.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMq;
using Services.Identity.Commands.Handlers;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddMediatR(typeof(RegisterUserCommandHandler).GetTypeInfo().Assembly);

            services.AddControllers();

            return services.BuildContainer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRabbitMq();

            DbInitilializer.Initialize(app.ApplicationServices);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Identity service is up."));
            });
        }
    }
}
