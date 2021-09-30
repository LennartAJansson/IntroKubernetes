using SmhiDb.Model;

using WeatherContracts;

namespace SmhiDb.Extensions
{
    public static class SmhiExtensions
    {
        public static StationDTO ToStationDTO(this SmhiStation station)
            => new(station.Key, station.Name, station.Owner, station.OwnerCategory, station.Height);

        public static ParameterDTO ToParameterDTO(this SmhiParameter parameter)
            => new(parameter.Key, parameter.Name, parameter.Summary, parameter.Unit);

        public static PositionDTO ToPositionDTO(this SmhiPosition position)
            => new(position.Height, position.Latitude, position.Longitude, position.From, position.To);

        public static LinkDTO ToLinkDTO(this SmhiLink link)
            => new(link.Rel, link.Type, link.Href);

        public static ValueDTO ToValueDTO(this SmhiValue value)
            => new(value.Quality, value.Date, value.Value);

        public static SmhiParameter ToSmhiParameter(this ParameterDTO parameter)
            => new() { Key = parameter.Key, Name = parameter.Name, Summary = parameter.Summary, Unit = parameter.Unit };

        public static SmhiStation ToSmhiStation(this StationDTO station, SmhiParameter parameter)
            => new() { Key = station.Key, Name = station.Name, Owner = station.Owner, OwnerCategory = station.OwnerCategory, Height = station.Height, SmhiParameter = parameter };

        public static SmhiPosition ToSmhiPosition(this PositionDTO position, SmhiStation station)
            => new() { From = position.FromTransformed, To = position.ToTransformed, Latitude = position.Latitude, Longitude = position.Longitude, Height = position.Height, SmhiStation = station, SmhiStationId = station.Id };

        public static SmhiValue ToSmhiValue(this ValueDTO observation, SmhiStation station)
            => new() { Date = observation.DateTransformed, Value = observation.ValueTransformed, Quality = observation.Quality, SmhiStation = station, SmhiStationId = station.Id };

        public static SmhiLink ToSmhiLink(this LinkDTO link, SmhiStation station)
            => new() { Rel = link.Rel, Type = link.Type, Href = link.Href, SmhiStation = station, SmhiStationId = station.Id };
    }
}
