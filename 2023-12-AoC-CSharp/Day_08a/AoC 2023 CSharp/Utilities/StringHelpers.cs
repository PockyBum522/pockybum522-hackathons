using Serilog;

namespace AoC_2023_CSharp.Utilities;

public static class StringHelpers
{
    public static string[] SplitAndDropFirst(this string splitString, string splitOn)
    {
        var firstSplit = splitString.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

        var returnSplitElements = new List<string>();
        
        for (var i = 1; i < firstSplit.Length; i++)
        {
            returnSplitElements.Add(firstSplit[i]);
        }

        return returnSplitElements.ToArray();
    }
}