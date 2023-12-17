namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
 
    private static readonly Stopwatch ElapsedTotal = new();
    
    public static async Task Main()
    {
        ElapsedTotal.Start();
        Logger.Information("Starting!");
        
        var rawLines = RawData.SampleData01
            .Split(Environment.NewLine);

        var answerTotal = 0;
        
        foreach (var line in rawLines)
        {
            
            
            //answerTotal += 1;
        }

        Logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        await Log.CloseAndFlushAsync();
        await Task.Delay(500);
    }
}
