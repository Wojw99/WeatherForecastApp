using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastApp.Class
{
    public class Weather
    {
        public string City { get; set; }

        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Clouds { get; set; }
        public double WindSpeed { get; set; }

        public DateTime Date { get; set; }

        public Weather(string city, double temperature, double pressure, double clouds, double windSpeed, DateTime date)
        {
            City = city;
            Temperature = temperature;
            Pressure = pressure;
            Clouds = clouds;
            WindSpeed = windSpeed;
            Date = date;
        }
    }
}
