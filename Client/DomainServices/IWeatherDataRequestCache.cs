using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain;
using Shared.Protos;

namespace Client.DomainServices
{
    /// <summary>
    /// A data cache for ProtoWeatherDataPoint classes
    /// </summary>
    public interface IWeatherDataRequestCache
    {
        IEnumerable<ProtoWeatherDataPoint> GetAll();
        void Add(ProtoWeatherDataPoint weatherData);
        int GetCount();
        void Clear();
    }
}
