using Api.Models;
using Application.Interface;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Api.Controllers
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
            try
            {
                var (responseCode, result) = await _platformService.GetPlatformDataAsync(_endpoint, platformApiParameters);
                // Todo remove dependency between api and infra layer
                if(responseCode == HttpStatusCode.OK)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)responseCode);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the request.");
                return StatusCode(500, "An internal server error occurred.");
            }

        }
    }
}
