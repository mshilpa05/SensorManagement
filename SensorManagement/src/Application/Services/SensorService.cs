using Application.DTO;
using Application.Interface;
using AutoMapper;

namespace Application.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;
        public SensorService(ISensorRepository sensorRepository, IMapper mapper)
        {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SensorViewDTO>?> GetAllSensorsAsync()
        {
            var sensors = await _sensorRepository.GetAllSensorsAsync();

            if(sensors == null)
            { 
                return null;
            }

            return sensors.Select(sensor => _mapper.Map<SensorViewDTO>(sensor));
        }
    }
}
