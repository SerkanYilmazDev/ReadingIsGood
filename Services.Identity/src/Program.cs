﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Shared.Logging;
using Microsoft.AspNetCore.Builder;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseLogging("Identity.Service");
    }
}
