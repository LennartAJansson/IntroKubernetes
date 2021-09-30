using CsvHelper.Configuration;

namespace SmhiApi.Model
{
    public sealed class CsvStationMap : ClassMap<CsvStation>
    {
        public CsvStationMap()
        {
            Map(m => m.Name).Name("Stationsnamn");
            Map(m => m.Key).Name("Klimatnummer");
            Map(m => m.Height).Name("Mäthöjd (meter över marken)");
        }
    }
}
