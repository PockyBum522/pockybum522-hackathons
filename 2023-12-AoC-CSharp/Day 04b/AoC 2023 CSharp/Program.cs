using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

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

        for (var i = 0; i < rawLines.Length; i++)
        {
            var currentCard = new ScratchCard(Logger, i, rawLines);

            answerTotal += GetScore(currentCard);
        }

        Logger.Information("Answer: {AnswerTotal}", answerTotal);
    }

    private static int GetScore(ScratchCard scratchCardToScore)
    {
        var finalScore = 1;
        
        foreach (var subCard in scratchCardToScore.AdditionalWonCards)
        {
            finalScore += GetScore(subCard);
        }

        return finalScore;
    }
}
