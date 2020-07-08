using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace WeatherForecastApp.Class
{
    using R = Properties.Resources;
    /// <summary>
    /// Klasa odpowiedzialna za połączenie z API geolokalizacyjnym (api.opencagedata.com)
    /// </summary>
    public class GeoLocationApi
    {
        private static readonly string apiKey = R.Geolocation_API_key;

        /// <summary>
        /// Zwraca współrzędne geograficzne dla podanego miasta
        /// </summary>
        public static async Task<Location> LoadLocation(string city)
        {
            string urlDaily = $"https://api.opencagedata.com/geocode/v1/json?q={city}&key={apiKey}";

            if(Regex.IsMatch(city, @"\d"))
            {
                throw new Exception(R.message_city_not_exist);
            }

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(urlDaily))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var lan = Double.Parse(GetValueOf(R.lat, json));
                        var lon = Double.Parse(GetValueOf(R.lng, json));
                        return new Location(city, lan, lon);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Metoda zwraca wartość dla zadanego parametru JSON. UWAGA! Prawdopodobnie działa tylko dla "lat" i "lng".
        /// </summary>
        /// <param name="name">Parametr JSON do wyszukania</param>
        /// <param name="json">Obiekt JSON, w którym szukać</param>
        private static string GetValueOf(string name, string json)
        {
            int index = json.IndexOf(name);
            string number = "";

            json = json.Replace("u00b0", "");
            //Debug.WriteLine(json);

            for (int i = index; i < json.Length; i++)
            {
                if (Regex.IsMatch(json[i].ToString(), @"\d"))
                {
                    number += json[i];
                }
                else if (json[i] == '\\')
                {
                    number += ".";
                }
                else if (json[i] == '\'')
                {
                    return number;
                }
            }

            throw new Exception(R.message_not_found);
        }
    }
}
