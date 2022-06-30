using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using AutoMapper.Execution;
using Client.SerialConsole;
using Shared.Domain;
using Shared.DomainServices;
using Shared.Protos;
using Grpc.Net.Client;
using Shared.SerialConsole;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = new Mapper(config);
            // structDemo(mapper);
            //grpcDemo(mapper);
            
        }

        public static void structDemo(Mapper mapper)
        {
            WeatherStationDataStruct dataStruct = new WeatherStationDataStruct
            {
                c1 = 'L',
                c2 = 'O',
                c3 = 'O',
                c4 = 'P',
                PacketType = 0,
                NextRec = 0,
                Barometer = 5,
                InsideTemp = 200,
                InsideHum = 30,
                OutsideTemp = 300,
                WindSpeed = 30,
                AvgWindSpeed = 20,
                WindDir = 90,
                OutsideHum = 40,
                RainRate = 10,
                SolarRad = 5,
                XmitBatt = 1,
                BattLevel = 90,
                Sunrise = 40,
                Sunset = 140
            };
            ProtoWeatherDataPoint dataPoint = mapper.Map<ProtoWeatherDataPoint>(dataStruct);
            Console.WriteLine(dataPoint);
        }

        public static void grpcDemo(Mapper mapper)
        {
            List<WeatherDataPoint> points = new List<WeatherDataPoint>();
            points.Add(
                new WeatherDataPoint(){ 
                    AvgWindSpeed = 15,
                    Barometer = 18,
                    BattLevel = 22,
                    InsideHum = 49,
                    InsideTemp = 89,
                    OutsideHum = 3948,
                    OutsideTemp = 60,
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
                    OutsideTemp = 60,
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
            var client = new WeatherData.WeatherDataClient(channel);
            //var reply = await client.SaveWeatherDataAsync(protoWeatherData);
            var data = client.GetWeatherData(new WeatherDataRequest { Timeunit = "day", TimeAmount = 6}); 
            foreach(var point in data.WeatherDataPoints)
            {
                Console.WriteLine(point.WindSpeed);
            }
            Console.WriteLine("Druk op een toets om te stoppen");
            Console.ReadLine();
        }
    }
}
