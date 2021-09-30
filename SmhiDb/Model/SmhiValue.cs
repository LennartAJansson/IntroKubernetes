using System;

namespace SmhiDb.Model
{
    public class SmhiValue
    {
        public int Id { get; set; }
        public int SmhiStationId { get; set; }
        public SmhiStation SmhiStation { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Value { get; set; }
        public string Quality { get; set; }
    }
}
