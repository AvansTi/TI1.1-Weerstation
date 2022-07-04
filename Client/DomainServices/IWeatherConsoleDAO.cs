using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shared.Domain;

namespace Client.DomainServices
{
    public interface IWeatherConsoleDAO
    {
        WeatherDataPoint Get(Mapper mapper);
    }
}
