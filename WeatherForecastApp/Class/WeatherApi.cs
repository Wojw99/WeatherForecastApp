using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastApp.Class
{
    public class WeatherApi
    {
        private static readonly string apiKey = "54443f860b9257ccbab2a8815b5f3e0f";

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

        public static async Task<String> LoadHourlyForecast(double lon, double lat)
        {
            string urlHourly = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=minutely,current,daily&appid={apiKey}";

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
