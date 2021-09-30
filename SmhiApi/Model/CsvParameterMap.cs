using CsvHelper.Configuration;

namespace SmhiApi.Model
{
    public sealed class CsvParameterMap : ClassMap<CsvParameter>
    {
        public CsvParameterMap()
        {
            Map(m => m.Name).Name("Parameternamn");
            Map(m => m.Description).Name("Beskrivning");
            Map(m => m.Unit).Name("Enhet");
        }
    }
}
