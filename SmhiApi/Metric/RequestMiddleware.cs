using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Prometheus;

using System;
using System.Threading.Tasks;

namespace SmhiApi.Metric
{
    //https://www.c-sharpcorner.com/article/reporting-metrics-to-prometheus-in-asp-net-core/
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1
    public class RequestMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestMiddleware> logger;
        private readonly Counter totalRequests;
        private readonly Counter okRequests;
        private readonly Counter exceptionRequests;
        private readonly Gauge requestExecuteTime;

        public RequestMiddleware(ILogger<RequestMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;

            //This counter count all requests
            totalRequests = Metrics.CreateCounter("smhiapi_request_total", "HTTP Requests Total", new CounterConfiguration
            {
                LabelNames = new[] { "path", "method", "status" }
            });

            //This counter count all ok requests
            okRequests = Metrics.CreateCounter("smhiapi_request_ok", "This fields indicates the transactions that were processed correctly.", new CounterConfiguration
            {
                LabelNames = new[] { "path", "method", "status" }
            });

            //This counter count all request ending in exception
            exceptionRequests = Metrics.CreateCounter("smhiapi_request_exception", "This fields indicates the exception count.", new CounterConfiguration
            {
                LabelNames = new[] { "path", "method", "status" }
            });

            requestExecuteTime = Metrics.CreateGauge("smhiapi_request_executiontime", "Counts total execution time for handling requests", new GaugeConfiguration
            {
                LabelNames = new[] { "path", "method", "status" }
            });
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string host = httpContext.Request.Host.Value;
            string path = httpContext.Request.Path.Value;
            string method = httpContext.Request.Method;

            //Set default http statuscode
            int statusCode = 200;

            DateTime startDateTime = DateTime.Now;

            if (!path.Contains("/metrics") && !path.Contains("/healthy"))
            {
                try
                {
                    //Call down the chain (will end up in the controller endpoints)
                    await next.Invoke(httpContext);
                    statusCode = httpContext.Response.StatusCode;
                    okRequests.Labels(host, "get/post", "200").Inc();
                    //okRequests.Labels(path, method, statusCode.ToString()).Inc();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error in the controller {method} {host}{path}");
                    logger.LogError("------------------------------------------------------------------");
                    statusCode = 500;
                    exceptionRequests.Labels(host, "get/post", "500").Inc();
                    //exceptionRequests.Labels(path, method, statusCode.ToString()).Inc();
                    throw;
                }
                finally
                {
                    DateTime endDateTime = DateTime.Now;
                    requestExecuteTime.Labels(host, "get/post", "???").Set((endDateTime - startDateTime).TotalMilliseconds);
                    //requestExecuteTime.Labels(path, method, statusCode.ToString()).Set((endDateTime - startDateTime).TotalMilliseconds);
                    totalRequests.Labels(host, "get/post", "???").Inc();
                    //totalRequests.Labels(path, method, statusCode.ToString()).Inc();
                }
            }
        }
    }
}