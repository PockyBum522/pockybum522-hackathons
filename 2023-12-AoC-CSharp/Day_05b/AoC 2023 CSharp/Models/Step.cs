using System.Drawing;
using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Models;

public class Step
{
    private readonly ILogger _logger;

    public Step(ILogger logger, string header, string[] rawLines)
    {
        Header = header;
        _logger = logger;
        MappingLines = ParseMappingDataLines(header, rawLines);
        
        _logger.Debug("Mapping lines for {Header} are: {@MappingLines}", header, MappingLines);
    }
    
    public MappingLine[] MappingLines { get; }
    
    public string Header { get; }
    
    private static MappingLine[] ParseMappingDataLines(string headerString, string[] rawLines)
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
        
        return returnMappingDataLines.ToArray();
    }
}