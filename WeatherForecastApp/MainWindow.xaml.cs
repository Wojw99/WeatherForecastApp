using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using WeatherForecastApp.Class;

namespace WeatherForecastApp
{
    using R = Properties.Resources;
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFirst = true; //jeśli zmienna jest ustawiona na true, oznacza to, że jest to pierwsze pobranie danych

        public MainWindow()
        {
            InitializeComponent();
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

        // zdarzenie asynchroniczne odpowiedzialne za odwołanie się do klas Api i pobranie wyników za pomocą metod tychże klas
        private async void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            buttonCheck.IsEnabled = false;
            var cityName = textBoxCity.Text;
            textBoxMessage.Text = $"{R.search_for} {textBoxCity.Text} {R.wait_please}";

            try
            {
                var location = await GeoLocationApi.LoadLocation(cityName); // pobierz współrzędne dla podanego miasta
                var json = await WeatherApi.LoadDailyForecast(location.Latitude, location.Longitude); // pobierz tygodniową prognozę dla podanych współrzędnych
                var hourlyJson = await WeatherApi.LoadHourlyForecast(location.Latitude, location.Longitude); // pobierz godzinową prognozę dla podanych współrzędnych
                var weather = JsonConvert.DeserializeObject<WeatherMap>(json); // deserializuj obiekt JSON na WeatherMap
                var hourlyWeather = JsonConvert.DeserializeObject<WeatherHourlyMap>(hourlyJson); // deserializuj obiekt JSON na WeatherHourlyMap

                // metody aktualizacji widoku
                UpdateDays(weather);
                UpdateHours(hourlyWeather);
                ActivateButtons();
            }
            catch(IndexOutOfRangeException ex)
            {
                MessageBox.Show(R.message_city_not_exist);
                DeactivateButtons();
                HidePanels();
                textBoxMessage.Text = "";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                DeactivateButtons();
                HidePanels();
                textBoxMessage.Text = "";
            }

            cityName = cityName.ToUpper();
            textBoxMessage.Text = cityName;
            buttonCheck.IsEnabled = true;
        }

        // ukrywa panel Today i panel Week
        private void HidePanels()
        {
            panelToday.Visibility = Visibility.Hidden;
            panelWeek.Visibility = Visibility.Hidden;
        }

        // deaktywuje buttony Week i Today
        private void DeactivateButtons()
        {
            if (!isFirst)
            {
                buttonToday.IsEnabled = false;
                buttonWeek.IsEnabled = false;
            }
            isFirst = true;
        }

        // aktywuje buttony Week i Today
        private void ActivateButtons()
        {
            if (isFirst)
            {
                buttonToday.IsEnabled = true;
                buttonWeek.IsEnabled = true;
            }
            isFirst = false;
        }

        //metoda odpowiedzialna za wyświetlanie informacji w TodayPanel, godzina po godzinie
        private void UpdateHours(WeatherHourlyMap hourlyWeather)
        {
            IEnumerator<OneHour> hours = FindLogicalChildren<OneHour>(panelToday).GetEnumerator();

            for (int i = 0; i < 24; i++)
            {
                hours.MoveNext();
                DateTime date = DateTime.UtcNow.AddHours(i);
                hours.Current.txtHour.Content = $"{date.ToLocalTime().Hour}:00";
                hours.Current.txtTemperature.Content = Math.Round((hourlyWeather.hourly[i].temp - 272.15), 2) + R.celsius_sign;
                hours.Current.txtPressure.Content = hourlyWeather.hourly[i].pressure + R.hPa_sign;
                hours.Current.txtWind.Content = R.wind + " " + hourlyWeather.hourly[i].wind_speed + R.ms_sign;
                hours.Current.img.Source = IconManager.GetIconSource(hourlyWeather.hourly[i].weather[0].main);
            }
        }

        //metoda odpowiedzialna za wyświetlanie informacji w WeekPanel, dzień po dniu
        private void UpdateDays(WeatherMap weather)
        {
            IEnumerator<OneDay> days = FindLogicalChildren<OneDay>(panelWeek).GetEnumerator();

            for (int i = 0; i < 7; i++)
            {
                days.MoveNext();
                DateTime date = DateTime.UtcNow.AddDays(i);
                days.Current.txtDate.Content = $"{date.Day}.{date.Month}.{date.Year}";
                days.Current.txtTemperature.Content = Math.Round((weather.daily[i].temp.day - 272.15), 2) + R.celsius_sign;
                days.Current.txtPressure.Content = weather.daily[i].pressure + R.hPa_sign;
                days.Current.txtWind.Content = R.wind + " " + weather.daily[i].wind_speed + R.ms_sign;
                days.Current.img.Source = IconManager.GetIconSource(weather.daily[i].weather[0].main);
            }
        }

        //metoda służąca do zwracaniu wszystkich kontrolek danego typu w danym panelu
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