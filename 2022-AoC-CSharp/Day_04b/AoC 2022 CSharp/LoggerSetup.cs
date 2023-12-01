using Serilog;

namespace AoC_2022_CSharp;

public class LoggerSetup
{
    /// <summary>
    /// Full path to base folder for logs (the folder, not the log files themselves)
    /// </summary>
    public static string AppName => "AoC-2023_";
    
    /// <summary>
    /// Full path to base folder for logs (the folder, not the log files themselves)
    /// </summary>
    public static string LogAppBasePath =>
        Path.Combine(
            GetAppRoot(), 
            "Logs");
    
    /// <summary>
    /// Full path to a generic log filename, for Serilog
    /// </summary>
    public static string LogPath => 
        Path.Combine(
            LogAppBasePath,
            $"{AppName}.log");
    
    public static ILogger BuildLogger()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LogPath) ?? "");

        return new LoggerConfiguration()
            .Enrich.WithProperty("Application", "SerilogTestContext")
            //.MinimumLevel.Information()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(LogPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
    
    /// <summary>
    /// Full path to the directory the app is running from, used for building log and settings directories
    /// </summary>
    public static string GetAppRoot()
    {
        try
        {
            return Path.GetDirectoryName(Environment.ProcessPath) ?? "ERROR_GETTING_APP_PATH";
        }
        catch (IOException ex)
        {
            throw new Exception($"Can't get app root directory{Environment.NewLine}{ex.StackTrace}");
        }
    }
}