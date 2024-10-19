using Api.Controllers;
using Application.DTO;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests
{
    public class SensorControllerTests
    {
        private readonly Mock<ISensorService> _mockSensorService;
        private readonly SensorController _controller;
        private readonly Mock<ILogger<SensorController>> _mockLogger;
        public SensorControllerTests()
        {
            _mockSensorService = new Mock<ISensorService>();
            _mockLogger = new Mock<ILogger<SensorController>>();
            _controller = new SensorController(_mockSensorService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GivenListOfSensors_OnGetAllSensors__ReturnsOk()
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
        public async Task GivenNoSensorsInDb_OnGetAllSensors_ReturnsNotFound()
        {
            _mockSensorService.Setup(s => s.GetAllSensorsAsync()).ReturnsAsync(new List<SensorViewDTO>());

            var result = await _controller.GetAllSensors();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSensors = Assert.IsType<List<SensorViewDTO>>(okResult.Value);
            Assert.Empty(returnedSensors);
        }

        [Fact]
        public async Task GivenSensorExists_OnGetSensorById_ReturnsOk()
        {
            var sensorId = Guid.NewGuid();
            var sensor = new SensorViewDTO { Id = sensorId, Name = "SensorZ", Location = "Auckland", UpperWarning = 500, LowerWarning = 100, CreationTime = DateTime.UtcNow };
            _mockSensorService.Setup(s => s.GetSensorByIdAsync(sensorId)).ReturnsAsync(sensor);

            var result = await _controller.GetSensorById(sensorId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSensor = Assert.IsType<SensorViewDTO>(okResult.Value);
            Assert.Equal(sensorId, returnedSensor.Id);
        }

        [Fact]
        public async Task GivenSensorDoesNotExist_OnGetSensorById_ReturnsNotFound()
        {
            var sensorId = Guid.NewGuid();
            _mockSensorService.Setup(s => s.GetSensorByIdAsync(sensorId)).ReturnsAsync((SensorViewDTO?) null);

            var result = await _controller.GetSensorById(sensorId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task GivenSensorIsCreatedSuccessfully_OnCreateSensor_ReturnsOkResult()
        {
            var sensorId = Guid.NewGuid();
            var expectedOkResultObject = new
            {
                Message = "Resource created successfully.",
                Data = sensorId.ToString(),
            };
            var newSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 10, UpperWarning = 400};
            _mockSensorService.Setup(s => s.CreateSensorAsync(newSensor)).ReturnsAsync(sensorId);

            var result = await _controller.CreateSensor(newSensor);

            var createdResult = Assert.IsType<CreatedResult>(result);
            var responseObject = createdResult.Value;
            Assert.Equal("Resource created successfully.", responseObject.GetType().GetProperty("Message")?.GetValue(responseObject, null));
            Assert.Equal(sensorId.ToString(), responseObject.GetType().GetProperty("Data")?.GetValue(responseObject, null));
        }

        [Fact]
        public async Task GivenSensorAdditionFails_OnCreateSensor_ReturnsServerError()
        {
            var newSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 10, UpperWarning = 400 };
            _mockSensorService.Setup(s => s.CreateSensorAsync(newSensor)).Throws(new Exception());

            var result = await _controller.CreateSensor(newSensor);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An internal server error occurred.", objectResult.Value);
        }

        [Fact]
        public async Task GivenUpperWarningGreaterThanLowerWarning_OnCreateSensor_ReturnsBadRequest()
        {
            var newSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 100, UpperWarning = 40 };

            var result = await _controller.CreateSensor(newSensor);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenSensorIsUpdatedSuccessfully_OnUpdateSensor_ReturnsOkResult()
        {
            var sensorId = Guid.NewGuid();
            var updatedSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 100, UpperWarning = 400 };
            _mockSensorService.Setup(s => s.UpdateSensorAsync(sensorId, updatedSensor)).ReturnsAsync(true);

            var result = await _controller.UpdateSensor(sensorId, updatedSensor);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Resource updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task GivenSensorDoesNotExist_OnUpdateSensor_ReturnsNotFound()
        {
            var sensorId = Guid.NewGuid();
            var updatedSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 100, UpperWarning = 400 };
            _mockSensorService.Setup(s => s.UpdateSensorAsync(sensorId, updatedSensor)).ReturnsAsync(false);

            var result = await _controller.UpdateSensor(sensorId, updatedSensor);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Resource not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GivenUpperWarningGreaterThanLowerWarning_OnUpdateSensor_ReturnsBadRequest()
        {
            var sensorId = Guid.NewGuid();
            var newSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 100, UpperWarning = 40 };

            var result = await _controller.UpdateSensor(sensorId, newSensor);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenSensorUpdationFails_OnUpdateSensor_ReturnsServerError()
        {
            var sensorId = Guid.NewGuid();
            var newSensor = new SensorDTO { Name = "Test Sensor", Location = "Auckland", LowerWarning = 10, UpperWarning = 400 };
            _mockSensorService.Setup(s => s.UpdateSensorAsync(sensorId, newSensor)).Throws(new Exception());

            var result = await _controller.UpdateSensor(sensorId, newSensor);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An internal server error occurred.", objectResult.Value);
        }

        [Fact]
        public async Task GivenSensorIsDeletedSuccessfully_OnDeleteSensor_ReturnsNoContent()
        {
            var sensorId = Guid.NewGuid();
            _mockSensorService.Setup(s => s.DeleteSensorAsync(sensorId)).ReturnsAsync(true);

            var result = await _controller.DeleteSensor(sensorId);

            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GivenSensorDoesNotExist_OnDeleteSensor_ReturnsNotFound()
        {
            var sensorId = Guid.NewGuid();
            _mockSensorService.Setup(s => s.DeleteSensorAsync(sensorId)).ReturnsAsync(false);

            var result = await _controller.DeleteSensor(sensorId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Resource not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GivenSensorDeletionFails_OnDeleteSensor_ReturnsServerError()
        {
            var sensorId = Guid.NewGuid();
            _mockSensorService.Setup(s => s.DeleteSensorAsync(sensorId)).Throws(new Exception());

            var result = await _controller.DeleteSensor(sensorId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An internal server error occurred.", objectResult.Value);
        }

    }
}
