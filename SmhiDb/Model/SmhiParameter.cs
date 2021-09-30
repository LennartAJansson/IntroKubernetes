using System.Collections.Generic;

namespace SmhiDb.Model
{
    public class SmhiParameter
    {
        public int Id { get; set; }
        public string Key { get; set; }//Not related to SmhiStation.Key
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Unit { get; set; }
        public ICollection<SmhiStation> SmhiStations { get; set; } = new HashSet<SmhiStation>();
    }
}
