using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        private readonly IWeatherService _weatherService;

        public MainPage(IWeatherService weatherService)
        {
            InitializeComponent();
            _weatherService = weatherService;
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("No Connection", "You are not connected to the internet.", "OK");
                    return;
                }

                var city = txt_cidade.Text?.Trim();

                if (string.IsNullOrEmpty(city))
                {
                    lbl_res.Text = "Please enter a city name.";
                    return;
                }

                var forecast = await _weatherService.GetForecastAsync(city);

                if (forecast != null)
                {
                    lbl_res.Text = $"Latitude: {forecast.Latitude}\n" +
                                   $"Longitude: {forecast.Longitude}\n" +
                                   $"Description: {forecast.Description}\n" +
                                   $"Sunrise: {forecast.Sunrise}\n" +
                                   $"Sunset: {forecast.Sunset}\n" +
                                   $"Max Temp: {forecast.TempMax}°C\n" +
                                   $"Min Temp: {forecast.TempMin}°C\n" +
                                   $"Wind: {forecast.WindSpeed} m/s\n" +
                                   $"Visibility: {forecast.Visibility} m";
                }
                else
                {
                    lbl_res.Text = "No forecast data available.";
                }
            }
            catch (InvalidOperationException ex)
            {
                lbl_res.Text = ex.Message;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
