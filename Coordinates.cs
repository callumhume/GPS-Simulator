using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSimulator
{
    public class Coordinates
    {
        double lat = 0.0;
        double lon = 0.0;

        public Coordinates(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

        public Coordinates(Coordinates reference)
        {
            lat = reference.getLatitude();
            lon = reference.getLongitude();
        }

        public double getLatitude()
        {
            return lat;
        }

        public double getLongitude()
        {
            return lon;
        }
    }
}
