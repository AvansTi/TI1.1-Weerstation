using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain;
using Shared.Protos;

namespace Client.Domain
{
    public interface IWeatherDataRequestCache
    {
        void Add(List<ProtoWeatherData> weatherdata);
        List<ProtoWeatherData> GetAll();
        void Add(ProtoWeatherData weatherData);
    }
}
