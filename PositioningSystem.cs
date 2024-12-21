using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSimulator
{
    public class PositioningSystem
    {
        double[] equatorialRadiusMeters =
        {   // TODO: Add planar math??
            6378137.0, // Perfect sphere
            6378137.0 // WGS-84
                // TODO: Add other projections??
        };
        double[] polarRadiusMeters =
        {   // TODO: Add planar math??
            6378137.0, // Perfect sphere
            6356752.31424518 // WGS-84
                // TODO: Add other projections??
        };
        double[] flatteningOfEllipsoid =
        {   // TODO: Add planar math??
            1 / 298.257223563, // perfect sphere?  I don't remember what this number is for :/
            1 / 298.257223563 // WGS-84
                // TODO: Add other projections??
        };

        int projection = 0;
        Coordinates currentLocation = new Coordinates(42.255637, -85.661945); // TODO: take starting coordinates in constructor


        public PositioningSystem(int projection) // TODO: enum?
        { // TODO: Add initial coordinates
            this.projection = projection;
        }


        double getEarthRadiusMeters(double latitudeRadians)
        {
            return Math.Sqrt(
                (Math.Pow(Math.Pow(equatorialRadiusMeters[projection], 2) * Math.Cos(latitudeRadians), 2)
                    + Math.Pow(Math.Pow(polarRadiusMeters[projection], 2) * Math.Sin(latitudeRadians), 2)) /* end numerator */
                / (Math.Pow(equatorialRadiusMeters[projection] * Math.Cos(latitudeRadians), 2)
                    + Math.Pow(polarRadiusMeters[projection] * Math.Sin(latitudeRadians), 2)) /* end denominator */
                ); /* End sqrt */
        }

        public double toRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public double toDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public Coordinates getNewCoordinates(double latDegrees, double lonDegrees, double distanceMeters, double bearingDegrees)
        {
            if (distanceMeters == 0) return new Coordinates(latDegrees, lonDegrees);

            double latitudeRadians = toRadians(latDegrees);
            double longitudeRadians = toRadians(lonDegrees);
            double bearingRadians = toRadians(bearingDegrees);
            double distanceRadians = distanceMeters / getEarthRadiusMeters(latitudeRadians);
            double newLatRadians = Math.Asin(Math.Sin(latitudeRadians) * Math.Cos(distanceRadians) + Math.Cos(latitudeRadians) * Math.Cos(bearingRadians) * Math.Sin(distanceRadians));
            double newLonRadians = longitudeRadians + Math.Atan2(Math.Sin(bearingRadians) * Math.Sin(distanceRadians) * Math.Cos(latitudeRadians), Math.Cos(distanceRadians) - Math.Sin(latitudeRadians) * Math.Sin(newLatRadians));

            return new Coordinates(toDegrees(newLatRadians), toDegrees(newLonRadians));
        }

        public void updatePosition(double distanceMeters, double bearingDegrees)
        {
            currentLocation = getNewCoordinates(currentLocation.getLatitude(), currentLocation.getLongitude(), distanceMeters, bearingDegrees); 
        }

        public Coordinates getPosition()
        {
            return currentLocation;
        }
    }
}
