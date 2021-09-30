using System.Threading.Tasks;

using WeatherContracts;

namespace SmhiExtractor.Services
{
    public interface IApiService
    {
        Task SendQueueStationDataRequestAsync(QueueStationDataRequest request);
    }
}
