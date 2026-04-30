namespace ApparitionsClient.Services;

public interface ILocationService
{
    string Info { get; }
    
    string ListenerStatus { get; }
    
    double Latitude { get; set; }
    
    double Longitude { get; set; }
}