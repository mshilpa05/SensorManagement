using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace SensorManagement.src.Application.Mappings
{
    public class SensorMappingProfile : Profile
    {
        public SensorMappingProfile()
        {
            CreateMap<Sensor, SensorViewDTO>();
        }
    }
}
