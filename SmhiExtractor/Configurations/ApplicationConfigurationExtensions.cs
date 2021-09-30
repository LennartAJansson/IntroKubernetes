using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SmhiExtractor.Configurations
{
    public static class ApplicationConfigurationExtensions
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, HostBuilderContext context)
        {
            services.Configure<ApiClientSettings>(settings =>
                context.Configuration.GetSection(ApiClientSettings.SectionName).Bind(settings));

            services.Configure<SmhiClientSettings>(settings =>
                context.Configuration.GetSection(SmhiClientSettings.SectionName).Bind(settings));

            return services;
        }
    }
}
