using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Domain;

namespace Client.Repos
{
    public interface IWeatherConsoleDAO
    {
        List<WeatherDataPoint> Get();
    }
}
