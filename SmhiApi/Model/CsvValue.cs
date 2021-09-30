namespace SmhiApi.Model
{
    public class CsvValue
    {
        public const string Header = "Datum";
        public string Date { get; set; }
        public string Time { get; set; }
        public double Value { get; set; }
        public string Quality { get; set; }
    }
}
