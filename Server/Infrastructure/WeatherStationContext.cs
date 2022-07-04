//using gRPCServer.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCServer.Infrastructure
{
    public class WeatherStationContext : DbContext
    {
        public WeatherStationContext(DbContextOptions<WeatherStationContext> options) : base(options)
        {

        }
        public DbSet<WeatherDataPoint> WeatherDataPoint { get; set; }
    }
}
