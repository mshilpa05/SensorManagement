using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;
        private readonly ILogger _logger;
        public SensorController(ISensorService sensorService, ILogger<SensorController> logger)
        {
            _sensorService = sensorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSensors()
        {
            try
            {
                var sensorDtos = await _sensorService.GetAllSensorsAsync();

                _logger.LogInformation("Request handled successfully.");
                return Ok(sensorDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the request.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
