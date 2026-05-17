using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace MauiAppTempoAgora
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            MainPage = serviceProvider.GetRequiredService<MainPage>();
        }
    }
}
