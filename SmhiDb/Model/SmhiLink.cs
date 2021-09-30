namespace SmhiDb.Model
{
    public class SmhiLink
    {
        public int Id { get; set; }
        public int SmhiStationId { get; set; }
        public SmhiStation SmhiStation { get; set; }
        public string Rel { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
    }
}
