using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using SmhiApi.Configurations;
using SmhiApi.Services;

namespace SmhiApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().AddMigrations().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration))
                .ConfigureServices((hostBuilderContext, services) => services
                    .AddApplicationConfiguration(hostBuilderContext)
                    .AddApplicationServices()
                    .AddHostedService<Worker>())
                .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>());
    }
}
