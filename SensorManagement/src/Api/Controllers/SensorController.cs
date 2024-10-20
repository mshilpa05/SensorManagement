using Application.DTO;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpGet("id")]
        public async Task<IActionResult> GetSensorById(Guid id)
        {
            try
            {
                var sensorDtos = await _sensorService.GetSensorByIdAsync(id);

                _logger.LogInformation("Request handled successfully.");
                return Ok(sensorDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the request.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensor(SensorDTO sensorCreateDTO)
        {
            try
            {
                if (sensorCreateDTO.UpperWarning < sensorCreateDTO.LowerWarning)
                {
                    return BadRequest("UpperWarning cannot be lower than LowerWarning");
                }

                var sensorId = await _sensorService.CreateSensorAsync(sensorCreateDTO);

                return Created(nameof(GetSensorById), new
                {
                    Message = "Resource created successfully.",
                    Data = sensorId.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the request.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensor(Guid id, SensorDTO sensorUpdateDTO)
        {
            try
            {
                if(sensorUpdateDTO.UpperWarning < sensorUpdateDTO.LowerWarning)
                {
                    return BadRequest("UpperWarning cannot be lower than LowerWarning");
                }

                var updated = await _sensorService.UpdateSensorAsync(id, sensorUpdateDTO);

                if (updated)
                {
                    return Ok("Resource updated successfully." );
                }

                return NotFound("Resource not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the request.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(Guid id)
        {
            try
            {
                var isDeleted = await _sensorService.DeleteSensorAsync(id);

                if (isDeleted)
                {
                    return NoContent();
                }

                return NotFound("Resource not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the request.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
