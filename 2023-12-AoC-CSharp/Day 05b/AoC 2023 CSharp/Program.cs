using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

// Attempts:

// 7873085 - wrong
// 0 - wrong
// 110359151 - wrong

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Debug()
            .CreateLogger();

    public static async Task Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.SampleData01
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

        Logger.Debug("Seed ranges final: {@Ranges}", ranges);
        
        var checkTasks = new List<Task>();

        MapSingleValue(463750354, stepsArray);
        
        var counter = 0;
        foreach (var range in ranges) 
        {
            checkTasks.Add(
                Task.Run(() => Task.FromResult(CheckRange(range, stepsArray, counter++))));
        }

        await Task.WhenAll(checkTasks);

        var results = new List<ulong>();
        
        foreach (Task<ulong> checkTask in checkTasks)
        {
            results.Add(await checkTask);
        }
        
        results.Sort();
        
        Logger.Information("{@ReturnValues}", results);

        //Logger.Information("Answer: {@Answer}", _lowestLocation);
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

            var newRange = new SeedRange(ulong.Parse(seedNumberString), ulong.Parse(seedRangeString));
            
            convertedSeedNumbers.Add(newRange);
            
            Logger.Debug("Adding new seed range starting at: {Start} with length: {Length} ", newRange.Start, newRange.Range);
        }

        return convertedSeedNumbers.ToArray();
    }

    private static ulong CheckRange(SeedRange range, Step[] steps, int batchNumber)
    {
        Logger.Information("Starting to process {SeedRange} entries", range.Range);

        var lowestValue = ulong.MaxValue;
        
        for (var i = range.Start; i <= range.End; i++)
        {
            ulong mappedValue = MapSingleValue(i, steps);

            if (mappedValue > 100 &&
                mappedValue < lowestValue)
            {
                lowestValue = mappedValue;

                Logger.Information("New lowest location in batch {BatchNum}! {LowestLocation}", batchNumber, lowestValue);
            }
            
            if (mappedValue == 7873085)         
            {
                lowestValue = mappedValue;

                Logger.Information("in batch {BatchNum}! Final value: {MappedValue}, StartValue was {I}", batchNumber, mappedValue, i);
            }
            
            var remainingEntries = range.End - i;
            
            if (i % 10000000 == 0)
                Logger.Information("Remaining in batch {BathNum}: {I}", batchNumber, remainingEntries);
        }

        return lowestValue;
    }

    private static ulong MapSingleValue(ulong valueToMap, Step[] steps)
    {
        Logger.Debug("STARTING TO MAP {ValueToMap}", valueToMap);

        long valueToMapLong = -1;
        
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
                    var mapModifyingAmount = (long)mappingLine.DestinationRangeStart - (long)mappingLine.SourceRangeStart;

                    Logger.Debug("valueToMap now {ValueToMap}", valueToMap);
                    Logger.Debug("mappingLine.SourceRangeStart: {SourceStart} | sourceRangeMaximum {SourceRangeMaximum} | mappingLine.DestinationRangeStart {DestinationStart} | rangeLength: {RangeLength}", mappingLine.SourceRangeStart, sourceRangeMaximum, mappingLine.DestinationRangeStart, mappingLine.RangeLength);
                    Logger.Debug("mapModifyingAmount now {MapModifyingAmount}", mapModifyingAmount);

                    if (valueToMap < long.MaxValue)
                    {
                        valueToMapLong = (long)valueToMap;
                    
                        valueToMapLong += mapModifyingAmount;

                        valueToMap = (ulong)valueToMapLong;
                    }
                    else
                    {
                        throw new Exception("TOO LONG");
                    }
                    
                    Logger.Debug("valueToMap now {ValueToMap}", valueToMap);
                    
                    break;
                }
            }
        }
        
        // If we checked all the ranges, and it's not in any of them, keep it as the number
        return (ulong)valueToMap;
    }
    
    // private static SeedRange[] GetSeedNumbers(string[] rawLines)
    // {
    //     var seedLineStartString = "seeds: ";
    //     
    //     var seedNumbers = new List<string>();
    //     
    //     foreach (var line in rawLines)
    //     {
    //         if (!line.ToLower().StartsWith(seedLineStartString)) continue;
    //
    //         seedNumbers = line.Split(' ').ToList();
    //         
    //         // Get rid of the startString element
    //         seedNumbers.RemoveAt(0);                        
    //         
    //         Logger.Debug("Seed numbers are: {@SeedNumbers}", seedNumbers);
    //     }
    //
    //     var convertedSeedNumbers = new List<SeedRange>();
    //
    //     foreach (var seedNumberString in seedNumbers)
    //     {
    //         convertedSeedNumbers.Add(
    //             new SeedRange(long.Parse(seedNumberString), 1));
    //     }
    //     return convertedSeedNumbers.ToArray();
    //
    // }
}