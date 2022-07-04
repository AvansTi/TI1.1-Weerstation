using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Domain;
using Shared.ApplicationServices;
using Shared.Protos;
using gRPCServer.Infrastructure;
using AutoMapper;
using AutoMapper.Execution;
using Server.DomainServices;

namespace Server.Services
{
    public class WeatherDataService : WeatherData.WeatherDataBase
    {
        private readonly IWeatherStationRepo _weatherStationRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public WeatherDataService(IWeatherStationRepo weatherStationRepo,IMapper mapper,ILogger logger)
        {
            _weatherStationRepo = weatherStationRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<SavedReply> SaveWeatherData(ProtoWeatherData request, ServerCallContext context)
        {
            try
            {
                //Map the proto class to domain class and add them to th repo
                List<WeatherDataPoint> weatherDataMapped =
                    _mapper.Map<List<WeatherDataPoint>>(request.WeatherDataPoints);
                await _weatherStationRepo.AddRangeAsync(weatherDataMapped);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred: {StackTrace}",e.StackTrace);
                return new SavedReply
                {
                    Message = "An error occurred",
                    StatusCode = 500
                };
            }
            return new SavedReply
            {
                Message = request.WeatherDataPoints.Count + " Data points are saved",
                StatusCode = 200
            };
        }

        public override Task<ProtoWeatherDataResponse> GetWeatherData(WeatherDataRequest request, ServerCallContext context)
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

            List<ProtoWeatherDataPoint> protoWeatherDataPoints = null;
            try
            {
                var weatherDatapoints = (from weatherdatapoint in _weatherStationRepo.GetQueryable()
                    where weatherdatapoint.Timestamp > dateTime
                    select weatherdatapoint);
                //Map response to proto classes
                protoWeatherDataPoints = _mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints);
            }
            catch(Exception e)
            {
                _logger.LogError("Error with lookup or mapping of request: {StackTraces}",e.StackTrace);
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
        public override Task<ProtoWeatherDataResponse> GetWeatherDataBetween(TimeBlock request, ServerCallContext context)
        {
            DateTime timeStart = request.TimeStart.ToDateTime();
            DateTime timeEnd = request.TimeEnd.ToDateTime();
            List<ProtoWeatherDataPoint> protoWeatherDataPoints = null;
            try
            {
                var weatherDatapoints = (from weatherdatapoint in _weatherStationRepo.GetQueryable()
                    where timeStart < weatherdatapoint.Timestamp.Date
                    && weatherdatapoint.Timestamp.Date < timeEnd.Date 
                    select weatherdatapoint);
                protoWeatherDataPoints = _mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints);
            }
            catch(Exception e)
            {
                _logger.LogError("Error with lookup or mapping of request: {StackTraces}",e.StackTrace);
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
        public override Task<ProtoWeatherDataResponse> GetWeatherDataByStation(ProtoWeatherStation request, ServerCallContext context)
        {
            var weatherDatapoints = (from weatherdatapoint in _weatherStationRepo.GetQueryable()
                where weatherdatapoint.Station.StationId == request.StationId
                select weatherdatapoint);

            ProtoWeatherDataResponse protoWeatherDataResponse = new ProtoWeatherDataResponse();
            protoWeatherDataResponse.WeatherDataPoints.AddRange(_mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints));

            return Task.FromResult(protoWeatherDataResponse);
        }
    }
}
