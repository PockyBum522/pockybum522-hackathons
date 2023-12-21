using AoC_2023_CSharp.Models;
using Serilog.Core;

namespace AoC_2023_CSharp;

internal static class Program
{
    // ReSharper disable once InconsistentNaming because it's less annoying than having the same name as the class
    private static readonly ILogger _logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
 
    private static readonly Stopwatch ElapsedTotal = new();

    public static async Task Main()
    {
        ElapsedTotal.Start();
        _logger.Information("Starting!");
        
        var rawLines = RawData.ActualData01
            .Split(Environment.NewLine);

        var answerTotal = 0;

        // The broadcaster has to load all other modules properly as submodules when it is constructed
        var broadcaster = GetBroadcasterModule(rawLines);

        LoadDestinationModules(broadcaster, rawLines);
        
        // Load all children and subchildren
        WalkAllModules((curModule) => LoadDestinationModules(curModule, rawLines), broadcaster);
        
        // Let's check things out
        WalkAllModules(LogModuleTree, broadcaster);
        
        _logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        _logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        await Log.CloseAndFlushAsync();
        await Task.Delay(500);
    }

    private static void LoadDestinationModules(BaseModule currentModule, string[] rawLines)
    {
        var rawModules = new List<BaseModule>();
        
        foreach (var line in rawLines)
        {
            switch (line[0])
            {
                case RawData.FlipFlopModulePrefix:
                
                    rawModules.Add(new FlipFlopModule(line, currentModule));
                
                    break;
            
                case RawData.ConjunctionModulePrefix:

                    rawModules.Add(new ConjunctionModule(line, currentModule));

                    break;
            }
        }

        var destinationStrings = 
            currentModule.RawLine.SplitGeneric<string>("->", 0, ' ')[1]
                .SplitGeneric<string>(",", 0, ' ');
        
        var foundModules = new List<BaseModule>();
        
        foreach (var module in rawModules)
        {
            if (!destinationStrings.Contains(module.Id)) continue;
            
            foundModules.Add(module);
        }

        currentModule.Destinations = foundModules.ToArray();
    }

    private static void WalkAllModules(Action<BaseModule> actionOnWalk, BaseModule rootModule)
    {
        foreach (var module in rootModule.Destinations)
        {
            actionOnWalk.Invoke(module);
            
            WalkAllModules(actionOnWalk, module);
        }
    }
    
    private static void LogModuleTree(BaseModule currentModule)
    {
        _logger.Information("Level: [{Level}] In ID: {ModuleId}", currentModule.GetLevel(), currentModule.Id);
    }

    private static BaseModule GetBroadcasterModule(string[] rawLines)
    {
        BroadcasterModule? returnModule = null;
        
        foreach (var line in rawLines)
        {
            if (line[0] != RawData.BroadcasterModulePrefix) continue;

            returnModule = new BroadcasterModule(line, null);
        }
        
        return returnModule ?? throw new Exception("Could not find broadcaster line in raw data");
    }

    

    private static void WalkAllDestinationModules(BaseModule currentModule, List<BaseModule> rawModules)
    {
        foreach (var module in currentModule.Destinations)
        {
            AttachDestinationModules(module, rawModules);
            
            WalkAllDestinationModules(module, rawModules);
        }
    }

    private static void AttachDestinationModules(BaseModule currentModule, List<BaseModule> rawModules)
    {
        var destinationsFound = new List<BaseModule>();
        
        foreach (var destinationModule in currentModule.Destinations)
        {
            foreach (var rawModule in rawModules)
            {
                if (rawModule.Id != destinationModule.Id) continue;
                
                destinationsFound.Add(rawModule);
            }
        }

        currentModule.Destinations = destinationsFound.ToArray();
    }
}
