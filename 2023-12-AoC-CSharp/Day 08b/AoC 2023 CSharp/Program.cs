using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

// I thought of another way to do it that's way faster than how I did it below. 
//
// If instead of making a list of all the elements you've seen so far and checking them for two matching sets in a row to tell when the loop period is...which works, but took a few hours of multithreading... 
//
// What you could do is as you traverse the list, every time you hit a header that ends in 'Z' just check how many steps you've taken so far, double that, and run a check on the header that's the doubled number of steps in.
//
// If it's the same header that is at the non-doubled index, then you know that's your loop period. 
//
// ...which kinda is the algorithm Jurrd found, but attacked from a different side. The algo he found would still be faster, but this is at least getting close to the same big O notation as that algo.

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger();

    private static List<DataLine> _dataLines = new();

    private static ulong _stepsCounter = 0;

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

        CheckLoopsConvergence(rawLines, commandLine);

        // Make sure if we log right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    private static void CheckLoopsConvergence(string[] rawLines, string commandLine)
    {
        var loopNha = new Loop(0, 11309); 
        var loopJqa = new Loop(0, 13939);  
        var loopFsa = new Loop(0, 15517);
        var loopLla = new Loop(0, 17621); 
        var loopMna = new Loop(0, 18673);
        var loopAaa = new Loop(0, 20777);

        // Logger.Information("For start: {StartHeader} - Header at steps: {StepsToCheckAt} is {FoundHeader}", 
        //     startHeader, stepsToCheckAt, GetStringAtSteps(startHeader, stepsToCheckAt, rawLines, commandLine));
        
        // var startHeader = "NHA";
        // ulong stepsToCheckAt = 639630977;
        //
        // Logger.Information("For start: {StartHeader} - Header at steps: {StepsToCheckAt} is {FoundHeader}", 
        //     startHeader, stepsToCheckAt, GetStringAtSteps(startHeader, stepsToCheckAt - 2, rawLines, commandLine));
        //
        // Logger.Information("For start: {StartHeader} - Header at steps: {StepsToCheckAt} is {FoundHeader}", 
        //     startHeader, stepsToCheckAt, GetStringAtSteps(startHeader, stepsToCheckAt - 1, rawLines, commandLine));
        //
        // Logger.Information("For start: {StartHeader} - Header at steps: {StepsToCheckAt} is {FoundHeader}", 
        //     startHeader, stepsToCheckAt, GetStringAtSteps(startHeader, stepsToCheckAt, rawLines, commandLine));
        //
        // Logger.Information("For start: {StartHeader} - Header at steps: {StepsToCheckAt} is {FoundHeader}", 
        //     startHeader, stepsToCheckAt, GetStringAtSteps(startHeader, stepsToCheckAt + 1, rawLines, commandLine));
        //
        // Logger.Information("For start: {StartHeader} - Header at steps: {StepsToCheckAt} is {FoundHeader}", 
        //     startHeader, stepsToCheckAt, GetStringAtSteps(startHeader, stepsToCheckAt + 2, rawLines, commandLine));
        
        // var loops = new List<Loop>();
        //
        // loops.Add(loopNha);
        // loops.Add(loopJqa);
        // loops.Add(loopFsa);
        // loops.Add(loopLla);
        // loops.Add(loopMna);
        // loops.Add(loopAaa);
        
        
        // Original values
        // var loopNha = new Loop(4, 11309); 
        // var loopJqa = new Loop(4, 13939);  
        // var loopFsa = new Loop(3, 15517);
        // var loopLla = new Loop(5, 17621); 
        // var loopMna = new Loop(3, 18673);
        // var loopAaa = new Loop(3, 20777); 
        
        
        ulong counter = 0;

        
        while (counter++ < ulong.MaxValue)
        {
            _stepsCounter = counter;
            var loopAaaPosition = loopAaa.CurrentValueAtMultiple(counter);

            var mnaUnder = loopMna.GetClosestValueTo(loopAaaPosition);
            var llaUnder = loopLla.GetClosestValueTo(loopAaaPosition);
            var fsaUnder = loopFsa.GetClosestValueTo(loopAaaPosition);
            var jqaUnder = loopJqa.GetClosestValueTo(loopAaaPosition);
            var nhaUnder = loopNha.GetClosestValueTo(loopAaaPosition);
            
            // LogDifferences(mnaUnder, loopAaaPosition, llaUnder, fsaUnder, jqaUnder, nhaUnder);

            // var mnaUnder = loopMna.GetClosestValueTo(loopAaaPosition);
            if (mnaUnder != loopAaaPosition) continue;
            
            // var llaUnder = loopLla.GetClosestValueTo(loopAaaPosition);
            if (llaUnder != loopAaaPosition) continue;
            
            // var fsaUnder = loopFsa.GetClosestValueTo(loopAaaPosition);
            if (fsaUnder != loopAaaPosition) continue;
            
            // var jqaUnder = loopJqa.GetClosestValueTo(loopAaaPosition);
            if (jqaUnder != loopAaaPosition) continue;
            
            // var nhaUnder = loopNha.GetClosestValueTo(loopAaaPosition);
            if (nhaUnder != loopAaaPosition) continue;
            
            Logger.Information($"ALL loops aligned at: {loopAaaPosition}");
            Logger.Information("Steps: {Counter}", _stepsCounter);

            break;
        }
    }

    private static void LogDifferences(ulong mnaUnder, ulong loopAaaPosition, ulong llaUnder, ulong fsaUnder, ulong jqaUnder, ulong nhaUnder)
    {
        var mnaDifference = (long)loopAaaPosition - (long)mnaUnder;
        var llaDifference = (long)loopAaaPosition - (long)llaUnder;
        var fsaDifference = (long)loopAaaPosition - (long)fsaUnder;
        var jqaDifference = (long)loopAaaPosition - (long)jqaUnder;
        var nhaDifference = (long)loopAaaPosition - (long)nhaUnder;
        
        // Logger.Information("AAA: {AaaVal}, MNA: {MnaVal}, LLA: {LlaVal}, FSA: {FsaVal}, JQA: {JqaVal}, NHA: {NhaVal}",
        //     loopAaaPosition, mnaDifference, llaDifference, fsaDifference, jqaDifference, nhaDifference);

        var matchTolerance = 100;

        var matchingCount = 0;

        if (IsWithinX(mnaDifference, matchTolerance)) matchingCount++;
        if (IsWithinX(llaDifference, matchTolerance)) matchingCount++;
        if (IsWithinX(fsaDifference, matchTolerance)) matchingCount++;
        if (IsWithinX(jqaDifference, matchTolerance)) matchingCount++;
        if (IsWithinX(nhaDifference, matchTolerance)) matchingCount++;
        
        if (matchingCount >= 4)
        {
            
            Logger.Information("{MnaVal} \t {LlaVal} \t {FsaVal} \t {JqaVal} \t {NhaVal}",
                mnaDifference, llaDifference, fsaDifference, jqaDifference, nhaDifference);
            
            //Logger.Information("");
        }
        
        if (matchingCount >= 5)
        {
            Logger.Information("");

            Logger.Information("AAA: {MnaVal}", loopAaaPosition);
            
            Logger.Information("MNA: {MnaVal} \t LLA: {LlaVal} \t FSA: {FsaVal} \t JQA: {JqaVal} \t NHA: {NhaVal}",
                mnaDifference, llaDifference, fsaDifference, jqaDifference, nhaDifference);
            
            Logger.Information("");
        }
        
        // if (IsWithin5(mnaDifference))
        // {
        //     Logger.Information("MNA Difference: {Difference}", mnaDifference);
        // }
        
        // CHECKS:
        
        // -2
        // if (IsWithin5(llaDifference))
        // {
        //     Logger.Information("LLA Difference: {Difference}", llaDifference);
        // }
        
        // 0
        // if (IsWithin5(fsaDifference))
        // {
        //     Logger.Information("FSA Difference: {Difference}", fsaDifference);
        // }
        
        // -1
        // if (IsWithin5(jqaDifference))
        // {
        //     Logger.Information("JQA Difference: {Difference}", jqaDifference);
        // }
        
        // -1
        // if (IsWithin5(nhaDifference))
        // {
        //     Logger.Information("NHA Difference: {Difference}", nhaDifference);
        // }
    }

    private static void LogAllValues(ulong mnaUnder, ulong loopAaaPosition, ulong llaUnder, ulong fsaUnder, ulong jqaUnder,
        ulong nhaUnder)
    {
        // if (IsWithin5(mnaUnder, loopAaaPosition))
        // {
        //     Logger.Information("AAA: {AaaVal}, MNA: {MnaVal}, LLA: {LlaVal}, FSA: {FsaVal}, JQA: {JqaVal}, NHA: {NhaVal} (AAA and MNA)",
        //         loopAaaPosition, mnaUnder, llaUnder, fsaUnder, jqaUnder, nhaUnder);
        // }
        //     
        // if (IsWithin5(llaUnder, loopAaaPosition))
        // {
        //     Logger.Information("AAA: {AaaVal}, MNA: {MnaVal}, LLA: {LlaVal}, FSA: {FsaVal}, JQA: {JqaVal}, NHA: {NhaVal} (AAA and LLA)",
        //         loopAaaPosition, mnaUnder, llaUnder, fsaUnder, jqaUnder, nhaUnder);
        // }
        //     
        // if (IsWithin5(fsaUnder, loopAaaPosition))
        // {
        //     Logger.Information("AAA: {AaaVal}, MNA: {MnaVal}, LLA: {LlaVal}, FSA: {FsaVal}, JQA: {JqaVal}, NHA: {NhaVal} (AAA and FSA)",
        //         loopAaaPosition, mnaUnder, llaUnder, fsaUnder, jqaUnder, nhaUnder);
        // }
        //     
        // if (IsWithin5(jqaUnder, loopAaaPosition))
        // {
        //     Logger.Information("AAA: {AaaVal}, MNA: {MnaVal}, LLA: {LlaVal}, FSA: {FsaVal}, JQA: {JqaVal}, NHA: {NhaVal} (AAA and JQA)",
        //         loopAaaPosition, mnaUnder, llaUnder, fsaUnder, jqaUnder, nhaUnder);
        // }
        //     
        // if (IsWithin5(nhaUnder, loopAaaPosition))
        // {
        //     Logger.Information("AAA: {AaaVal}, MNA: {MnaVal}, LLA: {LlaVal}, FSA: {FsaVal}, JQA: {JqaVal}, NHA: {NhaVal} (AAA and NHA)",
        //         loopAaaPosition, mnaUnder, llaUnder, fsaUnder, jqaUnder, nhaUnder);
        // }
    }

    private static void GetAllLoopPeriods(string commandLine)
    {
        var startPositions = 
             FindDataLinesEndingWith('A');

        Logger.Information("{@Starts}", startPositions);
        
        // var answer = FindAllStartPositionsLoopPeriods(startPositions, commandLine);
        
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
    
    private static bool IsWithinX(long checkValue, long matchTolerance)
    {
        if (checkValue < matchTolerance &&
            checkValue > matchTolerance * -1)
        {
            return true;   
        }
            
        return false;
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

    private static string[] FindDataLinesEndingWith(char headerEndCharNeedle)
    {
        var startLines = new List<string>();
        
        foreach (var dataLine in _dataLines)
        {
            if (dataLine.Header.EndsWith(headerEndCharNeedle))
                startLines.Add(dataLine.Header);
        }
        
        return startLines.ToArray();
    }

    public static string GetStringAtSteps(string startHeader, ulong stepsToRun, string[] rawLines, string commandLine)
    {
        var numberOfCommandSteps = (ulong)0;

        var currentHeader = startHeader;

        while (numberOfCommandSteps < stepsToRun)
        {
            for (var commandIndex = 0; commandIndex < commandLine.Length; commandIndex++)
            {
                numberOfCommandSteps++;
            
                var currentCommand = commandLine[commandIndex];

                currentHeader = 
                    FindDataLineWithHeader(currentHeader).FindNextHeaderValue(currentCommand);
                
                if (numberOfCommandSteps >= stepsToRun) break;
            }
        }

        return currentHeader;
    }
    
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
