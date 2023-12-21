namespace AoC_2023_CSharp.Models;

public class FlipFlopModule : BaseModule
{
    public FlipFlopModule(string rawLine, BaseModule parent) : base(rawLine, parent)
    {
        CurrentState = null;
    }
}