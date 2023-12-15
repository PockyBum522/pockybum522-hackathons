using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger();

    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        List<int> times = new List<int>();
        List<int> distances = new List<int>();
        
        foreach (var line in rawLines)
        {
            if (line.StartsWith("Time: "))
            {
                var timesStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (var i = 1; i < timesStrings.Length; i++)
                {
                    times.Add(int.Parse(timesStrings[i]));
                }
                
                Logger.Debug("{@TimesStrings}", timesStrings);
            }
            
            if (line.StartsWith("Distance: "))
            {
                var distancesStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
                for (var i = 1; i < distancesStrings.Length; i++)
                {
                    distances.Add(int.Parse(distancesStrings[i]));
                }
                
                Logger.Debug("{@DistStrings}", distancesStrings);
            }
        }

        var answer = 0;
        
        for (var i = 0; i < times.Count; i++)
        {
            var newNumberOfRecords = RunAllPossibleButtonTimes(i, times, distances);
            
            Logger.Debug("Records you got: {@Records}", newNumberOfRecords);
            
            if (answer > 0)
                answer *= RunAllPossibleButtonTimes(i, times, distances);
            else
                answer = RunAllPossibleButtonTimes(i, times, distances);
        }
        
        Logger.Information("Final answer: {Answer}", answer);
    }

    private static int RunAllPossibleButtonTimes(int whichRace, List<int> times, List<int> distances)
    {
        var timesRecordBeaten = 0;
        
        for (var i = 0; i < times[whichRace] - 1; i++)
        {
            var buttonPressedTime = i;

            var thisBoat = new Boat(Logger, buttonPressedTime);

            thisBoat.RunRace(times[whichRace]);

            if (thisBoat.Distance > distances[whichRace])
            {
                Logger.Debug("Adding new record! Distance: {Distance} mm in the race lasting {RaceDuration}", thisBoat.Distance, times[whichRace]);
                timesRecordBeaten++;
            }
        }

        return timesRecordBeaten;
    }
}