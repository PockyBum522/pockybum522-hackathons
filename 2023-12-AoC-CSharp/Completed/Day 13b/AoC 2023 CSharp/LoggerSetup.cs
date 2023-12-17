using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

public class LoggerSetup
{
    /// <summary>
    /// Full path to base folder for logs (the folder, not the log files themselves)
    /// </summary>
    private static string AppName => "AoC-2023_";
    
    /// <summary>
    /// Full path to base folder for logs (the folder, not the log files themselves)
    /// </summary>
    private static string LogAppBasePath =>
        Path.Combine(
            GetAppRoot(), 
            "Logs");
    
    /// <summary>
    /// Full path to a generic log filename, for Serilog
    /// </summary>
    private static string LogPath => 
        Path.Combine(
            LogAppBasePath,
            $"{AppName}.log");
    
    public static LoggerConfiguration ConfigureLogger()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LogPath) ?? "");

        return new LoggerConfiguration()
            .Enrich.WithProperty("Application", "SerilogTestContext")
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(LogPath, rollingInterval: RollingInterval.Hour);
    }
    
    /// <summary>
    /// Full path to the directory the app is running from, used for building log and settings directories
    /// </summary>
    private static string GetAppRoot()
    {
        return Path.GetDirectoryName(Environment.ProcessPath) ?? "ERROR_GETTING_APP_PATH";
    }
}