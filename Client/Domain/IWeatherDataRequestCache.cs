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
        IEnumerable<ProtoWeatherDataPoint> GetAll();
        void Add(ProtoWeatherDataPoint weatherData);
        int GetCount();
    }
}
