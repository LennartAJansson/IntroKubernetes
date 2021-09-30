using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Prometheus;

using SmhiApi.Services;

using System;
using System.Threading.Tasks;

using WeatherContracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmhiApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UpdateController
    {
        private readonly ILogger<UpdateController> logger;
        private readonly RequestQueue request;
        private readonly Gauge updateRequestExecuteTime;

        public UpdateController(ILogger<UpdateController> logger, RequestQueue request)
        {
            this.logger = logger;
            this.request = request;
            updateRequestExecuteTime = Metrics.CreateGauge("smhiapi_update_request_executiontime", "Counts total execution time for handling update requests", new GaugeConfiguration
            {
                LabelNames = new[] { "path", "method" }
            });
        }

        // POST <UpdateController>
        /// <summary>
        /// Enqueue GetStationDataRequest so Worker can pick it up and run the request on a separate thread
        /// </summary>
        /// <param name="stationDataRequest">QueueStationDataRequest</param>
        /// <returns></returns>
        [HttpPost("stations/")]
        public Task PostAsync([FromBody] QueueStationDataRequest stationDataRequest)
        {
            DateTime startDateTime = DateTime.Now;

            request.Queue.Enqueue(new Request
            {
                RequestType = stationDataRequest.RequestType,
                StationKey = stationDataRequest.StationKey,
                //TODO! Subject to obsoletion
                NameIfMissing = stationDataRequest.NameIfMissing
            });

            logger.LogInformation("Queued station {stationKey}", stationDataRequest.StationKey);

            DateTime endDateTime = DateTime.Now;
            updateRequestExecuteTime.Labels($"update/stations/", "POST").Set((endDateTime - startDateTime).TotalMilliseconds);

            return Task.CompletedTask;
        }
    }
}
