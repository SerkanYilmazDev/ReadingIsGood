using System;
using System.Net.Http.Headers;
using Shared.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Customers.Commands;
using Services.Customers.Data;
using Shared;
using Services.Customers.HttpServices;
using Services.Customers.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using MediatR;

namespace Services.Customers
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
            services.AddDbContext<CustomerDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
            );

            var bookHttpServiceUrl = Configuration.GetValue<string>("HttpServices:BookHttpServiceUrl");
            services.AddHttpClient<IBookHttpService, BookHttpService>(client =>
            {
                client.BaseAddress = new Uri(bookHttpServiceUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddMediatR(typeof(AddBookToBasketCommand).GetTypeInfo().Assembly);

            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services.BuildContainer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRabbitMq()
               .SubscribeEvent<UserCreated>(_namespace: "identity")
               .SubscribeEvent<OrderCompleted>(_namespace: "orders")
               .SubscribeEvent<BookCreated>(_namespace: "books");

            DbInitilializer.Initialize(app.ApplicationServices);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Customer service is up."));
            });
        }
    }
}
