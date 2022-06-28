using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Domain;
using Server.Protos;

namespace Server.Services
{
    public class WeatherSaverService : WeatherSaver.WeatherSaverBase
    {
        public WeatherSaverService()
        {
        }

        public override Task<SavedReply> SaveWeatherData(ProtoWeatherData request, ServerCallContext context)
        {
            foreach(ProtoWeatherDataPoint datapoint in request.WeatherDataPoints){
                Console.WriteLine(datapoint.ToString());
            }
            return Task.FromResult(new SavedReply
            {
                Message = request.WeatherDataPoints.Count + " Data points are saved"
            });
        }
    }
}
