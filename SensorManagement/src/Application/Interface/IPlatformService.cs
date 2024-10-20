using Application.DTO;
using Application.Models;

namespace Application.Interface
{
    public interface IPlatformService
    {
        Task<PlatformApiResponseDTO?> GetPlatformDataAsync(string endpoint, PlatformApiParameters platformApiParameters);
    }
}
