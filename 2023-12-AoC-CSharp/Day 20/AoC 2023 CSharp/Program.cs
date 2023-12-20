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
            
            
            //answerTotal += 1;
        }

        _logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        _logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        await Log.CloseAndFlushAsync();
        await Task.Delay(500);
    }
}
