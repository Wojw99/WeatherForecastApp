using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastApp.Class
{
    using R = Properties.Resources;
    /// <summary>
    /// Klasa odpowiedzialna za połączenie z API pogodowym (api.openweathermap.org)
    /// </summary>
    public class WeatherApi
    {
        private static readonly string apiKey = R.Weather_API_key;

        /// <summary>
        /// Zwraca obiekt JSON dla zadanych współrzędnych (prognozy tygodniowej).
        /// </summary>
        public static async Task<String> LoadDailyForecast(double lat, double lng)
        {
            string urlDaily = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lng}&exclude=current,minutely,hourly&appid={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(urlDaily))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Zwraca obiekt JSON dla zadanych współrzędnych (prognozy godzinowej dla aktualnego dnia).
        /// </summary>
        public static async Task<String> LoadHourlyForecast(double lat, double lng)
        {
            string urlHourly = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lng}&exclude=minutely,current,daily&appid={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(urlHourly))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        result = result.Replace("1h", "_1h"); // because C# doesnt't accept a name with number as first letter
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }
    }
}
