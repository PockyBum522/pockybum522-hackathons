namespace AoC_2023_CSharp.Models;

public class SeedRange
{
    public SeedRange(ulong seedStart, ulong seedRange)
    {
        Start = seedStart;
        Range = seedRange;
        End = seedStart + seedRange;
    }
    
    public ulong Start { get; }
    
    public ulong End { get; }
    
    public ulong Range { get; }
    
}