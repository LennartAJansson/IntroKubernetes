namespace SmhiApi.Configurations
{
    //url=servicename.namespace.svc.cluster.local

    public class SmhiClientSettings
    {
        public const string SectionName = "SmhiClientSettings";
        public string Url { get; set; }
    }
}
