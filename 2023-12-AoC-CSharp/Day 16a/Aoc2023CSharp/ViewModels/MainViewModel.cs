namespace Aoc2023CSharp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    
    private readonly ILogger _logger;
    private readonly LoggerConfiguration _loggerConfiguration;

    public MainViewModel() { throw new NotImplementedException(); }     // This method being present keeps the designer happy
    
    public MainViewModel(LoggerConfiguration loggerConfiguration)
    {
        _loggerConfiguration = loggerConfiguration;

        _logger = loggerConfiguration
            .MinimumLevel.Debug()
            .CreateLogger();
    }

}
