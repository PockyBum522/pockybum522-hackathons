using System.Diagnostics;

namespace AoC_2023_CSharp.Utilities;

public static class StopwatchHelper
{
    public static string GetStopwatchFinalTimes(Stopwatch elapsedTotal)
    {
        elapsedTotal.Stop();

        var elapsedString = elapsedTotal.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        
        return $"Final total time elapsed hh:mm:ss.fff: ${elapsedString}";
    }
}