using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            buttonCheck.IsEnabled = false;
            var cityName = textBoxCity.Text;
            string tmp = textBoxCity.Text;
            textBoxCity.Text = "wyszukuję dla: " + textBoxCity.Text + " proszę czekać";
            var location = await GeoLocationApi.LoadLocation(cityName);
            var json = await WeatherApi.LoadDailyForecast(location.Latitude, location.Longitude);
            var weather = JsonConvert.DeserializeObject<WeatherMap>(json);

            textBoxCity.Text = "";

            IEnumerator<OneDay> days = FindVisualChildren<OneDay>(panelWeek).GetEnumerator();

            for (int i = 0; i < 7; i++)
            {
                days.MoveNext();
                DateTime date = DateTime.UtcNow.AddDays(i);
                days.Current.txtDate.Content = $"{date.Day}.{date.Month}.{date.Year}";
                days.Current.txtTemperature.Content = Math.Round((weather.daily[i].temp.day - 272.15), 2) + "° C";
                days.Current.txtPressure.Content = weather.daily[i].pressure + "hPA";
                days.Current.txtWind.Content = "wiatr " + weather.daily[i].wind_speed + "km/h"; //czy to nie są mph?
                days.Current.img.Source = IconManager.GetIconSource(weather.daily[i].weather[0].main);
            }
            buttonCheck.IsEnabled = true;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}