using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using Moq;
using Moq.Protected;
using System.Net;

namespace MauiAppTempoAgora.Tests;

public class WeatherServiceTests
{
    [Fact]
    public void GetForecastAsync_WithNullCity_ThrowsArgumentException()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => weatherService.GetForecastAsync(null!));
        Assert.Equal("City name cannot be empty", exception.Result.Message);
    }

    [Fact]
    public void GetForecastAsync_WithEmptyCity_ThrowsArgumentException()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => weatherService.GetForecastAsync(string.Empty));
        Assert.Equal("City name cannot be empty", exception.Result.Message);
    }

    [Fact]
    public async Task GetForecastAsync_WithWhitespaceCity_ThrowsArgumentException()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => weatherService.GetForecastAsync("   "));
        Assert.Equal("City name cannot be empty", exception.Message);
    }

    [Fact]
    public async Task GetForecastAsync_CityNotFound_ThrowsInvalidOperationException()
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
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => weatherService.GetForecastAsync("InvalidCity"));
        Assert.Equal("City not found.", exception.Message);
    }

    [Fact]
    public async Task GetForecastAsync_ServerError_ThrowsHttpRequestException()
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
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => weatherService.GetForecastAsync("London"));
        Assert.Contains("Error fetching weather forecast", exception.Message);
    }

    [Fact]
    public async Task GetForecastAsync_Success_ReturnsWeatherForecast()
    {
        // Arrange
        var jsonResponse = @"{
            ""coord"": { ""lon"": -0.1257, ""lat"": 51.5085 },
            ""weather"": [{ ""main"": ""Clouds"", ""description"": ""broken clouds"" }],
            ""main"": { ""temp_min"": 10.5, ""temp_max"": 12.3 },
            ""visibility"": 10000,
            ""wind"": { ""speed"": 5.2 },
            ""sys"": { ""sunrise"": 1609459200, ""sunset"": 1609492800 }
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
                Content = new StringContent(jsonResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act
        var result = await weatherService.GetForecastAsync("London");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(51.5085, result.Latitude);
        Assert.Equal(-0.1257, result.Longitude);
        Assert.Equal("broken clouds", result.Description);
        Assert.Equal("Clouds", result.MainCondition);
        Assert.Equal(10.5, result.TempMin);
        Assert.Equal(12.3, result.TempMax);
        Assert.Equal(5.2, result.WindSpeed);
        Assert.Equal(10000, result.Visibility);
    }

    [Fact]
    public async Task GetForecastAsync_WithNullWeatherData_HandlesGracefully()
    {
        // Arrange
        var jsonResponse = @"{
            ""coord"": null,
            ""weather"": null,
            ""main"": null,
            ""visibility"": 0,
            ""wind"": null,
            ""sys"": null
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
                Content = new StringContent(jsonResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act
        var result = await weatherService.GetForecastAsync("London");

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
    public async Task GetForecastAsync_VerifiesCorrectApiUrl()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var weatherService = new WeatherService(httpClient);

        // Act
        try
        {
            await weatherService.GetForecastAsync("Paris");
        }
        catch (InvalidOperationException)
        {
            // Expected exception
        }

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.NotNull(capturedRequest.RequestUri);
        Assert.Contains("api.openweathermap.org", capturedRequest.RequestUri.ToString());
        Assert.Contains("q=Paris", capturedRequest.RequestUri.ToString());
        Assert.Contains("units=metric", capturedRequest.RequestUri.ToString());
        Assert.Contains("appid=6135072afe7f6cec1537d5cb08a5a1a2", capturedRequest.RequestUri.ToString());
    }

    [Fact]
    public void WeatherService_Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new WeatherService(null!));
        Assert.Equal("httpClient", exception.ParamName);
    }
}
