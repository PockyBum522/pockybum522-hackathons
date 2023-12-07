using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Utilities;

public static class StringHelpers
{
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    
    // ReSharper disable once InconsistentNaming because it would be public if every actually used
    private static ILogger? LoggerToUse;  // Make this public and set this if debugging, but should largely be unnecessary
    
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
    
    public static T[] SplitGeneric<T>(this string splitString, string splitOn, bool dropFirstElement = true, char? trimChar = null)
    {
        LoggerToUse?.Verbose("In method: {ThisMethodName}", nameof(SplitGeneric));
        LoggerToUse?.Verbose("Input string: {SplitString}", splitString);

        var firstSplit = splitString.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

        var returnSplitElements = new List<T>();

        var startAt = 0;

        if (dropFirstElement)
            startAt = 1;
        
        for (var i = startAt; i < firstSplit.Length; i++)
        {
            var rawElement = firstSplit[i];

            LoggerToUse?.Verbose("firstSplit: {@FirstSplit}", firstSplit);
            LoggerToUse?.Verbose("this line (rawElement): {ThisLine}", rawElement);
            
            if (trimChar is not null)
                rawElement = rawElement.Trim((char)trimChar);
            
            var converted = (T)Convert.ChangeType(rawElement, typeof(T));
            
            returnSplitElements.Add(converted);
        }
        
        LoggerToUse?.Debug("converted list: {@ConvertedObject}", returnSplitElements);

        return returnSplitElements.ToArray();
    }
}
