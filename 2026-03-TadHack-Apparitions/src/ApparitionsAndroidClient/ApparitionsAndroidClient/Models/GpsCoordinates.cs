namespace ApparitionsAndroidClient.Models;

public class GpsCoordinates(double latitude, double longitude)
{
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;
}