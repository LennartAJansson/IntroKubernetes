using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SmhiApi.Configurations;

using System;
using System.Net.Http;

namespace SmhiApi.Services
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpClient<ISyncDataService, SyncDataService>(ConfigureUnsecureHttpClientOptions);
            services.AddSingleton<RequestQueue>();

            return services;
        }
        private static void ConfigureUnsecureHttpClientOptions(IServiceProvider serviceProvider, HttpClient client)
        {
            SmhiClientSettings settings = serviceProvider.GetRequiredService<IOptions<SmhiClientSettings>>().Value;
            client.BaseAddress = new Uri(settings.Url);
            client.Timeout = TimeSpan.FromSeconds(300);
        }
    }
}
