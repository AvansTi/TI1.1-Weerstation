using Grpc.Net.Client;
using System;
using gRPCClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace gRPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<WeatherDataPoint> points = new List<WeatherDataPoint>();
            points.Add(new WeatherDataPoint { NameStation = "StansWeerstation", Rain = 13.55, Temp = 14.55, Windspeed = 12.14 });
            points.Add(new WeatherDataPoint { NameStation = "StansWeerstation", Rain = 14.55, Temp = 17.99, Windspeed = 19.88 });
            Console.WriteLine("Druk op een toets om te starten");
            Console.ReadLine();
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
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
