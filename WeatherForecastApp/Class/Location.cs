using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;

namespace WeatherForecastApp.Class
{
    public class Location
    {
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(string city, double lan, double lon)
        {
            City = city;
            Latitude = lan;
            Longitude = lon;
        }
    }
}
