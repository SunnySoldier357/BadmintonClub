using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Backend
{
    public class Program
    {
        // Public Methods
        public static void Main(string[] args) =>
            CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

            // No Azure App Services logging except when running on Azure App Service
            string regionName = Environment.GetEnvironmentVariable("REGION_NAME");
            if (null != regionName)
                hostBuilder.UseAzureAppServices();

            return hostBuilder;
        }
    }
}