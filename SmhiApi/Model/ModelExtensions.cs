
using SmhiDb.Model;

using System;
using System.Linq;

namespace SmhiApi.Model
{
    /// <summary>
    /// This is a class that transforms from Json and Csv values to datamodel
    /// </summary>
    public static class ModelExtensions
    {
        public static SmhiObservations ToSmhiObservations(this CsvObservations observations, string nameIfMissing)
        {
            return new()
            {
                Station = observations.Station.ToSmhiStation(nameIfMissing),
                Parameter = observations.Parameter.ToSmhiParameter(),
                Positions = observations.Positions.Select(p => p.ToSmhiPosition()),
                Links = Enumerable.Empty<SmhiLink>(),
                Values = observations.Values.Select(v => v.ToSmhiValue())
            };
        }

        public static SmhiObservations ToSmhiObservations(this JsonObservations observations, string nameIfMissing)
        {
            return new()
            {
                Station = observations.Station.ToSmhiStation(nameIfMissing),
                Parameter = observations.Parameter.ToSmhiParameter(),
                Positions = observations.Positions.Select(p => p.ToSmhiPosition()),
                Links = observations.Links.Select(l => l.ToSmhiLink()),
                Values = observations.Values.Select(v => v.ToSmhiValue())
            };
        }

        public static SmhiStation ToSmhiStation(this CsvStation station, string nameIfMissing) =>
            new() { Key = station.Key, Name = string.IsNullOrWhiteSpace(station.Name) ? nameIfMissing : station.Name, Owner = "", OwnerCategory = "", Height = station.Height };
        public static SmhiStation ToSmhiStation(this JsonStation station, string nameIfMissing) =>
            new() { Key = station.Key, Name = string.IsNullOrWhiteSpace(station.Name) ? nameIfMissing : station.Name, Owner = station.Owner, OwnerCategory = station.OwnerCategory, Height = station.Height };

        public static SmhiParameter ToSmhiParameter(this CsvParameter parameter) =>
            new() { Key = "", Name = parameter.Name, Summary = parameter.Description, Unit = parameter.Unit };
        public static SmhiParameter ToSmhiParameter(this JsonParameter parameter) =>
            new() { Key = parameter.Key, Name = parameter.Name, Summary = parameter.Summary, Unit = parameter.Unit };

        public static SmhiPosition ToSmhiPosition(this CsvPosition position) =>
            new() { From = position.From, To = position.To, Latitude = position.Latitude, Longitude = position.Longitude, Height = position.Altitude };
        public static SmhiPosition ToSmhiPosition(this JsonPosition position) =>
            new() { From = position.FromTransformed, To = position.ToTransformed, Latitude = position.Latitude, Longitude = position.Longitude, Height = position.Height };

        public static SmhiLink ToSmhiLink(this JsonLink link) =>
            new() { Type = link.Type, Rel = link.Rel, Href = link.Href };

        public static SmhiValue ToSmhiValue(this CsvValue value) =>
            new() { Date = DateTimeOffset.Parse($"{value.Date} {value.Time}"), Quality = value.Quality, Value = value.Value };
        public static SmhiValue ToSmhiValue(this JsonValue value) =>
            new() { Date = value.DateTransformed, Quality = value.Quality, Value = value.ValueTransformed };
    }
}
