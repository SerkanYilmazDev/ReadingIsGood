using System;
using Shared;
using Shared.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Customers.Data;
using Services.Books.Data;
using Services.Books.Events;
using Services.Books.Commands.Handlers;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace Services.Books
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
            services.AddDbContext<BookDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddMediatR(typeof(CreateBookCommandHandler).GetTypeInfo().Assembly);

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
                   .SubscribeEvent<OrderCreated>(_namespace: "orders");

            DbInitializer.Initialize(app.ApplicationServices);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Book service is up."));
            });
        }
    }
}
