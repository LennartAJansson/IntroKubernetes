using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SmhiExtractor.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using WeatherContracts;

namespace SmhiExtractor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IApiService service;
        private readonly IHostEnvironment environment;
        private bool ranOnce = false;

        public Worker(ILogger<Worker> logger, IApiService service, IHostEnvironment environment)
        {
            this.logger = logger;
            this.service = service;
            this.environment = environment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TimeSpan delay = TimeSpan.FromMinutes(5);

            if (environment.IsDevelopment())
            {
                delay = TimeSpan.FromSeconds(30);
            }

            while (!stoppingToken.IsCancellationRequested)
            {

                logger.LogInformation("Worker running at: {time}, {delay} left to sync requests", DateTimeOffset.Now, delay.ToString(@"mm\:ss"));
                await Task.Delay(delay, stoppingToken);

                if (!ranOnce)
                {
                    await service.SendQueueStationDataRequestAsync(new QueueStationDataRequest(RequestType.LatestMonths, "107420", "Gävle A")).ConfigureAwait(false);
                    await service.SendQueueStationDataRequestAsync(new QueueStationDataRequest(RequestType.LatestMonths, "62040", "Helsingborg A")).ConfigureAwait(false);

                    await service.SendQueueStationDataRequestAsync(new QueueStationDataRequest(RequestType.LatestDay, "107420", "Gävle A")).ConfigureAwait(false);
                    await service.SendQueueStationDataRequestAsync(new QueueStationDataRequest(RequestType.LatestDay, "62040", "Helsingborg A")).ConfigureAwait(false);

                    ranOnce = true;
                }

                await service.SendQueueStationDataRequestAsync(new QueueStationDataRequest(RequestType.LatestHour, "107420", "Gävle A")).ConfigureAwait(false);
                await service.SendQueueStationDataRequestAsync(new QueueStationDataRequest(RequestType.LatestHour, "62040", "Helsingborg A")).ConfigureAwait(false);

                for (int loop = 6; loop > 1; loop--)
                {
                    logger.LogInformation("Worker running at: {time}, {delay} left to sync requests", DateTimeOffset.Now, delay.Multiply(loop).ToString(@"mm\:ss"));
                    await Task.Delay(delay, stoppingToken);
                }
            }
        }
    }
}
