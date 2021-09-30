using System;

namespace SmhiDb.Model
{
    public class SmhiPosition
    {
        public int Id { get; set; }
        public int SmhiStationId { get; set; }
        public SmhiStation SmhiStation { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public double Height { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
