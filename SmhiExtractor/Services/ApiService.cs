
using Microsoft.Extensions.Logging;

using Prometheus;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using WeatherContracts;

namespace SmhiExtractor.Services
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> logger;
        private readonly HttpClient client;
        private readonly Gauge requestExecuteTime;

        public ApiService(ILogger<ApiService> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
            requestExecuteTime = Metrics.CreateGauge("smhiextractor_request_executiontime", "Counts total execution time for sending requests", new GaugeConfiguration
            {
                LabelNames = new[] { "path", "method", "result" }
            });
        }

        public async Task SendQueueStationDataRequestAsync(QueueStationDataRequest request)
        {
            DateTime startDateTime = DateTime.Now;

            string path = "update/stations";

            string json = JsonSerializer.Serialize(request);

            HttpContent content = CreatePostContent($"{client.BaseAddress}{path}", json);

            int result = await SendData(path, content).ConfigureAwait(false);

            DateTime endDateTime = DateTime.Now;
            requestExecuteTime.Labels($"{client.BaseAddress}{path}", "Post", result.ToString()).Set((endDateTime - startDateTime).TotalMilliseconds);
        }

        private HttpContent CreatePostContent(string url, string json)
        {
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            logger.LogHttpRequest(LogLevel.Debug, new(HttpMethod.Post, url)
            {
                Content = content
            }, "Request to web api");

            return content;
        }

        private async Task<int> SendData(string path, HttpContent content)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsync(path, content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                logger.LogHttpResponse(LogLevel.Information, response, "Response from web api");
                return (int)response.StatusCode;
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error in posting observation");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in posting observation");
            }

            return -1;
        }
    }
}
