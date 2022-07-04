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
using gRPCServer.Infrastructure;
using AutoMapper;
using AutoMapper.Execution;
using Server.DomainServices;

namespace Server.Services
{
    public class WeatherDataService : WeatherData.WeatherDataBase
    {
        private readonly IWeatherStationDAO _weatherStationDao;
        private readonly IMapper _mapper;
        public WeatherDataService(IWeatherStationDAO weatherStationDao,IMapper mapper)
        {
            _weatherStationDao = weatherStationDao;
            _mapper = mapper;
        }

        public override async Task<SavedReply> SaveWeatherData(ProtoWeatherData request, ServerCallContext context)
        {
            try
            {
                List<WeatherDataPoint> weatherDataMapped =
                    _mapper.Map<List<WeatherDataPoint>>(request.WeatherDataPoints);
                await _weatherStationDao.AddRangeAsync(weatherDataMapped);
            }
            catch (Exception e)
            {
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
            switch (request.TimeUnit)
            {
                case WeatherDataRequest.Types.TIME_UNIT.Year:
                    dateTime.AddYears(0 - request.TimeAmount);
                    break;
                case WeatherDataRequest.Types.TIME_UNIT.Month:
                    dateTime.AddMonths(0 - request.TimeAmount);
                    break;
                case WeatherDataRequest.Types.TIME_UNIT.Day:
                    dateTime.AddDays(0 - request.TimeAmount);
                    break;
            }

            List<ProtoWeatherDataPoint> protoWeatherDataPoints = null;
            try
            {
                var weatherDatapoints = (from weatherdatapoint in _weatherStationDao.GetQueryable()
                    where weatherdatapoint.Timestamp > dateTime
                    select weatherdatapoint);
                protoWeatherDataPoints = _mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints);
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
        public override Task<ProtoWeatherDataResponse> GetWeatherDataBetween(WeatherDataBetweenDatesRequest request, ServerCallContext context)
        {
            DateTime dateFrom = request.From.ToDateTime();
            DateTime dateUpTo = request.UpTo.ToDateTime();
            List<ProtoWeatherDataPoint> protoWeatherDataPoints = null;
            try
            {
                var weatherDatapoints = (from weatherdatapoint in _weatherStationDao.GetQueryable()
                    where dateFrom < weatherdatapoint.Timestamp.Date
                    && weatherdatapoint.Timestamp.Date < dateUpTo.Date 
                    select weatherdatapoint);
                protoWeatherDataPoints = _mapper.Map<List<ProtoWeatherDataPoint>>(weatherDatapoints);
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
