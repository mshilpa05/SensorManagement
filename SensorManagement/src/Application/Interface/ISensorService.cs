using Application.DTO;

namespace Application.Interface
{
    public interface ISensorService
    {
        Task<IEnumerable<SensorViewDTO>?> GetAllSensorsAsync();
    }
}
