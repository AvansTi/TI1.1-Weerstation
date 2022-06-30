using System;
using System.Collections.Generic;
using System.Timers;
using AutoMapper;
using Client.Domain;
using Client.Repos;
using Client.SerialConsole;
using Grpc.Net.Client;
using Shared.Domain;
using Shared.DomainServices;
using Shared.Protos;

namespace Client;

public class Requester
{
    private const string ServerAdress = "http://localhost:5000";
    private Mapper _mapper;
    private IWeatherDataRequestCache _weatherDataRequestCache;
    private IWeatherConsoleDAO _weatherConsole;
    public Requester()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
        _mapper = new Mapper(config);
        _weatherConsole = new ConnectionManager();
        _weatherDataRequestCache = new InMemoryWeatherDataRequestCache();
    }
    public void repeatingTask()
    {
        Console.WriteLine("Request is started");
        WeatherDataPoint weatherDataPoint= _weatherConsole.Get(_mapper);
        if (_weatherDataRequestCache.GetCount() == 0)
        {
            using var channel = GrpcChannel.ForAddress(ServerAdress);
            var client = new WeatherData.WeatherDataClient(channel);
            var protoWeatherData = new ProtoWeatherData();
            protoWeatherData.WeatherDataPoints.Add(_mapper.Map<ProtoWeatherDataPoint>(weatherDataPoint));
            var reply = client.SaveWeatherDataAsync(protoWeatherData).ResponseAsync.Result;
            Console.WriteLine(reply);
        }
        else
        {
            _weatherDataRequestCache.Add(_mapper.Map<ProtoWeatherDataPoint>(weatherDataPoint));
            foreach (var request in _weatherDataRequestCache.GetAll())
            {
                using var channel = GrpcChannel.ForAddress(ServerAdress);
                var client = new WeatherData.WeatherDataClient(channel);
                var protoWeatherData = new ProtoWeatherData();
                protoWeatherData.WeatherDataPoints.AddRange(_weatherDataRequestCache.GetAll());
                var reply = client.SaveWeatherDataAsync(protoWeatherData).ResponseAsync.Result;
            }
        }
            
    }
}