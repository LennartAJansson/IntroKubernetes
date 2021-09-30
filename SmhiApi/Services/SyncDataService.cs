
using Microsoft.Extensions.Logging;

using SmhiApi.Model;
using SmhiApi.Serializers;

using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using WeatherContracts;

namespace SmhiApi.Services
{
    public class SyncDataService : ISyncDataService
    {
        private readonly ILogger<SyncDataService> logger;
        private readonly HttpClient client;

        public SyncDataService(ILogger<SyncDataService> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task<SmhiObservations> GetAsync(Request request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting station from SMHI: {stationKey}", request.StationKey);
            string path = "";

            switch (request.RequestType)
            {
                case RequestType.LatestHour:
                    path = $"{request.StationKey}/period/latest-hour/data.json";
                    break;
                case RequestType.LatestDay:
                    path = $"{request.StationKey}/period/latest-day/data.json";
                    break;
                case RequestType.LatestMonths:
                    path = $"{request.StationKey}/period/latest-months/data.json";
                    break;
                case RequestType.CorrectedArchive:
                    path = $"{request.StationKey}/period/corrected-archive/data.csv";
                    break;
            }


            logger.LogHttpRequest(LogLevel.Information, new(HttpMethod.Get, $"{client.BaseAddress}{path}"), "Request to web api");

            if (request.RequestType == RequestType.CorrectedArchive)
            {
                CsvObservations observations = await GetCsvObservationsAsync(path);
                return observations.ToSmhiObservations(request.NameIfMissing);
            }
            else
            {
                JsonObservations observations = await GetJsonObservationsAsync(path);
                return observations.ToSmhiObservations(request.NameIfMissing);
            }
        }

        private async Task<JsonObservations> GetJsonObservationsAsync(string path)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                logger.LogHttpResponse(LogLevel.Information, response, "Response from web api");

                string json = await response.Content.ReadAsStringAsync();

                JsonObservations result = await SmhiJsonSerializer.DeserializeAsync(json);

                return result;
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error in getting observations");
            }

            return null;
        }

        private async Task<CsvObservations> GetCsvObservationsAsync(string path)
        {
            try
            {
                string filename = @".\data.csv";

                using HttpResponseMessage response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead);

                using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();

                using (Stream streamToWriteTo = File.Open(filename, FileMode.Create))
                {
                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                }

                await Task.Delay(3000);

                return await SmhiCsvSerializer.DeserializeAsync(filename);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error in getting historical observations");
            }

            return null;
        }
    }
}
