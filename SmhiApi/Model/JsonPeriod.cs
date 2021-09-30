
using System;

namespace SmhiApi.Model
{
    public class JsonPeriod
    {
        public string Key { get; set; }
        public long From { get; set; }
        public long To { get; set; }
        public string Summary { get; set; }
        public string Sampling { get; set; }
        public DateTimeOffset FromTransformed => DateTimeOffset.FromUnixTimeSeconds(From / 1000).ToUniversalTime();
        public DateTimeOffset ToTransformed => DateTimeOffset.FromUnixTimeSeconds(To / 1000).ToUniversalTime();
    }
}
