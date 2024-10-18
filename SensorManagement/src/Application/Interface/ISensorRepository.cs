using Domain.Entities;

namespace Application.Interface
{
    public interface ISensorRepository
    {
        Task<IEnumerable<Sensor>> GetAllSensorsAsync();
    }
}
