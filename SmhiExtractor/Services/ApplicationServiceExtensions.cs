using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SmhiExtractor.Configurations;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SmhiExtractor.Services
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpClient<IApiService, ApiService>(ConfigureSecureHttpClientOptions);
            return services;
        }

        private static void ConfigureSecureHttpClientOptions(IServiceProvider serviceProvider, HttpClient client)
        {
            ApiClientSettings settings = serviceProvider.GetRequiredService<IOptions<ApiClientSettings>>().Value;
            client.BaseAddress = new Uri(settings.Url);
            client.Timeout = TimeSpan.FromSeconds(300);

            //https://stackoverflow.com/questions/30858890/how-to-use-httpclient-to-post-with-authentication

            //If using a ClientId + ClientSecret
            var byteArray = new UTF8Encoding().GetBytes($"{settings.ClientId}:{settings.ClientSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            //If using a JwtToken:
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
        }
    }
}
