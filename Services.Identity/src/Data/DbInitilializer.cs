using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Identity.Data
{
    public static class DbInitilializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IdentityDBContext>();
            context.Database.Migrate();
        }
    }
}