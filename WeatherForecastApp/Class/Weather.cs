using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastApp.Class
{
    public class Weather
    {
        public double Temp { get; set; }
        public DateTime Date { get; set; }

        public Weather(double temp, DateTime date)
        {
            Temp = temp;
            Date = date;
        }
    }
}
