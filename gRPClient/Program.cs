using Grpc.Net.Client;
using System;
using gRPCClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;

namespace gRPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<WeatherDataPoint> points = new List<WeatherDataPoint>();
            points.Add(
                new WeatherDataPoint 
                { 
                    AvgWindSpeed = 15,
                    Barometer = 18,
                    BattLevel = 22,
                    InsideHum = 49,
                    InsideTemp = 89,
                    OutsideHum = 3948,
                    OutsideTemp = 733948,
                    RainRate = 892,
                    SolarRad = 1289,
                    StationName = "Het echter weer station",
                    Sunrise = 3487,
                    Sunset = 3984,
                    Timestamp = DateTimeOffset.Now.ToTimestamp(),
                    Ts = 21948,
                    UVLevel = 214,
                    WindDir = 123,
                    WindSpeed = 14,
                    XmitBatt = 144,

                }
            );
            points.Add(
                new WeatherDataPoint
                {
                    AvgWindSpeed = 15,
                    Barometer = 18,
                    BattLevel = 22,
                    InsideHum = 49,
                    InsideTemp = 864,
                    OutsideHum = 3948,
                    OutsideTemp = 733948,
                    RainRate = 892,
                    SolarRad = 1289,
                    StationName = "Het 2e echter weer station",
                    Sunrise = 3487,
                    Sunset = 3984,
                    Timestamp = DateTimeOffset.Now.ToTimestamp(),
                    Ts = 21948,
                    UVLevel = 214,
                    WindDir = 123,
                    WindSpeed = 14,
                    XmitBatt = 144,

                }
            );
            Console.WriteLine("Druk op een toets om te starten");
            Console.ReadLine();
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new WeatherSaver.WeatherSaverClient(channel);
            var reply = await client.SaveWeatherDataAsync(
                new WeatherData
                {
                    WeatherDataPoints =
                    {
                        points
                    }
                });
            Console.WriteLine(reply.Message);
            Console.WriteLine("Druk op een toets om te stoppen");
            Console.ReadLine();
        }
    }
}
