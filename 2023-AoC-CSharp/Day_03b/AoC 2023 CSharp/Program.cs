using System.Drawing;
using AoC_2023_CSharp.Models;
using Serilog;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var total = 0;

        var uniqueSymbols = new List<char>() { '*' }; // Since we're only looking for one symbol now...
        var foundNumbers = new List<FoundNumber>(); 
        
        // For making sure a part number we're adding has not been just added
        var lastStartPosition = -1;
        var lastY = -1;
        
        for (var y = 0; y < rawLines.Length; y++)
        {
            var line = rawLines[y];
            
            for (var x = 0; x < line.Length; x++)
            {
                var currentCharacter = line[x];

                if (!char.IsDigit(currentCharacter)) continue;

                // Is a digit, let's check around the part number
                
                var fullGearNumber = GetFullGearNumberAt(x, line);

                var gearNumberStartPos = GetStartPositionOfGearNumber(x, line);
                var gearNumberEndPos = GetEndPositionOfGearNumber(x, line);
                var gearNumberLength = gearNumberEndPos - gearNumberStartPos;

                var asteriskPosition =
                    GetAdjacentSymbolPosition(gearNumberStartPos, y, gearNumberLength, uniqueSymbols, rawLines);
                
                if (asteriskPosition.X == -1)
                {
                    //PrintNonMatchingGearNumberForDebug(x, y, rawLines);
                    
                    lastStartPosition = gearNumberStartPos;
                    lastY = y;
                    
                    continue;
                }
                
                // If we get here, then current character is a digit and there's a symbol adjacent to it
                if (lastStartPosition != gearNumberStartPos ||
                    lastY != y)
                {
                    PrintGearNumberForDebug(x, y, rawLines, fullGearNumber);
                    
                    foundNumbers.Add(
                        new FoundNumber()
                        {
                            AsteriskPosition = asteriskPosition,
                            Number = fullGearNumber,
                            NumberPosition = new Point(gearNumberStartPos, y)
                        });
                }
                
                lastStartPosition = gearNumberStartPos;
                lastY = y;
            }
        }
        
        Logger.Information("Unique symbols found: {Symbols}", string.Join(", ", uniqueSymbols));

        for (var i = 0; i < foundNumbers.Count; i++)
        {
            var gearNumber = foundNumbers[i];
            Logger.Debug("Found number: {GearNumber} at X{NumX}, Y{NumY} with asterisk at X{AsteriskX}, Y{AsteriskY}",
                gearNumber.Number, gearNumber.NumberPosition.X, gearNumber.NumberPosition.Y,
                gearNumber.AsteriskPosition.X, gearNumber.AsteriskPosition.Y);

            // Starting on the next entry in the list, look for matches on the asterisk location
            for (var j = i + 1; j < foundNumbers.Count; j++)
            {
                var foundNumberToCompare = foundNumbers[j];

                if (gearNumber.AsteriskPosition.X == foundNumberToCompare.AsteriskPosition.X &&
                    gearNumber.AsteriskPosition.Y == foundNumberToCompare.AsteriskPosition.Y)
                {
                    // Found two numbers with the same asterisk
                    total +=
                        int.Parse(gearNumber.Number) *
                        int.Parse(foundNumberToCompare.Number);
                }
            }
        }

        Logger.Information("Answer: {Total}", total);
    }

    private static void PrintNonMatchingGearNumberForDebug(int x, int y, string[] rawLines)
    {
        try
        {
            var line = rawLines[y];

            Logger.Information("NON MATCHING AT: X{X}, Y{Y}", x, y);
            Logger.Information("Non matching p/n was: {Pn}", GetFullGearNumberAt(x, line));

            Logger.Information("Line above: -- {Line1}", rawLines[y - 1]);
            Logger.Information("Line at: ----- {Line1}", rawLines[y]);
            Logger.Information("Line below: -- {Line1}", rawLines[y + 1]);
            Logger.Information(" ");
        }
        catch
        {
            
        }
        
        
    }
    
    private static void PrintGearNumberForDebug(int x, int y, string[] rawLines, string fullGearNumber)
    {
        try
        {
            var line = rawLines[y];

            Logger.Information("MATCHING AT: X{X}, Y{Y}", x, y);
            Logger.Information("Matching p/n was: {Pn}", fullGearNumber);

            Logger.Information("Line above: -- {Line1}", rawLines[y - 1]);
            Logger.Information("Line at: ----- {Line1}", rawLines[y]);
            Logger.Information("Line below: -- {Line1}", rawLines[y + 1]);
            Logger.Information(" ");
        }
        catch
        {
            
        }
        
        
    }

    private static string GetFullGearNumberAt(int x, string line)
    {
        var gearNumberStartPos = GetStartPositionOfGearNumber(x, line);
        var gearNumberEndPos = GetEndPositionOfGearNumber(x, line);

        var fullGearNumberLength = gearNumberEndPos - gearNumberStartPos;

        var fullGearNumber = line.Substring(gearNumberStartPos, fullGearNumberLength + 1);
        
        Logger.Debug("In: {FullLine} | At X: {XPosition} | Char is: {CharAtX} | Full part number around that is: {FullGearNumber}", line, x, line[x], fullGearNumber);

        return fullGearNumber;
    }

    private static int GetEndPositionOfGearNumber(int searchStartX, string line)
    {
        Logger.Debug("Searching for end of part number in line: {Line}", line);
        
        var endPositionOfGearNumber = 0;
        
        for (var x = searchStartX; x < line.Length; x++)
        {
            var currentCharacter = line[x];

            Logger.Debug("At X:{XPosition} char is: {CharAtX}", x, currentCharacter);
            
            if (char.IsDigit(currentCharacter))
            {
                endPositionOfGearNumber = x;
            }
            else
            {
                break;
            }
        }

        return endPositionOfGearNumber;
    }

    private static int GetStartPositionOfGearNumber(int searchStartX, string line)
    {
        Logger.Debug("Searching for start of part number in line: {Line}", line);
        
        var startPositionOfGearNumber = -1;

        for (var x = searchStartX; x >= 0; x--)
        {
            var currentCharacter = line[x];

            Logger.Debug("At X:{XPosition} char is: {CharAtX}", x, currentCharacter);
            
            if (char.IsDigit(currentCharacter))
            {
                Logger.Debug("Char is digit, so start position updating to: {X}", x);
                startPositionOfGearNumber = x;
            }
            else
            {
                break;
            }
        }

        return startPositionOfGearNumber;
    }

    private static Point GetAdjacentSymbolPosition(int gearNumberStartPos, int y, int gearNumberLength, List<char> validSymbols,
        string[] rawLines)
    {
        var leftmostPositionToSearch = gearNumberStartPos - 1;
        var rightmostPositionToSearch = gearNumberStartPos + gearNumberLength + 1;
        var topLineToSearch = y - 1;
        var bottomLineToSearch = y + 1;
        
        Logger.Debug("Unchecked lines to search, top: {TopVal}, bottom: {BotVal}, left: {LeftVal}, right: {RightVal}", topLineToSearch, bottomLineToSearch, leftmostPositionToSearch, rightmostPositionToSearch);
        
        // Make sure that's not off the bounds of the strings we're searching
        if (topLineToSearch < 0) topLineToSearch = 0;
        if (bottomLineToSearch > rawLines.Length - 1) bottomLineToSearch = rawLines.Length - 1;
        if (leftmostPositionToSearch < 0) leftmostPositionToSearch = 0;
        if (rightmostPositionToSearch > rawLines[bottomLineToSearch].Length - 1) rightmostPositionToSearch = rawLines[bottomLineToSearch].Length - 1;

        Logger.Debug("Safe lines to search, top: {TopVal}, bottom: {BotVal}, left: {LeftVal}, right: {RightVal}", topLineToSearch, bottomLineToSearch, leftmostPositionToSearch, rightmostPositionToSearch);
        
        // Logger.Debug("Search lines:");
        // Logger.Debug("{FirstLine}", rawLines[topLineToSearch]);
        // Logger.Debug("{SecondLine}", rawLines[topLineToSearch + 1]);
        // Logger.Debug("{ThirdLine}", rawLines[bottomLineToSearch]);
        // Logger.Debug("About to search at position: X{X}, Y{Y}", x, y);
        
        for (var searchY = topLineToSearch; searchY <= bottomLineToSearch; searchY++)
        {
            for (var searchX = leftmostPositionToSearch; searchX <= rightmostPositionToSearch; searchX++)
            {
                var charToCheck = rawLines[searchY][searchX];
                
                Logger.Debug("Checking symbol: {CharToCheck} AT: X{X}, Y{Y}", charToCheck, searchX, searchY);

                if (!validSymbols.Contains(charToCheck)) continue;
                
                Logger.Debug("Valid symbol found!");
                
                return new Point(searchX, searchY);
            }
        }

        return new Point(-1, -1);
    }
}