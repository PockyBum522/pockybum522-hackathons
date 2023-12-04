using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Debug()
            .CreateLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);
        
        var answerTotal = 0;
        
        foreach (var line in rawLines)
        {
            var currentCard = new ScratchCard(Logger, line);
            
            answerTotal += currentCard.GetScore();
        }
        
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
    }
}