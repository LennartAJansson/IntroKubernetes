using SmhiApi.Model;

using System.Text.Json;
using System.Threading.Tasks;

namespace SmhiApi.Serializers
{
    public static class SmhiJsonSerializer
    {
        public static Task<JsonObservations> DeserializeAsync(string json)
        {
            JsonObservations result = JsonSerializer.Deserialize<JsonObservations>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Task.FromResult(result);
        }
    }
}
