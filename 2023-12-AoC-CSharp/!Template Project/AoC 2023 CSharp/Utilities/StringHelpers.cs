using Serilog;

namespace AoC_2023_CSharp.Utilities;

public static class StringHelpers
{
    public static T[] SplitAndDropFirst<T>(this string splitString, string splitOn, char? trimChar = null)
    {
        var firstSplit = splitString.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

        var returnSplitElements = new List<T>();
        
        for (var i = 1; i < firstSplit.Length; i++)
        {
            var rawElement = firstSplit[i];

            if (trimChar is not null)
                rawElement = rawElement.Trim((char)trimChar);
            
            var converted = (T)Convert.ChangeType(rawElement, typeof(T));
            
            returnSplitElements.Add(converted);
        }

        return returnSplitElements.ToArray();
    }
}
