using ApparitionsClient.Services;

namespace ApparitionsClient.Desktop.Services;

public class DesktopLocationService : ILocationService
{
    public string Info => "GPS behavior not yet implemented on desktop.";

    public string ListenerStatus => "GPS behavior not yet implemented on desktop.";
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}