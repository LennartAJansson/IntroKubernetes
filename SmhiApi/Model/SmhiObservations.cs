using SmhiDb.Model;

using System.Collections.Generic;

namespace SmhiApi.Model
{
    public class SmhiObservations
    {
        public SmhiStation Station { get; set; }
        public SmhiParameter Parameter { get; set; }
        public IEnumerable<SmhiPosition> Positions { get; set; }
        public IEnumerable<SmhiLink> Links { get; set; }
        public IEnumerable<SmhiValue> Values { get; set; }
    }
}
