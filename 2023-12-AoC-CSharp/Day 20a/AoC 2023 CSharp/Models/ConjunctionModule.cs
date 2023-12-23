namespace AoC_2023_CSharp.Models;

public class ConjunctionModule : BaseModule
{
    public ConjunctionModule(string rawLine, BaseModule parent) : base(rawLine, parent)
    {
        CurrentState = false;           // Per the instructions, conjunction modules start low
    }
}