namespace AoC_2023_CSharp.Models;

public class MappingLine
{
    public MappingLine(string rawLine)
    {
        var lineValues = rawLine.Split(' ');

        DestinationRangeStart = long.Parse(lineValues[0]);
        SourceRangeStart = long.Parse(lineValues[1]);
        RangeLength = long.Parse(lineValues[2]);
    }
    
    public long DestinationRangeStart { get; } 
    
    public long SourceRangeStart { get; } 
    
    public long RangeLength { get; } 
}