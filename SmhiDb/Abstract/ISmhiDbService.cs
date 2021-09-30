using SmhiDb.Model;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WeatherContracts;

namespace SmhiDb.Abstract
{
    public interface ISmhiDbService
    {
        Task AddAsync(SmhiStation station, SmhiParameter parameter, IEnumerable<SmhiPosition> positions, IEnumerable<SmhiLink> links, IEnumerable<SmhiValue> values, CancellationToken cancellationToken);
        //Task AddAsync(AddStationRequest request);
        //Task AddAsync(AddValuesRequest request);
        Task<GetValuesByStationIdResponse> GetValuesByStationIdAsync(string id, DateTime? fromDate, DateTime? toDate);
        Task<GetStationsResponse> GetStationsAsync();
        Task<GetStationByStationKeyResponse> GetStationByStationKeyAsync(string id);
    }
}
