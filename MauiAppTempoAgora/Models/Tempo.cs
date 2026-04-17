using Newtonsoft.Json;

namespace MauiAppTempoAgora.Models
{
    public class WeatherResponse
    {
        [JsonProperty("coord")]
        public Coordinates? Coordinates { get; set; }

        [JsonProperty("weather")]
        public List<WeatherCondition>? Weather { get; set; }

        [JsonProperty("main")]
        public MainWeatherInfo? Main { get; set; }

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("wind")]
        public WindInfo? Wind { get; set; }

        [JsonProperty("sys")]
        public SystemInfo? Sys { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }
    }

    public class WeatherCondition
    {
        [JsonProperty("main")]
        public string Main { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
    }

    public class MainWeatherInfo
    {
        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }
    }

    public class WindInfo
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
    }

    public class SystemInfo
    {
        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }

    public class WeatherForecast
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; } = string.Empty;
        public string MainCondition { get; set; } = string.Empty;
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double WindSpeed { get; set; }
        public int Visibility { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
    }
}

