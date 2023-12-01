using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    public static ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var dataLines = RawData.ActualData
            .Split(Environment.NewLine);

        var total = 0;

        for (var i = 0; i < dataLines.Length; i += 3)
        {
            var groupLines = new[] { dataLines[i], dataLines[i + 1], dataLines[i + 2] };

            var commonCharacter = GetCommonCharacter(groupLines); 
            
            var priorityScore = GetPriorityScore(commonCharacter);
            
            total += priorityScore;
            
            Logger.Debug("Group lines: {RawLine}", string.Join(" | ", groupLines));
            Logger.Debug("Common char: {CommonChar}", commonCharacter);
            Logger.Debug("Priority score: {PriorityScore}", priorityScore);
        }

        Logger.Information("Total: {Total}", total);
    }
    
    private static char GetCommonCharacter(string[] groupLines)
    {
        foreach (var character in groupLines[0])
        {
            if (groupLines[1].Contains(character) &&
                groupLines[2].Contains(character))
            {
                return character;
            }
        }

        throw new Exception($"Couldn't find common character between: {groupLines[0]}, {groupLines[1]}, and {groupLines[0]}");
    }

    private static int GetPriorityScore(char commonCharacter)
    {
        // Lowercase item types a through z have priorities 1 through 26.
        // Uppercase item types A through Z have priorities 27 through 52.

        var charValue = (int)commonCharacter;

        // A = 65
        // a = 97

        if (char.IsLower(commonCharacter))
        {
            charValue -= 96;
        }
        else
        {
            charValue -= 38;
        }
        
        // Logger.Debug("Char: {CharacterToCheck} raw numeric value: {RawValue}", commonCharacter, charValue);

        return charValue;
    }
}