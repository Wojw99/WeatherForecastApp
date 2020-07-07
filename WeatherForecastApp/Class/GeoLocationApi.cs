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
    public class GeoLocationApi
    {
        private static readonly string apiKey = "3d960304a58a466babdce85b27d3a4df ";

        public static async Task<Location> LoadLocation(string city)
        {
            string urlDaily = $"https://api.opencagedata.com/geocode/v1/json?q={city}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(urlDaily))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var lan = Double.Parse(GetValueOf("lat", json));
                        var lon = Double.Parse(GetValueOf("lng", json));
                        return new Location(city, lan, lon);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        // be careful when using this function
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

            throw new Exception("Not found.");
        }
    }
}
