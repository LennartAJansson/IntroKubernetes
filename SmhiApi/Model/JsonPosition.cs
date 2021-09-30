
using System;

namespace SmhiApi.Model
{
    public class JsonPosition
    {
        public long From { get; set; }
        public long To { get; set; }
        public double Height { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset FromTransformed => DateTimeOffset.FromUnixTimeSeconds(From / 1000).ToUniversalTime();
        public DateTimeOffset ToTransformed => DateTimeOffset.FromUnixTimeSeconds(To / 1000).ToUniversalTime();
    }
}
