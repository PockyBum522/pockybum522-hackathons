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
        
        foreach (var line in dataLines)
        {
            // Split the string into halves
            var firstHalf = GetLineFirstHalf(line);
            var secondHalf = GetLineSecondHalf(line);
            
            var commonCharacter = GetCommonCharacter(firstHalf, secondHalf);

            var priorityScore = GetPriorityScore(commonCharacter);

            total += priorityScore;
            
            Logger.Debug("Raw line: {RawLine}", line);
            Logger.Debug("First half: {FirstHalf} \t {SecondHalf} \t Common char: {CommonChar}", firstHalf, secondHalf, commonCharacter);
            Logger.Debug("Priority score: {PriorityScore}", priorityScore);
        }

        Logger.Information("Total: {Total}", total);
    }
    
    private static string GetLineFirstHalf(string line)
    {
        var lineLength = line.Length;

        return line.Substring(0, lineLength / 2);
    }
    
    private static string GetLineSecondHalf(string line)
    {
        var lineLength = line.Length;

        return line.Substring(lineLength / 2);
    }
    
    private static char GetCommonCharacter(string firstHalf, string secondHalf)
    {
        foreach (var character in firstHalf)
        {
            if (secondHalf.Contains(character))
                return character;
        }

        throw new Exception($"Couldn't find common character between halves: {firstHalf} and {secondHalf}");
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