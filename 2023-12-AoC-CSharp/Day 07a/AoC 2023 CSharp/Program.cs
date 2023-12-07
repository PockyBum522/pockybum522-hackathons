using AoC_2023_CSharp.Models;
using Serilog;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
    
    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.SampleData01
            .Split(Environment.NewLine);

        var answerTotal = 0;

        var allHands = new List<CamelCardsHand>();
        
        foreach (var line in rawLines)
        {
            allHands.Add(
                new CamelCardsHand(line));
        }
        
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
    }
}
