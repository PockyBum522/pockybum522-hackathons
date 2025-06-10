using Serilog;

namespace NotesServer.AppLogistics;

public static class ApplicationPathBuilder
{
    public static string GetLogPathPerMachine(string userName)
    {
        // Local image path on Jurrd's machine
        var jaredLoggingPath = "/home/jurrd3/Desktop/VconNotesServerLogs";

        var davidLoggingPath = "/home/david/Desktop/VconNotesServerLogs";
        
        var builtFullPath = "ERROR"; 
        
        if (userName.Contains("jurrd", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = jaredLoggingPath;

            Console.WriteLine($"SETTING LOG PATH: Detected as running on one of Jurrd's machines - using full path: {builtFullPath}");
        }
        
        if (userName.Contains("david", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = davidLoggingPath;
        
            Console.WriteLine($"SETTING LOG PATH: Detected as running on one of David's machines - using full path: {builtFullPath}");
        }
        
        // Ensure it exists
        Directory.CreateDirectory(builtFullPath);
        
        return builtFullPath;
    }
    
    public static string GetExampleImagePathPerMachine(string userName)
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
            
            Console.WriteLine($"SETTING EXAMPLE IMAGE PATH: Detected as running on one of Jurrd's machines - using full path: {builtFullPath}");
        }
        
        if (userName.Contains("david", StringComparison.InvariantCultureIgnoreCase))
        {
            builtFullPath = Path.Join(davidReposPath, imagePathInRepo);
        
            Console.WriteLine($"SETTING EXAMPLE IMAGE PATH: Detected as running on one of David's machines - using full path: {builtFullPath}");
        }
        
        return builtFullPath;
    }
}