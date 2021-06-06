using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Customers.Data
{
    public static class DbInitilializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<CustomerDBContext>();
            context.Database.Migrate();
        }
    }
}