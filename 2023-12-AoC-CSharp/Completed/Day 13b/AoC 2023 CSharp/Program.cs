using System.Diagnostics;
using AoC_2023_CSharp.Utilities;
using Serilog;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger _logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
    
    private static readonly Stopwatch ElapsedTotal = new();

    public static void Main()
    {
        ElapsedTotal.Start();
        _logger.Information("Starting!");

        var rawLines = RawData.ActualData01
            .Split(Environment.NewLine);
        
        var patterns = ParseAllPatterns(rawLines);
        
        var answerTotal = 0;

        var patternCount = 0;
        
        foreach (var pattern in patterns)
        {
            _logger.Information("On pattern {Count}", patternCount++);
            
            LogPattern(pattern);
            
            var originalScore = GetPartOnePatternScore(pattern);
            
            var alternateScore = GetPartTwoPatternScore(pattern, originalScore);

            if (alternateScore == -1)
                throw new Exception("Alternate pattern not found");

            answerTotal += alternateScore;
        }
        
        _logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        _logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    
    private static int GetPartOnePatternScore(string pattern)
    {
        //LogPattern(pattern);

        var columnsScore = GetPartOnePatternColumnsScore(pattern);
        var rowsScore = GetPartOnePatternRowsScore(pattern);

        if (columnsScore > 0 && rowsScore > 0)
        {
            throw new Exception("Both rows and columns scored something which should not happen");
        }
        
        _logger.Debug("ColumnsScore: {ColumnsScore}, RowsScore: {RowsScore}", columnsScore, rowsScore);
        
        var returnValue = columnsScore > rowsScore ? columnsScore : rowsScore;
        
        _logger.Debug("Returning: {ReturnValue}", returnValue);
        
        return returnValue;
    }
    
    private static int GetPartOnePatternRowsScore(string pattern)
    {
        //LogPattern(pattern);
        
        var rowsScore = 0;

        var patternLines = pattern.Split(Environment.NewLine);
        
        for (var i = 0; i < patternLines.Length; i++)
        {
            rowsScore = ScoreRowsStartingAt(i, pattern);
            if (rowsScore != 0) break;
        }
        
        _logger.Debug("RowsScore: {RowsScore}", rowsScore);
        
        return rowsScore;
    }    
    
    private static int GetPartOnePatternColumnsScore(string pattern)
    {
        //LogPattern(pattern);

        var columnsScore = 0;

        var patternLines = pattern.Split(Environment.NewLine);

        for (var i = 0; i < patternLines[0].Length; i++)
        {
            columnsScore = ScoreColumnsStartingAt(i, pattern);
            if (columnsScore != 0) break;
        }
        
        _logger.Debug("ColumnsScore: {ColumnsScore}", columnsScore);
        
        return columnsScore;
    }

    private static int GetPartTwoPatternRowsScore(string pattern, int originalScore)
    {
        //LogPattern(pattern);
        
        var rowsScore = 0;

        var patternLines = pattern.Split(Environment.NewLine);
        
        for (var i = 0; i < patternLines.Length; i++)
        {
            rowsScore = ScoreRowsStartingAt(i, pattern);
            if (rowsScore != 0 && 
                rowsScore != originalScore) break;
        }
        
        _logger.Debug("RowsScore: {RowsScore}", rowsScore);
        
        return rowsScore;
    }    
    
    private static int GetPartTwoPatternColumnsScore(string pattern, int originalScore)
    {
        //LogPattern(pattern);

        var columnsScore = 0;

        var patternLines = pattern.Split(Environment.NewLine);

        for (var i = 0; i < patternLines[0].Length; i++)
        {
            columnsScore = ScoreColumnsStartingAt(i, pattern);
            if (columnsScore != 0 &&
                columnsScore != originalScore) break;
        }
        
        _logger.Debug("ColumnsScore: {ColumnsScore}", columnsScore);
        
        return columnsScore;
    }

    private static int GetPartTwoPatternScore(string pattern, int originalScore)
    {
        var patternLines = pattern.Split(Environment.NewLine);

        for (var y = 0; y < patternLines.Length; y++)
        {
            for (var x = 0; x < patternLines[y].Length; x++)
            {
                var checkPatternLines = pattern.Split(Environment.NewLine);

                checkPatternLines[y] = checkPatternLines[y][x] switch
                {
                    '#' => ChangeCharacterInStringTo(patternLines[y], x, '.'),
                    '.' => ChangeCharacterInStringTo(patternLines[y], x, '#'),
                    _ => checkPatternLines[y]
                };

                //_logger.Debug("Original pattern:");
                //LogPattern(pattern);

                var altPattern = string.Join(Environment.NewLine, checkPatternLines);
                
                _logger.Debug("Changing char at X:{X}, Y:{Y}", x, y);
                _logger.Debug("Alternate pattern:");
                LogPattern(altPattern);
                
                var altPatternRowsScore = GetPartTwoPatternRowsScore(altPattern, originalScore);
                
                if (altPatternRowsScore != 0)
                {
                    return altPatternRowsScore;
                }
                
                var altPatternColumnsScore = GetPartTwoPatternColumnsScore(altPattern, originalScore);
                
                if (altPatternColumnsScore != 0)
                {
                    return altPatternColumnsScore;
                }
            }
        }

        return -1;
    }

    private static string ChangeCharacterInStringTo(string original, int position, char newCharacter)
    {
        var returnString = "";

        for (var i = 0; i < original.Length; i++)
        {
            if (i != position)
            {
                returnString += original[i];
                continue;
            }

            returnString += newCharacter;
        }

        return returnString;
    }

    private static int ScoreColumnsStartingAt(int startColumn, string pattern)
    {
        var patternLines = pattern.Split(Environment.NewLine);

        var patternColumns = GetPatternColumns(patternLines);
        
        var symmetryFound = true;

        var originalLeftColumnPosition = 0;
        
        var leftColumnPosition = -1;
        var rightColumnPosition = int.MaxValue;
        
        for (var x = startColumn; x < patternColumns.Length; x++)
        {
            leftColumnPosition = x;
            rightColumnPosition = x + 1;
            
            if (rightColumnPosition >= patternColumns.Length) continue;
            
            // Find two identical columns
            if (patternColumns[x] != patternColumns[x + 1]) continue;

            // Save the position of them
            originalLeftColumnPosition = rightColumnPosition;

            break;
        }
           
        while (--leftColumnPosition >= 0 &&
               ++rightColumnPosition < patternColumns.Length)
        {
            _logger.Debug("Checking column: {ColumnPosition}", leftColumnPosition);
            //LogColumn(patternColumns[leftColumnPosition]);
                
            _logger.Debug("Against column {ColumnPosition}", rightColumnPosition);
            //LogColumn(patternColumns[rightColumnPosition]);
                
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
            _logger.Debug("Checking row: {RowPosition}", topRowPosition);
            _logger.Debug(patternLines[topRowPosition]);
                
            _logger.Debug("Against row {RowPosition}", bottomRowPosition);
            _logger.Debug(patternLines[bottomRowPosition]);
                
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

        _logger.Debug("");

        foreach (var line in patternLines)
        {
            _logger.Debug(line);
        }
    }

    private static void LogColumn(string patternColumn)
    {
        foreach (var character in patternColumn)
        {
            _logger.Debug("{Character}", character);
        }

        _logger.Debug("");
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
