using MauiAppTempoAgora.Models;
using Newtonsoft.Json;
using Xunit;

namespace MauiAppTempoAgora.Tests.Models
{
    public class WeatherResponseTests
    {
        [Fact]
        public void WeatherResponse_DeserializesCorrectly()
        {
            var json = @"{
                ""coord"": { ""lon"": -46.3, ""lat"": -23.5 },
                ""weather"": [{ ""main"": ""Clouds"", ""description"": ""broken clouds"" }],
                ""main"": { ""temp_min"": 18, ""temp_max"": 25 },
                ""visibility"": 10000,
                ""wind"": { ""speed"": 3.5 },
                ""sys"": { ""sunrise"": 1609459200, ""sunset"": 1609502400 }
            }";

            var response = JsonConvert.DeserializeObject<WeatherResponse>(json);

            Assert.NotNull(response);
            Assert.NotNull(response.Coordinates);
            Assert.Equal(-46.3, response.Coordinates.Longitude);
            Assert.Equal(-23.5, response.Coordinates.Latitude);
            Assert.NotNull(response.Weather);
            Assert.Single(response.Weather);
            Assert.Equal("Clouds", response.Weather[0].Main);
            Assert.Equal("broken clouds", response.Weather[0].Description);
            Assert.NotNull(response.Main);
            Assert.Equal(18, response.Main.TempMin);
            Assert.Equal(25, response.Main.TempMax);
            Assert.Equal(10000, response.Visibility);
            Assert.NotNull(response.Wind);
            Assert.Equal(3.5, response.Wind.Speed);
            Assert.NotNull(response.Sys);
            Assert.Equal(1609459200, response.Sys.Sunrise);
            Assert.Equal(1609502400, response.Sys.Sunset);
        }

        [Fact]
        public void Coordinates_HasDefaultValues()
        {
            var coordinates = new Coordinates();

            Assert.Equal(0, coordinates.Longitude);
            Assert.Equal(0, coordinates.Latitude);
        }

        [Fact]
        public void WeatherCondition_InitializesWithEmptyStrings()
        {
            var condition = new WeatherCondition();

            Assert.Equal(string.Empty, condition.Main);
            Assert.Equal(string.Empty, condition.Description);
        }

        [Fact]
        public void MainWeatherInfo_HasDefaultValues()
        {
            var main = new MainWeatherInfo();

            Assert.Equal(0, main.TempMin);
            Assert.Equal(0, main.TempMax);
        }

        [Fact]
        public void WindInfo_HasDefaultValue()
        {
            var wind = new WindInfo();

            Assert.Equal(0, wind.Speed);
        }

        [Fact]
        public void SystemInfo_HasDefaultValues()
        {
            var sys = new SystemInfo();

            Assert.Equal(0, sys.Sunrise);
            Assert.Equal(0, sys.Sunset);
        }

        [Fact]
        public void WeatherForecast_HasDefaultValues()
        {
            var forecast = new WeatherForecast();

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
        public void WeatherResponse_DeserializesNullArraysGracefully()
        {
            var json = @"{
                ""coord"": null,
                ""weather"": null,
                ""main"": null,
                ""visibility"": 5000,
                ""wind"": null,
                ""sys"": null
            }";

            var response = JsonConvert.DeserializeObject<WeatherResponse>(json);

            Assert.NotNull(response);
            Assert.Null(response.Coordinates);
            Assert.Null(response.Weather);
            Assert.Null(response.Main);
            Assert.Equal(5000, response.Visibility);
            Assert.Null(response.Wind);
            Assert.Null(response.Sys);
        }
    }
}
