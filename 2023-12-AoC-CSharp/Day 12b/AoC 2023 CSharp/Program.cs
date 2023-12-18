namespace AoC_2023_CSharp;

internal static class Program
{
    // ReSharper disable once InconsistentNaming because it's less annoying than having the same name as the class
    private static readonly ILogger _logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
 
    private static readonly Stopwatch ElapsedTotal = new();
    private static int _maxThreads = 30;

    public static async Task Main()
    {
        ElapsedTotal.Start();
        _logger.Information("Starting!");
        
        var rawLines = RawData.SampleData01
            .Split(Environment.NewLine);

        var answerTotal = 0;

        var tasksList = new List<Task<int>>();
        
        foreach (var line in rawLines)
        {
            var total = CalculateNumberOfValidArrangements(line);

            _logger.Information("Answer for line: {Line} *5 was: {Total}", line, total );
            
            // tasksList.Add(task);
            //     
            // await LimitAddingThreads(tasksList);
        }

        _logger.Information("Waiting for all tasks to finish");
        
        var tasksStillRunning = 0;

        var finishedTasksForRemoval = new List<Task<int>>();
        
        do
        {
            tasksStillRunning = 0;
            
            foreach (var task in tasksList)
            {
                if (task.IsCompleted)
                {
                    await task.ContinueWith( originalTask =>
                    {
                        if (originalTask.IsCanceled)
                            _logger.Information("Task was cancelled");
                    
                        else if (originalTask.IsFaulted)
                            _logger.Error("Exception in task: {Ex}", originalTask.Exception);
                    
                    });

                    answerTotal += await task;
                    
                    finishedTasksForRemoval.Add(task);
                }
                else
                {
                    tasksStillRunning++;
                }
            }

            foreach (var task in finishedTasksForRemoval)
            {
                tasksList.Remove(task);
            }
            
            await Task.Delay(1000);
        } 
        while (tasksStillRunning > 0);
        
        _logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        _logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        await Log.CloseAndFlushAsync();
        await Task.Delay(500);
    }

    private static ulong CalculateNumberOfValidArrangements(string line)
    {
        ulong lineTotal = 0;
                
        var springLine = line.Split(" ")[0];
        var numbersLine = line.Split(" ")[1];
                
        // If it's stupid and it works...
        springLine = springLine + springLine + springLine + springLine + springLine;
        numbersLine = $"{numbersLine},{numbersLine},{numbersLine},{numbersLine},{numbersLine}";
                
        var numbers = numbersLine.SplitGeneric<int>(",");

        var finalPermutationCount = GetFinalPermutation(springLine);

        _logger.Information("On line: {Line} 0/{Count}", line, finalPermutationCount);
        _logger.Information("*5 line: {Line}", springLine);
        _logger.Information("*5 numbers: {Numbers}", numbersLine);
                
        for (var i = 0; (ulong)i < finalPermutationCount + 1; i++)
        {
            var linePossibility = GetLinePermutationAt(springLine, i);

            // Parse each brute force group to see if it's correct
            if (LineValid(linePossibility, numbers))
            {
                lineTotal++;
            }
                    
            if (i % 1000000 == 0 && i != 0)
                _logger.Information("On line: {Line} {I}/{Count}", line, i, finalPermutationCount);
        }

        _logger.Information("Inside task, answer is: {Ans}", lineTotal);

        return lineTotal;
    }

    private static async Task LimitAddingThreads(List<Task<int>> tasksList)
    {
        await Task.Delay(10);
        
        var runningTasks = 0;

        do
        {
            runningTasks = 0;
            
            foreach (var task in tasksList)
            {
                if (!task.IsCompleted)
                    runningTasks++;
            }

            _logger.Information("Running threads at: {Count}", runningTasks);

            if (runningTasks > _maxThreads - 1)
                await Task.WhenAny(tasksList);
        }
        while (runningTasks > _maxThreads - 1);
    }

    private static ulong GetFinalPermutation(string springLine)
    {
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
        
        var result = (ulong)Math.Pow(2d, unknownPositionsCount);
        
        return result;
    }

    private static string GetLinePermutationAt(string springLine, int permutationCount)
    {
        var unknownPositionsCount = 0;
        
        foreach (var character in springLine)
        {
            if (character == '?')
            {
                unknownPositionsCount++;
            }
        }
        
        _logger.Verbose("SpringLine: {Line} | ? count: {UnknownPositionsCount}", springLine, unknownPositionsCount);

        var questionMarksFound = 0;
        var binaryString = "";
        var counter = permutationCount;

        var stringToAdd = "";
    
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
        
        //_logger.Information("Adding: {StringToAdd}", stringToAdd);
        
        return stringToAdd;
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

        _logger.Information("For line: {Line} test case: {TestNumbers} and parsed: {ParsedGroups}", line, numbers, parsedGroups);

        if (numbers.Length != parsedGroups.Count)
            return false;
        
        for (var i = 0; i < parsedGroups.Count; i++)
        {
            if (parsedGroups[i] != numbers[i])
            {
                return false;
            }
        }
        
        return true;
    }
}
