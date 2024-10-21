using Application.Interface;
using Application.Models;
using Newtonsoft.Json;
using Domain.Entities;
using System.Net;

namespace Infrastructure.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PlatformService> _logger;

        public PlatformService(HttpClient httpClient, ILogger<PlatformService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(HttpStatusCode, PlatformApiResponse?)> GetPlatformDataAsync(string endpoint, PlatformApiParameters platformApiParameters)
        {
            try
            {
                var queryString = GetQueryString(platformApiParameters);
                var fullEndpoint = $"{endpoint}?{queryString}";

                var response = await _httpClient.GetAsync(fullEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return (response.StatusCode,JsonConvert.DeserializeObject<PlatformApiResponse>(data));
                }
                else
                {
                    _logger.LogError($"Failed to fetch data from {endpoint}: {response.StatusCode}");
                    return (response.StatusCode, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling the platform API");
                return (HttpStatusCode.InternalServerError, null);
            }
        }

        private string GetQueryString(PlatformApiParameters platformApiParameters)
        {
            var properties = from prop in platformApiParameters.GetType().GetProperties()
                             let value = prop.GetValue(platformApiParameters)
                             where value != null
                             select $"{prop.Name}={WebUtility.UrlEncode(value.ToString())}";

            return string.Join("&", properties);
        }
    }
}
