using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class SensorMappingProfile : Profile
    {
        public SensorMappingProfile()
        {
            CreateMap<Sensor, SensorViewDTO>();
            CreateMap<SensorDTO, Sensor>();
        }
    }
}
