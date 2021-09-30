using CsvHelper.Configuration;

namespace SmhiApi.Model
{
    public sealed class CsvPositionMap : ClassMap<CsvPosition>
    {
        public CsvPositionMap()
        {
            Map(m => m.From).Name("Tidsperiod (fr.o.m)");
            Map(m => m.To).Name("Tidsperiod (t.o.m)");
            Map(m => m.Altitude).Name("Höjd (meter över havet)");
            Map(m => m.Latitude).Name("Latitud (decimalgrader)");
            Map(m => m.Longitude).Name("Longitud (decimalgrader)");
        }
    }
}
