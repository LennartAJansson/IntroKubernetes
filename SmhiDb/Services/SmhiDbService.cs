using Microsoft.Extensions.Logging;

using Prometheus;

using SmhiDb.Abstract;
using SmhiDb.Extensions;
using SmhiDb.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WeatherContracts;

namespace SmhiDb.Services
{
    public class SmhiDbService : ISmhiDbService
    {
        private readonly ILogger<SmhiDbService> logger;
        private readonly Counter totalRequests;

        #region SmhiDbContext exposed as IUnitOfWork
        private readonly IUnitOfWork unitOfWork;
        #endregion DbContext exposed as IUnitOfWork

        #region DbSet's exposed as IGenericRepositorys
        private IGenericRepository<SmhiStation> Stations { get; set; }
        private IGenericRepository<SmhiParameter> Parameters { get; set; }
        private IGenericRepository<SmhiPosition> Positions { get; set; }
        private IGenericRepository<SmhiLink> Links { get; set; }
        private IGenericRepository<SmhiValue> Values { get; set; }
        #endregion DbSet's exposed as IGenericRepositorys

        public SmhiDbService(ILogger<SmhiDbService> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            Stations = unitOfWork.Repository<SmhiStation>();
            Parameters = unitOfWork.Repository<SmhiParameter>();
            Positions = unitOfWork.Repository<SmhiPosition>();
            Links = unitOfWork.Repository<SmhiLink>();
            Values = unitOfWork.Repository<SmhiValue>();

            //This counter count all requests
            totalRequests = Metrics.CreateCounter("smhidb_request_total", "Db Requests Total", new CounterConfiguration
            {
                LabelNames = new[] { "method", "station" }
            });
        }

        #region Used in UpdateController
        public async Task AddAsync(SmhiStation station, SmhiParameter parameter, IEnumerable<SmhiPosition> positions, IEnumerable<SmhiLink> links, IEnumerable<SmhiValue> values, CancellationToken cancellationToken)
        {
            SmhiParameter smhiParameter = await AddOrGetParameter(parameter, station.Key, cancellationToken);//Will save changes

            station.SmhiParameter = smhiParameter;
            station.SmhiParameterId = smhiParameter.Id;
            //smhiParameter.SmhiStations.Add(station);

            SmhiStation smhiStation = await AddOrGetStation(station, cancellationToken);//Will save changes

            await AddOrGetPositions(positions, smhiStation.Id, smhiStation.Key, cancellationToken);
            await AddOrGetLinks(links, smhiStation.Id, smhiStation.Key, cancellationToken);
            await AddOrGetValues(values, smhiStation.Id, smhiStation.Key, cancellationToken);
        }
        #endregion Used in UpdateController

        #region Used in WeatherDataController
        public Task<GetStationsResponse> GetStationsAsync()
        {
            IEnumerable<SmhiStation> stations = unitOfWork.Repository<SmhiStation>()
                .Get();

            GetStationsResponse result = new(stations.Select(s => s.ToStationDTO()));

            totalRequests.Labels("Get stations", "").Inc();

            return Task.FromResult(result);
        }

        public Task<GetStationByStationKeyResponse> GetStationByStationKeyAsync(string stationKey)
        {
            SmhiStation station = unitOfWork.Repository<SmhiStation>()
                .Get(filter: s => s.Key == stationKey, includeProperties: "SmhiParameter,SmhiPositions,SmhiLinks")
                .FirstOrDefault();

            GetStationByStationKeyResponse result = new(station.ToStationDTO(),
                station.SmhiParameter.ToParameterDTO(),
                station.SmhiPositions.Select(p => p.ToPositionDTO()),
                station.SmhiLinks.Select(l => l.ToLinkDTO()));

            totalRequests.Labels("Get station", stationKey).Inc();

            return Task.FromResult(result);
        }

        public Task<GetValuesByStationIdResponse> GetValuesByStationIdAsync(string stationKey, DateTime? fromDate, DateTime? toDate)
        {
            DateTimeOffset from = fromDate != null ? fromDate.Value.ToUniversalTime() : DateTimeOffset.MinValue;
            DateTimeOffset to = toDate != null ? toDate.Value.ToUniversalTime() : DateTimeOffset.Now;

            SmhiStation station = unitOfWork.Repository<SmhiStation>()
                .Get(filter: s => s.Key == stationKey, includeProperties: "SmhiValues")
                .FirstOrDefault();

            GetValuesByStationIdResponse result = new(station.SmhiValues.Where(v => v.Date > from && v.Date <= to).Select(v => v.ToValueDTO()));

            totalRequests.Labels("Get values", stationKey).Inc(result.Values.Count());

            return Task.FromResult(result);
        }

        #endregion Used in WeatherDataController

        #region Internal support routines
        private async Task<SmhiParameter> AddOrGetParameter(SmhiParameter parameter, string stationKey, CancellationToken cancellationToken)
        {
            if (Parameters.Exists(p => p.Key == parameter.Key))
            {
                logger.LogInformation("Found parameter with key {key}", parameter.Key);
                parameter = Parameters.Get(p => p.Key == parameter.Key).First();
            }
            else
            {
                logger.LogInformation("Adding parameter with key {key}", parameter.Key);
                await Parameters.InsertAsync(parameter);
                totalRequests.Labels("Add parameter", stationKey).Inc();
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return parameter;
        }

        private async Task<SmhiStation> AddOrGetStation(SmhiStation station, CancellationToken cancellationToken)
        {
            if (Stations.Exists(s => s.Key.Trim() == station.Key.Trim()))
            {
                logger.LogInformation("Found station with key {key}", station.Key);
                station = Stations.Get(s => s.Key.Trim() == station.Key.Trim()).FirstOrDefault();
            }
            else
            {
                logger.LogInformation("Adding station with key {key}", station.Key);
                await Stations.InsertAsync(station);
                totalRequests.Labels("Add station", station.Key).Inc();
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return station;
        }

        private async Task<IEnumerable<SmhiPosition>> AddOrGetPositions(IEnumerable<SmhiPosition> positions, int stationId, string stationKey, CancellationToken cancellationToken)
        {
            int added = 0;
            foreach (SmhiPosition position in positions)
            {
                if (!Positions.Exists(p => p.Latitude == position.Latitude && p.Longitude == position.Longitude))
                {
                    added++;
                    logger.LogInformation("Adding position for the period {from} - {to}", position.From, position.To);
                    position.SmhiStationId = stationId;
                    await Positions.InsertAsync(position);
                    totalRequests.Labels("Add position", stationKey).Inc();
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Added and saved {added} positions out of {count}", added, positions.Count());

            return Positions.Get(p => p.SmhiStationId == stationId);
        }

        private async Task<IEnumerable<SmhiLink>> AddOrGetLinks(IEnumerable<SmhiLink> links, int stationId, string stationKey, CancellationToken cancellationToken)
        {
            int added = 0;
            foreach (SmhiLink link in links.Where(l => l.Rel == "data" && (l.Type == "text/plain" || l.Type == "application/json")))
            {
                if (!Links.Exists(l => l.Href == link.Href))
                {
                    added++;
                    logger.LogInformation("Adding link for {rel} - {type}", link.Rel, link.Type);
                    link.SmhiStationId = stationId;
                    await Links.InsertAsync(link);
                    totalRequests.Labels("Add link", stationKey).Inc();
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Added and saved {added} links out of {count}", added, links.Count());

            return Links.Get(l => l.SmhiStationId == stationId);
        }

        private async Task<IEnumerable<SmhiValue>> AddOrGetValues(IEnumerable<SmhiValue> values, int stationId, string stationKey, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting to add {count} values, could take a while", values.Count());
            var existingValues = await GetExistingValues(stationId);

            int loop = 0;
            int added = 0;
            foreach (SmhiValue value in values)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Cancelling after examining {loop} values, {added} was added", loop, added);
                    break;
                }

                loop++;
                if (loop % 10000 == 0)
                {
                    logger.LogInformation("Inspected {loop} values of {count}. Saving...", loop, values.Count());
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                if (!existingValues.Any(v => v.Date == value.Date && v.SmhiStationId == stationId))
                {
                    added++;
                    value.SmhiStationId = stationId;
                    await Values.InsertAsync(value);
                }
            }

            totalRequests.Labels("Add value", stationKey).Inc(added);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Saved {added} values out of {count} for station {stationid}", added, values.Count(), stationId);

            return Values.Get(v => v.SmhiStationId == stationId);
        }

        private Task<IEnumerable<SmhiValue>> GetExistingValues(int stationId)
            => Task.FromResult(Values.Get(v => v.SmhiStationId == stationId));
        #endregion Internal support routines
    }
}
