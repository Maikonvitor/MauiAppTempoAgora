using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Collections.ObjectModel;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        private readonly IWeatherService _weatherService;
        public static ObservableCollection<string> CityHistory { get; } = new ObservableCollection<string>();
        
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public MainPage(IWeatherService weatherService)
        {
            InitializeComponent();
            _weatherService = weatherService;
            BindingContext = this;
            
            // Carregar histórico salvo
            LoadCityHistory();
        }

        private void LoadCityHistory()
        {
            var savedCities = Preferences.Get("city_history", string.Empty);
            if (!string.IsNullOrEmpty(savedCities))
            {
                var cities = savedCities.Split('|');
                foreach (var city in cities)
                {
                    if (!string.IsNullOrWhiteSpace(city) && !CityHistory.Contains(city))
                    {
                        CityHistory.Add(city);
                    }
                }
                historyFrame.IsVisible = CityHistory.Count > 0;
            }
        }

        private void SaveCityHistory(string city)
        {
            if (!CityHistory.Contains(city))
            {
                CityHistory.Insert(0, city);
                if (CityHistory.Count > 5)
                {
                    CityHistory.RemoveAt(5);
                }
                
                var citiesString = string.Join("|", CityHistory);
                Preferences.Set("city_history", citiesString);
                historyFrame.IsVisible = true;
            }
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
                    await DisplayAlert("Warning", "Please enter a city name.", "OK");
                    return;
                }

                // Atualizar UI com animação
                weatherCard.IsVisible = true;
                lblMainTemp.Text = "⏳";
                lblDescription.Text = "Loading...";
                
                var forecast = await _weatherService.GetForecastAsync(city);

                if (forecast != null)
                {
                    // Salvar no histórico
                    SaveCityHistory(city);
                    
                    // Atualizar UI com dados
                    lblMainTemp.Text = $"{forecast.TempMax:F1}°C";
                    lblDescription.Text = $"{GetWeatherIcon(forecast.MainCondition)} {forecast.Description}";
                    lblTempMax.Text = $"{forecast.TempMax:F1}°C";
                    lblTempMin.Text = $"{forecast.TempMin:F1}°C";
                    lblWindSpeed.Text = $"{forecast.WindSpeed:F1} m/s";
                    lblVisibility.Text = $"{forecast.Visibility / 1000.0:F1} km";
                    lblSunrise.Text = forecast.Sunrise.ToString("HH:mm");
                    lblSunset.Text = forecast.Sunset.ToString("HH:mm");
                    lblCoordinates.Text = $"📍 {forecast.Latitude:F4}, {forecast.Longitude:F4}";
                    
                    // Animar entrada do card
                    await AnimateCardEntry();
                }
                else
                {
                    await DisplayAlert("Error", "No forecast data available.", "OK");
                }
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlert("Not Found", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnCurrentLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status == PermissionStatus.Granted)
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    
                    if (location != null)
                    {
                        // Usar reverse geocoding para obter nome da cidade
                        var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                        var placemark = placemarks?.FirstOrDefault();
                        
                        if (placemark != null && !string.IsNullOrEmpty(placemark.Locality))
                        {
                            txt_cidade.Text = placemark.Locality;
                            OnSearchClicked(sender, e);
                        }
                        else
                        {
                            // Fallback: usar coordenadas diretas na API
                            await DisplayAlert("Location", $"Coordinates: {location.Latitude:F4}, {location.Longitude:F4}", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Location", "Could not get current location.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Permission Denied", "Location permission is required to use this feature.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Location error: {ex.Message}", "OK");
            }
        }

        private async Task AnimateCardEntry()
        {
            weatherCard.Opacity = 0;
            weatherCard.TranslationY = 50;
            
            await weatherCard.FadeTo(1, 250, Easing.CubicOut);
            await weatherCard.TranslateTo(0, 0, 250, Easing.CubicOut);
        }

        private string GetWeatherIcon(string condition)
        {
            return condition.ToLower() switch
            {
                var c when c.Contains("clear") || c.Contains("sunny") => "☀️",
                var c when c.Contains("cloud") => "☁️",
                var c when c.Contains("rain") || c.Contains("drizzle") => "🌧️",
                var c when c.Contains("thunder") => "⛈️",
                var c when c.Contains("snow") => "❄️",
                var c when c.Contains("mist") || c.Contains("fog") => "🌫️",
                _ => "🌤️"
            };
        }

        private async void OnChipClicked(object sender, EventArgs e)
        {
            if (sender is Button chipButton && chipButton.BindingContext is string city)
            {
                txt_cidade.Text = city;
                OnSearchClicked(sender, e);
            }
        }
    }
}
