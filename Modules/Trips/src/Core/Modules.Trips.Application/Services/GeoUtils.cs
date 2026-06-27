using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Application.Services
{
    public static class GeoUtils
    {
        private const double EarthRadiusMeters = 6371000;

        public static double DistanceInMeters(
            double latitude1,
            double longitude1,
            double latitude2,
            double longitude2)
        {
            double dLat = DegreesToRadians(latitude2 - latitude1);
            double dLon = DegreesToRadians(longitude2 - longitude1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(latitude1)) *
                Math.Cos(DegreesToRadians(latitude2)) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMeters * c;
        }

        private static double DegreesToRadians(double degrees)
            => degrees * Math.PI / 180.0;
    }
}