using System.Collections.Generic;
using Client.Domain;
using Shared.Protos;

namespace Client.Repos;

public class InMemoryWeatherDataRequestCache : IWeatherDataRequestCache
{
    private List<ProtoWeatherDataPoint> cache;
    public void Add(ProtoWeatherDataPoint weatherdata)
    {
        cache.Add(weatherdata);
    }

    public IEnumerable<ProtoWeatherDataPoint> GetAll()
    {
        return cache;
    }

    public int GetCount()
    {
        return cache.Count;
    }
}