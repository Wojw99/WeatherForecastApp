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
        bool isFirst = true;

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
            textBoxCity.Text = "wyszukuję dla: " + textBoxCity.Text + " proszę czekać";

            try
            {
                var location = await GeoLocationApi.LoadLocation(cityName);
                var json = await WeatherApi.LoadDailyForecast(location.Latitude, location.Longitude);
                var hourlyJson = await WeatherApi.LoadHourlyForecast(location.Latitude, location.Longitude);
                var weather = JsonConvert.DeserializeObject<WeatherMap>(json);
                var hourlyWeather = JsonConvert.DeserializeObject<WeatherHourlyMap>(hourlyJson);

                UpdateDays(weather);
                UpdateHours(hourlyWeather);
                ActivateButtons();
            }
            catch(IndexOutOfRangeException ex)
            {
                MessageBox.Show("The city doesn't exists!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                DeactivateButtons();
                HidePanels();
            }

            textBoxCity.Text = cityName;
            buttonCheck.IsEnabled = true;
        }

        private void HidePanels()
        {
            panelToday.Visibility = Visibility.Hidden;
            panelWeek.Visibility = Visibility.Hidden;
        }

        private void DeactivateButtons()
        {
            if (!isFirst)
            {
                buttonToday.IsEnabled = false;
                buttonWeek.IsEnabled = false;
            }
            isFirst = true;
        }

        private void ActivateButtons()
        {
            if (isFirst)
            {
                buttonToday.IsEnabled = true;
                buttonWeek.IsEnabled = true;
            }
            isFirst = false;
        }

        private void UpdateHours(WeatherHourlyMap hourlyWeather)
        {
            //IEnumerator<OneHour> hours = FindVisualChildren<OneHour>(panelToday).GetEnumerator();
            IEnumerator<OneHour> hours = FindLogicalChildren<OneHour>(panelToday).GetEnumerator();

            for (int i = 0; i < 24; i++)
            {
                hours.MoveNext();
                DateTime date = DateTime.UtcNow.AddHours(i);
                hours.Current.txtHour.Content = $"{date.ToLocalTime().Hour}:00";
                hours.Current.txtTemperature.Content = Math.Round((hourlyWeather.hourly[i].temp - 272.15), 2) + "° C";
                hours.Current.txtPressure.Content = hourlyWeather.hourly[i].pressure + "hPA";
                hours.Current.txtWind.Content = "wiatr " + hourlyWeather.hourly[i].wind_speed + "km/h"; //czy to nie są mph?
                hours.Current.img.Source = IconManager.GetIconSource(hourlyWeather.hourly[i].weather[0].main);
            }
        }

        private void UpdateDays(WeatherMap weather)
        {
            //IEnumerator<OneDay> days = FindVisualChildren<OneDay>(panelWeek).GetEnumerator();
            IEnumerator<OneDay> days = FindLogicalChildren<OneDay>(panelWeek).GetEnumerator();

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
        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject)rawChild;
                        if (child is T)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindLogicalChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        //public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        //{
        //    if (depObj != null)
        //    {
        //        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        //        {
        //            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
        //            if (child != null && child is T)
        //            {
        //                yield return (T)child;
        //            }

        //            foreach (T childOfChild in FindVisualChildren<T>(child))
        //            {
        //                yield return childOfChild;
        //            }
        //        }
        //    }
        //}
    }
}