using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Models;

public class ScratchCard
{
    private readonly ILogger _logger;

    public ScratchCard(ILogger logger, string rawLine)
    {
        _logger = logger;
        
        _logger.Debug("Parsing raw line {RawLine} into card:", rawLine);
        
        Id = ParseCardId(rawLine);
        WinningNumbers = ParseNumbers(rawLine, 0);
        OurNumbers = ParseNumbers(rawLine, 1);
        
        _logger.Debug("Winning numbers: {@WinningNumbers}", WinningNumbers);
        _logger.Debug("Our numbers: {@OurNumbers}", OurNumbers);
    }

    public string Id { get; }

    public string[] WinningNumbers { get; }

    public string[] OurNumbers { get; }

    public int GetScore()
    {
        var finalScore = 0;
        
        foreach (var winningNumber in WinningNumbers)
        {
            if (!OurNumbers.Contains(winningNumber)) continue;

            if (finalScore == 0)
            {
                finalScore = 1;
            }
            else
            {
                finalScore *= 2;
            }
        }

        return finalScore;
    }
    
    private string ParseCardId(string line)
    {
        var idString = line.Split(' ')[1];
            
        idString = 
            idString.Replace(":", "");

        _logger.Debug("Setting card ID to: {ParsedId}", idString);
        
        return idString;
    }

    private string[] ParseNumbers(string rawLine, int whichNumbers)
    {
        var trimmedLine = rawLine.Replace($"Card {Id}: ", "");

        var winningNumbersRaw = trimmedLine.Split("|")[whichNumbers];

        winningNumbersRaw = winningNumbersRaw.Trim();

        return winningNumbersRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
}