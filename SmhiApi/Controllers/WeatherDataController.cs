using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Prometheus;

using SmhiDb.Abstract;

using System;
using System.Threading.Tasks;

using WeatherContracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmhiApi.Controllers
{
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing
    [Route("[controller]")]
    [ApiController]
    public class WeatherDataController : ControllerBase
    {
        private readonly ILogger<WeatherDataController> logger;
        private readonly ISmhiDbService service;
        private readonly Gauge weatherdataRequestExecuteTime;

        public WeatherDataController(ILogger<WeatherDataController> logger, ISmhiDbService service)
        {
            this.logger = logger;
            this.service = service;
            weatherdataRequestExecuteTime = Metrics.CreateGauge("weatherdata_request_executiontime", "Counts total execution time for handling weatherdata requests", new GaugeConfiguration
            {
                LabelNames = new[] { "path", "method" }
            });
        }

        // GET <WeatherDataController>/stations
        /// <summary>
        /// This route will return all stations
        /// </summary>
        /// <returns>All stations</returns>
        [HttpGet("stations")]
        public async Task<GetStationsResponse> GetStations()
        //async Task<ActionResult<TodoItem>>
        {
            DateTime startDateTime = DateTime.Now;

            logger.LogInformation("Getting stations");
            GetStationsResponse result = await service.GetStationsAsync();

            DateTime endDateTime = DateTime.Now;
            weatherdataRequestExecuteTime.Labels($"weatherdata/stations/", "GET").Set((endDateTime - startDateTime).TotalMilliseconds);

            return result;
        }

        // GET <WeatherDataController>/stations/5
        /// <summary>
        /// Get all info regarding a specific station
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns>All info regarding a specific station</returns>
        [HttpGet("stations/{stationKey}")]
        public async Task<GetStationByStationKeyResponse> GetStationByStationId(string stationKey)
        //async Task<ActionResult<TodoItem>>
        {
            DateTime startDateTime = DateTime.Now;

            logger.LogInformation("Getting station {stationKey}", stationKey);
            GetStationByStationKeyResponse result = await service.GetStationByStationKeyAsync(stationKey);

            DateTime endDateTime = DateTime.Now;
            weatherdataRequestExecuteTime.Labels($"weatherdata/stations/{stationKey}", "GET").Set((endDateTime - startDateTime).TotalMilliseconds);

            return result;
        }

        // GET: <WeatherDataController>/stations/{stationKey}&{fromDate}&{toDate}
        /// <summary>
        /// Get all values from a specific station between fromDate and toDate
        /// </summary>
        /// <param name="stationKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>Values from a specific station between fromDate and toDate</returns>
        [HttpGet("values/{stationKey}/{fromDate}&{toDate}")]
        public async Task<GetValuesByStationIdResponse> GetValuesByStationId(string stationKey, DateTime? fromDate, DateTime? toDate)
        //async Task<ActionResult<TodoItem>>
        {
            DateTime startDateTime = DateTime.Now;

            DateTime from = fromDate != null ? fromDate.Value : DateTime.MinValue;
            DateTime to = toDate != null ? toDate.Value : DateTime.Now;

            logger.LogInformation("Getting values between {from} and {to} for station {stationKey}", from, to, stationKey);
            GetValuesByStationIdResponse result = await service.GetValuesByStationIdAsync(stationKey, from, to);

            DateTime endDateTime = DateTime.Now;
            weatherdataRequestExecuteTime.Labels($"weatherdata/values/{stationKey}", "GET").Set((endDateTime - startDateTime).TotalMilliseconds);

            return result;
        }
    }
}
