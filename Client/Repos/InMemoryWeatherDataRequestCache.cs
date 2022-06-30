using System.Collections.Generic;
using Client.Domain;
using Shared.Protos;

namespace Client.Repos;

public class InMemoryWeatherDataRequestCache : IWeatherDataRequestCache
{
    private List<ProtoWeatherData> cache;
    public void Add(ProtoWeatherData weatherdata)
    {
        cache.Add(weatherdata);
    }

    public List<ProtoWeatherData> GetAll()
    {
        return cache;
    }
}