using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Execution;
using Client.Domain;
using Client.DomainServices;
using Client.Protos;
using Grpc.Net.Client;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = new Mapper(config);
            List<WeatherDataPoint> points = new List<WeatherDataPoint>();
            points.Add(
                new WeatherDataPoint(){ 
                    AvgWindSpeed = 15,
                    Barometer = 18,
                    BattLevel = 22,
                    InsideHum = 49,
                    InsideTemp = 89,
                    OutsideHum = 3948,
                    OutsideTemp = 733948,
                    RainRate = 892,
                    SolarRad = 1289,
                    Station = new WeatherStation 
                    {
                        Name = "Het TI weerstation", 
                        Description = "Dit is het weerstation van Avans" ,
                        Location = "Breda",
                    },
                    Sunrise = 3487,
                    Sunset = 3984,
                    Timestamp = DateTime.Now,
                    Ts = 21948,
                    UvLevel = 214,
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
                    Station = new WeatherStation
                    {
                        Name = "Het stan weerstation",
                        Description = "Dit is het weerstation van Stan",
                        Location = "Gorinchem",
                    },
                    Sunrise = 3487,
                    Sunset = 3984,
                    Timestamp = DateTime.Now,
                    Ts = 21948,
                    UvLevel = 214,
                    WindDir = 123,
                    WindSpeed = 14,
                    XmitBatt = 144,

                }
            );
            var protoWeatherData = new ProtoWeatherData();
                protoWeatherData.WeatherDataPoints.AddRange(mapper.Map<List<ProtoWeatherDataPoint>>(points));

            Console.WriteLine(protoWeatherData.WeatherDataPoints.First().Timestamp);
            Console.WriteLine(points.First().Timestamp);
            
            Console.WriteLine("Druk op een toets om te starten");
            Console.ReadLine();
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            // var client = new WeatherSaver.saveWeatherData(channel);
            // var reply = await client.SaveWeatherDataAsync(
            //     new ProtoWeatherData
            //     {
            //         WeatherDataPoints =
            //         {
            //             points
            //         }
            //     });
            // Console.WriteLine(reply.Message);
            Console.WriteLine("Druk op een toets om te stoppen");
            Console.ReadLine();
        }
    }
}
