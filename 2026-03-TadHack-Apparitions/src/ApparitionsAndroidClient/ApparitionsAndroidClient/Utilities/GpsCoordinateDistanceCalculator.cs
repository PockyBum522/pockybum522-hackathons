using System;
using ApparitionsAndroidClient.Models;

namespace ApparitionsAndroidClient.Utilities;

public class GpsCoordinateDistanceCalculator
{
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //:::                                                                         :::
    //:::  This routine calculates the distance between two points (given the     :::
    //:::  latitude/longitude of those points). It is being used to calculate     :::
    //:::  the distance between two locations using GeoDataSource(TM) products    :::
    //:::                                                                         :::
    //:::  Definitions:                                                           :::
    //:::    South latitudes are negative, east longitudes are positive           :::
    //:::                                                                         :::
    //:::  Passed to function:                                                    :::
    //:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
    //:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
    //:::    unit = the unit you desire for results                               :::
    //:::           where: 'M' is statute miles (default)                         :::
    //:::                  'K' is kilometers                                      :::
    //:::                  'N' is nautical miles                                  :::
    //:::                  'F' is feet                                            :::
    //:::                                                                         :::
    //:::  Worldwide cities and other features databases with latitude longitude  :::
    //:::  are available at https://www.geodatasource.com                         :::
    //:::                                                                         :::
    //:::  For enquiries, please contact sales@geodatasource.com                  :::
    //:::                                                                         :::
    //:::  Official Web site: https://www.geodatasource.com                       :::
    //:::                                                                         :::
    //:::           GeoDataSource.com (C) All Rights Reserved 2022                :::
    //:::                                                                         :::
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    public static double GetDistance(GpsCoordinates firstCoordinate, GpsCoordinates secondCoordinate, char unit) 
    {
        if ((Math.Abs(firstCoordinate.Latitude - secondCoordinate.Latitude) < 0.0000001) && 
            (Math.Abs(firstCoordinate.Longitude - secondCoordinate.Longitude) < 0.0000001)) 
        {
            return 0;
        }
        
        var theta = firstCoordinate.Longitude - secondCoordinate.Longitude;
        var dist = Math.Sin(deg2rad(firstCoordinate.Latitude)) * Math.Sin(deg2rad(secondCoordinate.Latitude)) + Math.Cos(deg2rad(firstCoordinate.Latitude)) * Math.Cos(deg2rad(secondCoordinate.Latitude)) * Math.Cos(deg2rad(theta));

        dist = Math.Acos(dist);
        dist = rad2deg(dist);

        dist = unit switch
        {
            'K' => dist * 1.609344,
            'N' => dist * 0.8684,
            'F' => dist * 221760,
            _ => dist * 60 * 1.1515
        };

        return (dist);
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //::  This function converts decimal degrees to radians             :::
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private static double deg2rad(double deg) 
    {
      return (deg * Math.PI / 180.0);
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //::  This function converts radians to decimal degrees             :::
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private static double rad2deg(double rad) 
    {
      return (rad / Math.PI * 180.0);
    }

}