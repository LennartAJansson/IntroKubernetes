using CsvHelper;
using CsvHelper.Configuration;

using SmhiApi.Model;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmhiApi.Serializers
{
    public static class SmhiCsvSerializer
    {
        private static List<CsvStation> CsvStations { get; set; } = new();
        private static List<CsvParameter> CsvParameters { get; set; } = new();
        private static List<CsvPosition> CsvPositions { get; set; } = new();
        private static List<CsvValue> CsvValues { get; set; } = new();

        public static Task<CsvObservations> DeserializeAsync(string filename)
        {
            CsvStations.Clear();
            CsvParameters.Clear();
            CsvPositions.Clear();
            CsvValues.Clear();

            //TODO! Break apart responsibility for reading a file
            using (StreamReader reader = new(filename))
            using (CsvReader csv = new(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = false,
                Delimiter = ";"
            }))
            {
                csv.Context.RegisterClassMap<CsvStationMap>();
                csv.Context.RegisterClassMap<CsvParameterMap>();
                csv.Context.RegisterClassMap<CsvPositionMap>();
                csv.Context.RegisterClassMap<CsvValueMap>();

                bool isHeader = true;
                while (csv.Read())
                {
                    if (isHeader)
                    {
                        csv.ReadHeader();
                        isHeader = false;
                        continue;
                    }

                    if (string.IsNullOrEmpty(csv.GetField(0)))
                    {
                        isHeader = true;
                        continue;
                    }

                    switch (csv.HeaderRecord[0])
                    {
                        case CsvStation.Header:
                            CsvStations.Add(csv.GetRecord<CsvStation>());
                            break;
                        case CsvParameter.Header:
                            CsvParameters.Add(csv.GetRecord<CsvParameter>());
                            break;
                        case CsvPosition.Header:
                            CsvPositions.Add(csv.GetRecord<CsvPosition>());
                            break;
                        case CsvValue.Header:
                            CsvValues.Add(csv.GetRecord<CsvValue>());
                            break;
                        default:
                            throw new InvalidOperationException("Invalid row found in CSV data. Unknown record type.");
                    }
                }
            }

            return Task.FromResult(new CsvObservations()
            {
                Station = CsvStations.FirstOrDefault(),
                Parameter = CsvParameters.FirstOrDefault(),
                Positions = CsvPositions,
                Values = CsvValues.OrderBy(v => v.Date).OrderBy(v => v.Time)
            });
        }
    }
}
