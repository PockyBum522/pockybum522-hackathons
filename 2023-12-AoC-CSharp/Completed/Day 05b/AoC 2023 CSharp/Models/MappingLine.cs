namespace AoC_2023_CSharp.Models;

public class MappingLine
{
    public MappingLine(string rawLine)
    {
        var lineValues = rawLine.Split(' ');

        DestinationRangeStart = ulong.Parse(lineValues[0]);
        SourceRangeStart = ulong.Parse(lineValues[1]);
        RangeLength = ulong.Parse(lineValues[2]);
    }
    
    public ulong DestinationRangeStart { get; } 
    
    public ulong SourceRangeStart { get; }

    public ulong SourceRangeMaximum => SourceRangeStart + RangeLength - 1;
    
    public ulong RangeLength { get; }

    public long ModifyingAmount => (long)DestinationRangeStart - (long)SourceRangeStart;
}