using WeatherContracts;

namespace SmhiApi.Services
{
    public class Request
    {
        public RequestType RequestType { get; set; }
        public string StationKey { get; set; }
        public string NameIfMissing { get; set; }
    }
}
