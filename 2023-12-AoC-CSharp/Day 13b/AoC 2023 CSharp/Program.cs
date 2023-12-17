using System.Diagnostics;
using AoC_2023_CSharp.Utilities;
using Serilog;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
    
    private static readonly Stopwatch ElapsedTotal = new();

    public static void Main()
    {
        ElapsedTotal.Start();
        Logger.Information("Starting!");

        var rawLines = RawData.ActualData01
            .Split(Environment.NewLine);
        
        var patterns = ParseAllPatterns(rawLines);
        
        var answerTotal = 0;
        
        foreach (var pattern in patterns)
        {
            answerTotal += GetPatternScore(pattern);
        }
        
        Logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    private static int GetPatternScore(string pattern)
    {
        LogPattern(pattern);

        var columnsScore = 0;
        var rowsScore = 0;

        var patternLines = pattern.Split(Environment.NewLine);

        for (var i = 0; i < patternLines[0].Length; i++)
        {
            columnsScore = ScoreColumnsStartingAt(i, pattern);
            if (columnsScore != 0) break;
        }

        for (var i = 0; i < patternLines.Length; i++)
        {
            rowsScore = ScoreRowsStartingAt(i, pattern);
            if (rowsScore != 0) break;
        }
        
        if (columnsScore > 0 && rowsScore > 0)
        {
            throw new Exception("Both rows and columns scored something which should not happen");
        }
        
        Logger.Debug("ColumnsScore: {ColumnsScore}, RowsScore: {RowsScore}", columnsScore, rowsScore);
        
        var returnValue = columnsScore > rowsScore ? columnsScore : rowsScore;
        
        Logger.Debug("Returning: {ReturnValue}", returnValue);
        
        return returnValue;
    }

    private static int ScoreColumnsStartingAt(int startColumn, string pattern)
    {
        var patternLines = pattern.Split(Environment.NewLine);

        var patternColumns = GetPatternColumns(patternLines);
        
        var symmetryFound = true;

        var originalLeftColumnPosition = 0;
        
        var leftColumnPosition = -1;
        var rightColumnPosition = int.MaxValue;
        
        for (var x = startColumn; x < patternColumns.Length - 1; x++)
        {
            leftColumnPosition = x;
            rightColumnPosition = x + 1;
            
            // Find two identical columns
            if (patternColumns[x] != patternColumns[x + 1]) continue;

            // Save the position of them
            originalLeftColumnPosition = rightColumnPosition;

            break;
        }
           
        while (--leftColumnPosition >= 0 &&
               ++rightColumnPosition < patternColumns.Length)
        {
            Logger.Debug("Checking column: {ColumnPosition}", leftColumnPosition);
            LogColumn(patternColumns[leftColumnPosition]);
                
            Logger.Debug("Against column {ColumnPosition}", rightColumnPosition);
            LogColumn(patternColumns[rightColumnPosition]);
                
            if (patternColumns[leftColumnPosition] == patternColumns[rightColumnPosition]) continue;
                
            symmetryFound = false;
        }

        // Return the number of columns to the left of the original symmetry columns
        if (symmetryFound)
            return originalLeftColumnPosition;

        return 0;
    }

    private static int ScoreRowsStartingAt(int startRow, string pattern)
    {
        var patternLines = pattern.Split(Environment.NewLine);

        var symmetryFound = true;

        var originalTopRowPosition = 0;
        
        var topRowPosition = -1;
        var bottomRowPosition = int.MaxValue;
        
        for (var x = startRow; x < patternLines.Length; x++)
        {
            topRowPosition = x;
            bottomRowPosition = x + 1;
            
            // Logger.Information("Would check topRowPosition: {TopPosition} against bottomRowPosition: {BottomPosition}", topRowPosition, bottomRowPosition);
            
            if (bottomRowPosition >= patternLines.Length) continue;
            
            // Find two identical rows
            if (patternLines[topRowPosition] != patternLines[bottomRowPosition]) continue;

            // Save the position of them
            originalTopRowPosition = bottomRowPosition;

            break;
        }
           
        while (--topRowPosition >= 0 &&
               ++bottomRowPosition < patternLines.Length)
        {
            Logger.Debug("Checking row: {RowPosition}", topRowPosition);
            Logger.Debug(patternLines[topRowPosition]);
                
            Logger.Debug("Against row {RowPosition}", bottomRowPosition);
            Logger.Debug(patternLines[bottomRowPosition]);
                
            if (patternLines[topRowPosition] == patternLines[bottomRowPosition]) continue;
                
            symmetryFound = false;
        }

        // Return the number of rows above the original symmetry columns * 100
        if (symmetryFound)
            return originalTopRowPosition * 100;

        return 0;
    }

    private static void LogPattern(string pattern)
    {
        var patternLines = pattern.Split(Environment.NewLine);

        Logger.Debug("");

        foreach (var line in patternLines)
        {
            Logger.Debug(line);
        }
    }

    private static void LogColumn(string patternColumn)
    {
        foreach (var character in patternColumn)
        {
            Logger.Debug("{Character}", character);
        }

        Logger.Debug("");
    }

    private static string[] GetPatternColumns(string[] patternLines)
    {
        var patternColumns = new List<string>();

        var width = patternLines[0].Length;
        var height = patternLines.Length;
        
        for (var x = 0; x < width; x++)
        {
            var thisColumn = "";
            
            for (var y = 0; y < height; y++)
            {
                thisColumn += patternLines[y][x];
            }
            
            patternColumns.Add(thisColumn);
        }

        return patternColumns.ToArray();
    }

    private static List<string> ParseAllPatterns(string[] rawLines)
    {
        var patterns = new List<string>();
        
        var currentPattern = "";

        for (var i = 0; i < rawLines.Length; i++)
        {
            var line = rawLines[i];
            
            // Logger.Verbose("Current line: {Line}", line);

            if (string.IsNullOrWhiteSpace(line))
            {
                patterns.Add(currentPattern);

                currentPattern = "";

                continue;
            }

            currentPattern += line;

            var nextLinePosition = i + 1;

            // Handle end of lines to parse or a blank line by not adding newline in those cases
            if (nextLinePosition >= rawLines.Length) continue;
            
            if (!string.IsNullOrWhiteSpace(rawLines[nextLinePosition]))
                currentPattern += Environment.NewLine;
        }

        // Add last pattern since there's no newline after
        patterns.Add(currentPattern);

        return patterns;
    }
}
