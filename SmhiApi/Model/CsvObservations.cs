using System.Collections.Generic;

namespace SmhiApi.Model
{
    public class CsvObservations
    {
        public IEnumerable<CsvValue> Values { get; set; }
        public CsvParameter Parameter { get; set; }
        public CsvStation Station { get; set; }
        public IEnumerable<CsvPosition> Positions { get; set; }
    }
}
