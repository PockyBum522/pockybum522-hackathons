using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Models;

public class ScratchCard
{
    private readonly ILogger _logger;

    public ScratchCard(ILogger logger, int lineNumber, string[] rawLines)
    {
        _logger = logger;
        
        var thisCardsLine = rawLines[lineNumber];
        
        _logger.Debug("Parsing raw line {RawLine} into card:", thisCardsLine);
        
        Id = ParseCardId(thisCardsLine);
        WinningNumbers = ParseNumbers(thisCardsLine, 0);
        OurNumbers = ParseNumbers(thisCardsLine, 1);

        var cardsToAddCount = WinningNumbersCount();
        _logger.Debug("Card {Id} won {NumWon} additional cards", Id, cardsToAddCount);
        
        // Add additional cards
        for (var i = lineNumber + 1; i < lineNumber + cardsToAddCount + 1; i++)
        {
            _logger.Debug("Adding new card attached to card {Id} from card on line: {AdditionalCardLine}", Id, i);
            _logger.Debug("Additional card: {AdditionalRawLine}", Id, rawLines[i]);
            
            AdditionalWonCards.Add(
                new ScratchCard(_logger, i, rawLines));
        }
    }

    public string Id { get; }

    public string[] WinningNumbers { get; }

    public string[] OurNumbers { get; }

    public List<ScratchCard> AdditionalWonCards { get; } = new();
    
    

    private int WinningNumbersCount()
    {
        var finalScore = 0;
        
        foreach (var winningNumber in WinningNumbers)
        {
            if (!OurNumbers.Contains(winningNumber)) continue;

            finalScore++;
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