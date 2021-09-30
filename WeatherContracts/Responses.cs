using System;
using System.Collections.Generic;

namespace WeatherContracts
{
    //public record AddStationRequest(StationDTO Station, ParameterDTO Parameter, IEnumerable<PositionDTO> Positions, IEnumerable<LinkDTO> Links);
    //public record AddStationResponse();

    //public record AddValueRequest(string StationKey, ValueDTO Value);
    //public record AddValueResponse();

    //public record AddValuesRequest(string StationKey, IEnumerable<ValueDTO> Values);
    //public record AddValuesResponse();

    //GetStations in WeatherDataController
    public record GetStationsResponse(IEnumerable<StationDTO> Stations);

    //GetStationByStationId in WeatherDataController
    public record GetStationByStationKeyResponse(StationDTO Station, ParameterDTO Parameter, IEnumerable<PositionDTO> Positions, IEnumerable<LinkDTO> Links);

    //GetValuesByStationId in WeatherDataController
    public record GetValuesByStationIdResponse(IEnumerable<ValueDTO> Values);

    //Queue data retrieval from source in UpdateController
    public record QueueStationDataRequest(RequestType RequestType, string StationKey, string NameIfMissing);


    //DTO's used by all responses
    public record StationDTO(string Key, string Name, string Owner, string OwnerCategory, double Height);
    public record ParameterDTO(string Key, string Name, string Summary, string Unit);
    public record PositionDTO(double Height, double Latitude, double Longitude, DateTimeOffset FromTransformed, DateTimeOffset ToTransformed);
    public record LinkDTO(string Rel, string Type, string Href);
    public record ValueDTO(string Quality, DateTimeOffset DateTransformed, double ValueTransformed);

    public enum RequestType
    {
        LatestHour,
        LatestDay,
        LatestMonths,
        CorrectedArchive
    }

}
