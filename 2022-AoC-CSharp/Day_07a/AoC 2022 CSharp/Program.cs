using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    public static ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawString = RawData.ActualData;

        
        
        Logger.Debug("First position after X number of unique chars: {FinalAnswer}");
    }
}