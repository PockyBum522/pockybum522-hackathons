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
        
        // Run this to get the loop periods for all starting positions. They are:
        // GetAllLoopPeriods(commandLine);

        // They are:
        // [07:12:50 INF] For start: NHA
        // [07:12:50 INF] Loop seen twice at: 22621
        // [07:12:50 INF] Loop period was: 11309
        // [07:12:50 INF] Loop happened after 4 steps that weren't part of the loop
        // [07:12:50 INF] NHA Answer: 22621
        //
        // [08:26:51 INF] For start: JQA
        // [08:26:51 INF] Loop seen twice at: 27881
        // [08:26:51 INF] Loop period was: 13939
        // [08:26:51 INF] Loop happened after 4 steps that weren't part of the loop
        // [08:26:51 INF] JQA Answer: 27881
        //
        // [09:27:35 INF] For start: FSA
        // [09:27:35 INF] Loop seen twice at: 31036
        // [09:27:35 INF] Loop period was: 15517
        // [09:27:35 INF] Loop happened after 3 steps that weren't part of the loop
        // [09:27:35 INF] FSA Answer: 31036
        //
        // [11:09:32 INF] For start: LLA
        // [11:09:32 INF] Loop seen twice at: 35246
        // [11:09:32 INF] Loop period was: 17621
        // [11:09:32 INF] Loop happened after 5 steps that weren't part of the loop
        // [11:09:32 INF] LLA Answer: 35246
        //
        // [12:09:49 INF] For start: MNA
        // [12:09:49 INF] Loop seen twice at: 37348
        // [12:09:49 INF] Loop period was: 18673
        // [12:09:49 INF] Loop happened after 3 steps that weren't part of the loop
        // [12:09:49 INF] MNA Answer: 37348
        //
        // [14:27:34 INF] For start: AAA
        // [14:27:34 INF] Loop seen twice at: 41556
        // [14:27:34 INF] Loop period was: 20777
        // [14:27:34 INF] Loop happened after 3 steps that weren't part of the loop
        // [14:27:34 INF] AAA Answer: 41556

        CheckLoopsConvergence();

        // Make sure if we log right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    private static void CheckLoopsConvergence()
    {
        // NHA
        var loopStartAt01 = 4;
        var loopPeriod01 = (ulong)11309;
        
        // JQA
        var loopStartAt02 = 4;
        var loopPeriod02 = (ulong)13939;
        
        // FSA
        var loopStartAt03 = 3;
        var loopPeriod03 = (ulong)15517;
        
        // LLA
        var loopStartAt04 = 5;
        var loopPeriod04 = (ulong)17621;
        
        // MNA
        var loopStartAt05 = 3;
        var loopPeriod05 = (ulong)18673;
        
        // AAA
        var loopStartAt06 = 3;
        var loopPeriod06 = (ulong)20777;

        var loopPosition01 = (ulong)loopStartAt01 + loopPeriod01; 
        var loopPosition02 = (ulong)loopStartAt02 + loopPeriod02; 
        var loopPosition03 = (ulong)loopStartAt03 + loopPeriod03; 
        var loopPosition04 = (ulong)loopStartAt04 + loopPeriod04; 
        var loopPosition05 = (ulong)loopStartAt05 + loopPeriod05; 
        var loopPosition06 = (ulong)loopStartAt06 + loopPeriod06;

        ulong counter = 0;

        while (counter++ < ulong.MaxValue)
        {
            loopPosition01 += loopPeriod01;
            loopPosition02 += loopPeriod02;
            loopPosition03 += loopPeriod03;
            loopPosition04 += loopPeriod04;
            loopPosition05 += loopPeriod05;
            loopPosition06 += loopPeriod06;

            Logger.Debug("loopPosition01 at: {LoopValue}", loopPosition01);
            Logger.Debug("loopPosition02 at: {LoopValue}", loopPosition02);
            Logger.Debug("loopPosition03 at: {LoopValue}", loopPosition03);
            Logger.Debug("loopPosition04 at: {LoopValue}", loopPosition04);
            Logger.Debug("loopPosition05 at: {LoopValue}", loopPosition05);
            Logger.Debug("loopPosition06 at: {LoopValue}", loopPosition06);
            
            if (loopPosition01 == loopPosition02 &&
                loopPosition01 == loopPosition03 &&
                loopPosition01 == loopPosition04 &&
                loopPosition01 == loopPosition05 &&
                loopPosition01 == loopPosition06)
            {
                Console.WriteLine($"All loops aligned at: {loopPosition01}");
                Logger.Information("All loops aligned at: {LoopValue}", loopPosition01);
                
                return;
            }
        }
    }

    private static void GetAllLoopPeriods(string commandLine)
    {
        // var startPositions = 
        //     FindDataLinesEndingWith('A');

        //var answer = FindAllStartPositionsLoopPeriods(startPositions, commandLine);
        
        //Logger.Information("22A Answer: {Answer}",FindLoopPeriodForStartPosition("22A", commandLine));
        
        //Logger.Information("11A Answer: {Answer}",FindLoopPeriodForStartPosition("11A", commandLine));
        
        Logger.Information("JQA Answer: {Answer}",FindLoopPeriodForStartPosition("JQA", commandLine));
        Logger.Information("NHA Answer: {Answer}",FindLoopPeriodForStartPosition("NHA", commandLine));
        Logger.Information("AAA Answer: {Answer}",FindLoopPeriodForStartPosition("AAA", commandLine));
        Logger.Information("FSA Answer: {Answer}",FindLoopPeriodForStartPosition("FSA", commandLine));
        Logger.Information("LLA Answer: {Answer}",FindLoopPeriodForStartPosition("LLA", commandLine));
        Logger.Information("MNA Answer: {Answer}",FindLoopPeriodForStartPosition("MNA", commandLine));
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

        var positionRecords = new string[int.MaxValue - 100];

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

                // Save it so we can compare and find out when we've looped
                positionRecords[numberOfCommandSteps - 1] = currentHeader;
                
                currentHeader = 
                    FindDataLineWithHeader(currentHeader).FindNextHeaderValue(currentCommand);
            
                if (numberOfCommandSteps % 1000 == 0)
                {
                    Logger.Information("At command steps: {CommandSteps} - After applying command: {CurrentCommand} currentHeader now: {CurrentPosition}", 
                        numberOfCommandSteps, currentCommand, currentHeader);    
                }

                var loopCheckResult = LoopSeenTwice(positionRecords, numberOfCommandSteps);
                
                if (loopCheckResult < 0) continue;
            
                Logger.Information("Loop seen twice at: {NumberOfCommandSteps}", numberOfCommandSteps);
                Logger.Information("Loop period was: {Periodicity}", loopCheckResult);
                Logger.Information("Loop happened after {StepsInCount} steps that weren't part of the loop",
                    numberOfCommandSteps - (loopCheckResult * 2) + 1);
                
                return numberOfCommandSteps;
            }
        }
    }

    private static int LoopSeenTwice(string[] positionRecords, int positionRecordsSize)
    {
        // Not enough records to see if we've looped until 4 
        if (positionRecordsSize < 2) return -1;
        
        var halfOfRecordsCount = positionRecordsSize / 2;
        
        for (int i = 0; i < positionRecordsSize; i++)
        {
            Logger.Debug("All: {RecordsItem}", positionRecords[i]);
        }
        
        // This for loop:
        // i Start: Half of positionRecords count
        // Until: i >= 0
        // Inc: i--
        for (var elementsToCheckCount = halfOfRecordsCount; elementsToCheckCount > 0; elementsToCheckCount--)
        {
            var allEqual = true;

            // Debugging ranges:
            
            // var numberToCheck = 5;
            // var recordsCount = 12;
            //
            // for (int i = 0; i < numberToCheck; i++)
            // {
            //     var mappedFirstHalfIndex = MapIndexForFirstHalfOfRecords(i, numberToCheck, positionRecords, recordsCount);
            //     
            //     Logger.Debug("mappedFirstHalfIndex for numberToCheck: {NumberToCheck} and recordsCount: {RecordsCount} was: {MappedIndex}",
            //         numberToCheck, recordsCount, mappedFirstHalfIndex);
            // }
            //
            // for (int i = 0; i < numberToCheck; i++)
            // {
            //     var mappedSecondHalfIndex = MapIndexForSecondHalfOfRecords(i, numberToCheck, positionRecords, recordsCount);
            //     
            //     Logger.Debug("mappedSecondHalfIndex for numberToCheck: {NumberToCheck} and recordsCount: {RecordsCount} was: {MappedIndex}",
            //         numberToCheck, recordsCount, mappedSecondHalfIndex);
            // }
            
            for (var i = 0; i < elementsToCheckCount; i++)
            {
                var firstHalfItemIndex = MapIndexForFirstHalfOfRecords(i, elementsToCheckCount, positionRecords, positionRecordsSize);
                var secondHalfItemIndex = MapIndexForSecondHalfOfRecords(i, elementsToCheckCount, positionRecords, positionRecordsSize);

                var firstHalfItem = positionRecords[firstHalfItemIndex];
                var secondHalfItem = positionRecords[secondHalfItemIndex];
            
                Logger.Debug("Checking firstHalfItem: {FirstHalfItem} == secondHalfItem {SecondHalfItem}", firstHalfItem, secondHalfItem);
                
                if (firstHalfItem == secondHalfItem) continue;

                allEqual = false;
            }

            if (allEqual)
                return elementsToCheckCount;
        }

        return -1;
    }

    private static int MapIndexForFirstHalfOfRecords(int index, int elementsToCheckCount, string[] positionRecords, int positionRecordsSize)
    {
        var secondHalfMinimumNumber = positionRecordsSize - elementsToCheckCount;
        var firstHalfMinimumNumber = secondHalfMinimumNumber - elementsToCheckCount;
        
        var adjustedIndex = firstHalfMinimumNumber + index; 
        
        // Logger.Debug("In first half of records:");
        // Logger.Debug("Total size: {Total}", positionRecordsSize);
        // Logger.Debug("secondHalfMinimumNumber: {SecondHalfMinimumNumber}", secondHalfMinimumNumber);
        // Logger.Debug("firstHalfMinimumNumber: {FirstHalfMinimumNumber}", firstHalfMinimumNumber);
        // Logger.Debug("Index requested: {RequestedIndex}", index);
        // Logger.Debug("adjustedIndex: {AdjustedIndex}", adjustedIndex);
        // Logger.Debug("returning: {ReturnItem}", positionRecords[adjustedIndex]);
        
        return adjustedIndex;
    }

    private static int MapIndexForSecondHalfOfRecords(int index, int elementsToCheckCount, string[] positionRecords, int positionRecordsSize)
    {
        var secondHalfMinimumNumber = positionRecordsSize - elementsToCheckCount;
        var adjustedIndex = secondHalfMinimumNumber + index;

        // Logger.Debug("In second half of records:");
        // Logger.Debug("Total size: {Total}", positionRecordsSize);
        // Logger.Debug("secondHalfMinimumNumber: {SecondHalfMinimumNumber}", secondHalfMinimumNumber);
        // Logger.Debug("Index requested: {RequestedIndex}", index);
        // Logger.Debug("adjustedIndex: {AdjustedIndex}", adjustedIndex);
        // Logger.Debug("returning: {ReturnItem}", positionRecords[adjustedIndex]);
        
        return adjustedIndex;
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
