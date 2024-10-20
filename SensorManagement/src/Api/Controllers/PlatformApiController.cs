using Api.Models;
using Application.Interface;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SensorManagement.src.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformApiController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly string _endpoint;
        private readonly ILogger<PlatformApiController> _logger;

        public PlatformApiController(IPlatformService platformService,
                                    IOptions<PlatformApiConfig> platformApiConfig,
                                    ILogger<PlatformApiController> logger)
        {
            _platformService = platformService;
            _endpoint = platformApiConfig.Value.Endpoint;
            _logger = logger;   
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetPlatformData([FromQuery]PlatformApiParameters platformApiParameters)
        {
            var result = await _platformService.GetPlatformDataAsync(_endpoint, platformApiParameters);
            // Todo remove dependency between api and infra layer

            if (result == null)
            {
                _logger.LogError("Failed to fetch data from external API");
                return StatusCode(500, "Failed to fetch data from external API");
            }

            return Ok(result);
        }
    }
}
