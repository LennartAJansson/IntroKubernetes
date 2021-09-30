
using System;

namespace SmhiApi.Model
{
    public class JsonValue
    {
        public long Date { get; set; }
        public string Value { get; set; }
        public string Quality { get; set; }
        public DateTimeOffset DateTransformed => DateTimeOffset.FromUnixTimeSeconds(Date / 1000).ToUniversalTime();
        public double ValueTransformed => Value.ToDouble(0.0);
    }
}
