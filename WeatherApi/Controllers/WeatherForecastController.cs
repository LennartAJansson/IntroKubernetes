namespace WeatherApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Prometheus;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Gauge weatherdataRequestExecuteTime;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            weatherdataRequestExecuteTime = Metrics.CreateGauge("weatherdata_request_executiontime", "Counts total execution time for handling weatherdata requests", new GaugeConfiguration
            {
                LabelNames = new[] { "path", "method" }
            });
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            DateTime startDateTime = DateTime.Now;
            Random rng = new Random();
            WeatherForecast[] result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            DateTime endDateTime = DateTime.Now;
            weatherdataRequestExecuteTime.Labels($"weatherforecast/", "GET").Set((endDateTime - startDateTime).TotalMilliseconds);

            return result;
        }
    }
}
