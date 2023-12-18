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
        
        var rawLines = RawData.ActualData01
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
            var validPermutations = CountValidPermutations(linePossibilities, numbers);

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
        
        _logger.Verbose("SpringLine: {Line} | ? count: {UnknownPositionsCount}", springLine, unknownPositionsCount);

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
                
                _logger.Verbose("At char {I}: char is {Character} questionMarksFound = {QuestionMarksCount}",
                    i, character, questionMarksFound);
                
                if (character == '?')
                {
                    stringToAdd += binaryString[questionMarksFound] == '0' ? '.' : '#';

                    questionMarksFound++;
                    
                    continue;
                }

                stringToAdd += springLine[i];
            }

            questionMarksFound = 0;
            returnLines.Add(stringToAdd);
            _logger.Verbose("Adding: {StringToAdd}", stringToAdd);
            
            counter++;

            stringToAdd = "";
        }

        foreach (var line in returnLines)
        {
            _logger.Verbose("Line: {Line}", line);
        }
        
        return returnLines.ToArray();
    }

    private static int CountValidPermutations(string[] linePossibilities, int[] numbers)
    {
        var validCount = 0;

        foreach (var line in linePossibilities)
        {
            _logger.Debug("Comparing line: {Line} to validation numbers: {@Numbers}", line, numbers);
            
            if (LineValid(line, numbers))
            {
                _logger.Debug("Valid!");
                
                validCount++;
            }
        }

        return validCount;
    }

    private static bool LineValid(string line, int[] numbers)
    {
        var parsedGroups = new List<int>();

        var thisGroupSpringsCount = 0;
        
        foreach (var character in line)
        {
            if (character == '#')
            {
                thisGroupSpringsCount++;       
            }
            else
            {
                if (thisGroupSpringsCount != 0)
                    parsedGroups.Add(thisGroupSpringsCount);
                
                thisGroupSpringsCount = 0;
            }
        }
        
        // Handle when line ends
        if (thisGroupSpringsCount != 0)
            parsedGroups.Add(thisGroupSpringsCount);

        _logger.Debug("For line: {Line} test case: {TestNumbers} and parsed: {ParsedGroups}", line, numbers, parsedGroups);

        if (numbers.Length != parsedGroups.Count)
            return false;
        
        for (var i = 0; i < parsedGroups.Count; i++)
        {
            var group = parsedGroups[i];

            if (parsedGroups[i] != numbers[i])
            {
                return false;
            }
        }
        
        return true;
    }
}
