using System.Net.Http;

//Use Microsofts namespace for logging to avoid having to add your own usings everywhere logextension is going to be used
namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogHttpRequest<T>(this ILogger<T> logger, LogLevel level, HttpRequestMessage request, string message, params object[] args)
            where T : class
        {
            logger.Log(level, message, args);

            logger.LogHttpRequest(level, request);

            logger.LogHttpContent(level, request.Content);
        }

        private static void LogHttpRequest<T>(this ILogger<T> logger, LogLevel level, HttpRequestMessage request)
            where T : class
        {
            string contentText = $"HTTP {request.Version} Method: {request.Method} Url: {request.RequestUri}";
            logger.Log(level, contentText);
        }

        public static void LogHttpResponse<T>(this ILogger<T> logger, LogLevel level, HttpResponseMessage response, string message, params object[] args)
            where T : class
        {
            logger.Log(level, message, args);

            logger.LogHttpResponse(level, response);

            logger.LogHttpContent(level, response.Content);
        }

        private static void LogHttpResponse<T>(this ILogger<T> logger, LogLevel level, HttpResponseMessage response)
            where T : class
        {
            string contentText = $"HTTP {response.Version} Statuscode: {response.StatusCode} - {response.ReasonPhrase}";
            logger.Log(level, contentText);
        }

        public static void LogHttpContent<T>(this ILogger<T> logger, LogLevel level, HttpContent content)
            where T : class
        {
            string contentText = content != null ? $"Content: {content.ReadAsStringAsync().Result}" : "Content: No Content";
            logger.Log(level, contentText);
        }
    }
}