using Api.Controllers;
using Application.DTO;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class SensorControllerTests
    {
        private readonly Mock<ISensorService> _mockSensorService;
        private readonly SensorController _controller;
        private readonly Mock<ILogger<SensorController>> _logger;
        public SensorControllerTests()
        {
            _mockSensorService = new Mock<ISensorService>();
            _logger = new Mock<ILogger<SensorController>>();
            _controller = new SensorController(_mockSensorService.Object, _logger.Object);
        }

        [Fact]
        public async Task WhenGetAllSensors_WithListOfSensors_ReturnsOk()
        {
            var sensors = new List<SensorViewDTO>
                {
                    new SensorViewDTO { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 },
                    new SensorViewDTO { Id = Guid.NewGuid(), Name = "Sensor 2", Location = "Vienna", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 }
                };
            _mockSensorService.Setup(s => s.GetAllSensorsAsync()).ReturnsAsync(sensors);

            var result = await _controller.GetAllSensors();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSensors = Assert.IsType<List<SensorViewDTO>>(okResult.Value);
            Assert.Equal(2, returnedSensors.Count);
        }

        [Fact]
        public async Task WhenGetAllSensors_WillNoSensorsInDb_ReturnsNotFound()
        {
            _mockSensorService.Setup(s => s.GetAllSensorsAsync()).ReturnsAsync(new List<SensorViewDTO>());

            var result = await _controller.GetAllSensors();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSensors = Assert.IsType<List<SensorViewDTO>>(okResult.Value);
            Assert.Empty(returnedSensors);
        }
    }
}
