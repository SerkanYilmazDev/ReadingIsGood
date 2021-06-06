using System;
using System.Net.Http.Headers;
using Shared.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Orders.Commands;
using Services.Orders.Data;
using Shared;
using Services.Orders.HttpServices;
using Services.Customers.Data;
using Services.Orders.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace Services.Orders
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
            services.AddDbContext<OrderDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
            );

            var customerHttpServiceUrl = Configuration.GetValue<string>("HttpServices:CustomerHttpServiceUrl");
            services.AddHttpClient<ICustomerHttpService, CustomerHttpService>(client =>
            {
                client.BaseAddress = new Uri(customerHttpServiceUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

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
               .SubscribeCommand<CreateOrderCommand>()
               .SubscribeEvent<BooksReserved>(_namespace: "books")
               .SubscribeEvent<BooksReserveFailed>(_namespace: "books");

            DbInitializer.Initialize(app.ApplicationServices);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Order service is up."));
            });
        }
    }
}
