using MauiAppTempoAgora.Models;
using Newtonsoft.Json;

namespace MauiAppTempoAgora.Tests;

public class ModelTests
{
    [Fact]
    public void WeatherResponse_DeserializesCorrectly()
    {
        // Arrange
        var json = @"{
            ""coord"": { ""lon"": -0.1257, ""lat"": 51.5085 },
            ""weather"": [{ ""main"": ""Clouds"", ""description"": ""broken clouds"" }],
            ""main"": { ""temp_min"": 10.5, ""temp_max"": 12.3 },
            ""visibility"": 10000,
            ""wind"": { ""speed"": 5.2 },
            ""sys"": { ""sunrise"": 1609459200, ""sunset"": 1609492800 }
        }";

        // Act
        var result = JsonConvert.DeserializeObject<WeatherResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Coordinates);
        Assert.Equal(-0.1257, result.Coordinates.Longitude);
        Assert.Equal(51.5085, result.Coordinates.Latitude);
        Assert.NotNull(result.Weather);
        Assert.Single(result.Weather);
        Assert.Equal("Clouds", result.Weather[0].Main);
        Assert.Equal("broken clouds", result.Weather[0].Description);
        Assert.NotNull(result.Main);
        Assert.Equal(10.5, result.Main.TempMin);
        Assert.Equal(12.3, result.Main.TempMax);
        Assert.Equal(10000, result.Visibility);
        Assert.NotNull(result.Wind);
        Assert.Equal(5.2, result.Wind.Speed);
        Assert.NotNull(result.Sys);
        Assert.Equal(1609459200, result.Sys.Sunrise);
        Assert.Equal(1609492800, result.Sys.Sunset);
    }

    [Fact]
    public void Coordinates_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var coordinates = new Coordinates { Latitude = 48.8566, Longitude = 2.3522 };
        var json = JsonConvert.SerializeObject(coordinates);

        // Act
        var deserialized = JsonConvert.DeserializeObject<Coordinates>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(48.8566, deserialized.Latitude);
        Assert.Equal(2.3522, deserialized.Longitude);
    }

    [Fact]
    public void WeatherCondition_HasDefaultValues()
    {
        // Arrange & Act
        var weatherCondition = new WeatherCondition();

        // Assert
        Assert.Equal(string.Empty, weatherCondition.Main);
        Assert.Equal(string.Empty, weatherCondition.Description);
    }

    [Fact]
    public void MainWeatherInfo_PropertiesWorkCorrectly()
    {
        // Arrange
        var mainWeather = new MainWeatherInfo { TempMin = -5.5, TempMax = 15.3 };

        // Assert
        Assert.Equal(-5.5, mainWeather.TempMin);
        Assert.Equal(15.3, mainWeather.TempMax);
    }

    [Fact]
    public void WindInfo_SerializesWithCorrectJsonProperty()
    {
        // Arrange
        var json = @"{ ""speed"": 10.5 }";

        // Act
        var result = JsonConvert.DeserializeObject<WindInfo>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10.5, result.Speed);
    }

    [Fact]
    public void SystemInfo_HandlesUnixTimestamps()
    {
        // Arrange
        var systemInfo = new SystemInfo { Sunrise = 1609459200, Sunset = 1609492800 };

        // Assert
        Assert.Equal(1609459200, systemInfo.Sunrise);
        Assert.Equal(1609492800, systemInfo.Sunset);
    }

    [Fact]
    public void WeatherForecast_HasDefaultValues()
    {
        // Arrange & Act
        var forecast = new WeatherForecast();

        // Assert
        Assert.Equal(0, forecast.Latitude);
        Assert.Equal(0, forecast.Longitude);
        Assert.Equal(string.Empty, forecast.Description);
        Assert.Equal(string.Empty, forecast.MainCondition);
        Assert.Equal(0, forecast.TempMin);
        Assert.Equal(0, forecast.TempMax);
        Assert.Equal(0, forecast.WindSpeed);
        Assert.Equal(0, forecast.Visibility);
        Assert.Equal(default(DateTime), forecast.Sunrise);
        Assert.Equal(default(DateTime), forecast.Sunset);
    }

    [Fact]
    public void WeatherForecast_CanBeInitializedWithValues()
    {
        // Arrange & Act
        var forecast = new WeatherForecast
        {
            Latitude = 51.5085,
            Longitude = -0.1257,
            Description = "Clear sky",
            MainCondition = "Clear",
            TempMin = 8.0,
            TempMax = 15.0,
            WindSpeed = 3.5,
            Visibility = 10000,
            Sunrise = DateTime.Now,
            Sunset = DateTime.Now.AddHours(8)
        };

        // Assert
        Assert.Equal(51.5085, forecast.Latitude);
        Assert.Equal(-0.1257, forecast.Longitude);
        Assert.Equal("Clear sky", forecast.Description);
        Assert.Equal("Clear", forecast.MainCondition);
        Assert.Equal(8.0, forecast.TempMin);
        Assert.Equal(15.0, forecast.TempMax);
        Assert.Equal(3.5, forecast.WindSpeed);
        Assert.Equal(10000, forecast.Visibility);
    }

    [Fact]
    public void WeatherResponse_HandlesEmptyWeatherList()
    {
        // Arrange
        var json = @"{
            ""coord"": { ""lon"": 0, ""lat"": 0 },
            ""weather"": [],
            ""main"": { ""temp_min"": 0, ""temp_max"": 0 },
            ""visibility"": 0,
            ""wind"": { ""speed"": 0 },
            ""sys"": { ""sunrise"": 0, ""sunset"": 0 }
        }";

        // Act
        var result = JsonConvert.DeserializeObject<WeatherResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Weather);
        Assert.Empty(result.Weather);
    }

    [Fact]
    public void WeatherResponse_HandlesMissingOptionalFields()
    {
        // Arrange
        var json = @"{}";

        // Act
        var result = JsonConvert.DeserializeObject<WeatherResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Coordinates);
        Assert.Null(result.Weather);
        Assert.Null(result.Main);
        Assert.Equal(0, result.Visibility);
        Assert.Null(result.Wind);
        Assert.Null(result.Sys);
    }
}
