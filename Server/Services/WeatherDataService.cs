using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
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
            try
            {
                List<WeatherDataPoint> weatherDataMapped =
                    mapper.Map<List<WeatherDataPoint>>(request.WeatherDataPoints);
                _dbContext.AddRange(weatherDataMapped);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return Task.FromResult(new SavedReply
                {
                    Message = "An error occurred",
                    StatusCode = 500
                });
            }
            return Task.FromResult(new SavedReply
            {
                Message = request.WeatherDataPoints.Count + " Data points are saved",
                StatusCode = 200
            });
        }

        public override Task<ProtoWeatherDataResponse> GetWeatherData(WeatherDataRequest request, ServerCallContext context)
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

            List<ProtoWeatherDataPoint> protoWeatherDataPoints = null;
            try
            {
                var weatherDatapoints = (from weatherdatapoint in _dbContext.WeatherDataPoint
                    where weatherdatapoint.Timestamp > dateTime
                    select weatherdatapoint);
                protoWeatherDataPoints = mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Error with lookup or mapping of request: ",e);
                return Task.FromResult(new ProtoWeatherDataResponse
                {
                    StatusCode = 500
                });
            }
            ProtoWeatherDataResponse protoWeatherDataResponse = new ProtoWeatherDataResponse();
            protoWeatherDataResponse.WeatherDataPoints.AddRange(protoWeatherDataPoints);
            protoWeatherDataResponse.StatusCode = 200;

            return Task.FromResult(protoWeatherDataResponse);
        }
    }
}
