using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SmhiApi.Model;
using SmhiApi.Services;

using SmhiDb.Abstract;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmhiApi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ISyncDataService service;
        private readonly ISmhiDbService dbService;
        private readonly RequestQueue requestQueue;
        private readonly IHostEnvironment environment;

        public Worker(ILogger<Worker> logger, ISyncDataService dataService, ISmhiDbService dbService, RequestQueue requestQueue, IHostEnvironment environment)
        {
            this.logger = logger;
            this.service = dataService;
            this.dbService = dbService;
            this.requestQueue = requestQueue;
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
                logger.LogInformation("Update Worker running at: {time}, checking for update requests", DateTimeOffset.Now);

                while (!requestQueue.Queue.IsEmpty)
                {
                    if (requestQueue.Queue.TryDequeue(out Request request))
                    {
                        logger.LogInformation("Got update request from queue: {stationKey}", request.StationKey);

                        SmhiObservations observations = await service.GetAsync(request, stoppingToken);

                        if (observations != null)
                        {
                            await dbService.AddAsync(observations.Station, observations.Parameter, observations.Positions, observations.Links, observations.Values, stoppingToken);
                        }
                    }
                    else
                    {
                        logger.LogInformation("No new update requests from queue");
                    }
                }

                logger.LogInformation("Update Worker running at: {time}, {delay} left before checking for update requests", DateTimeOffset.Now, delay.ToString(@"mm\:ss"));
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
