using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastApp.Class
{
    /// <summary>
    /// Klasa nadrzędna dla wszystkich klas prognozy godzinowej aktualnego dnia.
    /// </summary>
    public class WeatherHourlyMap
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        /// <summary>
        /// Lista prognozy dla następnych 24h.
        /// </summary>
        public List<Hourly> hourly { get; set; }
    }
}
