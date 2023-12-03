using Serilog;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var totalCumulative = 0;
        
        foreach (var line in rawLines)
        {
            
            
            totalCumulative += 1;
        }
        
        Logger.Information("Answer: {Total}", totalCumulative);
    }
}