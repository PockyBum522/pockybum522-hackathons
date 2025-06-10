using Serilog;

namespace NotesServer.AppLogistics;

public class ApplicationPathBuilder
{
    private readonly ILogger _logger;

    public ApplicationPathBuilder(ILogger logger)
    {
        _logger = logger;
    }
    
    public string GetLogPathPerMachine(string userName)
    {
        // Local image path on Jurrd's machine
        var jaredLoggingPath = "/home/jurrd3/Desktop/VconNotesServerLogs";

        var davidLoggingPath = "/home/david/Desktop/VconNotesServerLogs";
        
        var builtFullPath = "ERROR"; 
        
        if (userName.Contains("jurrd", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = jaredLoggingPath;
            
            _logger.Information("SETTING LOG PATH: Detected as running on one of Jurrd's machines - using full path: {BuiltFullPath}", builtFullPath);
        }
        
        if (userName.Contains("david", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = davidLoggingPath;
        
            _logger.Information("SETTING LOG PATH: Detected as running on one of David's machines - using full path: {BuiltFullPath}", builtFullPath);
        }
        
        // Ensure it exists
        Directory.CreateDirectory(builtFullPath);
        
        return builtFullPath;
    }
    
    public string GetExampleImagePathPerMachine(string userName)
    {
        // Local image path on Jurrd's machine
        var jaredReposPath = "/home/jurrd3/repos";

        var davidReposPath = "/media/secondary/repos";
        
        var imagePathInRepo =
            "pockybum522-hackathons/2025-06-TadHack-vCon/example-input/model-numbers-easier/PXL_20250516_132015872.jpg";

        var builtFullPath = "ERROR"; 
        
        if (userName.Contains("jurrd", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = Path.Join(jaredReposPath, imagePathInRepo);
            
            _logger.Information("SETTING LOG PATH: Detected as running on one of Jurrd's machines - using full path: {BuiltFullPath}", builtFullPath);
        }
        
        if (userName.Contains("david", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = Path.Join(davidReposPath, imagePathInRepo);
        
            _logger.Information("SETTING LOG PATH: Detected as running on one of David's machines - using full path: {BuiltFullPath}", builtFullPath);
        }
        
        return builtFullPath;
    }
}