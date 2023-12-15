using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Debug()
            .CreateLogger(); 
    
    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData01
            .Split(",");

        var answerTotal = 0;
        
        foreach (var step in rawLines)
        {
            answerTotal += HashAlgorithm(step);

            //answerTotal += 1;
        }

            
        
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    public static int HashAlgorithm(string stringToHash)
    {
        
        // The current value starts at 0.
        var returnValue = 0;

        foreach (var letter in stringToHash)
        {
            var letterValue = (int)letter;

            returnValue += letterValue;

            Logger.Debug("letter value: {R}", returnValue);

            returnValue *= 17;
            
            Logger.Debug("After * 17: {R}", returnValue);

            returnValue = returnValue % 256;
            
            Logger.Debug("After mod: {R}", returnValue);
        }

        return returnValue;
        // The first character is H; its ASCII code is 72.

        // The current value increases to 72.
        // The current value is multiplied by 17 to become 1224.
        // The current value becomes 200 (the remainder of 1224 divided by 256).

        // The next character is A; its ASCII code is 65.
        // The current value increases to 265.
        // The current value is multiplied by 17 to become 4505.
        // The current value becomes 153 (the remainder of 4505 divided by 256).
        // The next character is S; its ASCII code is 83.
        // The current value increases to 236.
        // The current value is multiplied by 17 to become 4012.
        // The current value becomes 172 (the remainder of 4012 divided by 256).
        // The next character is H; its ASCII code is 72.
        // The current value increases to 244.
        // The current value is multiplied by 17 to become 4148.
        // The current value becomes 52 (the remainder of 4148 divided by 256).



    }

    private static void HashLetter(char letter)
    {
        throw new NotImplementedException();
    }
}
