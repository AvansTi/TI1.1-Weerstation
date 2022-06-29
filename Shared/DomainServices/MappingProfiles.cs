using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Shared;
using Shared.Protos;
using Shared.Domain;

namespace Shared.DomainServices
{
    using AutoMapper;
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherDataPoint, ProtoWeatherDataPoint>()
                .ForMember(x => x.Timestamp, opt => opt.MapFrom<TimestampResolver>());
            CreateMap<ProtoWeatherDataPoint, WeatherDataPoint>()
                .ForMember(x => x.Timestamp, opt => opt.MapFrom<DateTimeResolver>());
            CreateMap<ProtoWeatherStation, WeatherStation>().ReverseMap();
        }
    }
    public class TimestampResolver : AutoMapper.IValueResolver<WeatherDataPoint, ProtoWeatherDataPoint, Timestamp>
    {
        public Timestamp Resolve(WeatherDataPoint source, ProtoWeatherDataPoint destination, Timestamp member, ResolutionContext context)
        {
            return Timestamp.FromDateTime(source.Timestamp.ToUniversalTime());
        }
    }
    public class DateTimeResolver : AutoMapper.IValueResolver<ProtoWeatherDataPoint, WeatherDataPoint, DateTime>
    {
        public DateTime Resolve(ProtoWeatherDataPoint source, WeatherDataPoint destination, DateTime member, ResolutionContext context)
        {
            //return source.Timestamp.ToDateTime().ToLocalTime();
            
            return source.Timestamp.ToDateTime();
            
        }
    }
}