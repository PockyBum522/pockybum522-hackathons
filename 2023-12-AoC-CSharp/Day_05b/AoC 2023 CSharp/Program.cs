using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger();

    private static long _lowestLocation;

    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);
        
        var mapHeaderStrings = new List<string>()
        {
            "seed-to-soil",
            "soil-to-fertilizer",
            "fertilizer-to-water",
            "water-to-light",
            "light-to-temperature",
            "temperature-to-humidity",
            "humidity-to-location"
        };

        var steps = new List<Step>();
        
        foreach (var headerString in mapHeaderStrings)
        {
            steps.Add(
                new Step(Logger, headerString, rawLines));
        }

        var stepsArray = steps.ToArray();

        var ranges = new List<SeedRange> ();
        
        ranges.Add(new SeedRange(1514493331, 295250933));
        ranges.Add(new SeedRange(3793791524, 105394212));
        ranges.Add(new SeedRange(828589016, 654882197)); 
        ranges.Add(new SeedRange(658370118, 49359719)); 
        ranges.Add(new SeedRange(4055197159, 59237418));
        ranges.Add(new SeedRange(314462259, 268880047)); 
        ranges.Add(new SeedRange(2249227634, 74967914)); 
        ranges.Add(new SeedRange(2370414906, 38444198)); 
        ranges.Add(new SeedRange(3291001718, 85800943)); 
        ranges.Add(new SeedRange(2102534948, 5923540));
        
        // ranges.Add(new SeedRange(79, 14));
        // ranges.Add(new SeedRange(55, 13));

        _lowestLocation = long.MaxValue;
        
        foreach (var range in ranges)
        {
            CheckRange(range, stepsArray);
        }

        Logger.Information("Answer: {@Answer}", _lowestLocation);
    }

    private static void CheckRange(SeedRange range, Step[] steps)
    {
        Logger.Information("Starting to process {SeedRange} entries", range.Range);
        
        for (var i = range.Start; i < range.End; i++)
        {
            var mappedValue = MapSingleValue(i, steps);

            if (mappedValue < _lowestLocation)
            {
                _lowestLocation = mappedValue;

                Logger.Information("New lowest location! {LowestLocation}", _lowestLocation);
            }
            
            if (i % 1000000 == 0)
                Logger.Information("i: {I}", i);
        }
    }
    
    private static long MapSingleValue(long valueToMap, Step[] steps)
    {
        Logger.Debug("STARTING TO MAP {ValueToMap}", valueToMap);
        
        foreach (var step in steps)
        {
            Logger.Debug("Running step {StepHeader} with: {@MappingLines}", step.Header, step.MappingLines);
            
            foreach (var mappingLine in step.MappingLines)
            {
                var sourceRangeMaximum = mappingLine.SourceRangeStart + mappingLine.RangeLength;
            
                // Is the number >= sourceRangeStart && <= sourceRangeStart + rangeLength (In any of the ranges given for this particular header)
                if (valueToMap >= mappingLine.SourceRangeStart &&
                    valueToMap <= sourceRangeMaximum)
                {
                    // If so, map using range:
                    // D - S = val to apply to incoming num
                    // 52 - 50 = +2
                    var mapModifyingAmount = mappingLine.DestinationRangeStart - mappingLine.SourceRangeStart;

                    Logger.Debug("valueToMap now {ValueToMap}", valueToMap);
                    Logger.Debug("mappingLine.SourceRangeStart: {SourceStart} - sourceRangeMaximum {SourceRangeMaximum}, mappingLine.DestinationRangeStart {DestinationStart}", mappingLine.SourceRangeStart, sourceRangeMaximum, mappingLine.DestinationRangeStart);
                    Logger.Debug("mapModifyingAmount now {MapModifyingAmount}", mapModifyingAmount);

                    valueToMap += mapModifyingAmount;

                    break;
                }
                
                Logger.Debug("valueToMap now {ValueToMap}", valueToMap);
            }
        }
        
        // If we checked all the ranges, and it's not in any of them, keep it as the number
        return valueToMap;
    }
}