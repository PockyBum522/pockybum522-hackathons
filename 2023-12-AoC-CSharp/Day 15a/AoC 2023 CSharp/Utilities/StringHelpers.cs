using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Utilities;

public static class StringHelpers
{
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    
    // ReSharper disable once InconsistentNaming because it would be public if every actually used
    private static ILogger? LoggerToUse;  // Make this public and set this if debugging, but should largely be unnecessary
    
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
    
    /// <summary>
    /// Splits an incoming string of elements that are splittable on splitOn into a typed array
    ///
    /// Example:
    /// "34 45 67 78 90".SplitGeneric&lt;int>(" ");
    /// 
    /// Would return int[] containing the ints: 34, 45, 67, 78, 90. 
    /// </summary>
    /// <param name="splitString">What string should be split</param>
    /// <param name="splitOn">What string to split on (Delimiter)</param>
    /// <param name="dropFirstElements">How many elements to drop from the front. 0 by default</param>
    /// <param name="trimChar">Character to trim on each split element before converting to a typed entry in the array</param>
    /// <typeparam name="T">Type to parse elements of the split as</typeparam>
    /// <returns>An array of type T containing converted elements from the original string</returns>
    public static T[] SplitGeneric<T>(this string splitString, string splitOn, int dropFirstElements = 0, char? trimChar = null)
    {
        LoggerToUse?.Verbose("In method: {ThisMethodName}", nameof(System.Reflection.MethodBase.GetCurrentMethod));
        LoggerToUse?.Verbose("Input string: {SplitString}", splitString);

        var firstSplit = splitString.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

        var returnSplitElements = new List<T>();
        
        for (var i = dropFirstElements; i < firstSplit.Length; i++)
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
    
    /// <summary>
    /// Splits a line first on ':' (by default) and then on spaces after that, to get header information from a line
    ///
    /// Example:
    /// "Card 56: 34 45 67 78 90 | 12 34 43 21 12 34".GetLineHeader&lt;int>(":", 1);
    /// 
    /// Would return 56, splitting first on ':' and then on spaces, dropping the first element of "Card 56" split on spaces
    /// </summary>
    /// <param name="splitString">Line of data string to operate on</param>
    /// <param name="splitOn">String or character (as string) to split on to get the header section. ':' by default</param>
    /// <param name="dropFirstElements">How many elements to drop to get the header information (if any) 0 by default</param>
    /// <param name="trimChar">Char to use to trim element strings before they are converted</param>
    /// <typeparam name="T">Type to convert the elements to, also type of returned array</typeparam>
    /// <returns>Type T of the header information</returns>
    /// <exception cref="Exception"></exception>
    public static T GetLineHeader<T>(this string splitString, string splitOn = ":", int dropFirstElements = 0, char? trimChar = null)
    {
        LoggerToUse?.Verbose("In method: {ThisMethodName}", nameof(System.Reflection.MethodBase.GetCurrentMethod));
        LoggerToUse?.Verbose("Input string: {SplitString}", splitString);

        var firstSplit = splitString.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)[0];
        
        var secondSplit = firstSplit.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (dropFirstElements > secondSplit.Length)
            throw new Exception($"dropFirstElements was: {dropFirstElements} but {splitString} doesn't seem to have that many elements");
    
        var rawElement = secondSplit[dropFirstElements];

        LoggerToUse?.Verbose("firstSplit: {@FirstSplit}", firstSplit);
        LoggerToUse?.Verbose("secondSplit: {@FirstSplit}", firstSplit);
        LoggerToUse?.Verbose("this line (rawElement): {ThisLine}", rawElement);
        
        if (trimChar is not null)
            rawElement = rawElement.Trim((char)trimChar);
        
        var converted = (T)Convert.ChangeType(rawElement, typeof(T));

        return converted;
    }
}
