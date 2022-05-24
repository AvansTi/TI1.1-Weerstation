using gRPCClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRPClient.Repo_s
{
    public interface IWeatherConsoleDAO
    {
        List<WeatherDataPoint> Get();
    }
}
