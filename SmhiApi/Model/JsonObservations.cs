
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmhiApi.Model
{
    public class JsonObservations
    {
        [JsonPropertyName("parameter")]
        public JsonParameter Parameter { get; set; }
        [JsonPropertyName("station")]
        public JsonStation Station { get; set; }
        [JsonPropertyName("period")]
        public JsonPeriod Period { get; set; }

        [JsonPropertyName("position")]
        public IEnumerable<JsonPosition> Positions { get; set; }
        [JsonPropertyName("link")]
        public IEnumerable<JsonLink> Links { get; set; }
        [JsonPropertyName("value")]
        public IEnumerable<JsonValue> Values { get; set; }
        public long Updated { get; set; }
        public DateTimeOffset UpdatedTransformed => DateTimeOffset.FromUnixTimeSeconds(Updated / 1000).ToUniversalTime();
    }
}
