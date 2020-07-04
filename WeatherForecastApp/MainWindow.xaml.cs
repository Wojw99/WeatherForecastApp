using Newtonsoft.Json;
using System.Windows;
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
            //string json = JsonConvert.SerializeObject();
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
            var json = await WeatherApi.LoadHourlyForecast(19.01, 50.16);
            var root = JsonConvert.DeserializeObject<WeatherHourlyMap>(json);

            //textBoxCity.Text = $"{root.daily[0].weather[0].description} : {root.daily[0].temp.day - 272.15}";
            textBoxCity.Text = $"{root.hourly[0].weather[0].description} : {root.hourly[0].temp - 272.15}";
        }
    }
}
