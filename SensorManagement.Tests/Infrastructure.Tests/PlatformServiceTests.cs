using Application.Models;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Infrastructure.Tests
{
    public class PlatformServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<PlatformService>> _mockLogger;
        private readonly PlatformService _platformService;
        public PlatformServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockLogger = new Mock<ILogger<PlatformService>>();
            _platformService = new PlatformService(_httpClient, _mockLogger.Object);
        }

        [Fact]
        public async Task GivenApiCallIsSuccessful_OnGetPlatformApiAsync_ReturnsOkResult()
        {
            var platformApiParameters = new PlatformApiParameters();
            var expectedResponse = new PlatformApiResponse 
            { 
                StreamId = Guid.NewGuid(),
                StoredAt = DateTime.UtcNow, 
                Timestamp = DateTime.UtcNow,
                Value = 1
            };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var (statusCode, response) = await _platformService.GetPlatformDataAsync("https://example.com", platformApiParameters);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(response);
            Assert.Equal(response.StreamId, expectedResponse.StreamId);
            Assert.Equal(response.StoredAt, expectedResponse.StoredAt);
            Assert.Equal(response.Timestamp, expectedResponse.Timestamp);
            Assert.Equal(response.Value, expectedResponse.Value);
        }

        [Theory]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.BadRequest)]
        public async Task GivenApiCallFails_OnGetPlatformDataAsync_ReturnsTheStatusCodeFromAPI(HttpStatusCode statusCode)
        {
            var platformApiParameters = new PlatformApiParameters();
            var httpResponse = new HttpResponseMessage(statusCode);

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                });

            var (statusCodeInResponse, response) = await _platformService.GetPlatformDataAsync("https://example.com", platformApiParameters);

            Assert.Equal(statusCode, statusCodeInResponse);
            Assert.Null(response);
        }

        [Fact]
        public async Task GivenExceptionIsThrown_OnGetPlatformDataAsync_ShouldReturnServerError()
        {
           var platformApiParameters = new PlatformApiParameters();
            _mockHttpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ThrowsAsync(new Exception());

            var (statusCode, response) = await _platformService.GetPlatformDataAsync("https://example.com", platformApiParameters);

            Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
            Assert.Null(response);
        }
    }
}
