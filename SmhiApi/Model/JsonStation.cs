using System.Text.Json.Serialization;

namespace SmhiApi.Model
{
    public class JsonStation
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("owner")]
        public string Owner { get; set; }
        [JsonPropertyName("ownerCategory")]
        public string OwnerCategory { get; set; }
        [JsonPropertyName("height")]
        public double Height { get; set; }
    }
}
