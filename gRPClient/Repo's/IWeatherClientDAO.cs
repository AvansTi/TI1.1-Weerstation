using gRPCClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRPClient.Repo_s
{
    public interface IWeatherCLientDAO
    {
        void SendWeatherData(List<WeatherDataPoint> weatherdata);
    }
}
