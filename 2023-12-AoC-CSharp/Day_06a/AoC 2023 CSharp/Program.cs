using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Verbose()
            .CreateLogger();

    private static List<int> _records;

    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.SampleData01
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

        _records = new List<int>();

        for (var i = 0; i < times.Count; i++)
        {
            RunAllPossibleButtonTimes(i, times, distances);
        }

        var answer = 0;
        
        foreach (var record in _records)
        {
            answer *= record;
        }
        
        Logger.Debug("Records you got: {@Records}", _records);
        Logger.Information("Final answer: {Answer}", answer);
    }

    private static void RunAllPossibleButtonTimes(int whichRace, List<int> times, List<int> distances)
    {
        for (var i = 0; i < times[whichRace] - 1; i++)
        {
            var buttonPressedTime = i;

            var thisBoat = new Boat(Logger, buttonPressedTime);

            thisBoat.RunRace(times[whichRace]);

            if (thisBoat.Distance > distances[whichRace])
            {
                Logger.Debug("Adding new record! Distance: {Distance} mm in the race lasting {RaceDuration}", thisBoat.Distance, times[whichRace]);
                _records.Add(thisBoat.Distance);
            }
        }
    }
}