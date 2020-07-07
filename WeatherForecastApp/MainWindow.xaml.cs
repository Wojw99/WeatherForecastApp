using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WeatherForecastApp.Class;

namespace WeatherForecastApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //img.Source = IconManager.GetIconSource("Clear");
        }

        private void ButtonToday_Click(object sender, RoutedEventArgs e)
        {
            panelToday.Visibility = Visibility.Visible;
            panelWeek.Visibility = Visibility.Hidden;
            buttonToday.IsEnabled = false;
            buttonWeek.IsEnabled = true;
        }

        private void ButtonWeek_Click(object sender, RoutedEventArgs e)
        {
            panelToday.Visibility = Visibility.Hidden;
            panelWeek.Visibility = Visibility.Visible;
            buttonToday.IsEnabled = true;
            buttonWeek.IsEnabled = false;
        }

        private async void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            var cityName = textBoxCity.Text;
            var location = await GeoLocationApi.LoadLocation(cityName);
            var json = await WeatherApi.LoadDailyForecast(location.Latitude, location.Longitude);
            var weather = JsonConvert.DeserializeObject<WeatherMap>(json);
            
            textBoxCity.Text = $"{location.City} ({location.Latitude}, {location.Longitude}) : {weather.daily[0].weather[0].description} : {weather.daily[0].temp.day - 272.15}";
        }
    }
}
