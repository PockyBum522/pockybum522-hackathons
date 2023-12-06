using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    public static ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawString = RawData.ActualData;

        // Number of unique chars that mean start of packet
        const int requiredUniqueChars = 14;

        var answer = FindPositionAfterUniqueChars(rawString, requiredUniqueChars);
        
        Logger.Debug("First position after X number of unique chars: {FinalAnswer}", answer);
    }

    private static int FindPositionAfterUniqueChars(string rawString, int requiredUniqueChars)
    {
        Logger.Debug("Raw string to check: {RawString}", rawString);
        
        for (var i = 0; i < rawString.Length; i++)
        {
            var charsToCheck = rawString.Substring(i, requiredUniqueChars);

            var areAllUnique = CheckIfAllCharsUnique(charsToCheck);
            
            Logger.Debug("Checking chars: {CharsToCheck} | Are all unique? {AreAllUnique}", charsToCheck, areAllUnique);

            if (areAllUnique) return i + requiredUniqueChars;
        }

        return -1;
    }

    private static bool CheckIfAllCharsUnique(string charsToCheck)
    {
        var seenCharsList = new List<char>();
        
        foreach (var charToCheck in charsToCheck)
        {
            if (seenCharsList.Contains(charToCheck)) return false;
            
            seenCharsList.Add(charToCheck);
        }

        return true;
    }
}