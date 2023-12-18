namespace AoC_2023_CSharp;

internal static class Program
{
    // ReSharper disable once InconsistentNaming because it's less annoying than having the same name as the class
    private static readonly ILogger _logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
 
    private static readonly Stopwatch ElapsedTotal = new();
    
    public static async Task Main()
    {
        ElapsedTotal.Start();
        _logger.Information("Starting!");
        
        var rawLines = RawData.SampleData01
            .Split(Environment.NewLine);

        var answerTotal = 0;
        
        
        
        foreach (var line in rawLines)
        {
            var springLine = line.Split(" ")[0];
            var numbersLine = line.Split(" ")[1];

            var numbers = numbersLine.SplitGeneric<int>(",");

            // Brute force any ???
            var linePossibilities = GetAllLinePermutations(springLine);

            // Parse each brute force group to see if it's correct
            var validPermutations = CountValidPermutations(linePossibilities);

            answerTotal += validPermutations;
        }

        _logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        _logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        await Log.CloseAndFlushAsync();
        await Task.Delay(500);
    }

    private static string[] GetAllLinePermutations(string springLine)
    {
        var returnLines = new List<string>();

        var unknownPositionsCount = 0;
        var finalBinaryString = "";
        
        foreach (var character in springLine)
        {
            if (character == '?')
            {
                unknownPositionsCount++;

                finalBinaryString += '1';
            }
        }

        var questionMarksFound = 0;
        var binaryString = "";
        var counter = 0;

        var stringToAdd = "";
        
        while (binaryString != finalBinaryString)
        {
            binaryString = Convert.ToString(counter, 2).PadLeft(unknownPositionsCount, '0');

            for (var i = 0; i < springLine.Length; i++)
            {
                var character = springLine[i];
                
                if (character == '?')
                {
                    stringToAdd += binaryString[questionMarksFound] == '0' ? '.' : '#';

                    questionMarksFound++;
                    
                    continue;
                }

                stringToAdd += springLine[i];
            }
            
            returnLines.Add(stringToAdd);

            stringToAdd = "";
        }

        _logger.Debug("");
        _logger.Debug("Strings to add: ");

        foreach (var line in returnLines)
        {
            _logger.Debug(line);
        }
        
        // ?###????????
        
        // Start with ten dots
        
        // .###........
        
        // Add that
        
        // First char flip to #
        
        // ####........
        
        // Add that
        
        // Then flip second char
        
        // Add
        
        // Now flip 3rd, do both above again

        return returnLines.ToArray();
    }

    private static int CountValidPermutations(string[] linePossibilities)
    {
        throw new NotImplementedException();
    }
}
