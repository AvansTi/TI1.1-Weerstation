using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCServer
{
    public class WeatherSaverService : WeatherSaver.WeatherSaverBase
    {
        public WeatherSaverService()
        {
        }

        public override Task<SavedReply> SaveWeatherData(WeatherData request, ServerCallContext context)
        {
            return Task.FromResult(new SavedReply
            {
                Message = request.WeatherDataPoints.Count + " Data points are saved"
            });
        }
    }
}
