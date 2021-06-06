using System;
using Shared.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Notifications.Events;
using Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace Services.Notifications
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
            services.AddControllers();

            return services.BuildContainer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRabbitMq()
               .SubscribeEvent<OrderCompleted>(_namespace: "orders")
               .SubscribeEvent<OrderFailed>(_namespace: "orders");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Notification service is up."));
            });
        }
    }
}
