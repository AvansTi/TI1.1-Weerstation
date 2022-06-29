using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain;

namespace Client.Repos
{
    public interface IWeatherConsoleDAO
    {
        List<WeatherDataPoint> Get();
    }
}
