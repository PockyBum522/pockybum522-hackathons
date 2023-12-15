using AoC_2023_CSharp.Models;
using Serilog;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger();

    public static async Task Main()
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

        Logger.Debug("Seed ranges final: {@Ranges}", ranges);
        
        var results = new List<ulong>();
        
        var checkTasks = new List<Task>();
        
        var counter = 0;

        foreach (var range in ranges) 
        {
            checkTasks.Add(
                Task.Run(() => Task.FromResult(CheckRange(range, stepsArray, counter++))));
        }

        await Task.WhenAll(checkTasks);

        foreach (Task<ulong> checkTask in checkTasks)
        {
            results.Add(await checkTask);
        }

        results.Sort();

        Logger.Information("{@ReturnValues}", results);
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
            
            Logger.Information("Adding new seed range starting at: {Start} with length: {Length} ending at: {End}", newRange.Start, newRange.Range, newRange.End);
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

            if (lowestValue > mappedValue)
            {
                lowestValue = mappedValue;
                Logger.Information("New lowest value: {NewLowest}", lowestValue);
            }
        }

        return lowestValue;
    }

    private static ulong MapSingleValue(ulong valueToMap, Step[] steps)
    {
        Logger.Debug("STARTING TO MAP {ValueToMap}", valueToMap);

        var originalValue = valueToMap;
        
        long valueToMapLong = -1;
        
        foreach (var step in steps)
        {
            Logger.Debug("Running step {StepHeader}", step.Header);
            
            foreach (var mappingLine in step.MappingLines)
            {
                var sourceRangeMaximum = mappingLine.SourceRangeStart + mappingLine.RangeLength;
            
                // Is the number >= sourceRangeStart && <= sourceRangeStart + rangeLength (In any of the ranges given for this particular header)
                if (valueToMap >= mappingLine.SourceRangeStart &&
                    valueToMap <= mappingLine.SourceRangeMaximum)
                {
                    Logger.Debug("valueToMap now {ValueToMap}", valueToMap);
                    Logger.Debug("mappingLine.SourceRangeStart: {SourceStart}", mappingLine.SourceRangeStart);
                    Logger.Debug("mappingLine.SourceRangeStart: {SourceRangeMaximum}", mappingLine.SourceRangeMaximum);
                    Logger.Debug("mappingLine.SourceRangeStart: {DestinationRangeStart}", mappingLine.DestinationRangeStart);
                    Logger.Debug("mappingLine.SourceRangeStart: {RangeLength}", mappingLine.RangeLength);
                    Logger.Debug("mappingLine.SourceRangeStart: {ModAmount}", mappingLine.ModifyingAmount);
                    
                    if (valueToMap < long.MaxValue)
                    {
                        valueToMapLong = (long)valueToMap;
                    
                        valueToMapLong += mappingLine.ModifyingAmount;

                        valueToMap = (ulong)valueToMapLong;
                    }
                    else
                    {
                        Logger.Warning("TOO LONG! Start was {StartVal}", originalValue);
                        
                        throw new Exception("TOO LONG");
                    }
                    
                    Logger.Debug("valueToMap now {ValueToMap}", valueToMap);
                    
                    break;
                }
            }
        }
        
        // If we checked all the ranges, and it's not in any of them, keep it as the number
        if (valueToMapLong == -1) Logger.Warning("valueToMapLong was -1, value returned won't be right");
        
        try
        {
            checked
            {
                return (ulong)valueToMapLong;
            }
        }
        catch (OverflowException e)
        {
            Logger.Warning(e.Message);  // output: Arithmetic operation resulted in an overflow.
        }
        
        Logger.Warning("Overflow issue");  // output: Arithmetic operation resulted in an overflow.

        throw new Exception("Couldn't return correct value");
    }
}