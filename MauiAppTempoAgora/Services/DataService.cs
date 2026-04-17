using MauiAppTempoAgora.Models;
using Newtonsoft.Json;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast?> GetForecastAsync(string city);
    }

    public class WeatherService : IWeatherService
    {
        private const string ApiKey = "6135072afe7f6cec1537d5cb08a5a1a2";
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<WeatherForecast?> GetForecastAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name cannot be empty", nameof(city));
            }

            var url = $"https://api.openweathermap.org/data/2.5/weather?" +
                      $"q={city}&units=metric&appid={ApiKey}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(json);

                if (weatherResponse == null)
                {
                    return null;
                }

                var baseTime = DateTime.UnixEpoch;
                var sunrise = baseTime.AddSeconds(weatherResponse.Sys?.Sunrise ?? 0).ToLocalTime();
                var sunset = baseTime.AddSeconds(weatherResponse.Sys?.Sunset ?? 0).ToLocalTime();

                return new WeatherForecast
                {
                    Latitude = weatherResponse.Coordinates?.Latitude ?? 0,
                    Longitude = weatherResponse.Coordinates?.Longitude ?? 0,
                    Description = weatherResponse.Weather?.FirstOrDefault()?.Description ?? string.Empty,
                    MainCondition = weatherResponse.Weather?.FirstOrDefault()?.Main ?? string.Empty,
                    TempMin = weatherResponse.Main?.TempMin ?? 0,
                    TempMax = weatherResponse.Main?.TempMax ?? 0,
                    WindSpeed = weatherResponse.Wind?.Speed ?? 0,
                    Visibility = weatherResponse.Visibility,
                    Sunrise = sunrise,
                    Sunset = sunset
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException("City not found.");
            }
            else
            {
                throw new HttpRequestException($"Error fetching weather forecast. HTTP Code: {(int)response.StatusCode}.");
            }
        }
    }
}
