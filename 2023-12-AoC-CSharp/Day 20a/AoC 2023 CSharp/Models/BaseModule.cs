namespace AoC_2023_CSharp.Models;

public class BaseModule
{
    public BaseModule(string rawLine, BaseModule? parent)
    {
        CurrentState = null;

        RawLine = rawLine;
        
        Id = rawLine.SplitGeneric<string>("->", 0, ' ')[0].TrimSymbols();
        
        if (parent is null)
        {
            if (this is not BroadcasterModule)
                throw new Exception("Parent may only be null on BroadcasterModule and no other");
        }
        else
        {
            Parent = parent;    
        }
    }
    
    public bool? CurrentState { get; set; }
    
    public string Id { get; }
    
    public BaseModule Parent { get; }
    public BaseModule[] Destinations { get; set; }
    
    public string RawLine { get; }

    private static void LoadModuleDestinations(string rawLine, string[] rawLines, BaseModule moduleToAttachTo)
    {
        
    }

    public int GetLevel()
    {
        var level = 1;

        var checkModule = Parent;

        while (checkModule is not BroadcasterModule)
        {
            checkModule = checkModule.Parent;

            level++;
        }

        return level;
    }
}