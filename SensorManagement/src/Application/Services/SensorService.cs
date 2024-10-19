using Application.DTO;
using Application.Interface;
using AutoMapper;
using Domain.Entities;

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

        public async Task<SensorViewDTO?> GetSensorByIdAsync(Guid id)
        {
            var sensor = await _sensorRepository.GetSensorByIdAsync(id);

            if(sensor == null)
            {
                return null;
            }

            return _mapper.Map<SensorViewDTO>(sensor);
        }

        public async Task<Guid> CreateSensorAsync(SensorDTO sensorCreateDTO)
        {
            var sensor = _mapper.Map<Sensor>(sensorCreateDTO);
            sensor.Id = Guid.NewGuid();

            await _sensorRepository.CreateSensorAsync(sensor);

            return sensor.Id;
        }

        public async Task<bool> UpdateSensorAsync(Guid id, SensorDTO sensorUpdateDTO)
        {
            var sensor = await _sensorRepository.GetSensorByIdAsync(id);
            if (sensor == null) return false;

            sensor.Update(sensorUpdateDTO.Name, sensorUpdateDTO.Location, sensorUpdateDTO.UpperWarning, sensorUpdateDTO.LowerWarning);

            await _sensorRepository.UpdateSensorAsync(sensor);

            return true;
        }

        public async Task<bool> DeleteSensorAsync(Guid id)
        {
            var sensor = await _sensorRepository.GetSensorByIdAsync(id);

            if (sensor == null)
            {
                return false;
            }

            await _sensorRepository.DeleteSensorAsync(id);
            return true;
        }
    }
}
