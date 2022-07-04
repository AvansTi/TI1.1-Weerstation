using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using AutoMapper;
using Client.DomainServices;
using Client.Infrastructure;
using Client.SerialConsole;
using Grpc.Net.Client;
using Shared.Domain;
using Shared.ApplicationServices;
using Shared.Protos;

namespace Client;

public class Requester
{
    public static readonly string ServerAddress = Environment.GetEnvironmentVariable("SERVER_URL");
    private readonly Mapper _mapper;
    private readonly IWeatherDataRequestCache _weatherDataRequestCache;
    private readonly IWeatherConsoleDAO _weatherConsole;
    private readonly GrpcChannel _grpcChannel;

    public Requester()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
        _mapper = new Mapper(config);
        _weatherConsole = new ConnectionManager();
        _weatherDataRequestCache = new InMemoryWeatherDataRequestCache();
        _grpcChannel = GrpcChannel.ForAddress(ServerAddress);
    }

    public void ReadConsoleAndSendRequest()
    {
        WeatherDataPoint weatherDataPoint = null;
        //Tries to get data from weather console. If it fails, it aborts the method and tries again when the method is started again by the timer.
        try
        {
            weatherDataPoint = _weatherConsole.Get(_mapper);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Something went wrong while reading the console! Aborting method.");
            return;
        }
        //Set up grpc connection
        var client = new WeatherData.WeatherDataClient(_grpcChannel);
        var protoWeatherData = new ProtoWeatherData();

        //Adds the retrieved weatherdatapoint to the weatherdatapoints "list"
        protoWeatherData.WeatherDataPoints.Add(_mapper.Map<ProtoWeatherDataPoint>(weatherDataPoint));

        //If there is something in the cache, add it to the request
        if (_weatherDataRequestCache.GetCount() != 0)
        {
            protoWeatherData.WeatherDataPoints.AddRange(_weatherDataRequestCache.GetAll());
        }
        //Try to send the weatherDataPoints, if it fails, add the retrieved datapoint to the cache and wait for the next method call
        try
        {
            var reply = client.SaveWeatherDataAsync(protoWeatherData).ResponseAsync.Result;
            Console.WriteLine(reply);
            if (reply.StatusCode != 200)
            {
                throw new InvalidOperationException($"Statuscode {reply.StatusCode}");
            }
        }
        //Return on error, when the request is successful, clear the cache
        catch (Exception e)
        {
            Console.Error.WriteLine("Error while requesting data, adding request to cache: ", e);
            _weatherDataRequestCache.Add(protoWeatherData.WeatherDataPoints[0]);
            return;
        }
        _weatherDataRequestCache.Clear();
    }
}