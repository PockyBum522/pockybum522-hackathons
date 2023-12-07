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
        
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);
        
        var seedNumbers = GetSeedNumbers(rawLines);

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

        var valuesToMap = seedNumbers;
        
        foreach (var headerString in mapHeaderStrings)
        {
            valuesToMap = MapValuesWithHeader(headerString, valuesToMap, rawLines);
            
            Logger.Debug("After mapping with {HeaderString}, new values are: {@CurrentValues}", headerString, valuesToMap);
        }
        
        var valuesForAnswer = valuesToMap.ToList();
        valuesForAnswer.Sort();
        
        Logger.Information("Answer: {@Answer}", valuesForAnswer);
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