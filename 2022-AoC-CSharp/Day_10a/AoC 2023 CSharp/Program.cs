using Serilog;
using Serilog.Events;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
    
    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var answerTotal = 0;
        
        foreach (var line in rawLines)
        {
            
            
            //answerTotal += 1;
        }
        
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
    }
}