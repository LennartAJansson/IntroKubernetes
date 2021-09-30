
using System;

namespace SmhiApi.Model
{
    public class CsvPosition
    {
        public const string Header = "Tidsperiod (fr.o.m)";
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
