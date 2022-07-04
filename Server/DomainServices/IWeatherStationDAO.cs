using Shared.Domain;

namespace Server.DomainServices;

public interface IWeatherStationDAO : IRepo<WeatherDataPoint>
{
    
}