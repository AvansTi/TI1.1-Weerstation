using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Domain;
using Shared.DomainServices;
using Shared.Protos;
using gRPCServer.Repos;
using AutoMapper;
using AutoMapper.Execution;

namespace Server.Services
{
    public class WeatherDataService : WeatherData.WeatherDataBase
    {
        private readonly WeatherStationContext _dbContext;
        public WeatherDataService(WeatherStationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task<SavedReply> SaveWeatherData(ProtoWeatherData request, ServerCallContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = new Mapper(config);

            List<WeatherDataPoint> weatherDataMapped = mapper.Map<List<WeatherDataPoint>>(request.WeatherDataPoints);
            _dbContext.AddRange(weatherDataMapped);
            _dbContext.SaveChanges();

            return Task.FromResult(new SavedReply
            {
                Message = request.WeatherDataPoints.Count + " Data points are saved"
            });
        }

        public override Task<ProtoWeatherData> GetWeatherData(WeatherDataRequest request, ServerCallContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = new Mapper(config);

            DateTime dateTime = DateTime.Today;

            switch (request.Timeunit)
            {
                case "year":
                    dateTime.AddYears(0 - request.TimeAmount);
                    break;
                case "month":
                    dateTime.AddMonths(0 - request.TimeAmount);
                    break;
                default:
                    dateTime.AddDays(0 - request.TimeAmount);
                    break;
            }

            var weatherDatapoints = (from weatherdatapoint in _dbContext.WeatherDataPoint
                                    where weatherdatapoint.Timestamp > dateTime
                                    select weatherdatapoint);

            ProtoWeatherData protoWeatherData = new ProtoWeatherData();
            protoWeatherData.WeatherDataPoints.AddRange(mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints));

            return Task.FromResult(protoWeatherData);
        }
    }
}
