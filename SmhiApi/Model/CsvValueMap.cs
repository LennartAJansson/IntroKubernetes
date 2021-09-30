using CsvHelper.Configuration;

namespace SmhiApi.Model
{
    public sealed class CsvValueMap : ClassMap<CsvValue>
    {
        public CsvValueMap()
        {
            Map(m => m.Date).Name("Datum");
            Map(m => m.Time).Name("Tid (UTC)");
            Map(m => m.Value).Name("Lufttemperatur");
            Map(m => m.Quality).Name("Kvalitet");
        }
    }
}
