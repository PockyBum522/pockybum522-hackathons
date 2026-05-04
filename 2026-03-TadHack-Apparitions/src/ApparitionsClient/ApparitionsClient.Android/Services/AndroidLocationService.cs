using ApparitionsClient.Services;

namespace ApparitionsClient.Android.Services;

public class AndroidLocationService : ILocationService
{
    // (it actually is implemented, I just need to get the interfaces working first)
    public string Info => "GPS behavior not yet implemented on Android.";

    public string ListenerStatus => "GPS behavior not yet implemented on Android.";

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}