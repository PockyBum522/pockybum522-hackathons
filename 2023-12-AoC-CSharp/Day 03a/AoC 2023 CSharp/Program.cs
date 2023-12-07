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

        var uniqueSymbols = GetUniqueSymbolsOnly(rawLines);

        var uniquePartNumbers = new List<string>();
        
        // For making sure a part number we're adding has not been just added
        var lastStartPosition = 0;
        var lastY = 0;
        
        for (var y = 0; y < rawLines.Length; y++)
        {
            var line = rawLines[y];
            
            for (var x = 0; x < line.Length; x++)
            {
                var currentCharacter = line[x];

                if (!char.IsDigit(currentCharacter)) continue;

                // Is a digit, let's check around the part number
                
                var fullPartNumber = GetFullPartNumberAt(x, line);

                var partNumberStartPos = GetStartPositionOfPartNumber(x, line);
                var partNumberEndPos = GetEndPositionOfPartNumber(x, line);
                var partNumberLength = partNumberEndPos - partNumberStartPos;
                
                if (!SymbolIsAdjacentToPartNumberAt(partNumberStartPos, y, partNumberLength, uniqueSymbols, rawLines))
                {
                    //PrintNonMatchingPartNumberForDebug(x, y, rawLines);
                    
                    lastStartPosition = partNumberStartPos;
                    lastY = y;
                    
                    continue;
                }
                
                // If we get here, then current character is a digit and there's a symbol adjacent to it
                if (lastStartPosition != partNumberStartPos ||
                    lastY != y)
                {
                    PrintPartNumberForDebug(x, y, rawLines, fullPartNumber);
                    uniquePartNumbers.Add(fullPartNumber);
                }
                
                lastStartPosition = partNumberStartPos;
                lastY = y;
            }
        }
        
        Logger.Information("Unique symbols found: {Symbols}", string.Join(", ", uniqueSymbols));
        
        Logger.Information("Valid part numbers were: {PartNumbers}", string.Join(", ", uniquePartNumbers));

        foreach (var partNumber in uniquePartNumbers)
        {
            totalCumulative += int.Parse(partNumber);
        }
        
        Logger.Information("Answer: {Total}", totalCumulative);
    }

    private static void PrintNonMatchingPartNumberForDebug(int x, int y, string[] rawLines)
    {
        try
        {
            var line = rawLines[y];

            Logger.Information("NON MATCHING AT: X{X}, Y{Y}", x, y);
            Logger.Information("Non matching p/n was: {Pn}", GetFullPartNumberAt(x, line));

            Logger.Information("Line above: -- {Line1}", rawLines[y - 1]);
            Logger.Information("Line at: ----- {Line1}", rawLines[y]);
            Logger.Information("Line below: -- {Line1}", rawLines[y + 1]);
            Logger.Information(" ");
        }
        catch
        {
            
        }
        
        
    }
    
    private static void PrintPartNumberForDebug(int x, int y, string[] rawLines, string fullPartNumber)
    {
        try
        {
            var line = rawLines[y];

            Logger.Information("MATCHING AT: X{X}, Y{Y}", x, y);
            Logger.Information("Matching p/n was: {Pn}", fullPartNumber);

            Logger.Information("Line above: -- {Line1}", rawLines[y - 1]);
            Logger.Information("Line at: ----- {Line1}", rawLines[y]);
            Logger.Information("Line below: -- {Line1}", rawLines[y + 1]);
            Logger.Information(" ");
        }
        catch
        {
            
        }
        
        
    }

    private static string GetFullPartNumberAt(int x, string line)
    {
        var partNumberStartPos = GetStartPositionOfPartNumber(x, line);
        var partNumberEndPos = GetEndPositionOfPartNumber(x, line);

        var fullPartNumberLength = partNumberEndPos - partNumberStartPos;

        var fullPartNumber = line.Substring(partNumberStartPos, fullPartNumberLength + 1);
        
        Logger.Debug("In: {FullLine} | At X: {XPosition} | Char is: {CharAtX} | Full part number around that is: {FullPartNumber}", line, x, line[x], fullPartNumber);

        return fullPartNumber;
    }

    private static int GetEndPositionOfPartNumber(int searchStartX, string line)
    {
        Logger.Debug("Searching for end of part number in line: {Line}", line);
        
        var endPositionOfPartNumber = 0;
        
        for (var x = searchStartX; x < line.Length; x++)
        {
            var currentCharacter = line[x];

            Logger.Debug("At X:{XPosition} char is: {CharAtX}", x, currentCharacter);
            
            if (char.IsDigit(currentCharacter))
            {
                endPositionOfPartNumber = x;
            }
            else
            {
                break;
            }
        }

        return endPositionOfPartNumber;
    }

    private static int GetStartPositionOfPartNumber(int searchStartX, string line)
    {
        Logger.Debug("Searching for start of part number in line: {Line}", line);
        
        var startPositionOfPartNumber = -1;

        for (var x = searchStartX; x >= 0; x--)
        {
            var currentCharacter = line[x];

            Logger.Debug("At X:{XPosition} char is: {CharAtX}", x, currentCharacter);
            
            if (char.IsDigit(currentCharacter))
            {
                Logger.Debug("Char is digit, so start position updating to: {X}", x);
                startPositionOfPartNumber = x;
            }
            else
            {
                break;
            }
        }

        return startPositionOfPartNumber;
    }

    private static bool SymbolIsAdjacentToPartNumberAt(int partNumberStartPos, int y, int partNumberLength, List<char> validSymbols,
        string[] rawLines)
    {
        var leftmostPositionToSearch = partNumberStartPos - 1;
        var rightmostPositionToSearch = partNumberStartPos + partNumberLength + 1;
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
                return true;
            }
        }

        return false;
    }

    private static List<char> GetUniqueSymbolsOnly(IEnumerable<string> rawLines)
    {
        var uniqueSymbols = new List<char>();
        
        foreach (var line in rawLines)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '.') continue;
                if (char.IsDigit(line[i])) continue;
                if (uniqueSymbols.Contains(line[i])) continue;
                
                // Otherwise:
                uniqueSymbols.Add(line[i]);
            }
        }

        return uniqueSymbols;
    }
}