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

        var ranges = GetSeedRanges(rawLines);

        Logger.Information("Seed ranges final: {@Ranges}", ranges);
        
        _lowestLocation = long.MaxValue;
        
        foreach (var range in ranges) 
        {
            CheckRange(range, stepsArray);
        }

        Logger.Information("Answer: {@Answer}", _lowestLocation);
    }

    private static SeedRange[] GetSeedRanges(string[] rawLines)
    {
        var seedLineStartString = "seeds: ";
        
        var seedNumbers = new List<string>();
        
        foreach (var line in rawLines)
        {
            if (!line.ToLower().StartsWith(seedLineStartString)) continue;

            seedNumbers = line.Split(' ').ToList();
            
            // Get rid of the startString element
            seedNumbers.RemoveAt(0);                        
            
            Logger.Debug("Seed numbers are: {@SeedNumbers}", seedNumbers);
        }

        var convertedSeedNumbers = new List<SeedRange>();

        for (var i = 0; i < seedNumbers.Count; i+=2)
        {
            var seedNumberString = seedNumbers[i];
            var seedRangeString = seedNumbers[i + 1];
            
            convertedSeedNumbers.Add(
                new SeedRange(long.Parse(seedNumberString), long.Parse(seedRangeString)));
        }

        return convertedSeedNumbers.ToArray();
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

            var remainingEntries = range.End - i;
            
            if (i % 1000000 == 0)
                Logger.Information("Remaining in this batch: {I}", remainingEntries);
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