using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Logging;
using System.Threading.Tasks;
using System.Linq;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;
using Api.Controllers.Dtos;
using Microsoft.Extensions.Hosting;
using Shared;
using Microsoft.AspNetCore.Http;

namespace Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot()
                    .AddPolly()
                    .AddCacheManager(x =>
                    {
                        x.WithDictionaryHandle();
                    });

            services.AddControllers();

            // Api Gateway validates the JWT token which is created by the Identity Service.
            AddJwtAuthentication(services);

            return services.BuildContainer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Api Gateway is up."));
            });

            app.UseOcelot().Wait();
        }

        private void AddJwtAuthentication(IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SomeStaticAccessKey12345!")),
                ValidateIssuer = true,
                ValidIssuer = "localhost",
                ValidateAudience = true,
                ValidAudience = "Serkan YILMAZ",
                ValidateLifetime = true,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
                x.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = (context) =>
                    {
                        var name = context.Principal.Identity.Name;
                        if (string.IsNullOrEmpty(name))
                        {
                            context.Fail("Unauthorized. Please re-login");
                        }
                        context.HttpContext.Items.Add("CurrentUser",
                            new UserClaimDto
                            {
                                Email = context.Principal.Identity.Name,
                                UserId = Guid.Parse(context.Principal.Claims.First(s => s.Type == "user_id").Value),
                            });
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
