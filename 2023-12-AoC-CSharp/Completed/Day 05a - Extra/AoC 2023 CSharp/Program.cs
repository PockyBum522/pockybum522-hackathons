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
        
        var rawLines = RawData.SampleData01
            .Split(Environment.NewLine);
        
        const int seedNumber = 79;
        
        // Seed 79, seed-to-soil map row 1 and 2
        // DebugOutputFullMappingDataRanges(50, 98, 2, seedNumber);
        // DebugOutputFullMappingDataRanges(52, 50, 48, seedNumber);
        
        // soil-to-fertilizer map lines:

        var mappingDataLines = ParseMappingDataLines("soil-to-fertilizer", rawLines);
        Logger.Debug("Data lines for {HeaderString}: {@DataLines}", "soil-to-fertilizer", mappingDataLines);

        var mappedValues = new List<long>();

        var outString = "{";
        
        for (int i = 0; i < 99; i++)
        {
            outString += $", {i}: ";
            outString += MapSingleValue(i, mappingDataLines);
        }

        outString += "}";
        
        Logger.Information(outString);
        
        // DebugOutputFullMappingDataRanges(0, 15, 37, seedNumber);  
        // DebugOutputFullMappingDataRanges(37, 52, 2, seedNumber);
        // DebugOutputFullMappingDataRanges(39, 0, 15, seedNumber);

        
        // fertilizer-to-water map lines:
        // DebugOutputFullMappingDataRanges(49, 53, 8, seedNumber);
        // DebugOutputFullMappingDataRanges(0, 11, 42, seedNumber);
        // DebugOutputFullMappingDataRanges(42, 0, 7, seedNumber);
        // DebugOutputFullMappingDataRanges(57, 7, 4, seedNumber);
        
        
        // water-to-light map lines:
        DebugOutputFullMappingDataRanges(88, 18, 7, seedNumber);
        DebugOutputFullMappingDataRanges(18, 25, 70, seedNumber);
        //
        // light-to-temperature map lines:
        // DebugOutputFullMappingDataRanges(45, 77, 23, seedNumber);
        // DebugOutputFullMappingDataRanges(81, 45, 19, seedNumber);
        // DebugOutputFullMappingDataRanges(68, 64, 13, seedNumber);
        
        // temperature-to-humidity map lines:
        // DebugOutputFullMappingDataRanges(0, 69, 1, seedNumber);
        // DebugOutputFullMappingDataRanges(1, 0, 69, seedNumber);
        
        // humidity-to-location map lines:
        // DebugOutputFullMappingDataRanges(60, 56, 37, seedNumber);
        // DebugOutputFullMappingDataRanges(56, 93, 4, seedNumber);
    }

    private static long DebugOutputFullMappingDataRanges(int destinationStart, int sourceStart, int rangeLength, long incomingNumber)
    {
        var sourceRangeNumbers = new List<long>();
        var destinationRangeNumbers = new List<long>();
        
        // Build all source range numbers in the range
        var sourceRange = sourceStart + rangeLength;
        
        for (var i = sourceStart; i < sourceRange; i++)
        {
            sourceRangeNumbers.Add(i);
        }
        
        // Build all destination range numbers in the range
        var destinationRange = destinationStart + rangeLength;
        
        for (var i = destinationStart; i < destinationRange; i++)
        {
            destinationRangeNumbers.Add(i);
        }

        long mappedNumber = -1;
        for (var i = 0; i < sourceRangeNumbers.Count; i++)
        {
            // Find what element of sourceMapNumbers matches the incoming number we want to map
            if (sourceRangeNumbers[i] == incomingNumber)
            {
                // Then get the matching (mapped to) number in the destination range we built
                mappedNumber = destinationRangeNumbers[i];
            }
        }

        Logger.Information("Incoming number to map: {IncomingToMap}", incomingNumber);
        Logger.Information("For mapping data line: {DestinationStart} {SourceStart} {RangeLength}", destinationStart, sourceStart, rangeLength);
        Logger.Information("Source (incoming) numbers range: --------------------- {IncomingNumbersMap}", sourceRangeNumbers);    
        Logger.Information("Destination (what to map source to) numbers range: --- {DestinationNumbersMap}", destinationRangeNumbers);
        Logger.Information("Number after being mapped: {MappedNumber}", mappedNumber);
        
        return mappedNumber;
    }

    private static long[] GetSeedNumbers(string[] rawLines)
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

        var convertedSeedNumbers = new List<long>();

        foreach (var seedNumberString in seedNumbers)
        {
            convertedSeedNumbers.Add(long.Parse(seedNumberString));
        }

        return convertedSeedNumbers.ToArray();
    }

    private static long[] MapValuesWithHeader(string headerString, long[] currentValuesToMap, string[] rawLines)
    {
        var mappingDataLines = ParseMappingDataLines(headerString, rawLines);
        Logger.Debug("Data lines for {HeaderString}: {@DataLines}", headerString, mappingDataLines);

        var mappedValues = new List<long>();

        foreach (var valueToMap in currentValuesToMap)
        {
            var mappedValue = MapSingleValue(valueToMap, mappingDataLines);
            
            Logger.Debug("About to add mapped value: {MappedVal}", mappedValue);
            
            mappedValues.Add(mappedValue);    
        }

        return mappedValues.ToArray();
    }

    private static long MapSingleValue(long valueToMap, List<MappingLine> mappingDataLines)
    {
        foreach (var mappingLine in mappingDataLines)
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

                return valueToMap + mapModifyingAmount;
            }
        }
        
        // If we checked all the ranges, and it's not in any of them, keep it as the number
        return valueToMap;
    }

    private static List<MappingLine> ParseMappingDataLines(string headerString, string[] rawLines)
    {
        var foundHeaderStringLine = false;
        var returnMappingDataLines = new List<MappingLine>();
        
        // Each line after the header string but before the next header is mapping data
        foreach (var line in rawLines)
        {
            if (foundHeaderStringLine)
            {
                if (string.IsNullOrWhiteSpace(line)) break;
                
                // Start grabbing values
                returnMappingDataLines.Add(new MappingLine(line));
            }
            
            if (!line.StartsWith(headerString)) continue;

            // Otherwise, found it:
            foundHeaderStringLine = true;
        }

        if (returnMappingDataLines.Count == 0)
            throw new Exception($"Could not get mapping data lines for {headerString}");
        
        return returnMappingDataLines;
    }
}