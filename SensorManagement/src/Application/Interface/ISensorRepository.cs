using Domain.Entities;

namespace Application.Interface
{
    public interface ISensorRepository
    {
        Task<IEnumerable<Sensor>?> GetAllSensorsAsync();
        Task<Sensor?> GetSensorByIdAsync(Guid id);
        Task CreateSensorAsync(Sensor sensor);
        Task UpdateSensorAsync(Sensor sensor);
        Task DeleteSensorAsync(Guid id);
    }
}
