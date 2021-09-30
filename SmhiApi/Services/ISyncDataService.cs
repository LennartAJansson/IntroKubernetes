using SmhiApi.Model;

using System.Threading;
using System.Threading.Tasks;

namespace SmhiApi.Services
{
    public interface ISyncDataService
    {
        Task<SmhiObservations> GetAsync(Request request, CancellationToken cancellationToken);
    }
}
