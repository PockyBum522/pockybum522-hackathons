namespace AoC_2023_CSharp.Models;

public class BroadcasterModule : BaseModule
{
    public BroadcasterModule(string rawLine, BaseModule? parent) : base(rawLine, parent)
    {
        CurrentState = null;
    }
}