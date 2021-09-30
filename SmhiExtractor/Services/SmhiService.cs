namespace SmhiExtractor.Services
{
    public interface ISmhiService
    {
    }

    public class SmhiService : ISmhiService
    {
    }
    //Roten
    //https://opendata-download-metobs.smhi.se/api/version/1.0.json
    //Get all kind of parameters (Stationtypes)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter.json

    //Get all stations for a specific parameter (1 - Lufttemperatur - momentanvärde, 1 gång/tim)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/1.json
    //Get all stations for a specific parameter (9 - Lufttryck reducerat havsytans nivå - vid havsytans nivå, momentanvärde, 1 gång/tim)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/9.json
    //Get all stations for a specific parameter (6 - Relativ Luftfuktighet - momentanvärde, 1 gång/tim)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/6.json
    //Get all stations for a specific parameter (12 - Sikt - momentanvärde, 1 gång/tim)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/12.json
    //Get all stations for a specific parameter (4 - Vindhastighet - medelvärde 10 min, 1 gång/tim)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/4.json
    //Get all stations for a specific parameter (5 - Nederbördsmängd - summa 1 dygn, 1 gång/dygn, kl 06)
    //https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/5.json

}
