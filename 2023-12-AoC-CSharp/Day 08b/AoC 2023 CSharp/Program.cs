using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Debug()
            .CreateLogger();

    private static List<DataLine> _dataLines = new();

    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.SampleData02
            .Split(Environment.NewLine);
        
        var commandLine = rawLines[0];

        _dataLines = GetParsedDataLines(rawLines);
        
        // var startPositions = 
        //     FindDataLinesEndingWith('A');

        //var answer = FindAllStartPositionsLoopPeriods(startPositions, commandLine);
        
        Logger.Information("11A Answer: {Answer}",FindLoopPeriodForStartPosition("11A", commandLine));
        
        //Logger.Information("JQA Answer: {Answer}",FindLoopPeriodForStartPosition("JQA", commandLine));
        //Logger.Information("NHA Answer: {Answer}",FindLoopPeriodForStartPosition("NHA", commandLine));
        // Logger.Information("AAA Answer: {Answer}",FindLoopPeriodForStartPosition("AAA", commandLine));
        // Logger.Information("FSA Answer: {Answer}",FindLoopPeriodForStartPosition("FSA", commandLine));
        // Logger.Information("LLA Answer: {Answer}",FindLoopPeriodForStartPosition("LLA", commandLine));
        // Logger.Information("MNA Answer: {Answer}",FindLoopPeriodForStartPosition("MNA", commandLine));
        
        //var loopPeriod = FindLoopPeriodForStartPosition("JQA", commandLine);

        //Logger.Information("Answer: {AnswerTotal}", loopPeriod);
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

    private static int FindLoopPeriodForStartPosition(string startPosition, string commandLine)
    {
        var numberOfCommandSteps = 0;
        
        var positionRecords = new List<string>();
        
        Logger.Information("Start! - CurrentPosition: {CurrentPosition}", startPosition);

        var currentHeader = startPosition;
        var lastHeader = startPosition;

        while (true)
        {
            for (var commandIndex = 0; commandIndex < commandLine.Length; commandIndex++)
            {
                numberOfCommandSteps++;
            
                var currentCommand = commandLine[commandIndex];
                
                Logger.Debug("At command steps: {CommandSteps} - currentHeader is: {CurrentPosition}",
                    numberOfCommandSteps, currentHeader);

                currentHeader = 
                    FindDataLineWithHeader(currentHeader).FindNextHeaderValue(currentCommand);
            
                // Save it so we can compare and find out when we've looped
                positionRecords.Add(currentHeader);

                if (positionRecords.Count % 1000 == 0)
                {
                    Logger.Information("At command steps: {CommandSteps} - After applying command: {CurrentCommand} currentHeader now: {CurrentPosition} - positionRecords count: {PositionRecordsCount}", 
                        numberOfCommandSteps, currentCommand, currentHeader, positionRecords.Count);    
                }

                var loopCheckResult = LoopSeenTwice(positionRecords);
                
                if (loopCheckResult < 0) continue;
            
                Logger.Information("Loop seen twice at: {NumberOfCommandSteps}", numberOfCommandSteps);
                Logger.Information("Loop period was: {Periodicity}", loopCheckResult);
                Logger.Information("Loop happened after {StepsInCount} steps that weren't part of the loop",
                    numberOfCommandSteps - (loopCheckResult * 2) + 1);
                
                return numberOfCommandSteps;
            }
        }
    }

    private static int LoopSeenTwice(List<string> positionRecords)
    {
        // Not enough records to see if we've looped until 4 
        if (positionRecords.Count < 2) return -1;
        
        var halfOfRecordsCount = positionRecords.Count / 2;
        
        // This for loop:
        // i Start: Half of positionRecords count
        // Until: i >= 0
        // Inc: i--
        for (var elementsToCheckCount = halfOfRecordsCount; elementsToCheckCount > 0; elementsToCheckCount--)
        {
            var firstHalfList = GetFirstHalfOfList(elementsToCheckCount, positionRecords);
            var secondHalfList = GetSecondHalfOfList(elementsToCheckCount, positionRecords);
            
            Logger.Debug("First half list: {@FirstHalfList}", firstHalfList);
            Logger.Debug("Second half list: {@SecondHalfList}", secondHalfList);

            var allEqual = true;
            
            for (var i = 0; i < firstHalfList.Count; i++)
            {
                Logger.Debug("Checking firstHalfList[i]: {} == secondHalfList[i] {}", firstHalfList[i], secondHalfList[i]);
                
                if (firstHalfList[i] == secondHalfList[i]) continue;

                allEqual = false;
            }

            if (allEqual)
                return firstHalfList.Count;
        }

        return -1;
    }

    private static List<string> GetFirstHalfOfList(int elementsToCheckCount, List<string> positionRecords)
    {
        var returnList = new List<string>();

        var total = positionRecords.Count;
        var secondHalfMinimumNumber = total - elementsToCheckCount;
        var firstHalfMinimumNumber = secondHalfMinimumNumber - elementsToCheckCount - 1;
        
        for (var i = secondHalfMinimumNumber - 1; i > firstHalfMinimumNumber; i--)
        {
            Logger.Verbose("Total: {Total}, first half i start: {Start}, first half current: {Index}", total, secondHalfMinimumNumber - 1, i);

            returnList.Insert(0, positionRecords[i]);
        }
        
        Logger.Verbose("Made first half of:");
        Logger.Verbose("{@AllRecords}", positionRecords);
        
        Logger.Verbose("Into list:");
        Logger.Verbose("{@SecondHalfList}", returnList);

        return returnList;
    }

    private static List<string> GetSecondHalfOfList(int elementsToCheckCount, List<string> positionRecords)
    {
        var returnList = new List<string>();

        var secondHalfStart = positionRecords.Count - 1;
        var secondHalfMinimumNumber = secondHalfStart - elementsToCheckCount;
        
        for (var i = secondHalfStart; i > secondHalfMinimumNumber; i--)
        {
            Logger.Verbose("Total: {Total}, second half min: {SecondHalfMin}, Second half current: {SecondHalfIndex}", secondHalfStart, secondHalfMinimumNumber, i);
            
            returnList.Insert(0, positionRecords[i]);
        }
        
        Logger.Verbose("Made second half of:");
        Logger.Verbose("{@AllRecords}", positionRecords);
        
        Logger.Verbose("Into list:");
        Logger.Verbose("{@SecondHalfList}", returnList);

        return returnList;
    }


    // private static void DebugPrintListHeaders(List<string> listToPrint, string listName)
    // {
    //     Logger.Information("");
    //     Logger.Information("Printing full list of {Name}: ", listName);
    //
    //     var lineCounter = 1;
    //     
    //     foreach (var line in listToPrint)
    //     {
    //         Logger.Information("#{LineNumber} - {Header}", lineCounter++.ToString("00"), line  );
    //     }
    // }

    // private static string[] FindDataLinesEndingWith(char headerEndCharNeedle)
    // {
    //     var startLines = new List<string>();
    //     
    //     foreach (var dataLine in _dataLines)
    //     {
    //         if (dataLine.Header.EndsWith(headerEndCharNeedle))
    //             startLines.Add(dataLine.Header);
    //     }
    //     
    //     return startLines.ToArray();
    // }

    // private static int FindAllStartPositionsLoopPeriods(string[] startPositions, string commandLine)
    // {
    //     var numberOfCommandSteps = 0;
    //     
    //     var currentPositions = startPositions.ToArray();
    //     
    //     Logger.Verbose("Start! - CurrentPosition(s): {@CurrentPositions}", currentPositions);
    //                     
    //     for (var currentPositionIndex = 0; currentPositionIndex < currentPositions.Length; currentPositionIndex++)
    //     {
    //         Logger.Information("Checking loop period for start position: {StartPosition}", currentPositions[currentPositionIndex]);
    //         
    //         var loopPeriod = FindLoopPeriodForStartPosition(
    //             currentPositions[currentPositionIndex], commandLine);
    //         
    //         Logger.Information("For start position: {StartPosition}, loopPeriod is: {LoopPeriod}", currentPositions[currentPositionIndex], loopPeriod);
    //     }
    //     
    //
    //     return numberOfCommandSteps;
    // }
}
