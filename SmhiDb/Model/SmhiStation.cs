using System.Collections.Generic;

namespace SmhiDb.Model
{
    public class SmhiStation
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string OwnerCategory { get; set; }
        public double Height { get; set; }
#nullable enable
        public int? SmhiParameterId { get; set; }
        public SmhiParameter? SmhiParameter { get; set; }
#nullable disable
        public ICollection<SmhiPosition> SmhiPositions { get; set; } = new HashSet<SmhiPosition>();
        public ICollection<SmhiLink> SmhiLinks { get; set; } = new HashSet<SmhiLink>();
        public ICollection<SmhiValue> SmhiValues { get; set; } = new HashSet<SmhiValue>();
    }
}
