using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WeatherForecastApp.Class
{
    public static class IconManager
    {
        /// <summary>
        /// Function returns BitmapImage for given name
        /// </summary>
        /// <param name="iconName">"Clear", "Rain" or "Clouds"</param>
        public static BitmapImage GetIconSource(string iconName)
        {
            if (iconName == "Clear")
            {
                return new BitmapImage(new Uri("/Images/sun.png", UriKind.Relative));
            }
            else if (iconName == "Rain")
            {
                return new BitmapImage(new Uri("/Images/rain.png", UriKind.Relative));
            }
            else if (iconName == "Clouds")
            {
                return new BitmapImage(new Uri("/Images/cloud.png", UriKind.Relative));
            }
            else
            {
                return new BitmapImage(new Uri("/Images/cloud.png", UriKind.Relative));
            }
        }
    }
}
