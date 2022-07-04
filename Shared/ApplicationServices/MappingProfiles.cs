using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Shared;
using Shared.Protos;
using Shared.Domain;
using Shared.SerialConsole;


namespace Shared.ApplicationServices
{
    using AutoMapper;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherDataPoint, ProtoWeatherDataPoint>()
                .ForMember(dest => dest.Timestamp,
                    opt => opt.MapFrom(src => Timestamp.FromDateTime(src.Timestamp.ToUniversalTime())));
            CreateMap<ProtoWeatherDataPoint, WeatherDataPoint>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp.ToDateTime()));
            CreateMap<ProtoWeatherStation, WeatherStation>()
                .ReverseMap();
            CreateMap<WeatherStationDataStruct, WeatherDataPoint>()
                .ForMember(dest => dest.Timestamp, opts => opts.MapFrom(src => DateTime.UtcNow));
            CreateMap<WeatherStationDataStruct, ProtoWeatherDataPoint>()
                .ForMember(dest => dest.Timestamp,
                    opts => opts.MapFrom(src => Timestamp.FromDateTime(DateTime.UtcNow)));
        }
    }
}