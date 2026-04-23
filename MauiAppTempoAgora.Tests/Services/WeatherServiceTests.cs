using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace MauiAppTempoAgora.Tests.Services
{
    public class WeatherServiceTests
    {
        [Fact]
        public void GetForecastAsync_ThrowsArgumentException_WhenCityIsEmpty()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await weatherService.GetForecastAsync(""));
            Assert.Equal("city", exception.ParamName);
        }

        [Fact]
        public void GetForecastAsync_ThrowsArgumentException_WhenCityIsWhitespace()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await weatherService.GetForecastAsync("   "));
            Assert.Equal("city", exception.ParamName);
        }

        [Fact]
        public void GetForecastAsync_ThrowsArgumentNullException_WhenHttpClientIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WeatherService(null!));
        }

        [Fact]
        public async Task GetForecastAsync_ThrowsInvalidOperationException_WhenCityNotFound()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await weatherService.GetForecastAsync("InvalidCity"));
            Assert.Equal("City not found.", exception.Message);
        }

        [Fact]
        public async Task GetForecastAsync_ThrowsHttpRequestException_WhenServerErrorOccurs()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await weatherService.GetForecastAsync("TestCity"));
            Assert.Contains("500", exception.Message);
        }

        [Fact]
        public async Task GetForecastAsync_ReturnsWeatherForecast_WhenSuccessful()
        {
            // Arrange
            var json = @"{
                ""coord"": { ""lon"": -46.3, ""lat"": -23.5 },
                ""weather"": [{ ""main"": ""Clear"", ""description"": ""clear sky"" }],
                ""main"": { ""temp_min"": 18, ""temp_max"": 25 },
                ""visibility"": 10000,
                ""wind"": { ""speed"": 3.5 },
                ""sys"": { ""sunrise"": 1609459200, ""sunset"": 1609502400 }
            }";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act
            var result = await weatherService.GetForecastAsync("SaoPaulo");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(-23.5, result.Latitude);
            Assert.Equal(-46.3, result.Longitude);
            Assert.Equal("Clear", result.MainCondition);
            Assert.Equal("clear sky", result.Description);
            Assert.Equal(18, result.TempMin);
            Assert.Equal(25, result.TempMax);
            Assert.Equal(3.5, result.WindSpeed);
            Assert.Equal(10000, result.Visibility);
        }

        [Fact]
        public async Task GetForecastAsync_ReturnsNull_WhenResponseIsNull()
        {
            // Arrange
            var json = "{}";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act
            var result = await weatherService.GetForecastAsync("TestCity");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetForecastAsync_HandlesMissingOptionalFields()
        {
            // Arrange
            var json = @"{
                ""coord"": { ""lon"": 0, ""lat"": 0 },
                ""weather"": [],
                ""main"": {},
                ""visibility"": 0,
                ""wind"": {},
                ""sys"": {}
            }";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherService = new WeatherService(httpClient);

            // Act
            var result = await weatherService.GetForecastAsync("TestCity");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Latitude);
            Assert.Equal(0, result.Longitude);
            Assert.Equal(string.Empty, result.Description);
            Assert.Equal(string.Empty, result.MainCondition);
            Assert.Equal(0, result.TempMin);
            Assert.Equal(0, result.TempMax);
            Assert.Equal(0, result.WindSpeed);
            Assert.Equal(0, result.Visibility);
        }

        [Fact]
        public void Constructor_SetsHttpClient()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            // Act
            var weatherService = new WeatherService(httpClient);

            // Assert
            Assert.NotNull(weatherService);
        }
    }
}
