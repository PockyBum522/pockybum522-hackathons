using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    public static ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var dataLines = RawData.SampleDataPartA
            .Split(Environment.NewLine);

        var total = 0;

        for (var i = 0; i < dataLines.Length; i++)
        {
            
            
            // total += 
            
            // Logger.Debug("Group lines: {RawLine}", string.Join(" | ", groupLines));
        }

        Logger.Information("Total: {Total}", total);
    }
}