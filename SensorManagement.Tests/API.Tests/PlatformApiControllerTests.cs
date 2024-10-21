using Application.Interface;
using Microsoft.Extensions.Logging;
using Moq;
using Api.Controllers;
using Microsoft.Extensions.Options;
using Api.Models;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System.Net;

namespace API.Tests
{
    public class PlatformApiControllerTests
    {
        private readonly Mock<IPlatformService> _mockPlatformService;
        private readonly Mock<ILogger<PlatformApiController>> _mockLogger;
        private readonly PlatformApiController _controller;
        public PlatformApiControllerTests()
        {
            _mockPlatformService = new Mock<IPlatformService>();
            _mockLogger = new Mock<ILogger<PlatformApiController>>();

            var mockOptions = Options.Create(new PlatformApiConfig
            {
                Endpoint = "https://api.example.com/platform"
            });

            _controller = new PlatformApiController(_mockPlatformService.Object, mockOptions, _mockLogger.Object);
        }

        [Fact]
        public async Task GivenDataIsReturned_OnGetPlatformData_ReturnsData()
        {
            var platformApiParameters = new PlatformApiParameters();
            var mockPlatformApiResponse = new PlatformApiResponse { StreamId = Guid.NewGuid(), Timestamp = DateTime.UtcNow, StoredAt = DateTime.UtcNow, Value = 100 };
            _mockPlatformService.Setup(mps => mps.GetPlatformDataAsync(It.IsAny<string>(), platformApiParameters))
                .ReturnsAsync((HttpStatusCode.OK, mockPlatformApiResponse));

            var result = await _controller.GetPlatformData(platformApiParameters);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(mockPlatformApiResponse, okResult.Value);
        }

        [Theory]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.BadRequest)]
        public async Task GivenServiceReturnsNoContent_OnGetPlatformData_ReturnsTheSameResponseCode(HttpStatusCode statusCode)
        {
            _mockPlatformService
                .Setup(service => service.GetPlatformDataAsync(It.IsAny<string>(), It.IsAny<PlatformApiParameters>()))
                .ReturnsAsync((statusCode,It.IsAny<PlatformApiResponse>()));

            var result = await _controller.GetPlatformData(It.IsAny<PlatformApiParameters>());

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)statusCode, statusCodeResult.StatusCode);
        }
    }
}
