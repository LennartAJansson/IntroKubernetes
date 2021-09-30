using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using SmhiExtractor.Configurations;
using SmhiExtractor.Services;

namespace SmhiExtractor
{
    /// <summary>
    /// This application will run in the background and collect data from SMHI
    /// The data will be pushed to the api in this solution
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplicationConfiguration(hostContext);
                    services.AddApplicationServices();
                    services.AddHostedService<Worker>();
                })
                .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>());
    }
}
