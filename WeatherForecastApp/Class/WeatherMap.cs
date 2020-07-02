using System.Collections.Generic;

namespace WeatherForecastApp.Class
{
    public class WeatherMap
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public List<Daily> daily { get; set; }
    }
}
