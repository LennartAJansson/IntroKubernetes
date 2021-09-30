using System.Net.Http;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogHttpRequest<T>(this ILogger<T> logger, LogLevel level, HttpRequestMessage request, string message, params object[] args)
            where T : class
        {
            string json = request.Content != null ? request.Content.ReadAsStringAsync().Result : "No Content";
            string debugMessage = $"(HTTP {request.Version}) Method: {request.Method} Url: {request.RequestUri}\nContent: {json}";
            logger.Log(level, message, args);
            logger.Log(level, debugMessage);
        }

        public static void LogHttpResponse<T>(this ILogger<T> logger, LogLevel level, HttpResponseMessage response, string message, params object[] args)
            where T : class
        {
            string json = response.Content != null ? response.Content.ReadAsStringAsync().Result : "No Content";
            string debugMessage = $"(HTTP {response.Version}) Statuscode: {response.StatusCode} - {response.ReasonPhrase}\nContent: {json}";
            logger.Log(level, message, args);
            logger.Log(level, debugMessage);
        }
    }
}
