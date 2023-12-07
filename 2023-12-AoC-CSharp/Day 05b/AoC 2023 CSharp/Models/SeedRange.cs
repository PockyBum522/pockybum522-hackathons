namespace AoC_2023_CSharp.Models;

public class SeedRange
{
    public SeedRange(long seedStart, long seedRange)
    {
        Start = seedStart;
        Range = seedRange;
        End = seedStart + seedRange;
    }
    
    public long Start { get; }
    
    public long End { get; }
    
    public long Range { get; }
    
}