using AoC_2023_CSharp.Utilities;
using Microsoft.VisualBasic;
using Serilog;

namespace AoC_2023_CSharp.Models;

public class DataLine
{
    private readonly ILogger? _logger;

    public DataLine(string rawLine, ILogger? logger = null)
    {
        _logger = logger;
        
        var trimmedHeader = rawLine.GetLineHeader<string>("=").Trim();

        Header = trimmedHeader;

        // Kill the header and trailing ')' in the raw string
        var rawValuesOnly = 
            rawLine.Replace($"{trimmedHeader} = (", "")
            .Replace(")", "");

        var splitValues = rawValuesOnly.SplitGeneric<string>(",");

        LeftValue = splitValues[0].Trim();

        RightValue = splitValues[1].Trim();
        
        _logger?.Debug("From rawLine: {RawLine}, creating: {@DataLine}", rawLine, this);
    }
    
    public string Header { get; }
    
    public string LeftValue { get; }
    
    public string RightValue { get; }

    public string FindNextHeaderValue(char commandChar)
    {
        if (commandChar == 'L') return LeftValue;
        if (commandChar == 'R') return RightValue;

        throw new Exception($"Invalid character for passed command: {commandChar}");
    }
}
