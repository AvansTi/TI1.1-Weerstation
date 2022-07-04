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
        private Mapper _mapper;
        public WeatherDataService(WeatherStationContext dbContext)
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>()));
            _dbContext = dbContext;
        }

        public override Task<SavedReply> SaveWeatherData(ProtoWeatherData request, ServerCallContext context)
        {
           
            List<WeatherDataPoint> weatherDataMapped = _mapper.Map<List<WeatherDataPoint>>(request.WeatherDataPoints);
            _dbContext.AddRange(weatherDataMapped);
            _dbContext.SaveChanges();

            return Task.FromResult(new SavedReply
            {
                Message = request.WeatherDataPoints.Count + " Data points are saved"
            });
        }

        public override Task<ProtoWeatherData> GetWeatherData(WeatherDataRequest request, ServerCallContext context)
        {
            DateTime dateTime = DateTime.Today;

            switch (request.Timeunit)
            {
                case "year":
                    dateTime = dateTime.AddYears(0 - request.TimeAmount);
                    break;
                case "month":
                    dateTime = dateTime.AddMonths(0 - request.TimeAmount);
                    break;
                case "day":
                    dateTime = dateTime.AddDays(0 - request.TimeAmount);
                    break;
                default:
                    dateTime = dateTime.AddHours(0 - request.TimeAmount);
                    break;
            }

            var weatherDatapoints = (from weatherdatapoint in _dbContext.WeatherDataPoint
                                    where weatherdatapoint.Timestamp > dateTime
                                    select weatherdatapoint);

            ProtoWeatherData protoWeatherData = new ProtoWeatherData();
            protoWeatherData.WeatherDataPoints.AddRange(_mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints));

            return Task.FromResult(protoWeatherData);
        }

        public override Task<ProtoWeatherData> GetLastDataPoint(WeatherDataRequest request, ServerCallContext context)
        {
            List<WeatherDataPoint> weatherDatapoints = new List<WeatherDataPoint>();

            var weatherDatapoint = _dbContext.WeatherDataPoint.OrderByDescending(x => x.Timestamp).First();
            weatherDatapoints.Add(weatherDatapoint);

            ProtoWeatherData protoWeatherData = new ProtoWeatherData();
            protoWeatherData.WeatherDataPoints.AddRange(_mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints));

            return Task.FromResult(protoWeatherData);
        }

        public override Task<ProtoWeatherData> GetWeatherDataBetween(TimeBlock request, ServerCallContext context)
        {
            var weatherDatapoints = (from weatherdatapoint in _dbContext.WeatherDataPoint
                                     where weatherdatapoint.Timestamp > request.TimeStart.ToDateTime() && weatherdatapoint.Timestamp < request.TimeEnd.ToDateTime()
                                     select weatherdatapoint);

            ProtoWeatherData protoWeatherData = new ProtoWeatherData();
            protoWeatherData.WeatherDataPoints.AddRange(_mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints));

            return Task.FromResult(protoWeatherData);
        }

        public override Task<ProtoWeatherData> GetWeatherDataByStation(ProtoWeatherStation request, ServerCallContext context)
        {
            var weatherDatapoints = (from weatherdatapoint in _dbContext.WeatherDataPoint
                                     where weatherdatapoint.Station.StationId == request.StationId
                                     select weatherdatapoint);

            ProtoWeatherData protoWeatherData = new ProtoWeatherData();
            protoWeatherData.WeatherDataPoints.AddRange(_mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints));

            return Task.FromResult(protoWeatherData);
        }
    }
}
