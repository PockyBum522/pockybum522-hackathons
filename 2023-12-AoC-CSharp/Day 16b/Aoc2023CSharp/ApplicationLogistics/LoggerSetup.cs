namespace Aoc2023CSharp.ApplicationLogistics;

public static class LoggerSetup
{
    /// <summary>
    /// Full path to base folder for logs (the folder, not the log files themselves)
    /// </summary>
    private static string LogAppBasePath =>
        Path.Combine(
            ApplicationData.RootDirectory, 
            "Logs");
    
    /// <summary>
    /// Full path to a generic log filename, for Serilog
    /// </summary>
    private static string LogPath => 
        Path.Combine(
            LogAppBasePath,
            $"{ApplicationData.Name}_.log");
    
    public static LoggerConfiguration ConfigureLogger()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LogPath) ?? "");

        return new LoggerConfiguration()
            .Enrich.WithProperty("Application", "Aoc2023CSharpContext")
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(LogPath, rollingInterval: RollingInterval.Hour);
    }
}