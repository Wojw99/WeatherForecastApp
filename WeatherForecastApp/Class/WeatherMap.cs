using System.Collections.Generic;

namespace WeatherForecastApp.Class
{
    /// <summary>
    /// Klasa nadrzędna dla klas prognozy tygodniowej
    /// </summary>
    public class WeatherMap
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        /// <summary>
        /// Lista prognozy dla następnych 7 dni.
        /// </summary>
        public List<Daily> daily { get; set; }
    }
}
