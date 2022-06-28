using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gRPClient.Domain;

namespace gRPClient.Repos
{
    public interface IWeatherCLientDAO
    {
        void SendWeatherData(List<WeatherDataPoint> weatherdata);
    }
}
