using AoC_2023_CSharp.Models;
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
        
        var rawLines = RawData.ActualDataPart2
            .Split(Environment.NewLine);

        var time = ParseTime(rawLines);
        var distance = ParseDistance(rawLines);

        Logger.Information("Time: {@Times}", time);
        Logger.Information("Distance: {@Distances}", distance);
        
        var numberOfRecords = RunAllPossibleButtonTimes(time, distance);
        
        Logger.Information("Final answer: {Answer}", numberOfRecords);
    }

    private static long RunAllPossibleButtonTimes(long time, long distance)
    {
        var lastLogAt = 0;
        
        var margin = (long)100000; 
        var timesRecordBeaten = (long)0;
        
        for (var i = 0; i < time; i++)
        {
            var buttonPressedTime = (long)i;

            var thisBoat = new Boat(Logger, buttonPressedTime);

            thisBoat.RunRace(time);
    
            // Log every 1k
            if (i - lastLogAt > 10 && i != 0)
            {
                Logger.Information("i: {I}", i);
                Logger.Information("Boat at index beat old record by: {Distance} mm", thisBoat.Distance - distance);

                lastLogAt = i;
            }

            var thisBoatIsBeatingExistingRecordBy = thisBoat.Distance - distance;

            var skipAmount = Math.Abs(thisBoatIsBeatingExistingRecordBy) / 100000000;

            if (skipAmount < 1) skipAmount = 1;
            if (skipAmount > margin) skipAmount = margin;
            
            Logger.Debug("Skip amount is/would be: {SkipAmount}", skipAmount);
            
            // Handle skipping ahead when record is severely under beating the existing record by much more than margin
            if (thisBoatIsBeatingExistingRecordBy < (margin * 10000 * -1))
            {
                Logger.Debug("Severely under-beating record. Distance: {Distance} mm in the race lasting {RaceDuration}", thisBoat.Distance, time);
                Logger.Debug("Boat beat old record by: {Distance} mm", thisBoat.Distance - distance);
                
                i += (int)skipAmount - 1; // -1 as the for loop will add one next loop to make it even
                
                continue;
            }
            
            // Handle skipping ahead when record is beaten by much more than margin
            if (thisBoatIsBeatingExistingRecordBy > (margin * 10000))
            {
                Logger.Debug("Adding new record! Distance: {Distance} mm in the race lasting {RaceDuration}", thisBoat.Distance, time);
                Logger.Debug("Boat beat old record by: {Distance} mm", thisBoat.Distance - distance);
                
                timesRecordBeaten += skipAmount;
                
                i += (int)skipAmount - 1; // -1 as the for loop will add one next loop to make it even
                
                continue;
            }
            
            if (thisBoat.Distance > distance)
            {
                Logger.Debug("Not skipping, incrementing by 1");
                Logger.Debug("Adding new record! Distance: {Distance} mm in the race lasting {RaceDuration}", thisBoat.Distance, time);
                timesRecordBeaten++;
            }
        }

        return timesRecordBeaten;
    }

    private static long ParseTime(string[] rawLines)
    {
        long time = -1;
            
        foreach (var line in rawLines)
        {
            if (!line.StartsWith("Time: ")) continue;
            
            var timesStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (var i = 1; i < timesStrings.Length; i++)
            {
                time = long.Parse(timesStrings[i]);
            }
                
            Logger.Debug("{@TimesStrings}", timesStrings);
        }

        return time;
    }
    
    private static long ParseDistance(string[] rawLines)
    {
        long distance = -1;
            
        foreach (var line in rawLines)
        {
            if (!line.StartsWith("Distance: ")) continue;
            
            var distancesStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
            for (var i = 1; i < distancesStrings.Length; i++)
            {
                distance = long.Parse(distancesStrings[i]);
            }
                
            Logger.Debug("{@DistStrings}", distancesStrings);
        }

        return distance;
    }
}