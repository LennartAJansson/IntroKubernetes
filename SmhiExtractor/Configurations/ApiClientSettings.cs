namespace SmhiExtractor.Configurations
{
    public class ApiClientSettings
    {
        public const string SectionName = "ApiClientSettings";
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
