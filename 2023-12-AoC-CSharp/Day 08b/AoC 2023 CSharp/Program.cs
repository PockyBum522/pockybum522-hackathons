using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger();

    private static List<DataLine> _dataLines = new();

    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData01
            .Split(Environment.NewLine);
        
        var commandLine = rawLines[0];

        _dataLines = GetParsedDataLines(rawLines);

        var startPositions = new List<DataLine>();

        startPositions.Add(
            FindDataLineWithHeader("AAA"));

        var answer = 
            RunAllStartPositionsUntilEndCondition(startPositions, commandLine);

        Logger.Information("Answer: {AnswerTotal}", answer);
    }

    private static List<DataLine> GetParsedDataLines(string[] rawLines)
    {
        var dataLines = new List<DataLine>();
        
        for (var i = 2; i < rawLines.Length; i++)
        {
            var rawDataLine = rawLines[i];
            
            dataLines.Add(new DataLine(rawDataLine));
        }

        return dataLines;
    }

    private static DataLine FindDataLineWithHeader(string headerNeedle)
    {
        foreach (var dataLine in _dataLines)
        {
            if (dataLine.Header == headerNeedle)
                return dataLine;
        }

        throw new Exception($"Couldn't find dataLine matching headerNeedle: {headerNeedle}");
    }

    private static int RunAllStartPositionsUntilEndCondition(List<DataLine> startPositions, string commandLine)
    {
        var numberOfCommandSteps = 0;
        
        var currentPositions = startPositions.ToArray();
        
        do
        {
            for (var currentPositionIndex = 0; currentPositionIndex < currentPositions.Length; currentPositionIndex++)
            { 
                for (var commandIndex = 0; commandIndex < commandLine.Length; commandIndex++)
                {
                    numberOfCommandSteps++;
                    
                    Logger.Debug("At command steps: {CommandSteps} - CurrentPosition is: {CurrentPosition}", numberOfCommandSteps, currentPositions[currentPositionIndex]);

                    var currentCommand = commandLine[commandIndex];
                    
                    var nextHeaderValue = currentPositions[currentPositionIndex].FindNextHeaderValue(currentCommand);

                    currentPositions[currentPositionIndex] = FindDataLineWithHeader(nextHeaderValue);

                    Logger.Debug("At command steps: {CommandSteps} - After applying command: {CurrentCommand} position now: {@CurrentPosition}", numberOfCommandSteps, currentCommand, currentPositions[currentPositionIndex]);
                }
            }
        } 
        while (!EndConditionMet(currentPositions));

        return numberOfCommandSteps;
    }

    private static bool EndConditionMet(DataLine[] currentPositions)
    {
        if (currentPositions[0].Header == "ZZZ") 
            return true;

        return false;
    }
}
