using Application.Models;
using Domain.Entities;
using System.Net;

namespace Application.Interface
{
    public interface IPlatformService
    {
        Task<(HttpStatusCode,PlatformApiResponse?)> GetPlatformDataAsync(string endpoint, PlatformApiParameters platformApiParameters);
    }
}
