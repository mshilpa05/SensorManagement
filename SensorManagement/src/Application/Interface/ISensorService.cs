using Application.DTO;

namespace Application.Interface
{
    public interface ISensorService
    {
        Task<IEnumerable<SensorViewDTO>?> GetAllSensorsAsync();
        Task<SensorViewDTO?> GetSensorByIdAsync(Guid id);
        Task<Guid> CreateSensorAsync(SensorDTO sensorCreateDTO);
        Task<bool> UpdateSensorAsync(Guid id, SensorDTO sensorUpdateDTO);
        Task<bool> DeleteSensorAsync(Guid id);

    }
}
