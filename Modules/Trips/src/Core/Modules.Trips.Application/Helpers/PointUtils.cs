using NetTopologySuite.Geometries;

namespace Modules.Trips.Application.Helpers;

public static class PointUtils
{
    public static Point PointFromCoordinates(double longitude, double latitude)
    {
        return new Point(longitude, latitude) { SRID = 4326 };
    }
}
