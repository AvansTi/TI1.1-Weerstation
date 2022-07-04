using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.DomainServices;
using Shared.Domain;

namespace gRPCServer.Infrastructure;

public class DbWeatherdataRepo : IWeatherStationDAO
{
    protected WeatherStationContext _context { get; set; }

    public DbWeatherdataRepo(WeatherStationContext weatherStationContext) {
        _context = weatherStationContext;
    }

    public async Task<IEnumerable<WeatherDataPoint>> GetAllAsync()
    {
        return await _context.WeatherDataPoint.ToListAsync();
    }

    public void Add(WeatherDataPoint item) {
        _context.Add(item);
        _context.SaveChanges();
    }
    public async Task AddRangeAsync(IEnumerable<WeatherDataPoint> item) {
        await _context.AddRangeAsync(item);
        await _context.SaveChangesAsync();
    }

    public int Count()
    {
        return _context.WeatherDataPoint.Count();
    }

    public void Remove(WeatherDataPoint item) {
        _context.Remove(item);
        _context.SaveChanges();
    }

    public void Update(WeatherDataPoint item) {
        _context.Update(item);
        _context.SaveChanges();
    }

    public IQueryable<WeatherDataPoint> GetQueryable()
    {
        return _context.WeatherDataPoint;
    }

    public WeatherDataPoint GetWhereId(int id)
    {
        return _context.WeatherDataPoint
            .SingleOrDefault(x => x.StationId == id);
    }
}